using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using SajamKnjiga.Core.Serialization;


namespace SajamKnjiga.Core.Models;

public class Adresa : ISerializable
{
    public int Id { get; set; }
    public string Ulica { get; set; } = string.Empty;
    public string Broj { get; set; } = string.Empty;
    public string Grad { get; set; } = string.Empty;
    public string Drzava { get; set; } = string.Empty;

    public Adresa() { }

    public Adresa(string ulica, string broj, string grad, string drzava)
    {
        Ulica = ulica;
        Broj = broj;
        Grad = grad;
        Drzava = drzava;
    }

    public override string ToString()
    {
        return $"{Ulica} {Broj}, {Grad}, {Drzava}";
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(),
            Ulica,
            Broj,
            Grad,
            Drzava
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Ulica = values[1];
        Broj = values[2];
        Grad = values[3];
        Drzava = values[4];
    }

}
