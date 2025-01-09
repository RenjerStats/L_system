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

            FinalNodeConstructor();
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

            FinalNodeConstructor();
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

            FinalNodeConstructor();
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

            FinalNodeConstructor();
        }
        private object GetOutput()
        {
            double firstValue = Inputs[0] == null ? (double)defaultInputs[0] : (double)Inputs[0].GetValue();
            double secondValue = Inputs[1] == null ? (double)defaultInputs[1] : (double)Inputs[1].GetValue();
            return firstValue / secondValue;
        }
    }
    internal class NodeSin : Node
    {
        public NodeSin()
        {
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[1]);
            defaultInputs = [Math.PI/4];
            Outputs = [new OutputOfNode(GetOutput)];
            NameOfNode = "Sin";
            NameOfInputs = ["Число"];
            NameOfOutputs = ["[-1;1]"];

            FinalNodeConstructor();
        }
        private object GetOutput()
        {
            double firstValue = Inputs[0] == null ? (double)defaultInputs[0] : (double)Inputs[0].GetValue();
            return Math.Sin(firstValue);
        }
    }



}
