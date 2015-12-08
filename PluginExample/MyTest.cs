using System;
using System.Collections.Generic;
using BoxedIce.ServerDensity.Agent.PluginSupport;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace PluginExample
{
    public class MyTest : ICheck
    {
        #region ICheck Members

        public string Key
        {
            get { return "ML_status"; }
        }

        public object DoCheck()
        {
            TcpClient client = new TcpClient("localhost", 10102);

            Stream s = client.GetStream();
            StreamReader sr = new StreamReader(s);
            StreamWriter sw = new StreamWriter(s);
            sw.AutoFlush = true;
            sw.WriteLine("status");
            sr.ReadLine();
            string output = sr.ReadLine();

            Regex regex = new Regex(@"blackbox status=""ok""");
            Match match = regex.Match(output);

            IDictionary<string, object> values = new Dictionary<string, object>();

            if (match.Success)
            {
                values.Add("status", 1);
            }
            else
            {
                values.Add("status", 0);
            }

            s.Close();
            client.Close();

            return values;
        }

        #endregion
    }
}