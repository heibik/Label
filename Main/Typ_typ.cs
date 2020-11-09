using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Typ_typ : Tabelle
    {
        public string sBezeichnung { get; set; }

        public Typ_typ(int? iId, string sBezeichnung):base(iId)
        {
            this.sBezeichnung = sBezeichnung;
        }

        public Typ_typ():base()
        {
        }

        public override string ToString()
        {
            return iId + " " + sBezeichnung;
        }
    }
}
