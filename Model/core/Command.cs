using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core
{
    public enum CommandType
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
    public readonly struct Command(CommandType type, double val = 0)
    {
        public readonly double val = val;
        public readonly CommandType type = type;

        public override readonly bool Equals(object? obj)
        {
            if (obj is not Command)
                return false;
            else
                return val == ((Command)obj).val && type == ((Command)obj).type;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(val, (char)type);
        }

        public override readonly string? ToString()
        {
            return $"{type}: {val}";
        }

        public static bool operator ==(Command left, Command right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Command left, Command right)
        {
            return !(left == right);
        }
    }
}
