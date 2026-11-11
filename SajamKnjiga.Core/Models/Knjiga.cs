using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SajamKnjiga.Core.Models;

public class Knjiga
{
    public int Id { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string Naziv { get; set; } = string.Empty;
    public string Zanr { get; set; } = string.Empty;
    public int GodinaIzdanja { get; set; }
    public double Cena { get; set; }
    public int BrojStrana { get; set; }

    // null ako knjiga nema izdavaca
    public int? IzdavacId { get; set; }
    public Izdavac? Izdavac { get; set; }

    // popunjavaju se nakon ucitavanja iz fajlova veza, ne cuvaju se u fajlu knjiga
    public List<Autor> Autori { get; set; } = new();
    public List<Posetilac> ZeljeliPosetioci { get; set; } = new();

    public Knjiga()
    {
    }

    public Knjiga(string isbn, string naziv, string zanr, int godinaIzdanja,
        double cena, int brojStrana, int? izdavacId)
    {
        Isbn = isbn;
        Naziv = naziv;
        Zanr = zanr;
        GodinaIzdanja = godinaIzdanja;
        Cena = cena;
        BrojStrana = brojStrana;
        IzdavacId = izdavacId;
    }

    public override string ToString()
    {
        return $"{Isbn,-18} | {Naziv,-25} | {Cena,10:F2} | {GodinaIzdanja,6} | {Zanr,-15}";
    }
}
