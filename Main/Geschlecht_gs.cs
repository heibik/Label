using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Geschlecht_gs : Tabelle
    {
        public string sGeschlecht { get; set; }
        public char sAbkuerzung { get; set; }

        public Geschlecht_gs(int? iId, string sGeschlecht, char sAbkuerzung) : base(iId)
        {
            this.sGeschlecht = sGeschlecht;
            this.sAbkuerzung = sAbkuerzung;
        }

        public Geschlecht_gs() : base()
        {
        }
    }

}
