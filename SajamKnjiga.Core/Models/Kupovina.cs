using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using SajamKnjiga.Core.Serialization;

namespace SajamKnjiga.Core.Models;

// ToCSV/FromCSV po uzoru na Vehicle iz CRUDExample (vjezbe 3)
public class Kupovina : ISerializable
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

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(),
            PosetilacId.ToString(),
            KnjigaId.ToString(),
            DatumKupovine.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            Ocena.ToString(),
            Komentar
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        PosetilacId = int.Parse(values[1]);
        KnjigaId = int.Parse(values[2]);
        DatumKupovine = DateOnly.ParseExact(values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
        Ocena = int.Parse(values[4]);
        Komentar = values[5];
    }
}
