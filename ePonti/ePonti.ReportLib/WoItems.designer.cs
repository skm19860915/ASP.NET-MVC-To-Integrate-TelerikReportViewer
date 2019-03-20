namespace ePonti.ReportLib
{
    partial class WoItems
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group3 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule5 = new Telerik.Reporting.Drawing.StyleRule();
            this.areaGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.shape2 = new Telerik.Reporting.Shape();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.areaGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.divisionGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.divisionGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.actHrsCaptionTextBox = new Telerik.Reporting.TextBox();
            this.estHrsCaptionTextBox = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.taskDataTextBox = new Telerik.Reporting.TextBox();
            this.estHrsDataTextBox = new Telerik.Reporting.TextBox();
            this.actHrsDataTextBox = new Telerik.Reporting.TextBox();
            this.rptWoTaskData = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // areaGroupFooterSection
            // 
            this.areaGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.5D);
            this.areaGroupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.shape2,
            this.textBox3,
            this.textBox5});
            this.areaGroupFooterSection.Name = "areaGroupFooterSection";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Format = "{0:N2}";
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox1.StyleName = "Data";
            this.textBox1.TextWrap = false;
            this.textBox1.Value = "= Sum(Fields.EstHrs)";
            // 
            // shape2
            // 
            this.shape2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.40000006556510925D), Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D));
            this.shape2.Name = "shape2";
            this.shape2.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.6999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D));
            this.shape2.Style.Color = System.Drawing.Color.Gray;
            this.shape2.Style.LineColor = System.Drawing.Color.Silver;
            this.shape2.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.shape2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.shape2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            // 
            // textBox3
            // 
            this.textBox3.CanGrow = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox3.Style.Color = System.Drawing.Color.DimGray;
            this.textBox3.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.StyleName = "Caption";
            this.textBox3.TextWrap = false;
            this.textBox3.Value = "WO Hours";
            // 
            // textBox5
            // 
            this.textBox5.CanGrow = true;
            this.textBox5.Format = "{0:N2}";
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.5D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox5.Style.BorderColor.Bottom = System.Drawing.Color.Silver;
            this.textBox5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Dotted;
            this.textBox5.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.textBox5.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox5.StyleName = "Data";
            this.textBox5.TextWrap = false;
            this.textBox5.Value = "";
            // 
            // areaGroupHeaderSection
            // 
            this.areaGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D);
            this.areaGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2});
            this.areaGroupHeaderSection.Name = "areaGroupHeaderSection";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.textBox2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.textBox2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox2.StyleName = "Data";
            this.textBox2.TextWrap = false;
            this.textBox2.Value = "= Fields.Area";
            // 
            // divisionGroupFooterSection
            // 
            this.divisionGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.1607142835855484D);
            this.divisionGroupFooterSection.Name = "divisionGroupFooterSection";
            // 
            // divisionGroupHeaderSection
            // 
            this.divisionGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D);
            this.divisionGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox4,
            this.actHrsCaptionTextBox,
            this.estHrsCaptionTextBox,
            this.shape1});
            this.divisionGroupHeaderSection.Name = "divisionGroupHeaderSection";
            // 
            // textBox4
            // 
            this.textBox4.CanGrow = true;
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.textBox4.Style.Font.Name = "Microsoft Sans Serif";
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.textBox4.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.textBox4.StyleName = "Data";
            this.textBox4.TextWrap = false;
            this.textBox4.Value = "= Fields.Division";
            // 
            // actHrsCaptionTextBox
            // 
            this.actHrsCaptionTextBox.CanGrow = true;
            this.actHrsCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6D), Telerik.Reporting.Drawing.Unit.Inch(0.050000008195638657D));
            this.actHrsCaptionTextBox.Name = "actHrsCaptionTextBox";
            this.actHrsCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.actHrsCaptionTextBox.Style.Color = System.Drawing.Color.DimGray;
            this.actHrsCaptionTextBox.Style.Font.Name = "Microsoft Sans Serif";
            this.actHrsCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.actHrsCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.actHrsCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.actHrsCaptionTextBox.StyleName = "Caption";
            this.actHrsCaptionTextBox.TextWrap = false;
            this.actHrsCaptionTextBox.Value = "Act Hrs";
            // 
            // estHrsCaptionTextBox
            // 
            this.estHrsCaptionTextBox.CanGrow = true;
            this.estHrsCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5D), Telerik.Reporting.Drawing.Unit.Inch(0.050000008195638657D));
            this.estHrsCaptionTextBox.Name = "estHrsCaptionTextBox";
            this.estHrsCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.estHrsCaptionTextBox.Style.Color = System.Drawing.Color.DimGray;
            this.estHrsCaptionTextBox.Style.Font.Name = "Microsoft Sans Serif";
            this.estHrsCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.estHrsCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.estHrsCaptionTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.estHrsCaptionTextBox.StyleName = "Caption";
            this.estHrsCaptionTextBox.TextWrap = false;
            this.estHrsCaptionTextBox.Value = "Est Hrs";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D), Telerik.Reporting.Drawing.Unit.Inch(0.24996072053909302D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.6999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D));
            this.shape1.Style.Color = System.Drawing.Color.LightGray;
            this.shape1.Style.LineColor = System.Drawing.Color.Silver;
            this.shape1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.30000001192092896D);
            this.shape1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.shape1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.1607142835855484D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = false;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D);
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            this.labelsGroupHeaderSection.Style.Visible = false;
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.taskDataTextBox,
            this.estHrsDataTextBox,
            this.actHrsDataTextBox});
            this.detail.Name = "detail";
            // 
            // taskDataTextBox
            // 
            this.taskDataTextBox.CanGrow = true;
            this.taskDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.550000011920929D), Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D));
            this.taskDataTextBox.Name = "taskDataTextBox";
            this.taskDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.2999997138977051D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.taskDataTextBox.Style.Font.Name = "Microsoft Sans Serif";
            this.taskDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.taskDataTextBox.StyleName = "Data";
            this.taskDataTextBox.TextWrap = true;
            this.taskDataTextBox.Value = "= Fields.Task";
            // 
            // estHrsDataTextBox
            // 
            this.estHrsDataTextBox.CanGrow = true;
            this.estHrsDataTextBox.Format = "{0:N2}";
            this.estHrsDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5D), Telerik.Reporting.Drawing.Unit.Inch(0.05000000074505806D));
            this.estHrsDataTextBox.Name = "estHrsDataTextBox";
            this.estHrsDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.estHrsDataTextBox.Style.Font.Name = "Microsoft Sans Serif";
            this.estHrsDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.estHrsDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.estHrsDataTextBox.StyleName = "Data";
            this.estHrsDataTextBox.TextWrap = false;
            this.estHrsDataTextBox.Value = "= Fields.EstHrs";
            // 
            // actHrsDataTextBox
            // 
            this.actHrsDataTextBox.CanGrow = true;
            this.actHrsDataTextBox.Format = "{0:N2}";
            this.actHrsDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.5D), Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D));
            this.actHrsDataTextBox.Name = "actHrsDataTextBox";
            this.actHrsDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.19999998807907105D));
            this.actHrsDataTextBox.Style.BorderColor.Bottom = System.Drawing.Color.Silver;
            this.actHrsDataTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Dotted;
            this.actHrsDataTextBox.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.actHrsDataTextBox.Style.Font.Name = "Microsoft Sans Serif";
            this.actHrsDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.actHrsDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.actHrsDataTextBox.StyleName = "Data";
            this.actHrsDataTextBox.TextWrap = false;
            this.actHrsDataTextBox.Value = "";
            // 
            // rptWoTaskData
            // 
            this.rptWoTaskData.ConnectionString = "DefaultConnection";
            this.rptWoTaskData.Name = "rptWoTaskData";
            this.rptWoTaskData.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@WoID", System.Data.DbType.Int32, "= Parameters.WoID.Value")});
            this.rptWoTaskData.SelectCommand = "dbo.RptWoTasksByWoID";
            this.rptWoTaskData.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // WoItems
            // 
            this.DataSource = this.rptWoTaskData;
            group1.GroupFooter = this.areaGroupFooterSection;
            group1.GroupHeader = this.areaGroupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.Area"));
            group1.Name = "areaGroup";
            group2.GroupFooter = this.divisionGroupFooterSection;
            group2.GroupHeader = this.divisionGroupHeaderSection;
            group2.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.Division"));
            group2.Name = "divisionGroup";
            group3.GroupFooter = this.labelsGroupFooterSection;
            group3.GroupHeader = this.labelsGroupHeaderSection;
            group3.Name = "labelsGroup";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2,
            group3});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.areaGroupHeaderSection,
            this.areaGroupFooterSection,
            this.divisionGroupHeaderSection,
            this.divisionGroupFooterSection,
            this.labelsGroupHeaderSection,
            this.labelsGroupFooterSection,
            this.detail});
            this.Name = "WoItems";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D), Telerik.Reporting.Drawing.Unit.Inch(0.10000000149011612D));
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
        private Telerik.Reporting.GroupHeaderSection areaGroupHeaderSection;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.GroupFooterSection areaGroupFooterSection;
        private Telerik.Reporting.GroupHeaderSection divisionGroupHeaderSection;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.GroupFooterSection divisionGroupFooterSection;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.TextBox estHrsCaptionTextBox;
        private Telerik.Reporting.TextBox actHrsCaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox taskDataTextBox;
        private Telerik.Reporting.TextBox estHrsDataTextBox;
        private Telerik.Reporting.TextBox actHrsDataTextBox;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.Shape shape2;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.SqlDataSource rptWoTaskData;
    }
}