using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model
{
    public enum InstructionType
    {
        rotate = 'r',
        move = 'm',
        jump = 'j',
        save = '[',
        load = ']',
        nothingDoing1 = '1',
        nothingDoing2 = '2',
        nothingDoing3 = '3'
    }
    public readonly struct Instruction
    {
        public readonly float val;
        public readonly InstructionType type;

        public Instruction()
        {
            val = 0;
            type = InstructionType.nothingDoing1;
        }
        public Instruction(InstructionType type, float val = 0)
        {
            this.val = val;
            this.type = type;
        }

        public override readonly bool Equals(object? obj)
        {
            if (obj is not Instruction)
                return false;
            else
                return val == ((Instruction)obj).val && type == ((Instruction)obj).type;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(val, (char)type);
        }

        public override readonly string? ToString()
        {
            return $"{type}: {val}";
        }

        public static bool operator ==(Instruction left, Instruction right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Instruction left, Instruction right)
        {
            return !(left == right);
        }
    }
}
