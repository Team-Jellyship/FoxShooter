using System;
using System.Collections.Generic;
using System.Linq;
using FoxShooter.Game.GamemodeGraph;
using Unity.GraphToolkit.Editor;

namespace Characters.AI.StateTree.Editor
{
    [Serializable]
    [UseWithGraph(typeof(StateTreeGraph))]
    public class StateNode : ContextNode
    {
        public int id;
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<string>("Name").WithDefaultValue("State").Build();
            context.AddInputPort("Parent").Build();
            context.AddInputPort("Transitions").Build();

            context.AddOutputPort("Child").Build();
            context.AddOutputPort("Canceled").Build();
            context.AddOutputPort("Succeeded").Build();
        }

        public string GetName()
        {
            GetInputPort(0).TryGetValue<string>(out var result);
            return result;
        }

        public List<StateNode> GetChildren()
        {
            var port = GetOutputPort(0);
            var result = new List<StateNode>();
            var outPorts = new List<IPort>();
            port.GetConnectedPorts(outPorts);
            foreach (var subPort in outPorts)
            {
                if (subPort.GetNode() is StateNode child)
                {
                    result.Add(child);
                }
            }
            return result;
        }

        public StateNode GetParent()
        {
            return GetInputPort(1).firstConnectedPort.GetNode() as StateNode;
        }

        public StateNode GetSucceed()
        {
            return GetOutputPort(2).firstConnectedPort.GetNode() as StateNode;
        }

        public StateNode GetCancelled()
        {
            return GetOutputPort(1).firstConnectedPort.GetNode() as StateNode;
        }
    }

    [Serializable]
    [UseWithGraph(typeof(StateTreeGraph))]
    public class RootNode : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort("Root").Build();
        }

        public StateNode GetNode()
        {
            var outputPort = GetOutputPorts().First()?.firstConnectedPort;
            return outputPort.GetNode() as StateNode;
        }
    }
}