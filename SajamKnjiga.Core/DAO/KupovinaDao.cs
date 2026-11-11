using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Storage;

namespace SajamKnjiga.Core.DAO;
public class KupovinaDao
{
    private readonly List<Kupovina> _kupovine;
    private readonly Storage<Kupovina> _storage;

    public KupovinaDao()
    {
        _storage = new Storage<Kupovina>("kupovine.csv");
        _kupovine = _storage.Load();
    }

    private int GenerateId()
    {
        if (_kupovine.Count == 0) return 0;
        return _kupovine.Max(k => k.Id) + 1;
    }

    public Kupovina Add(Kupovina nov)
    {
        nov.Id = GenerateId();
        _kupovine.Add(nov);
        _storage.Save(_kupovine);
        return nov;
    }

    public Kupovina? Update(Kupovina izmenjen)
    {
        Kupovina? stari = GetById(izmenjen.Id);
        if (stari is null) return null;

        stari.PosetilacId = izmenjen.PosetilacId;
        stari.KnjigaId = izmenjen.KnjigaId;
        stari.DatumKupovine = izmenjen.DatumKupovine;
        stari.Ocena = izmenjen.Ocena;
        stari.Komentar = izmenjen.Komentar;

        _storage.Save(_kupovine);
        return stari;
    }

    public Kupovina? Remove(int id)
    {
        Kupovina? k = GetById(id);
        if (k is null) return null;

        _kupovine.Remove(k);
        _storage.Save(_kupovine);
        return k;
    }

    public Kupovina? GetById(int id)
    {
        return _kupovine.Find(k => k.Id == id);
    }

    public List<Kupovina> GetAll()
    {
        return _kupovine;
    }
}
