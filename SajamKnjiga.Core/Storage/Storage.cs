using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SajamKnjiga.Core.Serialization;

namespace SajamKnjiga.Core.Storage;

// Preuzeo sa vjezbi 3 (CRUDExample, putanja izmjenjena da koristi Path.DirectorySeparatorChar)
public class Storage<T> where T : ISerializable, new()
{
    private readonly string _filePath;
    private readonly Serializer<T> _serializer = new();

    public Storage(string fileName)
    {
        _filePath = GetDataFolder() + fileName;
    }

    // .exe se nalazi u <projekat>/bin/Debug/net8.0/, pa idemo 4 nivoa gore do foldera solution-a
    private static string GetDataFolder()
    {
        char s = Path.DirectorySeparatorChar;
        string gore = ".." + s;
        string dataFolder = Path.GetFullPath(AppContext.BaseDirectory + gore + gore + gore + gore + "Data" + s);
        Directory.CreateDirectory(dataFolder);
        return dataFolder;
    }

    public List<T> Load()
    {
        // ukoliko fajl ne postoji napravi novi fajl zatvori stream za pisanje u fajl
        if (!File.Exists(_filePath))
        {
            FileStream fs = File.Create(_filePath);
            fs.Close();
        }

        IEnumerable<string> lines = File.ReadLines(_filePath);
        List<T> objects = _serializer.FromCSV(lines);

        return objects;
    }

    public void Save(List<T> objects)
    {
        string serializedObjects = _serializer.ToCSV(objects);
        using (StreamWriter streamWriter = new StreamWriter(_filePath))
        {
            streamWriter.Write(serializedObjects);
        }
    }
}
