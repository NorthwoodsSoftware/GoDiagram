/* Copyright 1998-2024 by Northwoods Software Corporation. */
namespace Demo.Samples.GameOfLife {
  partial class GameOfLifeControl {
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
      this.lblGameControls = new System.Windows.Forms.Label();
      this.btnStart = new System.Windows.Forms.Button();
      this.btnStep = new System.Windows.Forms.Button();
      this.btnClear = new System.Windows.Forms.Button();
      this.lblSamplePatterns = new System.Windows.Forms.Label();
      this.goWebBrowser1 = new WinFormsDemoApp.GoWebBrowser();
      this.patterns = new System.Windows.Forms.ComboBox();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).BeginInit();
      this.SuspendLayout();
      //
      // tableLayoutPanel1
      //
      this.tableLayoutPanel1.AutoScroll = true;
      this.tableLayoutPanel1.ColumnCount = 6;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.lblGameControls, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.btnStart, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.btnStep, 2, 1);
      this.tableLayoutPanel1.Controls.Add(this.btnClear, 3, 1);
      this.tableLayoutPanel1.Controls.Add(this.lblSamplePatterns, 4, 1);
      this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.patterns, 5, 1);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(2144, 3000);
      this.tableLayoutPanel1.TabIndex = 0;
      //
      // diagramControl1
      //
      this.diagramControl1.AllowDrop = true;
      this.tableLayoutPanel1.SetColumnSpan(this.diagramControl1, 6);
      this.diagramControl1.Location = new System.Drawing.Point(4, 5);
      this.diagramControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.diagramControl1.Name = "diagramControl1";
      this.diagramControl1.Size = new System.Drawing.Size(500, 500);
      this.diagramControl1.TabIndex = 0;
      this.diagramControl1.Text = "diagramControl1";
      //
      // lblGameControls
      //
      this.lblGameControls.AutoSize = true;
      this.lblGameControls.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.lblGameControls.Location = new System.Drawing.Point(4, 515);
      this.lblGameControls.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.lblGameControls.Name = "lblGameControls";
      this.lblGameControls.Size = new System.Drawing.Size(177, 32);
      this.lblGameControls.TabIndex = 1;
      this.lblGameControls.Text = "Game Controls:";
      this.lblGameControls.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      //
      // btnStart
      //
      this.btnStart.AutoSize = true;
      this.btnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.btnStart.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.btnStart.Location = new System.Drawing.Point(189, 515);
      this.btnStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new System.Drawing.Size(72, 42);
      this.btnStart.TabIndex = 2;
      this.btnStart.Text = "Start";
      this.btnStart.UseVisualStyleBackColor = true;
      //
      // btnStep
      //
      this.btnStep.AutoSize = true;
      this.btnStep.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.btnStep.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.btnStep.Location = new System.Drawing.Point(269, 515);
      this.btnStep.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnStep.Name = "btnStep";
      this.btnStep.Size = new System.Drawing.Size(71, 42);
      this.btnStep.TabIndex = 3;
      this.btnStep.Text = "Step";
      this.btnStep.UseVisualStyleBackColor = true;
      //
      // btnClear
      //
      this.btnClear.AutoSize = true;
      this.btnClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.btnClear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.btnClear.Location = new System.Drawing.Point(348, 515);
      this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new System.Drawing.Size(78, 42);
      this.btnClear.TabIndex = 4;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      //
      // lblSamplePatterns
      //
      this.lblSamplePatterns.AutoSize = true;
      this.lblSamplePatterns.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.lblSamplePatterns.Location = new System.Drawing.Point(434, 515);
      this.lblSamplePatterns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.lblSamplePatterns.Name = "lblSamplePatterns";
      this.lblSamplePatterns.Size = new System.Drawing.Size(190, 32);
      this.lblSamplePatterns.TabIndex = 5;
      this.lblSamplePatterns.Text = "Sample Patterns:";
      this.lblSamplePatterns.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      //
      // goWebBrowser1
      //
      this.tableLayoutPanel1.SetColumnSpan(this.goWebBrowser1, 6);
      this.goWebBrowser1.CreationProperties = null;
      this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
      this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
      this.goWebBrowser1.Location = new System.Drawing.Point(4, 567);
      this.goWebBrowser1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.goWebBrowser1.Name = "goWebBrowser1";
      this.goWebBrowser1.Size = new System.Drawing.Size(2136, 907);
      this.goWebBrowser1.TabIndex = 7;
      this.goWebBrowser1.ZoomFactor = 1D;
      //
      // patterns
      //
      this.patterns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.patterns.FormattingEnabled = true;
      this.patterns.Items.AddRange(new object[] {
            "Symmetry",
            "Pulsar",
            "Spaceships",
            "Big gliders"});
      this.patterns.Location = new System.Drawing.Point(632, 515);
      this.patterns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.patterns.Name = "patterns";
      this.patterns.Size = new System.Drawing.Size(182, 33);
      this.patterns.TabIndex = 8;
      //
      // GameOfLifeControl
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tableLayoutPanel1);
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "GameOfLifeControl";
      this.Size = new System.Drawing.Size(2144, 3000);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.Label lblGameControls;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnStep;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.Label lblSamplePatterns;
    private WinFormsDemoApp.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.ComboBox patterns;
  }
}
