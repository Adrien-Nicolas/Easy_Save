using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace EasySaveV2.Others
{
    public sealed class SingleInstance
    {
        public static bool AlreadyRunning()
        {
            bool running = false;
            try
            {
                // Getting collection of process  
                Process currentProcess = Process.GetCurrentProcess();

                // Check with other process already running   
                foreach (var p in Process.GetProcesses())
                {
                    if (p.Id != currentProcess.Id) // Check running process   
                    {
                        if (p.ProcessName.Equals(currentProcess.ProcessName) == true)
                        {
                            running = true;
                            MessageBox.Show("Application déja lancée");
                            break;
                        }
                    }
                }
            }
            catch { }
            return running;
        }


    }
}
