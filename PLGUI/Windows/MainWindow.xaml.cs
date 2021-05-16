
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BLAPI;
using PO;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PLGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        IBL bl = BLFactory.GetBL();
        public BO.User MyUser { get; set; }
        /// <summary>
        /// collection of stations
        /// </summary>
        ObservableCollection<BO.Station> listStations;
        /// <summary>
        /// collection of busses
        /// </summary>
        ObservableCollection<PO.Bus> listBuses;
        /// <summary>
        /// collection of lines
        /// </summary>
        ObservableCollection<BO.Line> lineList;
       public bool PassengerOpen { get; set; }
       
        public MainWindow(BO.User user)
        {

            InitializeComponent();
            //reset list of ststions
            listStations = new ObservableCollection<BO.Station>(bl.GetAllStations());
            //reset the list of the buses   
            listBuses = new ObservableCollection<PO.Bus>((from b in bl.GetAllBus()
                                                         select new PO.Bus(b)));
            //reset the lines list
            lineList = new ObservableCollection<BO.Line>(bl.GetAllLines());

            MyUser = user;
            userGrid.DataContext = MyUser;

            //check the status of the buses
            foreach (var b in listBuses) b.Status(1);

            lstStations.ItemsSource = listStations;
            LVlistBuses.ItemsSource = listBuses;
            viewBusLines.ItemsSource = lineList;

            //show the lines
            cmbLines.ItemsSource = lineList;
            cmbLines.DisplayMemberPath = "LineCode";
        }
      
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Controls.TextBox text = sender as TextBox;

            if (e.Key == Key.D0 || e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4 || e.Key == Key.D5 || e.Key == Key.D6 || e.Key == Key.D7 || e.Key == Key.D8 || e.Key == Key.D9 || e.Key == Key.Enter || e.Key == Key.Delete || e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
            e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
            || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;
            e.Handled = true;
        }

        #region Station
        /// <summary>
        /// an event to show the details station window
        /// </summary>

        private void detailStation_Click(object sender, MouseButtonEventArgs e)
        {
            BO.Station station = bl.GetStationWithAdjacents((((FrameworkElement)e.OriginalSource).DataContext as BO.Station).Code);
            StationDetails detailStation = new StationDetails(station);
            detailStation.Show();
        }
        /// <summary>
        /// an event to show the add station window
        /// </summary>

        private void addStation_Click(object sender, RoutedEventArgs e)
        {
            AddUpdateStation newStation = new AddUpdateStation();
            if (newStation.ShowDialog() == true)//if the user could click on ok
            {
                bl.AddStation(newStation.MyStation);
                listStations.Insert(0, newStation.MyStation);
            }

        }
     
       
        /// <summary>
        /// an event to show the update station window
        /// </summary>

        private void update_Click(object sender, RoutedEventArgs e)
        {
            BO.Station s = ((Button)sender).DataContext as BO.Station;
            AddUpdateStation newStation = new AddUpdateStation(s.Code);
         
            if (newStation.ShowDialog() == true)//if the user fill all the ditals
            {
                bl.UpdateStation(newStation.MyStation);
                int i = listStations.IndexOf(s);
                listStations.Remove(s);
                listStations.Insert(i, newStation.MyStation);
                
            }
        }
        #endregion

        #region Line
        /// <summary>
        /// an event to show the details line window
        /// </summary>

        private void linesDetails_Click(object sender, MouseButtonEventArgs e)
        {
            LinesDetails detailLine = new LinesDetails(((ListView)sender).SelectedItem as BO.Line, false, false);
            detailLine.ShowDialog();
        }
        /// <summary>
        /// an event to remove the current line window
        /// </summary>

        private void removeOneLine_Click(object sender, RoutedEventArgs e)
        {
            BO.Line line = ((Button)sender).DataContext as BO.Line;
            bl.DeleteLine(line.ID);
            lineList.Remove(line);
            MessageBox.Show("The bus line: " + line.LineCode + " deleted succsesfully!", "Done succusesfully", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    
        /// <summary>
        /// an event to show the add line window
        /// </summary>

        private void AddLine_Click(object sender, RoutedEventArgs e)
        {
            LinesDetails newLine = new LinesDetails(null, true, false);
            if (newLine.ShowDialog() == true)//if the user could click on ok
            {
                bl.AddLine(newLine.MyLine);
                lineList.Insert(0, newLine.MyLine);
            }
        }
        /// <summary>
        /// an event to show the update line window
        /// </summary>

        private void updateBusLine_Click(object sender, RoutedEventArgs e)
        {
            BO.Line l = ((Button)sender).DataContext as BO.Line;
            LinesDetails linesDetails = new LinesDetails(l, false, true);
            if (linesDetails.ShowDialog() == true)//if the user fill all the ditals
            {
                bl.UpdateLine(l);
                int i = lineList.IndexOf(l);
                lineList.Remove(l);
                lineList.Insert(i, l);
            }
        }
        #endregion

        #region Bus
        /// <summary>
        /// an event to delete the current bus
        /// </summary>

        public void DeletBus_Click(object sender, RoutedEventArgs e)
        {
            BO.Bus b = ((Button)sender).DataContext as BO.Bus;
            bl.DeleteBus(b.LicensNumber);
            listBuses.Remove((PO.Bus)b);
            MessageBox.Show("The bus: " + b.LicensNumber + " deleted succsesfully!", "Done succusesfully", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// an event to show the update bus window
        /// </summary>

        public void UpDateBus_Click(object sender, RoutedEventArgs e)
        {
            PO.Bus b = ((Button)sender).DataContext as PO.Bus;
            AddBus upBus = new AddBus(false, b);
            if (upBus.ShowDialog() == true)
            {

                bl.UpdateBusDetails(b);
                b.Status(1);
                int i = listBuses.IndexOf((PO.Bus)b);
                listBuses.Remove((PO.Bus)b);
                listBuses.Insert(i, (PO.Bus)b);
            }
        }
        /// <summary>
        /// an event to show the add line window
        /// </summary>

        public void AddBus_Click(object sender, RoutedEventArgs e)
        {
            AddBus addBus = new AddBus(true);
            if (addBus.ShowDialog() == true)
            {
                addBus.MyBus.Status(1);
                listBuses.Add(addBus.MyBus);
                bl.AddBus(addBus.MyBus);
            }

        }
        /// <summary>
        /// an event to remove all the busses
        /// </summary>

        public void RemoveAllBus_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete all buses? :(", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                listBuses.Clear();
                bl.DeleteAllBuses();
                MessageBox.Show("All the buses deleted succsesfully :)");
            }

        }
        /// <summary>
        /// an event to start a traveling the bus
        /// </summary>

        public void BusDrive_Click(object sender, RoutedEventArgs e)
        {
            PO.Bus MyBus = ((Button)sender).DataContext as PO.Bus;
            Button btn = (Button)sender;
            TextBox tb = ((StackPanel)btn.Parent).FindName("drive") as TextBox;
            int time=0;
            double distance = 0;
            if (MyBus.Upstatus == BO.Status.Ready)
            {
                try
                {

                    if (cmbLines.SelectedItem == null)
                        MessageBox.Show("Please choose a line for drive");
                    else
                    {
                        var listRouth = (cmbLines.SelectedItem as BO.Line).StationsRoute.ToList();
                        if (listRouth != null && listRouth.Count() > 0)
                        {
                            distance = bl.GetDistanceBetweenStations(listRouth[0].Station.Code, listRouth[listRouth.Count - 1].Station.Code, (cmbLines.SelectedItem as BO.Line).ID);
                            time = (int)(bl.GetTimeBetweenStations(listRouth[0].Station.Code, listRouth[listRouth.Count - 1].Station.Code, (cmbLines.SelectedItem as BO.Line).ID).TotalSeconds);

                            if (MyBus.Upstatus == BO.Status.Ready && MyBus.Status((int)distance / 1000))
                            {
                                if (MyBus.simulation == null)
                                    MyBus.simulation = new SimulationOfBus(MyBus);

                                MyBus.simulation.Driving((int)(time * 0.98));
                                int index = listBuses.IndexOf(MyBus);
                                listBuses.Remove(MyBus);
                                listBuses.Insert(index, MyBus);
                                bl.Drive(MyBus, (int)distance / 1000);
                                bl.UpdateBusDetails(MyBus);
                                BO.BusOnTrip busOnTrip = new BO.BusOnTrip { LicensNumber = MyBus.LicensNumber, ActualTakeOff = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), LineNumber = (cmbLines.SelectedItem as BO.Line).LineCode, LineId = (cmbLines.SelectedItem as BO.Line).ID, ID = bl.GetBusOnId(), PrevStation = (cmbLines.SelectedItem as BO.Line).StationsRoute.ToList()[0].Station.Code, DriverId=MyUser.UserName };
                                bl.AddBusOnTrip(busOnTrip);
                                GoToTrip(busOnTrip, (cmbLines.SelectedItem as BO.Line));
                            }
                            else MessageBox.Show("The Bus is not ready to drive");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show("The Bus is not ready to drive");

        }
        /// <summary>
        /// an event to get the bus on a trip
        /// </summary>

        public void GoToTrip(BO.BusOnTrip b, BO.Line l)
        {
            PO.CurrentStation current = new PO.CurrentStation { TripStartTime = b.ActualTakeOff, EndStationName = l.StationsRoute.ToList().Last().Station.Address, UplineStations = l.StationsRoute, LineID= l.ID , LineNum=l.LineCode};
            PO.CurrentStation.LineTimings.Add(current);
            if(current.simulation==null)
            current.simulation = new SimulationOfTrip(current);
            current.simulation.Starting();
        }
        /// <summary>
        /// an event to start refueling the bus
        /// </summary>

        private void BusRefuel_Click(object sender, RoutedEventArgs e)
        {
            PO.Bus MyBus = ((Button)sender).DataContext as PO.Bus;
            if (MyBus.Upstatus != BO.Status.DurringDrive && MyBus.Upstatus != BO.Status.InTreatment && MyBus.Upstatus != BO.Status.Refueling)
            {
                if (MyBus.simulation == null)
                    MyBus.simulation = new SimulationOfBus(MyBus);
                MyBus.simulation.Refuling();
                bl.UpdateBusDetails(MyBus);
                int i = listBuses.IndexOf(MyBus);
                listBuses.Remove(MyBus);
                listBuses.Insert(i, MyBus);
            }
            else MessageBox.Show("The bus is busy :(", "wait!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <summary>
        /// an event to start treating the bus
        /// </summary>
  
        private void BusTreat_Click(object sender, RoutedEventArgs e)
        {
            PO.Bus MyBus = (PO.Bus)(((Button)sender).DataContext);
            if (MyBus.Upstatus != BO.Status.DurringDrive && MyBus.Upstatus != BO.Status.InTreatment && MyBus.Upstatus != BO.Status.Refueling)
            {
                bl.Treatment(MyBus);
                if (MyBus.simulation == null)
                    MyBus.simulation = new SimulationOfBus(MyBus);

                MyBus.simulation.Treating();
                bl.UpdateBusDetails(MyBus);
                int i = listBuses.IndexOf(MyBus);
                listBuses.Remove(MyBus);
                listBuses.Insert(i, MyBus);
            }
            else MessageBox.Show("The bus is busy :(", "wait!", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        /// <summary>
        /// an event to show the line details window
        /// </summary>

        private void LVlistBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bus busData = new Bus(((ListView)sender).SelectedItem as PO.Bus, listBuses.IndexOf(((ListView)sender).SelectedItem as PO.Bus));
            busData.Show();
        }

        #endregion

        #region User 
        /// <summary>
        /// an event show the login window
        /// </summary>

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Login enter = new Login();
                enter.Show();
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// an event to show the passenger window
        /// </summary>

        private void ChangePassenger_Click(object sender, RoutedEventArgs e)
        {
            PassengerOpen = true;
            Passenger passenger = new Passenger(MyUser, this);
            passenger.Show();
        }

        #endregion
        

        }
}
