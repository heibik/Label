using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class mod_mat : Tabelle
    {
        public int? mod_iNummer { get; set; }
        public int? mat_iId { get; set; }

        public mod_mat()
        {
        }

        public mod_mat(int? iId, int? mod_iNummer, int? mat_iId) : base(iId)
        {
            this.mod_iNummer = mod_iNummer;
            this.mat_iId = mat_iId;
        }

        public override string ToString()
        {
            return mod_iNummer +" "+ mat_iId;
        }
    }

}
