
namespace ISD.SendMailCalendar
{
    partial class frmSendMailCalendar
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
            this.timerSend = new System.Windows.Forms.Timer(this.components);
            this.btnTrackingBounceMail = new System.Windows.Forms.Button();
            this.timerTrackingBounceMail = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(277, 113);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(260, 95);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send Mail";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // timerSend
            // 
            this.timerSend.Enabled = true;
            this.timerSend.Tick += new System.EventHandler(this.timerSend_Tick);
            // 
            // btnTrackingBounceMail
            // 
            this.btnTrackingBounceMail.Location = new System.Drawing.Point(561, 309);
            this.btnTrackingBounceMail.Name = "btnTrackingBounceMail";
            this.btnTrackingBounceMail.Size = new System.Drawing.Size(196, 95);
            this.btnTrackingBounceMail.TabIndex = 1;
            this.btnTrackingBounceMail.Text = "Tracking Bounce Mail";
            this.btnTrackingBounceMail.UseVisualStyleBackColor = true;
            this.btnTrackingBounceMail.Visible = false;
            this.btnTrackingBounceMail.Click += new System.EventHandler(this.btnTrackingBounceMail_Click);
            // 
            // timerTrackingBounceMail
            // 
            this.timerTrackingBounceMail.Enabled = true;
            this.timerTrackingBounceMail.Tick += new System.EventHandler(this.timerTrackingBounceMail_Tick);
            // 
            // frmSendMailCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTrackingBounceMail);
            this.Controls.Add(this.btnSend);
            this.Name = "frmSendMailCalendar";
            this.Text = "Send Mail";
            this.Shown += new System.EventHandler(this.frmSendMailCalendar_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Timer timerSend;
        private System.Windows.Forms.Button btnTrackingBounceMail;
        private System.Windows.Forms.Timer timerTrackingBounceMail;
    }
}