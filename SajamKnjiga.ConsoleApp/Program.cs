using System.Text;
using SajamKnjiga.ConsoleApp.Views;
using SajamKnjiga.Core.Services;

// Da bi se slova č, ć, š, ž, đ ispravno prikazivala i unosila
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

SajamService service = new SajamService();
MainConsoleView mainView = new MainConsoleView(service);
mainView.RunMenu();