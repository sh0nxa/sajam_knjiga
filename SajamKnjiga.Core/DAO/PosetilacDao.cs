using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Storage;

namespace SajamKnjiga.Core.DAO;
// Po uzoru na VehiclesDAO iz CRUDExample (vjezbe 3), GenerateId izmijenjen da koristi Max
public class PosetilacDao
{
    private readonly List<Posetilac> _posetioci;
    private readonly Storage<Posetilac> _storage;

    public PosetilacDao()
    {
        _storage = new Storage<Posetilac>("posetioci.csv");
        _posetioci = _storage.Load();
    }

    private int GenerateId()
    {
        if (_posetioci.Count == 0) return 0;
        return _posetioci.Max(p => p.Id) + 1;
    }

    public Posetilac Add(Posetilac nov)
    {
        nov.Id = GenerateId();
        _posetioci.Add(nov);
        _storage.Save(_posetioci);
        return nov;
    }

    public Posetilac? Update(Posetilac izmenjen)
    {
        Posetilac? stari = GetById(izmenjen.Id);
        if (stari is null) return null;

        stari.Ime = izmenjen.Ime;
        stari.Prezime = izmenjen.Prezime;
        stari.DatumRodjenja = izmenjen.DatumRodjenja;
        stari.KontaktTelefon = izmenjen.KontaktTelefon;
        stari.Email = izmenjen.Email;
        stari.BrojClanskeKarte = izmenjen.BrojClanskeKarte;
        stari.TrenutnaGodinaClanstva = izmenjen.TrenutnaGodinaClanstva;
        stari.Status = izmenjen.Status;
        stari.AdresaId = izmenjen.AdresaId;

        _storage.Save(_posetioci);
        return stari;
    }

    public Posetilac? Remove(int id)
    {
        Posetilac? p = GetById(id);
        if (p is null) return null;

        _posetioci.Remove(p);
        _storage.Save(_posetioci);
        return p;
    }

    public Posetilac? GetById(int id)
    {
        return _posetioci.Find(p => p.Id == id);
    }

    public List<Posetilac> GetAll()
    {
        return _posetioci;
    }
}
