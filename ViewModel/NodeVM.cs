using L_system.Model.core.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace L_system.ViewModel
{
    public class NodeVM
    {
        private Node nodeCore;
        private ConnectionVM[] connectionVMToInputs;

        public NodeVM(Node nodeCore)
        {
            this.nodeCore = nodeCore;
            connectionVMToInputs = new ConnectionVM[nodeCore.Inputs.Count];
            nodeCore.PropertyChanged += NodeCore_PropertyChanged;
        }

        public bool CanCreateConnection(int inputIndex, NodeVM prefNode, int outputIndex)
        {
            return nodeCore.CanConnect(inputIndex, prefNode.nodeCore, outputIndex);
        }

        public void CreateConnection(int inputIndex, NodeVM prefNode, int outputIndex, ConnectionVM connection)
        {
            nodeCore.Connect(inputIndex, prefNode.nodeCore, outputIndex);
            connectionVMToInputs[inputIndex] = connection;
        }

        public void Disconnect(int inputIndex, int outputIndex)
        {
            nodeCore.ResetInputToDefault(inputIndex, outputIndex);

        }

        public int GetInputsCount()
        {
            return nodeCore.NameOfInputs == null? 0 : nodeCore.NameOfInputs.Length;
        }

        public int GetOutputsCount()
        {
            return nodeCore.NameOfOutputs == null ? 0 : nodeCore.NameOfOutputs.Length;
        }
        public string[] GetNameOfInputs()
        {
            return nodeCore.NameOfInputs;
        }
        public string[] GetNameOfOutputs()
        {
            return nodeCore.NameOfOutputs;
        }
        public string GetNameOfNode()
        {
            return nodeCore.NameOfNode;
        }

        public object GetValueFromOutput(int outputIndex)
        {
            return nodeCore.Outputs[outputIndex].GetValue();
        }

        public object GetValueFromDefInput(int inputIndex)
        {
            return nodeCore.defaultInputs[inputIndex];
        }

        public void SetValueToDefInput(object value, int inputIndex)
        {
            nodeCore.defaultInputs[inputIndex] = value;
        }

        public bool IsInputFree(int inputIndex)
        {
            return nodeCore.Inputs[inputIndex] == null;
        }

        public string GetTypeOfOutput(int outputIndex)
        {
            if (nodeCore.Outputs.Length == 0) return "End";
            return nodeCore.Outputs[outputIndex].GetValue().GetType().Name;
        }
        public string GetTypeOfInput(int inputIndex)
        {
            if (nodeCore.Inputs.Count == 0) return "Empty";
            return nodeCore.defaultInputs[inputIndex].GetType().Name;
        }

        public void OnOutputChanged(Action action)
        {
            actions.Add(action);
        }
        private List<Action> actions = new List<Action>();
        private void NodeCore_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            foreach (var action in actions)
            {
                action();
            }
        }
    }
}
