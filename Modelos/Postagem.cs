using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRUD.Modelos;

public class Postagem : INotifyPropertyChanged
{
    private bool _foiCurtido;
    private int _curtidas;

    public int Id { get; set; }
    public string Conteudo { get; set; }

    public int Curtidas
    {
        get => _curtidas;
        set
        {
            _curtidas = value;
            NotifyPropertyChanged();
        }
    }
    public DateTime Postado_em { get; set; }
    public Usuario Usuario { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

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