    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Represents a Station in the line
    /// </summary>
    public class LineStation : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Station Station { get; set; }
        public int ID { get; set; }

        int indexInLine;
        public int IndexInLine { get => indexInLine; set { indexInLine = value; OnPropertyChanged(nameof(indexInLine)); } }
        public int LineId { get; set; }

        double distance;
        public double Distance { get => distance; set { distance = value; OnPropertyChanged(nameof(distance)); } }

        TimeSpan drivingTime;
        public TimeSpan DrivingTime { get => drivingTime; set { drivingTime = value; OnPropertyChanged(nameof(drivingTime)); } }

    }
}
