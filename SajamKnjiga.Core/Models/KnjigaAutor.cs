using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Serialization;

namespace SajamKnjiga.Core.Models;

// vise-na-vise izmedju knjige i autora (jedan red u knjiga_autor.csv)
public class KnjigaAutor : ISerializable
{
    public int KnjigaId { get; set; }
    public int AutorId { get; set; }

    public KnjigaAutor()
    {
    }

    public KnjigaAutor(int knjigaId, int autorId)
    {
        KnjigaId = knjigaId;
        AutorId = autorId;
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            KnjigaId.ToString(),
            AutorId.ToString()
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        KnjigaId = int.Parse(values[0]);
        AutorId = int.Parse(values[1]);
    }
}