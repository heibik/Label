using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Modell_mod:Tabelle
    {
        public string sBezeichnung { get; set; }
        public string sGeschlecht{ get; set; }
        public byte[] bSymbolBild { get; set; }
        public int? typ_iId { get; set; }
        public List<Tabelle> Materialien { get; set; }

        public Modell_mod(int? iId, string sBezeichnung, char sGeschlecht, byte[] bSymbolBild, int? typ_iId)
        :base(iId)
        {
            this.sBezeichnung = sBezeichnung;
            this.sGeschlecht = sGeschlecht.ToString();
            this.bSymbolBild = bSymbolBild;
            this.typ_iId = typ_iId;
            Materialien = new List<Tabelle>();
        }

        public Modell_mod() : base()
        {
            Materialien = new List<Tabelle>();
        }

        public override string ToString()
        {
            return String.Format($"{iId} {sBezeichnung}");
        }
    }
}
