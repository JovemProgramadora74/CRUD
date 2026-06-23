using System.Windows;
using System.Windows.Controls;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class MeuPerfil : Window
{
    private readonly Usuario _usuarioAtual;

    public MeuPerfil(Usuario usuario)
    {
        InitializeComponent();
        _usuarioAtual = usuario;
        TxtNome.Text = _usuarioAtual.Nome;
        TxtEmail.Text = _usuarioAtual.Email;
        TxtUsername.Text = _usuarioAtual.Username;
    }

    private void BtnSalvar_OnClick(object sender, RoutedEventArgs e)
    {
        Dictionary<TextBox, string> caixasTexto = new ()
        {
            { TxtNome, "NOME" },
            { TxtEmail, "EMAIL" },
            { TxtUsername, "USERNAME" }
        };

        foreach (var caixa in caixasTexto)
        {
            if (string.IsNullOrWhiteSpace(caixa.Key.Text))
            {
                MessageBox.Show($"O campo {caixa.Value} não pode estar vazio.");
                caixa.Key.Focus();
                return;
            }
        }

        var senhaFoiAlterada = !string.IsNullOrWhiteSpace(TxtSenha.Password);

        _usuarioAtual.Username = TxtUsername.Text;
        _usuarioAtual.Nome = TxtNome.Text;
        _usuarioAtual.Email = TxtEmail.Text;
        if (senhaFoiAlterada) _usuarioAtual.Senha = TxtSenha.Password;

        using var conexao = new MySqlConnection(App.StringConexao);
        var query = "UPDATE usuarios SET username = @username, nome = @nome, email = @email";

        if (senhaFoiAlterada) query += ", senha = @senha";

        query += " WHERE id = @id";

        using var comando = new MySqlCommand(query, conexao);

        comando.Parameters.AddWithValue("@username", _usuarioAtual.Username);
        comando.Parameters.AddWithValue("@nome", _usuarioAtual.Nome);
        comando.Parameters.AddWithValue("@email", _usuarioAtual.Email);
        comando.Parameters.AddWithValue("@id", _usuarioAtual.Id);

        if (senhaFoiAlterada) comando.Parameters.AddWithValue("@senha", _usuarioAtual.Senha);

        try
        {
            conexao.Open();
            var linhasAfetadas = comando.ExecuteNonQuery();

            if (linhasAfetadas < 1) throw new Exception("Erro ao atualizar o cadastro!");

            MessageBox.Show("Cadastro atualizado com sucesso!");
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Erro no banco: {exception.Message}");
        }
        finally
        {
            conexao.Close();
        }
    }

    private void BtnDeletarPerfil_OnClick(object sender, RoutedEventArgs e)
    {
        var resultadoMessageBox = MessageBox.Show("Você tem certeza que deseja apagar o seu perfil?",
            "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (resultadoMessageBox == MessageBoxResult.No) return;
        
        const string query = "DELETE FROM usuarios WHERE id = @id";
        
        using var conexao = new MySqlConnection(App.StringConexao);
        
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@id", _usuarioAtual.Id);
        
        try
        {
            conexao.Open();

            var linhasAfetadas = comando.ExecuteNonQuery();
            if (linhasAfetadas < 1) throw new Exception("Erro ao excluir perfil!");

            MessageBox.Show("Perfil deletado com sucesso!");
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Erro no banco: {exception.Message}");
        }
        finally
        {
            conexao.Close();
        }
    }
}