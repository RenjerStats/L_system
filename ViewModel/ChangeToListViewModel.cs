using L_system.AppTools.EventCommands;
using L_system.Model;
using L_system.Model.core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChangeToManager = L_system.Model.ChangeToManager;

namespace L_system.ViewModel
{
    public class ChangeToListViewModel
    {
        public ObservableCollection<ChangeToItem> Items { get; set; }
        public ICommand AddInstructionCommand { get; set; }
        public ICommand DeleteInstructionCommand { get; set; }

        public ChangeToListViewModel()
        {
            Items = ChangeToManager.items;
            AddInstructionCommand = new RelayCommand(AddBlock, CanAddBlock);
            DeleteInstructionCommand = new RelayCommand(DeleteBlock, CanDeleteBlock);
        }

        private bool CanDeleteBlock(object obj)
        {
            return Items.Count != 0;
        }

        private void DeleteBlock(object obj)
        {
            Items.Remove((ChangeToItem)obj);
        }

        private bool CanAddBlock(object obj)
        {
            return Items.Count < 10;
        }

        private void AddBlock(object obj)
        {
            Items.Add(new ChangeToItem());
        }
    }
}
