using System.Windows;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class Cadastro : Window
{
    public Cadastro()
    {
        InitializeComponent();
    }

    private void BtnCadastrar_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtNome.Text) ||
            string.IsNullOrWhiteSpace(TxtEmail.Text) ||
            string.IsNullOrWhiteSpace(TxtUsername.Text) ||
            string.IsNullOrWhiteSpace(TxtSenha.Password))
        {
            MessageBox.Show("Todos os campos são obrigatórios!", "Erro!");
            return;
        }

        using (var conexao = new MySqlConnection(App.StringConexao))
        {
            var query = "INSERT INTO usuarios(nome, username, email, senha) VALUES(@nome, @username, @email, @senha)";

            using (var comando = new MySqlCommand(query, conexao))
            {
                comando.Parameters.AddWithValue("@nome", TxtNome.Text);
                comando.Parameters.AddWithValue("@username", TxtUsername.Text);
                comando.Parameters.AddWithValue("@email", TxtEmail.Text);
                comando.Parameters.AddWithValue("@senha", TxtSenha.Password);

                try
                {
                    conexao.Open();
                    var linhasAfetadas = comando.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        MessageBox.Show("Cadastro realizado!");
                    }
                }
                catch (Exception exception)
                {
                    if (exception is MySqlException erroSql)
                    {
                        if (erroSql.Number == 1062)
                        {
                            MessageBox.Show("O email ou username já foram utilizados!");
                            return;
                        }
                    }
                    
                    Console.WriteLine(exception);
                    return;
                }
            }
        }
    }
}