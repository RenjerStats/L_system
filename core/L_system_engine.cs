using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace L_system.core
{
    public class L_system_engine(Command[] axiom, Dictionary<Command, Command[]> changeTo)
    {
        private List<Command> axiom = new (axiom);
        private readonly Dictionary<Command, Command[]> changeTo = changeTo;

        public Command[] GetInstructions() => axiom.ToArray();

        public void Iterate()
        {
            List<int> indexWhatAlreadyBeenUsed = [];

            foreach (var change in changeTo.Keys)
            {
                var to = changeTo[change];
                for (int i = 0; i < axiom.Count; i++)
                {
                    if (!indexWhatAlreadyBeenUsed.Contains(i) && change == axiom[i])
                    {
                            InsertingListWithReplacement(i, to);
                        for (int q = 0; q < to.Length; q++)
                            indexWhatAlreadyBeenUsed.Add(i + q);
                    }
                }
            }
        }

        private void InsertingListWithReplacement(int index, Command[] to)
        {
            if (axiom.Count == 1)
                axiom = new List<Command>(to);
            else
            {
                List<Command> right = axiom[(index + 1)..^0];
                axiom = axiom[0..index];

                axiom.AddRange(to);
                axiom.AddRange(right);
            }
        }
    }
}
