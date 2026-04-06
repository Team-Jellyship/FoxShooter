using System;
using System.ComponentModel;
using System.Linq;
using Unity.GraphToolkit.Editor;
using FoxShooter.Game.GamemodeGraph.Runtime;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph
{

[Serializable]
class StartNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort("Start").Build();
    }
    
    public GamemodeNode GetNextNode()
    {
        var outputPort = GetOutputPorts().First()?.firstConnectedPort;
        return outputPort.GetNode() as GamemodeNode;
    }
}
    
[Serializable]
public class GamemodeNode : ContextNode
{
    private const string InputCountName = "Inputs";
    private const string Name = "Name";
    private const string TimeName = "Time";
    private const string ScriptName = "Script";

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<string>(Name).Build();
        context.AddOption<float>(TimeName)
            .WithDisplayName("Duration")
            .WithDefaultValue(0.0f).Build();
        context.AddOption<Gamemode>(ScriptName).Build();
    }

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("Input").Build();
    }

    public string GetModeName()
    {
        return GetNodeOptionByName(Name).TryGetValue<string>(out var name) ? name : "null";
    }

    public float GetModeTime()
    {
        return GetNodeOptionByName(TimeName).TryGetValue<float>(out var time) ? time : 0.0f;
    }
}

[Serializable]
[UseWithContext(typeof(GamemodeNode))]
[DisplayName("Transition")]
class GamemodeTransitionNode : BlockNode
{
    private const string TransitionFlagName = "TransitionFlag";

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<GamemodeTransitionFlag>(TransitionFlagName)
            .WithDisplayName("Transition").Build();
    }

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort("Output").Build();
    }
    
    
    public GamemodeTransitionFlag GetTransitionFlag()
    {
        GetNodeOption(0).TryGetValue<GamemodeTransitionFlag>(out var flag);
        return flag;
    }

    public GamemodeNode GetNextNode()
    {
        var outputPort = GetOutputPorts().FirstOrDefault()?.firstConnectedPort;
        return outputPort.GetNode() as GamemodeNode;
    }
}
}