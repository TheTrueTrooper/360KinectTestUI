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
using System.Diagnostics;

namespace Kinect.ColourCam
{
    /// <summary>
    /// this class is for the event that returns a picture
    /// </summary>
    public class PictureReadyEventArgs : EventArgs
    { 
        /// <summary>
        /// the BitMap Returned to our interface, programme, or UI
        /// </summary>
        public Bitmap Picture { private set; get; }

        /// <summary>
        /// The Event Constructor
        /// </summary>
        /// <param name="PictureIn">The BitMap to return</param>
        internal PictureReadyEventArgs(Bitmap PictureIn) : base()
        {
            Picture = PictureIn;
        }
    }

    /// <summary>
    /// A class used int rasing the  KinectSensorsChanged
    /// </summary>
    public class KinectSensorsChangedEventArgs : EventArgs
    {

        /// <summary>
        /// The Cams New Number
        /// </summary>
        public int ThisCamsNewNumber { private set; get; }

        /// <summary>
        /// The Connected Kinects new Count
        /// </summary>
        public int NewMaxCamCount { private set; get; }

        /// <summary>
        /// EventConstructor
        /// </summary>
        /// <param name="ThisCamsNewNumberIn">The New Number For This Cam</param>
        /// <param name="NewMaxCamIn">The new Max Sensors</param>
        internal KinectSensorsChangedEventArgs(int ThisCamsNewNumberIn, int NewMaxCamCountIn) : base()
        {
            ThisCamsNewNumber = ThisCamsNewNumberIn;
            NewMaxCamCount = NewMaxCamCountIn;
        }
    }

    /// <summary>
    /// this is a delegate type for the event to return to
    /// </summary>
    /// <param name="sender">the Cam that rased event</param>
    /// <param name="e">the Event object with useful data (The bitmap in this case see event arg type)</param>
    public delegate void PicturReadyEvent(object sender, PictureReadyEventArgs e);

    /// <summary>
    /// this is a delegate type for the event to return to
    /// </summary>
    /// <param name="sender">the Cam that rased event</param>
    /// <param name="e">the Event object with useful data (The new Connected Kinect Count And this Connects new Space)</param>
    public delegate void KinectSensorsChangedEvent(object sender, KinectSensorsChangedEventArgs e);

    /// <summary>
    /// the camera UI to interface with the kinect
    /// </summary>
    public class KinectCam : IDisposable
    {
        /// <summary>
        /// The Max Cool down for the Cams tilt
        /// </summary>
        const int TiltCoolDown = 1200;

        /// <summary>
        /// A quick ref to all the Cams in case you need it and dont want to Ref the original lib
        /// </summary>
        public static KinectSensorCollection KinectCams { get { return KinectSensor.KinectSensors; }}

        /// <summary>
        /// A quick ref to all the Cams in case you need it and dont want to Ref the original lib
        /// </summary>
        public static int KinectCamCount { get { return KinectSensor.KinectSensors.Count(); } }

        /// <summary>
        /// The Cameras maxi Tilt after single retrieval 
        /// (I wasnt sure if it was a const or depended on the model so i grab it once from the device to keep the com open)
        /// </summary>
        public int MaxTilt { private set; get; }
        /// <summary>
        /// The Cameras mini Tilt after single retrieval 
        /// (I wasnt sure if it was a const or depended on the model so i grab it once from the device to keep the com open)
        /// </summary>
        public int MinTilt { private set; get; }

        /// <summary>
        /// The Kinects conection number for ref. Is direct from the machine so Use wisely Ie Spam creates a HRefReuslt that exeptions out
        /// </summary>
        public string KinectNumberID { get { return Kinect.UniqueKinectId; } }

        /// <summary>
        /// Gets our number in the list of connects so we can compare retrieval
        /// </summary>
        public int KinectSensorNumber { get { return KinectSensor.KinectSensors.IndexOf(Kinect); } }

        /// <summary>
        /// the Kinect we are using
        /// </summary>
        KinectSensor Kinect;

        /// <summary>
        /// Returns the Kinect Dev Status. Is direct from the machine so Use wisely Ie Spam creates a HRefReuslt that exeptions out
        /// </summary>
        public KinectStatus Status { get { return Kinect.Status; } }

        /// <summary>
        /// Returns if the connect is running or not {True is running}. Is direct from the machine so Use wisely Ie Spam creates a HRefReuslt that exeptions out
        /// </summary>
        public bool IsRunning { get { return Kinect.IsRunning; } }

