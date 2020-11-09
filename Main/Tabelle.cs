using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class Tabelle : IComparable<Tabelle>
    {
        public int? iId { get; set; }

        public Tabelle(int? iId)
        {
            this.iId = iId;
        }

        public Tabelle()
        {

        }

        public int? GetId()
        {
            return iId;
        }

        public override string ToString()
        {
            return String.Format($"{iId}");
        }

        public int CompareTo(Tabelle other)
        {
            throw new NotImplementedException();
        }
    }
}
