using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRUD.Modelos;

public class Postagem : INotifyPropertyChanged
{
    public int Id { get; set; }
    public string Conteudo { get; set; }
    public int Curtidas { get; set; }
    public DateTime Postado_em { get; set; }
    public Usuario Usuario { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    private bool _foiCurtido;

    public bool FoiCurtido
    {
        get => _foiCurtido;
        set
        {
            if (_foiCurtido != value)
            {
                _foiCurtido = value;
                NotifyPropertyChanged();
            }
        }
    }

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}