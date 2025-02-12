using L_system.Model.core;
using L_system.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DrawTest
{
    public class CommandTranslator
    {
        private Drawer drawSystem;

        public CommandTranslator(Drawer drawSystem)
        {
            this.drawSystem = drawSystem;
        }

        public DrawingGroup Convert(Command[] commands)
        {
            foreach (var command in commands)
            {
                switch (command.type)
                {
                    case CommandType.rotate:
                        drawSystem.RotatePen(command.val);
                        break;
                    case CommandType.move:
                        drawSystem.DrawLine(command.val);
                        break;
                    case CommandType.jump:
                        drawSystem.DrawLine(command.val);
                        break;
                    case CommandType.save:
                        drawSystem.SaveStartPosition();
                        break;
                    case CommandType.load:
                        drawSystem.LoadStartPosition();
                        break;
                    default:
                        break;
                }
            }

            return drawSystem.Lines;
        }
    }
}
