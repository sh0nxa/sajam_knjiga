using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Storage;

namespace SajamKnjiga.Core.DAO;
public class KnjigaDao
{
    private readonly List<Knjiga> _knjige;
    private readonly Storage<Knjiga> _storage;

    public KnjigaDao()
    {
        _storage = new Storage<Knjiga>("knjige.csv");
        _knjige = _storage.Load();
    }

    private int GenerateId()
    {
        if (_knjige.Count == 0) return 0;
        return _knjige.Max(k => k.Id) + 1;
    }

    public Knjiga Add(Knjiga nov)
    {
        nov.Id = GenerateId();
        _knjige.Add(nov);
        _storage.Save(_knjige);
        return nov;
    }

    public Knjiga? Update(Knjiga izmenjen)
    {
        Knjiga? stari = GetById(izmenjen.Id);
        if (stari is null) return null;

        stari.Isbn = izmenjen.Isbn;
        stari.Naziv = izmenjen.Naziv;
        stari.Zanr = izmenjen.Zanr;
        stari.GodinaIzdanja = izmenjen.GodinaIzdanja;
        stari.Cena = izmenjen.Cena;
        stari.BrojStrana = izmenjen.BrojStrana;
        stari.IzdavacId = izmenjen.IzdavacId;

        _storage.Save(_knjige);
        return stari;
    }

    public Knjiga? Remove(int id)
    {
        Knjiga? k = GetById(id);
        if (k is null) return null;

        _knjige.Remove(k);
        _storage.Save(_knjige);
        return k;
    }

    public Knjiga? GetById(int id)
    {
        return _knjige.Find(k => k.Id == id);
    }

    public List<Knjiga> GetAll()
    {
        return _knjige;
    }
}