        /// <summary>
        /// Returns the Kinect Dev Actual Elevation. Is direct from the machine so Use wisely Ie Spam creates a HRefReuslt that exeptions out
        /// </summary>
        public int Tilt { get { return Kinect.ElevationAngle; } }

        /// <summary>
        /// The Picture ready event to Tie to. Event Returns This class as obj and a PictureReadyEventArgs with the picture
        /// </summary>
        public event PicturReadyEvent PictureReady;

        /// <summary>
        /// The Sensor count Changed ready event to Tie to. Event Returns This class as obj and a KinectSensorsChangedEvent with the this Cams new index and the new Count
        /// </summary>
        public event KinectSensorsChangedEvent KinectSensorsChanged;

        /// <summary>
        /// The cams tilt needs a cool down
        /// </summary>
        Stopwatch CoolDownTimer = new Stopwatch();

        /// <summary>
        /// a loop for ticking that runs asyn in the back ground but not on another thread
        /// </summary>
        Task TickLoopTask;

        /// <summary>
        /// a count used for rasing the Count Changed Event
        /// </summary>
        int KinectSensorsOldCount = KinectCams.Count;

        /// <summary>
        /// Creats a Basic camera with Kinects that are connected 
        /// </summary>
        /// <param name="KinectToUse">The Kinect Number to use</param>
        public KinectCam(int KinectToUse = 0)
        {
            if (KinectSensor.KinectSensors.Count > KinectToUse)
                Kinect = KinectSensor.KinectSensors[KinectToUse];
            else
                throw new Exception("This Kinect is not connected or your Kinect index is out of range.");

            //Grab all of these once from the device to minimize the actual calls to the device (realy not sure if they vary from device to device but a lack of static marking suggests so (only have one cam so I couldnt exp to much))
            MaxTilt = Kinect.MaxElevationAngle;
            MinTilt = Kinect.MinElevationAngle;

            // set the stream for colour to on 
            Kinect.ColorStream.Enable();

            //tie our event to thiers (realy should have already come as a bitmap guys)
            Kinect.ColorFrameReady += InternalPictureReady;

            // start a asyn pattern for ticking
            TickLoopTask = new Task(new Action(TickLoopFunc));
            TickLoopTask.Start();
        }


        /// <summary>
        /// our retrieval event for the camera. Basicly Grabs the raw data and puts it to a managed bitmap form
        /// </summary>
        /// <param name="sender">The cammera</param>
        /// <param name="e"></param>
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

                PictureReady?.Invoke(this, new PictureReadyEventArgs(BitMap));
            }
        }

        /// <summary>
        /// Trys to tilt the camera. 
        /// Due to Design The camera is not meant to be spamed with tilt calls 
        /// So I made a cool down if it is on cool down it will not move
        ///     Returns 
        /// false if it didnt move and true if it did. IE return success
        ///     Addional Notes: 
        /// Motor is ruff there for changes of 1 often dont work especally on the down
        /// To force change make changes of 2
        /// </summary>
        /// <param name="SetTilt">The Tilt to set it to</param>
        /// <returns>If it was successful in setting the tilt</returns>
        public bool SetTilt(int SetTilt)
        {
           
            if (SetTilt < MaxTilt && SetTilt > MinTilt && !CoolDownTimer.IsRunning)
            {
                CoolDownTimer.Start();
                Kinect.ElevationAngle = SetTilt;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Dispoes of all resorces from the camera and memory
        /// </summary>
        public void Dispose()
        {
            if(Kinect.IsRunning)
                Kinect.Stop();
            Kinect.Dispose();
            TickLoopTask.Dispose();
        }

        /// <summary>
        /// starts the cam call to use it
        /// </summary>
        public void StartCam()
        {
            Kinect.Start();
        }

        /// <summary>
        /// stops the cam call to kill feed
        /// </summary>
        public void StopCam()
        {
            Kinect.Stop();
        }

        /// <summary>
        /// toggles the cam 
        /// </summary>
        /// <returns>true when running and false if stoped after operation</returns>
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

        /// <summary>
        /// A method/func that is used for a tick on async pattern with out ting up more threads.
        /// </summary>
        void TickLoopFunc()
        {
            while (true)
            {
                if (CoolDownTimer.IsRunning && CoolDownTimer.ElapsedMilliseconds > TiltCoolDown)
                {
                    CoolDownTimer.Stop();
                    CoolDownTimer.Start();
                }

                if (KinectSensorsOldCount == KinectCams.Count)
                {
                    KinectSensorsOldCount = KinectCams.Count;
                    KinectSensorsChanged?.Invoke(this, new KinectSensorsChangedEventArgs(KinectSensorNumber, KinectCamCount));
                }
            }
        }

    }
}
