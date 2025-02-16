using L_system.Model.core.Nodes;
using System.ComponentModel;

namespace L_system.ViewModel
{
    public class NodeVM
    {
        private Node nodeCore;
        public bool Flipped { get; private set; }
        public void Flip() => Flipped = !Flipped;

        public NodeVM(Node nodeCore)
        {
            this.nodeCore = nodeCore;
            nodeCore.PropertyChanged += NodeCore_PropertyChanged;
            nodeCore.Inputs.CollectionChanged += Inputs_CollectionChanged;
        }

        public Node GetCopyCore()
        {
            return nodeCore.Copy();
        }

        #region Connection

        public bool CanCreateConnection(int inputIndex, NodeVM prefNode, int outputIndex)
        {
            return nodeCore.CanConnect(inputIndex, prefNode.nodeCore, outputIndex);
        }
        public void CreateConnection(int inputIndex, NodeVM prefNode, int outputIndex)
        {
            nodeCore.Connect(inputIndex, prefNode.nodeCore, outputIndex);
        }
        public void Disconnect(int inputIndex, int outputIndex)
        {
            nodeCore.ResetInputToDefault(inputIndex, outputIndex);
        }
        #endregion

        #region Names
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
        #endregion

        #region Inputs
        public int GetInputsCount()
        {
            return nodeCore.Inputs == null ? 0 : nodeCore.Inputs.Count;
        }
        public object GetValueFromDefInput(int inputIndex)
        {
            return nodeCore.defaultInputs[inputIndex];
        }
        public void SetValueToDefInput(object value, int inputIndex)
        {
            if (value is double)
            {
                value = Math.Clamp((double)value, 0.0001D, 1000D);
            }
            nodeCore.defaultInputs[inputIndex] = value;
        }
        public bool IsInputFree(int inputIndex)
        {
            return nodeCore.Inputs[inputIndex] == null;
        }
        public string GetTypeOfInput(int inputIndex)
        {
            if (nodeCore.Inputs.Count == 0) return "Empty";
            return nodeCore.defaultInputs[inputIndex].GetType().Name;
        }
        #endregion

        #region Outputs
        public object GetValueFromOutput(int outputIndex)
        {
            return nodeCore.Outputs[outputIndex].GetValue();
        }
        public int GetOutputsCount()
        {
            return nodeCore.Outputs == null ? 0 : nodeCore.Outputs.Length;
        }
        public string GetTypeOfOutput(int outputIndex)
        {
            if (nodeCore.Outputs.Length == 0) return "End";
            return nodeCore.Outputs[outputIndex].GetValue().GetType().Name;
        }
        #endregion

        #region Parameters
        public int GetCountParameters()
        {
            if (nodeCore.Parameters == null) return 0;
            return nodeCore.Parameters.Length;
        }
        public object GetParameter(int index)
        {
            return nodeCore.Parameters[index];
        }
        public string GetTypeOfParameter(int index)
        {
            return nodeCore.Parameters[index].GetType().Name;
        }
        public void SetParameter(int index, object value)
        {
            nodeCore.Parameters[index] = value;
        }
        #endregion

        #region Actions

        public void OnOutputChanged(Action action)
        {
            actionsOnOutputChanged.Add(action);
        }
        private List<Action> actionsOnOutputChanged = new List<Action>();
        private void NodeCore_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            foreach (var action in actionsOnOutputChanged.ToList())
            {
                action();
            }
        }

        public void OnInputsChanged(Action action)
        {
            actionsOnInputsChanged.Add(action);
        }
        private List<Action> actionsOnInputsChanged = new List<Action>();
        private void Inputs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var action in actionsOnInputsChanged.ToList())
            {
                action();
            }
        }
        #endregion
    }
}
