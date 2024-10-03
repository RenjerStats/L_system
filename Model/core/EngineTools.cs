using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core
{
    public static class EngineTools
    {
        public static Dictionary<Command, Command[]> ToDictionary(Command[] changes, Command[][] to)
        {
            Dictionary<Command, Command[]> result = new Dictionary<Command, Command[]>();
            if (changes.Length != to.GetLength(0)) return null;

            for (int i = 0; i < changes.Length; i++)
            {
                result.Add(changes[i], to[i]);
            }
            return result;
        }
    }
}
