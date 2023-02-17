namespace WF_SyncData
{
    partial class frmSyncData
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
            this.btnSyncData = new System.Windows.Forms.Button();
            this.timerSyncMasterData = new System.Windows.Forms.Timer(this.components);
            this.btnSyncProfile = new System.Windows.Forms.Button();
            this.timerSyncProfile = new System.Windows.Forms.Timer(this.components);
            this.btnSyncMaterial = new System.Windows.Forms.Button();
            this.timerSyncMaterial = new System.Windows.Forms.Timer(this.components);
            this.btnSyncMaterialDetails = new System.Windows.Forms.Button();
            this.timerSyncMaterialDetail = new System.Windows.Forms.Timer(this.components);
            this.btnSyncContact = new System.Windows.Forms.Button();
            this.timerSyncContact = new System.Windows.Forms.Timer(this.components);
            this.btnSyncProfileWithCAG = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSyncData
            // 
            this.btnSyncData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncData.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSyncData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSyncData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncData.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncData.ForeColor = System.Drawing.Color.White;
            this.btnSyncData.Location = new System.Drawing.Point(93, 11);
            this.btnSyncData.Margin = new System.Windows.Forms.Padding(2);
            this.btnSyncData.Name = "btnSyncData";
            this.btnSyncData.Size = new System.Drawing.Size(500, 70);
            this.btnSyncData.TabIndex = 1;
            this.btnSyncData.Text = "SYNC MASTER DATA";
            this.btnSyncData.UseVisualStyleBackColor = false;
            this.btnSyncData.Click += new System.EventHandler(this.btnSyncData_Click);
            // 
            // timerSyncMasterData
            // 
            this.timerSyncMasterData.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnSyncProfile
            // 
            this.btnSyncProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncProfile.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSyncProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSyncProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncProfile.ForeColor = System.Drawing.Color.White;
            this.btnSyncProfile.Location = new System.Drawing.Point(93, 94);
            this.btnSyncProfile.Margin = new System.Windows.Forms.Padding(2);
            this.btnSyncProfile.Name = "btnSyncProfile";
            this.btnSyncProfile.Size = new System.Drawing.Size(500, 70);
            this.btnSyncProfile.TabIndex = 2;
            this.btnSyncProfile.Text = "SYNC PROFILE";
            this.btnSyncProfile.UseVisualStyleBackColor = false;
            this.btnSyncProfile.Click += new System.EventHandler(this.btnSyncProfile_Click);
            // 
            // timerSyncProfile
            // 
            this.timerSyncProfile.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // btnSyncMaterial
            // 
            this.btnSyncMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncMaterial.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSyncMaterial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSyncMaterial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncMaterial.ForeColor = System.Drawing.Color.White;
            this.btnSyncMaterial.Location = new System.Drawing.Point(93, 263);
            this.btnSyncMaterial.Margin = new System.Windows.Forms.Padding(2);
            this.btnSyncMaterial.Name = "btnSyncMaterial";
            this.btnSyncMaterial.Size = new System.Drawing.Size(500, 70);
            this.btnSyncMaterial.TabIndex = 3;
            this.btnSyncMaterial.Text = "SYNC MATERIAL";
            this.btnSyncMaterial.UseVisualStyleBackColor = false;
            this.btnSyncMaterial.Click += new System.EventHandler(this.btnSyncMaterial_Click);
            // 
            // timerSyncMaterial
            // 
            this.timerSyncMaterial.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // btnSyncMaterialDetails
            // 
            this.btnSyncMaterialDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncMaterialDetails.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSyncMaterialDetails.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnSyncMaterialDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncMaterialDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncMaterialDetails.ForeColor = System.Drawing.Color.White;
            this.btnSyncMaterialDetails.Location = new System.Drawing.Point(93, 350);
            this.btnSyncMaterialDetails.Margin = new System.Windows.Forms.Padding(2);
            this.btnSyncMaterialDetails.Name = "btnSyncMaterialDetails";
            this.btnSyncMaterialDetails.Size = new System.Drawing.Size(500, 70);
            this.btnSyncMaterialDetails.TabIndex = 4;
            this.btnSyncMaterialDetails.Text = "SYNC MATERIAL DETAILS";
            this.btnSyncMaterialDetails.UseVisualStyleBackColor = false;
            this.btnSyncMaterialDetails.UseWaitCursor = true;
            this.btnSyncMaterialDetails.Click += new System.EventHandler(this.btnSyncMaterialDetails_Click);
            // 
            // timerSyncMaterialDetail
            // 
            this.timerSyncMaterialDetail.Tick += new System.EventHandler(this.timerSyncMaterialDetail_Tick);
            // 
            // btnSyncContact
            // 
            this.btnSyncContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncContact.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSyncContact.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSyncContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncContact.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncContact.ForeColor = System.Drawing.Color.White;
            this.btnSyncContact.Location = new System.Drawing.Point(93, 177);
            this.btnSyncContact.Margin = new System.Windows.Forms.Padding(2);
            this.btnSyncContact.Name = "btnSyncContact";
            this.btnSyncContact.Size = new System.Drawing.Size(500, 70);
            this.btnSyncContact.TabIndex = 5;
            this.btnSyncContact.Text = "SYNC CONTACT";
            this.btnSyncContact.UseVisualStyleBackColor = false;
            this.btnSyncContact.Visible = false;
            this.btnSyncContact.Click += new System.EventHandler(this.btnSyncContact_Click);
            // 
            // timerSyncContact
            // 
            this.timerSyncContact.Tick += new System.EventHandler(this.timerContact_Tick);
            // 
            // btnSyncProfileWithCAG
            // 
            this.btnSyncProfileWithCAG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncProfileWithCAG.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSyncProfileWithCAG.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSyncProfileWithCAG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncProfileWithCAG.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyncProfileWithCAG.ForeColor = System.Drawing.Color.White;
            this.btnSyncProfileWithCAG.Location = new System.Drawing.Point(93, 437);
            this.btnSyncProfileWithCAG.Margin = new System.Windows.Forms.Padding(2);
            this.btnSyncProfileWithCAG.Name = "btnSyncProfileWithCAG";
            this.btnSyncProfileWithCAG.Size = new System.Drawing.Size(500, 70);
            this.btnSyncProfileWithCAG.TabIndex = 6;
            this.btnSyncProfileWithCAG.Text = "SYNC PROFILE WITH CAG";
            this.btnSyncProfileWithCAG.UseVisualStyleBackColor = false;
            this.btnSyncProfileWithCAG.Visible = false;
            this.btnSyncProfileWithCAG.Click += new System.EventHandler(this.btnSyncProfileWithCAG_Click);
            // 
            // frmSyncData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 428);
            this.Controls.Add(this.btnSyncProfileWithCAG);
            this.Controls.Add(this.btnSyncContact);
            this.Controls.Add(this.btnSyncMaterialDetails);
            this.Controls.Add(this.btnSyncMaterial);
            this.Controls.Add(this.btnSyncProfile);
            this.Controls.Add(this.btnSyncData);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmSyncData";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmSyncData_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSyncData;
        private System.Windows.Forms.Timer timerSyncMasterData;
        private System.Windows.Forms.Button btnSyncProfile;
        private System.Windows.Forms.Timer timerSyncProfile;
        private System.Windows.Forms.Button btnSyncMaterial;
        private System.Windows.Forms.Timer timerSyncMaterial;
        private System.Windows.Forms.Button btnSyncMaterialDetails;
        private System.Windows.Forms.Timer timerSyncMaterialDetail;
        private System.Windows.Forms.Button btnSyncContact;
        private System.Windows.Forms.Timer timerSyncContact;
        private System.Windows.Forms.Button btnSyncProfileWithCAG;
    }
}

