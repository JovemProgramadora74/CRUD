using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class MeuPerfil : Window
{
    private readonly Usuario UsuarioAtual;

    public MeuPerfil(Usuario usuario)
    {
        InitializeComponent();
        UsuarioAtual = usuario;
        TxtNome.Text = UsuarioAtual.Nome;
        TxtEmail.Text = UsuarioAtual.Email;
        TxtUsername.Text = UsuarioAtual.Username;
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

        UsuarioAtual.Username = TxtUsername.Text;
        UsuarioAtual.Nome = TxtNome.Text;
        UsuarioAtual.Email = TxtEmail.Text;
        if (senhaFoiAlterada) UsuarioAtual.Senha = TxtSenha.Password;

        using var conexao = new MySqlConnection(App.StringConexao);
        var query = "UPDATE usuarios SET username = @username, nome = @nome, email = @email";

        if (senhaFoiAlterada) query += ", senha = @senha";

        query += " WHERE id = @id";

        using var comando = new MySqlCommand(query, conexao);

        comando.Parameters.AddWithValue("@username", UsuarioAtual.Username);
        comando.Parameters.AddWithValue("@nome", UsuarioAtual.Nome);
        comando.Parameters.AddWithValue("@email", UsuarioAtual.Email);
        comando.Parameters.AddWithValue("@id", UsuarioAtual.Id);

        if (senhaFoiAlterada) comando.Parameters.AddWithValue("@senha", UsuarioAtual.Senha);

        try
        {
            conexao.Open();
            var linhasAfetadas = comando.ExecuteNonQuery();

            if (linhasAfetadas > 0)
                MessageBox.Show("Cadastro atualizado com sucesso!");
            else
                MessageBox.Show("Erro ao atualizar o cadastro!");
        }
        catch (Exception exception)
        {
            MessageBox.Show("Erro de DB.");
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
        comando.Parameters.AddWithValue("@id", UsuarioAtual.Id);
        try
        {
            // Abrir conexao
            conexao.Open();
            // Executar o comando
            var linhasAfetadas = comando.ExecuteNonQuery();
            // Verificar se o comando foi executado
            if (linhasAfetadas > 0)
            {
                MessageBox.Show("Perfil deletado com sucesso!");
                // Se ele foi executado, fechar a janela MeuPerfil
                Close();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}