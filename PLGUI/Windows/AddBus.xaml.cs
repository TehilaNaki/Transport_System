using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using BL;
using PO;
using BLAPI;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace PLGUI
{
    /// <summary>
    /// Interaction logic for AddBus.xaml
    /// </summary>

    public partial class AddBus : Window
    {
        public PO.Bus MyBus { get; set; }
        public bool Add;

        /// <summary>
        /// a window constractor
        /// </summary>
        /// <param name="isAdd">if is update or add</param>
        /// <param name="bus">if it update -the bus to update</param>
        public AddBus(bool isAdd = false, PO.Bus bus = default)
        {
            InitializeComponent();
            MyBus = bus;
            if (isAdd)
                MyBus = new PO.Bus();
            DataContext = MyBus;
            Add = isAdd;
            if (isAdd == false) licenum.IsReadOnly = true;
        }

        /// <summary>
        /// an event to save the bus we added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (date.DataContext == null || fuel.Text == "" || km.Text == "" || licenum.Text == "")
                MessageBox.Show("Not all ditails are set");
            else
            {
                DialogResult = true;
                if (Add)
                {
                    MessageBox.Show("The bus added succsesfully!");
                }
                else
                {
                    MessageBox.Show("The bus update succsesfully!");
                }
                Close();

            }
        }
        /// <summary>
        /// text box that allows only numbers to be entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }


    }
}
