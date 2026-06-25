using System.Windows;
using System.Windows.Controls;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class JanelaPostagem : Window
{
    private readonly Usuario _usuario;
    private readonly Postagem? _postagem;
    private readonly bool _ehEdicao = false;

    // Primeiro construtor, utilizado para criar uma postagem
    public JanelaPostagem(Usuario usuario)
    {
        _usuario = usuario;
        InitializeComponent();
        TbConteudo.Focus();
    }
    
    // Segundo construtor, utilizado para editar uma postagem
    public JanelaPostagem(Usuario usuario, Postagem postagem) : this(usuario)
    {
        _ehEdicao = true;
        _postagem = postagem;
        Title = "Editar Postagem";
        TbConteudo.Text = postagem.Conteudo;
        BtnPostar.Content = "Salvar alterações";
    }

    private void TbConteudo_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        LblCaracteresMax.Content = $"{TbConteudo.Text.Length}/140";
    }

    private void BtnPostar_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TbConteudo.Text))
        {
            MessageBox.Show("Escreva algo no conteudo!");
            TbConteudo.Focus();
            return;
        }

        using var conexao = new MySqlConnection(App.StringConexao);


        string query;

        if (_ehEdicao)
        {
            query = "UPDATE postagens SET conteudo = @conteudo WHERE id = @postagem_id";
        }
        else
        {
            query = "INSERT INTO postagens (conteudo, usuario_id) VALUES (@conteudo, @usuario_id)";
        }

        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@conteudo", TbConteudo.Text);

        if (_ehEdicao)
        {
            comando.Parameters.AddWithValue("@postagem_id", _postagem!.Id);
        }
        else
        {
            comando.Parameters.AddWithValue("@usuario_id", _usuario.Id);
        }
        
        try
        {
            conexao.Open();
            var linhasAfetadas = comando.ExecuteNonQuery();
            if (linhasAfetadas < 1) throw new Exception("Erro ao postar conteudo!");
            MessageBox.Show("Postagem realizada com sucesso");
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
        finally
        {
            conexao.Close();
            Close();
        }
    }
}