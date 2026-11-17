using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Services;

namespace SajamKnjiga.ConsoleApp.Views;

// Po uzoru na VehicleConsoleView iz CRUDExample (vjezbe 3)
class PosetilacConsoleView
{
    private readonly SajamService _service;

    public PosetilacConsoleView(SajamService service)
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
        Console.WriteLine("----- POSETIOCI -----");
        Console.WriteLine("1: Prikazi sve posetioce");
        Console.WriteLine("2: Dodaj posetioca");
        Console.WriteLine("3: Izmeni posetioca");
        Console.WriteLine("4: Obrisi posetioca");
        Console.WriteLine("0: Nazad");
        Console.Write("Izaberite opciju: ");
    }

    private void HandleMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                PrintPosetioci(_service.GetPosetioci());
                break;
            case "2":
                AddPosetilac();
                break;
            case "3":
                UpdatePosetilac();
                break;
            case "4":
                RemovePosetilac();
                break;
            default:
                Console.WriteLine("Nepostojeca opcija.");
                break;
        }
    }

    private void PrintPosetioci(List<Posetilac> posetioci)
    {
        if (posetioci.Count == 0)
        {
            Console.WriteLine("Nema posetilaca.");
            return;
        }

        Console.WriteLine($"{"ID",4} | {"Br. karte",-12} | {"Ime",-12} | {"Prezime",-15} | {"Adresa",-35} | {"Status",-8}");
        foreach (Posetilac p in posetioci)
        {
            Console.WriteLine($"{p.Id,4} | {p}");
        }
    }

    private Posetilac InputPosetilac(Posetilac? trenutni = null)
    {
        string ime = ConsoleViewUtils.InputText("Ime", trenutni?.Ime);
        string prezime = ConsoleViewUtils.InputText("Prezime", trenutni?.Prezime);
        DateOnly datumRodjenja = ConsoleViewUtils.InputDate("Datum rodjenja", trenutni?.DatumRodjenja);
        string telefon = ConsoleViewUtils.InputText("Kontakt telefon", trenutni?.KontaktTelefon);
        string email = ConsoleViewUtils.InputEmail("E-mail", trenutni?.Email);
        string brojKarte = ConsoleViewUtils.InputText("Broj clanske karte (npr. SK-1-2023)", trenutni?.BrojClanskeKarte);
        int godinaClanstva = ConsoleViewUtils.InputInt("Trenutna godina clanstva", trenutni?.TrenutnaGodinaClanstva, 1);
        StatusPosetioca status = ConsoleViewUtils.InputStatus("Status", trenutni?.Status);

        return new Posetilac(ime, prezime, datumRodjenja, telefon, email, brojKarte, godinaClanstva, status, 0);
    }

    private Posetilac? SelectPosetilac()
    {
        List<Posetilac> posetioci = _service.GetPosetioci();
        PrintPosetioci(posetioci);
        if (posetioci.Count == 0) return null;

        int id = ConsoleViewUtils.InputInt("ID posetioca");
        Posetilac? posetilac = posetioci.Find(p => p.Id == id);
        if (posetilac is null)
        {
            Console.WriteLine("Posetilac nije pronadjen.");
        }
        return posetilac;
    }

    private void AddPosetilac()
    {
        Posetilac posetilac = InputPosetilac();
        Adresa adresa = ConsoleViewUtils.InputAdresa();

        posetilac.AdresaId = _service.DodajAdresu(adresa).Id;
        _service.DodajPosetioca(posetilac);
        Console.WriteLine("Posetilac je dodat.");
    }

    private void UpdatePosetilac()
    {
        Posetilac? stari = SelectPosetilac();
        if (stari is null) return;

        Console.WriteLine("(Enter zadrzava trenutnu vrednost)");
        Posetilac izmenjen = InputPosetilac(stari);
        Adresa izmenjenaAdresa = ConsoleViewUtils.InputAdresa(stari.Adresa);

        izmenjen.Id = stari.Id;
        if (stari.Adresa is null)
        {
            izmenjen.AdresaId = _service.DodajAdresu(izmenjenaAdresa).Id;
        }
        else
        {
            izmenjenaAdresa.Id = stari.AdresaId;
            _service.IzmeniAdresu(izmenjenaAdresa);
            izmenjen.AdresaId = stari.AdresaId;
        }

        _service.IzmeniPosetioca(izmenjen);
        Console.WriteLine("Posetilac je izmenjen.");
    }

    private void RemovePosetilac()
    {
        Posetilac? posetilac = SelectPosetilac();
        if (posetilac is null) return;

        if (!ConsoleViewUtils.Potvrdi($"Da li ste sigurni da zelite da obrisete posetioca {posetilac.Ime} {posetilac.Prezime}?"))
        {
            Console.WriteLine("Brisanje je otkazano.");
            return;
        }

        _service.ObrisiPosetioca(posetilac.Id);
        Console.WriteLine("Posetilac je obrisan (zajedno sa njegovim kupovinama, listom zelja i adresom).");
    }
}