using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    internal class NodeTime : Node
    {
        private ulong counter = 1;
        private int oldPeriod = 1;
        private Timer timer;
        public NodeTime()
        {
            defaultInputs = [oldPeriod];

            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[1]);
            Outputs = [new OutputOfNode(() => (double)counter)];
            NameOfInputs = ["Период"];
            NameOfOutputs = ["Миллисекунды"];
            NameOfNode = "Таймер";

            SetEventConnection();

            timer = new Timer(UpdateOutput, null, 0, 1);
        }


        private void UpdateOutput(object? state)
        {
            int period = Inputs[0] == null ? (int)defaultInputs[0] : (int)Inputs[0].GetValue();
            if (period != oldPeriod)
            {
                timer.Change(0, period);
                oldPeriod = period;
            }
            counter++;
            OnPropertyChanged("Outputs");
        }

        ~NodeTime()
        {
            // Освобождаем ресурсы таймера
            timer?.Dispose();
        }
    }
}
