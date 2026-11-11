using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Storage;

namespace SajamKnjiga.Core.DAO;
public class AutorDao
{
    private readonly List<Autor> _autori;
    private readonly Storage<Autor> _storage;

    public AutorDao()
    {
        _storage = new Storage<Autor>("autori.csv");
        _autori = _storage.Load();
    }

    private int GenerateId()
    {
        if (_autori.Count == 0) return 0;
        return _autori.Max(a => a.Id) + 1;
    }

    public Autor Add(Autor nov)
    {
        nov.Id = GenerateId();
        _autori.Add(nov);
        _storage.Save(_autori);
        return nov;
    }

    public Autor? Update(Autor izmenjen)
    {
        Autor? stari = GetById(izmenjen.Id);
        if (stari is null) return null;

        stari.Ime = izmenjen.Ime;
        stari.Prezime = izmenjen.Prezime;
        stari.DatumRodjenja = izmenjen.DatumRodjenja;
        stari.Telefon = izmenjen.Telefon;
        stari.Email = izmenjen.Email;
        stari.BrojLicneKarte = izmenjen.BrojLicneKarte;
        stari.GodineIskustva = izmenjen.GodineIskustva;
        stari.AdresaId = izmenjen.AdresaId;

        _storage.Save(_autori);
        return stari;
    }

    public Autor? Remove(int id)
    {
        Autor? a = GetById(id);
        if (a is null) return null;

        _autori.Remove(a);
        _storage.Save(_autori);
        return a;
    }

    public Autor? GetById(int id)
    {
        return _autori.Find(a => a.Id == id);
    }

    public List<Autor> GetAll()
    {
        return _autori;
    }
}
