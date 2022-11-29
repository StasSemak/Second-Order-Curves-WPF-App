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
        List<Brush> brushes;
        List<Brush> usedBrushes;
        int elements_count = 0;
        int cell = 20;

        public MainWindow()
        {
            InitializeComponent();
            brushes = new List<Brush>() { Brushes.Green, Brushes.Yellow, Brushes.Red, Brushes.Blue, Brushes.BlueViolet, Brushes.Brown, Brushes.Magenta, Brushes.Orange, 
                Brushes.Plum, Brushes.YellowGreen };
            usedBrushes = new List<Brush>();
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

        void DefineCurve()
        {
            Match circle_match = Regex.Match(equation, @"\(x(?:-|\+)\d+\)\^2\+\(y(?:-|\+)\d+\)\^2=\d+");
            Match ellipse_match = Regex.Match(equation, @"x\^2/\d+\+y\^2/\d+=1");
            Match ellipse_dot_match = Regex.Match(equation, @"\(x(?:-|\+)\d+\)\^2/\d+\+\(y(?:-|\+)\d+\)\^2/\d+=1");
            Match hyperbole_match = Regex.Match(equation, @"x\^2/\d+-y\^2/\d+=1");
            Match hyperbole_dot_match = Regex.Match(equation, @"\(x(?:-|\+)\d+\)\^2/\d+-\(y(?:-|\+)\d+\)\^2/\d+=1");
            Match parabola_y_match = Regex.Match(equation, @"y\^2=[-]?2\*\d+\*x");
            Match parabola_x_match = Regex.Match(equation, @"x\^2=[-]?2\*\d+\*y");
            Match parabola_y_dot_match = Regex.Match(equation, @"\(y(?:-|\+)\d+\)\^2=2\*\d+\(x(?:-|\+)\d+\)");
            Match parabola_x_dot_match = Regex.Match(equation, @"\(x(?:-|\+)\d+\)\^2=2\*\d+\(y(?:-|\+)\d+\)");

            if (circle_match.Success) curveType = CurveType.Circle;
            else if (ellipse_match.Success || ellipse_dot_match.Success) curveType = CurveType.Ellipse;
            else if (hyperbole_match.Success || hyperbole_dot_match.Success) curveType = CurveType.Hyperbole;
            else if (parabola_y_match.Success || parabola_x_match.Success || parabola_y_dot_match.Success ||
                parabola_x_dot_match.Success) curveType = CurveType.Parabola;
        }

        void DrawCircle()
        {
            var equation_splitted = equation.Split("^2");

            double x = double.Parse(Regex.Match(equation_splitted[0], @"\d+").Value);
            double y = double.Parse(Regex.Match(equation_splitted[1], @"\d+").Value);
            double r = Math.Sqrt(double.Parse(Regex.Match(equation_splitted[2], @"\d+").Value)) * 2;

            if (equation_splitted[0].Contains('+')) x *= -1;
            if (equation_splitted[1].Contains("y+")) y *= -1;

            Ellipse circle = new Ellipse();
            circle.Margin = new Thickness((26 - x) * cell - (r / 2) * cell, (14 - y) * cell - (r / 2) * cell, 0, 0);
            circle.Stroke = SetBrush();
            circle.StrokeThickness = 3;
            circle.Width = r * cell;
            circle.Height = r * cell;

            canvas.Children.Add(circle);
            elements_count++;
        }
        void DrawEllipse()
        {
            double x, y, x0 = 0, y0 = 0;
            if(Regex.Match(equation, @"x\^2/\d+\+y\^2/\d+=1").Success)
            {
                var equation_splitted = equation.Split('/', '+', '=');
                x = Math.Sqrt(double.Parse(equation_splitted[1])) * 2;
                y = Math.Sqrt(double.Parse(equation_splitted[3])) * 2;
            }
            else
            {
                var equation_splitted = equation.Split("^2");
                var sub_split = equation_splitted[1].Split('(');
                var sub_split2 = equation_splitted[2].Split('=');

                x0 = double.Parse(Regex.Match(equation_splitted[0], @"\d+").Value);
                y0 = double.Parse(Regex.Match(sub_split[1], @"\d+").Value);
                x = Math.Sqrt(double.Parse(Regex.Match(sub_split[0], @"\d+").Value)) * 2;
                y = Math.Sqrt(double.Parse(Regex.Match(sub_split2[0], @"\d+").Value)) * 2;

                if (equation_splitted[0].Contains('+')) x0 *= -1;
                if (sub_split[1].Contains('+')) y0 *= -1;
            }

            Ellipse ellipse = new Ellipse();
            ellipse.Margin = new Thickness((26 - x0) * cell - (x / 2) * cell, (14 - y0) * cell - (y / 2) * cell, 0, 0);
            ellipse.Stroke = SetBrush();
            ellipse.StrokeThickness = 3;
            ellipse.Width = x * cell;
            ellipse.Height = y * cell;

            canvas.Children.Add(ellipse);
            elements_count++;
        }
        void DrawHyperbole() //needs fix
        {
            double x, y, x0 = 0, y0 = 0;
            if (Regex.Match(equation, @"x\^2/\d+-y\^2/\d+=1").Success)
            {
                var equation_splitted = equation.Split('/', '-', '=');
                x = Math.Sqrt(double.Parse(equation_splitted[1]));
                y = Math.Sqrt(double.Parse(equation_splitted[3]));
            }
            else
            {
                var equation_splitted = equation.Split("^2");
                var sub_split = equation_splitted[1].Split('(');
                var sub_split2 = equation_splitted[2].Split('=');

                x0 = double.Parse(Regex.Match(equation_splitted[0], @"\d+").Value);
                y0 = double.Parse(Regex.Match(sub_split[1], @"\d+").Value);
                x = Math.Sqrt(double.Parse(Regex.Match(sub_split[0], @"\d+").Value));
                y = Math.Sqrt(double.Parse(Regex.Match(sub_split2[0], @"\d+").Value));

                if (equation_splitted[0].Contains('+')) x0 *= -1;
                if (sub_split[1].Contains('+')) y0 *= -1;
            }

            Polyline polyline = new Polyline();
            polyline.Stroke = Brushes.Black;
            polyline.StrokeThickness = 3;
            for (double i = x; i < 50; i++)
            {
                double xp = i;
                double yp = (y * Math.Sqrt(xp * xp - x * x)) / x;
                polyline.Points.Add(new Point(xp * 20, yp * 20));
            }
            for (double i = x; i < 50; i++)
            {
                double xp = i;
                double yp = -1.0 * ((y * Math.Sqrt(xp * xp - x * x)) / x);
                polyline.Points.Add(new Point(xp * 20, yp * 20));
            }
        }
        void DrawParabola()
        {
            input.Text = "parabola";
        }

        Brush SetBrush()
        {
            Random random = new();
            Brush brush = brushes[random.Next(0, 9)];

            if(usedBrushes.Count < 10)
                while(usedBrushes.Contains(brush)) brush = brushes[random.Next(0, 9)];

            if (usedBrushes.Count == 10)
            {
                usedBrushes.Clear();
            }
            usedBrushes.Add(brush);

            return brush;
        }

        private void input_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(input.Text)) input.Text = "Enter an equation";
        }
        private void input_GotFocus(object sender, RoutedEventArgs e)
        {
            if (input.Text == "Enter an equation") input.Text = string.Empty;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            canvas.Children.RemoveRange(canvas.Children.Count - elements_count, elements_count);
            elements_count = 0;
        }
    }
}
