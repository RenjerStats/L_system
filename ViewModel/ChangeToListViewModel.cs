﻿using L_system.AppTools.EventCommands;
using L_system.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InstructionManager = L_system.Model.InstructionManager;

namespace L_system.ViewModel
{
    public class InstructionsListViewModel
    {
        public ObservableCollection<Instruction> Instructions { get; set; }
        public ICommand AddInstructionCommand { get; set; }
        public ICommand DeleteInstructionCommand { get; set; }

        public InstructionsListViewModel()
        {
            Instructions = InstructionManager.GetInstructions();
            AddInstructionCommand = new RelayCommand(AddBlock, CanAddBlock);
            DeleteInstructionCommand = new RelayCommand(DeleteBlock, CanDeleteBlock);
        }

        private bool CanDeleteBlock(object obj)
        {
            return Instructions.Count != 0;
        }

        private void DeleteBlock(object obj)
        {
            Instructions.Remove((Instruction)obj);
        }

        private bool CanAddBlock(object obj)
        {
            return Instructions.Count < 10;
        }

        private void AddBlock(object obj)
        {
            Instructions.Add(new Instruction());
        }
    }
}
