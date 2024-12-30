using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace L_system.Model.core.Nodes
{
    public class InputOfNode
    {
        private Func<object> refOnValue;

        public InputOfNode(object baseValue)
        {
            refOnValue = () => baseValue;
        }

        public InputOfNode(OutputOfNode output)
        {
            refOnValue = output.GetValue;
        }

        public object GetValue()
        {
            return refOnValue();
        }
    }

    public class OutputOfNode
    {
        private Func<object> refOnValue;

        public OutputOfNode(Func<object> refOnValue)
        {
            this.refOnValue = refOnValue;
        }
        public object GetValue()
        {
            return refOnValue();
        }
    }
}
