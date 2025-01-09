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

        public string GetTypeOfOutput(int outputIndex)
        {
            return nodeCore.Outputs[outputIndex].GetValue().GetType().Name;
        }

        public void OnOutputChanged(Action action)
        {
            this.action = action;
            nodeCore.PropertyChanged += NodeCore_PropertyChanged;
        }
        private Action action;
        private void NodeCore_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            action();
        }
    }
}
