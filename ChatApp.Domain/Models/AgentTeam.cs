using System;

namespace ChatApp.Domain.Models;

public class AgentTeam(IList<Agent> agents)
{
    public IList<Agent> Agents { get; }=agents;

    public int MaxConcurrency=>Agents.Sum(p=>p.MaxConcurrency);
}

public class AgentTeamA: AgentTeam{
    static readonly IList<Agent> agents=new List<Agent>{
        new TeamLeadAgent(){Id=Guid.NewGuid(),AgentName="lead"},
        new MidAgent(){Id=Guid.NewGuid(),AgentName="mid1 agt"},
        new MidAgent(){Id=Guid.NewGuid(),AgentName="mid2 agt"},
        new JuniorAgent(){Id=Guid.NewGuid(),AgentName="jr agt"}
    };
    public AgentTeamA():base(agents) { 
    }
}
public class AgentTeamB: AgentTeam{
    static readonly IList<Agent> agents=new List<Agent>{
        new SeniorAgent(){Id=Guid.NewGuid(),AgentName="senior agt"},
        new MidAgent(){Id=Guid.NewGuid(),AgentName="mid agt"},
        new JuniorAgent(){Id=Guid.NewGuid(),AgentName="jr1 agt"},
        new JuniorAgent(){Id=Guid.NewGuid(),AgentName="jr2 agt"}
    };
    public AgentTeamB():base(agents) { 
    }
}
public class AgentTeamC: AgentTeam{
    static readonly IList<Agent> agents=new List<Agent>{
        new MidAgent(){Id=Guid.NewGuid(),AgentName="mid1 agt"},
        new MidAgent(){Id=Guid.NewGuid(),AgentName="mid2 agt"}
    };
    public AgentTeamC():base(agents) { 
    }
}

public class AgentTeamOverflow: AgentTeam{
    static readonly IList<Agent> agents=new List<Agent>{
        new JuniorAgent(){Id=Guid.NewGuid(),AgentName="jr1 agt"},
        new JuniorAgent(){Id=Guid.NewGuid(),AgentName="jr2 agt"},
        new JuniorAgent(){Id=Guid.NewGuid(),AgentName="jr3 agt"},
        new JuniorAgent(){Id=Guid.NewGuid(),AgentName="jr4 agt"},
        new JuniorAgent(){Id=Guid.NewGuid(),AgentName="jr5 agt"},
        new JuniorAgent(){Id=Guid.NewGuid(),AgentName="jr6 agt"}
    };
    public AgentTeamOverflow():base(agents) { 
    }
}
