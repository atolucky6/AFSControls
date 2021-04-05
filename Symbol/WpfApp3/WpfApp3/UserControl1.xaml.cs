using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
       // private String myXAML =
        public UserControl1()
        {
            InitializeComponent();
            // MessageBox.Show("1");
            string path = @"C:\Users\nguye\source\repos\WpfApp3\WpfApp3\bin\Debug\Commercial building 3.xaml";
            StreamReader mysr = new StreamReader(path);
            //  MessageBox.Show(mysr.BaseStream.ToString());

            Canvas myCanvas = (Canvas)XamlReader.Load(File.OpenRead(path));
            Canvas canvas = XamlReader.Parse(mysr.BaseStream.ToString()) as Canvas;
            Viewbox dg = XamlReader.Load(mysr.BaseStream) as Viewbox;
            dg.Stretch = Stretch.Fill;
            dg.ClearValue(Control.HeightProperty);
            dg.ClearValue(Control.WidthProperty);
            parentGrid.Children.Add(dg);



        }
    }
}

