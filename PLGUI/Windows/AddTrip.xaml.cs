using BLAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddTripWindow.xaml
    /// </summary>
   
    public partial class AddTrip : Window
    {
        IBL bl = BLFactory.GetBL();
        public BO.BusOnTrip MyTrip { get; set; }
        public bool Add;
        public ObservableCollection<BO.Bus> buses;

        /// <summary>
        /// a window constructor
        /// </summary>
        /// <param name="ADD">is add or update</param>
        /// <param name="fromBus">is the window open from bus</param>
        /// <param name="busOn">if it is update the trip to update</param>
        public AddTrip(bool ADD, bool fromBus, int index = 0, BO.BusOnTrip busOn = default)
        {
            InitializeComponent();
            MyTrip = busOn;
            Add = ADD;
            if (ADD)
            {
                MyTrip = new BO.BusOnTrip();
                MyTrip.ID = bl.GetBusOnId();
            }
            else cmbLines.SelectedValue = MyTrip.LineId;
            DataContext = MyTrip;
            buses = new ObservableCollection<BO.Bus>(bl.GetAllBus());
            cmbLicens.ItemsSource = buses;
            if (fromBus)
            {
                cmbLicens.SelectedIndex = index;
                cmbLicens.IsEnabled = false;
            }
            else
            {
                cmbLines.SelectedIndex = index;
                cmbLines.IsEnabled = false;
            }
            cmbLicens.DisplayMemberPath = "LicensNumber";
            cmbLines.ItemsSource = bl.GetAllLines();
            cmbLines.DisplayMemberPath = "LineCode";
            ato.Text = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToString();

        }
        /// <summary>
        /// an event to save trip we added
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (cmbLicens.SelectedItem == null || cmbLines.SelectedItem == null  || (ato.Text == TimeSpan.Zero.ToString()))
                MessageBox.Show("Not all ditails are set");
            else if (driver.Text.Length < 9)
                MessageBox.Show("Driver Id short then 9 letters");
            else
            {
                DialogResult = true;
                if (Add)
                {
                    MyTrip.LineId = (cmbLines.SelectedItem as BO.Line).ID;
                    MyTrip.LineNumber = (cmbLines.SelectedItem as BO.Line).LineCode;
                    MyTrip.PrevStation = (cmbLines.SelectedItem as BO.Line).FirstStation;
                    MyTrip.LicensNumber = (cmbLicens.SelectedItem as BO.Bus).LicensNumber;
                    MyTrip.ActualTakeOff = TimeSpan.Parse(ato.Text);
                    MessageBox.Show("The trip added succsesfully!");
                    GoToTrip(MyTrip, (cmbLines.SelectedItem as BO.Line));
                }
                else
                {
                    MessageBox.Show("The trip update succsesfully!");
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
        /// an event to close this window
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        /// <summary>
        /// starts the simulation
        /// </summary>
        public void GoToTrip(BO.BusOnTrip b, BO.Line l)
        {
            PO.CurrentStation current;
            if (l.StationsRoute != null && l.StationsRoute.Count() > 0)
            {
                current = new PO.CurrentStation { TripStartTime = b.ActualTakeOff, EndStationName = l.StationsRoute.ToList().Last().Station.Address, UplineStations = l.StationsRoute, LineID = l.ID, LineNum = l.LineCode };
                PO.CurrentStation.LineTimings.Add(current);
                if (current.simulation == null)
                    current.simulation = new SimulationOfTrip(current);
                current.simulation.Starting();
            }
        }
    }
}
