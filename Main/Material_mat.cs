using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Material_mat : Tabelle
    {
        public string sNummer { get; set; }
        public string sBezeichnung { get; set; }

        public Material_mat(int? iId, string sNummer, string sBezeichnung)
            :base(iId)
        {
            this.sNummer = sNummer;
            this.sBezeichnung = sBezeichnung;
        }

        public Material_mat()
        {
        }

        public override string ToString()
        {
            return iId + " " + sNummer + " " + sBezeichnung;
        }
    }
}
