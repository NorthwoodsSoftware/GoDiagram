
namespace WinFormsExtensionControls.PackedLayoutSample {
  partial class PackedLayoutSampleControl {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLayoutHeight = new System.Windows.Forms.TextBox();
            this.lblLayoutHeight = new System.Windows.Forms.Label();
            this.txtLayoutWidth = new System.Windows.Forms.TextBox();
            this.lbllayoutWidth = new System.Windows.Forms.Label();
            this.lblGeneralProp = new System.Windows.Forms.Label();
            this.lblPackShape = new System.Windows.Forms.Label();
            this.lblPackMode = new System.Windows.Forms.Label();
            this.lblAspectRatio = new System.Windows.Forms.Label();
            this.txtAspectRatio = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.radBtnEllipticalPackShape = new System.Windows.Forms.RadioButton();
            this.radBtnRectPackShape = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.radBtnFit = new System.Windows.Forms.RadioButton();
            this.radBtnExpandToFit = new System.Windows.Forms.RadioButton();
            this.radBtnAspectRatio = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblNodeSortingProp = new System.Windows.Forms.Label();
            this.lblSortOrder = new System.Windows.Forms.Label();
            this.lblSortMode = new System.Windows.Forms.Label();
            this.lblPadding = new System.Windows.Forms.Label();
            this.lblSpacing = new System.Windows.Forms.Label();
            this.txtSpacing = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.radBtnDescending = new System.Windows.Forms.RadioButton();
            this.radBtnAscending = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.radBtnNone = new System.Windows.Forms.RadioButton();
            this.radBtnMaxSideLength = new System.Windows.Forms.RadioButton();
            this.radBtnArea = new System.Windows.Forms.RadioButton();
            this.lblCirclePacking = new System.Windows.Forms.Label();
            this.checkBxCircular = new System.Windows.Forms.CheckBox();
            this.checkBxSpiral = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblNodeGen = new System.Windows.Forms.Label();
            this.lblNumNodes = new System.Windows.Forms.Label();
            this.txtNumNodes = new System.Windows.Forms.TextBox();
            this.lblNodeShape = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.radBtnRectNodeShape = new System.Windows.Forms.RadioButton();
            this.radBtnEllipsesNodeShape = new System.Windows.Forms.RadioButton();
            this.lblMinSideLength = new System.Windows.Forms.Label();
            this.txtMinSideLength = new System.Windows.Forms.TextBox();
            this.lblMaxSideLength = new System.Windows.Forms.Label();
            this.txtMaxSideLength = new System.Windows.Forms.TextBox();
            this.checkBxSameWidthHeight = new System.Windows.Forms.CheckBox();
            this.btnRandomize = new System.Windows.Forms.Button();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1234, 1632);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.txtLayoutHeight, 1, 10);
            this.tableLayoutPanel2.Controls.Add(this.lblLayoutHeight, 0, 10);
            this.tableLayoutPanel2.Controls.Add(this.txtLayoutWidth, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.lbllayoutWidth, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.lblGeneralProp, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblPackShape, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblPackMode, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblAspectRatio, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.txtAspectRatio, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 0, 5);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 11;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(294, 279);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // txtLayoutHeight
            // 
            this.txtLayoutHeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLayoutHeight.Enabled = false;
            this.txtLayoutHeight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtLayoutHeight.Location = new System.Drawing.Point(94, 252);
            this.txtLayoutHeight.Name = "txtLayoutHeight";
            this.txtLayoutHeight.Size = new System.Drawing.Size(100, 23);
            this.txtLayoutHeight.TabIndex = 13;
            this.txtLayoutHeight.Text = "600";
            // 
            // lblLayoutHeight
            // 
            this.lblLayoutHeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLayoutHeight.AutoSize = true;
            this.lblLayoutHeight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLayoutHeight.Location = new System.Drawing.Point(3, 256);
            this.lblLayoutHeight.Name = "lblLayoutHeight";
            this.lblLayoutHeight.Size = new System.Drawing.Size(85, 15);
            this.lblLayoutHeight.TabIndex = 12;
            this.lblLayoutHeight.Text = "Layout Height:";
            // 
            // txtLayoutWidth
            // 
            this.txtLayoutWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLayoutWidth.Enabled = false;
            this.txtLayoutWidth.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtLayoutWidth.Location = new System.Drawing.Point(94, 222);
            this.txtLayoutWidth.Name = "txtLayoutWidth";
            this.txtLayoutWidth.Size = new System.Drawing.Size(100, 23);
            this.txtLayoutWidth.TabIndex = 11;
            this.txtLayoutWidth.Text = "600";
            // 
            // lbllayoutWidth
            // 
            this.lbllayoutWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbllayoutWidth.AutoSize = true;
            this.lbllayoutWidth.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbllayoutWidth.Location = new System.Drawing.Point(3, 226);
            this.lbllayoutWidth.Name = "lbllayoutWidth";
            this.lbllayoutWidth.Size = new System.Drawing.Size(81, 15);
            this.lbllayoutWidth.TabIndex = 10;
            this.lbllayoutWidth.Text = "Layout Width:";
            // 
            // lblGeneralProp
            // 
            this.lblGeneralProp.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lblGeneralProp, 2);
            this.lblGeneralProp.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblGeneralProp.Location = new System.Drawing.Point(3, 0);
            this.lblGeneralProp.Name = "lblGeneralProp";
            this.lblGeneralProp.Size = new System.Drawing.Size(135, 19);
            this.lblGeneralProp.TabIndex = 0;
            this.lblGeneralProp.Text = "General Properties";
            // 
            // lblPackShape
            // 
            this.lblPackShape.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lblPackShape, 2);
            this.lblPackShape.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPackShape.Location = new System.Drawing.Point(3, 19);
            this.lblPackShape.Name = "lblPackShape";
            this.lblPackShape.Size = new System.Drawing.Size(67, 15);
            this.lblPackShape.TabIndex = 1;
            this.lblPackShape.Text = "PackShape:";
            // 
            // lblPackMode
            // 
            this.lblPackMode.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lblPackMode, 2);
            this.lblPackMode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPackMode.Location = new System.Drawing.Point(3, 90);
            this.lblPackMode.Name = "lblPackMode";
            this.lblPackMode.Size = new System.Drawing.Size(66, 15);
            this.lblPackMode.TabIndex = 4;
            this.lblPackMode.Text = "PackMode:";
            // 
            // lblAspectRatio
            // 
            this.lblAspectRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAspectRatio.AutoSize = true;
            this.lblAspectRatio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblAspectRatio.Location = new System.Drawing.Point(3, 197);
            this.lblAspectRatio.Name = "lblAspectRatio";
            this.lblAspectRatio.Size = new System.Drawing.Size(76, 15);
            this.lblAspectRatio.TabIndex = 8;
            this.lblAspectRatio.Text = "Aspect Ratio:";
            // 
            // txtAspectRatio
            // 
            this.txtAspectRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtAspectRatio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtAspectRatio.Location = new System.Drawing.Point(94, 193);
            this.txtAspectRatio.Name = "txtAspectRatio";
            this.txtAspectRatio.Size = new System.Drawing.Size(100, 23);
            this.txtAspectRatio.TabIndex = 9;
            this.txtAspectRatio.Text = "1";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel5, 2);
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.radBtnEllipticalPackShape, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.radBtnRectPackShape, 0, 1);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 37);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel2.SetRowSpan(this.tableLayoutPanel5, 2);
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(325, 50);
            this.tableLayoutPanel5.TabIndex = 14;
            // 
            // radBtnEllipticalPackShape
            // 
            this.radBtnEllipticalPackShape.AutoSize = true;
            this.radBtnEllipticalPackShape.Checked = true;
            this.radBtnEllipticalPackShape.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnEllipticalPackShape.Location = new System.Drawing.Point(3, 3);
            this.radBtnEllipticalPackShape.Name = "radBtnEllipticalPackShape";
            this.radBtnEllipticalPackShape.Size = new System.Drawing.Size(69, 19);
            this.radBtnEllipticalPackShape.TabIndex = 0;
            this.radBtnEllipticalPackShape.TabStop = true;
            this.radBtnEllipticalPackShape.Text = "Elliptical";
            this.radBtnEllipticalPackShape.UseVisualStyleBackColor = true;
            // 
            // radBtnRectPackShape
            // 
            this.radBtnRectPackShape.AutoSize = true;
            this.radBtnRectPackShape.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnRectPackShape.Location = new System.Drawing.Point(3, 28);
            this.radBtnRectPackShape.Name = "radBtnRectPackShape";
            this.radBtnRectPackShape.Size = new System.Drawing.Size(88, 19);
            this.radBtnRectPackShape.TabIndex = 1;
            this.radBtnRectPackShape.Text = "Rectangular";
            this.radBtnRectPackShape.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel6, 2);
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.Controls.Add(this.radBtnFit, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.radBtnExpandToFit, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.radBtnAspectRatio, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 108);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel2.SetRowSpan(this.tableLayoutPanel6, 3);
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Size = new System.Drawing.Size(325, 79);
            this.tableLayoutPanel6.TabIndex = 15;
            // 
            // radBtnFit
            // 
            this.radBtnFit.AutoSize = true;
            this.radBtnFit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnFit.Location = new System.Drawing.Point(3, 53);
            this.radBtnFit.Name = "radBtnFit";
            this.radBtnFit.Size = new System.Drawing.Size(38, 19);
            this.radBtnFit.TabIndex = 2;
            this.radBtnFit.Text = "Fit";
            this.radBtnFit.UseVisualStyleBackColor = true;
            // 
            // radBtnExpandToFit
            // 
            this.radBtnExpandToFit.AutoSize = true;
            this.radBtnExpandToFit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnExpandToFit.Location = new System.Drawing.Point(3, 28);
            this.radBtnExpandToFit.Name = "radBtnExpandToFit";
            this.radBtnExpandToFit.Size = new System.Drawing.Size(95, 19);
            this.radBtnExpandToFit.TabIndex = 1;
            this.radBtnExpandToFit.Text = "Expand To Fit";
            this.radBtnExpandToFit.UseVisualStyleBackColor = true;
            // 
            // radBtnAspectRatio
            // 
            this.radBtnAspectRatio.AutoSize = true;
            this.radBtnAspectRatio.Checked = true;
            this.radBtnAspectRatio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnAspectRatio.Location = new System.Drawing.Point(3, 3);
            this.radBtnAspectRatio.Name = "radBtnAspectRatio";
            this.radBtnAspectRatio.Size = new System.Drawing.Size(91, 19);
            this.radBtnAspectRatio.TabIndex = 0;
            this.radBtnAspectRatio.TabStop = true;
            this.radBtnAspectRatio.Text = "Aspect Ratio";
            this.radBtnAspectRatio.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblNodeSortingProp, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblSortOrder, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblSortMode, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblPadding, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.lblSpacing, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.txtSpacing, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel7, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel8, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblCirclePacking, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.checkBxCircular, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.checkBxSpiral, 0, 10);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(303, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 11;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(294, 279);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // lblNodeSortingProp
            // 
            this.lblNodeSortingProp.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.lblNodeSortingProp, 3);
            this.lblNodeSortingProp.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNodeSortingProp.Location = new System.Drawing.Point(3, 0);
            this.lblNodeSortingProp.Name = "lblNodeSortingProp";
            this.lblNodeSortingProp.Size = new System.Drawing.Size(173, 19);
            this.lblNodeSortingProp.TabIndex = 0;
            this.lblNodeSortingProp.Text = "Node Sorting Properties";
            // 
            // lblSortOrder
            // 
            this.lblSortOrder.AutoSize = true;
            this.lblSortOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSortOrder.Location = new System.Drawing.Point(3, 19);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(61, 15);
            this.lblSortOrder.TabIndex = 1;
            this.lblSortOrder.Text = "SortOrder:";
            // 
            // lblSortMode
            // 
            this.lblSortMode.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.lblSortMode, 3);
            this.lblSortMode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSortMode.Location = new System.Drawing.Point(3, 50);
            this.lblSortMode.Name = "lblSortMode";
            this.lblSortMode.Size = new System.Drawing.Size(62, 15);
            this.lblSortMode.TabIndex = 4;
            this.lblSortMode.Text = "SortMode:";
            // 
            // lblPadding
            // 
            this.lblPadding.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.lblPadding, 3);
            this.lblPadding.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPadding.Location = new System.Drawing.Point(3, 145);
            this.lblPadding.Name = "lblPadding";
            this.lblPadding.Size = new System.Drawing.Size(173, 19);
            this.lblPadding.TabIndex = 8;
            this.lblPadding.Text = "Padding Between Nodes";
            // 
            // lblSpacing
            // 
            this.lblSpacing.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSpacing.AutoSize = true;
            this.lblSpacing.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSpacing.Location = new System.Drawing.Point(3, 171);
            this.lblSpacing.Name = "lblSpacing";
            this.lblSpacing.Size = new System.Drawing.Size(52, 15);
            this.lblSpacing.TabIndex = 9;
            this.lblSpacing.Text = "Spacing:";
            // 
            // txtSpacing
            // 
            this.txtSpacing.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel3.SetColumnSpan(this.txtSpacing, 2);
            this.txtSpacing.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSpacing.Location = new System.Drawing.Point(70, 167);
            this.txtSpacing.Name = "txtSpacing";
            this.txtSpacing.Size = new System.Drawing.Size(100, 23);
            this.txtSpacing.TabIndex = 10;
            this.txtSpacing.Text = "0";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel7, 2);
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.Controls.Add(this.radBtnDescending, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.radBtnAscending, 1, 0);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(70, 22);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.Size = new System.Drawing.Size(313, 25);
            this.tableLayoutPanel7.TabIndex = 11;
            // 
            // radBtnDescending
            // 
            this.radBtnDescending.AutoSize = true;
            this.radBtnDescending.Checked = true;
            this.radBtnDescending.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnDescending.Location = new System.Drawing.Point(3, 3);
            this.radBtnDescending.Name = "radBtnDescending";
            this.radBtnDescending.Size = new System.Drawing.Size(87, 19);
            this.radBtnDescending.TabIndex = 0;
            this.radBtnDescending.TabStop = true;
            this.radBtnDescending.Text = "Descending";
            this.radBtnDescending.UseVisualStyleBackColor = true;
            // 
            // radBtnAscending
            // 
            this.radBtnAscending.AutoSize = true;
            this.radBtnAscending.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnAscending.Location = new System.Drawing.Point(96, 3);
            this.radBtnAscending.Name = "radBtnAscending";
            this.radBtnAscending.Size = new System.Drawing.Size(81, 19);
            this.radBtnAscending.TabIndex = 1;
            this.radBtnAscending.Text = "Ascending";
            this.radBtnAscending.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel8, 3);
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.Controls.Add(this.radBtnNone, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.radBtnMaxSideLength, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.radBtnArea, 0, 2);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 68);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 3;
            this.tableLayoutPanel3.SetRowSpan(this.tableLayoutPanel8, 3);
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.Size = new System.Drawing.Size(438, 74);
            this.tableLayoutPanel8.TabIndex = 12;
            // 
            // radBtnNone
            // 
            this.radBtnNone.AutoSize = true;
            this.radBtnNone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnNone.Location = new System.Drawing.Point(3, 3);
            this.radBtnNone.Name = "radBtnNone";
            this.radBtnNone.Size = new System.Drawing.Size(158, 19);
            this.radBtnNone.TabIndex = 0;
            this.radBtnNone.Text = "None (do not sort nodes)";
            this.radBtnNone.UseVisualStyleBackColor = true;
            // 
            // radBtnMaxSideLength
            // 
            this.radBtnMaxSideLength.AutoSize = true;
            this.radBtnMaxSideLength.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnMaxSideLength.Location = new System.Drawing.Point(3, 28);
            this.radBtnMaxSideLength.Name = "radBtnMaxSideLength";
            this.radBtnMaxSideLength.Size = new System.Drawing.Size(113, 19);
            this.radBtnMaxSideLength.TabIndex = 1;
            this.radBtnMaxSideLength.Text = "Max Side Length";
            this.radBtnMaxSideLength.UseVisualStyleBackColor = true;
            // 
            // radBtnArea
            // 
            this.radBtnArea.AutoSize = true;
            this.radBtnArea.Checked = true;
            this.radBtnArea.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnArea.Location = new System.Drawing.Point(3, 53);
            this.radBtnArea.Name = "radBtnArea";
            this.radBtnArea.Size = new System.Drawing.Size(49, 19);
            this.radBtnArea.TabIndex = 2;
            this.radBtnArea.TabStop = true;
            this.radBtnArea.Text = "Area";
            this.radBtnArea.UseVisualStyleBackColor = true;
            // 
            // lblCirclePacking
            // 
            this.lblCirclePacking.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.lblCirclePacking, 3);
            this.lblCirclePacking.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCirclePacking.Location = new System.Drawing.Point(3, 193);
            this.lblCirclePacking.Name = "lblCirclePacking";
            this.lblCirclePacking.Size = new System.Drawing.Size(104, 19);
            this.lblCirclePacking.TabIndex = 13;
            this.lblCirclePacking.Text = "Circle Packing";
            // 
            // checkBxCircular
            // 
            this.checkBxCircular.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.checkBxCircular, 3);
            this.checkBxCircular.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBxCircular.Location = new System.Drawing.Point(3, 215);
            this.checkBxCircular.Name = "checkBxCircular";
            this.checkBxCircular.Size = new System.Drawing.Size(119, 19);
            this.checkBxCircular.TabIndex = 14;
            this.checkBxCircular.Text = "hasCircularNodes";
            this.checkBxCircular.UseVisualStyleBackColor = true;
            // 
            // checkBxSpiral
            // 
            this.checkBxSpiral.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.checkBxSpiral, 3);
            this.checkBxSpiral.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBxSpiral.Location = new System.Drawing.Point(3, 240);
            this.checkBxSpiral.Name = "checkBxSpiral";
            this.checkBxSpiral.Size = new System.Drawing.Size(101, 19);
            this.checkBxSpiral.TabIndex = 15;
            this.checkBxSpiral.Text = "isSpiralPacked";
            this.checkBxSpiral.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.52599F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.47401F));
            this.tableLayoutPanel4.Controls.Add(this.lblNodeGen, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblNumNodes, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtNumNodes, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblNodeShape, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel9, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblMinSideLength, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.txtMinSideLength, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.lblMaxSideLength, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.txtMaxSideLength, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.checkBxSameWidthHeight, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.btnRandomize, 0, 7);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(603, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 8;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 57.26496F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.73504F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(294, 279);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // lblNodeGen
            // 
            this.lblNodeGen.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.lblNodeGen, 2);
            this.lblNodeGen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNodeGen.Location = new System.Drawing.Point(3, 0);
            this.lblNodeGen.Name = "lblNodeGen";
            this.lblNodeGen.Size = new System.Drawing.Size(124, 19);
            this.lblNodeGen.TabIndex = 0;
            this.lblNodeGen.Text = "Node Generation";
            // 
            // lblNumNodes
            // 
            this.lblNumNodes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNumNodes.AutoSize = true;
            this.lblNumNodes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNumNodes.Location = new System.Drawing.Point(3, 23);
            this.lblNumNodes.Name = "lblNumNodes";
            this.lblNumNodes.Size = new System.Drawing.Size(105, 15);
            this.lblNumNodes.TabIndex = 1;
            this.lblNumNodes.Text = "Number of Nodes:";
            // 
            // txtNumNodes
            // 
            this.txtNumNodes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtNumNodes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNumNodes.Location = new System.Drawing.Point(192, 25);
            this.txtNumNodes.Name = "txtNumNodes";
            this.txtNumNodes.Size = new System.Drawing.Size(99, 23);
            this.txtNumNodes.TabIndex = 2;
            this.txtNumNodes.Text = "100";
            // 
            // lblNodeShape
            // 
            this.lblNodeShape.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNodeShape.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.lblNodeShape, 2);
            this.lblNodeShape.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNodeShape.Location = new System.Drawing.Point(3, 55);
            this.lblNodeShape.Name = "lblNodeShape";
            this.lblNodeShape.Size = new System.Drawing.Size(73, 15);
            this.lblNodeShape.TabIndex = 3;
            this.lblNodeShape.Text = "Node shape:";
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel9.ColumnCount = 1;
            this.tableLayoutPanel4.SetColumnSpan(this.tableLayoutPanel9, 2);
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.Controls.Add(this.radBtnRectNodeShape, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.radBtnEllipsesNodeShape, 0, 1);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 90);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.Size = new System.Drawing.Size(288, 53);
            this.tableLayoutPanel9.TabIndex = 4;
            // 
            // radBtnRectNodeShape
            // 
            this.radBtnRectNodeShape.AutoSize = true;
            this.radBtnRectNodeShape.Checked = true;
            this.radBtnRectNodeShape.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnRectNodeShape.Location = new System.Drawing.Point(3, 3);
            this.radBtnRectNodeShape.Name = "radBtnRectNodeShape";
            this.radBtnRectNodeShape.Size = new System.Drawing.Size(82, 19);
            this.radBtnRectNodeShape.TabIndex = 0;
            this.radBtnRectNodeShape.TabStop = true;
            this.radBtnRectNodeShape.Text = "Rectangles";
            this.radBtnRectNodeShape.UseVisualStyleBackColor = true;
            // 
            // radBtnEllipsesNodeShape
            // 
            this.radBtnEllipsesNodeShape.AutoSize = true;
            this.radBtnEllipsesNodeShape.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnEllipsesNodeShape.Location = new System.Drawing.Point(3, 28);
            this.radBtnEllipsesNodeShape.Name = "radBtnEllipsesNodeShape";
            this.radBtnEllipsesNodeShape.Size = new System.Drawing.Size(63, 19);
            this.radBtnEllipsesNodeShape.TabIndex = 1;
            this.radBtnEllipsesNodeShape.Text = "Ellipses";
            this.radBtnEllipsesNodeShape.UseVisualStyleBackColor = true;
            // 
            // lblMinSideLength
            // 
            this.lblMinSideLength.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMinSideLength.AutoSize = true;
            this.lblMinSideLength.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMinSideLength.Location = new System.Drawing.Point(3, 152);
            this.lblMinSideLength.Name = "lblMinSideLength";
            this.lblMinSideLength.Size = new System.Drawing.Size(125, 15);
            this.lblMinSideLength.TabIndex = 5;
            this.lblMinSideLength.Text = "Minimum Side Length";
            // 
            // txtMinSideLength
            // 
            this.txtMinSideLength.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMinSideLength.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMinSideLength.Location = new System.Drawing.Point(192, 149);
            this.txtMinSideLength.Name = "txtMinSideLength";
            this.txtMinSideLength.Size = new System.Drawing.Size(99, 23);
            this.txtMinSideLength.TabIndex = 6;
            this.txtMinSideLength.Text = "30";
            // 
            // lblMaxSideLength
            // 
            this.lblMaxSideLength.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMaxSideLength.AutoSize = true;
            this.lblMaxSideLength.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMaxSideLength.Location = new System.Drawing.Point(3, 181);
            this.lblMaxSideLength.Name = "lblMaxSideLength";
            this.lblMaxSideLength.Size = new System.Drawing.Size(127, 15);
            this.lblMaxSideLength.TabIndex = 7;
            this.lblMaxSideLength.Text = "Maximum Side Length";
            // 
            // txtMaxSideLength
            // 
            this.txtMaxSideLength.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMaxSideLength.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMaxSideLength.Location = new System.Drawing.Point(192, 177);
            this.txtMaxSideLength.Name = "txtMaxSideLength";
            this.txtMaxSideLength.Size = new System.Drawing.Size(99, 23);
            this.txtMaxSideLength.TabIndex = 8;
            this.txtMaxSideLength.Text = "50";
            // 
            // checkBxSameWidthHeight
            // 
            this.checkBxSameWidthHeight.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.checkBxSameWidthHeight, 2);
            this.checkBxSameWidthHeight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBxSameWidthHeight.Location = new System.Drawing.Point(3, 207);
            this.checkBxSameWidthHeight.Name = "checkBxSameWidthHeight";
            this.checkBxSameWidthHeight.Size = new System.Drawing.Size(127, 19);
            this.checkBxSameWidthHeight.TabIndex = 9;
            this.checkBxSameWidthHeight.Text = "Same width/height";
            this.checkBxSameWidthHeight.UseVisualStyleBackColor = true;
            // 
            // btnRandomize
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.btnRandomize, 2);
            this.btnRandomize.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRandomize.Location = new System.Drawing.Point(3, 236);
            this.btnRandomize.Name = "btnRandomize";
            this.btnRandomize.Size = new System.Drawing.Size(200, 35);
            this.btnRandomize.TabIndex = 10;
            this.btnRandomize.Text = "Randomize Graph";
            this.btnRandomize.UseVisualStyleBackColor = true;
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.tableLayoutPanel1.SetColumnSpan(this.diagramControl1, 4);
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 288);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(1228, 560);
            this.diagramControl1.TabIndex = 3;
            this.diagramControl1.Text = "diagramControl1";
            // 
            // goWebBrowser1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser1, 4);
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 854);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(1228, 775);
            this.goWebBrowser1.TabIndex = 4;
            this.goWebBrowser1.ZoomFactor = 1D;
            // 
            // PackedLayoutSampleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PackedLayoutSampleControl";
            this.Size = new System.Drawing.Size(1234, 1632);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.TextBox txtLayoutHeight;
    private System.Windows.Forms.Label lblLayoutHeight;
    private System.Windows.Forms.TextBox txtLayoutWidth;
    private System.Windows.Forms.Label lbllayoutWidth;
    private System.Windows.Forms.Label lblGeneralProp;
    private System.Windows.Forms.Label lblPackShape;
    private System.Windows.Forms.Label lblPackMode;
    private System.Windows.Forms.Label lblAspectRatio;
    private System.Windows.Forms.TextBox txtAspectRatio;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
    private System.Windows.Forms.RadioButton radBtnEllipticalPackShape;
    private System.Windows.Forms.RadioButton radBtnRectPackShape;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
    private System.Windows.Forms.RadioButton radBtnFit;
    private System.Windows.Forms.RadioButton radBtnExpandToFit;
    private System.Windows.Forms.RadioButton radBtnAspectRatio;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.Label lblNodeSortingProp;
    private System.Windows.Forms.Label lblSortOrder;
    private System.Windows.Forms.Label lblSortMode;
    private System.Windows.Forms.Label lblPadding;
    private System.Windows.Forms.Label lblSpacing;
    private System.Windows.Forms.TextBox txtSpacing;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
    private System.Windows.Forms.RadioButton radBtnDescending;
    private System.Windows.Forms.RadioButton radBtnAscending;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
    private System.Windows.Forms.RadioButton radBtnNone;
    private System.Windows.Forms.RadioButton radBtnMaxSideLength;
    private System.Windows.Forms.RadioButton radBtnArea;
    private System.Windows.Forms.Label lblCirclePacking;
    private System.Windows.Forms.CheckBox checkBxCircular;
    private System.Windows.Forms.CheckBox checkBxSpiral;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.Label lblNodeGen;
    private System.Windows.Forms.Label lblNumNodes;
    private System.Windows.Forms.TextBox txtNumNodes;
    private System.Windows.Forms.Label lblNodeShape;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
    private System.Windows.Forms.RadioButton radBtnRectNodeShape;
    private System.Windows.Forms.RadioButton radBtnEllipsesNodeShape;
    private System.Windows.Forms.Label lblMinSideLength;
    private System.Windows.Forms.TextBox txtMinSideLength;
    private System.Windows.Forms.Label lblMaxSideLength;
    private System.Windows.Forms.TextBox txtMaxSideLength;
    private System.Windows.Forms.CheckBox checkBxSameWidthHeight;
    private System.Windows.Forms.Button btnRandomize;
  }
}
