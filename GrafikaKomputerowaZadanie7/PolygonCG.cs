using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GrafikaKomputerowaZadanie7
{
    class PolygonCG
    {
        public Polygon polygon = new Polygon();
        
        public PolygonCG()
        {
            polygon.Points = new PointCollection();
            polygon.Fill = new SolidColorBrush(Colors.Red);
            //polygon.MouseDown += new MouseButtonEventHandler(onMouseDown);
            polygon.MouseMove += new MouseEventHandler(onMouseMove);
            polygon.MouseUp += new MouseButtonEventHandler(onMouseUp);
        }

        public Polygon onMouseDown(object sender, MouseButtonEventArgs e)
        {
            return this.polygon;

            //switch (Operation.option)
            //{
            //    case (Operation.Option.Move):
            //        if (IsSelected)
            //        {

            //        }
            //        break;
            //    case (Operation.Option.Resize):
            //        if (selectedObject != null)
            //        {
            //            selectedObject.resizeMouseDown(sender, e);
            //        }
            //        break;
            //    case (Operation.Option.Rotate):
            //        if (selectedObject != null)
            //        {
            //            selectedObject.rotateMouseDown(sender, e);
            //        }
            //        break;
            //    case (Operation.Option.Create):
            //        if (selectedObject != null)
            //        {
            //            selectedObject.createMouseDown(sender, e);
            //        }
            //        break;
            //}
        }
        public void onMouseMove(object sender, MouseEventArgs e)
        {

        }
        public void onMouseUp(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
