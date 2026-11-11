using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using SajamKnjiga.Core.Serialization;

namespace SajamKnjiga.Core.Models;

public class Autor : ISerializable
{
    public int Id { get; set; }
    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public DateOnly DatumRodjenja { get; set; }
    public string Telefon { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string BrojLicneKarte { get; set; } = string.Empty;
    public int GodineIskustva { get; set; }

    public int AdresaId { get; set; }
    public Adresa? Adresa { get; set; }

    // Popunjava se nakon ucitavanja iz fajla veza knjiga<->autor
    public List<Knjiga> Knjige { get; set; } = new();

    public Autor()
    {
    }

    public Autor(string ime, string prezime, DateOnly datumRodjenja, string telefon,
        string email, string brojLicneKarte, int godineIskustva, int adresaId)
    {
        Ime = ime;
        Prezime = prezime;
        DatumRodjenja = datumRodjenja;
        Telefon = telefon;
        Email = email;
        BrojLicneKarte = brojLicneKarte;
        GodineIskustva = godineIskustva;
        AdresaId = adresaId;
    }

    public override string ToString()
    {
        return $"{Ime,-12} | {Prezime,-15} | {BrojLicneKarte,-12} | {DatumRodjenja:dd.MM.yyyy.} | {Email,-25}";
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(),
            Ime,
            Prezime,
            DatumRodjenja.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            Telefon,
            Email,
            BrojLicneKarte,
            GodineIskustva.ToString(),
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
        Telefon = values[4];
        Email = values[5];
        BrojLicneKarte = values[6];
        GodineIskustva = int.Parse(values[7]);
        AdresaId = int.Parse(values[8]);
    }

}