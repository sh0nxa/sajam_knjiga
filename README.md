# Sajam knjiga

Projekat iz predmeta Osnove informacionih sistema i softverskog inženjerstva (2026/2027).
Aplikacija je namijenjena referentu sajma knjiga: vodi evidenciju o posjetiocima, autorima, knjigama, izdavačima i kupovinama.

Rađeno u C#-u (.NET). Za sada sve ide kroz konzolu, a WPF dolazi u sljedećoj kontrolnoj tački.

## Kako pokrenuti
1. Otvorite `SajamKnjiga.sln` u Visual Studio-u.
2. Ctrl+F5 i to je to.

Projekti su na .NET 8. Ako na računaru imate samo noviju verziju .NET-a, konzolna aplikacija će se svejedno pokrenuti.

## Šta se gdje nalazi
- `SajamKnjiga.Core` – "mozak" aplikacije:
  - `Models` – Posetilac, Autor, Knjiga, Izdavac, Kupovina, Adresa
  - `Serialization` i `Storage` – čitanje i pisanje CSV fajlova
  - `DAO` – po jedan DAO za svaki entitet, plus dva za veze (knjiga–autor i lista želja)
  - `Services/SajamService` – povezuje entitete i pazi na pravila (npr. šta smije da se obriše)
- `SajamKnjiga.ConsoleApp` – konzolni meniji za prikaz, dodavanje, izmjenu i brisanje svih entiteta
- `Data` – CSV fajlovi sa podacima (separator je `|`)

Dio koda (serijalizacija, Storage, obrazac DAO-a i menija) urađen je po uzoru na primjer CRUDExample sa vježbi. To je naznačeno i komentarima u kodu.

## Podaci
Svi podaci se čuvaju u folderu `Data`, u korijenu repozitorijuma.
Ubacio sam i testne podatke da ne krećete od nule: 5 posjetilaca, 4 autora, 3 izdavača, 6 knjiga, 8 kupovina i par stavki na listama želja.

## Podjela posla
Projekat radim sam.

| Član tima | Kontrolna tačka 1 |
|---|---|
| Djordje Gusic | sve: model, serijalizacija i konzolni CRUD za sve entitete |
