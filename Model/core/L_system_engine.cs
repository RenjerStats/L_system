﻿namespace L_system.Model.core
{
    public class L_system_engine(Command[] axiom, Command change, Command[] to)
    {
        private List<Command> axiom = new(axiom);
        private readonly Command change = change;
        private readonly Command[] to = to;

        public Command[] GetCommands() => axiom.ToArray();

        public void Iterate()
        {
            List<int> indexWhatAlreadyBeenUsed = [];

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
