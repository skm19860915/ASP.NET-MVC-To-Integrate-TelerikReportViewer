namespace ePonti.web.Reports.Default.Administration
{
    partial class QuoteReport
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuoteReport));
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule5 = new Telerik.Reporting.Drawing.StyleRule();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.subItemDetail = new Telerik.Reporting.SubReport();
            this.QuoteReportData = new Telerik.Reporting.SqlDataSource();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.RptHeader = new Telerik.Reporting.TextBox();
            this.CoLogo = new Telerik.Reporting.PictureBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.jobCaptionTextBox = new Telerik.Reporting.TextBox();
            this.JobInfo = new Telerik.Reporting.HtmlTextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.htmlTextBox3 = new Telerik.Reporting.HtmlTextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.htmlTextBox1 = new Telerik.Reporting.HtmlTextBox();
            this.reportFooter = new Telerik.Reporting.ReportFooterSection();
            this.htmlTextBox4 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox2 = new Telerik.Reporting.HtmlTextBox();
            this.shape4 = new Telerik.Reporting.Shape();
            this.shape3 = new Telerik.Reporting.Shape();
            this.textBox20 = new Telerik.Reporting.TextBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.1607142835855484D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = false;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.699999988079071D);
            this.labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.subItemDetail});
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            // 
            // subItemDetail
            // 
            this.subItemDetail.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.1999996155500412D));
            this.subItemDetail.Name = "subItemDetail";
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("ProjectID", "= Fields.ProjectID"));
            typeReportSource1.TypeName = "ePonti.web.Reports.Default.Administration.ItemDetail, ePonti.Reports, Version=1.0" +
    ".0.0, Culture=neutral, PublicKeyToken=null";
            this.subItemDetail.ReportSource = typeReportSource1;
            this.subItemDetail.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.5D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D));
            // 
            // QuoteReportData
            // 
            this.QuoteReportData.ConnectionString = "DefaultConnection";
            this.QuoteReportData.Name = "QuoteReportData";
            this.QuoteReportData.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@ProjectID", System.Data.DbType.Int32, "3179")});
            this.QuoteReportData.SelectCommand = "dbo.rptProjectInfoDetail";
            this.QuoteReportData.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Inch(1.3999999761581421D);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.RptHeader,
            this.CoLogo});
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.Style.BorderColor.Bottom = System.Drawing.Color.Red;
            this.pageHeader.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // RptHeader
            // 
            this.RptHeader.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.4000000953674316D), Telerik.Reporting.Drawing.Unit.Inch(0.90000003576278687D));
            this.RptHeader.Name = "RptHeader";
            this.RptHeader.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.3999999463558197D));
            this.RptHeader.Style.Color = System.Drawing.Color.DimGray;
            this.RptHeader.Style.Font.Name = "Roboto Condensed";
            this.RptHeader.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(21D);
            this.RptHeader.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.RptHeader.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.RptHeader.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.RptHeader.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.RptHeader.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.RptHeader.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.RptHeader.StyleName = "Title";
            this.RptHeader.Value = "Quote";
            // 
            // CoLogo
            // 
            this.CoLogo.Anchoring = ((Telerik.Reporting.AnchoringStyles)((Telerik.Reporting.AnchoringStyles.Top | Telerik.Reporting.AnchoringStyles.Left)));
            this.CoLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(-1.5137688436084318E-08D));
            this.CoLogo.MimeType = "";
            this.CoLogo.Name = "CoLogo";
            this.CoLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.89999997615814209D), Telerik.Reporting.Drawing.Unit.Inch(0.89999997615814209D));
            this.CoLogo.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.CoLogo.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.CoLogo.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.CoLogo.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.CoLogo.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.CoLogo.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.CoLogo.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.CoLogo.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
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
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.8689889907836914D), Telerik.Reporting.Drawing.Unit.Inch(3.9236885641003028E-05D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.pageInfoTextBox.Style.Color = System.Drawing.Color.DimGray;
            this.pageInfoTextBox.Style.Font.Name = "Roboto Condensed Light";
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
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.4689903259277344D), Telerik.Reporting.Drawing.Unit.Inch(3.9236885641003028E-05D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.3999997079372406D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.textBox10.Style.Color = System.Drawing.Color.DimGray;
            this.textBox10.Style.Font.Name = "Roboto Condensed Light";
            this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox10.Value = "Page";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Inch(4D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.jobCaptionTextBox,
            this.JobInfo,
            this.textBox4,
            this.textBox6,
            this.textBox7,
            this.htmlTextBox3,
            this.textBox1,
            this.htmlTextBox1});
            this.reportHeader.Name = "reportHeader";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Format = "{0:d}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.1690673828125D), Telerik.Reporting.Drawing.Unit.Inch(1.1512566804885864D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox2.Style.Font.Name = "Roboto";
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox2.StyleName = "Data";
            this.textBox2.Value = "= Fields.Date.Date";
            // 
            // jobCaptionTextBox
            // 
            this.jobCaptionTextBox.CanGrow = true;
            this.jobCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(1.9499411582946777D));
            this.jobCaptionTextBox.Name = "jobCaptionTextBox";
            this.jobCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.jobCaptionTextBox.Style.Color = System.Drawing.Color.DimGray;
            this.jobCaptionTextBox.Style.Font.Name = "Roboto Condensed Light";
            this.jobCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.jobCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.jobCaptionTextBox.StyleName = "Caption";
            this.jobCaptionTextBox.Value = "Job";
            // 
            // JobInfo
            // 
            this.JobInfo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(2.1500198841094971D));
            this.JobInfo.Name = "JobInfo";
            this.JobInfo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.JobInfo.Style.Font.Name = "Roboto";
            this.JobInfo.Value = resources.GetString("JobInfo.Value");
            // 
            // textBox4
            // 
            this.textBox4.CanGrow = true;
            this.textBox4.Format = "{0:t}";
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.1690673828125D), Telerik.Reporting.Drawing.Unit.Inch(1.3525724411010742D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox4.Style.Font.Name = "Roboto";
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox4.StyleName = "Data";
            this.textBox4.Value = "= Fields.Status";
            // 
            // textBox6
            // 
            this.textBox6.CanGrow = true;
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.1689887046813965D), Telerik.Reporting.Drawing.Unit.Inch(1.1512566804885864D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox6.Style.Color = System.Drawing.Color.DimGray;
            this.textBox6.Style.Font.Name = "Roboto Condensed Light";
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.StyleName = "Caption";
            this.textBox6.Value = "Date";
            // 
            // textBox7
            // 
            this.textBox7.CanGrow = true;
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.1689887046813965D), Telerik.Reporting.Drawing.Unit.Inch(1.3525724411010742D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox7.Style.Color = System.Drawing.Color.DimGray;
            this.textBox7.Style.Font.Name = "Roboto Condensed Light";
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.StyleName = "Caption";
            this.textBox7.Value = "Status";
            // 
            // htmlTextBox3
            // 
            this.htmlTextBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1999998539686203D), Telerik.Reporting.Drawing.Unit.Inch(0.09999992698431015D));
            this.htmlTextBox3.Name = "htmlTextBox3";
            this.htmlTextBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.htmlTextBox3.Style.Font.Name = "Roboto Condensed Light";
            this.htmlTextBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.htmlTextBox3.Value = "{Fields.CoName}<br />{Fields.CoAddress1}<br />{Fields.CoAddress2}<br />{Fields.Co" +
    "Phone}<br />{Fields.License}<br />";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(3.3999998569488525D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox1.Style.Color = System.Drawing.Color.DimGray;
            this.textBox1.Style.Font.Name = "Roboto Condensed Light";
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "Scope";
            // 
            // htmlTextBox1
            // 
            this.htmlTextBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(3.60007905960083D));
            this.htmlTextBox1.Name = "htmlTextBox1";
            this.htmlTextBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.4999995231628418D), Telerik.Reporting.Drawing.Unit.Inch(0.2000001072883606D));
            this.htmlTextBox1.Value = "{Fields.ProjectNotes}";
            // 
            // reportFooter
            // 
            this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Inch(3D);
            this.reportFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.htmlTextBox4,
            this.htmlTextBox2,
            this.shape4,
            this.shape3,
            this.textBox20,
            this.textBox19,
            this.textBox18,
            this.textBox17,
            this.textBox16});
            this.reportFooter.Name = "reportFooter";
            // 
            // htmlTextBox4
            // 
            this.htmlTextBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.75539690256118774D));
            this.htmlTextBox4.Name = "htmlTextBox4";
            this.htmlTextBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.htmlTextBox4.Style.Font.Name = "Roboto";
            this.htmlTextBox4.Value = "<span style=\"font-family: roboto\">{Fields.Customer}<br />{Fields.ProjectAddress1}" +
    "&nbsp; {Fields.ProjectAddress2}<br />{Fields.ProjectCity}, {Fields.ProjectState}" +
    " {Fields.ProjectZip}</span>";
            // 
            // htmlTextBox2
            // 
            this.htmlTextBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4D), Telerik.Reporting.Drawing.Unit.Inch(0.75539690256118774D));
            this.htmlTextBox2.Name = "htmlTextBox2";
            this.htmlTextBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.htmlTextBox2.Style.Font.Name = "Roboto";
            this.htmlTextBox2.Value = "{Fields.CoName}<br />{Fields.CoAddress1}<br />{Fields.CoAddress2}<br />";
            // 
            // shape4
            // 
            this.shape4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(2.2553966045379639D));
            this.shape4.Name = "shape4";
            this.shape4.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D));
            this.shape4.Style.LineColor = System.Drawing.Color.DimGray;
            this.shape4.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            // 
            // shape3
            // 
            this.shape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4D), Telerik.Reporting.Drawing.Unit.Inch(2.2553966045379639D));
            this.shape3.Name = "shape3";
            this.shape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.019999999552965164D));
            this.shape3.Style.LineColor = System.Drawing.Color.DimGray;
            this.shape3.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            // 
            // textBox20
            // 
            this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.55531781911849976D));
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox20.Style.Color = System.Drawing.Color.DimGray;
            this.textBox20.Style.Font.Bold = true;
            this.textBox20.Style.Font.Name = "Roboto";
            this.textBox20.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox20.Style.LineColor = System.Drawing.Color.DimGray;
            this.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox20.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox20.TextWrap = false;
            this.textBox20.Value = "Accepted by:";
            // 
            // textBox19
            // 
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(2.2946827411651611D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.textBox19.Style.Color = System.Drawing.Color.DimGray;
            this.textBox19.Style.Font.Bold = true;
            this.textBox19.Style.Font.Name = "Roboto";
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox19.Style.LineColor = System.Drawing.Color.DimGray;
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox19.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox19.TextWrap = false;
            this.textBox19.Value = "Accepted by:";
            // 
            // textBox18
            // 
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.0000009536743164D), Telerik.Reporting.Drawing.Unit.Inch(2.2946827411651611D));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.textBox18.Style.Color = System.Drawing.Color.DimGray;
            this.textBox18.Style.Font.Bold = true;
            this.textBox18.Style.Font.Name = "Roboto";
            this.textBox18.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox18.Style.LineColor = System.Drawing.Color.DimGray;
            this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox18.TextWrap = false;
            this.textBox18.Value = "= Fields.CoName";
            // 
            // textBox17
            // 
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.9999995231628418D), Telerik.Reporting.Drawing.Unit.Inch(2.2946827411651611D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.textBox17.Style.Color = System.Drawing.Color.DimGray;
            this.textBox17.Style.Font.Bold = true;
            this.textBox17.Style.Font.Name = "Roboto";
            this.textBox17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox17.Style.LineColor = System.Drawing.Color.DimGray;
            this.textBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox17.TextWrap = false;
            this.textBox17.Value = "Date";
            // 
            // textBox16
            // 
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.4999990463256836D), Telerik.Reporting.Drawing.Unit.Inch(2.2946827411651611D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            this.textBox16.Style.Color = System.Drawing.Color.DimGray;
            this.textBox16.Style.Font.Bold = true;
            this.textBox16.Style.Font.Name = "Roboto";
            this.textBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox16.Style.LineColor = System.Drawing.Color.DimGray;
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox16.TextWrap = false;
            this.textBox16.Value = "Date";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.1607142835855484D);
            this.detail.Name = "detail";
            this.detail.Style.Visible = false;
            // 
            // QuoteReport
            // 
            this.DataSource = this.QuoteReportData;
            group1.GroupFooter = this.labelsGroupFooterSection;
            group1.GroupHeader = this.labelsGroupHeaderSection;
            group1.Name = "ItemDetail";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeaderSection,
            this.labelsGroupFooterSection,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.reportFooter,
            this.detail});
            this.Name = "QuoteReport";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
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

        private Telerik.Reporting.SqlDataSource QuoteReportData;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.ReportFooterSection reportFooter;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox RptHeader;
        private Telerik.Reporting.PictureBox CoLogo;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox jobCaptionTextBox;
        private Telerik.Reporting.HtmlTextBox JobInfo;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.HtmlTextBox htmlTextBox3;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.HtmlTextBox htmlTextBox1;
        private Telerik.Reporting.SubReport subItemDetail;
        private Telerik.Reporting.HtmlTextBox htmlTextBox4;
        private Telerik.Reporting.HtmlTextBox htmlTextBox2;
        private Telerik.Reporting.Shape shape4;
        private Telerik.Reporting.Shape shape3;
        private Telerik.Reporting.TextBox textBox20;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox16;
    }
}