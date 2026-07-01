using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;
using Npgsql;

namespace CRUD;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        TxtUsuario.Focus();
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

        using var conexao = new NpgsqlConnection(App.StringConexao);
        const string query = "SELECT * FROM usuarios WHERE username = @username AND senha = @senha";

        using var comando = new NpgsqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@username", TxtUsuario.Text);
        comando.Parameters.AddWithValue("@senha", TxtSenha.Password);

        try
        {
            conexao.Open();
            using var leitor = comando.ExecuteReader();
            if (!leitor.HasRows)
            {
                MessageBox.Show("Usuário e/ou senha estão errados.", "Erro!");
                return;
            }

            while (leitor.Read())
            {
                var usuarioBanco = new Usuario
                {
                    Id = Convert.ToInt32(leitor["id"]),
                    Nome = leitor["nome"].ToString()!,
                    Email = leitor["email"].ToString()!,
                    Username = leitor["username"].ToString()!
                };

                new Feed(usuarioBanco).Show();
                Close();
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Erro: {exception.Message}", "Erro!");
        }
        finally
        {
            conexao.Close();
        }
    }

    private void BtnCadastro_OnClick(object sender, RoutedEventArgs e)
    {
        new Cadastro().Show();
        Close();
    }
}