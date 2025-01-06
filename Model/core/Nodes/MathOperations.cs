using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    internal class NodePlus : Node
    {
        public NodePlus()
        {
            defaultInputs = [1.0D, 2.0D];
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[2]);
            Outputs = [new OutputOfNode(GetOutput)]; // самозацикливание
            NameOfNode = "Сложение";
            NameOfInputs = ["Слагаемое", "Слагаемое"];
            NameOfOutputs = ["Сумма"];

            SetEventConnection();
        }
        private object GetOutput()
        {
            double firstValue = Inputs[0] ==  null ? (double)defaultInputs[0] : (double)Inputs[0].GetValue();
            double secondValue = Inputs[1] == null ? (double)defaultInputs[1] : (double)Inputs[1].GetValue();
            return firstValue + secondValue;
        } 
    }
    internal class NodeSub : Node
    {
        public NodeSub()
        {
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[2]);
            defaultInputs = [1.0D, 2.0D];
            Outputs = [new OutputOfNode(GetOutput)];
            NameOfNode = "Вычитание";
            NameOfInputs = ["Уменьшаемое", "Вычитаемое"];
            NameOfOutputs = ["Разность"];

            SetEventConnection();
        }
        private object GetOutput()
        {
            double firstValue = Inputs[0] == null ? (double)defaultInputs[0] : (double)Inputs[0].GetValue();
            double secondValue = Inputs[1] == null ? (double)defaultInputs[1] : (double)Inputs[1].GetValue();
            return firstValue - secondValue;
        }
    }
    internal class NodeMult : Node
    {
        public NodeMult()
        {
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[2]);
            defaultInputs = [1.0D, 2.0D];
            Outputs = [new OutputOfNode(GetOutput)];
            NameOfNode = "Умножение";
            NameOfInputs = ["Множитель", "Множитель"];
            NameOfOutputs = ["Произведение"];

            SetEventConnection();
        }
        private object GetOutput()
        {
            double firstValue = Inputs[0] == null ? (double)defaultInputs[0] : (double)Inputs[0].GetValue();
            double secondValue = Inputs[1] == null ? (double)defaultInputs[1] : (double)Inputs[1].GetValue();
            return firstValue * secondValue;
        }
    }
    internal class NodeDiv : Node
    {
        public NodeDiv()
        {
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[2]);
            defaultInputs = [1.0D, 2.0D];
            Outputs = [new OutputOfNode(GetOutput)];
            NameOfNode = "Деление";
            NameOfInputs = ["Делимое", "Делитель"];
            NameOfOutputs = ["Частное"];

            SetEventConnection();
        }
        private object GetOutput()
        {
            double firstValue = Inputs[0] == null ? (double)defaultInputs[0] : (double)Inputs[0].GetValue();
            double secondValue = Inputs[1] == null ? (double)defaultInputs[1] : (double)Inputs[1].GetValue();
            return firstValue / secondValue;
        }
    }


}
