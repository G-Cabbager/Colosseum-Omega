namespace MapSpawnerGUI;

static class Program
{
    [STAThread]
    static void Main()
    {
        try
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"程序启动失败:\n\n异常类型: {ex.GetType().Name}\n消息: {ex.Message}\n堆栈:\n{ex.StackTrace}",
                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }    
}
