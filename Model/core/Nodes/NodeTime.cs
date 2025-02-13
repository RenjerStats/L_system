using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace L_system.Model.core.Nodes
{
    internal class NodeTime : Node
    {
        private ulong counter = 1;
        private DispatcherTimer timer;
        public NodeTime()
        {
            defaultInputs = [1D];

            Inputs = new ObservableCollection<InputOfNode>(new InputOfNode[1]);
            Outputs = [new OutputOfNode(() => (double)counter)];
            NameOfInputs = ["Период"];
            NameOfOutputs = ["Миллисек"];
            NameOfNode = "Таймер";

            FinalNodeConstructor();

            timer = new DispatcherTimer(DispatcherPriority.Input);
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            int period = Inputs[0] == null ? Convert.ToInt32(defaultInputs[0]) : Convert.ToInt32(Inputs[0].GetValue());
            if (timer.Interval != TimeSpan.FromMilliseconds(period))
            {
                timer.Interval = TimeSpan.FromMilliseconds(period);
            }
            counter++;
            OnPropertyChanged("Outputs");
        }

        ~NodeTime()
        {
            timer.Stop();
        }
    }
}
