using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace L_system.Model.core
{
    public class L_system_engine(Instruction[] axiom, Dictionary<Instruction, Instruction[]> changeTo)
    {
        private List<Instruction> axiom = new(axiom);
        private readonly Dictionary<Instruction, Instruction[]> changeTo = changeTo;

        public Instruction[] GetInstructions() => axiom.ToArray();

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

        private void InsertingListWithReplacement(int index, Instruction[] to)
        {
            if (axiom.Count == 1)
                axiom = new List<Instruction>(to);
            else
            {
                List<Instruction> right = axiom[(index + 1)..^0];
                axiom = axiom[0..index];

                axiom.AddRange(to);
                axiom.AddRange(right);
            }
        }
    }
}
