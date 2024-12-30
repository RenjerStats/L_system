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

        public bool miniSize { get;private set; }

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

        public string[] GetNameOfInputs()
        {
            string[] result = new string[nodeCore.Inputs.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetNameOfType(nodeCore.Inputs[i].GetValue());
            }

            return result;
        }

        public string[] GetNameOfOutputs()
        {
            string[] result = new string[nodeCore.Outputs.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetNameOfType(nodeCore.Outputs[i].GetValue());
            }

            return result;
        }

        private string GetNameOfType(object value)
        {
            switch (value.GetType().Name)
            {
                case "Double":
                    return "число";
                case "String":
                    return "строка";
                case "Command[]":
                    return "команды";
                case "Command":
                    return "команда";
                default:
                    return "неизвестный тип";
            }
        }
    }
}
