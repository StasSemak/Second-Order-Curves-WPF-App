using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
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
using System.Xaml.Schema;
using System.Text.RegularExpressions;

namespace functions_app
{
    public partial class MainWindow : Window
    {
        string equation;
        CurveType curveType;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string input_value = input.Text;
            if (string.IsNullOrWhiteSpace(input_value)) return;

            equation = input_value.Trim();
            DefineCurve();

            switch (curveType)
            {
                case CurveType.Circle:
                    DrawCircle();
                    break;
                case CurveType.Ellipse:
                    DrawEllipse();
                    break;
                case CurveType.Parabola:
                    DrawParabola();
                    break;
                case CurveType.Hyperbole:
                    DrawHyperbole();
                    break;
                default:
                    break;
            }
        }

        void DefineCurve() //needs negative values validation
        {
            Match circle_match = Regex.Match(equation, @"\(x-\d+\)\^2\+\(y-\d+\)\^2=\d+");
            Match ellipse_match = Regex.Match(equation, @"x\^2/\d+\+y\^2/\d+=1");
            Match ellipse_dot_match = Regex.Match(equation, @"\(x-\d+\)\^2/\d+\+\(y-\d+\)\^2/\d+=1");
            Match hyperbole_match = Regex.Match(equation, @"x\^2/\d+-y\^2/\d+=1");
            Match hyperbole_dot_match = Regex.Match(equation, @"\(x-\d+\)\^2/\d+-\(y-\d+\)\^2/\d+=1");
            Match parabola_y_match = Regex.Match(equation, @"y\^2=2\*p\*\d+");
            Match parabola_x_match = Regex.Match(equation, @"x\^2=2\*p\*\d+");
            Match parabola_y_dot_match = Regex.Match(equation, @"\(y-\d+\)\^2=2\*\d+\(x-\d+\)");
            Match parabola_x_dot_match = Regex.Match(equation, @"\(x-\d+\)\^2=2\*\d+\(y-\d+\)");

            if (circle_match.Success) curveType = CurveType.Circle;
            else if (ellipse_match.Success || ellipse_dot_match.Success) curveType = CurveType.Ellipse;
            else if (hyperbole_match.Success || hyperbole_dot_match.Success) curveType = CurveType.Hyperbole;
            else if (parabola_y_match.Success || parabola_x_match.Success || parabola_y_dot_match.Success ||
                parabola_x_dot_match.Success) curveType = CurveType.Parabola;
        }

        void DrawCircle() //needs fix
        {
            int x, y, r;
            string xstr = string.Empty, ystr = string.Empty, rstr = string.Empty;
            var equat_splitted = equation.Split('+', '=');
            for (int i = 0; i < equat_splitted[0].Length; i++)
            {
                if (i == equat_splitted[0].IndexOf(')')) break;
                if (int.TryParse(i.ToString(), out x)) xstr += i;
            }
            for (int i = 0; i < equat_splitted[1].Length; i++)
            {
                if (i == equat_splitted[1].IndexOf(')')) break;
                if (int.TryParse(i.ToString(), out y)) ystr += i;
            }
            for (int i = 0; i < equat_splitted[2].Length; i++) rstr += i;

            x = int.Parse(xstr);
            y = int.Parse(ystr);
            r = int.Parse(rstr);
        }


        void DrawEllipse()
        {

        }
        void DrawHyperbole()
        {

        }
        void DrawParabola()
        {

        }
    }
}
