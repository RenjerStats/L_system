using L_system.Model.core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model
{
    public static class ChangeToManager
    {
        public static ObservableCollection<ChangeToItem> items = new ObservableCollection<ChangeToItem>();

        public static void SetChangeToItems(ChangeToItem[] changeToItems)
        {
            items = new ObservableCollection<ChangeToItem>(changeToItems);
        }
        public static List<List<Instruction[]>> CollectionToList()
        {
            List<List<Instruction[]>> result = new List<List<Instruction[]>>();

            foreach (ChangeToItem item in items)
            {
                result.Add([[item.Change], item.To.ToArray()]);
            }

            return result;
        }
    }
}
