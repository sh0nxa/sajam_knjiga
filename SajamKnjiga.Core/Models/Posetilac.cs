using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using SajamKnjiga.Core.Serialization;

namespace SajamKnjiga.Core.Models;

public class Posetilac : ISerializable
{
    public int Id { get; set; }
    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public DateOnly DatumRodjenja { get; set; } // DateOnly jer cuva samo datum, bezz vremena
    public string KontaktTelefon { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string BrojClanskeKarte {  get; set; } = string.Empty;
    public int TrenutnaGodinaClanstva {  get; set; } // ne znam tacno da li je godina kao redni broj godine ili godina kao npr 2019
    public StatusPosetioca Status {  get; set; }

    // cuva se u fajlu
    public int AdresaId { get; set; }
    // popunjava se nakon ucitavanja ne cuva se u fajlu
    public Adresa? Adresa { get; set; }

    public List<Kupovina> Kupovine { get; set; } = new();
    public List<Knjiga> ListaZelja { get; set; } = new();

    // Racuna se iz kupovina, ne cuva se
    public double ProsecnaOcena
    {
        get
        {
            if (Kupovine.Count == 0) return 0;
            return Kupovine.Average(k => k.Ocena);
        }
    }

    public Posetilac()
    {
    }

    public Posetilac(string ime, string prezime, DateOnly datumRodjenja, string kontaktTelefon, string email, string brojClanskeKarte, int trenutnaGodinaClanstva,
        StatusPosetioca status, int adresaId)
    {
        Ime = ime;
        Prezime = prezime;
        DatumRodjenja = datumRodjenja;
        KontaktTelefon = kontaktTelefon;
        Email = email;
        BrojClanskeKarte = brojClanskeKarte;
        TrenutnaGodinaClanstva = trenutnaGodinaClanstva;
        Status = status;
        AdresaId = adresaId;
    }


    

    public override string ToString()
    {
        return $"{BrojClanskeKarte,-12} | {Ime,-12} | {Prezime,-15} | {Adresa?.ToString() ?? "",-35} | {Status,-8}";
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(),
            Ime,
            Prezime,
            DatumRodjenja.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), // zbog razlicitosti formata
            KontaktTelefon,
            Email,
            BrojClanskeKarte,
            TrenutnaGodinaClanstva.ToString(),
            Status.ToString(),
            AdresaId.ToString()
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Ime = values[1];
        Prezime = values[2];
        DatumRodjenja = DateOnly.ParseExact(values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
        KontaktTelefon = values[4];
        Email = values[5];
        BrojClanskeKarte = values[6];
        TrenutnaGodinaClanstva = int.Parse(values[7]);
        Status = Enum.Parse<StatusPosetioca>(values[8]);
        AdresaId = int.Parse(values[9]);
    }


}