namespace MapSpawnerGUI;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.GroupBox grpEntityType;
    private System.Windows.Forms.RadioButton radioUnit;
    private System.Windows.Forms.RadioButton radioInfantry;
    private System.Windows.Forms.GroupBox grpCommon;
    private System.Windows.Forms.Label lblOwner;
    private System.Windows.Forms.TextBox txtOwner;
    private System.Windows.Forms.Label lblType;
    private System.Windows.Forms.TextBox txtType;
    private System.Windows.Forms.GroupBox grpUnit;
    private System.Windows.Forms.Label lblFacing;
    private System.Windows.Forms.NumericUpDown numFacing;
    private System.Windows.Forms.Label lblVeterancy;
    private System.Windows.Forms.NumericUpDown numVeterancy;
    private System.Windows.Forms.Label lblGroup;
    private System.Windows.Forms.NumericUpDown numGroup;
    private System.Windows.Forms.GroupBox grpInfantry;
    private System.Windows.Forms.Label lblSubPos;
    private System.Windows.Forms.NumericUpDown numSubPos;
    private System.Windows.Forms.Label lblVeterancyInf;
    private System.Windows.Forms.NumericUpDown numVeterancyInf;
    private System.Windows.Forms.Label lblGroupInf;
    private System.Windows.Forms.NumericUpDown numGroupInf;
    private System.Windows.Forms.GroupBox grpLayout;
    private System.Windows.Forms.Label lblDirection;
    private System.Windows.Forms.ComboBox cmbDirection;
    private System.Windows.Forms.Label lblCornerX;
    private System.Windows.Forms.NumericUpDown numCornerX;
    private System.Windows.Forms.Label lblCornerY;
    private System.Windows.Forms.NumericUpDown numCornerY;
    private System.Windows.Forms.Label lblRows;
    private System.Windows.Forms.NumericUpDown numRows;
    private System.Windows.Forms.Label lblTotal;
    private System.Windows.Forms.NumericUpDown numTotal;
    private System.Windows.Forms.Button btnGenerate;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.GroupBox grpOutput;
    private System.Windows.Forms.TextBox txtOutput;
    private System.Windows.Forms.Label lblStatus;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        grpEntityType = new GroupBox();
        radioUnit = new RadioButton();
        radioInfantry = new RadioButton();
        grpCommon = new GroupBox();
        lblOwner = new Label();
        txtOwner = new TextBox();
        lblType = new Label();
        txtType = new TextBox();
        grpUnit = new GroupBox();
        lblFacing = new Label();
        numFacing = new NumericUpDown();
        lblVeterancy = new Label();
        numVeterancy = new NumericUpDown();
        lblGroup = new Label();
        numGroup = new NumericUpDown();
        grpInfantry = new GroupBox();
        lblSubPos = new Label();
        numSubPos = new NumericUpDown();
        grpLayout = new GroupBox();
        lblDirection = new Label();
        cmbDirection = new ComboBox();
        lblCornerX = new Label();
        numCornerX = new NumericUpDown();
        lblCornerY = new Label();
        numCornerY = new NumericUpDown();
        lblRows = new Label();
        numRows = new NumericUpDown();
        lblTotal = new Label();
        numTotal = new NumericUpDown();
        btnGenerate = new Button();
        btnSave = new Button();
        grpOutput = new GroupBox();
        txtOutput = new TextBox();
        lblStatus = new Label();
        grpEntityType.SuspendLayout();
        grpCommon.SuspendLayout();
        grpUnit.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numFacing).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numVeterancy).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numGroup).BeginInit();
        grpInfantry.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numSubPos).BeginInit();
        grpLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numCornerX).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numCornerY).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numRows).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numTotal).BeginInit();
        grpOutput.SuspendLayout();
        SuspendLayout();

        // grpEntityType
        grpEntityType.Controls.Add(radioUnit);
        grpEntityType.Controls.Add(radioInfantry);
        grpEntityType.Location = new System.Drawing.Point(12, 12);
        grpEntityType.Size = new System.Drawing.Size(280, 55);
        grpEntityType.Text = "生成类型";

        radioUnit.AutoSize = true;
        radioUnit.Location = new System.Drawing.Point(20, 24);
        radioUnit.Size = new System.Drawing.Size(65, 21);
        radioUnit.Text = "单位 (Unit)";
        radioUnit.Checked = true;
        radioUnit.CheckedChanged += EntityTypeChanged;

        radioInfantry.AutoSize = true;
        radioInfantry.Location = new System.Drawing.Point(140, 24);
        radioInfantry.Size = new System.Drawing.Size(86, 21);
        radioInfantry.Text = "步兵 (Infantry)";
        radioInfantry.CheckedChanged += EntityTypeChanged;

        // grpCommon
        grpCommon.Controls.Add(lblOwner);
        grpCommon.Controls.Add(txtOwner);
        grpCommon.Controls.Add(lblType);
        grpCommon.Controls.Add(txtType);
        grpCommon.Location = new System.Drawing.Point(12, 73);
        grpCommon.Size = new System.Drawing.Size(610, 70);
        grpCommon.Text = "通用参数";

        lblOwner.Location = new System.Drawing.Point(15, 32);
        lblOwner.Size = new System.Drawing.Size(60, 20);
        lblOwner.Text = "所属方:";

        txtOwner.Location = new System.Drawing.Point(80, 30);
        txtOwner.Size = new System.Drawing.Size(200, 23);
        txtOwner.Text = "UnitedStates";

        lblType.Location = new System.Drawing.Point(305, 32);
        lblType.Size = new System.Drawing.Size(60, 20);
        lblType.Text = "类型代码:";

        txtType.Location = new System.Drawing.Point(370, 30);
        txtType.Size = new System.Drawing.Size(200, 23);
        txtType.Text = "MTNK";

        // grpUnit
        grpUnit.Controls.Add(lblFacing);
        grpUnit.Controls.Add(numFacing);
        grpUnit.Controls.Add(lblVeterancy);
        grpUnit.Controls.Add(numVeterancy);
        grpUnit.Controls.Add(lblGroup);
        grpUnit.Controls.Add(numGroup);
        grpUnit.Location = new System.Drawing.Point(12, 149);
        grpUnit.Size = new System.Drawing.Size(610, 60);
        grpUnit.Text = "单位参数";

        lblFacing.Location = new System.Drawing.Point(15, 28);
        lblFacing.Size = new System.Drawing.Size(50, 20);
        lblFacing.Text = "朝向:";

        numFacing.Location = new System.Drawing.Point(65, 26);
        numFacing.Size = new System.Drawing.Size(80, 23);
        numFacing.Maximum = 255;
        numFacing.Value = 64;

        lblVeterancy.Location = new System.Drawing.Point(175, 28);
        lblVeterancy.Size = new System.Drawing.Size(60, 20);
        lblVeterancy.Text = "经验等级:";

        numVeterancy.Location = new System.Drawing.Point(240, 26);
        numVeterancy.Size = new System.Drawing.Size(80, 23);
        numVeterancy.Maximum = 2;
        numVeterancy.Value = 0;

        lblGroup.Location = new System.Drawing.Point(350, 28);
        lblGroup.Size = new System.Drawing.Size(60, 20);
        lblGroup.Text = "分组:";

        numGroup.Location = new System.Drawing.Point(410, 26);
        numGroup.Size = new System.Drawing.Size(80, 23);
        numGroup.Minimum = -1;
        numGroup.Maximum = 9;
        numGroup.Value = -1;

        // grpInfantry
        grpInfantry.Controls.Add(lblSubPos);
        grpInfantry.Controls.Add(numSubPos);
        grpInfantry.Controls.Add(lblVeterancyInf);
        grpInfantry.Controls.Add(numVeterancyInf);
        grpInfantry.Controls.Add(lblGroupInf);
        grpInfantry.Controls.Add(numGroupInf);
        grpInfantry.Location = new System.Drawing.Point(12, 149);
        grpInfantry.Size = new System.Drawing.Size(610, 60);
        grpInfantry.Text = "步兵参数";

        lblSubPos.Location = new System.Drawing.Point(15, 28);
        lblSubPos.Size = new System.Drawing.Size(50, 20);
        lblSubPos.Text = "站位:";

        numSubPos.Location = new System.Drawing.Point(65, 26);
        numSubPos.Size = new System.Drawing.Size(80, 23);
        numSubPos.Minimum = 1;
        numSubPos.Maximum = 3;
        numSubPos.Value = 1;

        lblVeterancyInf.Location = new System.Drawing.Point(175, 28);
        lblVeterancyInf.Size = new System.Drawing.Size(60, 20);
        lblVeterancyInf.Text = "经验等级:";

        numVeterancyInf.Location = new System.Drawing.Point(240, 26);
        numVeterancyInf.Size = new System.Drawing.Size(80, 23);
        numVeterancyInf.Maximum = 200;
        numVeterancyInf.Increment = 100;
        numVeterancyInf.Value = 0;

        lblGroupInf.Location = new System.Drawing.Point(350, 28);
        lblGroupInf.Size = new System.Drawing.Size(45, 20);
        lblGroupInf.Text = "分组:";

        numGroupInf.Location = new System.Drawing.Point(405, 26);
        numGroupInf.Size = new System.Drawing.Size(80, 23);
        numGroupInf.Minimum = -1;
        numGroupInf.Maximum = 9;
        numGroupInf.Value = -1;

        // grpLayout
        grpLayout.Controls.Add(lblDirection);
        grpLayout.Controls.Add(cmbDirection);
        grpLayout.Controls.Add(lblCornerX);
        grpLayout.Controls.Add(numCornerX);
        grpLayout.Controls.Add(lblCornerY);
        grpLayout.Controls.Add(numCornerY);
        grpLayout.Controls.Add(lblRows);
        grpLayout.Controls.Add(numRows);
        grpLayout.Controls.Add(lblTotal);
        grpLayout.Controls.Add(numTotal);
        grpLayout.Location = new System.Drawing.Point(12, 215);
        grpLayout.Size = new System.Drawing.Size(610, 100);
        grpLayout.Text = "布局参数";

        lblDirection.Location = new System.Drawing.Point(15, 30);
        lblDirection.Size = new System.Drawing.Size(50, 20);
        lblDirection.Text = "方向:";

        cmbDirection.Location = new System.Drawing.Point(65, 28);
        cmbDirection.Size = new System.Drawing.Size(80, 23);
        cmbDirection.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDirection.Items.AddRange(new object[] { "左向", "右向" });
        cmbDirection.SelectedIndex = 0;

        lblCornerX.Location = new System.Drawing.Point(175, 30);
        lblCornerX.Size = new System.Drawing.Size(50, 20);
        lblCornerX.Text = "角 X:";

        numCornerX.Location = new System.Drawing.Point(225, 28);
        numCornerX.Size = new System.Drawing.Size(80, 23);
        numCornerX.Maximum = 1000;
        numCornerX.Value = 10;

        lblCornerY.Location = new System.Drawing.Point(330, 30);
        lblCornerY.Size = new System.Drawing.Size(50, 20);
        lblCornerY.Text = "角 Y:";

        numCornerY.Location = new System.Drawing.Point(380, 28);
        numCornerY.Size = new System.Drawing.Size(80, 23);
        numCornerY.Maximum = 1000;
        numCornerY.Value = 10;

        lblRows.Location = new System.Drawing.Point(15, 70);
        lblRows.Size = new System.Drawing.Size(50, 20);
        lblRows.Text = "行数:";

        numRows.Location = new System.Drawing.Point(65, 68);
        numRows.Size = new System.Drawing.Size(80, 23);
        numRows.Minimum = 1;
        numRows.Maximum = 100;
        numRows.Value = 3;

        lblTotal.Location = new System.Drawing.Point(175, 70);
        lblTotal.Size = new System.Drawing.Size(50, 20);
        lblTotal.Text = "总数:";

        numTotal.Location = new System.Drawing.Point(225, 68);
        numTotal.Size = new System.Drawing.Size(80, 23);
        numTotal.Minimum = 1;
        numTotal.Maximum = 1000;
        numTotal.Value = 6;

        // btnGenerate
        btnGenerate.Location = new System.Drawing.Point(12, 330);
        btnGenerate.Size = new System.Drawing.Size(120, 35);
        btnGenerate.Text = "追加生成";
        btnGenerate.Font = new System.Drawing.Font("Microsoft YaHei", 10F);
        btnGenerate.Click += BtnGenerate_Click;

        // btnSave
        btnSave.Location = new System.Drawing.Point(150, 330);
        btnSave.Size = new System.Drawing.Size(180, 35);
        btnSave.Text = "保存到 test.map";
        btnSave.Font = new System.Drawing.Font("Microsoft YaHei", 10F);
        btnSave.Click += BtnSave_Click;

        // lblStatus
        lblStatus.Location = new System.Drawing.Point(12, 380);
        lblStatus.Size = new System.Drawing.Size(980, 25);
        lblStatus.Text = "提示: 点击 [追加生成] 可多次累积，点击 [保存到 test.map] 写出文件";

        // grpOutput
        grpOutput.Controls.Add(txtOutput);
        grpOutput.Location = new System.Drawing.Point(12, 410);
        grpOutput.Size = new System.Drawing.Size(980, 510);
        grpOutput.Text = "输出预览";

        txtOutput.Location = new System.Drawing.Point(15, 25);
        txtOutput.Size = new System.Drawing.Size(950, 470);
        txtOutput.Multiline = true;
        txtOutput.ReadOnly = true;
        txtOutput.ScrollBars = ScrollBars.Both;
        txtOutput.Font = new System.Drawing.Font("Consolas", 10F);
        txtOutput.WordWrap = false;

        // Form1
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1004, 935);
        Controls.Add(grpEntityType);
        Controls.Add(grpCommon);
        Controls.Add(grpUnit);
        Controls.Add(grpInfantry);
        Controls.Add(grpLayout);
        Controls.Add(btnGenerate);
        Controls.Add(btnSave);
        Controls.Add(lblStatus);
        Controls.Add(grpOutput);
        Text = "RA2 Map Spawner";
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

        // 默认显示单位面板，隐藏步后面板
        grpInfantry.Visible = false;
        grpInfantry.SendToBack();
        grpUnit.BringToFront();

        grpEntityType.ResumeLayout(false);
        grpEntityType.PerformLayout();
        grpCommon.ResumeLayout(false);
        grpCommon.PerformLayout();
        grpUnit.ResumeLayout(false);
        grpUnit.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numFacing).EndInit();
        ((System.ComponentModel.ISupportInitialize)numVeterancy).EndInit();
        ((System.ComponentModel.ISupportInitialize)numGroup).EndInit();
        grpInfantry.ResumeLayout(false);
        grpInfantry.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numSubPos).EndInit();
        grpLayout.ResumeLayout(false);
        grpLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numCornerX).EndInit();
        ((System.ComponentModel.ISupportInitialize)numCornerY).EndInit();
        ((System.ComponentModel.ISupportInitialize)numRows).EndInit();
        ((System.ComponentModel.ISupportInitialize)numTotal).EndInit();
        grpOutput.ResumeLayout(false);
        grpOutput.PerformLayout();
        ResumeLayout(false);
    }
}