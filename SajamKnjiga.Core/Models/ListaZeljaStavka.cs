using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Serialization;

namespace SajamKnjiga.Core.Models;

// vise-na-vise izmedju posjetioca i knjige na njegovoj listi zelja (jedan red u lista_zelja.csv)
// ToCSV/FromCSV po uzoru na Vehicle iz CRUDExample (vjezbe 3)
public class ListaZeljaStavka : ISerializable
{
    public int PosetilacId { get; set; }
    public int KnjigaId { get; set; }

    public ListaZeljaStavka()
    {
    }

    public ListaZeljaStavka(int posetilacId, int knjigaId)
    {
        PosetilacId = posetilacId;
        KnjigaId = knjigaId;
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            PosetilacId.ToString(),
            KnjigaId.ToString()
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        PosetilacId = int.Parse(values[0]);
        KnjigaId = int.Parse(values[1]);
    }
}
