using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for CustomerProfileXtraReport
/// </summary>
public class CustomerProfileXtraReport : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
    private PageHeaderBand PageHeader;
    private GroupHeaderBand GroupHeader1;
    private GroupFooterBand GroupFooter1;
    private DetailReportBand DetailReport;
    private DetailBand Detail1;
    private XRLabel xrLabel2;
    private XRLabel xrLabel3;
    private XRLabel xrLabel23;
    private XRLabel xrLabel4;
    private XRLabel xrLabel5;
    private XRLabel xrLabel6;
    private XRLabel xrLabel7;
    private XRLabel xrLabel8;
    private XRLabel xrLabel24;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel9;
    private DetailReportBand DetailReport1;
    private DetailBand Detail2;
    private DetailReportBand DetailReport2;
    private DetailBand Detail3;
    private XRTable xrTable5;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private DetailReportBand DetailReport3;
    private DetailBand Detail4;
    private ReportHeaderBand ReportHeader2;
    private XRTable xrTable7;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell49;
    private XRTableCell xrTableCell51;
    private XRTableCell xrTableCell52;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell54;
    private XRTableCell xrTableCell55;
    private XRLabel xrLabel27;
    private DetailReportBand DetailReport4;
    private DetailBand Detail5;
    private XRTable xrTable9;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell62;
    private XRTableCell xrTableCell63;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell67;
    private ReportHeaderBand ReportHeader3;
    private XRLabel xrLabel28;
    private DetailReportBand DetailReport5;
    private DetailBand Detail6;
    private ReportHeaderBand ReportHeader4;
    private XRLabel xrLabel29;
    private DetailReportBand DetailReport6;
    private DetailBand Detail7;
    private XRPictureBox xrPictureBox1;
    private XRPictureBox xrPictureBox2;
    private XRLabel xrLabel12;
    private XRTable xrTable13;
    private XRTableRow xrTableRow25;
    private XRTableCell xrTableCell123;
    private XRTableCell xrTableCell124;
    private XRTableCell xrTableCell125;
    private XRTableCell xrTableCell126;
    private ReportFooterBand ReportFooter1;
    private XRTable xrTable16;
    private XRTableRow xrTableRow26;
    private XRTableCell xrTableCell127;
    private XRTableCell xrTableCell128;
    private XRTableCell xrTableCell129;
    private XRTableCell xrTableCell130;
    private XRTableCell xrTableCell131;
    private ReportHeaderBand ReportHeader6;
    private XRLabel xrLabel30;
    private DetailReportBand DetailReport7;
    private DetailBand Detail8;
    private XRTable xrTable15;
    private XRTableRow xrTableRow31;
    private XRTableCell xrTableCell152;
    private XRTableCell xrTableCell153;
    private XRTableCell xrTableCell154;
    private XRTableCell xrTableCell155;
    private XRTableCell xrTableCell156;
    private ReportHeaderBand ReportHeader5;
    private XRLabel xrLabel31;
    private DetailReportBand DetailReport8;
    private DetailBand Detail9;
    private XRTable xrTable19;
    private XRTableRow xrTableRow22;
    private XRTableCell xrTableCell107;
    private XRTableCell xrTableCell108;
    private XRTableCell xrTableCell109;
    private XRTableCell xrTableCell110;
    private XRTableCell xrTableCell111;
    private ReportHeaderBand ReportHeader7;
    private XRLabel xrLabel32;
    private DetailReportBand DetailReport9;
    private DetailBand Detail10;
    private XRTable xrTable21;
    private XRTableRow xrTableRow24;
    private XRTableCell xrTableCell119;
    private XRTableCell xrTableCell120;
    private XRTableCell xrTableCell121;
    private XRTableCell xrTableCell144;
    private XRTableCell xrTableCell145;
    private XRTableCell xrTableCell143;
    private ReportHeaderBand ReportHeader8;
    private XRLabel xrLabel35;
    private XRLabel xrLabel33;
    private XRLabel xrLabel22;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel17;
    private XRLabel xrLabel16;
    private XRLabel xrLabel15;
    private XRLabel xrLabel14;
    private XRLabel xrLabel13;
    private XRLabel xrLabel1;
    private PageFooterBand PageFooter;
    private XRTableCell xrTableCell50;
    private ReportHeaderBand ReportHeader9;
    private XRLabel xrLabel34;
    private Reports.XDataSet.DsCustomerProfile dsCustomerProfile1;
    private XRLabel xrLabel41;
    private XRLabel xrLabel40;
    private XRLabel xrLabel39;
    private XRLabel xrLabel38;
    private XRLabel xrLabel37;
    private XRLabel xrLabel36;
    private XRLabel xrLabel25;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell142;
    private DetailReportBand DetailReport10;
    private DetailBand Detail11;
    private XRTable xrTable24;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell146;
    private XRTableCell xrTableCell147;
    private ReportHeaderBand ReportHeader10;
    private XRLabel xrLabel42;
    private DetailReportBand DetailReport11;
    private DetailBand Detail12;
    private XRTable xrTable26;
    private XRTableRow xrTableRow29;
    private XRTableCell xrTableCell159;
    private XRTableCell xrTableCell160;
    private XRTableCell xrTableCell161;
    private XRTableCell xrTableCell162;
    private XRTableCell xrTableCell163;
    private ReportHeaderBand ReportHeader11;
    private XRLabel xrLabel43;
    private XRTableCell xrTableCell61;
    private ReportFooterBand ReportFooter;
    private XRTable xrTable12;
    private XRTableRow xrTableRow20;
    private XRTableCell xrTableCell97;
    private XRTableCell xrTableCell98;
    private XRTableCell xrTableCell99;
    private XRTableCell xrTableCell100;
    private XRTableCell xrTableCell101;
    private XRTableCell xrTableCell122;
    private XRLabel xrLabel54;
    private XRLabel xrLabel53;
    private XRLabel xrLabel52;
    private XRLabel xrLabel46;
    private XRLabel xrLabel51;
    private XRLabel xrLabel50;
    private XRLabel xrLabel49;
    private XRLabel xrLabel48;
    private XRLabel xrLabel47;
    private XRLabel xrLabel45;
    private XRLabel xrLabel44;
    private XRLabel xrLabel55;
    private XRLabel xrLabel56;
    private DetailReportBand DetailReport12;
    private DetailBand Detail13;
    private XRLabel xrLabel57;
    private XRLabel xrLabel58;
    private XRLabel xrLabel59;
    private XRPageInfo xrPageInfo1;
    private XRTable xrTable3;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTable xrTable11;
    private XRTableRow xrTableRow19;
    private XRTableCell xrTableCell82;
    private XRTableCell xrTableCell84;
    private XRTableCell xrTableCell85;
    private XRTableCell xrTableCell92;
    private XRTableCell xrTableCell95;
    private XRTableCell xrTableCell96;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private GroupHeaderBand GroupHeader2;
    private XRTable xrTable4;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell30;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell40;
    private GroupHeaderBand GroupHeader3;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private ReportHeaderBand ReportHeader1;
    private XRLabel xrLabel26;
    private GroupHeaderBand GroupHeader4;
    private XRTable xrTable27;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell65;
    private XRTableCell xrTableCell68;
    private XRTableCell xrTableCell69;
    private GroupHeaderBand GroupHeader5;
    private XRTable xrTable6;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell44;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell47;
    private XRTableCell xrTableCell48;
    private GroupHeaderBand GroupHeader6;
    private XRTable xrTable22;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell70;
    private XRTableCell xrTableCell71;
    private GroupHeaderBand GroupHeader7;
    private XRTable xrTable10;
    private XRTableRow xrTableRow17;
    private XRTableCell xrTableCell64;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell83;
    private XRTableCell xrTableCell87;
    private XRTableRow xrTableRow18;
    private XRTableCell xrTableCell88;
    private XRTableCell xrTableCell89;
    private XRTableCell xrTableCell90;
    private XRTableCell xrTableCell91;
    private XRTableCell xrTableCell93;
    private XRTableCell xrTableCell94;
    private GroupHeaderBand GroupHeader8;
    private XRTable xrTable17;
    private XRTableRow xrTableRow28;
    private XRTableCell xrTableCell137;
    private XRTableCell xrTableCell138;
    private XRTableCell xrTableCell139;
    private XRTableCell xrTableCell140;
    private XRTableCell xrTableCell141;
    private GroupHeaderBand GroupHeader9;
    private XRTable xrTable14;
    private XRTableRow xrTableRow27;
    private XRTableCell xrTableCell132;
    private XRTableCell xrTableCell133;
    private XRTableCell xrTableCell134;
    private XRTableCell xrTableCell135;
    private XRTableCell xrTableCell136;
    private GroupHeaderBand GroupHeader10;
    private XRTable xrTable18;
    private XRTableRow xrTableRow21;
    private XRTableCell xrTableCell102;
    private XRTableCell xrTableCell103;
    private XRTableCell xrTableCell104;
    private XRTableCell xrTableCell105;
    private XRTableCell xrTableCell106;
    private XRTableCell xrTableCell13;
    private GroupHeaderBand GroupHeader12;
    private XRTable xrTable20;
    private XRTableRow xrTableRow23;
    private XRTableCell xrTableCell112;
    private XRTableCell xrTableCell113;
    private XRTableCell xrTableCell114;
    private XRTableCell xrTableCell117;
    private XRTableCell xrTableCell118;
    private XRTableCell xrTableCell115;
    private XRTableCell xrTableCell116;
    private GroupHeaderBand GroupHeader13;
    private XRTable xrTable23;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    private GroupHeaderBand GroupHeader11;
    private XRTable xrTable25;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell149;
    private XRTableCell xrTableCell150;
    private XRTableCell xrTableCell151;
    private XRTableCell xrTableCell157;
    private XRTableCell xrTableCell158;
    private XRLabel xrLabel62;
    private XRLabel xrLabel61;
    private XRLabel xrLabel60;
    private Reports.XDataSet.DsCustomerProfile dsCustomerProfile2;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public CustomerProfileXtraReport()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

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

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerProfileXtraReport));
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary6 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary7 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel49 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader3 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.dsCustomerProfile1 = new Reports.XDataSet.DsCustomerProfile();
            this.DetailReport1 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail2 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader9 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader4 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable27 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport2 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail3 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailReport3 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail4 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader2 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader5 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport4 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail5 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable9 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader3 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader6 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable22 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport5 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail6 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable11 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell92 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell95 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell96 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader4 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrTable12 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow20 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell97 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell98 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell99 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell100 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell101 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader7 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable10 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell89 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell93 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell94 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport6 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail7 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable13 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow25 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell122 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell123 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell124 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell125 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell126 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportFooter1 = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrTable16 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow26 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell127 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell128 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell129 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell130 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell131 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader6 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader8 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable17 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow28 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell137 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell138 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell139 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell140 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell141 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport7 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail8 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable15 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow31 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell152 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell153 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell154 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell155 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell156 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader5 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader9 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable14 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow27 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell132 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell133 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell134 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell135 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell136 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport8 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail9 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable19 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell107 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell108 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell109 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell110 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell111 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader7 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader10 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable18 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow21 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell102 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell103 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell104 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell105 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell106 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport9 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail10 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable21 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow24 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell119 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell120 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell121 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell144 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell145 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell142 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell143 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader8 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader12 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable20 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow23 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell112 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell113 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell114 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell117 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell118 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell115 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell116 = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.DetailReport10 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail11 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable24 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell146 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell147 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader10 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader13 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable23 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport11 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail12 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable26 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow29 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell159 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell160 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell161 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell162 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell163 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader11 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader11 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable25 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell149 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell150 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell151 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell157 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell158 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport12 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail13 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel58 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
            this.dsCustomerProfile2 = new Reports.XDataSet.DsCustomerProfile();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsCustomerProfile1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsCustomerProfile2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel62,
            this.xrLabel61,
            this.xrLabel60,
            this.xrLabel54,
            this.xrLabel53,
            this.xrLabel52,
            this.xrLabel46,
            this.xrLabel51,
            this.xrLabel50,
            this.xrLabel49,
            this.xrLabel48,
            this.xrLabel47,
            this.xrLabel45,
            this.xrLabel44,
            this.xrLabel41,
            this.xrLabel40,
            this.xrLabel39,
            this.xrLabel38,
            this.xrLabel37,
            this.xrLabel36,
            this.xrLabel25,
            this.xrLabel33,
            this.xrLabel22,
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel17,
            this.xrLabel16,
            this.xrLabel15,
            this.xrLabel14,
            this.xrLabel13,
            this.xrLabel2,
            this.xrLabel3,
            this.xrLabel23,
            this.xrLabel4,
            this.xrLabel5,
            this.xrLabel6,
            this.xrLabel7,
            this.xrLabel8,
            this.xrLabel24,
            this.xrLabel11,
            this.xrLabel10});
            this.Detail.HeightF = 328.4722F;
            this.Detail.KeepTogetherWithDetailReports = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel62
            // 
            this.xrLabel62.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(267.3622F, 302.0161F);
            this.xrLabel62.Name = "xrLabel62";
            this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel62.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel62.StylePriority.UseFont = false;
            this.xrLabel62.StylePriority.UseTextAlignment = false;
            this.xrLabel62.Text = ":";
            this.xrLabel62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel61
            // 
            this.xrLabel61.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(267.3622F, 279.0161F);
            this.xrLabel61.Name = "xrLabel61";
            this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel61.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel61.StylePriority.UseFont = false;
            this.xrLabel61.StylePriority.UseTextAlignment = false;
            this.xrLabel61.Text = ":";
            this.xrLabel61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel60
            // 
            this.xrLabel60.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(267.3622F, 256.0161F);
            this.xrLabel60.Name = "xrLabel60";
            this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel60.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel60.StylePriority.UseFont = false;
            this.xrLabel60.StylePriority.UseTextAlignment = false;
            this.xrLabel60.Text = ":";
            this.xrLabel60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel54
            // 
            this.xrLabel54.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(494.1903F, 139.6069F);
            this.xrLabel54.Name = "xrLabel54";
            this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel54.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel54.StylePriority.UseFont = false;
            this.xrLabel54.StylePriority.UseTextAlignment = false;
            this.xrLabel54.Text = ":";
            this.xrLabel54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel53
            // 
            this.xrLabel53.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(494.1903F, 162.6067F);
            this.xrLabel53.Name = "xrLabel53";
            this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel53.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel53.StylePriority.UseFont = false;
            this.xrLabel53.StylePriority.UseTextAlignment = false;
            this.xrLabel53.Text = ":";
            this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel52
            // 
            this.xrLabel52.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(494.1903F, 185.6067F);
            this.xrLabel52.Name = "xrLabel52";
            this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel52.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel52.StylePriority.UseFont = false;
            this.xrLabel52.StylePriority.UseTextAlignment = false;
            this.xrLabel52.Text = ":";
            this.xrLabel52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel46
            // 
            this.xrLabel46.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(198.7671F, 57.73913F);
            this.xrLabel46.Name = "xrLabel46";
            this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel46.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel46.StylePriority.UseFont = false;
            this.xrLabel46.StylePriority.UseTextAlignment = false;
            this.xrLabel46.Text = ":";
            this.xrLabel46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel51
            // 
            this.xrLabel51.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(126.0877F, 185.6068F);
            this.xrLabel51.Name = "xrLabel51";
            this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel51.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel51.StylePriority.UseFont = false;
            this.xrLabel51.StylePriority.UseTextAlignment = false;
            this.xrLabel51.Text = ":";
            this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel50
            // 
            this.xrLabel50.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(126.0877F, 162.6068F);
            this.xrLabel50.Name = "xrLabel50";
            this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel50.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel50.StylePriority.UseFont = false;
            this.xrLabel50.StylePriority.UseTextAlignment = false;
            this.xrLabel50.Text = ":";
            this.xrLabel50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel49
            // 
            this.xrLabel49.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel49.LocationFloat = new DevExpress.Utils.PointFloat(126.0877F, 139.6068F);
            this.xrLabel49.Name = "xrLabel49";
            this.xrLabel49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel49.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel49.StylePriority.UseFont = false;
            this.xrLabel49.StylePriority.UseTextAlignment = false;
            this.xrLabel49.Text = ":";
            this.xrLabel49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel48
            // 
            this.xrLabel48.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(126.0877F, 116.6068F);
            this.xrLabel48.Name = "xrLabel48";
            this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel48.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel48.StylePriority.UseFont = false;
            this.xrLabel48.StylePriority.UseTextAlignment = false;
            this.xrLabel48.Text = ":";
            this.xrLabel48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel47
            // 
            this.xrLabel47.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(126.0877F, 93.60687F);
            this.xrLabel47.Name = "xrLabel47";
            this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel47.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel47.StylePriority.UseFont = false;
            this.xrLabel47.StylePriority.UseTextAlignment = false;
            this.xrLabel47.Text = ":";
            this.xrLabel47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel45
            // 
            this.xrLabel45.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(198.7672F, 34.73913F);
            this.xrLabel45.Name = "xrLabel45";
            this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel45.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel45.StylePriority.UseFont = false;
            this.xrLabel45.StylePriority.UseTextAlignment = false;
            this.xrLabel45.Text = ":";
            this.xrLabel45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel44
            // 
            this.xrLabel44.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(198.7672F, 11.73917F);
            this.xrLabel44.Name = "xrLabel44";
            this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel44.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel44.StylePriority.UseFont = false;
            this.xrLabel44.StylePriority.UseTextAlignment = false;
            this.xrLabel44.Text = ":";
            this.xrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel41
            // 
            this.xrLabel41.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[IndicationsUseACMaterial]")});
            this.xrLabel41.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(277.3622F, 302.0161F);
            this.xrLabel41.Name = "xrLabel41";
            this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel41.SizeF = new System.Drawing.SizeF(228.9656F, 23F);
            this.xrLabel41.StylePriority.UseFont = false;
            this.xrLabel41.StylePriority.UseTextAlignment = false;
            this.xrLabel41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel40
            // 
            this.xrLabel40.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[HabitUsingNewProducts]")});
            this.xrLabel40.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(277.3622F, 279.0161F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(228.9656F, 23F);
            this.xrLabel40.StylePriority.UseFont = false;
            this.xrLabel40.StylePriority.UseTextAlignment = false;
            this.xrLabel40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel39
            // 
            this.xrLabel39.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FirstTradingDate]")});
            this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(277.3622F, 256.0161F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(228.9656F, 23F);
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.StylePriority.UseTextAlignment = false;
            this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel39.TextFormatString = "{0:dd/MM/yyyy}";
            // 
            // xrLabel38
            // 
            this.xrLabel38.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(50.29245F, 302.0161F);
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(217.0698F, 23F);
            this.xrLabel38.StylePriority.UseFont = false;
            this.xrLabel38.StylePriority.UseTextAlignment = false;
            this.xrLabel38.Text = "-      Chỉ định dùng vật liệu An Cường";
            this.xrLabel38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel37
            // 
            this.xrLabel37.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(50.35668F, 279.0161F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(217.0056F, 23F);
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.StylePriority.UseTextAlignment = false;
            this.xrLabel37.Text = "-      Thói quen dùng sản phẩm mới     ";
            this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel36
            // 
            this.xrLabel36.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(50.29245F, 256.0161F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(217.0698F, 23F);
            this.xrLabel36.StylePriority.UseFont = false;
            this.xrLabel36.StylePriority.UseTextAlignment = false;
            this.xrLabel36.Text = "-      Ngày giao dịch đầu tiên                ";
            this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel25
            // 
            this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(1.000226F, 225.5526F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(399.9134F, 23F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "LỊCH SỬ VÀ THÓI QUEN MUA HÀNG";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel33
            // 
            this.xrLabel33.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Email]")});
            this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(504.1903F, 185.607F);
            this.xrLabel33.Multiline = true;
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(229.8959F, 22.99998F);
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.StylePriority.UseTextAlignment = false;
            this.xrLabel33.Text = "xrLabel13";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel22
            // 
            this.xrLabel22.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Phone]")});
            this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(504.1903F, 162.607F);
            this.xrLabel22.Multiline = true;
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(229.8958F, 22.99998F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "xrLabel13";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel21
            // 
            this.xrLabel21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Website]")});
            this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(504.1903F, 139.6069F);
            this.xrLabel21.Multiline = true;
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(229.8958F, 23.00002F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "xrLabel13";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel20
            // 
            this.xrLabel20.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CustomerCareerName]")});
            this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(136.0877F, 185.6068F);
            this.xrLabel20.Multiline = true;
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(264.826F, 23F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "xrLabel13";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel19
            // 
            this.xrLabel19.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfileGroupName]")});
            this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(136.0877F, 162.6068F);
            this.xrLabel19.Multiline = true;
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(264.8259F, 23F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "xrLabel13";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel18
            // 
            this.xrLabel18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TaxNo]")});
            this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(136.0877F, 139.6068F);
            this.xrLabel18.Multiline = true;
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(264.8259F, 23.00002F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "xrLabel13";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel17
            // 
            this.xrLabel17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address]")});
            this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(136.0877F, 116.6068F);
            this.xrLabel17.Multiline = true;
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(605.912F, 23.00001F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "xrLabel13";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel16
            // 
            this.xrLabel16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfileName]")});
            this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(136.0877F, 93.60685F);
            this.xrLabel16.Multiline = true;
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(605.912F, 22.99997F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "xrLabel13";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel15
            // 
            this.xrLabel15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CreateTime]")});
            this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(208.7671F, 57.73913F);
            this.xrLabel15.Multiline = true;
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(271.331F, 23F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "xrLabel13";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel15.TextFormatString = "{0:dd/MM/yyyy}";
            // 
            // xrLabel14
            // 
            this.xrLabel14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfileForeignCode]")});
            this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(208.7671F, 34.73917F);
            this.xrLabel14.Multiline = true;
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(271.937F, 23F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "xrLabel13";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel13
            // 
            this.xrLabel13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfileCode]")});
            this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(208.7672F, 11.73917F);
            this.xrLabel13.Multiline = true;
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(271.9369F, 23F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "xrLabel13";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(1.000086F, 11.73917F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(197.767F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Mã CRM                ";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(1.000139F, 34.73916F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(197.767F, 23.00001F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Mã SAP                   ";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel23
            // 
            this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(1.000157F, 57.73919F);
            this.xrLabel23.Multiline = true;
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(197.7669F, 23.00001F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "Ngày tạo thông tin trên CRM";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(1.00021F, 93.60682F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(125.0875F, 23.00001F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Tên khách hàng     ";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(1.000148F, 116.6069F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(125.0875F, 23.00001F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Địa chỉ hóa đơn     ";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(1.00021F, 139.6068F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(125.0875F, 23.00002F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Mã số thuế             ";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(1.000148F, 162.6069F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(125.0875F, 23.00002F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Nhóm khách          ";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(1.000228F, 185.6068F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(125.0874F, 23.00002F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Ngành nghề           ";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(411.9352F, 162.6069F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(82.25507F, 23.00002F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "Điện thoại";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(411.9354F, 185.6068F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(82.25488F, 23.00002F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "Email       ";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(411.9354F, 139.6068F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(82.25488F, 23.00002F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "Website ";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 24F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 31F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1});
            this.PageHeader.HeightF = 71.25001F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(55.68679F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(630.2084F, 71.25001F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.GroupHeader1.HeightF = 51.58333F;
            this.GroupHeader1.Level = 1;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(749F, 38.92858F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "PROFILE KHÁCH HÀNG";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel12});
            this.GroupFooter1.HeightF = 143.764F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.Visible = false;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(441.8834F, 23F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "BIỂU ĐỒ DOANH SỐ KHÁCH HÀNG – ĐÁNH GIÁ";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel12.Visible = false;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox2.ImageSource"));
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(50.29243F, 0F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(635.6027F, 31.25F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSourceType = null;
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1,
            this.ReportHeader,
            this.GroupHeader3});
            this.DetailReport.DataMember = "InCharge";
            this.DetailReport.DataSource = this.dsCustomerProfile1;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail1.HeightF = 25.24622F;
            this.Detail1.Name = "Detail1";
            // 
            // xrTable2
            // 
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6,
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell12,
            this.xrTableCell9,
            this.xrTableCell10});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow2.StylePriority.UseBorders = false;
            this.xrTableRow2.StylePriority.UsePadding = false;
            this.xrTableRow2.StylePriority.UseTextAlignment = false;
            this.xrTableRow2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow2.Weight = 11.5D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DataSource.CurrentRowIndex] + 1")});
            this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseBorders = false;
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            xrSummary1.IgnoreNullValues = true;
            this.xrTableCell6.Summary = xrSummary1;
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell6.Weight = 0.11299700298920412D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SalesEmployeeCode]")});
            this.xrTableCell7.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseBorders = false;
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "xrTableCell7";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell7.Weight = 0.23282835755518716D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SalesEmployeeName]")});
            this.xrTableCell8.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 3, 100F);
            this.xrTableCell8.StylePriority.UseBorders = false;
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.StylePriority.UsePadding = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "xrTableCell8";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell8.Weight = 0.41029961000312787D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RoleName]")});
            this.xrTableCell12.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseBorders = false;
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "xrTableCell12";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell12.Weight = 0.39111124146315657D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PositionName]")});
            this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "xrTableCell9";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell9.Weight = 0.33385773404111257D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CompanyCode]")});
            this.xrTableCell10.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "xrTableCell10";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 0.22476612354075837D;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9});
            this.ReportHeader.HeightF = 36.58185F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 13.58185F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(241.9716F, 23F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "NHÂN VIÊN PHỤ TRÁCH";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader3
            // 
            this.GroupHeader3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.GroupHeader3.HeightF = 25F;
            this.GroupHeader3.Name = "GroupHeader3";
            this.GroupHeader3.RepeatEveryPage = true;
            // 
            // xrTable1
            // 
            this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell11,
            this.xrTableCell4,
            this.xrTableCell5});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow1.StylePriority.UseBorders = false;
            this.xrTableRow1.StylePriority.UsePadding = false;
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.Text = "STT";
            this.xrTableCell1.Weight = 0.36109905065115067D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.Text = "Mã NV";
            this.xrTableCell2.Weight = 0.74403842433408485D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.Text = "Tên NV";
            this.xrTableCell3.Weight = 1.3111746425407929D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.Text = "Phòng ban";
            this.xrTableCell11.Weight = 1.2498551691522513D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.Text = "Chức vụ";
            this.xrTableCell4.Weight = 1.0668925786600909D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.Text = "Mã công ty";
            this.xrTableCell5.Weight = 0.71827460853000091D;
            // 
            // dsCustomerProfile1
            // 
            this.dsCustomerProfile1.DataSetName = "DsCustomerProfile";
            this.dsCustomerProfile1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // DetailReport1
            // 
            this.DetailReport1.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail2,
            this.ReportHeader9,
            this.GroupHeader4});
            this.DetailReport1.DataMember = "Address";
            this.DetailReport1.DataSource = this.dsCustomerProfile1;
            this.DetailReport1.Level = 1;
            this.DetailReport1.Name = "DetailReport1";
            // 
            // Detail2
            // 
            this.Detail2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
            this.Detail2.HeightF = 25F;
            this.Detail2.Name = "Detail2";
            // 
            // xrTable3
            // 
            this.xrTable3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(10.00029F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTable3.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            this.xrTable3.StylePriority.UseFont = false;
            this.xrTable3.StylePriority.UseTextAlignment = false;
            this.xrTable3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell17});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow4.StylePriority.UseBorders = false;
            this.xrTableRow4.StylePriority.UsePadding = false;
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DataSource.CurrentRowIndex] + 1")});
            this.xrTableCell15.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell15.Multiline = true;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseBorders = false;
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell15.Weight = 0.36109905065115067D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AddressType]")});
            this.xrTableCell16.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell16.Multiline = true;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 3, 100F);
            this.xrTableCell16.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.xrTableCell16.StylePriority.UseBorders = false;
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.StylePriority.UsePadding = false;
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell16.Weight = 1.2303521852277686D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address]")});
            this.xrTableCell17.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell17.Multiline = true;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.xrTableCell17.StylePriority.UseBorders = false;
            this.xrTableCell17.StylePriority.UseFont = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = "xrTableCell17";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell17.Weight = 3.8598827001787623D;
            // 
            // ReportHeader9
            // 
            this.ReportHeader9.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel34});
            this.ReportHeader9.HeightF = 37.71137F;
            this.ReportHeader9.Name = "ReportHeader9";
            // 
            // xrLabel34
            // 
            this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(0F, 14.71137F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(241.9716F, 23F);
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.StylePriority.UseTextAlignment = false;
            this.xrLabel34.Text = "CÁC VĂN PHÒNG";
            this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader4
            // 
            this.GroupHeader4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable27});
            this.GroupHeader4.HeightF = 25F;
            this.GroupHeader4.Name = "GroupHeader4";
            this.GroupHeader4.RepeatEveryPage = true;
            // 
            // xrTable27
            // 
            this.xrTable27.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable27.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable27.Name = "xrTable27";
            this.xrTable27.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow14});
            this.xrTable27.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            this.xrTable27.StylePriority.UseFont = false;
            this.xrTable27.StylePriority.UseTextAlignment = false;
            this.xrTable27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell65,
            this.xrTableCell68,
            this.xrTableCell69});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow14.StylePriority.UseBorders = false;
            this.xrTableRow14.StylePriority.UsePadding = false;
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell65
            // 
            this.xrTableCell65.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell65.Name = "xrTableCell65";
            this.xrTableCell65.StylePriority.UseBorders = false;
            this.xrTableCell65.Text = "STT";
            this.xrTableCell65.Weight = 0.36109905065115067D;
            // 
            // xrTableCell68
            // 
            this.xrTableCell68.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell68.Name = "xrTableCell68";
            this.xrTableCell68.StylePriority.UseBorders = false;
            this.xrTableCell68.Text = "Tên chi nhánh";
            this.xrTableCell68.Weight = 1.2303521852277686D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.StylePriority.UseBorders = false;
            this.xrTableCell69.Text = "Địa chỉ";
            this.xrTableCell69.Weight = 3.8598827001787623D;
            // 
            // DetailReport2
            // 
            this.DetailReport2.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail3,
            this.GroupHeader2,
            this.ReportHeader1});
            this.DetailReport2.DataMember = "Contact";
            this.DetailReport2.DataSource = this.dsCustomerProfile1;
            this.DetailReport2.Level = 2;
            this.DetailReport2.Name = "DetailReport2";
            // 
            // Detail3
            // 
            this.Detail3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable5});
            this.Detail3.HeightF = 25F;
            this.Detail3.Name = "Detail3";
            // 
            // xrTable5
            // 
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(10.00031F, 0F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow9});
            this.xrTable5.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell34,
            this.xrTableCell35,
            this.xrTableCell41,
            this.xrTableCell36,
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell39});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow9.StylePriority.UseBorders = false;
            this.xrTableRow9.StylePriority.UsePadding = false;
            this.xrTableRow9.StylePriority.UseTextAlignment = false;
            this.xrTableRow9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow9.Weight = 11.5D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell34.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DataSource.CurrentRowIndex] + 1")});
            this.xrTableCell34.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell34.StylePriority.UseBorders = false;
            this.xrTableCell34.StylePriority.UseFont = false;
            this.xrTableCell34.StylePriority.UsePadding = false;
            this.xrTableCell34.StylePriority.UseTextAlignment = false;
            xrSummary2.IgnoreNullValues = true;
            xrSummary2.TreatStringsAsNumerics = false;
            this.xrTableCell34.Summary = xrSummary2;
            this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell34.Weight = 0.11929668256097906D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell35.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfileName]")});
            this.xrTableCell35.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell35.StylePriority.UseBorders = false;
            this.xrTableCell35.StylePriority.UseFont = false;
            this.xrTableCell35.StylePriority.UsePadding = false;
            this.xrTableCell35.StylePriority.UseTextAlignment = false;
            this.xrTableCell35.Text = "xrTableCell7";
            this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell35.Weight = 0.3534836197909465D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell41.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BirthDay]")});
            this.xrTableCell41.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell41.Multiline = true;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell41.StylePriority.UseBorders = false;
            this.xrTableCell41.StylePriority.UseFont = false;
            this.xrTableCell41.StylePriority.UsePadding = false;
            this.xrTableCell41.StylePriority.UseTextAlignment = false;
            this.xrTableCell41.Text = "xrTableCell41";
            this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell41.TextFormatString = "{0:dd/MM/yyyy}";
            this.xrTableCell41.Weight = 0.32443108572060403D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell36.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Phone]")});
            this.xrTableCell36.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell36.StylePriority.UseBorders = false;
            this.xrTableCell36.StylePriority.UseFont = false;
            this.xrTableCell36.StylePriority.UsePadding = false;
            this.xrTableCell36.StylePriority.UseTextAlignment = false;
            this.xrTableCell36.Text = "xrTableCell8";
            this.xrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell36.Weight = 0.32815468221938193D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell37.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Email]")});
            this.xrTableCell37.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell37.Multiline = true;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell37.StylePriority.UseBorders = false;
            this.xrTableCell37.StylePriority.UseFont = false;
            this.xrTableCell37.StylePriority.UsePadding = false;
            this.xrTableCell37.StylePriority.UseTextAlignment = false;
            this.xrTableCell37.Text = "xrTableCell12";
            this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell37.Weight = 0.34591312917718342D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell38.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DepartmentName]")});
            this.xrTableCell38.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell38.StylePriority.UseBorders = false;
            this.xrTableCell38.StylePriority.UseFont = false;
            this.xrTableCell38.StylePriority.UsePadding = false;
            this.xrTableCell38.StylePriority.UseTextAlignment = false;
            this.xrTableCell38.Text = "xrTableCell9";
            this.xrTableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell38.Weight = 0.33254937328520628D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell39.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PositionName]")});
            this.xrTableCell39.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell39.StylePriority.UseBorders = false;
            this.xrTableCell39.StylePriority.UseFont = false;
            this.xrTableCell39.StylePriority.UsePadding = false;
            this.xrTableCell39.StylePriority.UseTextAlignment = false;
            this.xrTableCell39.Text = "xrTableCell10";
            this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell39.Weight = 0.3634551582132744D;
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
            this.GroupHeader2.HeightF = 25F;
            this.GroupHeader2.Name = "GroupHeader2";
            this.GroupHeader2.RepeatEveryPage = true;
            // 
            // xrTable4
            // 
            this.xrTable4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8});
            this.xrTable4.SizeF = new System.Drawing.SizeF(732F, 25F);
            this.xrTable4.StylePriority.UseFont = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell28,
            this.xrTableCell29,
            this.xrTableCell30,
            this.xrTableCell31,
            this.xrTableCell32,
            this.xrTableCell33,
            this.xrTableCell40});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow8.StylePriority.UseBorders = false;
            this.xrTableRow8.StylePriority.UsePadding = false;
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseBorders = false;
            this.xrTableCell28.Text = "STT";
            this.xrTableCell28.Weight = 0.36109905065115067D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseBorders = false;
            this.xrTableCell29.Text = "Tên liên hệ";
            this.xrTableCell29.Weight = 1.0699593334026982D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.StylePriority.UseBorders = false;
            this.xrTableCell30.Text = "Ngày sinh";
            this.xrTableCell30.Weight = 0.98202023025191509D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell31.Multiline = true;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.StylePriority.UseBorders = false;
            this.xrTableCell31.Text = "SĐT";
            this.xrTableCell31.Weight = 0.99329045149688D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.StylePriority.UseBorders = false;
            this.xrTableCell32.Text = "Email";
            this.xrTableCell32.Weight = 1.0470449713504002D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.StylePriority.UseBorders = false;
            this.xrTableCell33.Text = "Phòng ban";
            this.xrTableCell33.Weight = 1.0065939156080526D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell40.Multiline = true;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.StylePriority.UseBorders = false;
            this.xrTableCell40.Text = "Chức vụ";
            this.xrTableCell40.Weight = 1.10014171147193D;
            // 
            // ReportHeader1
            // 
            this.ReportHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel26});
            this.ReportHeader1.HeightF = 38.76886F;
            this.ReportHeader1.Name = "ReportHeader1";
            // 
            // xrLabel26
            // 
            this.xrLabel26.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(0F, 15.76886F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(241.9716F, 23F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "LIÊN HỆ";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailReport3
            // 
            this.DetailReport3.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail4,
            this.ReportHeader2,
            this.GroupHeader5});
            this.DetailReport3.DataMember = "Contractor";
            this.DetailReport3.DataSource = this.dsCustomerProfile1;
            this.DetailReport3.Level = 3;
            this.DetailReport3.Name = "DetailReport3";
            // 
            // Detail4
            // 
            this.Detail4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable7});
            this.Detail4.HeightF = 25F;
            this.Detail4.Name = "Detail4";
            // 
            // xrTable7
            // 
            this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable7.Name = "xrTable7";
            this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow11});
            this.xrTable7.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell49,
            this.xrTableCell50,
            this.xrTableCell51,
            this.xrTableCell52,
            this.xrTableCell53,
            this.xrTableCell54,
            this.xrTableCell55});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow11.StylePriority.UseBorders = false;
            this.xrTableRow11.StylePriority.UsePadding = false;
            this.xrTableRow11.StylePriority.UseTextAlignment = false;
            this.xrTableRow11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow11.Weight = 11.5D;
            // 
            // xrTableCell49
            // 
            this.xrTableCell49.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell49.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DataSource.CurrentRowIndex] + 1")});
            this.xrTableCell49.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell49.Name = "xrTableCell49";
            this.xrTableCell49.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell49.StylePriority.UseBorders = false;
            this.xrTableCell49.StylePriority.UseFont = false;
            this.xrTableCell49.StylePriority.UsePadding = false;
            this.xrTableCell49.StylePriority.UseTextAlignment = false;
            xrSummary3.IgnoreNullValues = true;
            this.xrTableCell49.Summary = xrSummary3;
            this.xrTableCell49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell49.Weight = 0.11929668256097906D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell50.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfileName]")});
            this.xrTableCell50.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell50.StylePriority.UseBorders = false;
            this.xrTableCell50.StylePriority.UseFont = false;
            this.xrTableCell50.StylePriority.UsePadding = false;
            this.xrTableCell50.StylePriority.UseTextAlignment = false;
            this.xrTableCell50.Text = "xrTableCell7";
            this.xrTableCell50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell50.Weight = 0.3534836197909465D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell51.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ContactName]")});
            this.xrTableCell51.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell51.Multiline = true;
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell51.StylePriority.UseBorders = false;
            this.xrTableCell51.StylePriority.UseFont = false;
            this.xrTableCell51.StylePriority.UsePadding = false;
            this.xrTableCell51.StylePriority.UseTextAlignment = false;
            this.xrTableCell51.Text = "xrTableCell41";
            this.xrTableCell51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell51.Weight = 0.32443108572060403D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell52.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfilePhone]")});
            this.xrTableCell52.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell52.StylePriority.UseBorders = false;
            this.xrTableCell52.StylePriority.UseFont = false;
            this.xrTableCell52.StylePriority.UsePadding = false;
            this.xrTableCell52.StylePriority.UseTextAlignment = false;
            this.xrTableCell52.Text = "xrTableCell8";
            this.xrTableCell52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell52.Weight = 0.28644809449300485D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell53.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfileEmail]")});
            this.xrTableCell53.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell53.Multiline = true;
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell53.StylePriority.UseBorders = false;
            this.xrTableCell53.StylePriority.UseFont = false;
            this.xrTableCell53.StylePriority.UsePadding = false;
            this.xrTableCell53.StylePriority.UseTextAlignment = false;
            this.xrTableCell53.Text = "xrTableCell12";
            this.xrTableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell53.Weight = 0.34991695051763344D;
            // 
            // xrTableCell54
            // 
            this.xrTableCell54.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell54.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfileAddress]")});
            this.xrTableCell54.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell54.Name = "xrTableCell54";
            this.xrTableCell54.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell54.StylePriority.UseBorders = false;
            this.xrTableCell54.StylePriority.UseFont = false;
            this.xrTableCell54.StylePriority.UsePadding = false;
            this.xrTableCell54.StylePriority.UseTextAlignment = false;
            this.xrTableCell54.Text = "xrTableCell9";
            this.xrTableCell54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell54.Weight = 0.45366519559331414D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell55.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PositionName]")});
            this.xrTableCell55.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell55.StylePriority.UseBorders = false;
            this.xrTableCell55.StylePriority.UseFont = false;
            this.xrTableCell55.StylePriority.UsePadding = false;
            this.xrTableCell55.StylePriority.UseTextAlignment = false;
            this.xrTableCell55.Text = "xrTableCell10";
            this.xrTableCell55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell55.Weight = 0.28004210229109355D;
            // 
            // ReportHeader2
            // 
            this.ReportHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel27});
            this.ReportHeader2.HeightF = 36.23019F;
            this.ReportHeader2.Name = "ReportHeader2";
            // 
            // xrLabel27
            // 
            this.xrLabel27.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(0F, 13.23019F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(241.9716F, 23F);
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.StylePriority.UseTextAlignment = false;
            this.xrLabel27.Text = "THÔNG TIN NHÀ THẦU";
            this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader5
            // 
            this.GroupHeader5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
            this.GroupHeader5.HeightF = 25F;
            this.GroupHeader5.Name = "GroupHeader5";
            this.GroupHeader5.RepeatEveryPage = true;
            // 
            // xrTable6
            // 
            this.xrTable6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable6.Name = "xrTable6";
            this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow10});
            this.xrTable6.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            this.xrTable6.StylePriority.UseFont = false;
            this.xrTable6.StylePriority.UseTextAlignment = false;
            this.xrTable6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell42,
            this.xrTableCell43,
            this.xrTableCell44,
            this.xrTableCell45,
            this.xrTableCell46,
            this.xrTableCell47,
            this.xrTableCell48});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow10.StylePriority.UseBorders = false;
            this.xrTableRow10.StylePriority.UsePadding = false;
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StylePriority.UseBorders = false;
            this.xrTableCell42.Text = "STT";
            this.xrTableCell42.Weight = 0.36109905065115067D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StylePriority.UseBorders = false;
            this.xrTableCell43.Text = "Tên nhà thầu";
            this.xrTableCell43.Weight = 1.0699593334026982D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.StylePriority.UseBorders = false;
            this.xrTableCell44.Text = "Tên liên hệ";
            this.xrTableCell44.Weight = 0.98202023025191509D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell45.Multiline = true;
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.StylePriority.UseBorders = false;
            this.xrTableCell45.Text = "SĐT";
            this.xrTableCell45.Weight = 0.86704953283762853D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.StylePriority.UseBorders = false;
            this.xrTableCell46.Text = "Email";
            this.xrTableCell46.Weight = 1.0591637575134598D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.StylePriority.UseBorders = false;
            this.xrTableCell47.Text = "Địa chỉ";
            this.xrTableCell47.Weight = 1.3731989708437828D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell48.Multiline = true;
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.StylePriority.UseBorders = false;
            this.xrTableCell48.Text = "Chức vụ";
            this.xrTableCell48.Weight = 0.84765878873239153D;
            // 
            // DetailReport4
            // 
            this.DetailReport4.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail5,
            this.ReportHeader3,
            this.GroupHeader6});
            this.DetailReport4.DataMember = "DoanhSo";
            this.DetailReport4.DataSource = this.dsCustomerProfile1;
            this.DetailReport4.Level = 4;
            this.DetailReport4.Name = "DetailReport4";
            // 
            // Detail5
            // 
            this.Detail5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable9});
            this.Detail5.HeightF = 25F;
            this.Detail5.KeepTogether = true;
            this.Detail5.Name = "Detail5";
            // 
            // xrTable9
            // 
            this.xrTable9.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable9.Name = "xrTable9";
            this.xrTable9.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow13});
            this.xrTable9.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell61,
            this.xrTableCell62,
            this.xrTableCell63,
            this.xrTableCell66,
            this.xrTableCell67});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow13.StylePriority.UseBorders = false;
            this.xrTableRow13.StylePriority.UsePadding = false;
            this.xrTableRow13.StylePriority.UseTextAlignment = false;
            this.xrTableRow13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow13.Weight = 11.5D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell61.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[STT]")});
            this.xrTableCell61.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell61.StylePriority.UseBorders = false;
            this.xrTableCell61.StylePriority.UseFont = false;
            this.xrTableCell61.StylePriority.UsePadding = false;
            this.xrTableCell61.StylePriority.UseTextAlignment = false;
            xrSummary4.IgnoreNullValues = true;
            this.xrTableCell61.Summary = xrSummary4;
            this.xrTableCell61.Text = "1";
            this.xrTableCell61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell61.Weight = 0.11929668256097906D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell62.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Year]")});
            this.xrTableCell62.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell62.StylePriority.UseBorders = false;
            this.xrTableCell62.StylePriority.UseFont = false;
            this.xrTableCell62.StylePriority.UsePadding = false;
            this.xrTableCell62.StylePriority.UseTextAlignment = false;
            this.xrTableCell62.Text = "2019";
            this.xrTableCell62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell62.Weight = 0.3534836197909465D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell63.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([DoanhSo])")});
            this.xrTableCell63.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell63.Multiline = true;
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell63.StylePriority.UseBorders = false;
            this.xrTableCell63.StylePriority.UseFont = false;
            this.xrTableCell63.StylePriority.UsePadding = false;
            this.xrTableCell63.StylePriority.UseTextAlignment = false;
            this.xrTableCell63.Text = "xrTableCell41";
            this.xrTableCell63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell63.TextFormatString = "{0:n0}";
            this.xrTableCell63.Weight = 0.51120332296194337D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell66.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([ThanhToan])")});
            this.xrTableCell66.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell66.StylePriority.UseBorders = false;
            this.xrTableCell66.StylePriority.UseFont = false;
            this.xrTableCell66.StylePriority.UsePadding = false;
            this.xrTableCell66.StylePriority.UseTextAlignment = false;
            this.xrTableCell66.Text = "xrTableCell9";
            this.xrTableCell66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell66.TextFormatString = "{0:n0}";
            this.xrTableCell66.Weight = 0.55657909185562882D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell67.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([ConLai])")});
            this.xrTableCell67.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell67.StylePriority.UseBorders = false;
            this.xrTableCell67.StylePriority.UseFont = false;
            this.xrTableCell67.StylePriority.UsePadding = false;
            this.xrTableCell67.StylePriority.UseTextAlignment = false;
            this.xrTableCell67.Text = "xrTableCell10";
            this.xrTableCell67.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell67.TextFormatString = "{0:n0}";
            this.xrTableCell67.Weight = 0.6267211520436039D;
            // 
            // ReportHeader3
            // 
            this.ReportHeader3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel55,
            this.xrLabel28});
            this.ReportHeader3.HeightF = 35.55163F;
            this.ReportHeader3.KeepTogether = true;
            this.ReportHeader3.Name = "ReportHeader3";
            // 
            // xrLabel55
            // 
            this.xrLabel55.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(691.8915F, 12.55163F);
            this.xrLabel55.Name = "xrLabel55";
            this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel55.SizeF = new System.Drawing.SizeF(50.10864F, 23F);
            this.xrLabel55.StylePriority.UseFont = false;
            this.xrLabel55.StylePriority.UseTextAlignment = false;
            this.xrLabel55.Text = "VNĐ";
            this.xrLabel55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel28
            // 
            this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(0F, 12.55163F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(462.4107F, 23F);
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "DOANH SỐ MUA HÀNG – THANH TOÁN";
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader6
            // 
            this.GroupHeader6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable22});
            this.GroupHeader6.HeightF = 25F;
            this.GroupHeader6.KeepTogether = true;
            this.GroupHeader6.Name = "GroupHeader6";
            this.GroupHeader6.RepeatEveryPage = true;
            // 
            // xrTable22
            // 
            this.xrTable22.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
            this.xrTable22.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable22.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable22.Name = "xrTable22";
            this.xrTable22.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5});
            this.xrTable22.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            this.xrTable22.StylePriority.UseBorders = false;
            this.xrTable22.StylePriority.UseFont = false;
            this.xrTable22.StylePriority.UseTextAlignment = false;
            this.xrTable22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21,
            this.xrTableCell70,
            this.xrTableCell71});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow5.StylePriority.UseBorders = false;
            this.xrTableRow5.StylePriority.UsePadding = false;
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseBorders = false;
            this.xrTableCell19.Text = "STT";
            this.xrTableCell19.Weight = 0.36109905065115067D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseBorders = false;
            this.xrTableCell20.Text = "Năm";
            this.xrTableCell20.Weight = 1.0699593334026982D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseBorders = false;
            this.xrTableCell21.Text = "Doanh số mua";
            this.xrTableCell21.Weight = 1.5473608117982782D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell70.Multiline = true;
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.StylePriority.UseBorders = false;
            this.xrTableCell70.Text = "Thanh toán";
            this.xrTableCell70.Weight = 1.6847087767401505D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.StylePriority.UseBorders = false;
            this.xrTableCell71.Text = "Còn lại";
            this.xrTableCell71.Weight = 1.8970216916407487D;
            // 
            // DetailReport5
            // 
            this.DetailReport5.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail6,
            this.ReportHeader4,
            this.ReportFooter,
            this.GroupHeader7});
            this.DetailReport5.DataMember = "MuaHang";
            this.DetailReport5.DataSource = this.dsCustomerProfile1;
            this.DetailReport5.Level = 5;
            this.DetailReport5.Name = "DetailReport5";
            // 
            // Detail6
            // 
            this.Detail6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable11});
            this.Detail6.HeightF = 25F;
            this.Detail6.KeepTogether = true;
            this.Detail6.Name = "Detail6";
            // 
            // xrTable11
            // 
            this.xrTable11.LocationFloat = new DevExpress.Utils.PointFloat(9.999695F, 0F);
            this.xrTable11.Name = "xrTable11";
            this.xrTable11.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow19});
            this.xrTable11.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            // 
            // xrTableRow19
            // 
            this.xrTableRow19.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell82,
            this.xrTableCell84,
            this.xrTableCell85,
            this.xrTableCell92,
            this.xrTableCell95,
            this.xrTableCell96});
            this.xrTableRow19.Name = "xrTableRow19";
            this.xrTableRow19.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow19.StylePriority.UseBorders = false;
            this.xrTableRow19.StylePriority.UsePadding = false;
            this.xrTableRow19.StylePriority.UseTextAlignment = false;
            this.xrTableRow19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow19.Weight = 11.5D;
            // 
            // xrTableCell82
            // 
            this.xrTableCell82.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell82.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[STT]")});
            this.xrTableCell82.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell82.Name = "xrTableCell82";
            this.xrTableCell82.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell82.StylePriority.UseBorders = false;
            this.xrTableCell82.StylePriority.UseFont = false;
            this.xrTableCell82.StylePriority.UsePadding = false;
            this.xrTableCell82.StylePriority.UseTextAlignment = false;
            xrSummary5.IgnoreNullValues = true;
            this.xrTableCell82.Summary = xrSummary5;
            this.xrTableCell82.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell82.Weight = 0.11929668256097906D;
            // 
            // xrTableCell84
            // 
            this.xrTableCell84.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell84.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[GroupProduct]")});
            this.xrTableCell84.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell84.Name = "xrTableCell84";
            this.xrTableCell84.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell84.StylePriority.UseBorders = false;
            this.xrTableCell84.StylePriority.UseFont = false;
            this.xrTableCell84.StylePriority.UsePadding = false;
            this.xrTableCell84.StylePriority.UseTextAlignment = false;
            this.xrTableCell84.Text = "xrTableCell7";
            this.xrTableCell84.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell84.Weight = 0.40607005959815906D;
            // 
            // xrTableCell85
            // 
            this.xrTableCell85.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell85.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([Year1])")});
            this.xrTableCell85.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell85.Multiline = true;
            this.xrTableCell85.Name = "xrTableCell85";
            this.xrTableCell85.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell85.StylePriority.UseBorders = false;
            this.xrTableCell85.StylePriority.UseFont = false;
            this.xrTableCell85.StylePriority.UsePadding = false;
            this.xrTableCell85.StylePriority.UseTextAlignment = false;
            this.xrTableCell85.Text = "xrTableCell41";
            this.xrTableCell85.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell85.TextFormatString = "{0:n0}";
            this.xrTableCell85.Weight = 0.37314832734463421D;
            // 
            // xrTableCell92
            // 
            this.xrTableCell92.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell92.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([Year2])")});
            this.xrTableCell92.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell92.Multiline = true;
            this.xrTableCell92.Name = "xrTableCell92";
            this.xrTableCell92.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell92.StylePriority.UseBorders = false;
            this.xrTableCell92.StylePriority.UseFont = false;
            this.xrTableCell92.StylePriority.UsePadding = false;
            this.xrTableCell92.StylePriority.UseTextAlignment = false;
            this.xrTableCell92.Text = "xrTableCell12";
            this.xrTableCell92.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell92.TextFormatString = "{0:n0}";
            this.xrTableCell92.Weight = 0.44096923010105438D;
            // 
            // xrTableCell95
            // 
            this.xrTableCell95.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell95.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([Year3])")});
            this.xrTableCell95.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell95.Name = "xrTableCell95";
            this.xrTableCell95.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell95.StylePriority.UseBorders = false;
            this.xrTableCell95.StylePriority.UseFont = false;
            this.xrTableCell95.StylePriority.UsePadding = false;
            this.xrTableCell95.StylePriority.UseTextAlignment = false;
            this.xrTableCell95.Text = "xrTableCell9";
            this.xrTableCell95.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell95.TextFormatString = "{0:n0}";
            this.xrTableCell95.Weight = 0.43270148931479729D;
            // 
            // xrTableCell96
            // 
            this.xrTableCell96.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell96.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([Total])")});
            this.xrTableCell96.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell96.Name = "xrTableCell96";
            this.xrTableCell96.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell96.StylePriority.UseBorders = false;
            this.xrTableCell96.StylePriority.UseFont = false;
            this.xrTableCell96.StylePriority.UsePadding = false;
            this.xrTableCell96.StylePriority.UseTextAlignment = false;
            this.xrTableCell96.Text = "xrTableCell10";
            this.xrTableCell96.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell96.TextFormatString = "{0:n0}";
            this.xrTableCell96.Weight = 0.39509785860100127D;
            // 
            // ReportHeader4
            // 
            this.ReportHeader4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel56,
            this.xrLabel29});
            this.ReportHeader4.HeightF = 34.79869F;
            this.ReportHeader4.Name = "ReportHeader4";
            // 
            // xrLabel56
            // 
            this.xrLabel56.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(691.8915F, 11.79869F);
            this.xrLabel56.Name = "xrLabel56";
            this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel56.SizeF = new System.Drawing.SizeF(50.10864F, 23F);
            this.xrLabel56.StylePriority.UseFont = false;
            this.xrLabel56.StylePriority.UseTextAlignment = false;
            this.xrLabel56.Text = "VNĐ";
            this.xrLabel56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel29
            // 
            this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(0F, 11.79868F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(241.9716F, 23F);
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "CHI TIẾT MUA HÀNG";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable12});
            this.ReportFooter.HeightF = 25F;
            this.ReportFooter.KeepTogether = true;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrTable12
            // 
            this.xrTable12.LocationFloat = new DevExpress.Utils.PointFloat(9.999878F, 0F);
            this.xrTable12.Name = "xrTable12";
            this.xrTable12.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow20});
            this.xrTable12.SizeF = new System.Drawing.SizeF(731.9997F, 25F);
            // 
            // xrTableRow20
            // 
            this.xrTableRow20.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow20.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell97,
            this.xrTableCell98,
            this.xrTableCell99,
            this.xrTableCell100,
            this.xrTableCell101});
            this.xrTableRow20.Name = "xrTableRow20";
            this.xrTableRow20.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow20.StylePriority.UseBorders = false;
            this.xrTableRow20.StylePriority.UsePadding = false;
            this.xrTableRow20.StylePriority.UseTextAlignment = false;
            this.xrTableRow20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow20.Weight = 11.5D;
            // 
            // xrTableCell97
            // 
            this.xrTableCell97.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell97.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.xrTableCell97.Name = "xrTableCell97";
            this.xrTableCell97.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell97.StylePriority.UseBorders = false;
            this.xrTableCell97.StylePriority.UseFont = false;
            this.xrTableCell97.StylePriority.UsePadding = false;
            this.xrTableCell97.StylePriority.UseTextAlignment = false;
            this.xrTableCell97.Text = "Cộng";
            this.xrTableCell97.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell97.Weight = 0.525365883642385D;
            // 
            // xrTableCell98
            // 
            this.xrTableCell98.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell98.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Sum(ToDecimal([Year1]))")});
            this.xrTableCell98.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell98.Multiline = true;
            this.xrTableCell98.Name = "xrTableCell98";
            this.xrTableCell98.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell98.StylePriority.UseBorders = false;
            this.xrTableCell98.StylePriority.UseFont = false;
            this.xrTableCell98.StylePriority.UsePadding = false;
            this.xrTableCell98.StylePriority.UseTextAlignment = false;
            this.xrTableCell98.Text = "xrTableCell41";
            this.xrTableCell98.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell98.TextFormatString = "{0:n0}";
            this.xrTableCell98.Weight = 0.37314832734463421D;
            // 
            // xrTableCell99
            // 
            this.xrTableCell99.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell99.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Sum(ToDecimal([Year2]))")});
            this.xrTableCell99.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell99.Multiline = true;
            this.xrTableCell99.Name = "xrTableCell99";
            this.xrTableCell99.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell99.StylePriority.UseBorders = false;
            this.xrTableCell99.StylePriority.UseFont = false;
            this.xrTableCell99.StylePriority.UsePadding = false;
            this.xrTableCell99.StylePriority.UseTextAlignment = false;
            this.xrTableCell99.Text = "xrTableCell12";
            this.xrTableCell99.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell99.TextFormatString = "{0:n0}";
            this.xrTableCell99.Weight = 0.44096923010105438D;
            // 
            // xrTableCell100
            // 
            this.xrTableCell100.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell100.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Sum(ToDecimal([Year3]))")});
            this.xrTableCell100.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell100.Name = "xrTableCell100";
            this.xrTableCell100.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell100.StylePriority.UseBorders = false;
            this.xrTableCell100.StylePriority.UseFont = false;
            this.xrTableCell100.StylePriority.UsePadding = false;
            this.xrTableCell100.StylePriority.UseTextAlignment = false;
            this.xrTableCell100.Text = "xrTableCell9";
            this.xrTableCell100.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell100.TextFormatString = "{0:n0}";
            this.xrTableCell100.Weight = 0.43270148931479729D;
            // 
            // xrTableCell101
            // 
            this.xrTableCell101.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell101.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Sum(ToDecimal([Total]))")});
            this.xrTableCell101.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell101.Name = "xrTableCell101";
            this.xrTableCell101.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell101.StylePriority.UseBorders = false;
            this.xrTableCell101.StylePriority.UseFont = false;
            this.xrTableCell101.StylePriority.UsePadding = false;
            this.xrTableCell101.StylePriority.UseTextAlignment = false;
            this.xrTableCell101.Text = "xrTableCell10";
            this.xrTableCell101.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell101.TextFormatString = "{0:n0}";
            this.xrTableCell101.Weight = 0.39509785860100127D;
            // 
            // GroupHeader7
            // 
            this.GroupHeader7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable10});
            this.GroupHeader7.HeightF = 50F;
            this.GroupHeader7.KeepTogether = true;
            this.GroupHeader7.Name = "GroupHeader7";
            this.GroupHeader7.RepeatEveryPage = true;
            // 
            // xrTable10
            // 
            this.xrTable10.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable10.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable10.Name = "xrTable10";
            this.xrTable10.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow17,
            this.xrTableRow18});
            this.xrTable10.SizeF = new System.Drawing.SizeF(731.9999F, 50F);
            this.xrTable10.StylePriority.UseFont = false;
            this.xrTable10.StylePriority.UseTextAlignment = false;
            this.xrTable10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow17
            // 
            this.xrTableRow17.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell64,
            this.xrTableCell25,
            this.xrTableCell83,
            this.xrTableCell87});
            this.xrTableRow17.Name = "xrTableRow17";
            this.xrTableRow17.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow17.StylePriority.UseBorders = false;
            this.xrTableRow17.StylePriority.UsePadding = false;
            this.xrTableRow17.Weight = 1D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell64.Multiline = true;
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.RowSpan = 2;
            this.xrTableCell64.StylePriority.UseBorders = false;
            this.xrTableCell64.StylePriority.UseTextAlignment = false;
            this.xrTableCell64.Text = "STT";
            this.xrTableCell64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell64.Weight = 0.36109884597794034D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell25.Multiline = true;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.RowSpan = 2;
            this.xrTableCell25.StylePriority.UseBorders = false;
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.Text = "Nhóm sản phẩm";
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell25.Weight = 1.2291333972782215D;
            // 
            // xrTableCell83
            // 
            this.xrTableCell83.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell83.Name = "xrTableCell83";
            this.xrTableCell83.StylePriority.UseBorders = false;
            this.xrTableCell83.Text = "DOANH SỐ MUA";
            this.xrTableCell83.Weight = 3.773996387752296D;
            // 
            // xrTableCell87
            // 
            this.xrTableCell87.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell87.Multiline = true;
            this.xrTableCell87.Name = "xrTableCell87";
            this.xrTableCell87.RowSpan = 2;
            this.xrTableCell87.StylePriority.UseBorders = false;
            this.xrTableCell87.StylePriority.UseTextAlignment = false;
            this.xrTableCell87.Text = "Tổng";
            this.xrTableCell87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell87.Weight = 1.1959210332245687D;
            // 
            // xrTableRow18
            // 
            this.xrTableRow18.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell88,
            this.xrTableCell89,
            this.xrTableCell90,
            this.xrTableCell91,
            this.xrTableCell93,
            this.xrTableCell94});
            this.xrTableRow18.Name = "xrTableRow18";
            this.xrTableRow18.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow18.StylePriority.UseBorders = false;
            this.xrTableRow18.StylePriority.UsePadding = false;
            this.xrTableRow18.Weight = 1D;
            // 
            // xrTableCell88
            // 
            this.xrTableCell88.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell88.Multiline = true;
            this.xrTableCell88.Name = "xrTableCell88";
            this.xrTableCell88.RowSpan = 0;
            this.xrTableCell88.StylePriority.UseBorders = false;
            this.xrTableCell88.Text = "STT";
            this.xrTableCell88.Weight = 0.36109905065115067D;
            // 
            // xrTableCell89
            // 
            this.xrTableCell89.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell89.Multiline = true;
            this.xrTableCell89.Name = "xrTableCell89";
            this.xrTableCell89.StylePriority.UseBorders = false;
            this.xrTableCell89.Text = "Nhóm sản phẩm";
            this.xrTableCell89.Weight = 1.2291331926050109D;
            // 
            // xrTableCell90
            // 
            this.xrTableCell90.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell90.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([Year])-2")});
            this.xrTableCell90.Multiline = true;
            this.xrTableCell90.Name = "xrTableCell90";
            this.xrTableCell90.StylePriority.UseBorders = false;
            this.xrTableCell90.Text = "Năm 2019";
            this.xrTableCell90.Weight = 1.1294817257594545D;
            // 
            // xrTableCell91
            // 
            this.xrTableCell91.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell91.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToInt([Year]) - 1")});
            this.xrTableCell91.Multiline = true;
            this.xrTableCell91.Name = "xrTableCell91";
            this.xrTableCell91.StylePriority.UseBorders = false;
            this.xrTableCell91.Text = "Năm 2020";
            this.xrTableCell91.Weight = 1.3347705852458618D;
            // 
            // xrTableCell93
            // 
            this.xrTableCell93.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell93.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToInt([Year])")});
            this.xrTableCell93.Multiline = true;
            this.xrTableCell93.Name = "xrTableCell93";
            this.xrTableCell93.StylePriority.UseBorders = false;
            this.xrTableCell93.Text = "Năm 2021";
            this.xrTableCell93.Weight = 1.309742572512659D;
            // 
            // xrTableCell94
            // 
            this.xrTableCell94.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell94.Multiline = true;
            this.xrTableCell94.Name = "xrTableCell94";
            this.xrTableCell94.StylePriority.UseBorders = false;
            this.xrTableCell94.Text = "Tổng";
            this.xrTableCell94.Weight = 1.1959225374588898D;
            // 
            // DetailReport6
            // 
            this.DetailReport6.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail7,
            this.ReportFooter1,
            this.ReportHeader6,
            this.GroupHeader8});
            this.DetailReport6.DataMember = "Spons";
            this.DetailReport6.DataSource = this.dsCustomerProfile1;
            this.DetailReport6.Level = 6;
            this.DetailReport6.Name = "DetailReport6";
            // 
            // Detail7
            // 
            this.Detail7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable13});
            this.Detail7.HeightF = 25F;
            this.Detail7.Name = "Detail7";
            // 
            // xrTable13
            // 
            this.xrTable13.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable13.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable13.Name = "xrTable13";
            this.xrTable13.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow25});
            this.xrTable13.SizeF = new System.Drawing.SizeF(732.0001F, 25F);
            this.xrTable13.StylePriority.UseFont = false;
            this.xrTable13.StylePriority.UseTextAlignment = false;
            this.xrTable13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow25
            // 
            this.xrTableRow25.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow25.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell122,
            this.xrTableCell123,
            this.xrTableCell124,
            this.xrTableCell125,
            this.xrTableCell126});
            this.xrTableRow25.Name = "xrTableRow25";
            this.xrTableRow25.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow25.StylePriority.UseBorders = false;
            this.xrTableRow25.StylePriority.UsePadding = false;
            this.xrTableRow25.Weight = 1D;
            // 
            // xrTableCell122
            // 
            this.xrTableCell122.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell122.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[STT]")});
            this.xrTableCell122.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell122.Multiline = true;
            this.xrTableCell122.Name = "xrTableCell122";
            this.xrTableCell122.StylePriority.UseBorders = false;
            this.xrTableCell122.StylePriority.UseFont = false;
            this.xrTableCell122.Weight = 0.36109905065115067D;
            // 
            // xrTableCell123
            // 
            this.xrTableCell123.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell123.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Product]")});
            this.xrTableCell123.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell123.Multiline = true;
            this.xrTableCell123.Name = "xrTableCell123";
            this.xrTableCell123.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 3, 3, 100F);
            this.xrTableCell123.StylePriority.UseBorders = false;
            this.xrTableCell123.StylePriority.UseFont = false;
            this.xrTableCell123.StylePriority.UsePadding = false;
            this.xrTableCell123.StylePriority.UseTextAlignment = false;
            this.xrTableCell123.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell123.Weight = 1.0699593334026982D;
            // 
            // xrTableCell124
            // 
            this.xrTableCell124.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell124.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([Value])")});
            this.xrTableCell124.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell124.Multiline = true;
            this.xrTableCell124.Name = "xrTableCell124";
            this.xrTableCell124.StylePriority.UseBorders = false;
            this.xrTableCell124.StylePriority.UseFont = false;
            this.xrTableCell124.StylePriority.UseTextAlignment = false;
            this.xrTableCell124.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell124.TextFormatString = "{0:n0}";
            this.xrTableCell124.Weight = 1.5473608117982782D;
            // 
            // xrTableCell125
            // 
            this.xrTableCell125.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell125.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([ThuHoi])")});
            this.xrTableCell125.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell125.Multiline = true;
            this.xrTableCell125.Name = "xrTableCell125";
            this.xrTableCell125.StylePriority.UseBorders = false;
            this.xrTableCell125.StylePriority.UseFont = false;
            this.xrTableCell125.StylePriority.UseTextAlignment = false;
            this.xrTableCell125.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell125.TextFormatString = "{0:n0}";
            this.xrTableCell125.Weight = 1.6847087767401505D;
            // 
            // xrTableCell126
            // 
            this.xrTableCell126.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell126.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([Spons])")});
            this.xrTableCell126.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell126.Multiline = true;
            this.xrTableCell126.Name = "xrTableCell126";
            this.xrTableCell126.StylePriority.UseBorders = false;
            this.xrTableCell126.StylePriority.UseFont = false;
            this.xrTableCell126.StylePriority.UseTextAlignment = false;
            this.xrTableCell126.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell126.TextFormatString = "{0:n0}";
            this.xrTableCell126.Weight = 1.8970216916407487D;
            // 
            // ReportFooter1
            // 
            this.ReportFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable16});
            this.ReportFooter1.HeightF = 25F;
            this.ReportFooter1.Name = "ReportFooter1";
            // 
            // xrTable16
            // 
            this.xrTable16.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable16.Name = "xrTable16";
            this.xrTable16.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow26});
            this.xrTable16.SizeF = new System.Drawing.SizeF(732.0001F, 25F);
            // 
            // xrTableRow26
            // 
            this.xrTableRow26.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow26.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell127,
            this.xrTableCell128,
            this.xrTableCell129,
            this.xrTableCell130,
            this.xrTableCell131});
            this.xrTableRow26.Name = "xrTableRow26";
            this.xrTableRow26.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow26.StylePriority.UseBorders = false;
            this.xrTableRow26.StylePriority.UsePadding = false;
            this.xrTableRow26.StylePriority.UseTextAlignment = false;
            this.xrTableRow26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow26.Weight = 11.5D;
            // 
            // xrTableCell127
            // 
            this.xrTableCell127.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrTableCell127.Multiline = true;
            this.xrTableCell127.Name = "xrTableCell127";
            this.xrTableCell127.StylePriority.UseFont = false;
            this.xrTableCell127.StylePriority.UseTextAlignment = false;
            this.xrTableCell127.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell127.Weight = 0.11929668256097906D;
            // 
            // xrTableCell128
            // 
            this.xrTableCell128.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.xrTableCell128.Multiline = true;
            this.xrTableCell128.Name = "xrTableCell128";
            this.xrTableCell128.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 3, 3, 100F);
            this.xrTableCell128.StylePriority.UseFont = false;
            this.xrTableCell128.StylePriority.UsePadding = false;
            this.xrTableCell128.StylePriority.UseTextAlignment = false;
            this.xrTableCell128.Text = "Cộng";
            this.xrTableCell128.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell128.Weight = 0.3534836197909465D;
            // 
            // xrTableCell129
            // 
            this.xrTableCell129.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Sum(ToDecimal([Value]))")});
            this.xrTableCell129.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell129.Multiline = true;
            this.xrTableCell129.Name = "xrTableCell129";
            this.xrTableCell129.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 3, 100F);
            this.xrTableCell129.StylePriority.UseFont = false;
            this.xrTableCell129.StylePriority.UsePadding = false;
            this.xrTableCell129.StylePriority.UseTextAlignment = false;
            this.xrTableCell129.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell129.TextFormatString = "{0:n0}";
            this.xrTableCell129.Weight = 0.51120332296194337D;
            // 
            // xrTableCell130
            // 
            this.xrTableCell130.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Sum(ToDecimal([ThuHoi]))")});
            this.xrTableCell130.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell130.Multiline = true;
            this.xrTableCell130.Name = "xrTableCell130";
            this.xrTableCell130.StylePriority.UseFont = false;
            this.xrTableCell130.StylePriority.UseTextAlignment = false;
            this.xrTableCell130.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell130.TextFormatString = "{0:n0}";
            this.xrTableCell130.Weight = 0.55657909185562882D;
            // 
            // xrTableCell131
            // 
            this.xrTableCell131.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell131.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Sum(ToDecimal([Spons]))")});
            this.xrTableCell131.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell131.Multiline = true;
            this.xrTableCell131.Name = "xrTableCell131";
            this.xrTableCell131.StylePriority.UseBorders = false;
            this.xrTableCell131.StylePriority.UseFont = false;
            this.xrTableCell131.StylePriority.UseTextAlignment = false;
            this.xrTableCell131.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell131.TextFormatString = "{0:n0}";
            this.xrTableCell131.Weight = 0.6267211520436039D;
            // 
            // ReportHeader6
            // 
            this.ReportHeader6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel30});
            this.ReportHeader6.HeightF = 34.20338F;
            this.ReportHeader6.Name = "ReportHeader6";
            // 
            // xrLabel30
            // 
            this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(0F, 11.20338F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(241.9716F, 23F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.Text = "GIÁ TRỊ AN CƯỜNG TÀI TRỢ";
            this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader8
            // 
            this.GroupHeader8.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable17});
            this.GroupHeader8.HeightF = 25F;
            this.GroupHeader8.Name = "GroupHeader8";
            this.GroupHeader8.RepeatEveryPage = true;
            // 
            // xrTable17
            // 
            this.xrTable17.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable17.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable17.Name = "xrTable17";
            this.xrTable17.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow28});
            this.xrTable17.SizeF = new System.Drawing.SizeF(732.0001F, 25F);
            this.xrTable17.StylePriority.UseFont = false;
            this.xrTable17.StylePriority.UseTextAlignment = false;
            this.xrTable17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow28
            // 
            this.xrTableRow28.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow28.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell137,
            this.xrTableCell138,
            this.xrTableCell139,
            this.xrTableCell140,
            this.xrTableCell141});
            this.xrTableRow28.Name = "xrTableRow28";
            this.xrTableRow28.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow28.StylePriority.UseBorders = false;
            this.xrTableRow28.StylePriority.UsePadding = false;
            this.xrTableRow28.Weight = 1D;
            // 
            // xrTableCell137
            // 
            this.xrTableCell137.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell137.Name = "xrTableCell137";
            this.xrTableCell137.StylePriority.UseBorders = false;
            this.xrTableCell137.Text = "STT";
            this.xrTableCell137.Weight = 0.36109905065115067D;
            // 
            // xrTableCell138
            // 
            this.xrTableCell138.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell138.Name = "xrTableCell138";
            this.xrTableCell138.StylePriority.UseBorders = false;
            this.xrTableCell138.Text = "Sản phẩm";
            this.xrTableCell138.Weight = 1.0699593334026982D;
            // 
            // xrTableCell139
            // 
            this.xrTableCell139.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell139.Name = "xrTableCell139";
            this.xrTableCell139.StylePriority.UseBorders = false;
            this.xrTableCell139.Text = "Tổng giá trị tài trợ";
            this.xrTableCell139.Weight = 1.5473608117982782D;
            // 
            // xrTableCell140
            // 
            this.xrTableCell140.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell140.Multiline = true;
            this.xrTableCell140.Name = "xrTableCell140";
            this.xrTableCell140.StylePriority.UseBorders = false;
            this.xrTableCell140.Text = "Thu hồi lại";
            this.xrTableCell140.Weight = 1.6847087767401505D;
            // 
            // xrTableCell141
            // 
            this.xrTableCell141.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell141.Name = "xrTableCell141";
            this.xrTableCell141.StylePriority.UseBorders = false;
            this.xrTableCell141.Text = "Chi phí đã tài trợ";
            this.xrTableCell141.Weight = 1.8970216916407487D;
            // 
            // DetailReport7
            // 
            this.DetailReport7.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail8,
            this.ReportHeader5,
            this.GroupHeader9});
            this.DetailReport7.DataMember = "ShowroomGTB";
            this.DetailReport7.DataSource = this.dsCustomerProfile1;
            this.DetailReport7.Level = 7;
            this.DetailReport7.Name = "DetailReport7";
            // 
            // Detail8
            // 
            this.Detail8.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable15});
            this.Detail8.HeightF = 25F;
            this.Detail8.Name = "Detail8";
            // 
            // xrTable15
            // 
            this.xrTable15.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable15.Name = "xrTable15";
            this.xrTable15.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow31});
            this.xrTable15.SizeF = new System.Drawing.SizeF(732.0001F, 25F);
            // 
            // xrTableRow31
            // 
            this.xrTableRow31.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow31.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell152,
            this.xrTableCell153,
            this.xrTableCell154,
            this.xrTableCell155,
            this.xrTableCell156});
            this.xrTableRow31.Name = "xrTableRow31";
            this.xrTableRow31.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow31.StylePriority.UseBorders = false;
            this.xrTableRow31.StylePriority.UsePadding = false;
            this.xrTableRow31.StylePriority.UseTextAlignment = false;
            this.xrTableRow31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow31.Weight = 11.5D;
            // 
            // xrTableCell152
            // 
            this.xrTableCell152.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell152.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DataSource.CurrentRowIndex] + 1")});
            this.xrTableCell152.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell152.Name = "xrTableCell152";
            this.xrTableCell152.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell152.StylePriority.UseBorders = false;
            this.xrTableCell152.StylePriority.UseFont = false;
            this.xrTableCell152.StylePriority.UsePadding = false;
            this.xrTableCell152.StylePriority.UseTextAlignment = false;
            this.xrTableCell152.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell152.Weight = 0.11929668256097906D;
            // 
            // xrTableCell153
            // 
            this.xrTableCell153.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell153.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WorkFlowName]")});
            this.xrTableCell153.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell153.Name = "xrTableCell153";
            this.xrTableCell153.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell153.StylePriority.UseBorders = false;
            this.xrTableCell153.StylePriority.UseFont = false;
            this.xrTableCell153.StylePriority.UsePadding = false;
            this.xrTableCell153.StylePriority.UseTextAlignment = false;
            this.xrTableCell153.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell153.Weight = 0.3534836197909465D;
            // 
            // xrTableCell154
            // 
            this.xrTableCell154.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell154.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfileAddress]")});
            this.xrTableCell154.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell154.Multiline = true;
            this.xrTableCell154.Name = "xrTableCell154";
            this.xrTableCell154.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell154.StylePriority.UseBorders = false;
            this.xrTableCell154.StylePriority.UseFont = false;
            this.xrTableCell154.StylePriority.UsePadding = false;
            this.xrTableCell154.StylePriority.UseTextAlignment = false;
            this.xrTableCell154.Text = "xrTableCell78";
            this.xrTableCell154.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell154.Weight = 0.6549472933938395D;
            // 
            // xrTableCell155
            // 
            this.xrTableCell155.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell155.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StartDate]")});
            this.xrTableCell155.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell155.Multiline = true;
            this.xrTableCell155.Name = "xrTableCell155";
            this.xrTableCell155.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell155.StylePriority.UseBorders = false;
            this.xrTableCell155.StylePriority.UseFont = false;
            this.xrTableCell155.StylePriority.UsePadding = false;
            this.xrTableCell155.StylePriority.UseTextAlignment = false;
            this.xrTableCell155.Text = "xrTableCell79";
            this.xrTableCell155.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell155.TextFormatString = "{0:dd/MM/yyyy}";
            this.xrTableCell155.Weight = 0.47992811463879947D;
            // 
            // xrTableCell156
            // 
            this.xrTableCell156.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell156.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LastDate]")});
            this.xrTableCell156.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell156.Multiline = true;
            this.xrTableCell156.Name = "xrTableCell156";
            this.xrTableCell156.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell156.StylePriority.UseBorders = false;
            this.xrTableCell156.StylePriority.UseFont = false;
            this.xrTableCell156.StylePriority.UsePadding = false;
            this.xrTableCell156.StylePriority.UseTextAlignment = false;
            this.xrTableCell156.Text = "xrTableCell80";
            this.xrTableCell156.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell156.TextFormatString = "{0:dd/MM/yyyy}";
            this.xrTableCell156.Weight = 0.55962815882853711D;
            // 
            // ReportHeader5
            // 
            this.ReportHeader5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel31});
            this.ReportHeader5.HeightF = 35.03773F;
            this.ReportHeader5.Name = "ReportHeader5";
            // 
            // xrLabel31
            // 
            this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(0F, 12.03773F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(298.9161F, 23F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.Text = "THÔNG TIN ĐIỂM TRƯNG BÀY";
            this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader9
            // 
            this.GroupHeader9.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable14});
            this.GroupHeader9.HeightF = 25F;
            this.GroupHeader9.Name = "GroupHeader9";
            this.GroupHeader9.RepeatEveryPage = true;
            // 
            // xrTable14
            // 
            this.xrTable14.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable14.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable14.Name = "xrTable14";
            this.xrTable14.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow27});
            this.xrTable14.SizeF = new System.Drawing.SizeF(732.0001F, 25F);
            this.xrTable14.StylePriority.UseFont = false;
            this.xrTable14.StylePriority.UseTextAlignment = false;
            this.xrTable14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow27
            // 
            this.xrTableRow27.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow27.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell132,
            this.xrTableCell133,
            this.xrTableCell134,
            this.xrTableCell135,
            this.xrTableCell136});
            this.xrTableRow27.Name = "xrTableRow27";
            this.xrTableRow27.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow27.StylePriority.UseBorders = false;
            this.xrTableRow27.StylePriority.UsePadding = false;
            this.xrTableRow27.Weight = 1D;
            // 
            // xrTableCell132
            // 
            this.xrTableCell132.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell132.Name = "xrTableCell132";
            this.xrTableCell132.StylePriority.UseBorders = false;
            this.xrTableCell132.Text = "STT";
            this.xrTableCell132.Weight = 0.36109905065115067D;
            // 
            // xrTableCell133
            // 
            this.xrTableCell133.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell133.Name = "xrTableCell133";
            this.xrTableCell133.StylePriority.UseBorders = false;
            this.xrTableCell133.Text = "Loại";
            this.xrTableCell133.Weight = 1.0699593334026982D;
            // 
            // xrTableCell134
            // 
            this.xrTableCell134.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell134.Name = "xrTableCell134";
            this.xrTableCell134.StylePriority.UseBorders = false;
            this.xrTableCell134.Text = "Địa chỉ ĐTB";
            this.xrTableCell134.Weight = 1.9824593112425175D;
            // 
            // xrTableCell135
            // 
            this.xrTableCell135.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell135.Multiline = true;
            this.xrTableCell135.Name = "xrTableCell135";
            this.xrTableCell135.StylePriority.UseBorders = false;
            this.xrTableCell135.Text = "Ngày lắp";
            this.xrTableCell135.Weight = 1.4526944826842974D;
            // 
            // xrTableCell136
            // 
            this.xrTableCell136.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell136.Name = "xrTableCell136";
            this.xrTableCell136.StylePriority.UseBorders = false;
            this.xrTableCell136.Text = "Ngày chăm sóc gần nhất";
            this.xrTableCell136.Weight = 1.6939374862523626D;
            // 
            // DetailReport8
            // 
            this.DetailReport8.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail9,
            this.ReportHeader7,
            this.GroupHeader10});
            this.DetailReport8.DataMember = "CustomerCare";
            this.DetailReport8.DataSource = this.dsCustomerProfile1;
            this.DetailReport8.Level = 8;
            this.DetailReport8.Name = "DetailReport8";
            // 
            // Detail9
            // 
            this.Detail9.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable19});
            this.Detail9.HeightF = 25F;
            this.Detail9.Name = "Detail9";
            // 
            // xrTable19
            // 
            this.xrTable19.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable19.Name = "xrTable19";
            this.xrTable19.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrTable19.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow22});
            this.xrTable19.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            this.xrTable19.StylePriority.UsePadding = false;
            this.xrTable19.StylePriority.UseTextAlignment = false;
            this.xrTable19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow22
            // 
            this.xrTableRow22.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow22.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell107,
            this.xrTableCell108,
            this.xrTableCell109,
            this.xrTableCell110,
            this.xrTableCell111,
            this.xrTableCell14});
            this.xrTableRow22.Name = "xrTableRow22";
            this.xrTableRow22.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow22.StylePriority.UseBorders = false;
            this.xrTableRow22.StylePriority.UsePadding = false;
            this.xrTableRow22.StylePriority.UseTextAlignment = false;
            this.xrTableRow22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow22.Weight = 11.5D;
            // 
            // xrTableCell107
            // 
            this.xrTableCell107.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell107.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DataSource.CurrentRowIndex] + 1")});
            this.xrTableCell107.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell107.Multiline = true;
            this.xrTableCell107.Name = "xrTableCell107";
            this.xrTableCell107.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell107.StylePriority.UseBorders = false;
            this.xrTableCell107.StylePriority.UseFont = false;
            this.xrTableCell107.StylePriority.UsePadding = false;
            this.xrTableCell107.StylePriority.UseTextAlignment = false;
            this.xrTableCell107.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell107.Weight = 0.16264046251917844D;
            // 
            // xrTableCell108
            // 
            this.xrTableCell108.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell108.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrTableCell108.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell108.Multiline = true;
            this.xrTableCell108.Name = "xrTableCell108";
            this.xrTableCell108.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell108.StylePriority.UseBorders = false;
            this.xrTableCell108.StylePriority.UseFont = false;
            this.xrTableCell108.StylePriority.UsePadding = false;
            this.xrTableCell108.StylePriority.UseTextAlignment = false;
            this.xrTableCell108.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell108.Weight = 0.48191403640999986D;
            // 
            // xrTableCell109
            // 
            this.xrTableCell109.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell109.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Type]")});
            this.xrTableCell109.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell109.Multiline = true;
            this.xrTableCell109.Name = "xrTableCell109";
            this.xrTableCell109.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell109.StylePriority.UseBorders = false;
            this.xrTableCell109.StylePriority.UseFont = false;
            this.xrTableCell109.StylePriority.UsePadding = false;
            this.xrTableCell109.StylePriority.UseTextAlignment = false;
            this.xrTableCell109.Text = "xrTableCell78";
            this.xrTableCell109.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell109.Weight = 0.3942884040777842D;
            // 
            // xrTableCell110
            // 
            this.xrTableCell110.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell110.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EmployeeName]")});
            this.xrTableCell110.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell110.Multiline = true;
            this.xrTableCell110.Name = "xrTableCell110";
            this.xrTableCell110.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell110.StylePriority.UseBorders = false;
            this.xrTableCell110.StylePriority.UseFont = false;
            this.xrTableCell110.StylePriority.UsePadding = false;
            this.xrTableCell110.StylePriority.UseTextAlignment = false;
            this.xrTableCell110.Text = "xrTableCell79";
            this.xrTableCell110.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell110.Weight = 0.63087239849533261D;
            // 
            // xrTableCell111
            // 
            this.xrTableCell111.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell111.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LastDate]")});
            this.xrTableCell111.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell111.Multiline = true;
            this.xrTableCell111.Name = "xrTableCell111";
            this.xrTableCell111.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell111.StylePriority.UseBorders = false;
            this.xrTableCell111.StylePriority.UseFont = false;
            this.xrTableCell111.StylePriority.UsePadding = false;
            this.xrTableCell111.StylePriority.UseTextAlignment = false;
            this.xrTableCell111.Text = "xrTableCell80";
            this.xrTableCell111.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell111.TextFormatString = "{0:dd/MM/yyyy}";
            this.xrTableCell111.Weight = 0.74635439339339082D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([CountDataInYear])")});
            this.xrTableCell14.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell14.Multiline = true;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UsePadding = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell14.TextFormatString = "{0:N0}";
            this.xrTableCell14.Weight = 0.53864866038041082D;
            // 
            // ReportHeader7
            // 
            this.ReportHeader7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel32});
            this.ReportHeader7.HeightF = 33.00002F;
            this.ReportHeader7.Name = "ReportHeader7";
            // 
            // xrLabel32
            // 
            this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00002F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(741.9996F, 23F);
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "LỊCH CHĂM SÓC KHÁCH HÀNG 3 THÁNG GẦN NHẤT";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader10
            // 
            this.GroupHeader10.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable18});
            this.GroupHeader10.HeightF = 25F;
            this.GroupHeader10.Name = "GroupHeader10";
            this.GroupHeader10.RepeatEveryPage = true;
            // 
            // xrTable18
            // 
            this.xrTable18.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable18.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable18.Name = "xrTable18";
            this.xrTable18.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow21});
            this.xrTable18.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            this.xrTable18.StylePriority.UseFont = false;
            this.xrTable18.StylePriority.UseTextAlignment = false;
            this.xrTable18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow21
            // 
            this.xrTableRow21.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow21.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell102,
            this.xrTableCell103,
            this.xrTableCell104,
            this.xrTableCell105,
            this.xrTableCell106,
            this.xrTableCell13});
            this.xrTableRow21.Name = "xrTableRow21";
            this.xrTableRow21.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow21.StylePriority.UseBorders = false;
            this.xrTableRow21.StylePriority.UsePadding = false;
            this.xrTableRow21.Weight = 1D;
            // 
            // xrTableCell102
            // 
            this.xrTableCell102.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell102.Name = "xrTableCell102";
            this.xrTableCell102.StylePriority.UseBorders = false;
            this.xrTableCell102.Text = "STT";
            this.xrTableCell102.Weight = 0.49229634342323991D;
            // 
            // xrTableCell103
            // 
            this.xrTableCell103.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell103.Name = "xrTableCell103";
            this.xrTableCell103.StylePriority.UseBorders = false;
            this.xrTableCell103.Text = "Tên";
            this.xrTableCell103.Weight = 1.45870542610942D;
            // 
            // xrTableCell104
            // 
            this.xrTableCell104.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell104.Name = "xrTableCell104";
            this.xrTableCell104.StylePriority.UseBorders = false;
            this.xrTableCell104.Text = "Phân loại";
            this.xrTableCell104.Weight = 1.1934713817860967D;
            // 
            // xrTableCell105
            // 
            this.xrTableCell105.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell105.Multiline = true;
            this.xrTableCell105.Name = "xrTableCell105";
            this.xrTableCell105.StylePriority.UseBorders = false;
            this.xrTableCell105.Text = "Nhân viên phụ trách";
            this.xrTableCell105.Weight = 1.9095873122070479D;
            // 
            // xrTableCell106
            // 
            this.xrTableCell106.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell106.Name = "xrTableCell106";
            this.xrTableCell106.StylePriority.UseBorders = false;
            this.xrTableCell106.Text = "Ngày chăm sóc gần nhất";
            this.xrTableCell106.Weight = 2.2591420820100261D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell13.Multiline = true;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseBorders = false;
            this.xrTableCell13.Text = "Số lượt trong năm";
            this.xrTableCell13.Weight = 1.6304332110339426D;
            // 
            // DetailReport9
            // 
            this.DetailReport9.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail10,
            this.ReportHeader8,
            this.GroupHeader12});
            this.DetailReport9.DataMember = "CustomerService";
            this.DetailReport9.DataSource = this.dsCustomerProfile1;
            this.DetailReport9.Level = 10;
            this.DetailReport9.Name = "DetailReport9";
            // 
            // Detail10
            // 
            this.Detail10.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable21});
            this.Detail10.HeightF = 25F;
            this.Detail10.Name = "Detail10";
            // 
            // xrTable21
            // 
            this.xrTable21.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable21.Name = "xrTable21";
            this.xrTable21.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow24});
            this.xrTable21.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            // 
            // xrTableRow24
            // 
            this.xrTableRow24.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow24.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell119,
            this.xrTableCell120,
            this.xrTableCell121,
            this.xrTableCell144,
            this.xrTableCell145,
            this.xrTableCell142,
            this.xrTableCell143});
            this.xrTableRow24.Name = "xrTableRow24";
            this.xrTableRow24.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow24.StylePriority.UseBorders = false;
            this.xrTableRow24.StylePriority.UsePadding = false;
            this.xrTableRow24.StylePriority.UseTextAlignment = false;
            this.xrTableRow24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow24.Weight = 11.5D;
            // 
            // xrTableCell119
            // 
            this.xrTableCell119.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell119.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DataSource.CurrentRowIndex] + 1")});
            this.xrTableCell119.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell119.Multiline = true;
            this.xrTableCell119.Name = "xrTableCell119";
            this.xrTableCell119.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell119.StylePriority.UseBorders = false;
            this.xrTableCell119.StylePriority.UseFont = false;
            this.xrTableCell119.StylePriority.UsePadding = false;
            this.xrTableCell119.StylePriority.UseTextAlignment = false;
            xrSummary6.IgnoreNullValues = true;
            xrSummary6.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell119.Summary = xrSummary6;
            this.xrTableCell119.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell119.Weight = 0.17984964486974597D;
            // 
            // xrTableCell120
            // 
            this.xrTableCell120.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell120.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Content]")});
            this.xrTableCell120.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell120.Multiline = true;
            this.xrTableCell120.Name = "xrTableCell120";
            this.xrTableCell120.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell120.StylePriority.UseBorders = false;
            this.xrTableCell120.StylePriority.UseFont = false;
            this.xrTableCell120.StylePriority.UsePadding = false;
            this.xrTableCell120.StylePriority.UseTextAlignment = false;
            this.xrTableCell120.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell120.Weight = 0.66139144304350572D;
            // 
            // xrTableCell121
            // 
            this.xrTableCell121.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell121.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Type]")});
            this.xrTableCell121.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell121.Multiline = true;
            this.xrTableCell121.Name = "xrTableCell121";
            this.xrTableCell121.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell121.StylePriority.UseBorders = false;
            this.xrTableCell121.StylePriority.UseFont = false;
            this.xrTableCell121.StylePriority.UsePadding = false;
            this.xrTableCell121.StylePriority.UseTextAlignment = false;
            this.xrTableCell121.Text = "xrTableCell78";
            this.xrTableCell121.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell121.Weight = 0.40162689647002481D;
            // 
            // xrTableCell144
            // 
            this.xrTableCell144.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell144.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Status]")});
            this.xrTableCell144.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell144.Multiline = true;
            this.xrTableCell144.Name = "xrTableCell144";
            this.xrTableCell144.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell144.StylePriority.UseBorders = false;
            this.xrTableCell144.StylePriority.UseFont = false;
            this.xrTableCell144.StylePriority.UsePadding = false;
            this.xrTableCell144.StylePriority.UseTextAlignment = false;
            this.xrTableCell144.Text = "xrTableCell144";
            this.xrTableCell144.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell144.Weight = 0.45371448186733387D;
            // 
            // xrTableCell145
            // 
            this.xrTableCell145.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell145.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([ProductValue])")});
            this.xrTableCell145.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrTableCell145.Multiline = true;
            this.xrTableCell145.Name = "xrTableCell145";
            this.xrTableCell145.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 3, 100F);
            this.xrTableCell145.StylePriority.UseBorders = false;
            this.xrTableCell145.StylePriority.UseFont = false;
            this.xrTableCell145.StylePriority.UsePadding = false;
            this.xrTableCell145.StylePriority.UseTextAlignment = false;
            this.xrTableCell145.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell145.TextFormatString = "{0:n0}";
            this.xrTableCell145.Weight = 0.46465248916791752D;
            // 
            // xrTableCell142
            // 
            this.xrTableCell142.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell142.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "ToDecimal([WarrantyValue])")});
            this.xrTableCell142.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrTableCell142.Multiline = true;
            this.xrTableCell142.Name = "xrTableCell142";
            this.xrTableCell142.StylePriority.UseBorders = false;
            this.xrTableCell142.StylePriority.UseFont = false;
            this.xrTableCell142.StylePriority.UseTextAlignment = false;
            this.xrTableCell142.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell142.TextFormatString = "{0:n0}";
            this.xrTableCell142.Weight = 0.44603855550674576D;
            // 
            // xrTableCell143
            // 
            this.xrTableCell143.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell143.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StartDate]")});
            this.xrTableCell143.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell143.Multiline = true;
            this.xrTableCell143.Name = "xrTableCell143";
            this.xrTableCell143.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell143.StylePriority.UseBorders = false;
            this.xrTableCell143.StylePriority.UseFont = false;
            this.xrTableCell143.StylePriority.UsePadding = false;
            this.xrTableCell143.StylePriority.UseTextAlignment = false;
            this.xrTableCell143.Text = "xrTableCell80";
            this.xrTableCell143.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell143.TextFormatString = "{0:dd/MM/yyyy}";
            this.xrTableCell143.Weight = 0.65488665085013631D;
            // 
            // ReportHeader8
            // 
            this.ReportHeader8.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel35});
            this.ReportHeader8.HeightF = 37.82229F;
            this.ReportHeader8.Name = "ReportHeader8";
            // 
            // xrLabel35
            // 
            this.xrLabel35.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(0F, 14.82229F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(241.9716F, 23F);
            this.xrLabel35.StylePriority.UseFont = false;
            this.xrLabel35.StylePriority.UseTextAlignment = false;
            this.xrLabel35.Text = "DỊCH VỤ KHÁCH HÀNG";
            this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader12
            // 
            this.GroupHeader12.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable20});
            this.GroupHeader12.HeightF = 26.6883F;
            this.GroupHeader12.Name = "GroupHeader12";
            this.GroupHeader12.RepeatEveryPage = true;
            // 
            // xrTable20
            // 
            this.xrTable20.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable20.LocationFloat = new DevExpress.Utils.PointFloat(10.00002F, 0F);
            this.xrTable20.Name = "xrTable20";
            this.xrTable20.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow23});
            this.xrTable20.SizeF = new System.Drawing.SizeF(731.9999F, 26.6883F);
            this.xrTable20.StylePriority.UseFont = false;
            this.xrTable20.StylePriority.UseTextAlignment = false;
            this.xrTable20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow23
            // 
            this.xrTableRow23.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow23.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell112,
            this.xrTableCell113,
            this.xrTableCell114,
            this.xrTableCell117,
            this.xrTableCell118,
            this.xrTableCell115,
            this.xrTableCell116});
            this.xrTableRow23.Name = "xrTableRow23";
            this.xrTableRow23.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow23.StylePriority.UseBorders = false;
            this.xrTableRow23.StylePriority.UsePadding = false;
            this.xrTableRow23.Weight = 1D;
            // 
            // xrTableCell112
            // 
            this.xrTableCell112.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell112.Name = "xrTableCell112";
            this.xrTableCell112.StylePriority.UseBorders = false;
            this.xrTableCell112.StylePriority.UseTextAlignment = false;
            this.xrTableCell112.Text = "STT";
            this.xrTableCell112.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell112.Weight = 0.43556088537553916D;
            // 
            // xrTableCell113
            // 
            this.xrTableCell113.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell113.Name = "xrTableCell113";
            this.xrTableCell113.StylePriority.UseBorders = false;
            this.xrTableCell113.StylePriority.UseTextAlignment = false;
            this.xrTableCell113.Text = "Nội dung";
            this.xrTableCell113.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell113.Weight = 1.6017616685077916D;
            // 
            // xrTableCell114
            // 
            this.xrTableCell114.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell114.Name = "xrTableCell114";
            this.xrTableCell114.StylePriority.UseBorders = false;
            this.xrTableCell114.StylePriority.UseTextAlignment = false;
            this.xrTableCell114.Text = "Phân loại";
            this.xrTableCell114.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell114.Weight = 0.97266244414065917D;
            // 
            // xrTableCell117
            // 
            this.xrTableCell117.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell117.Multiline = true;
            this.xrTableCell117.Name = "xrTableCell117";
            this.xrTableCell117.StylePriority.UseBorders = false;
            this.xrTableCell117.StylePriority.UseTextAlignment = false;
            this.xrTableCell117.Text = "Trạng thái";
            this.xrTableCell117.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell117.Weight = 1.0988083115922105D;
            // 
            // xrTableCell118
            // 
            this.xrTableCell118.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell118.Multiline = true;
            this.xrTableCell118.Name = "xrTableCell118";
            this.xrTableCell118.StylePriority.UseBorders = false;
            this.xrTableCell118.StylePriority.UseTextAlignment = false;
            this.xrTableCell118.Text = "Giá trị";
            this.xrTableCell118.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell118.Weight = 1.1252980403782278D;
            // 
            // xrTableCell115
            // 
            this.xrTableCell115.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell115.Multiline = true;
            this.xrTableCell115.Name = "xrTableCell115";
            this.xrTableCell115.StylePriority.UseBorders = false;
            this.xrTableCell115.StylePriority.UseTextAlignment = false;
            this.xrTableCell115.Text = "Chi phí";
            this.xrTableCell115.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell115.Weight = 1.0802186183384077D;
            // 
            // xrTableCell116
            // 
            this.xrTableCell116.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell116.Multiline = true;
            this.xrTableCell116.Name = "xrTableCell116";
            this.xrTableCell116.StylePriority.UseBorders = false;
            this.xrTableCell116.StylePriority.UseTextAlignment = false;
            this.xrTableCell116.Text = "Ngày tiếp nhận";
            this.xrTableCell116.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell116.Weight = 1.5860082127832966D;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrPictureBox2});
            this.PageFooter.HeightF = 38.47979F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.xrPageInfo1.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(685.8951F, 2.770305F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.RunningBand = this.GroupHeader1;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(60.10852F, 28.4797F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPageInfo1.TextFormatString = "Page {0}";
            // 
            // DetailReport10
            // 
            this.DetailReport10.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail11,
            this.ReportHeader10,
            this.GroupHeader13});
            this.DetailReport10.DataMember = "CertificateAC";
            this.DetailReport10.DataSource = this.dsCustomerProfile1;
            this.DetailReport10.Level = 11;
            this.DetailReport10.Name = "DetailReport10";
            // 
            // Detail11
            // 
            this.Detail11.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable24});
            this.Detail11.HeightF = 25F;
            this.Detail11.Name = "Detail11";
            // 
            // xrTable24
            // 
            this.xrTable24.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable24.Name = "xrTable24";
            this.xrTable24.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable24.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            this.xrTable24.StylePriority.UseTextAlignment = false;
            this.xrTable24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell26,
            this.xrTableCell27,
            this.xrTableCell146,
            this.xrTableCell147});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow6.StylePriority.UseBorders = false;
            this.xrTableRow6.StylePriority.UsePadding = false;
            this.xrTableRow6.StylePriority.UseTextAlignment = false;
            this.xrTableRow6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow6.Weight = 11.5D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[STT]")});
            this.xrTableCell26.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell26.Multiline = true;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell26.StylePriority.UseBorders = false;
            this.xrTableCell26.StylePriority.UseFont = false;
            this.xrTableCell26.StylePriority.UsePadding = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell26.Weight = 0.16264046251917844D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell27.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Content]")});
            this.xrTableCell27.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell27.Multiline = true;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell27.StylePriority.UseBorders = false;
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.StylePriority.UsePadding = false;
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell27.Weight = 1.3147424764707418D;
            // 
            // xrTableCell146
            // 
            this.xrTableCell146.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell146.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StartDate]")});
            this.xrTableCell146.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell146.Multiline = true;
            this.xrTableCell146.Name = "xrTableCell146";
            this.xrTableCell146.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell146.StylePriority.UseBorders = false;
            this.xrTableCell146.StylePriority.UseFont = false;
            this.xrTableCell146.StylePriority.UsePadding = false;
            this.xrTableCell146.StylePriority.UseTextAlignment = false;
            this.xrTableCell146.Text = "xrTableCell78";
            this.xrTableCell146.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell146.Weight = 0.71437989761580079D;
            // 
            // xrTableCell147
            // 
            this.xrTableCell147.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell147.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EndDate]")});
            this.xrTableCell147.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell147.Multiline = true;
            this.xrTableCell147.Name = "xrTableCell147";
            this.xrTableCell147.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell147.StylePriority.UseBorders = false;
            this.xrTableCell147.StylePriority.UseFont = false;
            this.xrTableCell147.StylePriority.UsePadding = false;
            this.xrTableCell147.StylePriority.UseTextAlignment = false;
            this.xrTableCell147.Text = "xrTableCell79";
            this.xrTableCell147.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell147.Weight = 0.76295552362162644D;
            // 
            // ReportHeader10
            // 
            this.ReportHeader10.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel42});
            this.ReportHeader10.HeightF = 32.99999F;
            this.ReportHeader10.Name = "ReportHeader10";
            // 
            // xrLabel42
            // 
            this.xrLabel42.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.99999F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(241.9716F, 23F);
            this.xrLabel42.StylePriority.UseFont = false;
            this.xrLabel42.StylePriority.UseTextAlignment = false;
            this.xrLabel42.Text = "GIẤY CHỨNG NHẬN";
            this.xrLabel42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader13
            // 
            this.GroupHeader13.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable23});
            this.GroupHeader13.HeightF = 25F;
            this.GroupHeader13.Name = "GroupHeader13";
            this.GroupHeader13.RepeatEveryPage = true;
            // 
            // xrTable23
            // 
            this.xrTable23.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable23.LocationFloat = new DevExpress.Utils.PointFloat(10F, 0F);
            this.xrTable23.Name = "xrTable23";
            this.xrTable23.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTable23.SizeF = new System.Drawing.SizeF(731.9999F, 25F);
            this.xrTable23.StylePriority.UseFont = false;
            this.xrTable23.StylePriority.UseTextAlignment = false;
            this.xrTable23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18,
            this.xrTableCell22,
            this.xrTableCell23,
            this.xrTableCell24});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow3.StylePriority.UseBorders = false;
            this.xrTableRow3.StylePriority.UsePadding = false;
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseBorders = false;
            this.xrTableCell18.Text = "STT";
            this.xrTableCell18.Weight = 0.49229634342323991D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseBorders = false;
            this.xrTableCell22.Text = "Nội dung";
            this.xrTableCell22.Weight = 3.9795937486464723D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StylePriority.UseBorders = false;
            this.xrTableCell23.Text = "Ngày hiệu lực";
            this.xrTableCell23.Weight = 2.1623555318003671D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell24.Multiline = true;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseBorders = false;
            this.xrTableCell24.Text = "Ngày hết hạn";
            this.xrTableCell24.Weight = 2.3093900952145052D;
            // 
            // DetailReport11
            // 
            this.DetailReport11.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail12,
            this.ReportHeader11,
            this.GroupHeader11});
            this.DetailReport11.DataMember = "Event";
            this.DetailReport11.DataSource = this.dsCustomerProfile1;
            this.DetailReport11.Level = 9;
            this.DetailReport11.Name = "DetailReport11";
            // 
            // Detail12
            // 
            this.Detail12.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable26});
            this.Detail12.HeightF = 25F;
            this.Detail12.Name = "Detail12";
            // 
            // xrTable26
            // 
            this.xrTable26.LocationFloat = new DevExpress.Utils.PointFloat(10.00002F, 0F);
            this.xrTable26.Name = "xrTable26";
            this.xrTable26.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow29});
            this.xrTable26.SizeF = new System.Drawing.SizeF(731.9998F, 25F);
            // 
            // xrTableRow29
            // 
            this.xrTableRow29.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow29.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell159,
            this.xrTableCell160,
            this.xrTableCell161,
            this.xrTableCell162,
            this.xrTableCell163});
            this.xrTableRow29.Name = "xrTableRow29";
            this.xrTableRow29.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow29.StylePriority.UseBorders = false;
            this.xrTableRow29.StylePriority.UsePadding = false;
            this.xrTableRow29.StylePriority.UseTextAlignment = false;
            this.xrTableRow29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableRow29.Weight = 11.5D;
            // 
            // xrTableCell159
            // 
            this.xrTableCell159.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell159.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DataSource.CurrentRowIndex] + 1")});
            this.xrTableCell159.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell159.Multiline = true;
            this.xrTableCell159.Name = "xrTableCell159";
            this.xrTableCell159.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell159.StylePriority.UseBorders = false;
            this.xrTableCell159.StylePriority.UseFont = false;
            this.xrTableCell159.StylePriority.UsePadding = false;
            this.xrTableCell159.StylePriority.UseTextAlignment = false;
            xrSummary7.IgnoreNullValues = true;
            xrSummary7.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell159.Summary = xrSummary7;
            this.xrTableCell159.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell159.Weight = 0.11915339102112674D;
            // 
            // xrTableCell160
            // 
            this.xrTableCell160.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell160.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ContentName]")});
            this.xrTableCell160.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell160.Multiline = true;
            this.xrTableCell160.Name = "xrTableCell160";
            this.xrTableCell160.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell160.StylePriority.UseBorders = false;
            this.xrTableCell160.StylePriority.UseFont = false;
            this.xrTableCell160.StylePriority.UsePadding = false;
            this.xrTableCell160.StylePriority.UseTextAlignment = false;
            this.xrTableCell160.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell160.Weight = 0.89476200614544033D;
            // 
            // xrTableCell161
            // 
            this.xrTableCell161.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell161.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NumberInvitation]")});
            this.xrTableCell161.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell161.Multiline = true;
            this.xrTableCell161.Name = "xrTableCell161";
            this.xrTableCell161.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell161.StylePriority.UseBorders = false;
            this.xrTableCell161.StylePriority.UseFont = false;
            this.xrTableCell161.StylePriority.UsePadding = false;
            this.xrTableCell161.StylePriority.UseTextAlignment = false;
            this.xrTableCell161.Text = "xrTableCell78";
            this.xrTableCell161.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell161.Weight = 0.28277462848429996D;
            // 
            // xrTableCell162
            // 
            this.xrTableCell162.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell162.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NumberConfirm]")});
            this.xrTableCell162.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell162.Multiline = true;
            this.xrTableCell162.Name = "xrTableCell162";
            this.xrTableCell162.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell162.StylePriority.UseBorders = false;
            this.xrTableCell162.StylePriority.UseFont = false;
            this.xrTableCell162.StylePriority.UsePadding = false;
            this.xrTableCell162.StylePriority.UseTextAlignment = false;
            this.xrTableCell162.Text = "xrTableCell144";
            this.xrTableCell162.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell162.Weight = 0.39989244059974333D;
            // 
            // xrTableCell163
            // 
            this.xrTableCell163.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell163.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NumberEvent]")});
            this.xrTableCell163.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrTableCell163.Multiline = true;
            this.xrTableCell163.Name = "xrTableCell163";
            this.xrTableCell163.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 100F);
            this.xrTableCell163.StylePriority.UseBorders = false;
            this.xrTableCell163.StylePriority.UseFont = false;
            this.xrTableCell163.StylePriority.UsePadding = false;
            this.xrTableCell163.StylePriority.UseTextAlignment = false;
            this.xrTableCell163.Text = "xrTableCell145";
            this.xrTableCell163.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell163.Weight = 0.46465248916791752D;
            // 
            // ReportHeader11
            // 
            this.ReportHeader11.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel43});
            this.ReportHeader11.HeightF = 37.82229F;
            this.ReportHeader11.Name = "ReportHeader11";
            // 
            // xrLabel43
            // 
            this.xrLabel43.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(0F, 14.82229F);
            this.xrLabel43.Name = "xrLabel43";
            this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel43.SizeF = new System.Drawing.SizeF(241.9716F, 23F);
            this.xrLabel43.StylePriority.UseFont = false;
            this.xrLabel43.StylePriority.UseTextAlignment = false;
            this.xrLabel43.Text = "THAM GIA SỰ KIỆN";
            this.xrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader11
            // 
            this.GroupHeader11.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable25});
            this.GroupHeader11.HeightF = 26.6883F;
            this.GroupHeader11.Name = "GroupHeader11";
            this.GroupHeader11.RepeatEveryPage = true;
            // 
            // xrTable25
            // 
            this.xrTable25.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.xrTable25.LocationFloat = new DevExpress.Utils.PointFloat(10.00002F, 0F);
            this.xrTable25.Name = "xrTable25";
            this.xrTable25.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
            this.xrTable25.SizeF = new System.Drawing.SizeF(731.9998F, 26.6883F);
            this.xrTable25.StylePriority.UseFont = false;
            this.xrTable25.StylePriority.UseTextAlignment = false;
            this.xrTable25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell149,
            this.xrTableCell150,
            this.xrTableCell151,
            this.xrTableCell157,
            this.xrTableCell158});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.xrTableRow7.StylePriority.UseBorders = false;
            this.xrTableRow7.StylePriority.UsePadding = false;
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell149
            // 
            this.xrTableCell149.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell149.Name = "xrTableCell149";
            this.xrTableCell149.StylePriority.UseBorders = false;
            this.xrTableCell149.StylePriority.UseTextAlignment = false;
            this.xrTableCell149.Text = "STT";
            this.xrTableCell149.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell149.Weight = 0.288106910800073D;
            // 
            // xrTableCell150
            // 
            this.xrTableCell150.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell150.Name = "xrTableCell150";
            this.xrTableCell150.StylePriority.UseBorders = false;
            this.xrTableCell150.StylePriority.UseTextAlignment = false;
            this.xrTableCell150.Text = "Nội dung sự kiện";
            this.xrTableCell150.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell150.Weight = 2.167398924539822D;
            // 
            // xrTableCell151
            // 
            this.xrTableCell151.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell151.Name = "xrTableCell151";
            this.xrTableCell151.StylePriority.UseBorders = false;
            this.xrTableCell151.StylePriority.UseTextAlignment = false;
            this.xrTableCell151.Text = "Số lượt mời";
            this.xrTableCell151.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell151.Weight = 0.68482491977521132D;
            // 
            // xrTableCell157
            // 
            this.xrTableCell157.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell157.Multiline = true;
            this.xrTableCell157.Name = "xrTableCell157";
            this.xrTableCell157.StylePriority.UseBorders = false;
            this.xrTableCell157.StylePriority.UseTextAlignment = false;
            this.xrTableCell157.Text = "Số lượt tham gia";
            this.xrTableCell157.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell157.Weight = 0.96846255450109409D;
            // 
            // xrTableCell158
            // 
            this.xrTableCell158.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell158.Multiline = true;
            this.xrTableCell158.Name = "xrTableCell158";
            this.xrTableCell158.StylePriority.UseBorders = false;
            this.xrTableCell158.StylePriority.UseTextAlignment = false;
            this.xrTableCell158.Text = "Số sự kiện trong năm";
            this.xrTableCell158.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell158.Weight = 1.1252980403782278D;
            // 
            // DetailReport12
            // 
            this.DetailReport12.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail13});
            this.DetailReport12.DataMember = "Detail";
            this.DetailReport12.DataSource = this.dsCustomerProfile1;
            this.DetailReport12.Level = 12;
            this.DetailReport12.Name = "DetailReport12";
            // 
            // Detail13
            // 
            this.Detail13.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel57,
            this.xrLabel58,
            this.xrLabel59});
            this.Detail13.HeightF = 37.52438F;
            this.Detail13.Name = "Detail13";
            // 
            // xrLabel57
            // 
            this.xrLabel57.AutoWidth = true;
            this.xrLabel57.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(1.000135F, 10F);
            this.xrLabel57.Multiline = true;
            this.xrLabel57.Name = "xrLabel57";
            this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel57.SizeF = new System.Drawing.SizeF(125.0875F, 23.00002F);
            this.xrLabel57.StylePriority.UseFont = false;
            this.xrLabel57.StylePriority.UseTextAlignment = false;
            this.xrLabel57.Text = "GHI CHÚ KHÁC";
            this.xrLabel57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel58
            // 
            this.xrLabel58.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel58.LocationFloat = new DevExpress.Utils.PointFloat(126.0877F, 10F);
            this.xrLabel58.Name = "xrLabel58";
            this.xrLabel58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel58.SizeF = new System.Drawing.SizeF(10F, 23F);
            this.xrLabel58.StylePriority.UseFont = false;
            this.xrLabel58.Text = ":";
            // 
            // xrLabel59
            // 
            this.xrLabel59.AutoWidth = true;
            this.xrLabel59.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Note]")});
            this.xrLabel59.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(136.0877F, 10F);
            this.xrLabel59.Multiline = true;
            this.xrLabel59.Name = "xrLabel59";
            this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel59.SizeF = new System.Drawing.SizeF(605.9125F, 23F);
            this.xrLabel59.StylePriority.UseFont = false;
            this.xrLabel59.StylePriority.UseTextAlignment = false;
            this.xrLabel59.Text = "xrLabel13";
            this.xrLabel59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // dsCustomerProfile2
            // 
            this.dsCustomerProfile2.DataSetName = "DsCustomerProfile";
            this.dsCustomerProfile2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // CustomerProfileXtraReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.GroupHeader1,
            this.GroupFooter1,
            this.DetailReport,
            this.DetailReport1,
            this.DetailReport2,
            this.DetailReport3,
            this.DetailReport4,
            this.DetailReport5,
            this.DetailReport6,
            this.DetailReport7,
            this.DetailReport8,
            this.DetailReport9,
            this.PageFooter,
            this.DetailReport10,
            this.DetailReport11,
            this.DetailReport12});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1,
            this.dsCustomerProfile1,
            this.dsCustomerProfile2});
            this.DataMember = "Address";
            this.DataSource = this.dsCustomerProfile1;
            this.Margins = new System.Drawing.Printing.Margins(38, 39, 24, 31);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Version = "21.1";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsCustomerProfile1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsCustomerProfile2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
