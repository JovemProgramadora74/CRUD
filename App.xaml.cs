using System.Windows;
using DotNetEnv;

namespace CRUD;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        Env.Load("C:\\Users\\Aluno\\RiderProjects\\CRUD\\.env");
        
        base.OnStartup(e);
    }
}