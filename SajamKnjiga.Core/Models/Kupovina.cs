using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SajamKnjiga.Core.Models;

public class Kupovina
{
    public int Id { get; set; }
    public DateOnly DatumKupovine { get; set; }
    public int Ocena { get; set; }
    public string Komentar { get; set; } = string.Empty;

    public int PosetilacId { get; set; }
    public int KnjigaId { get; set; }
    public Posetilac? Posetilac { get; set; }
    public Knjiga? Knjiga { get; set; }

    public Kupovina()
    {
    }

    public Kupovina(int posetilacId, int knjigaId, DateOnly datumKupovine, int ocena, string komentar)
    {
        PosetilacId = posetilacId;
        KnjigaId = knjigaId;
        DatumKupovine = datumKupovine;
        Ocena = ocena;
        Komentar = komentar;
    }

    public override string ToString()
    {
        string posetilac = Posetilac is null ? "-" : $"{Posetilac.Ime} {Posetilac.Prezime}";
        string knjiga = Knjiga is null ? "-" : Knjiga.Naziv;
        return $"{posetilac,-25} | {knjiga,-25} | {DatumKupovine:dd.MM.yyyy.} | {Ocena,5} | {Komentar}";
    }
}
