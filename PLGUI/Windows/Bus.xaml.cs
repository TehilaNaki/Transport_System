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

namespace PLGUI
{
    /// <summary>
    /// Interaction logic for Bus.xaml
    /// </summary>
    public partial class Bus : Window
    {
        public PO.Bus MyBus;
        int Index;
        IBL bl = BLFactory.GetBL();
        public Bus(PO.Bus b, int index = 0)
        {
            InitializeComponent();
            Index = index;
            DataContext = MyBus = b;
        }

        /// <summary>
        /// an event to show the trips
        /// </summary>
        private void ShowTrips_Click(object sender, RoutedEventArgs e)
        {
            PO.Bus b = ((Button)sender).DataContext as PO.Bus;
            BusOnTrip busOn = new BusOnTrip(b.LicensNumber, true, Index);
            busOn.ShowDialog();
        }


    }
}
