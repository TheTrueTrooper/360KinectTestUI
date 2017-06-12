//Angelo San (BitSan) june 12, 1017
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

// to be commented
namespace KinectTestUI
{
    public partial class Form1 : Form
    {
        const int TiltCoolDown = 1200;
        Stopwatch TiltCoolDownTimer = new Stopwatch();

        KinectCam Cam;

        bool TiltUp = false;
        bool TiltDown = false;

        public Form1()
        {
            InitializeComponent();
            Cam = new KinectCam();
            Cam.PictureReady += PictureReady;
            timer1.Enabled = true;
        }

        private void _Bu_Up_Click(object sender, EventArgs e)
        {
            TiltUp = true;
        }

        private void _Bu_Down_Click(object sender, EventArgs e)
        {
            TiltDown = true;
        }

        private void _NuUD_Cam_ValueChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((TiltUp || TiltDown) && !TiltCoolDownTimer.IsRunning)
            {
                int Tilt = Cam.Tilt;
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

                Cam.SetTilt(Tilt);

                //to pevent a HReturn indicating an expetion that has to do with clicking to fast we must give the device time to move and re-ready
                TiltCoolDownTimer.Start();
            }

            if(TiltCoolDownTimer.IsRunning && TiltCoolDownTimer.ElapsedMilliseconds > TiltCoolDown)
            {
                TiltCoolDownTimer.Restart();
                TiltCoolDownTimer.Stop();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void PictureReady(object sender, PictureReadyEventArgs e)
        {
            if (e.Picture != null)
            {
                _PicBo_CamDisplay.Image = e.Picture;
            }
        }

        private void _Bu_Start_Click(object sender, EventArgs e)
        {
            if(Cam.ToggleCam())
            {
                _Bu_StartStop.Text = "Stop";
            }
            else
            {
                _Bu_StartStop.Text = "Start";
            }
        }
    }
}
