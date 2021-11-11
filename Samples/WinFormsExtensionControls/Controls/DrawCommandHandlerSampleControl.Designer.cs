
namespace WinFormsExtensionControls.DrawCommandHandlerSample {
  partial class DrawCommandHandlerSampleControl {
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
            this.btnColumn = new System.Windows.Forms.Button();
            this.btnRow = new System.Windows.Forms.Button();
            this.btnCenterY = new System.Windows.Forms.Button();
            this.btnCenterX = new System.Windows.Forms.Button();
            this.btnBottoms = new System.Windows.Forms.Button();
            this.btnTops = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.lblAlign = new System.Windows.Forms.Label();
            this.btnLeft = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btn180 = new System.Windows.Forms.Button();
            this.btnNeg90 = new System.Windows.Forms.Button();
            this.btn90 = new System.Windows.Forms.Button();
            this.btnNeg45 = new System.Windows.Forms.Button();
            this.lblRotate = new System.Windows.Forms.Label();
            this.btn45 = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblZOrder = new System.Windows.Forms.Label();
            this.btnFront = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.radBtnTree = new System.Windows.Forms.RadioButton();
            this.radBtnScroll = new System.Windows.Forms.RadioButton();
            this.radBtnSelect = new System.Windows.Forms.RadioButton();
            this.lblArrow = new System.Windows.Forms.Label();
            this.radBtnMove = new System.Windows.Forms.RadioButton();
            this.goWebBrowser1 = new WinFormsSharedControls.GoWebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.goWebBrowser1, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1286, 799);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // diagramControl1
            // 
            this.diagramControl1.AllowDrop = true;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.Location = new System.Drawing.Point(3, 3);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(1280, 394);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 9;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.tableLayoutPanel2.Controls.Add(this.btnColumn, 8, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnRow, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCenterY, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCenterX, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnBottoms, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnTops, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnRight, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblAlign, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnLeft, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 403);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1121, 44);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnColumn
            // 
            this.btnColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnColumn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnColumn.Location = new System.Drawing.Point(1003, 3);
            this.btnColumn.Name = "btnColumn";
            this.btnColumn.Size = new System.Drawing.Size(115, 38);
            this.btnColumn.TabIndex = 8;
            this.btnColumn.Text = "Column";
            this.btnColumn.UseVisualStyleBackColor = true;
            // 
            // btnRow
            // 
            this.btnRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRow.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRow.Location = new System.Drawing.Point(883, 3);
            this.btnRow.Name = "btnRow";
            this.btnRow.Size = new System.Drawing.Size(114, 38);
            this.btnRow.TabIndex = 7;
            this.btnRow.Text = "Row";
            this.btnRow.UseVisualStyleBackColor = true;
            // 
            // btnCenterY
            // 
            this.btnCenterY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenterY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCenterY.Location = new System.Drawing.Point(763, 3);
            this.btnCenterY.Name = "btnCenterY";
            this.btnCenterY.Size = new System.Drawing.Size(114, 38);
            this.btnCenterY.TabIndex = 6;
            this.btnCenterY.Text = "Center Y";
            this.btnCenterY.UseVisualStyleBackColor = true;
            // 
            // btnCenterX
            // 
            this.btnCenterX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenterX.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCenterX.Location = new System.Drawing.Point(643, 3);
            this.btnCenterX.Name = "btnCenterX";
            this.btnCenterX.Size = new System.Drawing.Size(114, 38);
            this.btnCenterX.TabIndex = 5;
            this.btnCenterX.Text = "Center X";
            this.btnCenterX.UseVisualStyleBackColor = true;
            // 
            // btnBottoms
            // 
            this.btnBottoms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBottoms.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnBottoms.Location = new System.Drawing.Point(523, 3);
            this.btnBottoms.Name = "btnBottoms";
            this.btnBottoms.Size = new System.Drawing.Size(114, 38);
            this.btnBottoms.TabIndex = 4;
            this.btnBottoms.Text = "Bottoms";
            this.btnBottoms.UseVisualStyleBackColor = true;
            // 
            // btnTops
            // 
            this.btnTops.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTops.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTops.Location = new System.Drawing.Point(403, 3);
            this.btnTops.Name = "btnTops";
            this.btnTops.Size = new System.Drawing.Size(114, 38);
            this.btnTops.TabIndex = 3;
            this.btnTops.Text = "Tops";
            this.btnTops.UseVisualStyleBackColor = true;
            // 
            // btnRight
            // 
            this.btnRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRight.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRight.Location = new System.Drawing.Point(253, 3);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(144, 38);
            this.btnRight.TabIndex = 2;
            this.btnRight.Text = "Right Sides";
            this.btnRight.UseVisualStyleBackColor = true;
            // 
            // lblAlign
            // 
            this.lblAlign.AutoSize = true;
            this.lblAlign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlign.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblAlign.Location = new System.Drawing.Point(3, 0);
            this.lblAlign.Name = "lblAlign";
            this.lblAlign.Size = new System.Drawing.Size(94, 44);
            this.lblAlign.TabIndex = 0;
            this.lblAlign.Text = "Align:";
            this.lblAlign.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLeft
            // 
            this.btnLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLeft.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLeft.Location = new System.Drawing.Point(103, 3);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(144, 38);
            this.btnLeft.TabIndex = 1;
            this.btnLeft.Text = "Left Sides";
            this.btnLeft.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 6;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel3.Controls.Add(this.btn180, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnNeg90, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn90, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnNeg45, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblRotate, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn45, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 453);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(477, 44);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // btn180
            // 
            this.btn180.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn180.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn180.Location = new System.Drawing.Point(403, 3);
            this.btn180.Name = "btn180";
            this.btn180.Size = new System.Drawing.Size(71, 38);
            this.btn180.TabIndex = 5;
            this.btn180.Text = "180°";
            this.btn180.UseVisualStyleBackColor = true;
            // 
            // btnNeg90
            // 
            this.btnNeg90.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNeg90.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNeg90.Location = new System.Drawing.Point(328, 3);
            this.btnNeg90.Name = "btnNeg90";
            this.btnNeg90.Size = new System.Drawing.Size(69, 38);
            this.btnNeg90.TabIndex = 4;
            this.btnNeg90.Text = "-90°";
            this.btnNeg90.UseVisualStyleBackColor = true;
            // 
            // btn90
            // 
            this.btn90.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn90.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn90.Location = new System.Drawing.Point(253, 3);
            this.btn90.Name = "btn90";
            this.btn90.Size = new System.Drawing.Size(69, 38);
            this.btn90.TabIndex = 3;
            this.btn90.Text = "90°";
            this.btn90.UseVisualStyleBackColor = true;
            // 
            // btnNeg45
            // 
            this.btnNeg45.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNeg45.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNeg45.Location = new System.Drawing.Point(178, 3);
            this.btnNeg45.Name = "btnNeg45";
            this.btnNeg45.Size = new System.Drawing.Size(69, 38);
            this.btnNeg45.TabIndex = 2;
            this.btnNeg45.Text = "-45°";
            this.btnNeg45.UseVisualStyleBackColor = true;
            // 
            // lblRotate
            // 
            this.lblRotate.AutoSize = true;
            this.lblRotate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRotate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRotate.Location = new System.Drawing.Point(3, 0);
            this.lblRotate.Name = "lblRotate";
            this.lblRotate.Size = new System.Drawing.Size(94, 44);
            this.lblRotate.TabIndex = 0;
            this.lblRotate.Text = "Rotate:";
            this.lblRotate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn45
            // 
            this.btn45.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn45.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn45.Location = new System.Drawing.Point(103, 3);
            this.btn45.Name = "btn45";
            this.btn45.Size = new System.Drawing.Size(69, 38);
            this.btn45.TabIndex = 1;
            this.btn45.Text = "45°";
            this.btn45.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableLayoutPanel4.Controls.Add(this.btnBack, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblZOrder, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnFront, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 503);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(406, 44);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // btnBack
            // 
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnBack.Location = new System.Drawing.Point(253, 3);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(150, 38);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Push To Back";
            this.btnBack.UseVisualStyleBackColor = true;
            // 
            // lblZOrder
            // 
            this.lblZOrder.AutoSize = true;
            this.lblZOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblZOrder.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblZOrder.Location = new System.Drawing.Point(3, 0);
            this.lblZOrder.Name = "lblZOrder";
            this.lblZOrder.Size = new System.Drawing.Size(94, 44);
            this.lblZOrder.TabIndex = 0;
            this.lblZOrder.Text = "Z-Order:";
            this.lblZOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFront
            // 
            this.btnFront.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFront.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnFront.Location = new System.Drawing.Point(103, 3);
            this.btnFront.Name = "btnFront";
            this.btnFront.Size = new System.Drawing.Size(144, 38);
            this.btnFront.TabIndex = 1;
            this.btnFront.Text = "Pull To Front";
            this.btnFront.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 5;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tableLayoutPanel5.Controls.Add(this.radBtnTree, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.radBtnScroll, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.radBtnSelect, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblArrow, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.radBtnMove, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 553);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(561, 44);
            this.tableLayoutPanel5.TabIndex = 4;
            // 
            // radBtnTree
            // 
            this.radBtnTree.AutoSize = true;
            this.radBtnTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radBtnTree.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnTree.Location = new System.Drawing.Point(453, 3);
            this.radBtnTree.Name = "radBtnTree";
            this.radBtnTree.Size = new System.Drawing.Size(105, 38);
            this.radBtnTree.TabIndex = 4;
            this.radBtnTree.Text = "Tree";
            this.radBtnTree.UseVisualStyleBackColor = true;
            // 
            // radBtnScroll
            // 
            this.radBtnScroll.AutoSize = true;
            this.radBtnScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radBtnScroll.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnScroll.Location = new System.Drawing.Point(353, 3);
            this.radBtnScroll.Name = "radBtnScroll";
            this.radBtnScroll.Size = new System.Drawing.Size(94, 38);
            this.radBtnScroll.TabIndex = 3;
            this.radBtnScroll.Text = "Scroll";
            this.radBtnScroll.UseVisualStyleBackColor = true;
            // 
            // radBtnSelect
            // 
            this.radBtnSelect.AutoSize = true;
            this.radBtnSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radBtnSelect.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnSelect.Location = new System.Drawing.Point(253, 3);
            this.radBtnSelect.Name = "radBtnSelect";
            this.radBtnSelect.Size = new System.Drawing.Size(94, 38);
            this.radBtnSelect.TabIndex = 2;
            this.radBtnSelect.Text = "Select";
            this.radBtnSelect.UseVisualStyleBackColor = true;
            // 
            // lblArrow
            // 
            this.lblArrow.AutoSize = true;
            this.lblArrow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblArrow.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblArrow.Location = new System.Drawing.Point(3, 0);
            this.lblArrow.Name = "lblArrow";
            this.lblArrow.Size = new System.Drawing.Size(144, 44);
            this.lblArrow.TabIndex = 0;
            this.lblArrow.Text = "Arrow Mode:";
            this.lblArrow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radBtnMove
            // 
            this.radBtnMove.AutoSize = true;
            this.radBtnMove.Checked = true;
            this.radBtnMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radBtnMove.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radBtnMove.Location = new System.Drawing.Point(153, 3);
            this.radBtnMove.Name = "radBtnMove";
            this.radBtnMove.Size = new System.Drawing.Size(94, 38);
            this.radBtnMove.TabIndex = 1;
            this.radBtnMove.TabStop = true;
            this.radBtnMove.Text = "Move";
            this.radBtnMove.UseVisualStyleBackColor = true;
            // 
            // goWebBrowser1
            // 
            this.goWebBrowser1.CreationProperties = null;
            this.goWebBrowser1.DefaultBackgroundColor = System.Drawing.Color.White;
            this.goWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goWebBrowser1.Location = new System.Drawing.Point(3, 603);
            this.goWebBrowser1.Name = "goWebBrowser1";
            this.goWebBrowser1.Size = new System.Drawing.Size(1280, 194);
            this.goWebBrowser1.TabIndex = 5;
            this.goWebBrowser1.ZoomFactor = 1D;
            // 
            // DrawCommandHandlerSampleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DrawCommandHandlerSampleControl";
            this.Size = new System.Drawing.Size(1286, 799);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.goWebBrowser1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private Northwoods.Go.WinForms.DiagramControl diagramControl1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Label lblAlign;
    private System.Windows.Forms.Button btnLeft;
    private System.Windows.Forms.Button btnColumn;
    private System.Windows.Forms.Button btnRow;
    private System.Windows.Forms.Button btnCenterY;
    private System.Windows.Forms.Button btnCenterX;
    private System.Windows.Forms.Button btnBottoms;
    private System.Windows.Forms.Button btnTops;
    private System.Windows.Forms.Button btnRight;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.Button btn180;
    private System.Windows.Forms.Button btnNeg90;
    private System.Windows.Forms.Button btn90;
    private System.Windows.Forms.Button btnNeg45;
    private System.Windows.Forms.Label lblRotate;
    private System.Windows.Forms.Button btn45;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    private System.Windows.Forms.Button btnBack;
    private System.Windows.Forms.Label lblZOrder;
    private System.Windows.Forms.Button btnFront;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
    private System.Windows.Forms.RadioButton radBtnTree;
    private System.Windows.Forms.RadioButton radBtnScroll;
    private System.Windows.Forms.RadioButton radBtnSelect;
    private System.Windows.Forms.Label lblArrow;
    private System.Windows.Forms.RadioButton radBtnMove;
    private WinFormsSharedControls.GoWebBrowser goWebBrowser1;
  }
}
