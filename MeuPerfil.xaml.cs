using System.Windows;
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
        if (string.IsNullOrWhiteSpace(TxtNome.Text))
        {
            MessageBox.Show("O campo NOME não pode estar vazio.");
            TxtNome.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(TxtEmail.Text))
        {
            MessageBox.Show("O campo EMAIL não pode estar vazio.");
            TxtEmail.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(TxtUsername.Text))
        {
            MessageBox.Show("O campo USERNAME não pode estar vazio.");
            TxtUsername.Focus();
            return;
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

        // Criar uma query
        const string query = "DELETE FROM usuarios WHERE id = @id";
        // Criar a conexao
        using var conexao = new MySqlConnection(App.StringConexao);
        // Criar o comando
        using var comando = new MySqlCommand(query, conexao);
        // Adicionar os parametros
        comando.Parameters.AddWithValue("@id", _usuarioAtual.Id);
        try
        {
            // Abrir conexao
            conexao.Open();
            // Executar o comando
            var linhasAfetadas = comando.ExecuteNonQuery();
            // Verificar se o comando foi executado
            if (linhasAfetadas < 1) throw new Exception("Erro ao excluir perfil!");

            MessageBox.Show("Perfil deletado com sucesso!");
            // Se ele foi executado, fechar a janela MeuPerfil
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