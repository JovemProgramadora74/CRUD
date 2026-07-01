using System.Windows;

namespace CRUD;

public partial class App : Application
{
    internal static string? StringConexao;

    protected override void OnStartup(StartupEventArgs e)
    {
        StringConexao = Environment.GetEnvironmentVariable("SUPABASE_STRING");

        base.OnStartup(e);
    }
}