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
using System.Windows.Shapes;

namespace Database_Project
{
    /// <summary>
    /// Interaction logic for ShowCheckinsWindow.xaml
    /// </summary>
    public partial class ShowCheckinsWindow : Window
    {
        public ShowCheckinsWindow()
        {
            InitializeComponent();
            //List<KeyValuePair<string, int>> myData = new List<KeyValuePair<string, int>>();
            //myData.Add(new KeyValuePair<string, int>("A", 1));
            //myData.Add(new KeyValuePair<string, int>("B", 2));
            //myData.Add(new KeyValuePair<string, int>("C", 3));
            //checkinChart.DataContext = myData;
        }
    }
} 
