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

namespace PLGUI
{
    /// <summary>
    /// Interaction logic for DetailStation.xaml
    /// </summary>
    public partial class StationDetails : Window
    {
        /// <summary>
        /// show the station details onthe window
        /// </summary>

        public StationDetails(BO.Station station)
        {
            InitializeComponent();
            DataContext = station;
        }
    }
}
