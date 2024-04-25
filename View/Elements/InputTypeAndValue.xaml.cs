using L_system.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace L_system.View.Elements
{
    /// <summary>
    /// Логика взаимодействия для InputTypeAndValue.xaml
    /// </summary>
    /// 

    public partial class InputTypeAndValue : UserControl
    {

        public int maxVal = 100;
        public int minVal = 0;
        public int value = 50;
        public int step = 5;
        Dictionary<InstructionType, string> InstructionsName = new Dictionary<InstructionType, string>()
        {
            { InstructionType.jump , "Переместить курсор на \'N\'" },
            { InstructionType.save , "Сохранить позицию в стек" },
            { InstructionType.load , "Загрузить позицию из стека" },
            { InstructionType.move , "Провести прямую линию, длинной \'N\'" },
            { InstructionType.rotate , "Повернуть курсор на \'N\' градусов" },
            { InstructionType.nothingDoing1 , "Пустая функция 1" },
            { InstructionType.nothingDoing2 , "Пустая функция 2" },
            { InstructionType.nothingDoing3 , "Пустая функция 3" },
        };

        public InputTypeAndValue()
        {
            InitializeComponent();
            TypeListBox.ItemsSource = InstructionsName.Values;
        }

        private void ButtonUP_Click(object sender, RoutedEventArgs e)
        {
            value += step;
            value = Math.Clamp(value, minVal, maxVal);
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            value -= step;
            value = Math.Clamp(value, minVal, maxVal);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(TextBox.Text, out int res))
            {
                value = Math.Clamp(res, minVal, maxVal);
            }
            else
            {
                //Something
            }
        }
    }
}
