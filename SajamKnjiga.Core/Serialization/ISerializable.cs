using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SajamKnjiga.Core.Serialization;

// Preuzeo sa vjezbi 3 (CRUDExample)
public interface ISerializable
{
    string[] ToCSV();

    void FromCSV(string[] values);
}