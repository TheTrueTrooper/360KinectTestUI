#region WritersSigniture
//Writer: Angelo Sanches (BitSan)(Git:TheTrueTrooper)
//Date Writen: June 23,2017
//Project Goal: To make the Kinect 360 cam useful for the computer so I can get more use out of it
//File Goal: The Generated.
//Link: https://github.com/TheTrueTrooper/360KinectTestUI
//Sources: 
//  {
//  Name: Kinect for Windows SDK v1.8 
//  Writer/Publisher: Microsoft
//  Link: https://www.microsoft.com/en-ca/download/details.aspx?id=40278
//  }
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KinectTestUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
