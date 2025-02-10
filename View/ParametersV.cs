using L_system.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace L_system.View
{
    public class ParametersV
    {
        private NodeVM node;
        private int parameterIndex;

        public ParametersV(NodeVM node, int inputIndex)
        {
            this.node = node;
            this.parameterIndex = inputIndex;
        }

        public UIElement CreateForm()
        {
            string type = node.GetTypeOfParameter(parameterIndex);

            if (type == "Double")
            {
                Slider slider = new Slider()
                {
                    Minimum = 1,
                    Maximum = 100,
                    Value = 10
                };
                return slider;
            }
            else
            {
                ComboBox comboBox = new ComboBox()
                {
                    ItemsSource = new TextBox[]
                    {
                      CreateSimpleText("Рисование линии"), CreateSimpleText("Перемещение курсора"), CreateSimpleText("Поворот курсора"),
                      CreateSimpleText("Сохранение курсора"), CreateSimpleText("Загрузка курсора")
                    },

                };
                //comboBox.SelectionChanged

                return comboBox;
            }
        }

        private TextBox CreateSimpleText(string content)
        {
            TextBox name = new TextBox();
            name.Foreground = Brushes.White;
            name.FontFamily = new FontFamily("Arial");
            name.FontSize = 12;
            name.Background = Brushes.Transparent;
            name.BorderBrush = Brushes.Transparent;
            name.Text = content;
            name.HorizontalContentAlignment = HorizontalAlignment.Center;
            name.VerticalContentAlignment = VerticalAlignment.Center;
            name.IsEnabled = false;
            return name;
        }
    }
}
