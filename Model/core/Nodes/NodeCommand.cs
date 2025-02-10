using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace L_system.Model.core.Nodes
{
    public class NodeCommand : Node
    {
        public NodeCommand()
        {
            defaultInputs = [new List<Command>(), 50D];
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[2]);
            NameOfInputs = ["Команды", "Число"];
            NameOfOutputs = ["Команды"];
            Outputs = [new OutputOfNode(GetResult)];
        }

        protected CommandType typeNode;
        protected object GetResult()
        {
            List<Command> list = Inputs[0] == null ? (List<Command>)defaultInputs[0] : (List<Command>)Inputs[0].GetValue();
            Double value = Inputs.Count == 1 ? 0 : (Inputs[1] == null ? (Double)defaultInputs[1] : (Double)Inputs[1].GetValue());
            list.Add(new Command(typeNode, value));
            return list;
        }
    }

    public class NodeMove : NodeCommand
    {
        public NodeMove() : base()
        {
            NameOfInputs[1] = "Длина";
            NameOfNode = "Линия";
            typeNode = CommandType.move;

            FinalNodeConstructor();
        }
    }
    public class NodeRotate : NodeCommand
    {
        public NodeRotate() : base()
        {
            NameOfInputs[1] = "Угол";
            NameOfNode = "Поворот";
            typeNode = CommandType.rotate;

            FinalNodeConstructor();
        }
    }
    public class NodeJump : NodeCommand
    {
        public NodeJump() : base()
        {
            NameOfInputs[1] = "Длина";
            NameOfNode = "Перемещение";
            typeNode = CommandType.jump;

            FinalNodeConstructor();
        }
    }
    public class NodeSave : NodeCommand
    {
        public NodeSave() : base()
        {
            NameOfNode = "Сохранение";
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[1]);
            defaultInputs = [new List<Command>()];
            NameOfInputs = ["Команды"];
            typeNode = CommandType.save;

            FinalNodeConstructor();
        }
    }
    public class NodeLoad : NodeCommand
    {
        public NodeLoad() : base()
        {
            NameOfNode = "Загрузка";
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[1]);
            defaultInputs = [new List<Command>()];
            NameOfInputs = ["Команды"];
            typeNode = CommandType.load;

            FinalNodeConstructor();
        }
    }
    public class NodeNothing1 : NodeCommand
    {
        public NodeNothing1() : base()
        {
            NameOfNode = "Пустая команда1";
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[1]);
            defaultInputs = [new List<Command>()];
            NameOfInputs = ["Команды"];
            typeNode = CommandType.nothingDoing1;

            FinalNodeConstructor();
        }
    }
    public class NodeNothing2 : NodeCommand
    {
        public NodeNothing2() : base()
        {
            NameOfNode = "Пустая команда2";
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[1]);
            defaultInputs = [new List<Command>()];
            NameOfInputs = ["Команды"];
            typeNode = CommandType.nothingDoing2;

            FinalNodeConstructor();
        }
    }
    public class NodeNothing3 : NodeCommand
    {
        public NodeNothing3() : base()
        {
            NameOfNode = "Пустая команда3";
            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[1]);
            defaultInputs = [new List<Command>()];
            NameOfInputs = ["Команды"];
            typeNode = CommandType.nothingDoing3;

            FinalNodeConstructor();
        }
    }
}
