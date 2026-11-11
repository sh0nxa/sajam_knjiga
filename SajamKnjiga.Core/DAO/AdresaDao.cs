using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Storage;

namespace SajamKnjiga.Core.DAO;

// Po uzoru na VehiclesDAO iz CRUDExample (vjezbe 3)
public class AdresaDao
{
    private readonly List<Adresa> _adrese;
    private readonly Storage<Adresa> _storage;

    public AdresaDao()
    {
        _storage = new Storage<Adresa>("adrese.csv");
        _adrese = _storage.Load();
    }

    private int GenerateId()
    {
        if (_adrese.Count == 0) return 0;
        return _adrese.Max(a => a.Id) + 1;
    }

    public Adresa Add(Adresa nov)
    {
        nov.Id = GenerateId();
        _adrese.Add(nov);
        _storage.Save(_adrese);
        return nov;
    }

    public Adresa? Update(Adresa izmenjen)
    {
        Adresa? stari = GetById(izmenjen.Id);
        if (stari is null) return null;

        stari.Ulica = izmenjen.Ulica;
        stari.Broj = izmenjen.Broj;
        stari.Grad = izmenjen.Grad;
        stari.Drzava = izmenjen.Drzava;

        _storage.Save(_adrese);
        return stari;
    }

    public Adresa? Remove(int id)
    {
        Adresa? a = GetById(id);
        if (a is null) return null;

        _adrese.Remove(a);
        _storage.Save(_adrese);
        return a;
    }

    public Adresa? GetById(int id)
    {
        return _adrese.Find(a => a.Id == id);
    }

    public List<Adresa> GetAll()
    {
        return _adrese;
    }
}
