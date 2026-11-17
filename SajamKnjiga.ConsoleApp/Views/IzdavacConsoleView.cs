using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Services;

namespace SajamKnjiga.ConsoleApp.Views;

// Po uzoru na VehicleConsoleView iz CRUDExample (vjezbe 3)
class IzdavacConsoleView
{
    private readonly SajamService _service;

    public IzdavacConsoleView(SajamService service)
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
        Console.WriteLine("----- IZDAVACI -----");
        Console.WriteLine("1: Prikazi sve izdavace");
        Console.WriteLine("2: Dodaj izdavaca");
        Console.WriteLine("3: Izmeni izdavaca");
        Console.WriteLine("4: Obrisi izdavaca");
        Console.WriteLine("0: Nazad");
        Console.Write("Izaberite opciju: ");
    }

    private void HandleMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                PrintIzdavaci(_service.GetIzdavaci());
                break;
            case "2":
                AddIzdavac();
                break;
            case "3":
                UpdateIzdavac();
                break;
            case "4":
                RemoveIzdavac();
                break;
            default:
                Console.WriteLine("Nepostojeca opcija.");
                break;
        }
    }

    private void PrintIzdavaci(List<Izdavac> izdavaci)
    {
        if (izdavaci.Count == 0)
        {
            Console.WriteLine("Nema izdavaca.");
            return;
        }

        Console.WriteLine($"{"ID",4} | {"Sifra",-10} | {"Naziv",-25} | {"Sef",-25} | {"Knjiga",6} | {"Autora",6}");
        foreach (Izdavac i in izdavaci)
        {
            Console.WriteLine($"{i.Id,4} | {i} | {i.Knjige.Count,6} | {i.Autori.Count,6}");
        }
    }

    private Izdavac InputIzdavac(Izdavac? trenutni = null)
    {
        string sifra = ConsoleViewUtils.InputText("Sifra", trenutni?.Sifra);
        string naziv = ConsoleViewUtils.InputText("Naziv", trenutni?.Naziv);
        int? sefId = InputSefId(trenutni?.SefId);

        return new Izdavac(sifra, naziv, sefId);
    }

    private int? InputSefId(int? trenutni)
    {
        List<Autor> kandidati = _service.GetAutori().FindAll(a => _service.MozeBitiSef(a.Id));
        Console.WriteLine("Mogu biti sef (5+ god. iskustva): " +
            (kandidati.Count == 0 ? "nema" : string.Join(", ", kandidati.Select(a => $"{a.Id} - {a.Ime} {a.Prezime}"))));

        while (true)
        {
            int? id = ConsoleViewUtils.InputOptionalId("ID sefa", trenutni);
            if (id is null || _service.MozeBitiSef(id.Value)) return id;

            Console.WriteLine("Autor ne postoji ili nema bar 5 godina iskustva.");
        }
    }

    private Izdavac? SelectIzdavac()
    {
        List<Izdavac> izdavaci = _service.GetIzdavaci();
        PrintIzdavaci(izdavaci);
        if (izdavaci.Count == 0) return null;

        int id = ConsoleViewUtils.InputInt("ID izdavaca");
        Izdavac? izdavac = izdavaci.Find(i => i.Id == id);
        if (izdavac is null)
        {
            Console.WriteLine("Izdavac nije pronadjen.");
        }
        return izdavac;
    }

    private void AddIzdavac()
    {
        Izdavac izdavac = InputIzdavac();

        if (_service.DodajIzdavaca(izdavac) is null)
        {
            Console.WriteLine("Izdavac nije dodat: izabrani sef nema bar 5 godina iskustva.");
            return;
        }
        Console.WriteLine("Izdavac je dodat.");
    }

    private void UpdateIzdavac()
    {
        Izdavac? stari = SelectIzdavac();
        if (stari is null) return;

        Console.WriteLine("(Enter zadrzava trenutnu vrednost)");
        Izdavac izmenjen = InputIzdavac(stari);
        izmenjen.Id = stari.Id;

        if (_service.IzmeniIzdavaca(izmenjen) is null)
        {
            Console.WriteLine("Izdavac nije izmenjen: izabrani sef nema bar 5 godina iskustva.");
            return;
        }
        Console.WriteLine("Izdavac je izmenjen.");
    }

    private void RemoveIzdavac()
    {
        Izdavac? izdavac = SelectIzdavac();
        if (izdavac is null) return;

        if (!ConsoleViewUtils.Potvrdi($"Da li ste sigurni da zelite da obrisete izdavaca {izdavac.Naziv}?"))
        {
            Console.WriteLine("Brisanje je otkazano.");
            return;
        }

        _service.ObrisiIzdavaca(izdavac.Id);
        Console.WriteLine("Izdavac je obrisan (njegove knjige su ostale bez izdavaca).");
    }
}