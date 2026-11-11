using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using SajamKnjiga.Core.Serialization;

namespace SajamKnjiga.Core.Models;

public class Knjiga : ISerializable
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

    // Popunjava se nakon ucitavanja kupovina
    public List<Kupovina> Kupovine { get; set; } = new();

    // Racuna se iz kupovina, ne cuva se
    public List<Posetilac> Kupci
    {
        get { return Kupovine.Select(k => k.Posetilac).OfType<Posetilac>().ToList(); }
    }


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

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(),
            Isbn,
            Naziv,
            Zanr,
            GodinaIzdanja.ToString(),
            Cena.ToString(CultureInfo.InvariantCulture),        // zbog razlicitosti formata ispisivanja dec broja (,.)
            BrojStrana.ToString(),
            IzdavacId?.ToString() ?? ""
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Isbn = values[1];
        Naziv = values[2];
        Zanr = values[3];
        GodinaIzdanja = int.Parse(values[4]);
        Cena = double.Parse(values[5], CultureInfo.InvariantCulture);
        BrojStrana = int.Parse(values[6]);
        IzdavacId = values[7] == "" ? null : int.Parse(values[7]);
    }
}
