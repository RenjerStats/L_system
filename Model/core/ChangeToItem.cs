using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core
{
    public class ChangeToItem
    {

        public Instruction Change { get; private set; }
        public ObservableCollection<Instruction> To { get; private set; }
        public ChangeToItem()
        {
            Change = new Instruction();
            To = [new Instruction()];
        }

        public ChangeToItem(Instruction change, Instruction[] to)
        {
            Change = change;
            To = new ObservableCollection<Instruction>(to);
        }

        public void AddChange()
        {

        }
        public void AddTo()
        {

        }

        public void DeleteChange()
        {

        }
        public void DeleteTo()
        {

        }
    }
}
