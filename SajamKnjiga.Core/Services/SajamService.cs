using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.DAO;
using SajamKnjiga.Core.Models;

namespace SajamKnjiga.Core.Services;

// povezuje entitete i vodi racuna o pravilima.
// konzola -> service -> dao.
public class SajamService
{
    public const int MinGodinaIskustvaZaSefa = 5;

    private readonly AdresaDao _adresaDao = new();
    private readonly PosetilacDao _posetilacDao = new();
    private readonly AutorDao _autorDao = new();
    private readonly IzdavacDao _izdavacDao = new();
    private readonly KnjigaDao _knjigaDao = new();
    private readonly KupovinaDao _kupovinaDao = new();
    private readonly KnjigaAutorDao _knjigaAutorDao = new();
    private readonly ListaZeljaDao _listaZeljaDao = new();

    public SajamService()
    {
        PoveziReference();
    }

  
    private void PoveziReference()
    {
        foreach (Posetilac p in _posetilacDao.GetAll())
        {
            p.Adresa = _adresaDao.GetById(p.AdresaId);
            p.Kupovine.Clear();
            p.ListaZelja.Clear();
        }

        foreach (Autor a in _autorDao.GetAll())
        {
            a.Adresa = _adresaDao.GetById(a.AdresaId);
            a.Knjige.Clear();
        }

        foreach (Izdavac i in _izdavacDao.GetAll())
        {
            i.Sef = i.SefId is null ? null : _autorDao.GetById(i.SefId.Value);
            i.Knjige.Clear();
        }

        foreach (Knjiga k in _knjigaDao.GetAll())
        {
            k.Izdavac = k.IzdavacId is null ? null : _izdavacDao.GetById(k.IzdavacId.Value);
            k.Izdavac?.Knjige.Add(k);
            k.Autori.Clear();
            k.ZeljeliPosetioci.Clear();
            k.Kupovine.Clear();
        }

        foreach (KnjigaAutor veza in _knjigaAutorDao.GetAll())
        {
            Knjiga? k = _knjigaDao.GetById(veza.KnjigaId);
            Autor? a = _autorDao.GetById(veza.AutorId);
            if (k is null || a is null) continue;

            k.Autori.Add(a);
            a.Knjige.Add(k);
        }

        foreach (ListaZeljaStavka stavka in _listaZeljaDao.GetAll())
        {
            Posetilac? p = _posetilacDao.GetById(stavka.PosetilacId);
            Knjiga? k = _knjigaDao.GetById(stavka.KnjigaId);
            if (p is null || k is null) continue;

            p.ListaZelja.Add(k);
            k.ZeljeliPosetioci.Add(p);
        }

        foreach (Kupovina kupovina in _kupovinaDao.GetAll())
        {
            kupovina.Posetilac = _posetilacDao.GetById(kupovina.PosetilacId);
            kupovina.Knjiga = _knjigaDao.GetById(kupovina.KnjigaId);
            kupovina.Posetilac?.Kupovine.Add(kupovina);
            kupovina.Knjiga?.Kupovine.Add(kupovina);
        }
    }

    // ---------- Adrese ----------

    public List<Adresa> GetAdrese() { return _adresaDao.GetAll(); }

    public Adresa DodajAdresu(Adresa adresa)
    {
        Adresa nova = _adresaDao.Add(adresa);
        PoveziReference();
        return nova;
    }

    public Adresa? IzmeniAdresu(Adresa adresa)
    {
        Adresa? izmenjena = _adresaDao.Update(adresa);
        PoveziReference();
        return izmenjena;
    }

    // ---------- Posjetioci ----------

    public List<Posetilac> GetPosetioci() { return _posetilacDao.GetAll(); }

    public Posetilac DodajPosetioca(Posetilac posetilac)
    {
        Posetilac novi = _posetilacDao.Add(posetilac);
        PoveziReference();
        return novi;
    }

    public Posetilac? IzmeniPosetioca(Posetilac posetilac)
    {
        Posetilac? izmenjen = _posetilacDao.Update(posetilac);
        PoveziReference();
        return izmenjen;
    }

