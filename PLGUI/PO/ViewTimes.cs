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
///A Class that represents the current station where the bus is in real time 
/// </summary>
namespace PO
{
  public class ViewTimes
    {
        IBL bl = BLFactory.GetBL();
        public static PO.ViewTimes times=new ViewTimes();
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int upLine;
        public int UpLine
        {
            get => upLine;
            set
            {
                upLine = value;
                OnPropertyChanged(nameof(upLine));
            }
        }
        private int upLineID;
        public int UpLineID
        {
            get => upLineID;
            set
            {
                upLine = value;
                OnPropertyChanged(nameof(upLineID));
            }
        }
        private string upDest;
        public string UpDest
        {
            get => upDest;
            set
            {
                upDest = value;
                OnPropertyChanged(nameof(upDest));
            }
        }
        private TimeSpan lastTime;
        public TimeSpan LastTime
        {
            get => lastTime;
            set
            {
                lastTime = value;
                OnPropertyChanged(nameof(lastTime));
            }
        }

        //function that return the times of lines that need to arrive the station
        public ObservableCollection<PO.ViewTimes> TimesOfStation(int stationKey)
        {
            var ListIenum = (from item in PO.CurrentStation.LineTimings
                             let x = bl.GetTimeBetweenStations(item.UpStation, stationKey, item.LineID)
                             where  x.TotalSeconds > 1
                             select new PO.ViewTimes { upLine = item.LineNum, upDest = item.EndStationName, LastTime = x, upLineID=item.LineID});
            ObservableCollection<PO.ViewTimes> listObserv = new ObservableCollection<ViewTimes>(ListIenum);
            return listObserv;
        }
        
    }
}

