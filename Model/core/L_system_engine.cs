using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace L_system.Model.core
{
    public class L_system_engine(Instruction[] axiom, List<List<Instruction[]>> changeTo)
    {
        private readonly Instruction[] axiom = axiom;
        private readonly List<List<Instruction[]>> changeTo = changeTo;
        private List<Instruction> currentOperations = new(axiom);

        public Instruction[] GetInstructions() => currentOperations.ToArray();

        public void Iterate()
        {
            List<int> indexWhatAlreadyBeenUsed = [];

            foreach (var row in changeTo)
            {
                var change = row[0][0];
                var to = row[1];
                for (int i = 0; i < currentOperations.Count; i++)
                {
                    if (!indexWhatAlreadyBeenUsed.Contains(i) && change == currentOperations[i])
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
            if (currentOperations.Count == 1)
                currentOperations = new List<Instruction>(to);
            else
            {
                List<Instruction> right = currentOperations[(index + 1)..^0];
                currentOperations = currentOperations[0..index];

                currentOperations.AddRange(to);
                currentOperations.AddRange(right);
            }
        }
    }
}
