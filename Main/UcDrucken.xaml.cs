using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;  // IAddChild
using System.Collections.ObjectModel;

namespace Main
{
    /// <summary>
    /// Interaktionslogik für UcDrucken.xaml
    /// </summary>

    public partial class UcDrucken : UserControl
    {

        private static Database _db = null;
        private static List<int?[]> _idListe = new List<int?[]>(); // ID Liste für die Abfrage in der DB
        private static List<int> rmanzahl = new List<int>(); // Liste mit der Anzahl der zuletzt hinzugefügten Aufkleber
        private static int aufklebeberdavor;

        public UcDrucken()
        {
            InitializeComponent();
        }

        #region Hilfsmethoden
        public static int Cm2Dip(double cm)
        {
            return (int)(cm / 2.54 * 96);
        }

        public static Size MeasureString(TextBlock tb)
        {
            var formattedText = new FormattedText(
                tb.Text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch),
                tb.FontSize,
                Brushes.Black);

            if (!double.IsInfinity(tb.MaxWidth))
            {
                formattedText.MaxTextWidth = tb.MaxWidth;
            }

            return new Size(formattedText.Width, formattedText.Height);
        }

        #endregion Hilfsmethoden

        internal void GenerateFixedDocument(List<Artikel_art> liste, int zahl)
        {
            bool ok;    // Parse
            int anzahldavor = 0; // anzahl der Aufkeber die ausgelassen werden sollen
            int aufkleberanzahl = 12;   // Anzahl der zu druckenden Aufkleber pro Blatt

            #region Positionen frei lassen
            if (uiTBAnzahlDavor.Text.Trim() != "")  // Wenn das Feld nicht leer gelassen wurde
            {
                ok = int.TryParse(uiTBAnzahlDavor.Text, out anzahldavor); // Dann Parses

                if (!ok || anzahldavor > aufkleberanzahl - 1) // Prüfen ob eine gültige Zahl übergeben wurde
                {
                    MessageBox.Show("Anzahl der davor gedruckten Aufkleber ist keine gültige Zahl [1 bis " + (aufkleberanzahl - 1) + "]", "Druckfehler");
                    return;
                }
                // Dann Fälle abprüfen, um die ausgellassen Positionen hinzuzufügen
                // Davor gedruckte Anzahl < als die vorherig eingetragene
                if (aufklebeberdavor > anzahldavor)
                {
                    _idListe.RemoveRange(0, aufklebeberdavor);
                    aufklebeberdavor = 0;
                    for (int i = 0; i < anzahldavor; i++)
                    {
                        _idListe.Insert(i, null);

                        aufklebeberdavor = anzahldavor;
                    }
                }
                // Davor gedruckte Anzahl > als die vorherig eingetragene
                else
                {
                    for (int i = 0; i < anzahldavor; i++)
                    {
                        if (_idListe.Count() < anzahldavor || _idListe[i] != null)
                        {
                            _idListe.Insert(i, null);
                        }
                        aufklebeberdavor = anzahldavor;
                    }
                }
            }


            // Wenn das Feld "" ist
            // davor gedruckte Anzahl wird zurückgesetzt
            else if (aufklebeberdavor > 0)
            {
                _idListe.RemoveRange(0, aufklebeberdavor);
                aufklebeberdavor = 0;
            }
            #endregion Positionen frei lassen

            #region Artikel2Id
            // Id von übergebenen Artikeln wird gesammelt 

            //int zahl = 1000;
            if (liste != null)
            {
                rmanzahl.Add(0);
                foreach (var item in liste)
                {
                    if (item.iId != null)
                    {
                        int?[] zwischen = new int?[2];
                        rmanzahl[rmanzahl.Count() - 1]++;
                        int? id = item.iId;
                        if (id != null)
                        {
                             zwischen[0] = id;
                            zwischen[1] = zahl;
                            _idListe.Add(zwischen);
                        }
                    }

                }
            }
            #endregion Artikel2Id

            // Wenn Aufkleber zum Drucken da sind
            if (_idListe.Count() != 0)
            {
                #region Id2Aufkleber
                List <Aufkleber> array = new List<Aufkleber>();
                for (int i = 0; i < _idListe.Count(); i++)
                {
                    if (_idListe[i] != null)
                    {
                        int?[] zwischen = new int?[2];
                        zwischen = _idListe[i];
                        if (zwischen[0] != null)
                        {
                            _db = new Database();
                            if (zwischen[1] > 1)
                            {
                                Aufkleber aZwischen = _db.LadeAufkleber((int)zwischen[0]);
                                
                                for (int j = 0; j < zwischen[1]; j++)
                                {
                                    array.Add(aZwischen);
                                }
                            }
                            else
                            {
                                array.Add(_db.LadeAufkleber((int)zwischen[0])); // Aufkleber Array wird aus DB geladen
                            }

                        }
                    }
                    else
                    {
                        array.Add(null); // freier Platz (bereits gedruckte Aufkleber)
                    }

                }
                #endregion Id2Aufkleber
                #region Dokument mit Aufklebern erstellen
                // Dokument erstellen
                uiDocumentViewer.FitToMaxPagesAcross(1);
                Size pagesize = new Size(Cm2Dip(21), Cm2Dip(29.7));  // DIN A4
                FixedDocument document = new FixedDocument();
                document.DocumentPaginator.PageSize = pagesize;

                double oben;
                double links;
                int counter;
                decimal seiten;

                seiten = ((decimal)array.Count()) / ((decimal)aufkleberanzahl);

                int stellearray = 0;
                int maxarray = aufkleberanzahl;

                // Seiten mit Aufklebern hinzufügen
                for (int i = 0; i < seiten; i++)
                {
                    oben = 0.0;
                    links = 0.75;
                    counter = 1;

                    // Neue Seite
                    FixedPage page1 = new FixedPage
                    {
                        Width = document.DocumentPaginator.PageSize.Width,
                        Height = document.DocumentPaginator.PageSize.Height
                    };

                    // Neuer Aufkleber
                    for (int j = stellearray; j < maxarray && j < array.Count(); j++)
                    {
                        // Nur wenn der Aufkleber nicht ausgelassen werden soll
                        if (array[j] != null)
                        {
                            UcAufkleber p1e1 = new UcAufkleber();
                            p1e1.DataContext = array[j];
                            p1e1.Margin = new Thickness(Cm2Dip(links), Cm2Dip(oben), 0, 0);
                            p1e1.Width = Cm2Dip(9);
                            p1e1.Height = Cm2Dip(4.5);
                            page1.Children.Add(p1e1);

                        }
                        // Position des nächsten Aufklebers festlegen
                        if (counter == 2)
                        {
                            counter = 1;
                            oben += 4.5;
                            links = 0.75;
                        }
                        else
                        {
                            counter++;
                            links += 9.75;
                        }

                    }

                    // Inhalt zur Seite hinzufügen
                    PageContent page3Content = new PageContent();
                    ((IAddChild)page3Content).AddChild(page1);
                    document.Pages.Add(page3Content);
                    stellearray += aufkleberanzahl;
                    maxarray += aufkleberanzahl;
                }

                // Dokument erstellen
                uiDocumentViewer.Document = document;
                #endregion Dokument mit Aufklebern erstellen
            }
        }

        private void UiButton_Entfernen(object sender, RoutedEventArgs e)
        {

            if (rmanzahl.Count() - 1 > 0 && _idListe.Count() > 0)
            {
                if (_idListe.Count() - rmanzahl[rmanzahl.Count() - 1] > 0)
                {

                    _idListe.RemoveRange(_idListe.Count() - rmanzahl[rmanzahl.Count() - 1], rmanzahl[rmanzahl.Count() - 1]);
                    rmanzahl.RemoveAt(rmanzahl.Count() - 1);
                    GenerateFixedDocument(null, 0);

                }
            }
            else
            {
                uiDocumentViewer.Document = null;
                _idListe.Clear();
                rmanzahl.Clear();
                aufklebeberdavor = 0;
            }


        }

        private void UiButton_Alle_Entfernen(object sender, RoutedEventArgs e)
        {
            uiDocumentViewer.Document = null;
            _idListe.Clear();
            rmanzahl.Clear();
            aufklebeberdavor = 0;
        }

        private void UiButton_Refresh(object sender, RoutedEventArgs e)
        {
            if (_idListe.Count() != 0)
            {
                GenerateFixedDocument(null,0);
            }

            else
            {
                uiDocumentViewer.Document = null;
            }


        }
    }
}
