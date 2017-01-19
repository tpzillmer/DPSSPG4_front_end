using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Interfaces
{
    public interface ISQLData
    {
        int Commit();
        void Delete(object entityType);
        void Add(object newObject);

    }
}
