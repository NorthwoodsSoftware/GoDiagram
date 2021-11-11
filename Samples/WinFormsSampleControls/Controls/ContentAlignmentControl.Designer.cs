
namespace WinFormsSampleControls.ContentAlignment {
  partial class ContentAlignmentControl {
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
      this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
      this.label8 = new System.Windows.Forms.Label();
      this.docBounds = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this.bottomBtn = new System.Windows.Forms.RadioButton();
      this.topBtn = new System.Windows.Forms.RadioButton();
      this.rightBtn = new System.Windows.Forms.RadioButton();
      this.leftBtn = new System.Windows.Forms.RadioButton();
      this.centerBtn = new System.Windows.Forms.RadioButton();
      this.noneBtn = new System.Windows.Forms.RadioButton();
      this.label2 = new System.Windows.Forms.Label();
      this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
      this.positionX = new System.Windows.Forms.TextBox();
      this.positionY = new System.Windows.Forms.TextBox();
      this.positionChangeBtn = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
      this.scale = new System.Windows.Forms.TextBox();
      this.scaleChangeBtn = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
      this.fixedX = new System.Windows.Forms.TextBox();
      this.fixedY = new System.Windows.Forms.TextBox();
      this.fixedW = new System.Windows.Forms.TextBox();
      this.fixedH = new System.Windows.Forms.TextBox();
      this.fixedBoundsSetBtn = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
      this.padT = new System.Windows.Forms.TextBox();
      this.padR = new System.Windows.Forms.TextBox();
      this.pabB = new System.Windows.Forms.TextBox();
      this.padL = new System.Windows.Forms.TextBox();
      this.paddingSetBtn = new System.Windows.Forms.Button();
      this.label6 = new System.Windows.Forms.Label();
      this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
      this.autoScaleNoneBtn = new System.Windows.Forms.RadioButton();
      this.autoScaleUniformBtn = new System.Windows.Forms.RadioButton();
      this.autoScaleUTFBtn = new System.Windows.Forms.RadioButton();
      this.label7 = new System.Windows.Forms.Label();
      this.zoomToFitBtn = new System.Windows.Forms.Button();
      this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.tableLayoutPanel9.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.tableLayoutPanel4.SuspendLayout();
      this.tableLayoutPanel5.SuspendLayout();
      this.tableLayoutPanel6.SuspendLayout();
      this.tableLayoutPanel7.SuspendLayout();
      this.tableLayoutPanel8.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.AutoScroll = true;
      this.tableLayoutPanel1.AutoScrollMargin = new System.Drawing.Size(50, 0);
      this.tableLayoutPanel1.AutoSize = true;
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 1);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 624);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // diagramControl1
      // 
      this.diagramControl1.AllowDrop = true;
      this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
      this.diagramControl1.Location = new System.Drawing.Point(3, 3);
      this.diagramControl1.Name = "diagramControl1";
      this.diagramControl1.Size = new System.Drawing.Size(494, 400);
      this.diagramControl1.TabIndex = 0;
      this.diagramControl1.Text = "diagramControl1";
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.AutoSize = true;
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel9, 0, 0);
      this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 2);
      this.tableLayoutPanel2.Controls.Add(this.label2, 0, 3);
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 4);
      this.tableLayoutPanel2.Controls.Add(this.label3, 0, 5);
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 6);
      this.tableLayoutPanel2.Controls.Add(this.label4, 0, 7);
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 0, 8);
      this.tableLayoutPanel2.Controls.Add(this.label5, 0, 9);
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel7, 0, 10);
      this.tableLayoutPanel2.Controls.Add(this.label6, 0, 11);
      this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel8, 0, 12);
      this.tableLayoutPanel2.Controls.Add(this.label7, 0, 13);
      this.tableLayoutPanel2.Controls.Add(this.zoomToFitBtn, 0, 14);
      this.tableLayoutPanel2.Location = new System.Drawing.Point(503, 3);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 15;
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
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel2.Size = new System.Drawing.Size(347, 453);
      this.tableLayoutPanel2.TabIndex = 2;
      // 
      // tableLayoutPanel9
      // 
      this.tableLayoutPanel9.AutoSize = true;
      this.tableLayoutPanel9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.tableLayoutPanel9.ColumnCount = 2;
      this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel9.Controls.Add(this.label8, 0, 0);
      this.tableLayoutPanel9.Controls.Add(this.docBounds, 1, 0);
      this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel9.Name = "tableLayoutPanel9";
      this.tableLayoutPanel9.RowCount = 1;
      this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel9.Size = new System.Drawing.Size(222, 15);
      this.tableLayoutPanel9.TabIndex = 18;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(3, 0);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(157, 15);
      this.label8.TabIndex = 0;
      this.label8.Text = "Diagram.DocumentBounds: ";
      // 
      // docBounds
      // 
      this.docBounds.AutoSize = true;
      this.docBounds.Location = new System.Drawing.Point(166, 0);
      this.docBounds.Name = "docBounds";
      this.docBounds.Size = new System.Drawing.Size(53, 15);
      this.docBounds.TabIndex = 1;
      this.docBounds.Text = "x, y, w, h";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(157, 15);
      this.label1.TabIndex = 13;
      this.label1.Text = "Diagram.ContentAlignment:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.AutoSize = true;
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.Controls.Add(this.bottomBtn, 1, 2);
      this.tableLayoutPanel3.Controls.Add(this.topBtn, 0, 2);
      this.tableLayoutPanel3.Controls.Add(this.rightBtn, 1, 1);
      this.tableLayoutPanel3.Controls.Add(this.leftBtn, 0, 1);
      this.tableLayoutPanel3.Controls.Add(this.centerBtn, 1, 0);
      this.tableLayoutPanel3.Controls.Add(this.noneBtn, 0, 0);
      this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 33);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 3;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel3.Size = new System.Drawing.Size(131, 75);
      this.tableLayoutPanel3.TabIndex = 2;
      // 
      // bottomBtn
      // 
      this.bottomBtn.AutoSize = true;
      this.bottomBtn.Location = new System.Drawing.Point(63, 53);
      this.bottomBtn.Name = "bottomBtn";
      this.bottomBtn.Size = new System.Drawing.Size(65, 19);
      this.bottomBtn.TabIndex = 5;
      this.bottomBtn.Text = "Bottom";
      this.bottomBtn.UseVisualStyleBackColor = true;
      // 
      // topBtn
      // 
      this.topBtn.AutoSize = true;
      this.topBtn.Location = new System.Drawing.Point(3, 53);
      this.topBtn.Name = "topBtn";
      this.topBtn.Size = new System.Drawing.Size(44, 19);
      this.topBtn.TabIndex = 4;
      this.topBtn.Text = "Top";
      this.topBtn.UseVisualStyleBackColor = true;
      // 
      // rightBtn
      // 
      this.rightBtn.AutoSize = true;
      this.rightBtn.Location = new System.Drawing.Point(63, 28);
      this.rightBtn.Name = "rightBtn";
      this.rightBtn.Size = new System.Drawing.Size(53, 19);
      this.rightBtn.TabIndex = 3;
      this.rightBtn.Text = "Right";
      this.rightBtn.UseVisualStyleBackColor = true;
      // 
      // leftBtn
      // 
      this.leftBtn.AutoSize = true;
      this.leftBtn.Location = new System.Drawing.Point(3, 28);
      this.leftBtn.Name = "leftBtn";
      this.leftBtn.Size = new System.Drawing.Size(45, 19);
      this.leftBtn.TabIndex = 2;
      this.leftBtn.Text = "Left";
      this.leftBtn.UseVisualStyleBackColor = true;
      // 
      // centerBtn
      // 
      this.centerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.centerBtn.AutoSize = true;
      this.centerBtn.Location = new System.Drawing.Point(63, 3);
      this.centerBtn.Name = "centerBtn";
      this.centerBtn.Size = new System.Drawing.Size(60, 19);
      this.centerBtn.TabIndex = 1;
      this.centerBtn.Text = "Center";
      this.centerBtn.UseVisualStyleBackColor = true;
      // 
      // noneBtn
      // 
      this.noneBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.noneBtn.AutoSize = true;
      this.noneBtn.Checked = true;
      this.noneBtn.Location = new System.Drawing.Point(3, 3);
      this.noneBtn.Name = "noneBtn";
      this.noneBtn.Size = new System.Drawing.Size(54, 19);
      this.noneBtn.TabIndex = 0;
      this.noneBtn.TabStop = true;
      this.noneBtn.Text = "None";
      this.noneBtn.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 111);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(101, 15);
      this.label2.TabIndex = 14;
      this.label2.Text = "Diagram.Position:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tableLayoutPanel4
      // 
      this.tableLayoutPanel4.ColumnCount = 3;
      this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
      this.tableLayoutPanel4.Controls.Add(this.positionX, 0, 0);
      this.tableLayoutPanel4.Controls.Add(this.positionY, 1, 0);
      this.tableLayoutPanel4.Controls.Add(this.positionChangeBtn, 2, 0);
      this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 129);
      this.tableLayoutPanel4.Name = "tableLayoutPanel4";
      this.tableLayoutPanel4.RowCount = 1;
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel4.Size = new System.Drawing.Size(232, 29);
      this.tableLayoutPanel4.TabIndex = 4;
      // 
      // positionX
      // 
      this.positionX.Location = new System.Drawing.Point(3, 3);
      this.positionX.Name = "positionX";
      this.positionX.Size = new System.Drawing.Size(69, 23);
      this.positionX.TabIndex = 0;
      // 
      // positionY
      // 
      this.positionY.Location = new System.Drawing.Point(78, 3);
      this.positionY.Name = "positionY";
      this.positionY.Size = new System.Drawing.Size(69, 23);
      this.positionY.TabIndex = 1;
      // 
      // positionChangeBtn
      // 
      this.positionChangeBtn.Location = new System.Drawing.Point(153, 3);
      this.positionChangeBtn.Name = "positionChangeBtn";
      this.positionChangeBtn.Size = new System.Drawing.Size(75, 23);
      this.positionChangeBtn.TabIndex = 2;
      this.positionChangeBtn.Text = "Change";
      this.positionChangeBtn.UseVisualStyleBackColor = true;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 161);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(85, 15);
      this.label3.TabIndex = 15;
      this.label3.Text = "Diagram.Scale:";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tableLayoutPanel5
      // 
      this.tableLayoutPanel5.ColumnCount = 2;
      this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
      this.tableLayoutPanel5.Controls.Add(this.scale, 0, 0);
      this.tableLayoutPanel5.Controls.Add(this.scaleChangeBtn, 1, 0);
      this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 179);
      this.tableLayoutPanel5.Name = "tableLayoutPanel5";
      this.tableLayoutPanel5.RowCount = 1;
      this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel5.Size = new System.Drawing.Size(158, 29);
      this.tableLayoutPanel5.TabIndex = 6;
      // 
      // scale
      // 
      this.scale.Location = new System.Drawing.Point(3, 3);
      this.scale.Name = "scale";
      this.scale.Size = new System.Drawing.Size(69, 23);
      this.scale.TabIndex = 0;
      this.scale.Text = "1";
      // 
      // scaleChangeBtn
      // 
      this.scaleChangeBtn.Location = new System.Drawing.Point(78, 3);
      this.scaleChangeBtn.Name = "scaleChangeBtn";
      this.scaleChangeBtn.Size = new System.Drawing.Size(75, 23);
      this.scaleChangeBtn.TabIndex = 1;
      this.scaleChangeBtn.Text = "Change";
      this.scaleChangeBtn.UseVisualStyleBackColor = true;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(3, 211);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(231, 15);
      this.label4.TabIndex = 16;
      this.label4.Text = "Diagram.FixedBounds (x, y, width, height):";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tableLayoutPanel6
      // 
      this.tableLayoutPanel6.ColumnCount = 5;
      this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
      this.tableLayoutPanel6.Controls.Add(this.fixedX, 0, 0);
      this.tableLayoutPanel6.Controls.Add(this.fixedY, 1, 0);
      this.tableLayoutPanel6.Controls.Add(this.fixedW, 2, 0);
      this.tableLayoutPanel6.Controls.Add(this.fixedH, 3, 0);
      this.tableLayoutPanel6.Controls.Add(this.fixedBoundsSetBtn, 4, 0);
      this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 229);
      this.tableLayoutPanel6.Name = "tableLayoutPanel6";
      this.tableLayoutPanel6.RowCount = 1;
      this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel6.Size = new System.Drawing.Size(341, 29);
      this.tableLayoutPanel6.TabIndex = 8;
      // 
      // fixedX
      // 
      this.fixedX.Location = new System.Drawing.Point(3, 3);
      this.fixedX.Name = "fixedX";
      this.fixedX.Size = new System.Drawing.Size(69, 23);
      this.fixedX.TabIndex = 0;
      this.fixedX.Text = "NaN";
      // 
      // fixedY
      // 
      this.fixedY.Location = new System.Drawing.Point(78, 3);
      this.fixedY.Name = "fixedY";
      this.fixedY.Size = new System.Drawing.Size(69, 23);
      this.fixedY.TabIndex = 1;
      this.fixedY.Text = "NaN";
      // 
      // fixedW
      // 
      this.fixedW.Location = new System.Drawing.Point(153, 3);
      this.fixedW.Name = "fixedW";
      this.fixedW.Size = new System.Drawing.Size(69, 23);
      this.fixedW.TabIndex = 2;
      this.fixedW.Text = "NaN";
      // 
      // fixedH
      // 
      this.fixedH.Location = new System.Drawing.Point(228, 3);
      this.fixedH.Name = "fixedH";
      this.fixedH.Size = new System.Drawing.Size(69, 23);
      this.fixedH.TabIndex = 3;
      this.fixedH.Text = "NaN";
      // 
      // fixedBoundsSetBtn
      // 
      this.fixedBoundsSetBtn.Location = new System.Drawing.Point(303, 3);
      this.fixedBoundsSetBtn.Name = "fixedBoundsSetBtn";
      this.fixedBoundsSetBtn.Size = new System.Drawing.Size(35, 23);
      this.fixedBoundsSetBtn.TabIndex = 4;
      this.fixedBoundsSetBtn.Text = "Set";
      this.fixedBoundsSetBtn.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(3, 261);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(231, 15);
      this.label5.TabIndex = 17;
      this.label5.Text = "Diagram.Padding (top, right, bottom, left):";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tableLayoutPanel7
      // 
      this.tableLayoutPanel7.ColumnCount = 5;
      this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
      this.tableLayoutPanel7.Controls.Add(this.padT, 0, 0);
      this.tableLayoutPanel7.Controls.Add(this.padR, 1, 0);
      this.tableLayoutPanel7.Controls.Add(this.pabB, 2, 0);
      this.tableLayoutPanel7.Controls.Add(this.padL, 3, 0);
      this.tableLayoutPanel7.Controls.Add(this.paddingSetBtn, 4, 0);
      this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 279);
      this.tableLayoutPanel7.Name = "tableLayoutPanel7";
      this.tableLayoutPanel7.RowCount = 1;
      this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel7.Size = new System.Drawing.Size(341, 29);
      this.tableLayoutPanel7.TabIndex = 10;
      // 
      // padT
      // 
      this.padT.Location = new System.Drawing.Point(3, 3);
      this.padT.Name = "padT";
      this.padT.Size = new System.Drawing.Size(69, 23);
      this.padT.TabIndex = 0;
      this.padT.Text = "5";
      // 
      // padR
      // 
      this.padR.Location = new System.Drawing.Point(78, 3);
      this.padR.Name = "padR";
      this.padR.Size = new System.Drawing.Size(69, 23);
      this.padR.TabIndex = 1;
      this.padR.Text = "5";
      // 
      // pabB
      // 
      this.pabB.Location = new System.Drawing.Point(153, 3);
      this.pabB.Name = "pabB";
      this.pabB.Size = new System.Drawing.Size(69, 23);
      this.pabB.TabIndex = 2;
      this.pabB.Text = "5";
      // 
      // padL
      // 
      this.padL.Location = new System.Drawing.Point(228, 3);
      this.padL.Name = "padL";
      this.padL.Size = new System.Drawing.Size(69, 23);
      this.padL.TabIndex = 3;
      this.padL.Text = "5";
      // 
      // paddingSetBtn
      // 
      this.paddingSetBtn.Location = new System.Drawing.Point(303, 3);
      this.paddingSetBtn.Name = "paddingSetBtn";
      this.paddingSetBtn.Size = new System.Drawing.Size(35, 23);
      this.paddingSetBtn.TabIndex = 4;
      this.paddingSetBtn.Text = "Set";
      this.paddingSetBtn.UseVisualStyleBackColor = true;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(3, 311);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(111, 15);
      this.label6.TabIndex = 19;
      this.label6.Text = "Diagram.AutoScale:";
      // 
      // tableLayoutPanel8
      // 
      this.tableLayoutPanel8.AutoSize = true;
      this.tableLayoutPanel8.ColumnCount = 1;
      this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel8.Controls.Add(this.autoScaleNoneBtn, 0, 0);
      this.tableLayoutPanel8.Controls.Add(this.autoScaleUniformBtn, 0, 1);
      this.tableLayoutPanel8.Controls.Add(this.autoScaleUTFBtn, 0, 2);
      this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 329);
      this.tableLayoutPanel8.Name = "tableLayoutPanel8";
      this.tableLayoutPanel8.RowCount = 3;
      this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel8.Size = new System.Drawing.Size(102, 75);
      this.tableLayoutPanel8.TabIndex = 20;
      // 
      // autoScaleNoneBtn
      // 
      this.autoScaleNoneBtn.AutoSize = true;
      this.autoScaleNoneBtn.Location = new System.Drawing.Point(3, 3);
      this.autoScaleNoneBtn.Name = "autoScaleNoneBtn";
      this.autoScaleNoneBtn.Size = new System.Drawing.Size(54, 19);
      this.autoScaleNoneBtn.TabIndex = 0;
      this.autoScaleNoneBtn.TabStop = true;
      this.autoScaleNoneBtn.Text = "None";
      this.autoScaleNoneBtn.UseVisualStyleBackColor = true;
      // 
      // autoScaleUniformBtn
      // 
      this.autoScaleUniformBtn.AutoSize = true;
      this.autoScaleUniformBtn.Location = new System.Drawing.Point(3, 28);
      this.autoScaleUniformBtn.Name = "autoScaleUniformBtn";
      this.autoScaleUniformBtn.Size = new System.Drawing.Size(69, 19);
      this.autoScaleUniformBtn.TabIndex = 1;
      this.autoScaleUniformBtn.TabStop = true;
      this.autoScaleUniformBtn.Text = "Uniform";
      this.autoScaleUniformBtn.UseVisualStyleBackColor = true;
      // 
      // autoScaleUTFBtn
      // 
      this.autoScaleUTFBtn.AutoSize = true;
      this.autoScaleUTFBtn.Location = new System.Drawing.Point(3, 53);
      this.autoScaleUTFBtn.Name = "autoScaleUTFBtn";
      this.autoScaleUTFBtn.Size = new System.Drawing.Size(96, 19);
      this.autoScaleUTFBtn.TabIndex = 2;
      this.autoScaleUTFBtn.TabStop = true;
      this.autoScaleUTFBtn.Text = "UniformToFill";
      this.autoScaleUTFBtn.UseVisualStyleBackColor = true;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(3, 407);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(287, 15);
      this.label7.TabIndex = 21;
      this.label7.Text = "(but no greater than CommandHandler.DefaultScale)";
      // 
      // zoomToFitBtn
      // 
      this.zoomToFitBtn.AutoSize = true;
      this.zoomToFitBtn.Location = new System.Drawing.Point(3, 425);
      this.zoomToFitBtn.Name = "zoomToFitBtn";
      this.zoomToFitBtn.Size = new System.Drawing.Size(80, 25);
      this.zoomToFitBtn.TabIndex = 22;
      this.zoomToFitBtn.Text = "Zoom To Fit";
      this.zoomToFitBtn.UseVisualStyleBackColor = true;
      // 
      // goWebBrowser1
      // 
      this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser1, 2);
      this.goWebBrowser1.CreationProperties = null;
      this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
      this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
      this.goWebBrowser1.Location = new System.Drawing.Point(3, 462);
      this.goWebBrowser1.Name = "goWebBrowser1";
      this.goWebBrowser1.Size = new System.Drawing.Size(994, 159);
      this.goWebBrowser1.TabIndex = 3;
      this.goWebBrowser1.ZoomFactor = 1D;
      // 
      // ContentAlignmentControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "ContentAlignmentControl";
      this.Size = new System.Drawing.Size(1000, 624);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.tableLayoutPanel9.ResumeLayout(false);
      this.tableLayoutPanel9.PerformLayout();
      this.tableLayoutPanel3.ResumeLayout(false);
      this.tableLayoutPanel3.PerformLayout();
      this.tableLayoutPanel4.ResumeLayout(false);
      this.tableLayoutPanel4.PerformLayout();
      this.tableLayoutPanel5.ResumeLayout(false);
      this.tableLayoutPanel5.PerformLayout();
      this.tableLayoutPanel6.ResumeLayout(false);
      this.tableLayoutPanel6.PerformLayout();
      this.tableLayoutPanel7.ResumeLayout(false);
      this.tableLayoutPanel7.PerformLayout();
      this.tableLayoutPanel8.ResumeLayout(false);
      this.tableLayoutPanel8.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.RadioButton bottomBtn;
    private System.Windows.Forms.RadioButton topBtn;
    private System.Windows.Forms.RadioButton rightBtn;
    private System.Windows.Forms.RadioButton leftBtn;
    private System.Windows.Forms.RadioButton centerBtn;
    private System.Windows.Forms.RadioButton noneBtn;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    private System.Windows.Forms.TextBox positionX;
    private System.Windows.Forms.TextBox positionY;
    private System.Windows.Forms.Button positionChangeBtn;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
    private System.Windows.Forms.TextBox scale;
    private System.Windows.Forms.Button scaleChangeBtn;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
    private System.Windows.Forms.TextBox fixedX;
    private System.Windows.Forms.TextBox fixedY;
    private System.Windows.Forms.TextBox fixedW;
    private System.Windows.Forms.TextBox fixedH;
    private System.Windows.Forms.Button fixedBoundsSetBtn;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
    private System.Windows.Forms.TextBox padT;
    private System.Windows.Forms.TextBox padR;
    private System.Windows.Forms.TextBox pabB;
    private System.Windows.Forms.TextBox padL;
    private System.Windows.Forms.Button paddingSetBtn;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label docBounds;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
    private System.Windows.Forms.RadioButton autoScaleNoneBtn;
    private System.Windows.Forms.RadioButton autoScaleUniformBtn;
    private System.Windows.Forms.RadioButton autoScaleUTFBtn;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Button zoomToFitBtn;
  }
}
