
namespace WinFormsSampleControls.FDLayout {
  partial class FDLayoutControl {
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
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.minNodes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.maxNodes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.minChil = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.maxChil = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.maxIter = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.epsilon = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.infinity = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.arrangement = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.charge = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.mass = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.stiffness = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.length = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 705);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.BackColor = System.Drawing.Color.White;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 190);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(994, 468);
            this.diagramControl1.TabIndex = 2;
            // 
            // goWebBrowser1
            // 
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 665);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(994, 64);
            this.goWebBrowser1.TabIndex = 3;
            this.goWebBrowser1.ZoomFactor = 1D;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.AliceBlue;
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel4);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel5);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(921, 180);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.minNodes);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.maxNodes);
            this.flowLayoutPanel2.Controls.Add(this.label4);
            this.flowLayoutPanel2.Controls.Add(this.minChil);
            this.flowLayoutPanel2.Controls.Add(this.label5);
            this.flowLayoutPanel2.Controls.Add(this.maxChil);
            this.flowLayoutPanel2.Controls.Add(this.button1);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(200, 174);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.flowLayoutPanel2.SetFlowBreak(this.label1, true);
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "New Tree";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Min Nodes:";
            // 
            // minNodes
            // 
            this.flowLayoutPanel2.SetFlowBreak(this.minNodes, true);
            this.minNodes.Location = new System.Drawing.Point(85, 20);
            this.minNodes.Name = "minNodes";
            this.minNodes.Size = new System.Drawing.Size(100, 25);
            this.minNodes.TabIndex = 2;
            this.minNodes.Text = "20";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Max Nodes:";
            // 
            // maxNodes
            // 
            this.flowLayoutPanel2.SetFlowBreak(this.maxNodes, true);
            this.maxNodes.Location = new System.Drawing.Point(88, 51);
            this.maxNodes.Name = "maxNodes";
            this.maxNodes.Size = new System.Drawing.Size(100, 25);
            this.maxNodes.TabIndex = 4;
            this.maxNodes.Text = "100";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Min Children:";
            // 
            // minChil
            // 
            this.flowLayoutPanel2.SetFlowBreak(this.minChil, true);
            this.minChil.Location = new System.Drawing.Point(94, 82);
            this.minChil.Name = "minChil";
            this.minChil.Size = new System.Drawing.Size(100, 25);
            this.minChil.TabIndex = 6;
            this.minChil.Text = "1";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Max Children:";
            // 
            // maxChil
            // 
            this.flowLayoutPanel2.SetFlowBreak(this.maxChil, true);
            this.maxChil.Location = new System.Drawing.Point(97, 113);
            this.maxChil.Name = "maxChil";
            this.maxChil.Size = new System.Drawing.Size(100, 25);
            this.maxChil.TabIndex = 8;
            this.maxChil.Text = "10";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(3, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 27);
            this.button1.TabIndex = 9;
            this.button1.Text = "Generate Tree";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.label6);
            this.flowLayoutPanel3.Controls.Add(this.label7);
            this.flowLayoutPanel3.Controls.Add(this.maxIter);
            this.flowLayoutPanel3.Controls.Add(this.label8);
            this.flowLayoutPanel3.Controls.Add(this.epsilon);
            this.flowLayoutPanel3.Controls.Add(this.label9);
            this.flowLayoutPanel3.Controls.Add(this.infinity);
            this.flowLayoutPanel3.Controls.Add(this.label10);
            this.flowLayoutPanel3.Controls.Add(this.arrangement);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(209, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(245, 141);
            this.flowLayoutPanel3.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "Max Iterations:";
            // 
            // maxIter
            // 
            this.flowLayoutPanel3.SetFlowBreak(this.maxIter, true);
            this.maxIter.Location = new System.Drawing.Point(103, 20);
            this.maxIter.Name = "maxIter";
            this.maxIter.Size = new System.Drawing.Size(100, 25);
            this.maxIter.TabIndex = 2;
            this.maxIter.Text = "100";
            this.maxIter.Leave += new System.EventHandler(this._PropertyChanged);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "Epsilon:";
            // 
            // epsilon
            // 
            this.flowLayoutPanel3.SetFlowBreak(this.epsilon, true);
            this.epsilon.Location = new System.Drawing.Point(62, 51);
            this.epsilon.Name = "epsilon";
            this.epsilon.Size = new System.Drawing.Size(100, 25);
            this.epsilon.TabIndex = 4;
            this.epsilon.Text = "1";
            this.epsilon.Leave += new System.EventHandler(this._PropertyChanged);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 86);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 17);
            this.label9.TabIndex = 5;
            this.label9.Text = "Infinity:";
            // 
            // infinity
            // 
            this.flowLayoutPanel3.SetFlowBreak(this.infinity, true);
            this.infinity.Location = new System.Drawing.Point(57, 82);
            this.infinity.Name = "infinity";
            this.infinity.Size = new System.Drawing.Size(100, 25);
            this.infinity.TabIndex = 6;
            this.infinity.Text = "1000";
            this.infinity.Leave += new System.EventHandler(this._PropertyChanged);
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 117);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(133, 17);
            this.label10.TabIndex = 7;
            this.label10.Text = "ArrangementSpacing:";
            // 
            // arrangement
            // 
            this.arrangement.Location = new System.Drawing.Point(142, 113);
            this.arrangement.Name = "arrangement";
            this.arrangement.Size = new System.Drawing.Size(100, 25);
            this.arrangement.TabIndex = 8;
            this.arrangement.Text = "100 100";
            this.arrangement.Leave += new System.EventHandler(this._PropertyChanged);
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Controls.Add(this.label11);
            this.flowLayoutPanel4.Controls.Add(this.label12);
            this.flowLayoutPanel4.Controls.Add(this.charge);
            this.flowLayoutPanel4.Controls.Add(this.label13);
            this.flowLayoutPanel4.Controls.Add(this.mass);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(460, 3);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(234, 79);
            this.flowLayoutPanel4.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.flowLayoutPanel4.SetFlowBreak(this.label11, true);
            this.label11.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label11.Location = new System.Drawing.Point(3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 17);
            this.label11.TabIndex = 0;
            this.label11.Text = "Vertex Properties";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(108, 17);
            this.label12.TabIndex = 1;
            this.label12.Text = "Electrical Charge:";
            // 
            // charge
            // 
            this.flowLayoutPanel4.SetFlowBreak(this.charge, true);
            this.charge.Location = new System.Drawing.Point(117, 20);
            this.charge.Name = "charge";
            this.charge.Size = new System.Drawing.Size(100, 25);
            this.charge.TabIndex = 2;
            this.charge.Text = "150";
            this.charge.Leave += new System.EventHandler(this._PropertyChanged);
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 55);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(119, 17);
            this.label13.TabIndex = 3;
            this.label13.Text = "Gravitational Mass:";
            // 
            // mass
            // 
            this.mass.Location = new System.Drawing.Point(128, 51);
            this.mass.Name = "mass";
            this.mass.Size = new System.Drawing.Size(100, 25);
            this.mass.TabIndex = 4;
            this.mass.Text = "0";
            this.mass.Leave += new System.EventHandler(this._PropertyChanged);
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.AutoSize = true;
            this.flowLayoutPanel5.Controls.Add(this.label14);
            this.flowLayoutPanel5.Controls.Add(this.label15);
            this.flowLayoutPanel5.Controls.Add(this.stiffness);
            this.flowLayoutPanel5.Controls.Add(this.label16);
            this.flowLayoutPanel5.Controls.Add(this.length);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(700, 3);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(218, 79);
            this.flowLayoutPanel5.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.flowLayoutPanel5.SetFlowBreak(this.label14, true);
            this.label14.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label14.Location = new System.Drawing.Point(3, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(105, 17);
            this.label14.TabIndex = 0;
            this.label14.Text = "Edge Properties";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 24);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(101, 17);
            this.label15.TabIndex = 1;
            this.label15.Text = "Spring Stiffness:";
            // 
            // stiffness
            // 
            this.flowLayoutPanel5.SetFlowBreak(this.stiffness, true);
            this.stiffness.Location = new System.Drawing.Point(110, 20);
            this.stiffness.Name = "stiffness";
            this.stiffness.Size = new System.Drawing.Size(100, 25);
            this.stiffness.TabIndex = 2;
            this.stiffness.Text = "0.05";
            this.stiffness.Leave += new System.EventHandler(this._PropertyChanged);
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 55);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(92, 17);
            this.label16.TabIndex = 3;
            this.label16.Text = "Spring Length:";
            // 
            // length
            // 
            this.length.Location = new System.Drawing.Point(101, 51);
            this.length.Name = "length";
            this.length.Size = new System.Drawing.Size(100, 25);
            this.length.TabIndex = 4;
            this.length.Text = "50";
            this.length.Leave += new System.EventHandler(this._PropertyChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.flowLayoutPanel3.SetFlowBreak(this.label6, true);
            this.label6.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "FDLayout Properties";
            // 
            // ForceDirectedLayoutSampleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ForceDirectedLayoutSampleControl";
            this.Size = new System.Drawing.Size(1000, 705);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox minNodes;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox maxNodes;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox minChil;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox maxChil;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox maxIter;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox epsilon;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.TextBox infinity;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox arrangement;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.TextBox charge;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.TextBox mass;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.TextBox stiffness;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.TextBox length;
    private System.Windows.Forms.Label label6;
  }
}
