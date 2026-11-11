using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Storage;

namespace SajamKnjiga.Core.DAO;
public class IzdavacDao
{
    private readonly List<Izdavac> _izdavaci;
    private readonly Storage<Izdavac> _storage;

    public IzdavacDao()
    {
        _storage = new Storage<Izdavac>("izdavaci.csv");
        _izdavaci = _storage.Load();
    }

    private int GenerateId()
    {
        if (_izdavaci.Count == 0) return 0;
        return _izdavaci.Max(i => i.Id) + 1;
    }

    public Izdavac Add(Izdavac nov)
    {
        nov.Id = GenerateId();
        _izdavaci.Add(nov);
        _storage.Save(_izdavaci);
        return nov;
    }

    public Izdavac? Update(Izdavac izmenjen)
    {
        Izdavac? stari = GetById(izmenjen.Id);
        if (stari is null) return null;

        stari.Sifra = izmenjen.Sifra;
        stari.Naziv = izmenjen.Naziv;
        stari.SefId = izmenjen.SefId;

        _storage.Save(_izdavaci);
        return stari;
    }

    public Izdavac? Remove(int id)
    {
        Izdavac? i = GetById(id);
        if (i is null) return null;

        _izdavaci.Remove(i);
        _storage.Save(_izdavaci);
        return i;
    }

    public Izdavac? GetById(int id)
    {
        return _izdavaci.Find(i => i.Id == id);
    }

    public List<Izdavac> GetAll()
    {
        return _izdavaci;
    }
}
