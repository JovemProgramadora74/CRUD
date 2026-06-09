namespace CRUD.Modelos;

public class Postagem
{
    public int Id { get; set; }
    public string Conteudo { get; set; }
    public int Curtidas { get; set; }
    public DateTime Postado_em { get; set; }
    public Usuario Usuario { get; set; }
}