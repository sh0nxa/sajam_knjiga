using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using SajamKnjiga.Core.Models;

namespace SajamKnjiga.ConsoleApp.Views;

// SafeInputInt preuzeo sa vjezbi 3 (CRUDExample), ostale metode napravljene po istom principu.
// Svaka metoda pita ponovo dok unos nije ispravan.
static class ConsoleViewUtils
{
    private const string DatumFormat = "dd.MM.yyyy.";
    private static readonly string[] DozvoljeniFormatiDatuma = { "d.M.yyyy.", "d.M.yyyy" };

    public static int SafeInputInt()
    {
        int input;

        string rawInput = Console.ReadLine() ?? string.Empty;

        while (!int.TryParse(rawInput, out input))
        {
            Console.WriteLine("Nije ispravan broj, pokusajte ponovo: ");

            rawInput = Console.ReadLine() ?? string.Empty;
        }

        return input;
    }

    private static string Procitaj(string poruka, string? trenutna)
    {
        if (trenutna is null)
            Console.Write($"{poruka}: ");
        else
            Console.Write($"{poruka} [{trenutna}]: ");

        return (Console.ReadLine() ?? string.Empty).Trim();
    }

    public static string InputText(string poruka, string? trenutna = null)
    {
        while (true)
        {
            string unos = Procitaj(poruka, trenutna);

            if (unos == "" && trenutna is not null) return trenutna;
            if (unos == "")
            {
                Console.WriteLine("Polje ne sme biti prazno.");
                continue;
            }
            if (unos.Contains('|'))
            {
                Console.WriteLine("Znak '|' nije dozvoljen.");
                continue;
            }

            return unos;
        }
    }

    public static string InputEmail(string poruka, string? trenutna = null)
    {
        while (true)
        {
            string unos = InputText(poruka, trenutna);

            int et = unos.IndexOf('@');
            if (et > 0 && unos.IndexOf('.', et) > et + 1 && !unos.EndsWith('.')) return unos;

            Console.WriteLine("E-mail nije ispravan (primer: ime@domen.com).");
            trenutna = null;
        }
    }

    public static int InputInt(string poruka, int? trenutna = null, int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            string unos = Procitaj(poruka, trenutna?.ToString());

            if (unos == "" && trenutna is not null) return trenutna.Value;
            if (!int.TryParse(unos, out int broj))
            {
                Console.WriteLine("Nije ispravan ceo broj.");
                continue;
            }
            if (broj < min || broj > max)
            {
                Console.WriteLine($"Broj mora biti izmedju {min} i {max}.");
                continue;
            }

            return broj;
        }
    }

    // Prihvata i decimalnu tacku i decimalni zarez
    public static double InputDouble(string poruka, double? trenutna = null, double min = 0)
    {
        while (true)
        {
            string unos = Procitaj(poruka, trenutna?.ToString(CultureInfo.InvariantCulture));

            if (unos == "" && trenutna is not null) return trenutna.Value;
            if (!double.TryParse(unos.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out double broj))
            {
                Console.WriteLine("Nije ispravan broj.");
                continue;
            }
            if (broj < min)
            {
                Console.WriteLine($"Broj ne sme biti manji od {min}.");
                continue;
            }

            return broj;
        }
    }

    public static DateOnly InputDate(string poruka, DateOnly? trenutna = null)
    {
        while (true)
        {
            string unos = Procitaj($"{poruka} ({DatumFormat})", trenutna?.ToString(DatumFormat, CultureInfo.InvariantCulture));

            if (unos == "" && trenutna is not null) return trenutna.Value;
            if (DateOnly.TryParseExact(unos, DozvoljeniFormatiDatuma, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly datum))
            {
                return datum;
            }

            Console.WriteLine($"Datum nije ispravan, unesite u obliku {DatumFormat} (npr. 12.12.2002.).");
        }
    }

    public static StatusPosetioca InputStatus(string poruka, StatusPosetioca? trenutna = null)
    {
        while (true)
        {
            string? trenutnaOznaka = trenutna is null ? null : (trenutna == StatusPosetioca.VIP ? "V" : "R");
            string unos = Procitaj($"{poruka} (R - redovan, V - VIP)", trenutnaOznaka).ToUpper();

            if (unos == "" && trenutna is not null) return trenutna.Value;
            if (unos == "R") return StatusPosetioca.Redovan;
            if (unos == "V") return StatusPosetioca.VIP;

            Console.WriteLine("Unesite R ili V.");
        }
    }

    public static int? InputOptionalId(string poruka, int? trenutna = null)
    {
        while (true)
        {
            string unos = Procitaj($"{poruka} (Enter - {(trenutna is null ? "bez" : "zadrzi")}, '-' - bez)", trenutna?.ToString());

            if (unos == "") return trenutna;
            if (unos == "-") return null;
            if (int.TryParse(unos, out int id) && id >= 0) return id;

            Console.WriteLine("Unesite ID, Enter ili '-'.");
        }
    }

    public static bool Potvrdi(string pitanje)
    {
        string unos = Procitaj($"{pitanje} (da/ne)", null).ToLower();
        return unos == "da" || unos == "d";
    }
}