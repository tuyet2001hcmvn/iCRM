using ISD.EntityModels;
using ISD.WinForm.Data;
using ISD.WinForm.Models;
using ISD.WinForm.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISD.WinForm
{
    public partial class frmCRM : Form
    {
        private readonly AppConfig appConfig;

        private static string appPath = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        private static string logFilePath = System.IO.Path.Combine(appPath, DateTime.Now.ToString("yyyyMMdd") + "-SyncDataFromSAP" + ".log");


        public frmCRM()
        {
            InitializeComponent();

            appConfig = GetSetting();

            //SendNotification
            var timerTask = ConfigurationManager.AppSettings["TimerSendNotificationTask"];
            this.timerSendNotification.Interval = int.Parse(timerTask);

            //SyncProfile
            var timerProfile = ConfigurationManager.AppSettings["TimerSyncProfile"];
            this.timerSyncProfile.Interval = int.Parse(timerProfile);
        }

        #region SendMailCalendar
        private void btnSend_Click(object sender, EventArgs e)
        {
            AddSendingEffect();

            //Start service
            timerSend.Start();

            //Run fist time
            SendMailCalendar sendMailCalendar = new SendMailCalendar();
            sendMailCalendar.SendMail(appConfig);

            AddWaitingEffect();
        }
        private void AddSendingEffect()
        {
            btnSend.Text = " The service is running \n Sending email...";
            Console.WriteLine("chagne");
            btnSend.BackColor = Color.LightGreen;
        }
        private void AddWaitingEffect()
        {
            btnSend.Text = " The service is running \n Waiting for the next mailing time";
            btnSend.BackColor = Color.LightBlue;
        }
        private AppConfig GetSetting()
        {
            AppConfig config = new AppConfig();
            config.NumOfMailSend = Int32.Parse(ConfigurationManager.AppSettings["NumOfMailSend"]);
            config.ResendAfterMinutes = Int32.Parse(ConfigurationManager.AppSettings["ResendAfterMinutes"]);
            config.LogPath = Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["LogFolderPath"] + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            config.BounceMailPath = Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["BounceMailFolderPath"];
            config.TrackingBounceMailAfterMinutes = Int32.Parse(ConfigurationManager.AppSettings["TrackingBounceMailAfterMinutes"]);
            config.NET5ApiDomain = ConfigurationManager.AppSettings["NET5ApiDomain"];
            config.UnsubscribePageUrl = ConfigurationManager.AppSettings["UnsubscribePageUrl"];
            config.ConfirmLinkUrl = ConfigurationManager.AppSettings["ConfirmLinkUrl"];
            config.QRCodeUrl = ConfigurationManager.AppSettings["QRCodeUrl"];
            timerSend.Interval = config.ResendAfterMinutes * 60000;
            timerTrackingBounceMail.Interval = config.TrackingBounceMailAfterMinutes * 60000;
            config.StartTime = ConfigurationManager.AppSettings["StartTime"];
            config.EndTime = ConfigurationManager.AppSettings["EndTime"];
            return config;
        }
       

        private void timerSend_Tick(object sender, EventArgs e)
        {
            SendMailCalendar sendMailCalendar = new SendMailCalendar();
            sendMailCalendar.SendMail(appConfig);
        }
        #endregion SendMailCalendar

        #region SendNotification
        private void btnSendNotification_Click(object sender, EventArgs e)
        {
            var _context = new EntityDataContext();
            if (btnSendNotification.Text == "SEND NOTIFICATION")
            {
                btnSendNotification.Text = "SEND NOTIFICATION PROCESSING";
                btnSendNotification.BackColor = Color.DarkRed;

                //WriteLogFile(logFilePath, "SEND NOTIFICATION: Start.");
                SendNotification sendNotification = new SendNotification();
                sendNotification.Send();
                timerSendNotification.Start();
            }
            else if (btnSendNotification.Text == "SEND NOTIFICATION PROCESSING")
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn dừng lại không ?", "Chú ý !", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    timerSendNotification.Stop();
                    btnSendNotification.Text = "SEND NOTIFICATION";
                    btnSendNotification.BackColor = Color.DeepSkyBlue;
                }
            }
        }
        private void timerSendNotification_Tick(object sender, EventArgs e)
        {
            SendNotification sendNotification = new SendNotification();
            sendNotification.Send();
        }
        #endregion SendNotification

        #region WriteLogFile
        //public static void WriteLogFile(string filePath, string message)
        //{
        //    //Folder
        //    var folder = System.IO.Directory.GetParent(filePath).FullName;
        //    bool exists = System.IO.Directory.Exists(folder);
        //    if (!exists)
        //        System.IO.Directory.CreateDirectory(folder);
        //    //File
        //    if (!System.IO.File.Exists(filePath))
        //        System.IO.File.Create(filePath);

        //    var maxRetry = 3;
        //    for (int retry = 0; retry < maxRetry; retry++)
        //    {
        //        try
        //        {

        //            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
        //            {
        //                fileStream.Flush();
        //                fileStream.Close();
        //            }

        //            using (StreamWriter sw = new StreamWriter(filePath, true))
        //            {
        //                string lastRecordText = "# " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " # " + Environment.NewLine + "#" + message + " #" + Environment.NewLine;
        //                sw.WriteLine(lastRecordText);
        //                sw.Close();
        //                break; // you were successfull so leave the retry loop
        //            }
        //        }
        //        catch (IOException)
        //        {
        //            if (retry < maxRetry - 1)
        //            {
        //                System.Threading.Thread.Sleep(2000); // Wait some time before retry (2 secs)
        //            }
        //            else
        //            {
        //                // handle unsuccessfull write attempts or just ignore.
        //            }
        //        }
        //    }


        //}
        #endregion

        private void frmCRM_Shown(object sender, EventArgs e)
        {
            //btnSend_Click(sender, e);
            //btnTrackingBounceMail_Click(sender, e);

            btnSendNotification_Click(sender, e);
        }

        private void frmCRM_Load(object sender, EventArgs e)
        {
            btnSend.PerformClick();
        }

        #region SyncProfile
        private void btn_SyncProfile_Click(object sender, EventArgs e)
        {
            var _context = new EntityDataContext();
            if (btn_SyncProfile.Text == "SYNC PROFILE")
            {
                timerSyncProfile.Start();
                btn_SyncProfile.Text = "SYNC PROFILE PROCESSING";
                btn_SyncProfile.BackColor = Color.DarkRed;
                
                SyncProfile syncProfile = new SyncProfile(_context);
                syncProfile.Sync();
            }
            else if (btn_SyncProfile.Text == "SYNC PROFILE PROCESSING")
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn dừng lại không ?", "Chú ý !", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    timerSyncProfile.Stop();
                    btn_SyncProfile.Text = "SYNC PROFILE";
                    btn_SyncProfile.BackColor = Color.DeepSkyBlue;
                }
            }
        }
        #endregion SyncProfile

        private void timerSyncProfile_Tick(object sender, EventArgs e)
        {
            var _context = new EntityDataContext();
            SyncProfile syncProfile = new SyncProfile(_context);
            syncProfile.Sync();
        }
    }
}
