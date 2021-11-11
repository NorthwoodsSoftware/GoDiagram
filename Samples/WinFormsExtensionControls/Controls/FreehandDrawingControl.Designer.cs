
namespace WinFormsExtensionControls.FreehandDrawing {
  partial class FreehandDrawingControl {
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.diagramControl1 = new Northwoods.Go.WinForms.DiagramControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDraw = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.checkBxResizing = new System.Windows.Forms.CheckBox();
            this.checkBxReshaping = new System.Windows.Forms.CheckBox();
            this.checkBxRotating = new System.Windows.Forms.CheckBox();
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.saveLoadModel1 = new WinFormsSharedControls.SaveLoadModel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1342, 1326);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(1336, 687);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 282F));
            this.tableLayoutPanel2.Controls.Add(this.btnDraw, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSelect, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBxResizing, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBxReshaping, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBxRotating, 4, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 696);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(807, 42);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnDraw
            // 
            this.btnDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDraw.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDraw.Location = new System.Drawing.Point(128, 3);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(119, 36);
            this.btnDraw.TabIndex = 1;
            this.btnDraw.Text = "Draw Mode";
            this.btnDraw.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelect.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSelect.Location = new System.Drawing.Point(3, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(119, 36);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // checkBxResizing
            // 
            this.checkBxResizing.AutoSize = true;
            this.checkBxResizing.Checked = true;
            this.checkBxResizing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBxResizing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBxResizing.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBxResizing.Location = new System.Drawing.Point(253, 3);
            this.checkBxResizing.Name = "checkBxResizing";
            this.checkBxResizing.Size = new System.Drawing.Size(169, 36);
            this.checkBxResizing.TabIndex = 4;
            this.checkBxResizing.Text = "Allow Resizing";
            this.checkBxResizing.UseVisualStyleBackColor = true;
            // 
            // checkBxReshaping
            // 
            this.checkBxReshaping.AutoSize = true;
            this.checkBxReshaping.Checked = true;
            this.checkBxReshaping.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBxReshaping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBxReshaping.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBxReshaping.Location = new System.Drawing.Point(428, 3);
            this.checkBxReshaping.Name = "checkBxReshaping";
            this.checkBxReshaping.Size = new System.Drawing.Size(94, 36);
            this.checkBxReshaping.TabIndex = 5;
            this.checkBxReshaping.Text = "Allow Reshaping";
            this.checkBxReshaping.UseVisualStyleBackColor = true;
            // 
            // checkBxRotating
            // 
            this.checkBxRotating.AutoSize = true;
            this.checkBxRotating.Checked = true;
            this.checkBxRotating.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBxRotating.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBxRotating.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBxRotating.Location = new System.Drawing.Point(528, 3);
            this.checkBxRotating.Name = "checkBxRotating";
            this.checkBxRotating.Size = new System.Drawing.Size(276, 36);
            this.checkBxRotating.TabIndex = 6;
            this.checkBxRotating.Text = "Allow Rotating";
            this.checkBxRotating.UseVisualStyleBackColor = true;
            // 
            // goWebBrowser1
            // 
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 744);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(1336, 159);
            this.goWebBrowser1.TabIndex = 2;
            this.goWebBrowser1.ZoomFactor = 1D;
            // 
            // saveLoadModel1
            // 
            this.saveLoadModel1.AutoSize = true;
            this.saveLoadModel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveLoadModel1.Location = new System.Drawing.Point(3, 909);
            this.saveLoadModel1.Name = "saveLoadModel1";
            this.saveLoadModel1.Size = new System.Drawing.Size(1336, 414);
            this.saveLoadModel1.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // FreehandDrawingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FreehandDrawingControl";
            this.Size = new System.Drawing.Size(1342, 1326);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button btnDraw;
    private System.Windows.Forms.Button btnSelect;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.CheckBox checkBxResizing;
    private System.Windows.Forms.CheckBox checkBxReshaping;
    private System.Windows.Forms.CheckBox checkBxRotating;
    private WinFormsSharedControls.SaveLoadModel saveLoadModel1;
  }
}
