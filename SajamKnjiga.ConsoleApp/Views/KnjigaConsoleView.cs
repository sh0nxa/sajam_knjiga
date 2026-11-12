using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Services;

namespace SajamKnjiga.ConsoleApp.Views;

class KnjigaConsoleView
{
    private readonly SajamService _service;

    public KnjigaConsoleView(SajamService service)
    {
        _service = service;
    }

    public void RunMenu()
    {
        while (true)
        {
            ShowMenu();
            string userInput = Console.ReadLine() ?? "0";
            if (userInput == "0") break;
            HandleMenuInput(userInput);
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("----- KNJIGE -----");
        Console.WriteLine("1: Prikazi sve knjige");
        Console.WriteLine("2: Dodaj knjigu");
        Console.WriteLine("3: Izmeni knjigu");
        Console.WriteLine("4: Obrisi knjigu");
        Console.WriteLine("0: Nazad");
        Console.Write("Izaberite opciju: ");
    }

    private void HandleMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                PrintKnjige(_service.GetKnjige());
                break;
            case "2":
                AddKnjiga();
                break;
            case "3":
                UpdateKnjiga();
                break;
            case "4":
                RemoveKnjiga();
                break;
            default:
                Console.WriteLine("Nepostojeca opcija.");
                break;
        }
    }

    private void PrintKnjige(List<Knjiga> knjige)
    {
        if (knjige.Count == 0)
        {
            Console.WriteLine("Nema knjiga.");
            return;
        }

        Console.WriteLine($"{"ID",4} | {"ISBN",-18} | {"Naziv",-25} | {"Cena",10} | {"Godina",6} | {"Zanr",-15} | {"Izdavac",-20} | Autori");
        foreach (Knjiga k in knjige)
        {
            string izdavac = k.Izdavac?.Naziv ?? "-";
            string autori = k.Autori.Count == 0 ? "-" : string.Join(", ", k.Autori.Select(a => $"{a.Ime} {a.Prezime}"));
            Console.WriteLine($"{k.Id,4} | {k} | {izdavac,-20} | {autori}");
        }
    }

    private Knjiga InputKnjiga(Knjiga? trenutna = null)
    {
        string isbn = ConsoleViewUtils.InputText("ISBN", trenutna?.Isbn);
        string naziv = ConsoleViewUtils.InputText("Naziv", trenutna?.Naziv);
        string zanr = ConsoleViewUtils.InputText("Zanr", trenutna?.Zanr);
        int godina = ConsoleViewUtils.InputInt("Godina izdanja", trenutna?.GodinaIzdanja, 1, DateTime.Now.Year);
        double cena = ConsoleViewUtils.InputDouble("Cena", trenutna?.Cena);
        int brojStrana = ConsoleViewUtils.InputInt("Broj strana", trenutna?.BrojStrana, 1);
        int? izdavacId = InputIzdavacId(trenutna?.IzdavacId);

        return new Knjiga(isbn, naziv, zanr, godina, cena, brojStrana, izdavacId);
    }

    private int? InputIzdavacId(int? trenutni)
    {
        List<Izdavac> izdavaci = _service.GetIzdavaci();
        Console.WriteLine("Izdavaci: " + (izdavaci.Count == 0 ? "nema" : string.Join(", ", izdavaci.Select(i => $"{i.Id} - {i.Naziv}"))));

        while (true)
        {
            int? id = ConsoleViewUtils.InputOptionalId("ID izdavaca", trenutni);
            if (id is null || izdavaci.Exists(i => i.Id == id)) return id;

            Console.WriteLine("Izdavac sa tim ID-jem ne postoji.");
        }
    }

    private List<int> InputAutorIds(List<int>? trenutni)
    {
        List<Autor> autori = _service.GetAutori();
        Console.WriteLine("Autori: " + (autori.Count == 0 ? "nema" : string.Join(", ", autori.Select(a => $"{a.Id} - {a.Ime} {a.Prezime}"))));

        while (true)
        {
            List<int> ids = ConsoleViewUtils.InputIdList("ID-jevi autora", trenutni);
            List<int> nepostojeci = ids.FindAll(id => !autori.Exists(a => a.Id == id));
            if (nepostojeci.Count == 0) return ids;

            Console.WriteLine($"Ne postoje autori sa ID-jem: {string.Join(", ", nepostojeci)}.");
        }
    }

    private Knjiga? SelectKnjiga()
    {
        List<Knjiga> knjige = _service.GetKnjige();
        PrintKnjige(knjige);
        if (knjige.Count == 0) return null;

        int id = ConsoleViewUtils.InputInt("ID knjige");
        Knjiga? knjiga = knjige.Find(k => k.Id == id);
        if (knjiga is null)
        {
            Console.WriteLine("Knjiga nije pronadjena.");
        }
        return knjiga;
    }

    private void AddKnjiga()
    {
        Knjiga knjiga = InputKnjiga();
        List<int> autorIds = InputAutorIds(null);

        // Knjiga se prvo sacuva da bi dobila ID, pa se pravi veza sa svakim autorom
        Knjiga nova = _service.DodajKnjigu(knjiga);
        foreach (int autorId in autorIds)
        {
            _service.DodajAutoraKnjizi(nova.Id, autorId);
        }
        Console.WriteLine("Knjiga je dodata.");
    }

    private void UpdateKnjiga()
    {
        Knjiga? stara = SelectKnjiga();
        if (stara is null) return;

        List<int> stariAutori = stara.Autori.Select(a => a.Id).ToList();

        Console.WriteLine("(Enter zadrzava trenutnu vrednost)");
        Knjiga izmenjena = InputKnjiga(stara);
        List<int> noviAutori = InputAutorIds(stariAutori);

        izmenjena.Id = stara.Id;
        _service.IzmeniKnjigu(izmenjena);

        foreach (int autorId in stariAutori.Except(noviAutori))
        {
            _service.UkloniAutoraSaKnjige(izmenjena.Id, autorId);
        }
        foreach (int autorId in noviAutori.Except(stariAutori))
        {
            _service.DodajAutoraKnjizi(izmenjena.Id, autorId);
        }
        Console.WriteLine("Knjiga je izmenjena.");
    }

    private void RemoveKnjiga()
    {
        Knjiga? knjiga = SelectKnjiga();
        if (knjiga is null) return;

        if (_service.KnjigaImaKupovine(knjiga.Id))
        {
            Console.WriteLine("Knjiga ima kupovine i ne moze se obrisati.");
            return;
        }

        if (!ConsoleViewUtils.Potvrdi($"Da li ste sigurni da zelite da obrisete knjigu \"{knjiga.Naziv}\"?"))
        {
            Console.WriteLine("Brisanje je otkazano.");
            return;
        }

        _service.ObrisiKnjigu(knjiga.Id);
        Console.WriteLine("Knjiga je obrisana (uklonjena je i sa listi zelja).");
    }
}