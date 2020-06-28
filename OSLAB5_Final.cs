using System;
using System.Diagnostics;
using System.Management;
using System.Collections.Generic;
using System.Linq;

namespace OSLAB5_Final
{

    public class Functions
    {

        public void ComputerStatistics()
        {
            try
            {
                Console.WriteLine("Computer name: " + ComputerName());
                Console.WriteLine("CPU Usage: " + CpuUsage());
                Console.WriteLine("Disk Stats: " + DiskStats());
                IDictionary<string, Int64> ramData = MemmoryStats();
                Console.WriteLine("Total physical memmory: " + ramData["Total physical memmory"] + " MB");
                Console.WriteLine("Memmory usage: " + ramData["Memmory usage"] + " MB");
                Console.WriteLine(Get10AppEvents("Firefox"));
            }
            catch (ManagementException e )
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        private string DiskStats()
        {
            ManagementObjectSearcher dd = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            List<string> stat = new List<string>();
            foreach (ManagementObject current in dd.Get())
            {
                stat.Add(current["Model"] + " Size: " + current["Size"]);
            }
            return string.Join("\n", stat);
        }
        private string CpuUsage()
        {
            ManagementObjectSearcher fullData = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name=\"_Total\"");
            ManagementObjectCollection currentData = fullData.Get();
            ManagementObject queryObj = currentData.Cast<ManagementObject>().First();

            return Convert.ToString(queryObj["PercentIdleTime"]);

        }
        private string ComputerName()
        {
            ManagementObjectSearcher fullData = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT Name FROM Win32_ComputerSystem");
            ManagementObjectCollection currentData = fullData.Get();
            ManagementObject queryObj = currentData.Cast<ManagementObject>().First();
            return Convert.ToString(queryObj["Name"]);
        }
        private IDictionary<string, Int64> MemmoryStats()
        {
            Int64 ramUsage = Process.GetCurrentProcess().WorkingSet64;
            string query = "SELECT Capacity FROM Win32_PhysicalMemory";
            IDictionary<string, Int64> ramData = new Dictionary<string, Int64>();
            ManagementObjectSearcher fullData = new ManagementObjectSearcher(query);

            Int64 cap = 0;
            foreach (ManagementObject currentData in fullData.Get())
            {
                cap += Convert.ToInt64(currentData.Properties["Capacity"].Value);

            }
            cap = cap / (1024 * 1024);
            ramUsage = ramUsage / 1024;
            ramData.Add("Total physical memmory", cap);
            ramData.Add("Memmory usage", ramUsage);
            return ramData;
        }
        private IDictionary<string, EventLog> Las10LogEntries()
        {
            IDictionary<string, EventLog> eventionary = new Dictionary<string, EventLog>();
            EventLog events = new EventLog("Application", System.Environment.MachineName);
            foreach (EventLogEntry entry in events.Entries.Cast<EventLogEntry>().Reverse().Take(10))
            {
                Console.WriteLine(entry.Source);
            }
            Console.WriteLine("Geting logs");
            EventLog[] eventLogLIst;
            eventLogLIst = EventLog.GetEventLogs();
            foreach (EventLog log in eventLogLIst)
            {
                eventionary.Add(log.Log, log);
            }

            Console.WriteLine("Number of logs on computer: " + eventLogLIst.Length);
            int count = 0;
            foreach (EventLog log in eventLogLIst)
            {
                Console.WriteLine("Log: " + log.Log);
                try
                {
                    foreach (EventLogEntry _event in log.Entries)
                    {
                        if (count > 9){break;}
                        Console.WriteLine(count + " " + _event.Message);
                        count++;
                    }
                    count = 0;
                }
                catch
                {
                    Console.WriteLine("Unable to open " + log.Log + " log");
                }
            }
            return eventionary;
        }

        public string Get10AppEvents(string appName)
        {
            EventLog events = new EventLog("Application", Environment.MachineName);
            return string.Join("\n", events.Entries.Cast<EventLogEntry>().Where(pv => pv.Source.Contains(appName)).Take(10).Select(pv => pv.Message));
        }

    }
}
