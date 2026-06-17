using System.Windows;
using DotNetEnv;

namespace CRUD;

public partial class App : Application
{
    internal static string? StringConexao;

    protected override void OnStartup(StartupEventArgs e)
    {
        Env.Load("C:\\Users\\Aluno\\RiderProjects\\CRUD\\.env");

        StringConexao = Environment.GetEnvironmentVariable("MYSQL_STRING");

        base.OnStartup(e);
    }
}