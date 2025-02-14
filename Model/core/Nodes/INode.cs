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
        public object[] Parameters { get; set; }
        public string[] NameOfInputs { get; set; }
        public string[] NameOfOutputs { get; set; }
        public string NameOfNode { get; set; }
    }

    public class Node : INode, INotifyPropertyChanged
    {
        public Node[] nextNodes;
        private Node[] prefNodes;

        public object[] Parameters { get; set; }

        public ObservableCollection<object> defaultInputs;
        public ObservableCollection<InputOfNode> Inputs { get; set; }
        public OutputOfNode[] Outputs { get; set; }
        public string[] NameOfInputs { get; set; }
        public string[] NameOfOutputs { get; set; }
        public string NameOfNode { get; set; }

        public void FinalNodeConstructor()
        {
            defaultInputs.CollectionChanged += Inputs_CollectionChanged;
            Inputs.CollectionChanged += Inputs_CollectionChanged;
            nextNodes = new Node[Outputs.Length];
            prefNodes = new Node[Inputs.Count];
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
            
            prefNode.nextNodes[outputIndex] = this;
            prefNodes[inputIndex] = prefNode;

            Inputs[inputIndex] = new InputOfNode(prefNode.Outputs[outputIndex]);
        }
        public void ResetInputToDefault(int inputIndex, int outputIndex)
        {
            Inputs[inputIndex] = null;
            prefNodes[inputIndex].nextNodes[outputIndex] = null;
        }

        public void Inputs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Outputs");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

            if (PropertyChanged == null) return;
            foreach (var node in nextNodes)
            {
                node?.OnPropertyChanged("Outputs");
            }
        }

        public Node Copy() {
            var type = GetType();

            var newNode = (Node)Activator.CreateInstance(type);

            for (int i = 0; i < defaultInputs.Count; i++)
            {
                newNode.defaultInputs[i] = defaultInputs[i];
            }

            return newNode;
        }
    }
}
