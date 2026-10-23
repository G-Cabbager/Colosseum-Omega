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
        lblStatus.Text = $"{uInfo}  |  {iInfo}    (继续调整参数可追加生成，完成后点 [保存地图])";
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

    // 点击"清空"按钮
    private void BtnClear_Click(object? sender, EventArgs e)
    {
        if (_allUnits.Count == 0 && _allInfantry.Count == 0)
            return;

        _allUnits.Clear();
        _allInfantry.Clear();
        UpdatePreview();
        UpdateStatus();
    }

    // 预设 -> FORMS 目录下的模板文件名(顺序与 cmbPreset 的项一一对应)
    private static readonly string[] PresetForms = { "FORM1.map", "FORM2.map", "FORM3.map" };

    // 点击"保存地图"按钮
    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_allUnits.Count == 0 && _allInfantry.Count == 0)
        {
            MessageBox.Show("还没有生成任何实体。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            // 定位数据根目录(内含 FORMS\FORM1.map):发布版把 FORMS/MAPS 与 exe 放一起即命中;
            // 仓库内原位开发运行(bin\Debug\net9.0-windows 下)则逐级向上找到仓库根,保持原有行为。
            string dataRoot = ResolveDataRoot();

            // 按“保存设置”里的预设选择模板
            int presetIndex = cmbPreset.SelectedIndex;
            if (presetIndex < 0 || presetIndex >= PresetForms.Length) presetIndex = 0;
            string presetName = PresetForms[presetIndex];

            // 读取模板
            string templatePath = Path.Combine(dataRoot, "FORMS", presetName);
            string templateContent = "";
            if (File.Exists(templatePath))
            {
                templateContent = File.ReadAllText(templatePath);
                // 空模板(如刚新建的 FORM2/FORM3)不补换行,避免输出开头多出一个空行
                if (templateContent.Length > 0 && !templateContent.EndsWith("\n"))
                    templateContent += "\n";
            }
            else
            {
                MessageBox.Show($"未找到 FORMS\\{presetName}(已查找: {templatePath}),将只输出生成的区块。",
                    "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // 拼接
            var sb = new StringBuilder();
            sb.Append(templateContent);
            if (_allUnits.Count > 0)
                sb.Append(Generator.WriteUnitsBlock(_allUnits, 0));
            if (_allInfantry.Count > 0)
                sb.Append(Generator.WriteInfantryBlock(_allInfantry, 0));

            // 输出目标:文件名 + 位置
            string fileName = txtFileName.Text.Trim();
            if (fileName.Length == 0) fileName = "test";
            if (!fileName.EndsWith(".map", StringComparison.OrdinalIgnoreCase))
                fileName += ".map";

            string dirInput = txtOutputDir.Text.Trim();
            if (dirInput.Length == 0) dirInput = "MAPS";
            // 相对路径按数据根解析(默认 MAPS);绝对路径原样使用,便于直接输出到游戏地图目录
            string outputDir = Path.IsPathRooted(dirInput) ? dirInput : Path.Combine(dataRoot, dirInput);
            string outputPath = Path.Combine(outputDir, fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
            File.WriteAllText(outputPath, sb.ToString());

            MessageBox.Show(
                $"已保存到:\n{outputPath}\n\n模板: FORMS\\{presetName}\n[Units] {_allUnits.Count} 条  [Infantry] {_allInfantry.Count} 条",
                "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // 点击“浏览…”按钮:用文件夹对话框选择输出位置
    private void BtnBrowseOutputDir_Click(object? sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description = "选择地图输出位置",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = true
        };

        // 当前填的是已存在的目录(相对或绝对)时,作为对话框的起始位置
        string current = txtOutputDir.Text.Trim();
        if (current.Length > 0)
        {
            string resolved = Path.IsPathRooted(current) ? current : Path.Combine(ResolveDataRoot(), current);
            if (Directory.Exists(resolved))
                dlg.InitialDirectory = resolved;
        }

        // 选定后写入绝对路径(保存时按 Path.IsPathRooted 原样使用)
        if (dlg.ShowDialog(this) == DialogResult.OK && dlg.SelectedPath.Length > 0)
            txtOutputDir.Text = dlg.SelectedPath;
    }

    /// <summary>
    /// 定位“数据根目录”(内含 FORMS\FORM1.map 的目录)。
    /// - 发布/独立运行:把 FORMS、MAPS 与 exe 放在同一目录即可命中;
    /// - 仓库内开发运行:从 bin\Debug\net9.0-windows 逐级向上,仍能找到仓库根,行为不变;
    /// - 兜底:整个盘都找不到模板时退回 exe 所在目录。
    /// </summary>
    private static string ResolveDataRoot()
    {
        var dir = new DirectoryInfo(Application.StartupPath);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "FORMS", "FORM1.map")))
                return dir.FullName;
            dir = dir.Parent;
        }
        return Application.StartupPath;
    }
}