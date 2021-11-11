
namespace WinFormsSampleControls.LayeredDigraph {
  partial class LayeredDigraphControl {
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtMaxNodes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.generateDiagraph = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMinNodes = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.setPortSpots = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.packExpand = new System.Windows.Forms.CheckBox();
            this.packStraighten = new System.Windows.Forms.CheckBox();
            this.packMedian = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.aggressiveMore = new System.Windows.Forms.RadioButton();
            this.aggressiveLess = new System.Windows.Forms.RadioButton();
            this.aggressiveNone = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.initNaive = new System.Windows.Forms.RadioButton();
            this.initDepthFirstIn = new System.Windows.Forms.RadioButton();
            this.initDepthFirstOut = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.txtColumnSpacing = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLayerSpacing = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.up = new System.Windows.Forms.RadioButton();
            this.left = new System.Windows.Forms.RadioButton();
            this.down = new System.Windows.Forms.RadioButton();
            this.right = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.cycleGreedy = new System.Windows.Forms.RadioButton();
            this.cycleDepthFirst = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.layerLongestPathSink = new System.Windows.Forms.RadioButton();
            this.layerLongestPathSource = new System.Windows.Forms.RadioButton();
            this.layerOptimalLinkLength = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 923);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(994, 344);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.generateDiagraph, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(192, 141);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.txtMaxNodes, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 68);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(186, 29);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // txtMaxNodes
            // 
            this.txtMaxNodes.Location = new System.Drawing.Point(96, 3);
            this.txtMaxNodes.Name = "txtMaxNodes";
            this.txtMaxNodes.Size = new System.Drawing.Size(87, 23);
            this.txtMaxNodes.TabIndex = 4;
            this.txtMaxNodes.Text = "100";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "MaxNodes:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // generateDiagraph
            // 
            this.generateDiagraph.Location = new System.Drawing.Point(3, 103);
            this.generateDiagraph.Name = "generateDiagraph";
            this.generateDiagraph.Size = new System.Drawing.Size(129, 23);
            this.generateDiagraph.TabIndex = 1;
            this.generateDiagraph.Text = "Generate Diagraph";
            this.generateDiagraph.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtMinNodes, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 33);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(186, 29);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "MinNodes:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMinNodes
            // 
            this.txtMinNodes.Location = new System.Drawing.Point(96, 3);
            this.txtMinNodes.Name = "txtMinNodes";
            this.txtMinNodes.Size = new System.Drawing.Size(87, 23);
            this.txtMinNodes.TabIndex = 3;
            this.txtMinNodes.Text = "20";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 30);
            this.label1.TabIndex = 4;
            this.label1.Text = "New Graph";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.setPortSpots, 0, 9);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel13, 0, 8);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel12, 0, 7);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel11, 0, 6);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel8, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel9, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel10, 0, 4);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel14, 0, 5);
            this.tableLayoutPanel6.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(201, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 10;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(790, 338);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // setPortSpots
            // 
            this.setPortSpots.AutoSize = true;
            this.setPortSpots.Checked = true;
            this.setPortSpots.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setPortSpots.Location = new System.Drawing.Point(3, 313);
            this.setPortSpots.Name = "setPortSpots";
            this.setPortSpots.Size = new System.Drawing.Size(93, 19);
            this.setPortSpots.TabIndex = 10;
            this.setPortSpots.Text = "SetPortSpots";
            this.setPortSpots.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 4;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel13.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.packExpand, 3, 0);
            this.tableLayoutPanel13.Controls.Add(this.packStraighten, 2, 0);
            this.tableLayoutPanel13.Controls.Add(this.packMedian, 1, 0);
            this.tableLayoutPanel13.Location = new System.Drawing.Point(3, 278);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(605, 29);
            this.tableLayoutPanel13.TabIndex = 9;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(3, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 29);
            this.label12.TabIndex = 6;
            this.label12.Text = "Pack:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // packExpand
            // 
            this.packExpand.AutoSize = true;
            this.packExpand.Checked = true;
            this.packExpand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.packExpand.Location = new System.Drawing.Point(443, 3);
            this.packExpand.Name = "packExpand";
            this.packExpand.Size = new System.Drawing.Size(90, 19);
            this.packExpand.TabIndex = 3;
            this.packExpand.Text = "PackExpand";
            this.packExpand.UseVisualStyleBackColor = true;
            // 
            // packStraighten
            // 
            this.packStraighten.AutoSize = true;
            this.packStraighten.Checked = true;
            this.packStraighten.CheckState = System.Windows.Forms.CheckState.Checked;
            this.packStraighten.Location = new System.Drawing.Point(273, 3);
            this.packStraighten.Name = "packStraighten";
            this.packStraighten.Size = new System.Drawing.Size(105, 19);
            this.packStraighten.TabIndex = 2;
            this.packStraighten.Text = "PackStraighten";
            this.packStraighten.UseVisualStyleBackColor = true;
            // 
            // packMedian
            // 
            this.packMedian.AutoSize = true;
            this.packMedian.Checked = true;
            this.packMedian.CheckState = System.Windows.Forms.CheckState.Checked;
            this.packMedian.Location = new System.Drawing.Point(103, 3);
            this.packMedian.Name = "packMedian";
            this.packMedian.Size = new System.Drawing.Size(91, 19);
            this.packMedian.TabIndex = 1;
            this.packMedian.Text = "PackMedian";
            this.packMedian.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 4;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel12.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.aggressiveMore, 3, 0);
            this.tableLayoutPanel12.Controls.Add(this.aggressiveLess, 2, 0);
            this.tableLayoutPanel12.Controls.Add(this.aggressiveNone, 1, 0);
            this.tableLayoutPanel12.Location = new System.Drawing.Point(3, 243);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(605, 29);
            this.tableLayoutPanel12.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 29);
            this.label11.TabIndex = 6;
            this.label11.Text = "Aggresive:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // aggressiveMore
            // 
            this.aggressiveMore.AutoSize = true;
            this.aggressiveMore.Location = new System.Drawing.Point(443, 3);
            this.aggressiveMore.Name = "aggressiveMore";
            this.aggressiveMore.Size = new System.Drawing.Size(110, 19);
            this.aggressiveMore.TabIndex = 4;
            this.aggressiveMore.Text = "AggressiveMore";
            this.aggressiveMore.UseVisualStyleBackColor = true;
            // 
            // aggressiveLess
            // 
            this.aggressiveLess.AutoSize = true;
            this.aggressiveLess.Checked = true;
            this.aggressiveLess.Location = new System.Drawing.Point(273, 3);
            this.aggressiveLess.Name = "aggressiveLess";
            this.aggressiveLess.Size = new System.Drawing.Size(104, 19);
            this.aggressiveLess.TabIndex = 3;
            this.aggressiveLess.TabStop = true;
            this.aggressiveLess.Text = "AggressiveLess";
            this.aggressiveLess.UseVisualStyleBackColor = true;
            // 
            // aggressiveNone
            // 
            this.aggressiveNone.AutoSize = true;
            this.aggressiveNone.Location = new System.Drawing.Point(103, 3);
            this.aggressiveNone.Name = "aggressiveNone";
            this.aggressiveNone.Size = new System.Drawing.Size(111, 19);
            this.aggressiveNone.TabIndex = 2;
            this.aggressiveNone.Text = "AggressiveNone";
            this.aggressiveNone.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 4;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel11.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.initNaive, 3, 0);
            this.tableLayoutPanel11.Controls.Add(this.initDepthFirstIn, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.initDepthFirstOut, 1, 0);
            this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 208);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(605, 29);
            this.tableLayoutPanel11.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 29);
            this.label10.TabIndex = 6;
            this.label10.Text = "Initialize:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // initNaive
            // 
            this.initNaive.AutoSize = true;
            this.initNaive.Location = new System.Drawing.Point(443, 3);
            this.initNaive.Name = "initNaive";
            this.initNaive.Size = new System.Drawing.Size(72, 19);
            this.initNaive.TabIndex = 4;
            this.initNaive.Text = "InitNaive";
            this.initNaive.UseVisualStyleBackColor = true;
            // 
            // initDepthFirstIn
            // 
            this.initDepthFirstIn.AutoSize = true;
            this.initDepthFirstIn.Location = new System.Drawing.Point(273, 3);
            this.initDepthFirstIn.Name = "initDepthFirstIn";
            this.initDepthFirstIn.Size = new System.Drawing.Size(106, 19);
            this.initDepthFirstIn.TabIndex = 3;
            this.initDepthFirstIn.Text = "InitDepthFirstIn";
            this.initDepthFirstIn.UseVisualStyleBackColor = true;
            // 
            // initDepthFirstOut
            // 
            this.initDepthFirstOut.AutoSize = true;
            this.initDepthFirstOut.Checked = true;
            this.initDepthFirstOut.Location = new System.Drawing.Point(103, 3);
            this.initDepthFirstOut.Name = "initDepthFirstOut";
            this.initDepthFirstOut.Size = new System.Drawing.Size(116, 19);
            this.initDepthFirstOut.TabIndex = 2;
            this.initDepthFirstOut.TabStop = true;
            this.initDepthFirstOut.Text = "InitDepthFirstOut";
            this.initDepthFirstOut.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel8.Controls.Add(this.txtColumnSpacing, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 103);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(211, 29);
            this.tableLayoutPanel8.TabIndex = 4;
            // 
            // txtColumnSpacing
            // 
            this.txtColumnSpacing.Location = new System.Drawing.Point(108, 3);
            this.txtColumnSpacing.Name = "txtColumnSpacing";
            this.txtColumnSpacing.Size = new System.Drawing.Size(87, 23);
            this.txtColumnSpacing.TabIndex = 7;
            this.txtColumnSpacing.Text = "25";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 29);
            this.label7.TabIndex = 6;
            this.label7.Text = "ColumnSpacing:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel7.Controls.Add(this.txtLayerSpacing, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 68);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(211, 29);
            this.tableLayoutPanel7.TabIndex = 3;
            // 
            // txtLayerSpacing
            // 
            this.txtLayerSpacing.Location = new System.Drawing.Point(108, 3);
            this.txtLayerSpacing.Name = "txtLayerSpacing";
            this.txtLayerSpacing.Size = new System.Drawing.Size(87, 23);
            this.txtLayerSpacing.TabIndex = 7;
            this.txtLayerSpacing.Text = "25";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 29);
            this.label6.TabIndex = 6;
            this.label6.Text = "LayerSpacing:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 5;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel9.Controls.Add(this.up, 4, 0);
            this.tableLayoutPanel9.Controls.Add(this.left, 3, 0);
            this.tableLayoutPanel9.Controls.Add(this.down, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.right, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 33);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(491, 29);
            this.tableLayoutPanel9.TabIndex = 1;
            // 
            // up
            // 
            this.up.AutoSize = true;
            this.up.Location = new System.Drawing.Point(403, 3);
            this.up.Name = "up";
            this.up.Size = new System.Drawing.Size(69, 19);
            this.up.TabIndex = 4;
            this.up.Text = "Up (270)";
            this.up.UseVisualStyleBackColor = true;
            // 
            // left
            // 
            this.left.AutoSize = true;
            this.left.Location = new System.Drawing.Point(303, 3);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(74, 19);
            this.left.TabIndex = 3;
            this.left.Text = "Left (180)";
            this.left.UseVisualStyleBackColor = true;
            // 
            // down
            // 
            this.down.AutoSize = true;
            this.down.Location = new System.Drawing.Point(203, 3);
            this.down.Name = "down";
            this.down.Size = new System.Drawing.Size(79, 19);
            this.down.TabIndex = 2;
            this.down.Text = "Down (90)";
            this.down.UseVisualStyleBackColor = true;
            // 
            // right
            // 
            this.right.AutoSize = true;
            this.right.Checked = true;
            this.right.Location = new System.Drawing.Point(103, 3);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(70, 19);
            this.right.TabIndex = 1;
            this.right.TabStop = true;
            this.right.Text = "Right (0)";
            this.right.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 29);
            this.label5.TabIndex = 5;
            this.label5.Text = "Direction:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 3;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel10.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.cycleGreedy, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.cycleDepthFirst, 1, 0);
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 138);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(429, 29);
            this.tableLayoutPanel10.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 29);
            this.label8.TabIndex = 6;
            this.label8.Text = "CycleRemove:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cycleGreedy
            // 
            this.cycleGreedy.AutoSize = true;
            this.cycleGreedy.Location = new System.Drawing.Point(273, 3);
            this.cycleGreedy.Name = "cycleGreedy";
            this.cycleGreedy.Size = new System.Drawing.Size(91, 19);
            this.cycleGreedy.TabIndex = 2;
            this.cycleGreedy.Text = "CycleGreedy";
            this.cycleGreedy.UseVisualStyleBackColor = true;
            // 
            // cycleDepthFirst
            // 
            this.cycleDepthFirst.AutoSize = true;
            this.cycleDepthFirst.Checked = true;
            this.cycleDepthFirst.Location = new System.Drawing.Point(103, 3);
            this.cycleDepthFirst.Name = "cycleDepthFirst";
            this.cycleDepthFirst.Size = new System.Drawing.Size(108, 19);
            this.cycleDepthFirst.TabIndex = 1;
            this.cycleDepthFirst.TabStop = true;
            this.cycleDepthFirst.Text = "CycleDepthFirst";
            this.cycleDepthFirst.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 4;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel14.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.layerLongestPathSink, 3, 0);
            this.tableLayoutPanel14.Controls.Add(this.layerLongestPathSource, 2, 0);
            this.tableLayoutPanel14.Controls.Add(this.layerOptimalLinkLength, 1, 0);
            this.tableLayoutPanel14.Location = new System.Drawing.Point(3, 173);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(605, 29);
            this.tableLayoutPanel14.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 29);
            this.label9.TabIndex = 6;
            this.label9.Text = "Layering:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // layerLongestPathSink
            // 
            this.layerLongestPathSink.AutoSize = true;
            this.layerLongestPathSink.Location = new System.Drawing.Point(443, 3);
            this.layerLongestPathSink.Name = "layerLongestPathSink";
            this.layerLongestPathSink.Size = new System.Drawing.Size(141, 19);
            this.layerLongestPathSink.TabIndex = 4;
            this.layerLongestPathSink.Text = "LayerLongestPathSink";
            this.layerLongestPathSink.UseVisualStyleBackColor = true;
            // 
            // layerLongestPathSource
            // 
            this.layerLongestPathSource.AutoSize = true;
            this.layerLongestPathSource.Location = new System.Drawing.Point(273, 3);
            this.layerLongestPathSource.Name = "layerLongestPathSource";
            this.layerLongestPathSource.Size = new System.Drawing.Size(155, 19);
            this.layerLongestPathSource.TabIndex = 3;
            this.layerLongestPathSource.Text = "LayerLongestPathSource";
            this.layerLongestPathSource.UseVisualStyleBackColor = true;
            // 
            // layerOptimalLinkLength
            // 
            this.layerOptimalLinkLength.AutoSize = true;
            this.layerOptimalLinkLength.Checked = true;
            this.layerOptimalLinkLength.Location = new System.Drawing.Point(103, 3);
            this.layerOptimalLinkLength.Name = "layerOptimalLinkLength";
            this.layerOptimalLinkLength.Size = new System.Drawing.Size(155, 19);
            this.layerOptimalLinkLength.TabIndex = 2;
            this.layerOptimalLinkLength.TabStop = true;
            this.layerOptimalLinkLength.Text = "LayerOptimalLinkLength";
            this.layerOptimalLinkLength.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(784, 30);
            this.label4.TabIndex = 11;
            this.label4.Text = "LayeredDigraphLayout Properties";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 353);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(994, 494);
            this.diagramControl1.TabIndex = 2;
            this.diagramControl1.Text = "diagramControl1";
            // 
            // goWebBrowser1
            // 
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 853);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 60);
            this.goWebBrowser1.TabIndex = 3;
            this.goWebBrowser1.ZoomFactor = 1D;
            // 
            // LayeredDigraphControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LayeredDigraphControl";
            this.Size = new System.Drawing.Size(1000, 923);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel13.PerformLayout();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
    private System.Windows.Forms.TextBox txtMaxNodes;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button generateDiagraph;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtMinNodes;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
    private System.Windows.Forms.CheckBox setPortSpots;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.CheckBox packExpand;
    private System.Windows.Forms.CheckBox packStraighten;
    private System.Windows.Forms.CheckBox packMedian;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.RadioButton aggressiveMore;
    private System.Windows.Forms.RadioButton aggressiveLess;
    private System.Windows.Forms.RadioButton aggressiveNone;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.RadioButton initNaive;
    private System.Windows.Forms.RadioButton initDepthFirstIn;
    private System.Windows.Forms.RadioButton initDepthFirstOut;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
    private System.Windows.Forms.TextBox txtColumnSpacing;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
    private System.Windows.Forms.TextBox txtLayerSpacing;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
    private System.Windows.Forms.RadioButton up;
    private System.Windows.Forms.RadioButton left;
    private System.Windows.Forms.RadioButton down;
    private System.Windows.Forms.RadioButton right;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.RadioButton cycleGreedy;
    private System.Windows.Forms.RadioButton cycleDepthFirst;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.RadioButton layerLongestPathSink;
    private System.Windows.Forms.RadioButton layerLongestPathSource;
    private System.Windows.Forms.RadioButton layerOptimalLinkLength;
    private System.Windows.Forms.Label label4;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
  }
}
