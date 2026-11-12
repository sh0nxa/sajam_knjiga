# Sajam knjiga

Desktop aplikacija za referenta sajma knjiga.
Projekat iz predmeta Osnove informacionih sistema i softverskog inženjerstva, 2026/2027.

Tehnologije: C#, .NET

## Pokretanje
1. Otvoriti `SajamKnjiga.sln` u Visual Studio-u.
2. Pokrenuti sa Ctrl+F5

Projekti ciljaju .NET 8, konzolna aplikacija se pokreće i na novijoj verziji .NET-a ako 8 nije instaliran.

## Struktura
- `SajamKnjiga.Core` – model podataka, serijalizacija, DAO sloj i servis
  - `Models` – Posetilac, Autor, Knjiga, Izdavac, Kupovina, Adresa
  - `Serialization`, `Storage` – čitanje i pisanje CSV fajlova
  - `DAO` – po jedan DAO za svaki entitet i za veze knjiga–autor i lista želja
  - `Services/SajamService` – povezivanje referenci i pravila brisanja
- `SajamKnjiga.ConsoleApp` – konzolni prikaz, dodavanje, izmjena i brisanje svih entiteta
- `Data` – CSV fajlovi sa podacima (separator `|`)

## Podaci
Podaci se čuvaju u folderu `Data` u korijenu repozitorijuma. Uz projekat su priloženi testni podaci
(5 posetilaca, 4 autora, 3 izdavača, 6 knjiga, 8 kupovina, liste želja).

## Podjela posla
| Član tima		| Kontrolna tačka 1 |
| Djordje Gusic | sve funkcionalnosti (model, serijalizacija, konzolni CRUD za sve entitete) |