using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    public interface INode
    {
        public ObservableCollection<InputOfNode> Inputs { get; set; }
        public OutputOfNode[] Outputs { get; set; }
        public string[] NameOfInputs { get; set; }
        public string[] NameOfOutputs { get; set; }
        public string NameOfNode {  get; set; }
    }

    public class Node : INode, INotifyPropertyChanged
    {
        public List<Node> nextNodes = new List<Node>();

        public ObservableCollection<object> defaultInputs;
        public ObservableCollection<InputOfNode> Inputs { get; set; }
        public OutputOfNode[] Outputs { get; set; }
        public string[] NameOfInputs { get; set; }
        public string[] NameOfOutputs { get; set; }
        public string NameOfNode { get; set; }

        public void SetEventConnection()
        {
            defaultInputs.CollectionChanged += Inputs_CollectionChanged;
            Inputs.CollectionChanged += Inputs_CollectionChanged;
        }
        public void SetEventConnection(Action actionOnInputChanged)
        {
            defaultInputs.CollectionChanged += Inputs_CollectionChanged;
            Inputs.CollectionChanged += Inputs_CollectionChanged;
            defaultInputs.CollectionChanged += (object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => actionOnInputChanged();
            Inputs.CollectionChanged += (object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => actionOnInputChanged();
        }

        public bool CanConnect(int inputIndex, Node prefNode, int outputIndex)
        {
            if (inputIndex < 0 || inputIndex >= Inputs.Count ||
                    outputIndex < 0 || outputIndex >= prefNode.Outputs.Length)
                return false;

            var inputType = defaultInputs[inputIndex].GetType();
            var outputType = prefNode.Outputs[outputIndex].GetValue().GetType();

            return inputType.IsAssignableFrom(outputType);
        }
        public void Connect(int inputIndex, Node prefNode, int outputIndex)
        {
            if (!CanConnect(inputIndex, prefNode, outputIndex)) throw new ArgumentException("Разные типы входов и выходов.");
            
            prefNode.nextNodes.Add(this);

            Inputs[inputIndex] = new InputOfNode(prefNode.Outputs[outputIndex]);
        }

        public void Inputs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Outputs");
        }

        internal void ResetInputToDefault(int inputIndex)
        {
            Inputs[inputIndex] = null;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            foreach (var node in nextNodes)
            {
                node.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
