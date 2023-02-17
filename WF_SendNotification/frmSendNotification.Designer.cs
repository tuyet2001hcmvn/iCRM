namespace WF_SendNotification
{
    partial class frmSendNotification
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
            this.btnSendNotification = new System.Windows.Forms.Button();
            this.timerSendNotification = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnSendNotification
            // 
            this.btnSendNotification.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendNotification.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSendNotification.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSendNotification.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendNotification.ForeColor = System.Drawing.Color.White;
            this.btnSendNotification.Location = new System.Drawing.Point(145, 161);
            this.btnSendNotification.Margin = new System.Windows.Forms.Padding(2);
            this.btnSendNotification.Name = "btnSendNotification";
            this.btnSendNotification.Size = new System.Drawing.Size(500, 70);
            this.btnSendNotification.TabIndex = 2;
            this.btnSendNotification.Text = "SEND NOTIFICATION";
            this.btnSendNotification.UseVisualStyleBackColor = false;
            this.btnSendNotification.Click += new System.EventHandler(this.btnSendNotification_Click);
            // 
            // timerSendNotification
            // 
            this.timerSendNotification.Tick += new System.EventHandler(this.timerTask_Tick);
            // 
            // frmSendNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSendNotification);
            this.Name = "frmSendNotification";
            this.Text = "Send Notification";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSendNotification;
        private System.Windows.Forms.Timer timerSendNotification;
    }
}

