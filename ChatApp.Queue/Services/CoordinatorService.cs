


using ChatApp.Domain.Enums;
using ChatApp.Domain.Models;

public class CoordinatorService
{
    PriorityQueue<KeyValuePair<Guid,int>,int> _agentQueue = new PriorityQueue<KeyValuePair<Guid,int>, int>();
    PriorityQueue<KeyValuePair<Guid,int>,int> _overflowagentQueue = new PriorityQueue<KeyValuePair<Guid,int>, int>();
    Dictionary<Guid,Agent> _agentSet = new Dictionary<Guid,Agent>();
    int maxQueueSize=0;
    public void InitAgents()   
    {
        AgentTeam team = TimeOnly.FromDateTime(DateTime.Now) switch  
        {
            var t when  new TimeOnly(0,0)< t && t < new TimeOnly(7,59) => new AgentTeamA(),
            var t when  new TimeOnly(8,0)< t && t <= new TimeOnly(15,59) => new AgentTeamB(),
            var t when  new TimeOnly(16,0)< t && t <= new TimeOnly(23,59) => new AgentTeamC(),
        };
        maxQueueSize=team.MaxConcurrency;
        AgentLevel currentAgentLevel=AgentLevel.Junior;
        int nextPriority=0;
        int currentPriority=0;
        foreach (var a in team.Agents.OrderBy(a => Convert.ToInt32(a.AgentLevel))){
            _agentSet.TryAdd(a.Id,a);
            if(currentAgentLevel!=a.AgentLevel){
                currentAgentLevel=a.AgentLevel;
                nextPriority=currentPriority;
            }
            int priority=Convert.ToInt32(a.AgentLevel)+nextPriority;
            _agentQueue.Enqueue(new KeyValuePair<Guid, int>(a.Id,priority),priority);
            currentPriority = Convert.ToInt32(10 * a.Capacity);
        }
        var overflowAgents=new AgentTeamOverflow();
        foreach (var a in overflowAgents.Agents)
            _overflowagentQueue.Enqueue(new KeyValuePair<Guid,int>(a.Id,0),0);
    }

    public void DisposeAgentsQueue() => 
        _agentQueue.Clear();

    public Guid AddAgentToQueue(){ 
        if(_agentQueue.Count>=maxQueueSize)
            throw new OverflowException();

        var agent = _agentQueue.Dequeue();
        List<KeyValuePair<Guid,int>> tempAgents=new List<KeyValuePair<Guid, int>>();

        while(agent.Value>=_agentSet[agent.Key].MaxConcurrency && _agentQueue.Count>0){
            tempAgents.Add(new KeyValuePair<Guid, int>(agent.Key, agent.Value));
            agent=_agentQueue.Dequeue();
        }
        TimeOnly t=TimeOnly.FromDateTime(DateTime.Now);
        if(_agentQueue.Count==0 &&  new TimeOnly(8,0)< t && t <= new TimeOnly(15,59) ){
            var tagent=_overflowagentQueue.Dequeue();
            int tpriority=agent.Value+1;
            _overflowagentQueue.Enqueue(new KeyValuePair<Guid, int>(tagent.Key,tpriority),tpriority);
            return tagent.Key;
        }
        int priority=agent.Value+1;
        _agentQueue.Enqueue(new KeyValuePair<Guid, int>(agent.Key,priority),priority);
        while(tempAgents.Count>0){
            var agentPair=tempAgents.First();
            _agentQueue.Enqueue(new KeyValuePair<Guid, int>(agentPair.Key,agentPair.Value),agentPair.Value);
        }

        return agent.Key;
    }
    public Agent GetCurrentAvailableAgent()=>
        _agentSet[_agentQueue.Peek().Key];
}