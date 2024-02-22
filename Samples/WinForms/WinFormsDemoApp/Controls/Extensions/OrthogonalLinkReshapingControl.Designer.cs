/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Extensions.OrthogonalLinkReshaping {
  partial class OrthogonalLinkReshaping {
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
            this.label1 = new System.Windows.Forms.Label();
            this.orthogonalRb = new System.Windows.Forms.RadioButton();
            this.avoidsNodesRb = new System.Windows.Forms.RadioButton();
            this.desc1 = new WinFormsDemoApp.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(994, 594);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.orthogonalRb, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.avoidsNodesRb, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 603);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(402, 44);
            this.tableLayoutPanel2.TabIndex = 1;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "Routing:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // orthogonalRb
            //
            this.orthogonalRb.AutoSize = true;
            this.orthogonalRb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orthogonalRb.Location = new System.Drawing.Point(103, 3);
            this.orthogonalRb.Name = "orthogonalRb";
            this.orthogonalRb.Size = new System.Drawing.Size(144, 38);
            this.orthogonalRb.TabIndex = 1;
            this.orthogonalRb.Text = "Orthogonal";
            this.orthogonalRb.UseVisualStyleBackColor = true;
            //
            // avoidsNodesRb
            //
            this.avoidsNodesRb.AutoSize = true;
            this.avoidsNodesRb.Checked = true;
            this.avoidsNodesRb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.avoidsNodesRb.Location = new System.Drawing.Point(253, 3);
            this.avoidsNodesRb.Name = "avoidsNodesRb";
            this.avoidsNodesRb.Size = new System.Drawing.Size(146, 38);
            this.avoidsNodesRb.TabIndex = 2;
            this.avoidsNodesRb.TabStop = true;
            this.avoidsNodesRb.Text = "AvoidsNodes";
            this.avoidsNodesRb.UseVisualStyleBackColor = true;
            //
            // desc1
            //
            this.desc1.CreationProperties = null;
            this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.desc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.desc1.Location = new System.Drawing.Point(3, 653);
            this.desc1.Name = "desc1";
            this.desc1.Size = new System.Drawing.Size(994, 544);
            this.desc1.TabIndex = 2;
            this.desc1.ZoomFactor = 1D;
            //
            // OrthogonalLinkReshapingControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OrthogonalLinkReshapingControl";
            this.Size = new System.Drawing.Size(1000, 1200);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.RadioButton orthogonalRb;
    private System.Windows.Forms.RadioButton avoidsNodesRb;
    private WinFormsDemoApp.GoWebBrowser desc1;
  }
}
