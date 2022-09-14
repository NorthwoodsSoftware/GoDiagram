namespace Demo.Extensions.Robot {
  partial class Robot {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.paletteControl1 = new Northwoods.Go.WinForms.PaletteControl();
      this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
      this.desc1 = new WinFormsDemoApp.GoWebBrowser();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.outputTb = new System.Windows.Forms.Label();
      this.panBtn = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.doubleClickLambdaBtn = new System.Windows.Forms.Button();
      this.clickLambdaBtn = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.deleteBtn = new System.Windows.Forms.Button();
      this.contextMenuBtn = new System.Windows.Forms.Button();
      this.dragSelectBtn = new System.Windows.Forms.Button();
      this.copyNodeBtn = new System.Windows.Forms.Button();
      this.dragFromPaletteBtn = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.desc1)).BeginInit();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.AutoScroll = true;
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.paletteControl1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.diagramControl1, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.desc1, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 400F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(922, 919);
      this.tableLayoutPanel1.TabIndex = 0;
      //
      // paletteControl1
      //
      this.paletteControl1.AllowDrop = true;
      this.paletteControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.paletteControl1.Location = new System.Drawing.Point(3, 3);
      this.paletteControl1.Name = "paletteControl1";
      this.paletteControl1.Size = new System.Drawing.Size(190, 544);
      this.paletteControl1.TabIndex = 0;
      this.paletteControl1.Text = "paletteControl1";
      //
      // diagramControl1
      //
      this.diagramControl1.AllowDrop = true;
      this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.diagramControl1.Location = new System.Drawing.Point(199, 3);
      this.diagramControl1.Name = "diagramControl1";
      this.diagramControl1.Size = new System.Drawing.Size(798, 544);
      this.diagramControl1.TabIndex = 1;
      this.diagramControl1.Text = "diagramControl1";
      //
      // desc1
      //
      this.tableLayoutPanel1.SetColumnSpan(this.desc1, 2);
      this.desc1.CreationProperties = null;
      this.desc1.DefaultBackgroundColor = System.Drawing.Color.White;
      this.desc1.Dock = System.Windows.Forms.DockStyle.Top;
      this.desc1.Location = new System.Drawing.Point(3, 553);
      this.desc1.Name = "desc1";
      this.desc1.Size = new System.Drawing.Size(994, 75);
      this.desc1.TabIndex = 2;
      this.desc1.ZoomFactor = 1D;
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.AutoSize = true;
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Controls.Add(this.outputTb, 0, 11);
      this.tableLayoutPanel2.Controls.Add(this.panBtn, 0, 10);
      this.tableLayoutPanel2.Controls.Add(this.label3, 0, 9);
      this.tableLayoutPanel2.Controls.Add(this.doubleClickLambdaBtn, 0, 8);
      this.tableLayoutPanel2.Controls.Add(this.clickLambdaBtn, 0, 7);
      this.tableLayoutPanel2.Controls.Add(this.label2, 0, 6);
      this.tableLayoutPanel2.Controls.Add(this.deleteBtn, 0, 5);
      this.tableLayoutPanel2.Controls.Add(this.contextMenuBtn, 0, 4);
      this.tableLayoutPanel2.Controls.Add(this.dragSelectBtn, 0, 3);
      this.tableLayoutPanel2.Controls.Add(this.copyNodeBtn, 0, 2);
      this.tableLayoutPanel2.Controls.Add(this.dragFromPaletteBtn, 0, 1);
      this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
      this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 603);
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
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel2.Size = new System.Drawing.Size(268, 313);
      this.tableLayoutPanel2.TabIndex = 1;
      // 
      // outputTb
      // 
      this.outputTb.AutoSize = true;
      this.outputTb.Location = new System.Drawing.Point(3, 313);
      this.outputTb.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
      this.outputTb.Name = "outputTb";
      this.outputTb.ForeColor = System.Drawing.Color.Green;
      this.outputTb.Size = new System.Drawing.Size(108, 15);
      this.outputTb.TabIndex = 11;
      this.outputTb.Text = "";
      // 
      // panBtn
      // 
      this.panBtn.AutoSize = true;
      this.panBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panBtn.Location = new System.Drawing.Point(3, 285);
      this.panBtn.Name = "panBtn";
      this.panBtn.Size = new System.Drawing.Size(105, 25);
      this.panBtn.TabIndex = 10;
      this.panBtn.Text = "Pan the Diagram";
      this.panBtn.UseVisualStyleBackColor = true;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 267);
      this.label3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(108, 15);
      this.label3.TabIndex = 9;
      this.label3.Text = "Panning operation:";
      // 
      // doubleClickLambdaBtn
      // 
      this.doubleClickLambdaBtn.AutoSize = true;
      this.doubleClickLambdaBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.doubleClickLambdaBtn.Location = new System.Drawing.Point(3, 229);
      this.doubleClickLambdaBtn.Name = "doubleClickLambdaBtn";
      this.doubleClickLambdaBtn.Size = new System.Drawing.Size(130, 25);
      this.doubleClickLambdaBtn.TabIndex = 8;
      this.doubleClickLambdaBtn.Text = "Double Click Lambda";
      this.doubleClickLambdaBtn.UseVisualStyleBackColor = true;
      // 
      // clickLambdaBtn
      // 
      this.clickLambdaBtn.AutoSize = true;
      this.clickLambdaBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.clickLambdaBtn.Location = new System.Drawing.Point(3, 198);
      this.clickLambdaBtn.Name = "clickLambdaBtn";
      this.clickLambdaBtn.Size = new System.Drawing.Size(89, 25);
      this.clickLambdaBtn.TabIndex = 7;
      this.clickLambdaBtn.Text = "Click Lambda";
      this.clickLambdaBtn.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 180);
      this.label2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(112, 15);
      this.label2.TabIndex = 6;
      this.label2.Text = "Clicking operations:";
      // 
      // deleteBtn
      // 
      this.deleteBtn.AutoSize = true;
      this.deleteBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.deleteBtn.Location = new System.Drawing.Point(3, 142);
      this.deleteBtn.Name = "deleteBtn";
      this.deleteBtn.Size = new System.Drawing.Size(50, 25);
      this.deleteBtn.TabIndex = 5;
      this.deleteBtn.Text = "Delete";
      this.deleteBtn.UseVisualStyleBackColor = true;
      // 
      // contextMenuBtn
      // 
      this.contextMenuBtn.AutoSize = true;
      this.contextMenuBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.contextMenuBtn.Location = new System.Drawing.Point(3, 111);
      this.contextMenuBtn.Name = "contextMenuBtn";
      this.contextMenuBtn.Size = new System.Drawing.Size(156, 25);
      this.contextMenuBtn.TabIndex = 4;
      this.contextMenuBtn.Text = "Context Menu Click Alpha";
      this.contextMenuBtn.UseVisualStyleBackColor = true;
      // 
      // dragSelectBtn
      // 
      this.dragSelectBtn.AutoSize = true;
      this.dragSelectBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.dragSelectBtn.Location = new System.Drawing.Point(3, 80);
      this.dragSelectBtn.Name = "dragSelectBtn";
      this.dragSelectBtn.Size = new System.Drawing.Size(113, 25);
      this.dragSelectBtn.TabIndex = 3;
      this.dragSelectBtn.Text = "Drag Select Nodes";
      this.dragSelectBtn.UseVisualStyleBackColor = true;
      // 
      // copyNodeBtn
      // 
      this.copyNodeBtn.AutoSize = true;
      this.copyNodeBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.copyNodeBtn.Location = new System.Drawing.Point(3, 49);
      this.copyNodeBtn.Name = "copyNodeBtn";
      this.copyNodeBtn.Size = new System.Drawing.Size(77, 25);
      this.copyNodeBtn.TabIndex = 2;
      this.copyNodeBtn.Text = "Copy Node";
      this.copyNodeBtn.UseVisualStyleBackColor = true;
      // 
      // dragFromPaletteBtn
      // 
      this.dragFromPaletteBtn.AutoSize = true;
      this.dragFromPaletteBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.dragFromPaletteBtn.Location = new System.Drawing.Point(3, 18);
      this.dragFromPaletteBtn.Name = "dragFromPaletteBtn";
      this.dragFromPaletteBtn.Size = new System.Drawing.Size(112, 25);
      this.dragFromPaletteBtn.TabIndex = 1;
      this.dragFromPaletteBtn.Text = "Drag From Palette";
      this.dragFromPaletteBtn.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(262, 15);
      this.label1.TabIndex = 0;
      this.label1.Text = "Click these buttons in order from top to bottom:";
      // 
      // Form1
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(922, 1034);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.desc1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.PaletteControl paletteControl1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private WinFormsDemoApp.GoWebBrowser desc1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button dragFromPaletteBtn;
    private System.Windows.Forms.Button copyNodeBtn;
    private System.Windows.Forms.Button dragSelectBtn;
    private System.Windows.Forms.Button contextMenuBtn;
    private System.Windows.Forms.Button deleteBtn;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button clickLambdaBtn;
    private System.Windows.Forms.Button doubleClickLambdaBtn;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button panBtn;
    private System.Windows.Forms.Label outputTb;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
  }
}

