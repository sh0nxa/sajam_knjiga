using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SajamKnjiga.Core.Models;

public class Autor
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
}