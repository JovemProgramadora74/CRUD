using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CRUD.Modelos;

namespace CRUD.Utils;

public class FeedViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Postagem> Postagens { get; set; }

    public FeedViewModel()
    {
        Postagens = [];
        CarregarDadosIniciais();
    }

    private void CarregarDadosIniciais()
    {
        // Simulação de dados vindos de um banco de dados ou API
        var usuarioExemplo = new Usuario 
        { 
            Id = 1, 
            Nome = "Felipe Silva", 
            Username = "felipesilva" 
        };

        Postagens.Add(new Postagem
        {
            Id = 1,
            Conteudo = "Criando meu primeiro DataContext para um ItemsControl! 🚀",
            Curtidas = 12,
            FoiCurtido = false,
            PostadoEm = DateTime.Now.AddMinutes(-30),
            Usuario = usuarioExemplo
        });

        Postagens.Add(new Postagem
        {
            Id = 2,
            Conteudo = "C# e XAML formam uma excelente combinação para padrões MVVM.",
            Curtidas = 45,
            FoiCurtido = true,
            PostadoEm = DateTime.Now.AddHours(-2),
            Usuario = usuarioExemplo
        });
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string nomePropriedade = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
    }
}