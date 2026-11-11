using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SajamKnjiga.Core.Models;

public class Adresa
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

}
