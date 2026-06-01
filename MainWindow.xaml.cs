using System.Windows;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnLogin_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtUsuario.Text))
        {
            MessageBox.Show("Preencha o campo de usuário!");
            TxtUsuario.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(TxtSenha.Password))
        {
            MessageBox.Show("Preencha o campo de senha!");
            TxtSenha.Focus();
            return;
        }

        using (var conexao = new MySqlConnection(App.StringConexao))
        {
            var query = "SELECT * FROM usuarios WHERE username = @username AND senha = @senha";

            using (var comando = new MySqlCommand(query, conexao))
            {
                comando.Parameters.AddWithValue("@username", TxtUsuario.Text);
                comando.Parameters.AddWithValue("@senha", TxtSenha.Password);

                try
                {
                    conexao.Open();
                    using (var leitor = comando.ExecuteReader())
                    {
                        if (!leitor.HasRows)
                        {
                            MessageBox.Show("Usuário e/ou senha incorretos.", "Erro!");
                            return;
                        }

                        while (leitor.Read())
                        {
                            MessageBox.Show(leitor.GetString(1));
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    return;
                }
            }
        }
    }
}