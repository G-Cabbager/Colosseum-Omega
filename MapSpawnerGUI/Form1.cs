using System.Text;

namespace MapSpawnerGUI;

public partial class Form1 : Form
{
    // 按类型累积实体（各自从 0 开始编号）
    private readonly List<BaseEntity> _allUnits = new();
    private readonly List<BaseEntity> _allInfantry = new();

    public Form1()
    {
        InitializeComponent();
        UpdateStatus();
    }

    // 切换单位/步后面板
    private void EntityTypeChanged(object? sender, EventArgs e)
    {
        bool isUnit = radioUnit.Checked;
        grpUnit.Visible = isUnit;
        grpInfantry.Visible = !isUnit;
    }

    // 点击"追加生成"按钮
    private void BtnGenerate_Click(object? sender, EventArgs e)
    {
        try
        {
            var entityType = radioUnit.Checked ? EntityType.Unit : EntityType.Infantry;
            BaseEntity template;
            if (entityType == EntityType.Unit)
            {
                template = new BaseEntity
                {
                    Owner = txtOwner.Text.Trim(),
                    Type = txtType.Text.Trim(),
                    Facing = (int)numFacing.Value,
                    Veterancy = (int)numVeterancy.Value,
                    Group = (int)numGroup.Value,
                    SubPos = 1
                };
            }
            else
            {
                template = new BaseEntity
                {
                    Owner = txtOwner.Text.Trim(),
                    Type = txtType.Text.Trim(),
                    Facing = 64,
                    Veterancy = (int)numVeterancyInf.Value,
                    Group = (int)numGroupInf.Value,
                    SubPos = (int)numSubPos.Value
                };
            }

            bool isRight = cmbDirection.SelectedIndex == 1;
            int cornerX = (int)numCornerX.Value;
            int cornerY = (int)numCornerY.Value;
            int rows = (int)numRows.Value;
            int total = (int)numTotal.Value;

            // 生成实体
            var entities = Generator.GenerateEntities(
                isRight, cornerX, cornerY, rows, total, template, entityType);

            // 追加到对应容器
            if (entityType == EntityType.Unit)
                _allUnits.AddRange(entities);
            else
                _allInfantry.AddRange(entities);

            // 更新输出预览
            UpdatePreview();
            UpdateStatus();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"生成出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // 更新状态栏
    private void UpdateStatus()
    {
        string uInfo = _allUnits.Count > 0
            ? $"[Units] {_allUnits.Count} 条 (序号 0~{_allUnits.Count - 1})"
            : "[Units] 0 条";
        string iInfo = _allInfantry.Count > 0
            ? $"[Infantry] {_allInfantry.Count} 条 (序号 0~{_allInfantry.Count - 1})"
            : "[Infantry] 0 条";
        lblStatus.Text = $"{uInfo}  |  {iInfo}    (继续调整参数可追加生成，完成后点 [保存到 test.map])";
    }

    // 更新输出预览
    private void UpdatePreview()
    {
        var sb = new StringBuilder();
        if (_allUnits.Count > 0)
            sb.Append(Generator.WriteUnitsBlock(_allUnits, 0));
        if (_allInfantry.Count > 0)
            sb.Append(Generator.WriteInfantryBlock(_allInfantry, 0));
        txtOutput.Text = sb.ToString();
    }

    // 点击"保存到 test.map"按钮
    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_allUnits.Count == 0 && _allInfantry.Count == 0)
        {
            MessageBox.Show("还没有生成任何实体。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            // 读取模板
            string templatePath = Path.Combine(Application.StartupPath, "..\\..\\..\\..\\FORMS\\FORM1.map");
            string templateContent = "";
            if (File.Exists(templatePath))
            {
                templateContent = File.ReadAllText(templatePath);
                if (!templateContent.EndsWith("\n"))
                    templateContent += "\n";
            }
            else
            {
                MessageBox.Show("未找到 FORMS/FORM1.map，将只输出生成的区块。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // 拼接
            var sb = new StringBuilder();
            sb.Append(templateContent);
            if (_allUnits.Count > 0)
                sb.Append(Generator.WriteUnitsBlock(_allUnits, 0));
            if (_allInfantry.Count > 0)
                sb.Append(Generator.WriteInfantryBlock(_allInfantry, 0));

            // 写出
            string outputPath = Path.Combine(Application.StartupPath, "..\\..\\..\\..\\MAPS\\test.map");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
            File.WriteAllText(outputPath, sb.ToString());

            MessageBox.Show($"已保存到:\n{outputPath}\n\n[Units] {_allUnits.Count} 条  [Infantry] {_allInfantry.Count} 条",
                "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}