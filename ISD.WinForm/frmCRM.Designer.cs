namespace ISD.WinForm
{
    partial class frmCRM
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
            this.btnSend = new System.Windows.Forms.Button();
            this.btnTrackingBounceMail = new System.Windows.Forms.Button();
            this.btnSendNotification = new System.Windows.Forms.Button();
            this.timerSend = new System.Windows.Forms.Timer(this.components);
            this.timerTrackingBounceMail = new System.Windows.Forms.Timer(this.components);
            this.timerSendNotification = new System.Windows.Forms.Timer(this.components);
            this.btn_SyncProfile = new System.Windows.Forms.Button();
            this.timerSyncProfile = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(11, 11);
            this.btnSend.Margin = new System.Windows.Forms.Padding(2);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(195, 68);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send Mail";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnTrackingBounceMail
            // 
            this.btnTrackingBounceMail.Location = new System.Drawing.Point(237, 11);
            this.btnTrackingBounceMail.Margin = new System.Windows.Forms.Padding(2);
            this.btnTrackingBounceMail.Name = "btnTrackingBounceMail";
            this.btnTrackingBounceMail.Size = new System.Drawing.Size(147, 68);
            this.btnTrackingBounceMail.TabIndex = 2;
            this.btnTrackingBounceMail.Text = "Tracking Bounce Mail";
            this.btnTrackingBounceMail.UseVisualStyleBackColor = true;
            this.btnTrackingBounceMail.Visible = false;
            // 
            // btnSendNotification
            // 
            this.btnSendNotification.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendNotification.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSendNotification.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSendNotification.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendNotification.ForeColor = System.Drawing.Color.White;
            this.btnSendNotification.Location = new System.Drawing.Point(416, 9);
            this.btnSendNotification.Margin = new System.Windows.Forms.Padding(2);
            this.btnSendNotification.Name = "btnSendNotification";
            this.btnSendNotification.Size = new System.Drawing.Size(373, 70);
            this.btnSendNotification.TabIndex = 3;
            this.btnSendNotification.Text = "SEND NOTIFICATION";
            this.btnSendNotification.UseVisualStyleBackColor = false;
            this.btnSendNotification.Click += new System.EventHandler(this.btnSendNotification_Click);
            // 
            // timerSend
            // 
            this.timerSend.Enabled = true;
            this.timerSend.Tick += new System.EventHandler(this.timerSend_Tick);
            // 
            // timerSendNotification
            // 
            this.timerSendNotification.Tick += new System.EventHandler(this.timerSendNotification_Tick);
            // 
            // btn_SyncProfile
            // 
            this.btn_SyncProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SyncProfile.BackColor = System.Drawing.Color.DodgerBlue;
            this.btn_SyncProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_SyncProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SyncProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SyncProfile.ForeColor = System.Drawing.Color.White;
            this.btn_SyncProfile.Location = new System.Drawing.Point(416, 103);
            this.btn_SyncProfile.Margin = new System.Windows.Forms.Padding(2);
            this.btn_SyncProfile.Name = "btn_SyncProfile";
            this.btn_SyncProfile.Size = new System.Drawing.Size(373, 70);
            this.btn_SyncProfile.TabIndex = 4;
            this.btn_SyncProfile.Text = "SYNC PROFILE";
            this.btn_SyncProfile.UseVisualStyleBackColor = false;
            this.btn_SyncProfile.Visible = false;
            this.btn_SyncProfile.Click += new System.EventHandler(this.btn_SyncProfile_Click);
            // 
            // timerSyncProfile
            // 
            this.timerSyncProfile.Tick += new System.EventHandler(this.timerSyncProfile_Tick);
            // 
            // frmCRM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_SyncProfile);
            this.Controls.Add(this.btnSendNotification);
            this.Controls.Add(this.btnTrackingBounceMail);
            this.Controls.Add(this.btnSend);
            this.Name = "frmCRM";
            this.Text = "CRM";
            this.Load += new System.EventHandler(this.frmCRM_Load);
            this.Shown += new System.EventHandler(this.frmCRM_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnTrackingBounceMail;
        private System.Windows.Forms.Button btnSendNotification;
        private System.Windows.Forms.Timer timerSend;
        private System.Windows.Forms.Timer timerTrackingBounceMail;
        private System.Windows.Forms.Timer timerSendNotification;
        private System.Windows.Forms.Button btn_SyncProfile;
        private System.Windows.Forms.Timer timerSyncProfile;
    }
}