using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SajamKnjiga.Core.Models;

public class Izdavac
{
    public int Id { get; set; }
    public string Sifra { get; set; } = string.Empty;
    public string Naziv { get; set; } = string.Empty;

    // null ako izdavac jos nema sefa
    public int? SefId { get; set; }
    public Autor? Sef { get; set; }

    // Popunjava se nakon ucitavanja knjiga
    public List<Knjiga> Knjige { get; set; } = new();

    // Racuna se iz knjiga, ne cuva se
    public List<Autor> Autori
    {
        get { return Knjige.SelectMany(k => k.Autori).Distinct().ToList(); }
    }

    public Izdavac()
    {
    }

    public Izdavac(string sifra, string naziv, int? sefId)
    {
        Sifra = sifra;
        Naziv = naziv;
        SefId = sefId;
    }

    public override string ToString()
    {
        string sef = Sef is null ? "-" : $"{Sef.Ime} {Sef.Prezime}";
        return $"{Sifra,-10} | {Naziv,-25} | {sef,-25}";
    }
}
