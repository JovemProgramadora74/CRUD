using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

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

        using var conexao = new MySqlConnection(App.StringConexao);
        const string query = "SELECT * FROM usuarios WHERE username = @username AND senha = @senha";

        using var comando = new MySqlCommand(query, conexao);
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
                    Id = leitor.GetInt32("id"),
                    Nome = leitor.GetString("nome"),
                    Email = leitor.GetString("email"),
                    Username = leitor.GetString("username")
                };

                new Feed(usuarioBanco).Show();
                Close();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void BtnCadastro_OnClick(object sender, RoutedEventArgs e)
    {
        new Cadastro().Show();
        Close();
    }
}