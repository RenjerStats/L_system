using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    public class InputNode<T>
    {
        private OutputNode<T> connectedOutput;

        public InputNode(OutputNode<T> output)
        {
            connectedOutput = output;
        }

        public T GetValue()
        {
            return connectedOutput.GetValue();
        }
    }


    public interface IOutputNode<out T>
    {
        T GetValue();
    }
    public class OutputNode<T> : IOutputNode<T>
    {
        private Func<T> refOnValue;

        public OutputNode(Func<T> refOnValue)
        {
            this.refOnValue = refOnValue;
        }

        public OutputNode(T value)
        {
            refOnValue = () => value;
        }
        public T GetValue()
        {
            return refOnValue();
        }
    }
}
