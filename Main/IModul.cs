using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public interface IModul
    {
        void Object2Ui(Tabelle o);
        void Ui2Object(Tabelle o);

        void SetDgColumns();

        void Refresh();

        UcBasisModul BasisModul { get; set; }
    }
}
