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
using BLAPI;
using System.Collections.ObjectModel;

namespace PLGUI
{
    /// <summary>
    /// Interaction logic for LinesDetails.xaml
    /// </summary>
    public partial class LinesDetails : Window
    {
        IBL bl = BLFactory.GetBL();
  
        #region Properties
        public BO.Line MyLine { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool CanModified { get; set; }
        public bool CodeFull => lcode.Text != "" && CanModified;
        public BO.AdjacentStation Adj { get; set; }
        public ObservableCollection<Stat> RestStations { get; set; }
        public ObservableCollection<BO.LineStation> stationInLine { get; set; } = new ObservableCollection<BO.LineStation>();
        #endregion

        public LinesDetails(BO.Line line, bool isAdd, bool up = false)
        {
            InitializeComponent();
            MyLine = line;
            Add = isAdd;
            Update = up;
            CanModified = Update || Add;
            RestStations = new ObservableCollection<Stat>(from item in bl.GetAllStations() select new Stat(item.Code, item.Name));
            if (!Add)
            {
                lcode.IsEnabled = false;
                if (MyLine.StationsRoute != null)
                {
                    foreach (var item in MyLine.StationsRoute)
                        RestStations.Remove(RestStations.First(it => it.Code == item.Station.Code));
                }
                if(MyLine.StationsRoute != null)
                stationInLine = new ObservableCollection<BO.LineStation>(MyLine.StationsRoute);
                else
                    stationInLine = new ObservableCollection<BO.LineStation>();
            }
            cmbxStation.SelectedItem = RestStations[0];
            DataContext = this;
            if (Add && line == null)
            {
                MyLine = new BO.Line() { ID = bl.GetLineId() };
                bl.UpdateLineId();
                stationInLine = new ObservableCollection<BO.LineStation>();
                cmbxStation.IsEnabled = false;
                txbDist.IsEnabled = false;
                txbTime.IsEnabled = false;
            }
            else if (!Add && !up)
            {
                larea.SelectedValue = new AREA { Line_Area = MyLine.Area };
                larea.IsEnabled = false;
                lcode.IsEnabled = false;
            }
            if(Add)
            {
                adding.IsEnabled = false;
            }
            Station.ItemsSource = stationInLine;
            List<AREA> line_Areas = new List<AREA> { new AREA { Line_Area = BO.Line_Area.Eilat }, new AREA { Line_Area = BO.Line_Area.Haifa }, new AREA { Line_Area = BO.Line_Area.Herzelia }, new AREA { Line_Area = BO.Line_Area.Jerusalem }, new AREA { Line_Area = BO.Line_Area.Netanya }, new AREA { Line_Area = BO.Line_Area.Ramat_Gan }, new AREA { Line_Area = BO.Line_Area.Tel_Aviv } };
            larea.ItemsSource = line_Areas;
        }
        /// <summary>
        /// an event to save the data
        /// </summary>

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (larea.Text == "" || lcode.Text == "")
                MessageBox.Show("Not all ditails are set");
            else
            {
                DialogResult = true;
                if (Add)
                {
                    MessageBox.Show("The line added succsesfully!");
                }
                else
                {
                    MessageBox.Show("The line update succsesfully!");
                }
                Close();
            }
        }
        /// <summary>
        /// text box that allows only numbers to be entered
        /// </summary>

        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Controls.TextBox text = sender as TextBox;

            if (e.Key == Key.D0 || e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4 || e.Key == Key.D5 || e.Key == Key.D6 || e.Key == Key.D7 || e.Key == Key.D8 || e.Key == Key.D9 || e.Key == Key.Enter || e.Key == Key.Delete || e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
        e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
     || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;
            e.Handled = true;
        }
        /// <summary>
        ///  an event to close this window
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        /// <summary>
        /// an event to open the add station in line winndow
        /// </summary>

        private void AddStation_Click(object sender, RoutedEventArgs e)
        {
            BO.LineStation ToAdd = new BO.LineStation() { Station = bl.GetStation(((Stat)cmbxStation.SelectedItem).Code), Distance = 0, DrivingTime = TimeSpan.Zero, ID = bl.GetLineStationId(), IndexInLine = MyLine.RouteLength, LineId = MyLine.ID };
                 
            bl.AddLineStation(ToAdd);
            stationInLine.Add(ToAdd);
            MyLine.RouteLength++;

            if (Adj != null)
            {                
                Adj.Distance = int.Parse(txbDist.Text); 
                Adj.AverageTime = TimeSpan.Parse(txbTime.Text);
                stationInLine.Last().Distance = Adj.Distance;
                stationInLine.Last().DrivingTime = Adj.AverageTime;

                try
                {
                    if (Adj.AdjStation.Code != stationInLine.Last().Station.Code)
                        bl.AddAdjacentStations(Adj, stationInLine.Last().Station.Code);
                }
                catch (BO.BadAdjacentStationsException)
                {
                    bl.UpdateAdjacentStations(Adj, stationInLine.Last().Station.Code);
                }
            }
            RestStations.Remove(RestStations.First(it => it.Code == ToAdd.Station.Code));
            cmbxStation.SelectedItem = RestStations[0];
            
            bl.UpdateLineStationId();

            MyLine.StationsRoute = stationInLine;
            MyLine.FirstStation = stationInLine.FirstOrDefault().Station.Code ;
            MyLine.LastStation = stationInLine.Last().Station.Code;
        }
        /// <summary>
        ///  an event to open the trips window
        /// </summary>

        private void Trip_Click(object sender, RoutedEventArgs e)
        {
            BusOnTrip busOnTrips = new BusOnTrip(MyLine.ID.ToString(), false, 0, CanModified);
            busOnTrips.Show();
        }
        /// <summary>
        ///  an event to delete a line station 
        /// </summary>

        private void DeleteStations_Click(object sender, RoutedEventArgs e)
        {
            BO.LineStation s = ((Button)sender).DataContext as BO.LineStation;
            foreach (var item in stationInLine)
                if (item.IndexInLine > s.IndexInLine)
                {
                    item.IndexInLine -= 1;
                }
            MyLine.RouteLength--;
            bl.DeleteLineStationsBy(it => it.LineId == s.ID);
            stationInLine.Remove(s);
            if (stationInLine != null && stationInLine.Count() > 0)
            {
                MyLine.FirstStation = stationInLine.First().Station.Code;
                MyLine.LastStation = stationInLine.Last().Station.Code;
                stationInLine.Last().Distance = 0;
                stationInLine.Last().DrivingTime = TimeSpan.Zero;
            }
            MyLine.StationsRoute = stationInLine;
            bl.UpdateLine(MyLine);
        }
        private void cmbxStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Stat station = ((ComboBox)sender).SelectedItem as Stat;
            if(MyLine.StationsRoute!=null) { 
            if (MyLine.StationsRoute.Count() > 0 && station != null)
            {
                try
                {
                    Adj = bl.GetAdjacentStations(station.Code, MyLine.LastStation);

                }
                catch (Exception)
                {
                    Adj = new BO.AdjacentStation() { AdjStation = stationInLine.Last().Station, AverageTime = TimeSpan.Zero, Distance = 0 };
                }
                txbDist.Text = Adj.Distance.ToString();
                txbTime.Text = Adj.AverageTime.ToString();
            }
            }
        }
        private void lcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(lcode.Text, out int x) && x > 0 && CanModified)
                save.IsEnabled = true;
            else
                save.IsEnabled = false;
        }
    }

}