    // ---------- Autori ----------

    public List<Autor> GetAutori() { return _autorDao.GetAll(); }

    public Autor DodajAutora(Autor autor)
    {
        Autor novi = _autorDao.Add(autor);
        PoveziReference();
        return novi;
    }

    public Autor? IzmeniAutora(Autor autor)
    {
        Autor? izmenjen = _autorDao.Update(autor);
        PoveziReference();
        return izmenjen;
    }

    // ---------- Izdavaci ----------

    public List<Izdavac> GetIzdavaci() { return _izdavacDao.GetAll(); }

    // Sef izdavaca mora biti postojeci autor sa najmanje 5 godina iskustva
    public bool MozeBitiSef(int autorId)
    {
        Autor? autor = _autorDao.GetById(autorId);
        return autor is not null && autor.GodineIskustva >= MinGodinaIskustvaZaSefa;
    }

    public Izdavac? DodajIzdavaca(Izdavac izdavac)
    {
        if (izdavac.SefId is not null && !MozeBitiSef(izdavac.SefId.Value)) return null;

        Izdavac novi = _izdavacDao.Add(izdavac);
        PoveziReference();
        return novi;
    }

    public Izdavac? IzmeniIzdavaca(Izdavac izdavac)
    {
        if (izdavac.SefId is not null && !MozeBitiSef(izdavac.SefId.Value)) return null;

        Izdavac? izmenjen = _izdavacDao.Update(izdavac);
        PoveziReference();
        return izmenjen;
    }

    // ---------- Knjige ----------

    public List<Knjiga> GetKnjige() { return _knjigaDao.GetAll(); }

    public Knjiga DodajKnjigu(Knjiga knjiga)
    {
        Knjiga nova = _knjigaDao.Add(knjiga);
        PoveziReference();
        return nova;
    }

    public Knjiga? IzmeniKnjigu(Knjiga knjiga)
    {
        Knjiga? izmenjena = _knjigaDao.Update(knjiga);
        PoveziReference();
        return izmenjena;
    }

    public bool DodajAutoraKnjizi(int knjigaId, int autorId)
    {
        if (_knjigaDao.GetById(knjigaId) is null || _autorDao.GetById(autorId) is null) return false;

        bool dodato = _knjigaAutorDao.Add(knjigaId, autorId);
        PoveziReference();
        return dodato;
    }

    public bool UkloniAutoraSaKnjige(int knjigaId, int autorId)
    {
        bool uklonjeno = _knjigaAutorDao.Remove(knjigaId, autorId);
        PoveziReference();
        return uklonjeno;
    }

    // ---------- Lista zelja ----------

    public bool DodajNaListuZelja(int posetilacId, int knjigaId)
    {
        if (_posetilacDao.GetById(posetilacId) is null || _knjigaDao.GetById(knjigaId) is null) return false;

        bool dodato = _listaZeljaDao.Add(posetilacId, knjigaId);
        PoveziReference();
        return dodato;
    }

    public bool UkloniSaListeZelja(int posetilacId, int knjigaId)
    {
        bool uklonjeno = _listaZeljaDao.Remove(posetilacId, knjigaId);
        PoveziReference();
        return uklonjeno;
    }

    // ---------- Kupovine ----------

    public List<Kupovina> GetKupovine() { return _kupovinaDao.GetAll(); }

    public Kupovina? DodajKupovinu(Kupovina kupovina)
    {
        if (_posetilacDao.GetById(kupovina.PosetilacId) is null || _knjigaDao.GetById(kupovina.KnjigaId) is null) return null;

        Kupovina nova = _kupovinaDao.Add(kupovina);
        PoveziReference();
        return nova;
    }

    public Kupovina? IzmeniKupovinu(Kupovina kupovina)
    {
        Kupovina? izmenjena = _kupovinaDao.Update(kupovina);
        PoveziReference();
        return izmenjena;
    }

}
