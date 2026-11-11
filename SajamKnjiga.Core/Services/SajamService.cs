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



    // ---------- Brisanje uz referencijalne zavisnosti ----------

    public bool AdresaSeKoristi(int adresaId)
    {
        return _posetilacDao.GetAll().Exists(p => p.AdresaId == adresaId)
            || _autorDao.GetAll().Exists(a => a.AdresaId == adresaId);
    }

    // Adresa se ne moze obrisati dok je koristi neki posjetilac ili autor
    public bool ObrisiAdresu(int adresaId)
    {
        if (AdresaSeKoristi(adresaId)) return false;

        bool obrisana = _adresaDao.Remove(adresaId) is not null;
        PoveziReference();
        return obrisana;
    }

    // Brise posjetioca, njegove kupovine, listu zelja i adresu (ako je niko drugi ne koristi)
    public bool ObrisiPosetioca(int posetilacId)
    {
        Posetilac? posetilac = _posetilacDao.Remove(posetilacId);
        if (posetilac is null) return false;

        foreach (Kupovina k in posetilac.Kupovine)
        {
            _kupovinaDao.Remove(k.Id);
        }
        _listaZeljaDao.RemoveByPosetilac(posetilacId);
        if (!AdresaSeKoristi(posetilac.AdresaId))
        {
            _adresaDao.Remove(posetilac.AdresaId);
        }

        PoveziReference();
        return true;
    }

    // Brise autora, njegove veze sa knjigama i adresu (ako je niko drugi ne koristi).
    // Izdavaci kojima je bio sef ostaju bez sefa.
    public bool ObrisiAutora(int autorId)
    {
        Autor? autor = _autorDao.Remove(autorId);
        if (autor is null) return false;

        _knjigaAutorDao.RemoveByAutor(autorId);
        foreach (Izdavac i in _izdavacDao.GetAll())
        {
            if (i.SefId == autorId)
            {
                i.SefId = null;
                _izdavacDao.Update(i);
            }
        }
        if (!AdresaSeKoristi(autor.AdresaId))
        {
            _adresaDao.Remove(autor.AdresaId);
        }

        PoveziReference();
        return true;
    }

    // Brise izdavaca; njegove knjige ostaju bez izdavaca
    public bool ObrisiIzdavaca(int izdavacId)
    {
        Izdavac? izdavac = _izdavacDao.Remove(izdavacId);
        if (izdavac is null) return false;

        foreach (Knjiga k in _knjigaDao.GetAll())
        {
            if (k.IzdavacId == izdavacId)
            {
                k.IzdavacId = null;
                _knjigaDao.Update(k);
            }
        }

        PoveziReference();
        return true;
    }

    public bool KnjigaImaKupovine(int knjigaId)
    {
        return _kupovinaDao.GetAll().Exists(k => k.KnjigaId == knjigaId);
    }

    // Knjiga sa kupovinama se ne moze obrisati. Inace se brisu i njene veze sa autorima i listom zelja.
    public bool ObrisiKnjigu(int knjigaId)
    {
        if (KnjigaImaKupovine(knjigaId)) return false;

        Knjiga? knjiga = _knjigaDao.Remove(knjigaId);
        if (knjiga is null) return false;

        _knjigaAutorDao.RemoveByKnjiga(knjigaId);
        _listaZeljaDao.RemoveByKnjiga(knjigaId);

        PoveziReference();
        return true;
    }

    public bool ObrisiKupovinu(int kupovinaId)
    {
        bool obrisana = _kupovinaDao.Remove(kupovinaId) is not null;
        PoveziReference();
        return obrisana;
    }

}
