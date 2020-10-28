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

namespace GrafikaKomputerowaZadanie7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Polygon> objectsList = new List<Polygon>();
        Polygon selectedObject;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void listLandmarks_Selected(object sender, RoutedEventArgs e)
        {
            var list = (ListView)sender;
            try
            {
                var obj = list.SelectedItem;
                var elem = (Point)obj;
                XValue.Value = elem.X;
                YValue.Value = elem.Y;
            }
            catch(NullReferenceException nre)
            {
                System.Diagnostics.Debug.WriteLine(nre);
            }
        }

        private void CreateNewPoint_Click(object sender, RoutedEventArgs e)
        {
            if(selectedObject != null)
            {
                selectedObject.Points.Add(new Point());
                listLandmarks.Items.Refresh();
            }
        }

        private void UpdatePoint_Click(object sender, RoutedEventArgs e)
        {
            int index = listLandmarks.SelectedIndex;
            if (index == -1)
                return;
            selectedObject.Points[index] = new Point((double)XValue.Value, (double)YValue.Value);
            listLandmarks.Items.Refresh();
        }

        private void ShiftPoint_Click(object sender, RoutedEventArgs e)
        {
            double? xvalue = XPrim.Value;
            double? yvalue = YPrim.Value;
            if (xvalue == null || yvalue == null || selectedObject == null)
                return;
            for (int index = 0; index < selectedObject.Points.Count; index++)
            {
                selectedObject.Points[index] = new Point(selectedObject.Points[index].X + (double)xvalue, selectedObject.Points[index].Y + (double)yvalue);
            }
            listLandmarks.Items.Refresh();
        }

        private void SelectOperation(object sender, RoutedEventArgs e)
        {
            try
            {
                ICommandSource option = (ICommandSource)e.OriginalSource;
                System.Diagnostics.Debug.WriteLine(option.CommandParameter);
                switch (option.CommandParameter)
                {
                    case "Select":
                        Operation.option = Operation.Option.Select;
                        break;
                    case "Move":
                        Operation.option = Operation.Option.Move;
                        break;
                    case "Resize":
                        Operation.option = Operation.Option.Resize;
                        break;
                    case "Rotate":
                        Operation.option = Operation.Option.Rotate;
                        break;
                    case "Create":
                        Operation.option = Operation.Option.Create;
                        var elem = new Polygon();
                        elem.Points = new PointCollection();
                        elem.Fill = new SolidColorBrush(Colors.Transparent);
                        elem.Stroke = new SolidColorBrush(Colors.Red);
                        objectsList.Add(elem);
                        imageCanvas.Children.Add(elem);
                        listLandmarks.ItemsSource = elem.Points;
                        selectedObject = elem;
                        break;
                }
            }
            catch (InvalidCastException ice)
            {
                System.Diagnostics.Debug.WriteLine(ice);
            }
        }

        public void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (Operation.option)
            {
                case (Operation.Option.Select):
                    foreach (var elem in objectsList)
                    {
                        selectedObject = null;
                        if (elem.IsMouseOver)
                        {
                            selectedObject = elem;
                            listLandmarks.ItemsSource = elem.Points;
                            break;
                        }
                        listLandmarks.ItemsSource = null;

                    }
                    break;
                case (Operation.Option.Move):
                    if (selectedObject != null)
                    {
                        Canvas canvas = (Canvas)sender;
                        Point newValue = e.GetPosition(canvas);
                        Point oldValue = selectedObject.Points[0];

                        Point vector = new Point(newValue.X - oldValue.X, newValue.Y - oldValue.Y);

                        for(int index = 0; index< selectedObject.Points.Count; index++)
                        {
                            selectedObject.Points[index] = new Point(selectedObject.Points[index].X + vector.X, selectedObject.Points[index].Y + vector.Y);
                        }
                        listLandmarks.Items.Refresh();
                    }
                    break;
                case (Operation.Option.Resize):
                    if (selectedObject != null)
                    {
                        Canvas canvas = (Canvas)sender;
                        Point xsys = e.GetPosition(canvas);
                        double k = 1;
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            k = 1.01;
                        }
                        else if (e.RightButton == MouseButtonState.Pressed)
                        {
                            k = 0.99;
                        }
                        for (int index = 0; index < selectedObject.Points.Count; index++)
                        {
                            selectedObject.Points[index] = new Point((selectedObject.Points[index].X*k) + ((1-k) * xsys.X),
                                (selectedObject.Points[index].Y * k) + ((1 - k) * xsys.Y)
                                );
                        }
                        listLandmarks.Items.Refresh();
                    }
                    break;
                case (Operation.Option.Rotate):
                    if (selectedObject != null)
                    {
                        Canvas canvas = (Canvas)sender;
                        Point xoyo = e.GetPosition(canvas);
                        double alpha = 0;
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            alpha = Math.PI * 5/ 180;
                        }
                        else if (e.RightButton == MouseButtonState.Pressed)
                        {
                            alpha = (2 * Math.PI) - (Math.PI * 5/ 180);
                        }
                        RotateObject(xoyo, alpha);
                        listLandmarks.Items.Refresh();
                    }
                    break;
                case (Operation.Option.Create):
                    if (selectedObject != null)
                    {
                        Canvas canvas = (Canvas)sender;
                        Point point = e.GetPosition(canvas);
                        selectedObject.Points.Add(point);
                        listLandmarks.Items.Refresh();
                    }
                    break;
            }
        }

        
        public void RotateObject(Point xoyo, double alpha)
        {
            if (selectedObject == null)
                return;
            for (int index = 0; index < selectedObject.Points.Count; index++)
            {
                selectedObject.Points[index] = new Point(
                    xoyo.X + (selectedObject.Points[index].X - xoyo.X) * Math.Cos(alpha) - (selectedObject.Points[index].Y - xoyo.Y) * Math.Sin(alpha),
                    xoyo.Y + (selectedObject.Points[index].X - xoyo.X) * Math.Sin(alpha) + (selectedObject.Points[index].Y - xoyo.Y) * Math.Cos(alpha)
                    );
            }
        }

        private void ScalePoint_Click(object sender, RoutedEventArgs e)
        {
            double? xvalue = XPrim.Value;
            double? yvalue = YPrim.Value;
            double? k = ScaleValue.Value;

            if (k == null || xvalue == null || yvalue == null || selectedObject == null)
                return;
            for (int index = 0; index < selectedObject.Points.Count; index++)
            {
                selectedObject.Points[index] = new Point((selectedObject.Points[index].X * (double)k) + ((1 - (double)k) * (double)xvalue),
                    (selectedObject.Points[index].Y * (double)k) + ((1 - (double)k) * (double)yvalue)
                    );
            }
            listLandmarks.Items.Refresh();
        }

        private void RotatePoint_Click(object sender, RoutedEventArgs e)
        {
            double? xvalue = XPrim.Value;
            double? yvalue = YPrim.Value;
            double? alpha = RotateValue.Value;

            if (alpha == null || xvalue == null || yvalue == null || selectedObject == null)
                return;
            RotateObject(new Point((double)xvalue, (double)yvalue), Math.PI * (double)alpha / 180);
            listLandmarks.Items.Refresh();
        }
    }
}
