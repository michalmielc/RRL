using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Timers;

namespace RRL
{
    public class currentlySession
    {
        public  double zeit;

        

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();
        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }
        public static uint GetIdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);

            return ((uint)Environment.TickCount - lastInPut.dwTime);
        }
        /// <summary>
        /// Get the Last input time in milliseconds
        /// </summary>
        /// <returns></returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            if (!GetLastInputInfo(ref lastInPut))
            {
                throw new Exception(GetLastError().ToString());
            }
            return lastInPut.dwTime;
        }

        //   ZAKMNIĘCIE WSZYSTKICH OKIEN Z WYJĄTKIEM LOGOWANIA
        public void close_allForms()


        {

            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)

            {
                if (Application.OpenForms[i].Name != "oknoLogowanie")
                {
                    Application.OpenForms[i].Close();
                    Application.OpenForms[i].Dispose();

                }

            }

         //   oknoLogowanie oknoLogowanie = new oknoLogowanie();
         //   oknoLogowanie.Show();
        }

        public void start(double x)
        {

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed +=  new ElapsedEventHandler(czas);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;
            
        }

        public void czas(object source, ElapsedEventArgs e)
        {

            if (GetIdleTime()>zeit)
            {
                Application.Exit();
                Application.Restart();

            }
          
        
        }



}



}
