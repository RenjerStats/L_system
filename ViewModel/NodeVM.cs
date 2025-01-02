using L_system.Model.core.Nodes;
using System;
using System.Collections.Generic;
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
            connectionVMToInputs = new ConnectionVM[nodeCore.Inputs.Length];
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

        public void Disconnect(int inputIndex)
        {
            nodeCore.ResetInputToDefault(inputIndex);
        }

        public ConnectionVM FindConnectionToInput(int inputIndex)
        {
            return connectionVMToInputs[inputIndex];
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

        //private string GetNameOfType(object value)
        //{
        //    switch (value.GetType().Name)
        //    {
        //        case "Double":
        //            return "число";
        //        case "String":
        //            return "строка";
        //        case "Command[]":
        //            return "команды";
        //        case "Command":
        //            return "команда";
        //        default:
        //            return "неизвестный тип";
        //    }
        //}
    }
}
