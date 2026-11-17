using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Models;
using SajamKnjiga.Core.Services;

namespace SajamKnjiga.ConsoleApp.Views;

// Po uzoru na VehicleConsoleView iz CRUDExample (vjezbe 3)
class AutorConsoleView
{
    private readonly SajamService _service;

    public AutorConsoleView(SajamService service)
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
        Console.WriteLine("----- AUTORI -----");
        Console.WriteLine("1: Prikazi sve autore");
        Console.WriteLine("2: Dodaj autora");
        Console.WriteLine("3: Izmeni autora");
        Console.WriteLine("4: Obrisi autora");
        Console.WriteLine("0: Nazad");
        Console.Write("Izaberite opciju: ");
    }

    private void HandleMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                PrintAutori(_service.GetAutori());
                break;
            case "2":
                AddAutor();
                break;
            case "3":
                UpdateAutor();
                break;
            case "4":
                RemoveAutor();
                break;
            default:
                Console.WriteLine("Nepostojeca opcija.");
                break;
        }
    }

    private void PrintAutori(List<Autor> autori)
    {
        if (autori.Count == 0)
        {
            Console.WriteLine("Nema autora.");
            return;
        }

        Console.WriteLine($"{"ID",4} | {"Ime",-12} | {"Prezime",-15} | {"Br. LK",-12} | {"Rodjen",-11} | {"E-mail",-25} | {"Iskustvo",8} | {"Knjiga",6}");
        foreach (Autor a in autori)
        {
            Console.WriteLine($"{a.Id,4} | {a} | {a.GodineIskustva,8} | {a.Knjige.Count,6}");
        }
    }

    private Autor InputAutor(Autor? trenutni = null)
    {
        string ime = ConsoleViewUtils.InputText("Ime", trenutni?.Ime);
        string prezime = ConsoleViewUtils.InputText("Prezime", trenutni?.Prezime);
        DateOnly datumRodjenja = ConsoleViewUtils.InputDate("Datum rodjenja", trenutni?.DatumRodjenja);
        string telefon = ConsoleViewUtils.InputText("Telefon", trenutni?.Telefon);
        string email = ConsoleViewUtils.InputEmail("E-mail", trenutni?.Email);
        string brojLicneKarte = ConsoleViewUtils.InputText("Broj licne karte", trenutni?.BrojLicneKarte);
        int godineIskustva = ConsoleViewUtils.InputInt("Godine iskustva", trenutni?.GodineIskustva, 0);

        return new Autor(ime, prezime, datumRodjenja, telefon, email, brojLicneKarte, godineIskustva, 0);
    }

    private Autor? SelectAutor()
    {
        List<Autor> autori = _service.GetAutori();
        PrintAutori(autori);
        if (autori.Count == 0) return null;

        int id = ConsoleViewUtils.InputInt("ID autora");
        Autor? autor = autori.Find(a => a.Id == id);
        if (autor is null)
        {
            Console.WriteLine("Autor nije pronadjen.");
        }
        return autor;
    }

    private void AddAutor()
    {
        Autor autor = InputAutor();
        Adresa adresa = ConsoleViewUtils.InputAdresa();

        autor.AdresaId = _service.DodajAdresu(adresa).Id;
        _service.DodajAutora(autor);
        Console.WriteLine("Autor je dodat.");
    }

    private void UpdateAutor()
    {
        Autor? stari = SelectAutor();
        if (stari is null) return;

        Console.WriteLine("(Enter zadrzava trenutnu vrednost)");
        Autor izmenjen = InputAutor(stari);
        Izdavac? izdavac = _service.IzdavacKomeJeSef(stari.Id);
        
        if (izdavac is not null && izmenjen.GodineIskustva < SajamService.MinGodinaIskustvaZaSefa)
        {
            Console.WriteLine($"Autor je sef izdavaca {izdavac.Naziv} i mora imati bar {SajamService.MinGodinaIskustvaZaSefa} godina iskustva. Izmena je odbijena.");
            return;
        }

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

        _service.IzmeniAutora(izmenjen);
        Console.WriteLine("Autor je izmenjen.");
    }

    private void RemoveAutor()
    {
        Autor? autor = SelectAutor();
        if (autor is null) return;

        if (!ConsoleViewUtils.Potvrdi($"Da li ste sigurni da zelite da obrisete autora {autor.Ime} {autor.Prezime}?"))
        {
            Console.WriteLine("Brisanje je otkazano.");
            return;
        }

        _service.ObrisiAutora(autor.Id);
        Console.WriteLine("Autor je obrisan (uklonjen je sa svojih knjiga i sa mesta sefa izdavaca).");
    }
}