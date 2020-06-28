using System;
using System.Diagnostics;
using System.IO;


namespace OSLAB4
{
    class Program
    {
        public static void Main(string[] args)
        {
            var path = @"C:\Users\Dede\Desktop\";
            Process[] processes = Process.GetProcesses();
            StreamWriter writer = File.CreateText(path+"CurrentProcessList.txt");
            string date = DateTime.Now.ToString("yyyy-MM-dd HH-mm");
            StreamWriter writeCSV = File.CreateText(path + "TotalMemoryUsage" +date+ ".csv");

            

            foreach (Process theprocess in processes)
            {
                string processStr = theprocess.ProcessName.ToString();
                int processIdStr = theprocess.Id;
                string pilnasPav = processIdStr + processStr;
                long ramUsage = (theprocess.PeakPagedMemorySize64);
                if(theprocess.ProcessName == "Idle") { continue; }
                writer.WriteLine(" ID: {1}  Process: {0} ", processStr, processIdStr);
            }

            Console.WriteLine("Procesai irasyti i faila...");
            Console.WriteLine("Skaiciuojami Procesoriu uzemantys procesai...");

            for (int i = 0; i <= 5; i++)
            {
                if (processes[i].ProcessName == "Idle") { continue; }
                for (int j = 0; i <= processes.Length; j++)
                {
                    if (processes[j].ProcessName == "Idle") { break; }
                    if (processes[i].TotalProcessorTime < processes[j].TotalProcessorTime)
                    {
                        Process temp = processes[i];
                        processes[i] = processes[j];
                        processes[j] = temp;
                    }
                }
                Console.WriteLine("Procesas irasomas i CSV faila...");
                writeCSV.WriteLine(" ID: {1}  Process: {0}  Proc: {2}", processes[i].ProcessName, processes[i].Id,
                    processes[i].TotalProcessorTime);
            }
            Console.WriteLine("Irasymas baigtas...");

            writer.Close();
            writeCSV.Close();

            Process[] notepad = Process.GetProcessesByName("notepad");
            
            if(notepad.Length == 0)
            {
                Console.WriteLine("Notepad neveikia");
            }
            else { 
                Console.WriteLine("ID: {0}, RAM usage: {1} MB,"
                       , notepad[0].Id, notepad[0].WorkingSet64 / 1000000);
            }
            Console.WriteLine( "bye bye"  );
        }
    }
}
