using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Services;

namespace SajamKnjiga.ConsoleApp.Views;

// Po uzoru na VehicleConsoleView iz CRUDExample (vjezbe 3)
class AdresaConsoleView
{
    private readonly SajamService _service;

    public AdresaConsoleView(SajamService service)
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
        Console.WriteLine("----- ADRESE -----");
        Console.WriteLine("1: Prikazi sve adrese");
        Console.WriteLine("2: Dodaj adresu");
        Console.WriteLine("3: Izmeni adresu");
        Console.WriteLine("4: Obrisi adresu");
        Console.WriteLine("0: Nazad");
        Console.Write("Izaberite opciju: ");
    }

    private void HandleMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                PrintAdrese(_service.GetAdrese());
                break;
            case "2":
                AddAdresa();
                break;
            case "3":
                UpdateAdresa();
                break;
            case "4":
                RemoveAdresa();
                break;
            default:
                Console.WriteLine("Nepostojeca opcija.");
                break;
        }
    }

    private void PrintAdrese(List<Adresa> adrese)
    {
        if (adrese.Count == 0)
        {
            Console.WriteLine("Nema adresa.");
            return;
        }

        Console.WriteLine($"{"ID",4} | {"Adresa",-40} | Koristi");
        foreach (Adresa a in adrese)
        {
            Console.WriteLine($"{a.Id,4} | {a,-40} | {KoKoristi(a.Id)}");
        }
    }

    private string KoKoristi(int adresaId)
    {
        List<string> imena = new();
        foreach (Posetilac p in _service.GetPosetioci().FindAll(p => p.AdresaId == adresaId))
        {
            imena.Add($"{p.Ime} {p.Prezime} (posetilac)");
        }
        foreach (Autor a in _service.GetAutori().FindAll(a => a.AdresaId == adresaId))
        {
            imena.Add($"{a.Ime} {a.Prezime} (autor)");
        }
        return imena.Count == 0 ? "-" : string.Join(", ", imena);
    }

    private Adresa? SelectAdresa()
    {
        List<Adresa> adrese = _service.GetAdrese();
        PrintAdrese(adrese);
        if (adrese.Count == 0) return null;

        int id = ConsoleViewUtils.InputInt("ID adrese");
        Adresa? adresa = adrese.Find(a => a.Id == id);
        if (adresa is null)
        {
            Console.WriteLine("Adresa nije pronadjena.");
        }
        return adresa;
    }

    private void AddAdresa()
    {
        Adresa adresa = ConsoleViewUtils.InputAdresa();
        _service.DodajAdresu(adresa);
        Console.WriteLine("Adresa je dodata.");
    }

    private void UpdateAdresa()
    {
        Adresa? stara = SelectAdresa();
        if (stara is null) return;

        Console.WriteLine("(Enter zadrzava trenutnu vrednost)");
        Adresa izmenjena = ConsoleViewUtils.InputAdresa(stara);
        izmenjena.Id = stara.Id;
        _service.IzmeniAdresu(izmenjena);
        Console.WriteLine("Adresa je izmenjena.");
    }

    private void RemoveAdresa()
    {
        Adresa? adresa = SelectAdresa();
        if (adresa is null) return;

        if (_service.AdresaSeKoristi(adresa.Id))
        {
            Console.WriteLine("Adresa se koristi i ne moze se obrisati.");
            return;
        }

        if (!ConsoleViewUtils.Potvrdi($"Da li ste sigurni da zelite da obrisete adresu {adresa}?"))
        {
            Console.WriteLine("Brisanje je otkazano.");
            return;
        }

        _service.ObrisiAdresu(adresa.Id);
        Console.WriteLine("Adresa je obrisana.");
    }
}
