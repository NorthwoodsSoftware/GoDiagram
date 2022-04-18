/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace WinFormsSampleControls.SystemDynamics {
  partial class SystemDynamicsControl {
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
            this.influenceBtn = new System.Windows.Forms.Button();
            this.stockBtn = new System.Windows.Forms.Button();
            this.flowBtn = new System.Windows.Forms.Button();
            this.variableBtn = new System.Windows.Forms.Button();
            this.cloudBtn = new System.Windows.Forms.Button();
            this.pointerBtn = new System.Windows.Forms.Button();
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.saveLoadModel1 = new WinFormsSharedControls.SaveLoadModel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.saveLoadModel1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(983, 1183);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // diagramControl1
            //
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(977, 364);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 199F));
            this.tableLayoutPanel2.Controls.Add(this.influenceBtn, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.stockBtn, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowBtn, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.variableBtn, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.cloudBtn, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.pointerBtn, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 373);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(595, 29);
            this.tableLayoutPanel2.TabIndex = 1;
            //
            // influenceBtn
            //
            this.influenceBtn.Location = new System.Drawing.Point(493, 3);
            this.influenceBtn.Name = "influenceBtn";
            this.influenceBtn.Size = new System.Drawing.Size(97, 23);
            this.influenceBtn.TabIndex = 6;
            this.influenceBtn.Text = "Influence";
            this.influenceBtn.UseVisualStyleBackColor = true;
            //
            // stockBtn
            //
            this.stockBtn.Location = new System.Drawing.Point(101, 3);
            this.stockBtn.Name = "stockBtn";
            this.stockBtn.Size = new System.Drawing.Size(92, 23);
            this.stockBtn.TabIndex = 5;
            this.stockBtn.Text = "Stock";
            this.stockBtn.UseVisualStyleBackColor = true;
            //
            // flowBtn
            //
            this.flowBtn.Location = new System.Drawing.Point(395, 3);
            this.flowBtn.Name = "flowBtn";
            this.flowBtn.Size = new System.Drawing.Size(92, 23);
            this.flowBtn.TabIndex = 4;
            this.flowBtn.Text = "Flow";
            this.flowBtn.UseVisualStyleBackColor = true;
            //
            // variableBtn
            //
            this.variableBtn.Location = new System.Drawing.Point(297, 3);
            this.variableBtn.Name = "variableBtn";
            this.variableBtn.Size = new System.Drawing.Size(92, 23);
            this.variableBtn.TabIndex = 3;
            this.variableBtn.Text = "Variable";
            this.variableBtn.UseVisualStyleBackColor = true;
            //
            // cloudBtn
            //
            this.cloudBtn.Location = new System.Drawing.Point(199, 3);
            this.cloudBtn.Name = "cloudBtn";
            this.cloudBtn.Size = new System.Drawing.Size(92, 23);
            this.cloudBtn.TabIndex = 2;
            this.cloudBtn.Text = "Cloud";
            this.cloudBtn.UseVisualStyleBackColor = true;
            //
            // pointerBtn
            //
            this.pointerBtn.Location = new System.Drawing.Point(3, 3);
            this.pointerBtn.Name = "pointerBtn";
            this.pointerBtn.Size = new System.Drawing.Size(92, 23);
            this.pointerBtn.TabIndex = 0;
            this.pointerBtn.Text = "Pointer";
            this.pointerBtn.UseVisualStyleBackColor = true;
            //
            // goWebBrowser1
            //
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 408);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(977, 394);
            this.goWebBrowser1.TabIndex = 2;
            this.goWebBrowser1.ZoomFactor = 1D;
            //
            // saveLoadModel1
            //
            this.saveLoadModel1.AutoSize = true;
            this.saveLoadModel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.saveLoadModel1.Location = new System.Drawing.Point(3, 808);
            this.saveLoadModel1.Name = "saveLoadModel1";
            this.saveLoadModel1.Size = new System.Drawing.Size(977, 343);
            this.saveLoadModel1.TabIndex = 3;
            //
            // SystemDynamicsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SystemDynamicsControl";
            this.Size = new System.Drawing.Size(983, 1183);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button pointerBtn;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private WinFormsSharedControls.SaveLoadModel saveLoadModel1;
    private System.Windows.Forms.Button influenceBtn;
    private System.Windows.Forms.Button stockBtn;
    private System.Windows.Forms.Button flowBtn;
    private System.Windows.Forms.Button variableBtn;
    private System.Windows.Forms.Button cloudBtn;
  }
}
