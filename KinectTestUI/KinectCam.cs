//Angelo San (BitSan) june 12, 1017
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace KinectTestUI
{
    public class PictureReadyEventArgs : EventArgs
    {
        public Bitmap Picture { private set; get; }

        internal PictureReadyEventArgs(Bitmap PictureIn) : base()
        {
            Picture = PictureIn;
        }
    }

    public delegate void PicturReadyEvent(object sender, PictureReadyEventArgs e);

    public class KinectCam : IDisposable
    {
        public int MaxTilt { private set; get; }
        public int MinTilt { private set; get; }

        public int KinectNumber { private set; get; }

        KinectSensor Kinect = null;
        public KinectStatus Status { get { return Kinect.Status; } }

        public bool IsRunning { get { return Kinect.IsRunning; } }

        public int Tilt { get { return Kinect.ElevationAngle; } }

        public event PicturReadyEvent PictureReady;

        public KinectCam(int KinectToUse = 0)
        {
            if (KinectSensor.KinectSensors.Count > KinectToUse)
                Kinect = KinectSensor.KinectSensors.First();
            else
                throw new Exception("This Kinect is not connected.");

            MaxTilt = Kinect.MaxElevationAngle;
            MinTilt = Kinect.MinElevationAngle;

            KinectNumber = KinectToUse;

            Kinect.ColorStream.Enable();

            Kinect.ColorFrameReady += InternalPictureReady;
        }

        public void InternalPictureReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame Frame = e.OpenColorImageFrame())
            {
                if (Frame == null)
                    return;

                byte[] Pixels = new byte[Frame.PixelDataLength];
                Frame.CopyPixelDataTo(Pixels);

                int Stride = Frame.Width * Frame.BytesPerPixel;


                Bitmap BitMap = new Bitmap(Frame.Width, Frame.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                BitmapData BitMapData = BitMap.LockBits(new Rectangle(0, 0, Frame.Width, Frame.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                System.Runtime.InteropServices.Marshal.Copy(Pixels, 0, BitMapData.Scan0, Frame.PixelDataLength);

                BitMap.UnlockBits(BitMapData);

                PictureReady(this, new PictureReadyEventArgs(BitMap));
            }
        }

        public bool SetTilt(int SetTilt)
        {
            if (SetTilt < MaxTilt && SetTilt > MinTilt)
            {
                Kinect.ElevationAngle = SetTilt;
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            if(Kinect.IsRunning)
                Kinect.Start();
        }

        public void StartCam()
        {
            Kinect.Start();
        }

        public void StopCam()
        {
            Kinect.Stop();
        }

        public bool ToggleCam()
        {
            if (Kinect.IsRunning)
            {
                Kinect.Stop();
                return Kinect.IsRunning;
            }
            else
            {
                Kinect.Start();
                return Kinect.IsRunning;
            }
        }

    }
}
