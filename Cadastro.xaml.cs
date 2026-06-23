using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class Cadastro : Window
{
    public Cadastro()
    {
        InitializeComponent();
        TxtNome.Focus();
    }

    private void BtnCadastrar_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtNome.Text) ||
            string.IsNullOrWhiteSpace(TxtUsername.Text) ||
            string.IsNullOrWhiteSpace(TxtEmail.Text) ||
            string.IsNullOrWhiteSpace(TxtSenha.Password))
        {
            MessageBox.Show("Todos os campos são obrigatórios.", "Erro!");
            return;
        }

        using var conexao = new MySqlConnection(App.StringConexao);
        const string query =
            "INSERT INTO usuarios(nome, username, email, senha) VALUES(@nome, @username, @email, @senha); SELECT LAST_INSERT_ID()";

        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@nome", TxtNome.Text);
        comando.Parameters.AddWithValue("@username", TxtUsername.Text);
        comando.Parameters.AddWithValue("@email", TxtEmail.Text);
        comando.Parameters.AddWithValue("@senha", TxtSenha.Password);

        try
        {
            conexao.Open();
            var idGerado = comando.ExecuteScalar();
            if (idGerado is null) throw new Exception("Cadastro não foi realizado");
            new Feed(new Usuario
            {
                Nome = TxtNome.Text,
                Email = TxtEmail.Text,
                Username = TxtUsername.Text,
                Id = Convert.ToInt32(idGerado)
            }).Show();
            Close();
        }
        catch (Exception exception)
        {
            if (exception is MySqlException { Number: 1062 })
            {
                MessageBox.Show("O email ou username já foram utilizados");
                return;
            }

            MessageBox.Show(exception.Message);
        }
        finally
        {
            conexao.Close();
        }
    }
}