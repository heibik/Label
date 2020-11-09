using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Main
{
    class Database
    {
        private string _connectionString =
                       "Data Source=10.0.104.72;Initial Catalog=db_lkant;User Id=lkant;" +
                       "Password=lkant;MultipleActiveResultSets=true;";

        public Database(string constr = "")
        {
            if (!string.IsNullOrEmpty(constr))
            {
                _connectionString = constr;
            }

        }

        public void SearchData(Dictionary<string, StammListe> tabellen, string originTabellenName)
        {
            
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                


                foreach (var tabellenName in tabellen.Keys)
                {
                    if (tabellenName != "Geschlecht_gs" && tabellenName != originTabellenName && originTabellenName != "Artikel_art")
                    {


                        string sql = "SELECT " +
                        "Artikel_art.iId AS Artikel_art, " +
                        "Groesse_gro.iId AS Groesse_gro, " +
                        "Farbe_fab.iId AS Farbe_fab, " +
                        "Modell_mod.iId AS Modell_mod, " +
                        "Typ_typ.iId AS Typ_typ, " +
                        "Material_mat.iId AS Material_mat " +
                        "FROM Aufkleber.Artikel_art " +
                        "FULL OUTER JOIN Aufkleber.Modell_mod ON Artikel_art.mod_iId = Modell_mod.iId " +
                        "FULL OUTER JOIN Aufkleber.Groesse_gro ON Artikel_art.gro_iId = Groesse_gro.iId " +
                        "FULL OUTER JOIN Aufkleber.Farbe_fab ON Artikel_art.fab_iId = Farbe_fab.iId " +
                        "FULL OUTER JOIN Aufkleber.Typ_typ ON Modell_mod.typ_iId = Typ_typ.iId " +
                        "FULL OUTER JOIN Aufkleber.mod_mat ON Modell_mod.iId = mod_mat.mod_iId " +
                        "FULL OUTER JOIN Aufkleber.Material_mat ON mod_mat.mat_iId = Material_mat.iId " +
                        "WHERE 1 = 1 ";

                        foreach (var tabelle in tabellen)
                        {

                            if (tabelle.Key != tabellenName && tabelle.Key != "Artikel_art")
                            {
                                if (GetAllIds(tabelle.Value.GetSelectedObjects()) != "")
                                {
                                    sql += "AND " + tabelle.Key + ".iId IN (" + GetAllIds(tabelle.Value.GetSelectedObjects()) + ") ";
                                }
                            }
                        }

                        sql += " ORDER BY " + tabellenName + ".iId";

                        Console.WriteLine(sql);

                        SqlCommand cmd = new SqlCommand(sql, con);
                        Console.WriteLine("hier");
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            tabellen[tabellenName].ClearAuswahl();
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (tabellenName == reader.GetName(i))
                                    {
                                        int? id = (!reader.IsDBNull(i) ? reader.GetInt32(i) : (int?)null);

                                        if (id != null)
                                        {
                                            tabellen[reader.GetName(i)].AddAuswahl((int)id);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                con.Close();
            }
        }

        //Heider
        internal bool FarbeSpeichern(Farbe_fab farbe_fab, out string errms)
        {
            errms = ""; // Annahme: kein Fehler

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                String sql;

                if (farbe_fab.iId != null)
                {
                    sql = "UPDATE Aufkleber.Farbe_fab SET sBezeichnung=@pbezeichnung " +
                                 "WHERE iId=@pid";
                }
                else

                {
                    sql = "INSERT INTO Aufkleber.Farbe_fab " +
                                 "(sBezeichnung) " +
                                 "VALUES (@pbezeichnung);" +
                                  "SELECT SCOPE_IDENTITY();";


                }
                SqlCommand cmd = new SqlCommand(sql, con);
                if (farbe_fab.iId != null)
                {
                    cmd.Parameters.AddWithValue("@pid", farbe_fab.iId);
                }

                cmd.Parameters.AddWithValue("@pbezeichnung", farbe_fab.sBezeichnung);

                try
                {
                    if (farbe_fab.iId == null)
                    {
                        farbe_fab.iId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 2627)
                    {
                        errms = "Nummer war nicht eindeutig.";
                    }
                    else
                    {
                        // irgendein Fehler
                        errms = "Unbekannter DB-Fehler";
                    }
                    // Orginaltext noch anhängen
                    errms += "\n" + e.Message;
                    return false;
                }
            }


            return true;
        }

        //Lukas
        internal bool GroesseSpeichern(Groesse_gro g, out string errms)
        {
            errms = "";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                String sql;

                Console.WriteLine("xxxxxxxxxxxxxx" + g.rGroesse);

                // alter oder neuer DS
                if (g.iId != null)
                {
                    sql = "UPDATE Aufkleber.Groesse_gro SET rGroesse=@pGroesse WHERE iId=@pid";
                }
                else
                {
                    sql = "INSERT INTO Aufkleber.Groesse_gro " +
                    "(rGroesse) " +
                    "VALUES (@pGroesse); " +
                    "SELECT SCOPE_IDENTITY();";
                }

                SqlCommand cmd = new SqlCommand(sql, con);

                // wenn es ein alter DS ist, dann Id gleich vergeben
                if (g.iId != null)
                {
                    cmd.Parameters.AddWithValue("@pid", g.iId);
                }


                // Pflichtfelder einfach gleich belegen
                cmd.Parameters.AddWithValue("@pGroesse", g.rGroesse);

                try
                {
                    if (g.iId == null)
                    {
                        g.iId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException e)
                {
                    if (e.Number == 2627)
                    {
                        errms = "Numer war nicht eindeutig";
                    }
                    else
                    {
                        errms = "Unbekannter DB-Fehler";
                    }
                    // Originaltext noch anhängen
                    errms += "\n" + e.Message;
                    return false;
                }
                con.Close();
            }
            return true;
        }

        //Biedro
        internal bool TypSpeichern(Typ_typ typ_typ, out string errmsg)
        {
            errmsg = ""; // Annahme: kein Fehler

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                String sql;

                if (typ_typ.iId != null)
                {
                    sql = "UPDATE Aufkleber.Typ_typ SET sBezeichnung=@pbezeichnung " +
                                 "WHERE iId=@pid";
                }
                else

                {
                    sql = "INSERT INTO Aufkleber.Typ_typ " +
                                 "(sBezeichnung) " +
                                 "VALUES (@pbezeichnung);" +
                                  "SELECT SCOPE_IDENTITY();";


                }

                SqlCommand cmd = new SqlCommand(sql, con);
                if (typ_typ.iId != null)
                {
                    cmd.Parameters.AddWithValue("@pid", typ_typ.iId);
                }



                // Parameter für Pflichtfelder sind einfach
                // Man kann den Wert gleich belegen


                // cmd.Parameters.AddWithValue("@pid", typ_typ.iId);
                cmd.Parameters.AddWithValue("@pbezeichnung", typ_typ.sBezeichnung);




                // Will man etwa die Id des neuen Datensatzes, so nutzt man 
                // ExecuteScalar. Die Id wird durch die obige Query ausgegeben.
                // Entscheidend ist die Funktion SCOPE_IDENTITY().
                // Die muss mit der selben Query wie der INSERT aufgerufen werden, sonst
                // funktioniert es nicht.
                try
                {

                    if (typ_typ.iId == null)
                    {
                        typ_typ.iId = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine("Neue Id: {0}", typ_typ.iId);
                        //cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        // Liefert die Query keinen Wert, dann ExecuteNonQuery
                        // return-Wert ist die Anzahl der betroffenen DS.
                        cmd.ExecuteNonQuery();
                        //cmd.Parameters.AddWithValue("@pnr", k.sNr);
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 2627)
                    {
                        // Unique violation
                        errmsg = "Nummer war nicht eindeutig.";
                    }
                    else
                    {
                        // irgendein Fehler
                        errmsg = "Unbekannter DB-Fehler";
                    }
                    // Orginaltext noch anhängen
                    errmsg += "\n" + e.Message;
                    return false;
                }
            }


            return true;
        }

        //Lukas
        internal bool ArtikelSpeichern(Artikel_art a, out string errms)
        {
            errms = "";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                String sql;

                // alter oder neuer DS
                if (a.iId != null)
                {
                    sql = "UPDATE Aufkleber.Artikel_art SET sBezeichnung=@pBezeichnung, iArtNr=@iArtNr, mod_iId = @pMod_iId, " +
                    "fab_iId=@pFab_iId, gro_iId=@pGro_iId " +
                    "WHERE iId=@pid";
                }
                else
                {
                    sql = "INSERT INTO Aufkleber.Artikel_art " +
                    "(sBezeichnung, iArtNr, mod_iId, fab_iId, gro_iId) " +
                    "VALUES (@pBezeichnung, @iArtNr, @pMod_iId, @pFab_iId, @pGro_iId);" +
                    " SELECT SCOPE_IDENTITY();";
                }

                SqlCommand cmd = new SqlCommand(sql, con);

                if (a.iId != null)
                {
                    cmd.Parameters.AddWithValue("@pid", a.iId);
                }


                cmd.Parameters.AddWithValue("@pMod_iId", a.mod_iId);
                cmd.Parameters.AddWithValue("@pFab_iId", a.fab_iId);
                cmd.Parameters.AddWithValue("@pGro_iId", a.gro_iId);
                cmd.Parameters.AddWithValue("@iArtNr", a.iArtNr);

                // Nullable Parameter auf NULL setzen
                SqlParameter pBezeichnung = cmd.Parameters.Add(new
                SqlParameter("@pBezeichnung", System.Data.SqlDbType.NVarChar));
                pBezeichnung.IsNullable = true;
                pBezeichnung.Value = DBNull.Value;

                SqlParameter pEAN = cmd.Parameters.Add(new
                SqlParameter("@pEAN", System.Data.SqlDbType.NChar));
                pEAN.IsNullable = true; 
                pEAN.Value = DBNull.Value;


                // Werte zuweisen für die Nullable-Spalten
                if (!string.IsNullOrEmpty(a.sBezeichnung))
                {
                    pBezeichnung.Value = a.sBezeichnung;
                }


                
                try
                {
                    if (a.iId == null)
                    {
                        a.iId = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine("Neue Id: {0}", a.iId);
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException e)
                {
                    if (e.Number == 2627)
                    {
                        errms = "Numer war nicht eindeutig";
                    }
                    else
                    {
                        errms = "Unbekannter DB-Fehler";
                    }
                    errms += "\n" + e.Message;
                    return false;
                }
                con.Close();
            }

            return true;

        }

        public List<Tabelle> LadeFarben(string bezeichnung)
        {
            List<Tabelle> farbenListe = new List<Tabelle>();
            using(SqlConnection  con = new SqlConnection(_connectionString))
            {
                con.Open();
                string sql = "SELECT iId, sBezeichnung FROM Aufkleber.Farbe_fab WHERE iId LIKE @p1";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@p1", bezeichnung);

                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int iid = reader.GetInt32(0);
                        string sbezeichnung = reader.GetString(1);

                        farbenListe.Add(new Farbe_fab(iid, sbezeichnung));
                    }
                }
                con.Close();
            }
            return farbenListe;
        }

        public List<Tabelle> LadeArtikel()
        {

            List<Tabelle> artikel = new List<Tabelle>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String sql = "SELECT iId, sBezeichnung, iArtNr, mod_iId, fab_iId, gro_iId FROM Aufkleber.Artikel_art ORDER BY iId";
                SqlCommand cmd = new SqlCommand(sql, con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int iId = reader.GetInt32(0);
                        string sBezeichnung = (!reader.IsDBNull(1) ? reader.GetString(1) : null);
                        int iArtNr = reader.GetInt32(2);
                        int mod_iId = reader.GetInt32(3);
                        int fab_iId = reader.GetInt32(4);
                        int gro_iId = reader.GetInt32(5);

                        artikel.Add(new Artikel_art(iId, sBezeichnung, iArtNr, mod_iId, fab_iId, gro_iId));
                    }
                }
                con.Close();
            }
            return artikel;
        }

        public List<Tabelle> LadeModell(string bezeichnung, List<Tabelle> materialien)
    {
        List<Tabelle> modelle = new List<Tabelle>();
        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            con.Open();
            String sql = "SELECT * FROM Aufkleber.Modell_mod WHERE sBezeichnung LIKE @p1 ORDER BY iId";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@p1", bezeichnung);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int iId = reader.GetInt32(0);
                    string sBezeichnung = reader.GetString(1);
                    string sGeschlech = reader.GetString(2);
                        byte[] bSymbolbild = null;
                        if (!reader.IsDBNull(3))
                        {
                            int buffersize = 65536;  // 2^16
                            int arrayoffset = 0;
                            bSymbolbild = new byte[buffersize];
                            long nbytesreturned = 0;
                            long dataoffset = 0;

                            nbytesreturned = reader.GetBytes(3, dataoffset, bSymbolbild, arrayoffset, buffersize);
                            while (nbytesreturned == buffersize)
                            {
                                dataoffset += buffersize;
                                arrayoffset += buffersize;
                                Array.Resize(ref bSymbolbild, bSymbolbild.Length + buffersize);
                                nbytesreturned = reader.GetBytes(3, dataoffset, bSymbolbild, arrayoffset, buffersize);
                            }

                            Array.Resize(ref bSymbolbild, bSymbolbild.Length - (buffersize - (int)nbytesreturned));
                        }
                    int typ_iId = reader.GetInt32(4);
                        
                    char sGeschlecht = char.Parse(sGeschlech);
                    modelle.Add(new Modell_mod(iId, sBezeichnung, sGeschlecht, bSymbolbild, typ_iId));
                }
            }
            con.Close();

                AddMaterial2Modell(modelle, materialien);
        }
        return modelle;
    }

        public void AddMaterial2Modell(List<Tabelle> modelle, List<Tabelle> materialien)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String sql = "SELECT mod_iId, mat_iId FROM Aufkleber.mod_mat";
                SqlCommand cmd = new SqlCommand(sql, con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int mod_iId = reader.GetInt32(0);
                        int mat_iId = reader.GetInt32(1);

                        foreach (var modell in modelle)
                        {
                            if(modell.iId == mod_iId)
                            {
                                foreach (var mat in materialien)
                                {
                                    if(mat.iId == mat_iId)
                                    {
                                        ((Modell_mod)modell).Materialien.Add((Material_mat)mat);
                                    }
                                }
                            }
                        }
                        
                    }
                }
                con.Close();
            }
        }

        public List<Tabelle> LadeMaterial(string bezeichnung)
        {
            List<Tabelle> material = new List<Tabelle>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String sql = "SELECT * FROM Aufkleber.Material_mat WHERE sBezeichnung LIKE @p1 ORDER BY iId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@p1", bezeichnung);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int iId = reader.GetInt32(0);
                        string sNummer = reader.GetString(1);
                        string sBezeichnung = reader.GetString(2);
                        
                        material.Add(new Material_mat(iId, sNummer, sBezeichnung));
                    }
                }
                con.Close();
            }
            return material;
        }

        public List<Tabelle> LadeTyp(string bezeichnung)
        {
            List<Tabelle> typ = new List<Tabelle>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String sql = "SELECT * FROM Aufkleber.Typ_typ WHERE sBezeichnung LIKE @p1 ORDER BY iId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@p1", bezeichnung);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int iId = reader.GetInt32(0);
                        string sBezeichnung = reader.GetString(1);


                        typ.Add(new Typ_typ(iId, sBezeichnung));
                    }
                }
                con.Close();
            }
            return typ;
        }

        public List<Tabelle> LadeGroessen(string groesse)
        {
            List<Tabelle> groessenListe = new List<Tabelle>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                string sql = "SELECT iId, rgroesse FROM Aufkleber.Groesse_gro WHERE rgroesse LIKE @p1";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@p1", groesse);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int iid = reader.GetInt32(0);
                        decimal rgroesse = reader.GetDecimal(1);

                        groessenListe.Add(new Groesse_gro(iid, rgroesse));
                    }
                }

                con.Close();
            }
            return groessenListe;
        }

        
        public Aufkleber LadeAufkleber(int iId)
        {
            Aufkleber aufkleber = new Aufkleber();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String sql = "SELECT art.sBezeichnung, mod.sBezeichnung,gro.rGroesse, mod.bSymbolBild, mod.sGeschlecht, fab.sBezeichnung FROM Aufkleber.Artikel_art as art " +
                                "Inner join Aufkleber.Modell_mod as mod On mod.iId = art.mod_iId " +
                                "inner join Aufkleber.Groesse_gro as gro on art.gro_iId = gro.iId " +
                                "inner join Aufkleber.Farbe_fab as fab on fab.iId = art.fab_iId " +
                                "WHERE art.iId = @p1";


                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@p1", iId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string artbezeichnung = (!reader.IsDBNull(0) ? reader.GetString(0) : null); ; // Nullable
                        string modbezeichnung = reader.GetString(1); 
                        decimal groesseD = reader.GetDecimal(2);
                        byte[] bSymbolbild = null; // Nullable
                        if (!reader.IsDBNull(3))
                        {
                            int buffersize = 65536;  // 2^16
                            int arrayoffset = 0;
                            bSymbolbild = new byte[buffersize];
                            long nbytesreturned = 0;
                            long dataoffset = 0;

                            nbytesreturned = reader.GetBytes(3, dataoffset, bSymbolbild, arrayoffset, buffersize);
                            while (nbytesreturned == buffersize)
                            {
                                dataoffset += buffersize;
                                arrayoffset += buffersize;
                                Array.Resize(ref bSymbolbild, bSymbolbild.Length + buffersize);
                                nbytesreturned = reader.GetBytes(3, dataoffset, bSymbolbild, arrayoffset, buffersize);
                            }

                            Array.Resize(ref bSymbolbild, bSymbolbild.Length - (buffersize - (int)nbytesreturned));
                        }
                        string geschlechtS = reader.GetString(4);
                        string farbe = reader.GetString(5);

                        char[] geschlecht = geschlechtS.ToCharArray();
                        aufkleber = (new Aufkleber(artbezeichnung, modbezeichnung, groesseD, bSymbolbild, geschlecht[0], farbe));
                    }
                }
                con.Close();
            }
            return aufkleber;
        }



        public bool MaterialSpeichern(Material_mat m, out string errmsg)
        {
            errmsg = ""; // Annahme: kein Fehler

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                String sql;

                if (m.iId != null)
                {
                    sql = "UPDATE Aufkleber.Material_mat SET sNummer=@pnummer, sBezeichnung=@pbezeichnung " +
                                 "WHERE iId=@pid";
                }
                else

                {
                    sql = "INSERT INTO Aufkleber.Material_mat " +
                                 "(sNummer, sBezeichnung) " +
                                 "VALUES (@pnummer, @pbezeichnung);" +
                                  "SELECT SCOPE_IDENTITY();";


                }

                SqlCommand cmd = new SqlCommand(sql, con);
                if (m.iId != null)
                {
                    cmd.Parameters.AddWithValue("@pid", m.iId);
                }



                // Parameter für Pflichtfelder sind einfach
                // Man kann den Wert gleich belegen


                cmd.Parameters.AddWithValue("@pnummer", m.sNummer);
                cmd.Parameters.AddWithValue("@pbezeichnung", m.sBezeichnung);




                // Will man etwa die Id des neuen Datensatzes, so nutzt man 
                // ExecuteScalar. Die Id wird durch die obige Query ausgegeben.
                // Entscheidend ist die Funktion SCOPE_IDENTITY().
                // Die muss mit der selben Query wie der INSERT aufgerufen werden, sonst
                // funktioniert es nicht.
                try
                {




                    if (m.iId == null)
                    {
                        m.iId = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine("Neue Id: {0}", m.iId);
                        //cmd.ExecuteNonQuery();

                    }
                    else
                    {
                        // Liefert die Query keinen Wert, dann ExecuteNonQuery
                        // return-Wert ist die Anzahl der betroffenen DS.
                        cmd.ExecuteNonQuery();
                        //cmd.Parameters.AddWithValue("@pnr", k.sNr);
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 2627)
                    {
                        // Unique violation
                        errmsg = "Nummer war nicht eindeutig.";
                    }
                    else
                    {
                        // irgendein Fehler
                        errmsg = "Unbekannter DB-Fehler";
                    }
                    // Orginaltext noch anhängen
                    errmsg += "\n" + e.Message;
                    return false;
                }
            }


            return true;
        }


        public string GetAllIds(List<Tabelle> liste)
        {
            string sql = "";
            if (liste.Count != 0)
            {
                for (int i = 0; i < liste.Count; i++)
                {
                    if (i != 0)
                    {
                        sql += ", ";
                    }
                    sql += liste[i].GetId();
                }
            }

            return sql;
        }


        public bool ModMatSpeichern(Modell_mod m, string errmsg)
        {
            errmsg = "";
            Console.WriteLine("in ModMatSpeichern vor löschen");

            DeleteModMat(new List<Tabelle> { m }, errmsg);
            Console.WriteLine("in ModMatSpeichern Nach löschen");

            if (m.Materialien.Count == 0)
            {
                Console.WriteLine("in ModMatSpeichern hat 0 Materialien");
                return true;
            }


            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                String sql;

                sql = "INSERT INTO Aufkleber.mod_mat " +
                "(mod_iId, mat_iId) VALUES";


                for (int i = 0; i < m.Materialien.Count; i++)
                {
                    if (i != 0)
                    {
                        sql += ", ";
                    }
                    sql += "(" + m.iId + ", " + m.Materialien[i].iId + ")";
                }


                Console.WriteLine(sql);
                SqlCommand cmd = new SqlCommand(sql, con);


                try
                {
                        cmd.ExecuteNonQuery();
                    Console.WriteLine("in ModMatSpeichern Nach Execute");
                }
                catch (SqlException e)
                {
                    if (e.Number == 2627)
                    {
                        errmsg = "Numer war nicht eindeutig";
                    }
                    else
                    {
                        errmsg = "Unbekannter DB-Fehler";
                    }
                    errmsg += "\n" + e.Message;
                    return false;
                }
                con.Close();
            }

            Console.WriteLine("in ModMatSpeichern sollte gehen");
            return true;
        }


        public bool DeleteModMat(List<Tabelle> liste, string errmsg)
        {
            errmsg = "";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                string sql = "DELETE FROM Aufkleber.mod_mat WHERE mod_iId IN (" + GetAllIds(liste) + ")";

                SqlCommand cmd = new SqlCommand(sql, con);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    errmsg = "Unbekannter DB-Fehler";
                    errmsg += "\n" + e.Message;
                    return false;
                }

                con.Close();
            }


            return false;
        }



        public bool DeleteRecordFromTable(StammListe liste, string tabellenName, out string errmsg)
        {

            errmsg = "";

            switch (tabellenName)
            {
                case "Modell_mod": if(DeleteModMat(liste.GetSelectedObjects(), errmsg)) return false;
                    break;
            }


            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                string sql = "DELETE FROM Aufkleber."+tabellenName+ " WHERE iId IN (" + GetAllIds(liste.GetSelectedObjects()) + ")";

                SqlCommand cmd = new SqlCommand(sql, con);

                try
                {
                    cmd.ExecuteNonQuery();
                }catch(SqlException e)
                {
                    if(e.Number == 547)
                    {
                        errmsg = "Fremdschlüssel Konflikt\nZuerst alle DS mit dieser Eigenschaft Löschen";
                    }
                    else
                    {
                        errmsg = "Unbekannter DB-Fehler";
                    }

                    errmsg += "\n" + e.Message;
                    return false;
                }

                con.Close();
            }

            return true;
        }



        //Heider
        public bool ModellSpeichern(Modell_mod mod, out string errmsg)
        {
            errmsg = "";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                String sql;

                if (mod.iId != null)
                {
                    sql = "UPDATE Aufkleber.Modell_mod SET sBezeichnung=@sBezeichnung, sGeschlecht = @sGeschlecht, bSymbolBild = @bSymbolBild, typ_iId = @typ_iId " +
                                 "WHERE iId=@pid";
                }
                else

                {
                    sql = "INSERT INTO Aufkleber.Modell_mod " +
                                 "(sBezeichnung, sGeschlecht, bSymbolBild, typ_iId) " +
                                 "VALUES (@sBezeichnung, @sGeschlecht, @bSymbolBild, @typ_iId);" +
                                  "SELECT SCOPE_IDENTITY();";

                }

                SqlCommand cmd = new SqlCommand(sql, con);
                if (mod.iId != null)
                {
                    cmd.Parameters.AddWithValue("@pid", mod.iId);
                }
                
                cmd.Parameters.AddWithValue("@sBezeichnung", mod.sBezeichnung);
                cmd.Parameters.AddWithValue("@sGeschlecht", mod.sGeschlecht);

                SqlParameter symbolBild = cmd.Parameters.Add(new SqlParameter("@bSymbolBild", System.Data.SqlDbType.VarBinary));
                symbolBild.IsNullable = true;
                symbolBild.Value = DBNull.Value;
                if (mod.bSymbolBild != null)
                {
                    symbolBild.Value = mod.bSymbolBild;
                }


                cmd.Parameters.AddWithValue("@typ_iId", mod.typ_iId);

               

                try
                {

                    if (mod.iId == null)
                    {
                        mod.iId = Convert.ToInt32(cmd.ExecuteScalar());
                        //cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 2627)
                    {
                        // Unique violation
                        errmsg = "Nummer war nicht eindeutig.";
                    }
                    else
                    {
                        // irgendein Fehler
                        errmsg = "Unbekannter DB-Fehler";
                    }
                    // Orginaltext noch anhängen
                    errmsg += "\n" + e.Message;
                    return false;
                }
            }

            ModMatSpeichern(mod, errmsg);


            return true;
        }

    }
}
