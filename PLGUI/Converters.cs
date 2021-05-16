using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace PLGUI
{
    /// <summary>
    /// Help Converters for the GUI
    /// </summary>
    public class LicensPrint : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string LicensNumber = (string)value;
            string help = "";
            if (LicensNumber.Length == 7)//License plate length 7
                                         //index begin and length(substring)
                help = LicensNumber.Substring(0, 2) + "-" + LicensNumber.Substring(2, 3) + "-" + LicensNumber.Substring(5, 2);
            if (LicensNumber.Length == 8)//License plate length 8
                                         //index begin and length(substring)
                help = LicensNumber.Substring(0, 3) + "-" + LicensNumber.Substring(3, 2) + "-" + LicensNumber.Substring(5, 3);
            return help;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string LicensNumber = (string)value;
            string help = "";
            if (LicensNumber.Length == 7)//License plate length 7
                                         //index begin and length(substring)
                help = LicensNumber.Substring(0, 2) + "-" + LicensNumber.Substring(2, 3) + "-" + LicensNumber.Substring(5, 2);
            if (LicensNumber.Length == 8)//License plate length 8
                                         //index begin and length(substring)
                help = LicensNumber.Substring(0, 3) + "-" + LicensNumber.Substring(3, 2) + "-" + LicensNumber.Substring(5, 3);
            return help;
        }
    }
    public class StatusToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((BO.Status)value)
            {
                case BO.Status.Ready:
                    return "/images/ok_30px.png";
                case BO.Status.NeedRefuel:
                    return "/images/gas_station_100px.png";

                case BO.Status.NeedTreatment:
                    return "/images/brake_warning_50px.png";

                case BO.Status.DurringDrive:
                    return "/images/bus_30px.png";

                case BO.Status.Refueling:
                    return "/images/gas_pump_50px.png";

                case BO.Status.InTreatment:
                    return "/images/intreatment_24px.png";
            }
            return Enum.GetName(typeof(BO.Status), value) + ".png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DateMonthYear : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString("MM/yyyy");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string v = (string)value;
            //if (int.Parse(v.Substring(0, 2)) > 12 || int.Parse(v.Substring(2, 4)) > DateTime.Now.Year || int.Parse(v.Substring(2, 4)) < (DateTime.Now.Year - 30))
            //    return "         ";
            v = v.Substring(0, 2) + "/" + v.Substring(2, 4);
            return v;
        }
    }
    public class YesNoConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {


            if ((bool)value == true)
            {
                return "/images/Yes_24px.png";
            }
            else return "/images/No_32px.png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsBusyStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Status s = ((BO.Status)value);
            if (s != BO.Status.DurringDrive && s != BO.Status.InTreatment && s != BO.Status.Refueling)
                return Visibility.Hidden;
            return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class Width : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Status s = ((BO.Status)value);
            if (s != BO.Status.DurringDrive && s != BO.Status.InTreatment && s != BO.Status.Refueling)
                return 0;
            return 500;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ThousandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string last = value.ToString();
            int length = last.Length;
            string newS = length % 3 > 0 ? last.Substring(0, length % 3) + "," : " ";
            for (int i = 0; i < (length / 3); i++)
            {
                newS += last.Substring(length % 3 + 3 * i, 3) + ",";
            }
            newS = newS.Substring(0, newS.Length - 1);
            return newS;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string last = value.ToString();
            int length = last.Length;
            string newS = length % 3 > 0 ? last.Substring(0, length % 3) + "," : " ";
            for (int i = 0; i < (length / 3); i++)
            {
                newS += last.Substring(length % 3 + 3 * i, 3) + ",";
            }
            newS = newS.Substring(0, newS.Length - 1);
            return newS;
        }
    }
    public class BoolToVisiblityConverter : IValueConverter
    {
     
        public bool Collapse { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool bValue = (bool)value;
            if (bValue)
                return Visibility.Visible;
            else
            {
                if (Collapse)
                    return Visibility.Collapsed;
                else
                    return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
                return true;
            else
                return false;
        }

    }
}


