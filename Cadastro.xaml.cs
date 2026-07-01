using System.Windows;
using System.Windows.Controls;
using CRUD.Modelos;
using MySql.Data.MySqlClient;
using Npgsql;

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
        Dictionary<TextBox, string> caixasTexto = new()
        {
            { TxtNome, "NOME" },
            { TxtUsername, "USERNAME" },
            { TxtEmail, "EMAIL" }
        };

        foreach (var caixa in caixasTexto.Where(caixa => string.IsNullOrWhiteSpace(caixa.Key.Text)))
        {
            MessageBox.Show($"O campo {caixa.Value} não pode estar vazio.", "Erro!");
            caixa.Key.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(TxtSenha.Password))
        {
            MessageBox.Show("O campo SENHA não pode estar vazio.", "Erro!");
            TxtSenha.Focus();
            return;
        }

        using var conexao = new NpgsqlConnection(App.StringConexao);
        const string query =
            "INSERT INTO usuarios(nome, username, email, senha) VALUES(@nome, @username, @email, @senha) RETURNING id";

        using var comando = new NpgsqlCommand(query, conexao);
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
            if (exception is NpgsqlException { SqlState: "23505" })
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