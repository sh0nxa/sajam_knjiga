using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Storage;

namespace SajamKnjiga.Core.DAO;

// Cuva veze knjiga-autor (knjiga_autor.csv). Veza nema sta da mijenja, pa nema update.
public class KnjigaAutorDao
{
    private readonly List<KnjigaAutor> _veze;
    private readonly Storage<KnjigaAutor> _storage;

    public KnjigaAutorDao()
    {
        _storage = new Storage<KnjigaAutor>("knjiga_autor.csv");
        _veze = _storage.Load();
    }

    public bool Postoji(int knjigaId, int autorId)
    {
        return _veze.Exists(v => v.KnjigaId == knjigaId && v.AutorId == autorId);
    }

    public bool Add(int knjigaId, int autorId)
    {
        if (Postoji(knjigaId, autorId)) return false;

        _veze.Add(new KnjigaAutor(knjigaId, autorId));
        _storage.Save(_veze);
        return true;
    }

    public bool Remove(int knjigaId, int autorId)
    {
        int obrisano = _veze.RemoveAll(v => v.KnjigaId == knjigaId && v.AutorId == autorId);
        if (obrisano == 0) return false;

        _storage.Save(_veze);
        return true;
    }

    // Koristi se kada se brise knjiga
    public void RemoveByKnjiga(int knjigaId)
    {
        _veze.RemoveAll(v => v.KnjigaId == knjigaId);
        _storage.Save(_veze);
    }

    // Koristi se kada se brise autor
    public void RemoveByAutor(int autorId)
    {
        _veze.RemoveAll(v => v.AutorId == autorId);
        _storage.Save(_veze);
    }

    public List<KnjigaAutor> GetAll()
    {
        return _veze;
    }
}