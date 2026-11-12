using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Services;

namespace SajamKnjiga.ConsoleApp.Views;

// Glavni meni koji korisnika usmjerava na menije pojedinacnih entiteta, po uzoru na CRUDExample
class MainConsoleView
{
    private readonly SajamService _service;

    public MainConsoleView(SajamService service)
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
        Console.WriteLine("===== SAJAM KNJIGA =====");
        Console.WriteLine("1: Posetioci");
        Console.WriteLine("2: Autori");
        Console.WriteLine("3: Knjige");
        Console.WriteLine("4: Izdavaci");
        Console.WriteLine("5: Kupovine");
        Console.WriteLine("6: Adrese");
        Console.WriteLine("0: Izlaz");
        Console.Write("Izaberite opciju: ");
    }

    private void HandleMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                new PosetilacConsoleView(_service).RunMenu();
                break;
            case "2":
                new AutorConsoleView(_service).RunMenu();
                break;
            case "3":
                new KnjigaConsoleView(_service).RunMenu();
                break;
            case "4":
                new IzdavacConsoleView(_service).RunMenu();
                break;
            case "5":
                new KupovinaConsoleView(_service).RunMenu();
                break;
            case "6":
                new AdresaConsoleView(_service).RunMenu();
                break;
            default:
                Console.WriteLine("Nepostojeca opcija.");
                break;
        }
    }
}