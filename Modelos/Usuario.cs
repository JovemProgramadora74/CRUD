namespace CRUD.Modelos;

public class Usuario
{
    public string Email = string.Empty;
    public int Id;
    public string Senha = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}