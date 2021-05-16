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
    /// Interaction logic for BusOnTrips.xaml
    /// </summary>
    public partial class BusOnTrip : Window
    {
        string Code;
        bool isBus;
        int Index;
        public bool CanAddTrip { get; set; }
        IBL bl = BLFactory.GetBL();
        
        ObservableCollection<BO.BusOnTrip> trips;
        public BusOnTrip(string number, bool fromBus, int index = 0,bool canAddTrip=true)
        {
            InitializeComponent();
            CanAddTrip = canAddTrip;
            isBus = fromBus;
            Index = index;
            Code = number;
            if (fromBus)
            {
                trips = new ObservableCollection<BO.BusOnTrip>(bl.GetAllBusTrips(Code));
            }
            else
            {
                trips = new ObservableCollection<BO.BusOnTrip>(bl.GetAllLineTrips(int.Parse(Code)));
            }
            busTrips.ItemsSource = trips;
            busTrips.DataContext = trips;
            DataContext = this;

        }

        private void busTrips_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        /// <summary>
        /// an event that opens the add window
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddTrip trip = new AddTrip(true, isBus, Index);
            if (trip.ShowDialog() == true)
            {
                trips.Add(trip.MyTrip);
                bl.AddBusOnTrip(trip.MyTrip);
                busTrips.ItemsSource = trips;
            }
        }
        /// <summary>
        /// an event that opens the delte window
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            BO.BusOnTrip b = ((Button)sender).DataContext as BO.BusOnTrip;
            bl.DeleteBusOnTrip(b.ID);
            trips.Remove(b);
            MessageBox.Show("The Trip: " + b.ID + " deleted succsesfully!", "Done succusesfully", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// an event that opens the updade window
        /// </summary>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            BO.BusOnTrip b = ((Button)sender).DataContext as BO.BusOnTrip;
            AddTrip upTrip = new AddTrip(false, true, Index, b);
            if (upTrip.ShowDialog() == true)
            {
                bl.UpdateBusOnTrip(b);
                int i = trips.IndexOf(b);
                trips.Remove(b);
                trips.Insert(i, b);
                busTrips.ItemsSource = trips;
            }
        }
    }
}
