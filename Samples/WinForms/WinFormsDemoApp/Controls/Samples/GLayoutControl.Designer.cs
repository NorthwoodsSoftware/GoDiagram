/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace Demo.Samples.GLayout {
  partial class GLayout {
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
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.wrapColTb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.wrapWidthTb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cellSizeTb = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.spacingTb = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.alignPosRb = new System.Windows.Forms.RadioButton();
            this.alignLocRb = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.arrangeLTRRb = new System.Windows.Forms.RadioButton();
            this.arrangeRTLRb = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.sortingCb = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 857);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 255);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(994, 494);
            this.diagramControl1.TabIndex = 2;
            this.diagramControl1.Text = "diagramControl1";
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
            this.desc1.Location = new System.Drawing.Point(3, 755);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 53);
            this.desc1.TabIndex = 3;
            this.desc1.ZoomFactor = 1D;
            //
            // flowLayoutPanel1
            //
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.AliceBlue;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.wrapColTb);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.wrapWidthTb);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.cellSizeTb);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.Controls.Add(this.label8);
            this.flowLayoutPanel1.Controls.Add(this.spacingTb);
            this.flowLayoutPanel1.Controls.Add(this.label9);
            this.flowLayoutPanel1.Controls.Add(this.label10);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.label11);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel1.Controls.Add(this.label12);
            this.flowLayoutPanel1.Controls.Add(this.sortingCb);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(994, 246);
            this.flowLayoutPanel1.TabIndex = 4;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.label1, true);
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "GridLayout Properties";
            //
            // label2
            //
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Wrapping Column:";
            //
            // wrapColTb
            //
            this.wrapColTb.Location = new System.Drawing.Point(129, 25);
            this.wrapColTb.Name = "wrapColTb";
            this.wrapColTb.Size = new System.Drawing.Size(100, 25);
            this.wrapColTb.TabIndex = 2;
            this.wrapColTb.Text = "0";
            //
            // label3
            //
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.label3, true);
            this.label3.Location = new System.Drawing.Point(235, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "(0 means there\'s no limit)";
            //
            // label4
            //
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Wrapping Width:";
            //
            // wrapWidthTb
            //
            this.wrapWidthTb.Location = new System.Drawing.Point(119, 56);
            this.wrapWidthTb.Name = "wrapWidthTb";
            this.wrapWidthTb.Size = new System.Drawing.Size(100, 25);
            this.wrapWidthTb.TabIndex = 5;
            this.wrapWidthTb.Text = "NaN";
            //
            // label5
            //
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.label5, true);
            this.label5.Location = new System.Drawing.Point(225, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(282, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "(NaN means use the diagram\'s viewport width)";
            //
            // label6
            //
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 17);
            this.label6.TabIndex = 7;
            this.label6.Text = "Cell Size:";
            //
            // cellSizeTb
            //
            this.cellSizeTb.Location = new System.Drawing.Point(71, 87);
            this.cellSizeTb.Name = "cellSizeTb";
            this.cellSizeTb.Size = new System.Drawing.Size(100, 25);
            this.cellSizeTb.TabIndex = 8;
            this.cellSizeTb.Text = "NaN NaN";
            //
            // label7
            //
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.label7, true);
            this.label7.Location = new System.Drawing.Point(177, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(385, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "(NaN x NaN means use a cell size big enough to hold any node)";
            //
            // label8
            //
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 122);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 17);
            this.label8.TabIndex = 10;
            this.label8.Text = "Spacing:";
            //
            // spacingTb
            //
            this.spacingTb.Location = new System.Drawing.Point(69, 118);
            this.spacingTb.Name = "spacingTb";
            this.spacingTb.Size = new System.Drawing.Size(100, 25);
            this.spacingTb.TabIndex = 11;
            this.spacingTb.Text = "10 10";
            //
            // label9
            //
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.label9, true);
            this.label9.Location = new System.Drawing.Point(175, 122);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(244, 17);
            this.label9.TabIndex = 12;
            this.label9.Text = "(the minimum space between the nodes)";
            //
            // label10
            //
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 154);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 17);
            this.label10.TabIndex = 13;
            this.label10.Text = "Alignment:";
            //
            // flowLayoutPanel2
            //
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.alignPosRb);
            this.flowLayoutPanel2.Controls.Add(this.alignLocRb);
            this.flowLayoutPanel1.SetFlowBreak(this.flowLayoutPanel2, true);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(81, 149);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(159, 27);
            this.flowLayoutPanel2.TabIndex = 14;
            //
            // alignPosRb
            //
            this.alignPosRb.AutoSize = true;
            this.alignPosRb.Location = new System.Drawing.Point(3, 3);
            this.alignPosRb.Name = "alignPosRb";
            this.alignPosRb.Size = new System.Drawing.Size(72, 21);
            this.alignPosRb.TabIndex = 0;
            this.alignPosRb.Text = "Position";
            this.alignPosRb.UseVisualStyleBackColor = true;
            //
            // alignLocRb
            //
            this.alignLocRb.AutoSize = true;
            this.alignLocRb.Checked = true;
            this.alignLocRb.Location = new System.Drawing.Point(81, 3);
            this.alignLocRb.Name = "alignLocRb";
            this.alignLocRb.Size = new System.Drawing.Size(75, 21);
            this.alignLocRb.TabIndex = 1;
            this.alignLocRb.TabStop = true;
            this.alignLocRb.Text = "Location";
            this.alignLocRb.UseVisualStyleBackColor = true;
            //
            // label11
            //
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 187);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(87, 17);
            this.label11.TabIndex = 15;
            this.label11.Text = "Arrangement:";
            //
            // flowLayoutPanel3
            //
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.arrangeLTRRb);
            this.flowLayoutPanel3.Controls.Add(this.arrangeRTLRb);
            this.flowLayoutPanel1.SetFlowBreak(this.flowLayoutPanel3, true);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(99, 182);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(194, 27);
            this.flowLayoutPanel3.TabIndex = 16;
            //
            // arrangeLTRRb
            //
            this.arrangeLTRRb.AutoSize = true;
            this.arrangeLTRRb.Checked = true;
            this.arrangeLTRRb.Location = new System.Drawing.Point(3, 3);
            this.arrangeLTRRb.Name = "arrangeLTRRb";
            this.arrangeLTRRb.Size = new System.Drawing.Size(91, 21);
            this.arrangeLTRRb.TabIndex = 0;
            this.arrangeLTRRb.TabStop = true;
            this.arrangeLTRRb.Text = "LeftToRight";
            this.arrangeLTRRb.UseVisualStyleBackColor = true;
            //
            // arrangeRTLRb
            //
            this.arrangeRTLRb.AutoSize = true;
            this.arrangeRTLRb.Location = new System.Drawing.Point(100, 3);
            this.arrangeRTLRb.Name = "arrangeRTLRb";
            this.arrangeRTLRb.Size = new System.Drawing.Size(91, 21);
            this.arrangeRTLRb.TabIndex = 1;
            this.arrangeRTLRb.Text = "RightToLeft";
            this.arrangeRTLRb.UseVisualStyleBackColor = true;
            //
            // label12
            //
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 219);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 17);
            this.label12.TabIndex = 17;
            this.label12.Text = "Sorting:";
            //
            // sortingCb
            //
            this.sortingCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sortingCb.FormattingEnabled = true;
            this.sortingCb.Location = new System.Drawing.Point(65, 215);
            this.sortingCb.Name = "sortingCb";
            this.sortingCb.Size = new System.Drawing.Size(121, 25);
            this.sortingCb.TabIndex = 18;
            //
            // GridLayoutSampleControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GridLayoutSampleControl";
            this.Size = new System.Drawing.Size(1000, 857);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox wrapColTb;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox wrapWidthTb;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox cellSizeTb;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox spacingTb;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.RadioButton alignPosRb;
    private System.Windows.Forms.RadioButton alignLocRb;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    private System.Windows.Forms.RadioButton arrangeLTRRb;
    private System.Windows.Forms.RadioButton arrangeRTLRb;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.ComboBox sortingCb;
  }
}
