using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Storage;

namespace SajamKnjiga.Core.DAO;

// Ucitavanje i cuvanje preko Storage-a po uzoru na VehiclesDAO iz CRUDExample (vjezbe 3)
public class ListaZeljaDao
{
    private readonly List<ListaZeljaStavka> _stavke;
    private readonly Storage<ListaZeljaStavka> _storage;

    public ListaZeljaDao()
    {
        _storage = new Storage<ListaZeljaStavka>("lista_zelja.csv");
        _stavke = _storage.Load();
    }

    public bool Postoji(int posetilacId, int knjigaId)
    {
        return _stavke.Exists(s => s.PosetilacId == posetilacId && s.KnjigaId == knjigaId);
    }

    public bool Add(int posetilacId, int knjigaId)
    {
        if (Postoji(posetilacId, knjigaId)) return false;

        _stavke.Add(new ListaZeljaStavka(posetilacId, knjigaId));
        _storage.Save(_stavke);
        return true;
    }

    public bool Remove(int posetilacId, int knjigaId)
    {
        int obrisano = _stavke.RemoveAll(s => s.PosetilacId == posetilacId && s.KnjigaId == knjigaId);
        if (obrisano == 0) return false;

        _storage.Save(_stavke);
        return true;
    }

    // Koristi se kada se brise posetilac
    public void RemoveByPosetilac(int posetilacId)
    {
        _stavke.RemoveAll(s => s.PosetilacId == posetilacId);
        _storage.Save(_stavke);
    }

    // Koristi se kada se brise knjiga
    public void RemoveByKnjiga(int knjigaId)
    {
        _stavke.RemoveAll(s => s.KnjigaId == knjigaId);
        _storage.Save(_stavke);
    }

    public List<ListaZeljaStavka> GetAll()
    {
        return _stavke;
    }
}