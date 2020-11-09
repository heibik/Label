using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Farbe_fab : Tabelle
    {
        public string sBezeichnung { get; set; }

        public Farbe_fab(int? iId, string sBezeichnung) : base(iId)
        {
            this.sBezeichnung = sBezeichnung;
        }

        public Farbe_fab():base()
        {
        }

        public override string ToString()
        {
            return sBezeichnung;
        }
    }
}
