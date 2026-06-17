using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRUD.Modelos;

public class Postagem : INotifyPropertyChanged
{
    private int _curtidas;
    private bool _foiCurtido;

    public int Id { get; set; }
    public string Conteudo { get; set; } = string.Empty;

    public int Curtidas
    {
        get => _curtidas;
        set
        {
            _curtidas = value;
            NotifyPropertyChanged();
        }
    }

    public DateTime PostadoEm { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public bool FoiCurtido
    {
        get => _foiCurtido;
        set
        {
            if (_foiCurtido == value) return;
            _foiCurtido = value;
            NotifyPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}