namespace MapSpawnerGUI;

static class Program
{
    [STAThread]
    static void Main()
    {
        // ApplicationConfiguration.Initialize() 内部会调用 EnableVisualStyles(),
        // 后者需要往 %TEMP% 写一个激活上下文清单文件。若该写入被安全软件、
        // 组策略或权限设置拦截,会抛 UnauthorizedAccessException —— 这时程序
        // 连主窗体都打不开。视觉样式只是外观,不该致命,因此降级继续运行。
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
}
