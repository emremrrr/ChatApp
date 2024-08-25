using System;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Models;

public abstract class Agent(AgentLevel agentLevel,double capacity)
{
    
    public Guid Id { get; set; }
    public string AgentName { get; set; }
    public double Capacity { get; } =capacity;
    public AgentLevel AgentLevel { get; } =agentLevel;
    public int MaxConcurrency =>Convert.ToInt32(10*Capacity);
}

public class JuniorAgent: Agent
{
    public static readonly double Capacity = 0.4;
    public static readonly AgentLevel AgentLevel = AgentLevel.Junior;

    public JuniorAgent():base(AgentLevel,Capacity)
    {
    }
}
public class MidAgent: Agent
{
    public static readonly double Capacity = 0.6;
    public static readonly AgentLevel AgentLevel = AgentLevel.Mid;
    public MidAgent():base(AgentLevel,Capacity)
    {
    }
        
}
public class SeniorAgent: Agent
{
    public static readonly double Capacity = 0.8;
    public static readonly AgentLevel AgentLevel = AgentLevel.Senior;
    public SeniorAgent():base(AgentLevel,Capacity)
    {
    }
}
public class TeamLeadAgent: Agent
{
    public static readonly double Capacity = 0.5;
    public static readonly AgentLevel AgentLevel = AgentLevel.TeamLead;
    public TeamLeadAgent():base(AgentLevel,Capacity)
    {
    }
}
