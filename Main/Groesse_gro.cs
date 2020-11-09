using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Groesse_gro:Tabelle
    {

        public decimal  rGroesse { get; set; }

        public Groesse_gro(int? iId, decimal rGroesse):base(iId)
        {
            this.rGroesse = rGroesse;
        }

        public Groesse_gro() : base()
        {
        }

        public override string ToString()
        {
            return iId + " " + rGroesse;
        }
    }
}
