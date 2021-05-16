using BLAPI;
using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace PLGUI
{
	/// <summary>
	/// Interaction logic for addStationWindow.xaml
	/// </summary>
	public partial class AddUpdateStation : Window
	{

		IBL bl = BLFactory.GetBL();
		public Station MyStation { get; set; }
		bool Update;
		ObservableCollection<BO.AdjacentStation> Adjs;
		public AddUpdateStation(int? code = null)
		{
			InitializeComponent();
			if (code == null)//add
			{
				Update = false;
				MyStation = new BO.Station() { AdjacentStations = new List<BO.AdjacentStation>(), };
				Adjs = new ObservableCollection<BO.AdjacentStation>();
			}
			else //update
			{
				Update = true;
				MyStation = bl.GetStationWithAdjacents(code ?? 0);
				Adjs = new ObservableCollection<BO.AdjacentStation>(MyStation.AdjacentStations);
				txbCode.IsReadOnly = true;
			}
			DataContext = MyStation; //binding
			listAdj.ItemsSource = Adjs;
		}

		/// <summary>
		/// an event to close this window
		/// </summary>

		private void Cancel_click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private void Save_click(object sender, RoutedEventArgs e)
		{
			if (txbLongitude.Text==""|| txbLatitude.Text == "" || txbAddress.Text == "" || txbName.Text =="" || txbRegion.Text == ""|| txbCode.Text=="")
					
				MessageBox.Show("Not all ditails are set");
			else
			{
				DialogResult = true;
				Close();
			}
		}
		/// <summary>
		/// an event to remove the station
		/// </summary>

		private void RemoveAdj_Click(object sender, RoutedEventArgs e)
		{
			BO.AdjacentStation s = ((Button)sender).DataContext as BO.AdjacentStation;
			Adjs.Remove(s);
			MyStation.AdjacentStations.ToList().Remove(s);
		}
		/// <summary>
		///  an event to add the station
		/// </summary>

		private void AddAdjStation_Click(object sender, RoutedEventArgs e)
		{
			AddAdj ad = new AddAdj(Adjs, MyStation.Code);
			if (ad.ShowDialog() == true)
			{
				foreach (BO.Station item in ad.Selected)
				{
					BO.AdjacentStation adjacent = new BO.AdjacentStation { AdjStation = item, AverageTime = ad.adj.AverageTime, Distance = ad.adj.Distance };
					Adjs.Insert(0,adjacent);
					MyStation.AdjacentStations = Adjs;
					bl.AddAdjacentStations(adjacent, item.Code);
				}
			}
		}

		private void txbCode_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!Update && int.TryParse(txbCode.Text, out int code))
				if (CodeExist(code))
				{
					txbCode.BorderBrush = Brushes.Salmon;
				}
				else
				{
					txbCode.BorderBrush = Brushes.LightGray;
				}
		}

		private bool CodeExist(int code)
		{
			try
			{
				bl.GetStation(code);
				return true;
			}
			catch (BO.BadStationKeyException) { return false; }
		}

	}
}