namespace ePonti.ReportLib
{
    partial class WorkOrder
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkOrder));
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule5 = new Telerik.Reporting.Drawing.StyleRule();
            this.rptWoInfoData = new Telerik.Reporting.SqlDataSource();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.logoBox = new Telerik.Reporting.PictureBox();
            this.RptHeader = new Telerik.Reporting.TextBox();
            this.CoLogo = new Telerik.Reporting.PictureBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.jobCaptionTextBox = new Telerik.Reporting.TextBox();
            this.htmlTextBox1 = new Telerik.Reporting.HtmlTextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.htmlTextBox3 = new Telerik.Reporting.HtmlTextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.subWoItems = new Telerik.Reporting.SubReport();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.shape4 = new Telerik.Reporting.Shape();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.htmlTextBox2 = new Telerik.Reporting.HtmlTextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // rptWoInfoData
            // 
            this.rptWoInfoData.ConnectionString = "DefaultConnection";
            this.rptWoInfoData.Name = "rptWoInfoData";
            this.rptWoInfoData.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@WoID", System.Data.DbType.Int32, "= Parameters.WoID.Value")});
            this.rptWoInfoData.SelectCommand = "dbo.RptWoInfoByWoID";
            this.rptWoInfoData.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Inch(1.3999999761581421D);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.logoBox,
            this.RptHeader,
            this.CoLogo});
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.Style.BorderColor.Bottom = System.Drawing.Color.Red;
            this.pageHeader.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.pageHeader.Style.Color = System.Drawing.Color.Red;
            // 
            // logoBox
            // 
            this.logoBox.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Left)));
            this.logoBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(-2.0619049072265625D), Telerik.Reporting.Drawing.Unit.Inch(-0.49999994039535522D));
            this.logoBox.MimeType = "";
            this.logoBox.Name = "logoBox";
            this.logoBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.89999997615814209D), Telerik.Reporting.Drawing.Unit.Inch(0.89999997615814209D));
            this.logoBox.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.logoBox.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.logoBox.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.logoBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.logoBox.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.logoBox.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.logoBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.logoBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.logoBox.Value = "= Fields.Logo";
            // 
            // RptHeader
            // 
            this.RptHeader.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.3999996185302734D), Telerik.Reporting.Drawing.Unit.Inch(0.89999997615814209D));
            this.RptHeader.Name = "RptHeader";
            this.RptHeader.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.3999999463558197D));
            this.RptHeader.Style.Color = System.Drawing.Color.DimGray;
            this.RptHeader.Style.Font.Name = "Microsoft Sans Serif";
            this.RptHeader.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(21D);
            this.RptHeader.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.RptHeader.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.RptHeader.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.RptHeader.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.RptHeader.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.RptHeader.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.RptHeader.StyleName = "Title";
            this.RptHeader.Value = "Work Order";
            // 
            // CoLogo
            // 
            this.CoLogo.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Left)));
            this.CoLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.CoLogo.MimeType = "";
            this.CoLogo.Name = "CoLogo";
            this.CoLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.CoLogo.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.CoLogo.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.CoLogo.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.CoLogo.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.CoLogo.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.CoLogo.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.CoLogo.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.CoLogo.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.CoLogo.Value = "= Fields.Logo";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Inch(0.22380951046943665D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageInfoTextBox,
            this.textBox10});
            this.pageFooter.Name = "pageFooter";
            this.pageFooter.Style.BorderColor.Top = System.Drawing.Color.Red;
            this.pageFooter.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.pageFooter.Style.BorderWidth.Top = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.6999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(3.9236885641003028E-05D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.pageInfoTextBox.Style.Color = System.Drawing.Color.DimGray;
            this.pageInfoTextBox.Style.Font.Name = "Microsoft Sans Serif";
            this.pageInfoTextBox.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.pageInfoTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.pageInfoTextBox.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.pageInfoTextBox.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.pageInfoTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.2999997138977051D), Telerik.Reporting.Drawing.Unit.Inch(3.9236885641003028E-05D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.3999997079372406D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.textBox10.Style.Color = System.Drawing.Color.DimGray;
            this.textBox10.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox10.Value = "Page";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Inch(5.09999942779541D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox9,
            this.textBox2,
            this.jobCaptionTextBox,
            this.htmlTextBox1,
            this.textBox3,
            this.textBox4,
            this.textBox5,
            this.textBox6,
            this.textBox7,
            this.textBox8,
            this.htmlTextBox3,
            this.textBox1,
            this.textBox11,
            this.textBox12,
            this.textBox13});
            this.reportHeader.Name = "reportHeader";
            // 
            // textBox9
            // 
            this.textBox9.CanGrow = true;
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.0380563735961914D), Telerik.Reporting.Drawing.Unit.Inch(0.89996063709259033D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox9.Style.Color = System.Drawing.Color.DimGray;
            this.textBox9.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.StyleName = "Caption";
            this.textBox9.Value = "WO#";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Format = "{0:d}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.0381345748901367D), Telerik.Reporting.Drawing.Unit.Inch(1.1012763977050781D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox2.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox2.StyleName = "Data";
            this.textBox2.Value = "= Fields.AssignedDate.Date";
            // 
            // jobCaptionTextBox
            // 
            this.jobCaptionTextBox.CanGrow = true;
            this.jobCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.43805584311485291D), Telerik.Reporting.Drawing.Unit.Inch(1.7000000476837158D));
            this.jobCaptionTextBox.Name = "jobCaptionTextBox";
            this.jobCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.jobCaptionTextBox.Style.Color = System.Drawing.Color.DimGray;
            this.jobCaptionTextBox.Style.Font.Name = "Microsoft Sans Serif";
            this.jobCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.jobCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.jobCaptionTextBox.StyleName = "Caption";
            this.jobCaptionTextBox.Value = "Job";
            // 
            // htmlTextBox1
            // 
            this.htmlTextBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.43805584311485291D), Telerik.Reporting.Drawing.Unit.Inch(1.9000791311264038D));
            this.htmlTextBox1.Name = "htmlTextBox1";
            this.htmlTextBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.htmlTextBox1.Style.Font.Name = "Roboto";
            this.htmlTextBox1.Value = resources.GetString("htmlTextBox1.Value");
            // 
            // textBox3
            // 
            this.textBox3.CanGrow = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.0380563735961914D), Telerik.Reporting.Drawing.Unit.Inch(0.89996063709259033D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox3.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox3.StyleName = "Data";
            this.textBox3.Value = "= Fields.[Wo#]";
            // 
            // textBox4
            // 
            this.textBox4.CanGrow = true;
            this.textBox4.Format = "{0:t}";
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.0381345748901367D), Telerik.Reporting.Drawing.Unit.Inch(1.3025920391082764D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox4.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox4.StyleName = "Data";
            this.textBox4.Value = "= Fields.StartTime";
            // 
            // textBox5
            // 
            this.textBox5.CanGrow = true;
            this.textBox5.Format = "{0:t}";
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.0380563735961914D), Telerik.Reporting.Drawing.Unit.Inch(1.5039076805114746D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox5.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox5.StyleName = "Data";
            this.textBox5.Value = "= Fields.Resource";
            // 
            // textBox6
            // 
            this.textBox6.CanGrow = true;
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.0380563735961914D), Telerik.Reporting.Drawing.Unit.Inch(1.1012763977050781D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox6.Style.Color = System.Drawing.Color.DimGray;
            this.textBox6.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.StyleName = "Caption";
            this.textBox6.Value = "Date";
            // 
            // textBox7
            // 
            this.textBox7.CanGrow = true;
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.0380563735961914D), Telerik.Reporting.Drawing.Unit.Inch(1.3025920391082764D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox7.Style.Color = System.Drawing.Color.DimGray;
            this.textBox7.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.StyleName = "Caption";
            this.textBox7.Value = "Start";
            // 
            // textBox8
            // 
            this.textBox8.CanGrow = true;
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.0380563735961914D), Telerik.Reporting.Drawing.Unit.Inch(1.5039076805114746D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox8.Style.Color = System.Drawing.Color.DimGray;
            this.textBox8.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.StyleName = "Caption";
            this.textBox8.Value = "Resource";
            // 
            // htmlTextBox3
            // 
            this.htmlTextBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D));
            this.htmlTextBox3.Name = "htmlTextBox3";
            this.htmlTextBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.89999997615814209D));
            this.htmlTextBox3.Style.Font.Name = "Roboto Condensed Light";
            this.htmlTextBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.htmlTextBox3.Value = "{Fields.CoName}<br />{Fields.CoAddress1}<br />{Fields.CoAddress2}<br />{Fields.Co" +
    "Phone}<br />{Fields.License}<br />";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.43805578351020813D), Telerik.Reporting.Drawing.Unit.Inch(2.9999992847442627D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox1.Style.Color = System.Drawing.Color.DimGray;
            this.textBox1.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "Customer Comments";
            // 
            // textBox11
            // 
            this.textBox11.CanGrow = true;
            this.textBox11.Format = "{0:t}";
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.43805584311485291D), Telerik.Reporting.Drawing.Unit.Inch(3.2000777721405029D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.561943531036377D), Telerik.Reporting.Drawing.Unit.Inch(0.29999980330467224D));
            this.textBox11.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBox11.StyleName = "Data";
            this.textBox11.Value = "The Customer is experiencing issue with ....";
            // 
            // textBox12
            // 
            this.textBox12.CanGrow = true;
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.43805596232414246D), Telerik.Reporting.Drawing.Unit.Inch(3.6999998092651367D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox12.Style.Color = System.Drawing.Color.DimGray;
            this.textBox12.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox12.StyleName = "Caption";
            this.textBox12.Value = "Findings";
            // 
            // textBox13
            // 
            this.textBox13.CanGrow = true;
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.43805611133575439D), Telerik.Reporting.Drawing.Unit.Inch(4.4000000953674316D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox13.Style.Color = System.Drawing.Color.DimGray;
            this.textBox13.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox13.StyleName = "Caption";
            this.textBox13.Value = "Recommendations";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.5D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.subWoItems});
            this.detail.Name = "detail";
            // 
            // subWoItems
            // 
            this.subWoItems.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.subWoItems.Name = "subWoItems";
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("WoID", "= Fields.WoID"));
            typeReportSource1.TypeName = "ePonti.ReportLib.WoItems, ePonti.ReportLib, Version=1.0.0.0, Culture=neutral, Pub" +
    "licKeyToken=null";
            this.subWoItems.ReportSource = typeReportSource1;
            this.subWoItems.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.5D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.shape4,
            this.textBox19,
            this.textBox17,
            this.htmlTextBox2});
            this.reportFooterSection1.Name = "reportFooterSection1";
            // 
            // shape4
            // 
            this.shape4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.1999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.86071264743804932D));
            this.shape4.Name = "shape4";
            this.shape4.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D));
            this.shape4.Style.LineColor = System.Drawing.Color.DimGray;
            this.shape4.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            // 
            // textBox19
            // 
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.1999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.90000003576278687D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.textBox19.Style.Color = System.Drawing.Color.DimGray;
            this.textBox19.Style.Font.Bold = true;
            this.textBox19.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox19.Style.LineColor = System.Drawing.Color.DimGray;
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox19.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox19.TextWrap = false;
            this.textBox19.Value = "Accepted by: {Fields.Customer}";
            // 
            // textBox17
            // 
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.69999885559082D), Telerik.Reporting.Drawing.Unit.Inch(0.90000003576278687D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.textBox17.Style.Color = System.Drawing.Color.DimGray;
            this.textBox17.Style.Font.Bold = true;
            this.textBox17.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox17.Style.LineColor = System.Drawing.Color.DimGray;
            this.textBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox17.TextWrap = false;
            this.textBox17.Value = "Date";
            // 
            // htmlTextBox2
            // 
            this.htmlTextBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.50000035762786865D), Telerik.Reporting.Drawing.Unit.Inch(0.2000000923871994D));
            this.htmlTextBox2.Name = "htmlTextBox2";
            this.htmlTextBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.4999995231628418D), Telerik.Reporting.Drawing.Unit.Inch(0.2000001072883606D));
            this.htmlTextBox2.Style.Font.Name = "Roboto Condensed Light";
            this.htmlTextBox2.Value = "I agree that this work has been completed to my satisfaction...";
            // 
            // WorkOrder
            // 
            this.DataSource = this.rptWoInfoData;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.detail,
            this.reportFooterSection1});
            this.Name = "WorkOrder";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.Name = "WoID";
            reportParameter1.Text = "WoID";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Value = "1023";
            this.ReportParameters.Add(reportParameter1);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule2.Style.Color = System.Drawing.Color.Black;
            styleRule2.Style.Font.Bold = true;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18D);
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule3.Style.Color = System.Drawing.Color.Black;
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule5.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule5.Style.Font.Name = "Tahoma";
            styleRule5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            styleRule5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4,
            styleRule5});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(7.5D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource rptWoInfoData;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PictureBox logoBox;
        private Telerik.Reporting.TextBox RptHeader;
        private Telerik.Reporting.PictureBox CoLogo;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox jobCaptionTextBox;
        private Telerik.Reporting.HtmlTextBox htmlTextBox1;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.SubReport subWoItems;
        private Telerik.Reporting.HtmlTextBox htmlTextBox3;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.Shape shape4;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.HtmlTextBox htmlTextBox2;
    }
}