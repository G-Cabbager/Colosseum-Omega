namespace ColosseumOmega;

static class Program
{
    [STAThread]
    static void Main()
    {
        // 先确保临时目录可写:部分环境(杀毒软件、组策略、受限权限)会拦截程序
        // 在 %TEMP% 下创建文件,导致 EnableVisualStyles 写激活上下文清单时
        // 抛 UnauthorizedAccessException,程序在进主窗体之前就崩溃。
        EnsureWritableTemp();

        // 换目录仍失败时,退化为不使用视觉样式继续运行 —— 外观朴素,但程序可用。
        string? styleWarning = null;
        try
        {
            ApplicationConfiguration.Initialize();
        }
        catch (Exception ex)
        {
            styleWarning = $"{ex.GetType().Name}: {ex.Message}";
            try
            {
                Application.SetCompatibleTextRenderingDefault(false);
            }
            catch
            {
                // 兜底失败也无所谓,继续往下走
            }
        }

        try
        {
            var form = new Form1();
            if (styleWarning != null)
                form.ShowStartupWarning(styleWarning);
            Application.Run(form);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"程序启动失败:\n\n异常类型: {ex.GetType().Name}\n消息: {ex.Message}\n堆栈:\n{ex.StackTrace}",
                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// 检测临时目录是否可写;不可写时把本进程的 TEMP/TMP 改到可写位置。
    /// 候选顺序:程序目录下的 .tmp → %LOCALAPPDATA%\ColosseumOmega\tmp。
    /// 全部失败则不做改动,由调用方降级处理。
    /// </summary>
    private static void EnsureWritableTemp()
    {
        if (CanWriteTemp()) return;

        var candidates = new List<string>();

        string baseDir = AppContext.BaseDirectory;
        if (!string.IsNullOrEmpty(baseDir))
            candidates.Add(Path.Combine(baseDir, ".tmp"));

        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (!string.IsNullOrEmpty(localAppData))
            candidates.Add(Path.Combine(localAppData, "ColosseumOmega", "tmp"));

        foreach (string dir in candidates)
        {
            try
            {
                Directory.CreateDirectory(dir);
                Environment.SetEnvironmentVariable("TEMP", dir);
                Environment.SetEnvironmentVariable("TMP", dir);
                if (CanWriteTemp()) return;   // 切换成功,收工
            }
            catch
            {
                // 该候选不可用,试下一个
            }
        }
    }

    /// <summary>在当前 TEMP 下实际创建一个文件来验证可写性(DeleteOnClose 不留垃圾)。</summary>
    private static bool CanWriteTemp()
    {
        try
        {
            string dir = Path.GetTempPath();
            Directory.CreateDirectory(dir);
            string probe = Path.Combine(dir, "ColosseumOmega_" + Guid.NewGuid().ToString("N") + ".tmp");
            using (new FileStream(probe, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1, FileOptions.DeleteOnClose))
            {
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}
