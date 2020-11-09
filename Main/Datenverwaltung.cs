using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    static class Datenverwaltung
    {
        private static Database _db = null;
        private static Dictionary<string, StammListe> _tabellen = new Dictionary<string, StammListe>();
       
        public static void Initialize()
        {
            _db = new Database();

            _tabellen["Artikel_art"] = new StammListe(_db.LadeArtikel());
            _tabellen["Material_mat"] = new StammListe(_db.LadeMaterial("%"));
            _tabellen["Modell_mod"] = new StammListe(_db.LadeModell("%", _tabellen["Material_mat"].GetAllRecords()));
            _tabellen["Typ_typ"] = new StammListe(_db.LadeTyp("%"));
            _tabellen["Geschlecht_gs"] = new StammListe(LadeGeschlecht());
            _tabellen["Groesse_gro"] = new StammListe(_db.LadeGroessen("%"));
            _tabellen["Farbe_fab"] = new StammListe(_db.LadeFarben("%"));
        }

        static public List<Tabelle> LadeGeschlecht()
        {
            List<Tabelle> _geschlechtListe = new List<Tabelle>();
            Geschlecht_gs g1 = new Geschlecht_gs(1, "Männlich", 'm');
            Geschlecht_gs g2 = new Geschlecht_gs(2, "Weiblich", 'w');
            Geschlecht_gs g3 = new Geschlecht_gs(3, "Unisex", 'u');
            _geschlechtListe.Add(g1);
            _geschlechtListe.Add(g2);
            _geschlechtListe.Add(g3);
            return _geschlechtListe;
        }

        static public List<Tabelle> GetSelectedObjects(string tabellenName)
        {
            return _tabellen[tabellenName].GetSelectedObjects();
        }

        static public List<Tabelle> GetSearchResultsFromTable(string tabellenName)
        {
            return _tabellen[tabellenName].GetSearchResults();
        }

        static public List<Tabelle> GetAllRecordsFromTable(string tabellenName)
        {
            return _tabellen[tabellenName].GetAllRecords();
        }

        static public void SetSelectedObjekte(System.Collections.IList selectedItems, string tabellenName)
        {
            _tabellen[tabellenName].SetSelectedIds(selectedItems);
            _db.SearchData(_tabellen, tabellenName);
        }

        static public void GesamteAuswahlLoeschen()
        {
            foreach (var tabelle in _tabellen.Values)
            {
                tabelle.ChangeAuswahlToDefault();
            }
        }

        static public bool DeleteRecordFromTable(string tabellenName, out string errmsg)
        {

            if (!_db.DeleteRecordFromTable(_tabellen[tabellenName], tabellenName, out errmsg))
            {
                return false;
            }
            _tabellen[tabellenName].DeleteSelectedRecords();
            return true;
        }

        static public bool SafeObjectInDb(Tabelle record, string tabellenName, out string errms)
        {
            errms = "";

            switch (tabellenName)
            {
                case "Modell_mod":
                    return _db.ModellSpeichern(record as Modell_mod, out errms);
                case "Material_mat":
                    return _db.MaterialSpeichern(record as Material_mat, out errms);
                case "Artikel_art":
                    return _db.ArtikelSpeichern(record as Artikel_art, out errms);
                case "Typ_typ":
                    return _db.TypSpeichern(record as Typ_typ, out errms);
                case "Groesse_gro":
                    return _db.GroesseSpeichern(record as Groesse_gro, out errms);
                case "Farbe_fab":
                    return _db.FarbeSpeichern(record as Farbe_fab, out errms);
            }


            return false;
        }


        static public void AddRecord(Tabelle record, string tabellenName)
        {
            _tabellen[tabellenName].AddRecord(record);
        }

    }
}
