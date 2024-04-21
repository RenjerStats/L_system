using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model
{
    public class InstructionManager
    {
        public static ObservableCollection<Instruction> instructions = new ObservableCollection<Instruction>();

        public static ObservableCollection<Instruction> GetInstructions()
        {
            return instructions;
        }

        public static void AddInstructions(Instruction command)
        {
            instructions.Add(command);
        }

        public static void DeleteInstructions(Instruction command)
        {
            instructions.Remove(command);
        }
    }
}
