namespace KinectTestUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._PicBo_CamDisplay = new System.Windows.Forms.PictureBox();
            this._NuUD_Cam = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this._Bu_Up = new System.Windows.Forms.Button();
            this._Bu_Down = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this._Bu_StartStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._PicBo_CamDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._NuUD_Cam)).BeginInit();
            this.SuspendLayout();
            // 
            // _PicBo_CamDisplay
            // 
            this._PicBo_CamDisplay.BackgroundImage = global::KinectTestUI.Properties.Resources.images6;
            this._PicBo_CamDisplay.Location = new System.Drawing.Point(12, 12);
            this._PicBo_CamDisplay.Name = "_PicBo_CamDisplay";
            this._PicBo_CamDisplay.Size = new System.Drawing.Size(321, 223);
            this._PicBo_CamDisplay.TabIndex = 0;
            this._PicBo_CamDisplay.TabStop = false;
            // 
            // _NuUD_Cam
            // 
            this._NuUD_Cam.Location = new System.Drawing.Point(406, 12);
            this._NuUD_Cam.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._NuUD_Cam.Name = "_NuUD_Cam";
            this._NuUD_Cam.Size = new System.Drawing.Size(75, 20);
            this._NuUD_Cam.TabIndex = 1;
            this._NuUD_Cam.ValueChanged += new System.EventHandler(this._NuUD_Cam_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(339, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Kinect Cam";
            // 
            // _Bu_Up
            // 
            this._Bu_Up.Location = new System.Drawing.Point(342, 50);
            this._Bu_Up.Name = "_Bu_Up";
            this._Bu_Up.Size = new System.Drawing.Size(139, 23);
            this._Bu_Up.TabIndex = 3;
            this._Bu_Up.Text = "Kinect Up";
            this._Bu_Up.UseVisualStyleBackColor = true;
            this._Bu_Up.Click += new System.EventHandler(this._Bu_Up_Click);
            // 
            // _Bu_Down
            // 
            this._Bu_Down.Location = new System.Drawing.Point(342, 79);
            this._Bu_Down.Name = "_Bu_Down";
            this._Bu_Down.Size = new System.Drawing.Size(139, 23);
            this._Bu_Down.TabIndex = 4;
            this._Bu_Down.Text = "Kinect Down";
            this._Bu_Down.UseVisualStyleBackColor = true;
            this._Bu_Down.Click += new System.EventHandler(this._Bu_Down_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // _Bu_StartStop
            // 
            this._Bu_StartStop.Location = new System.Drawing.Point(342, 108);
            this._Bu_StartStop.Name = "_Bu_StartStop";
            this._Bu_StartStop.Size = new System.Drawing.Size(139, 23);
            this._Bu_StartStop.TabIndex = 5;
            this._Bu_StartStop.Text = "Kinect Start";
            this._Bu_StartStop.UseVisualStyleBackColor = true;
            this._Bu_StartStop.Click += new System.EventHandler(this._Bu_Start_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 253);
            this.Controls.Add(this._Bu_StartStop);
            this.Controls.Add(this._Bu_Down);
            this.Controls.Add(this._Bu_Up);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._NuUD_Cam);
            this.Controls.Add(this._PicBo_CamDisplay);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this._PicBo_CamDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._NuUD_Cam)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _PicBo_CamDisplay;
        private System.Windows.Forms.NumericUpDown _NuUD_Cam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _Bu_Up;
        private System.Windows.Forms.Button _Bu_Down;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button _Bu_StartStop;
    }
}

