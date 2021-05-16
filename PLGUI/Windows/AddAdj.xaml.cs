using BL;
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
using BO;
using BLAPI;
using System.Collections.ObjectModel;

namespace PLGUI
{
	/// <summary>
	/// Interaction logic for AddAdj.xaml
	/// </summary>
	public partial class AddAdj : Window
	{
		IBL bl = BLFactory.GetBL();

        #region Properties
        public List<BO.Station> Selectable { get; set; }
		public List<BO.Station> Selected { get; set; }
		public AdjacentStation adj{ get; set; }
        #endregion

        public AddAdj(IEnumerable<BO.AdjacentStation> exists, int? code = null)
		{
			InitializeComponent();
			if (code != null)
			{
				Selectable = new List<BO.Station>(bl.GetAllStationsBy(st => st.Code != code && !exists.Any((t) => t.AdjStation.Code == st.Code)).ToList());
			}
			else Selectable = new List<BO.Station>(bl.GetAllStations());

			Selected = new List<BO.Station>();
			DataContext = this;
			adj = new AdjacentStation();
			adj.AverageTime = TimeSpan.Zero;
		}

		/// <summary>
		/// checks if to add it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			Selected.Add(((CheckBox)sender).DataContext as BO.Station);
			btnOk.IsEnabled = Selected.Count > 0;
		}
		/// <summary>
		/// checks if to add it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			Selected.Remove(((CheckBox)sender).DataContext as BO.Station);
			btnOk.IsEnabled = Selected.Count > 0;
		}
		/// <summary>
		/// an event that when you press ok the result is true
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ok_click(object sender, RoutedEventArgs e)
		{
			adj.AverageTime = TimeSpan.Parse(txbTime.Text);
			DialogResult = true;
		}
	}
}
