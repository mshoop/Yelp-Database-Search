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
	/// Interaction logic for TipsTable.xaml
	/// </summary>
	public partial class TipsTable : Window
    {
		public TipsTable()
        {
			InitializeComponent();
		}

		public class Data
        {
			public string user { get; set;}
			public string date  {get; set;}
			public string numLikes  {get; set;}
			public string text { get; set; }
		}
	}
}
