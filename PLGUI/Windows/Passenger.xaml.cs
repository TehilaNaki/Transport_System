using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BLAPI;
using System.Collections.ObjectModel;

namespace PLGUI
{
    /// <summary>
    /// Interaction logic for Passenger.xaml
    /// </summary>
    public partial class Passenger : Window
    {
        public BO.User MyUser { get; set; }
        IBL bl = BLFactory.GetBL();
        public ObservableCollection<PO.ViewTimes> times=new ObservableCollection<PO.ViewTimes>();
        public TimeSpan second = new TimeSpan(0, 0, 0, 1);
        Window last = null;
        DispatcherTimer timer;
        public Passenger(BO.User user,Window w=default)
        {
            InitializeComponent();
            MyUser = user;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            cmbStations.ItemsSource = from BO.Station s in bl.GetAllStations()
                                      select new BO.Stat(s.Code, s.Name);
            lUser.DataContext = MyUser;
            lDate.Content = DateTime.Now.ToString("dd/MM/yy");
            cmbStations.SelectedIndex = 0;
            last = w;
        }
        /// <summary>
        /// an event that shows the timer
        /// </summary>
 
        private void timer_Tick(object sender, EventArgs e)
       {
            lblTime.Content = DateTime.Now.ToLongTimeString();

            for (int i=0; i<times.Count(); i++)
            {
                if (times[i].LastTime == TimeSpan.Zero)
                    times.Remove(times[i]);
                else times[i].LastTime -= second;
            }

            if (DateTime.Now.Second % 5 == 0)
            {
                ObservableCollection<PO.ViewTimes> listO = new ObservableCollection<PO.ViewTimes>(
                     from item in PO.ViewTimes.times.TimesOfStation((int)(cmbStations.SelectedValue))
                        where times.ToList().Find(x => x.UpLineID == item.UpLineID) == null
                        select item
                 );
                foreach(var item in listO)
                {
                    times.Add(item);
                }
            }
            ObservableCollection<PO.ViewTimes> listObserv = new ObservableCollection<PO.ViewTimes>(times);
            TimesList.ItemsSource = listObserv;
        }

        private void cmbStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<BO.Line> listObserv;
            StationNumber.Content = cmbStations.SelectedValue.ToString();
            IEnumerable<BO.Line> list = bl.GetLinePassInStation((int)(cmbStations.SelectedValue));
            listObserv =new ObservableCollection<BO.Line>(list);
            LinesData.ItemsSource= listObserv;
            times=PO.ViewTimes.times.TimesOfStation((int)(cmbStations.SelectedValue));
            TimesList.ItemsSource = times;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Login enter = new Login();
                enter.Show();
                if(MyUser.Admin==BO.Permission.Managment)
                last.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
            foreach (var c in PO.CurrentStation.LineTimings)
            {
                if (c.simulation != null)
                {
                    c.simulation.StopSimulation();
                }
            }
            if(last!=null&& last.IsActive)
            {
                ((MainWindow)last).PassengerOpen = false;
            }
            if(last!=null&& (!last.IsActive))
            {
                foreach (var c in PO.CurrentStation.LineTimings)
                {
                    if (c.simulation != null)
                    {
                        c.simulation.StopSimulation();
                    }
                }
            }
            
        }
    }
}
