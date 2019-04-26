using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorMoveUI
{
    class ContentToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int IColor = 0;
            if (value is TextBlock TextValue)
            {
                //Console.WriteLine("Text: " + TextValue.Text);
                IColor = int.Parse(TextValue.Text);
            }
            string ResColor = "#FFCCCCCC";
            switch (IColor)
            {
                case 0:
                    ResColor = "#FFCCCCCC";
                    break;
                case 1:
                    ResColor = "red";
                    break;
                case 2:
                    ResColor = "Orange";
                    break;
                case 3:
                    ResColor = "Yellow";
                    break;
                case 4:
                    ResColor = "green";
                    break;
                case 5:
                    ResColor = "RoyalBlue";
                    break;

            }
            //Console.WriteLine(value);
            return ResColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
