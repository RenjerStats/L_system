using L_system.AppTools.EventCommands;
using L_system.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace L_system.ViewModel
{
    public class ChangeToListViewModel
    {
        public ObservableCollection<Instruction> Change { get; set; }
        public ObservableCollection<Instruction> To { get; set; }
        public ICommand AddChangeCommand { get; set; }
        public ICommand DeleteChangeCommand { get; set; }
        public ICommand AddToCommand { get; set; }
        public ICommand DeleteToCommand { get; set; }

        public ChangeToListViewModel()
        {
            Change = new();
            To = new();
            AddChangeCommand = new RelayCommand((object parameter) => Change.Add(new Instruction()), (object parameter) => Change.Count <= 5);
            DeleteChangeCommand = new RelayCommand((object parameter) => Change.Remove((Instruction)parameter), (object parameter) => true);
            AddToCommand = new RelayCommand((object parameter) => To.Add(new Instruction()), (object parameter) => To.Count <= 5);
            DeleteToCommand = new RelayCommand((object parameter) => To.Remove((Instruction)parameter), (object parameter) => true);
        }
    }
}
