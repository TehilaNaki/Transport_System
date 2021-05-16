using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Help tools for the data base layer
    /// </summary>
    public static class Tools
    {
        static Random R=new Random();
        public static string ToStringProperty<T>(this T t)
        {
            string str = "";
            foreach (PropertyInfo item in typeof(T).GetProperties())
                str += "\n" + item.Name + ": " + item.GetValue(t, null);
            return str;
        }

    }
}
