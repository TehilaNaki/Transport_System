using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BO;
using BLAPI;
using PLGUI;


/// <summary>
/// A Class that represents the current station where the bus is in real time
/// </summary>
namespace PO
{
    public class CurrentStation : BO.LineTiming, INotifyPropertyChanged
    {
        public static ObservableCollection<PO.CurrentStation> LineTimings = new ObservableCollection<CurrentStation>();
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SimulationOfTrip simulation { get; set; }

        //update station
        private int upStation;
        public int UpStation
        {
            get => upStation;
            set
            {
                upStation = value;
                OnPropertyChanged(nameof(upStation));
            }
        }

        //update station in line
        private IEnumerable<BO.LineStation> uplineStations;
        public IEnumerable<BO.LineStation> UplineStations
        {
            get => uplineStations;
            set
            {
                uplineStations = value;
                OnPropertyChanged(nameof(uplineStations));
            }
        }

    }
}
