using System.Formats.Tar;
using System.Windows;
using System.Windows.Controls;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class Feed : Window
{
    private Usuario _usuario;
    
    public Feed(Usuario usuario)
    {
        _usuario = usuario;
        InitializeComponent();
        CarregarPosts_QuandoIniciar();
    }

    private void CarregarPosts_QuandoIniciar()
    {
        List<Postagem> listaPostagens = [];

        const string query =
            "SELECT p.id, p.conteudo, p.curtidas, p.postado_em, u.nome, u.username, IF(cp.usuario_id IS NOT NULL, TRUE, FALSE) AS curtido FROM postagens p INNER JOIN usuarios u ON p.usuario_id = u.id LEFT JOIN curtidas_postagens cp ON cp.postagem_id = p.id AND cp.usuario_id = @usuario_id ORDER BY p.postado_em DESC";
        using var conexao = new MySqlConnection(App.StringConexao);

        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@usuario_id", _usuario.Id);

        try
        {
            conexao.Open();
            var leitor = comando.ExecuteReader();
            if (!leitor.HasRows)
            {
                MessageBox.Show("Nenhuma postagem foi encontrada!");
                return;
            }

            while (leitor.Read())
            {
                var post = new Postagem
                {
                    Id = leitor.GetInt32("id"),
                    Conteudo = leitor.GetString("conteudo"),
                    Curtidas = leitor.GetInt32("curtidas"),
                    Postado_em = leitor.GetDateTime("postado_em"),
                    FoiCurtido =  leitor.GetBoolean("curtido"),
                    Usuario = new Usuario
                    {
                        Nome = leitor.GetString("nome"),
                        Username = leitor.GetString("username")
                    }
                };
                
                listaPostagens.Add(post);
            }

            ItemsControlFeed.ItemsSource = listaPostagens;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void BtnCurtir_OnClick(object sender, RoutedEventArgs e)
    {
        var botao = (Button)sender;
        var postagem = (Postagem)botao.Tag;

        var query = "SELECT 1 FROM curtidas_postagens WHERE usuario_id = @usuario AND postagem_id = @postagem";
        
        using var conexao = new MySqlConnection(App.StringConexao);
        
        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@usuario", _usuario.Id);
        comando.Parameters.AddWithValue("@postagem", postagem.Id);
        
        try
        {
            conexao.Open();
            var leitor = comando.ExecuteReader();
            string acao;
            
            if (leitor.HasRows)
            {
                query = "DELETE FROM curtidas_postagens WHERE usuario_id = @usuario AND postagem_id = @postagem";
                acao = "descurtir";
                postagem.FoiCurtido = false;
                postagem.Curtidas--;
            }
            else
            {
                query = "INSERT INTO curtidas_postagens (usuario_id, postagem_id) VALUES (@usuario, @postagem)";
                acao = "curtir";
                postagem.FoiCurtido = true;
                postagem.Curtidas++;
            }

            conexao.Close();
            comando.CommandText = query;
            conexao.Open();
            var linhasAfetadas = comando.ExecuteNonQuery();
            if (linhasAfetadas == 0) throw new Exception($"Erro ao {acao} postagem!");
            
            
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}