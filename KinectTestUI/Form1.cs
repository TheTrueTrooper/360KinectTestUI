#region WritersSigniture
//Writer: Angelo Sanches (BitSan)(Git:TheTrueTrooper)
//Date Writen: June 23,2017
//Project Goal: To make the Kinect 360 cam useful for the computer so I can get more use out of it
//File Goal: To Make a test UI for the Projects stuff
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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kinect.ColourCam;
using System.Drawing.Imaging;

// to be commented
namespace KinectTestUI
{
    /// <summary>
    /// The just Test UI
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// 
        /// </summary>
        const int TiltCoolDown = 1200;
        /// <summary>
        /// The cams tilt needs a cool down
        /// </summary>
        Stopwatch TiltCoolDownTimer = new Stopwatch();

        /// <summary>
        /// the Cam interface 
        /// </summary>
        KinectCam Cam;

        /// <summary>
        /// If we pressed a tilt button
        /// </summary>
        bool TiltUp;
        /// <summary>
        /// If we pressed a tilt button
        /// </summary>
        bool TiltDown;
        /// <summary>
        /// was the tilt successfully set
        /// </summary>
        bool TiltSet = true;
        /// <summary>
        /// Tilt
        /// </summary>
        int Tilt;

        /// <summary>
        /// creates the form and componets with a new cam
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            _NuUD_Cam.Maximum = KinectCam.KinectCamCount - 1;
            _NuUD_Cam.Value = 0;
            Cam = new KinectCam();
            Cam.PictureReady += PictureReady;
            Cam.KinectSensorsChanged += KinectSensorsChanged;
            timer1.Enabled = true;
        }

        /// <summary>
        /// if a Kinect is added Modify the forms conect sensor selector as needed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KinectSensorsChanged(object sender, KinectSensorsChangedEventArgs e)
        {
            _NuUD_Cam.Value = e.ThisCamsNewNumber;
            _NuUD_Cam.Maximum = e.NewMaxCamCount - 1;
        }

        /// <summary>
        /// sets the Tilt up to true to allow tilt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Bu_Up_Click(object sender, EventArgs e)
        {
            TiltUp = true;
        }

        /// <summary>
        /// sets the Tilt up to true to allow tilt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Bu_Down_Click(object sender, EventArgs e)
        {
            TiltDown = true;
        }

        /// <summary>
        /// If we select a different cammera build and set it up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _NuUD_Cam_ValueChanged(object sender, EventArgs e)
        {
            // Remove this subscription to the Cammera this minimizes side effects
            Cam.PictureReady -= PictureReady;
            Cam.KinectSensorsChanged -= KinectSensorsChanged;
            // build a completely new cam
            Cam = new KinectCam(Convert.ToInt32(_NuUD_Cam.Value));
            Cam.PictureReady += PictureReady;
            Cam.KinectSensorsChanged += KinectSensorsChanged;
        }

        /// <summary>
        /// Tick is used to control the cam in a non-blocking way
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            #region MotorControlAndCooldown
            //so we first see if we want to tilt and if not on cool down and not currently tilting we then set to tilt
            if ((TiltUp || TiltDown) && !TiltCoolDownTimer.IsRunning && TiltSet)
            {
                Tilt = Cam.Tilt;
                Thread.Sleep(20);

                //I'm not sure why but it can't detect a difference of one very good especially with down play
                if (TiltUp)
                {
                    Tilt+=2;
                    TiltUp = false;
                }
                if (TiltDown)
                {
                    Tilt-=2;
                    TiltDown = false;
                }

                TiltSet = false;
            }

            //At this point if we are trying to tilt start trying 
            if (!TiltSet)
            {
                //if tilt is successful use value for tilt and start the cool down (if tilt was unsuccessful this means we will keep trying)
                TiltSet = Cam.SetTilt(Tilt);
                if(TiltSet)
                    TiltCoolDownTimer.Start();
            }

            //on this stage check the cool down for motor
            if (TiltCoolDownTimer.IsRunning && TiltCoolDownTimer.ElapsedMilliseconds > TiltCoolDown)
            {
                TiltCoolDownTimer.Restart();
                TiltCoolDownTimer.Stop();
            }
            #endregion
        }

        /// <summary>
        /// Stops the cam if it is running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cam.StopCam();
        }

        /// <summary>
        /// If there is a picture to grab Gab and use it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureReady(object sender, PictureReadyEventArgs e)
        {
            if (e.Picture != null)
            {
                _PicBo_CamDisplay.Image = e.Picture;
            }
        }

        /// <summary>
        /// Starts or stops the cam with enable/disable save button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Bu_Start_Click(object sender, EventArgs e)
        {
            if(Cam.ToggleCam())
            {
                _Bu_StartStop.Text = "Stop";
                _Bu_CaptureImage.Enabled = true;
            }
            else
            {
                _Bu_StartStop.Text = "Start";
                _Bu_CaptureImage.Enabled = false;
            }
        }

        /// <summary>
        /// captures an image to save from the camera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Bu_CaptureImage_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog Location = new SaveFileDialog())
            {
                Location.AddExtension = true;
                Location.DefaultExt = ".png";
                if (Location.ShowDialog() == DialogResult.OK)
                {
                    _PicBo_CamDisplay.Image.Save( Location.FileName, ImageFormat.Png);
                }
            }
        }
    }
}
