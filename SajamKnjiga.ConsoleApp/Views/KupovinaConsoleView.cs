using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Services;

namespace SajamKnjiga.ConsoleApp.Views;

class KupovinaConsoleView
{
    private readonly SajamService _service;

    public KupovinaConsoleView(SajamService service)
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
        Console.WriteLine("----- KUPOVINE -----");
        Console.WriteLine("1: Prikazi sve kupovine");
        Console.WriteLine("2: Dodaj kupovinu");
        Console.WriteLine("3: Izmeni kupovinu");
        Console.WriteLine("4: Obrisi kupovinu");
        Console.WriteLine("0: Nazad");
        Console.Write("Izaberite opciju: ");
    }

    private void HandleMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                PrintKupovine(_service.GetKupovine());
                break;
            case "2":
                AddKupovina();
                break;
            case "3":
                UpdateKupovina();
                break;
            case "4":
                RemoveKupovina();
                break;
            default:
                Console.WriteLine("Nepostojeca opcija.");
                break;
        }
    }

    private void PrintKupovine(List<Kupovina> kupovine)
    {
        if (kupovine.Count == 0)
        {
            Console.WriteLine("Nema kupovina.");
            return;
        }

        Console.WriteLine($"{"ID",4} | {"Posetilac",-25} | {"Knjiga",-25} | {"Datum",-11} | {"Ocena",5} | Komentar");
        foreach (Kupovina k in kupovine)
        {
            Console.WriteLine($"{k.Id,4} | {k}");
        }
    }

    private int InputPosetilacId()
    {
        List<Posetilac> posetioci = _service.GetPosetioci();
        Console.WriteLine("Posetioci: " + string.Join(", ", posetioci.Select(p => $"{p.Id} - {p.Ime} {p.Prezime}")));

        while (true)
        {
            int id = ConsoleViewUtils.InputInt("ID posetioca");
            if (posetioci.Exists(p => p.Id == id)) return id;

            Console.WriteLine("Posetilac sa tim ID-jem ne postoji.");
        }
    }

    private int InputKnjigaId()
    {
        List<Knjiga> knjige = _service.GetKnjige();
        Console.WriteLine("Knjige: " + string.Join(", ", knjige.Select(k => $"{k.Id} - {k.Naziv}")));

        while (true)
        {
            int id = ConsoleViewUtils.InputInt("ID knjige");
            if (knjige.Exists(k => k.Id == id)) return id;

            Console.WriteLine("Knjiga sa tim ID-jem ne postoji.");
        }
    }

    private Kupovina? SelectKupovina()
    {
        List<Kupovina> kupovine = _service.GetKupovine();
        PrintKupovine(kupovine);
        if (kupovine.Count == 0) return null;

        int id = ConsoleViewUtils.InputInt("ID kupovine");
        Kupovina? kupovina = kupovine.Find(k => k.Id == id);
        if (kupovina is null)
        {
            Console.WriteLine("Kupovina nije pronadjena.");
        }
        return kupovina;
    }

    private void AddKupovina()
    {
        if (_service.GetPosetioci().Count == 0 || _service.GetKnjige().Count == 0)
        {
            Console.WriteLine("Za kupovinu je potreban bar jedan posetilac i bar jedna knjiga.");
            return;
        }

        int posetilacId = InputPosetilacId();
        int knjigaId = InputKnjigaId();
        DateOnly datum = ConsoleViewUtils.InputDate("Datum kupovine");
        int ocena = ConsoleViewUtils.InputInt("Ocena (1-5)", null, 1, 5);
        string komentar = ConsoleViewUtils.InputText("Komentar");

        _service.DodajKupovinu(new Kupovina(posetilacId, knjigaId, datum, ocena, komentar));
        Console.WriteLine("Kupovina je dodata.");
    }

    private void UpdateKupovina()
    {
        Kupovina? stara = SelectKupovina();
        if (stara is null) return;

        Console.WriteLine("(Enter zadrzava trenutnu vrednost)");
        DateOnly datum = ConsoleViewUtils.InputDate("Datum kupovine", stara.DatumKupovine);
        int ocena = ConsoleViewUtils.InputInt("Ocena (1-5)", stara.Ocena, 1, 5);
        string komentar = ConsoleViewUtils.InputText("Komentar", stara.Komentar);

        Kupovina izmenjena = new Kupovina(stara.PosetilacId, stara.KnjigaId, datum, ocena, komentar);
        izmenjena.Id = stara.Id;
        _service.IzmeniKupovinu(izmenjena);
        Console.WriteLine("Kupovina je izmenjena.");
    }

    private void RemoveKupovina()
    {
        Kupovina? kupovina = SelectKupovina();
        if (kupovina is null) return;

        if (!ConsoleViewUtils.Potvrdi("Da li ste sigurni da zelite da obrisete ovu kupovinu?"))
        {
            Console.WriteLine("Brisanje je otkazano.");
            return;
        }

        _service.ObrisiKupovinu(kupovina.Id);
        Console.WriteLine("Kupovina je obrisana.");
    }
}