using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Artikel_art:Tabelle
    {
        public string sBezeichnung { get; set; }
        public int iArtNr { get; set; }
        public int mod_iId { get; set; }
        public int fab_iId { get; set; }
        public int gro_iId { get; set; }

        public Artikel_art(int? iId, string sBezeichnung, int iArtNr, int mod_iId, int fab_iId, int gro_iId)
            :base(iId)
        {
            this.sBezeichnung = sBezeichnung;
            this.iArtNr = iArtNr;
            this.mod_iId = mod_iId;
            this.fab_iId = fab_iId;
            this.gro_iId = gro_iId;
        }

        public Artikel_art()
            :base()
        {

        }

        public override string ToString()
        {
            return String.Format($"{iId}");
        }

    }
}
