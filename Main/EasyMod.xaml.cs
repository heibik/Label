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

namespace Main
{
    /// <summary>
    /// Interaktionslogik für EasyMod.xaml
    /// </summary>
    public partial class EasyMod : UserControl
    {
        private Dictionary<string, UcBasisModul> _module = new Dictionary<string, UcBasisModul>();
        UcDrucken uiDruck;

        public EasyMod(UcDrucken druck)
        {
            InitializeComponent();
            SetArtikel();
            uiDruck = druck;

            uiTbModell.ItemsSource = Datenverwaltung.GetSearchResultsFromTable("Modell_mod");
            uiTbGroesse.ItemsSource = Datenverwaltung.GetSearchResultsFromTable("Groesse_gro");
            uiTbFarbe.ItemsSource = Datenverwaltung.GetSearchResultsFromTable("Farbe_fab");


        }



        private void SetArtikel()
        {
            ModulArtikel uiArtikel = new ModulArtikel();
            Grid.SetRow(uiArtikel, 1);
            uiArtikel.Margin = new Thickness(10, 10, 10, 10);
            Grid.SetRow(uiArtikel, 4);
            uiArtikel.HorizontalAlignment = HorizontalAlignment.Stretch;
            uiArtikel.VerticalAlignment = VerticalAlignment.Top;
            uiArtikel.Width = 618;
            uiGrSuche.Children.Add(uiArtikel);
            _module["Artikel_art"] = uiArtikel;
            uiArtikel.AddLabel2Druck += AddLabel2Druck;


            foreach (var modul in _module.Values)
            {
                modul.SelectedObjekt += DisplaySearchResult;
                modul.DisplayDefault += DisplayDefault;
            }


            //uiUcModell.SelectedObjekt += new UcModell.TabellenName(UcArtikel_SelectedArtikel);
            //uiUcMaterial.SelectedObjekt += new UcMaterial.TabellenName(UcArtikel_SelectedArtikel);
            //_module["Modell_mod"] = uiUcModell;
            //_module["Material_mat"] = uiUcMaterial;
        }


        private void DisplaySearchResult(string tabellenName)
        {
            /*foreach (var modul in _module)
            {
                if (modul.Key != tabellenName)
                {
                    modul.Value.DisplaySearchResult();
                }
            }*/
        }

        private void DisplayDefault()
        {
            uiTbModell.SelectedItem = null;
            uiTbGroesse.SelectedItem = null;
            uiTbFarbe.SelectedItem = null;

            uiTbModell.Items.Refresh();
            uiTbGroesse.Items.Refresh();
            uiTbFarbe.Items.Refresh();


            foreach (var modul in _module)
            {
                modul.Value.AlleAnzeigen();
            }
            
        }

        private void AddLabel2Druck(List<Artikel_art> artikelListe, int zahl)
        {
            uiDruck.GenerateFixedDocument(artikelListe, zahl);
        }


        public void AddDruck2Ui()
        {
            Grid.SetRow(uiDruck, 1);
            uiGrSuchergebnisse.Children.Add(uiDruck);
        }


        public void RemoveDruckFromUi()
        {
            try
            {
                uiGrSuchergebnisse.Children.Remove(uiDruck);
            }
            catch(Exception e)
            {
            }
        }

        private void Kriterium_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (sender == uiTbModell)
            {
                if(uiTbModell.SelectedItem == null)
                {
                    return;
                }
                List<object> test = new List<object>();
                test.Add(uiTbModell.SelectedItem);
                Datenverwaltung.SetSelectedObjekte(test, "Modell_mod");
            }else if (sender == uiTbGroesse)
            {
                if (uiTbGroesse.SelectedItem == null)
                {
                    return;
                }
                List<object> test = new List<object>();
                test.Add(uiTbGroesse.SelectedItem);
                Datenverwaltung.SetSelectedObjekte(test, "Groesse_gro");
            }else if (sender == uiTbFarbe)
            {
                if (uiTbFarbe.SelectedItem == null)
                {
                    return;
                }
                List<object> test = new List<object>();
                test.Add(uiTbFarbe.SelectedItem);
                Datenverwaltung.SetSelectedObjekte(test, "Farbe_fab");
            }

            uiTbModell.Items.Refresh();
            uiTbGroesse.Items.Refresh();
            uiTbFarbe.Items.Refresh();


            foreach (var modul in _module)
            {
                modul.Value.DisplaySearchResult();
            }


        }

        public void DisplaySearchResults()
        {
            foreach (var modul in _module)
            {
                modul.Value.DisplaySearchResult();
                uiTbModell.SelectedItem = null;
                uiTbGroesse.SelectedItem = null;
                uiTbFarbe.SelectedItem = null;
            }
        }

        private void UiDeleteAuswahl_Click(object sender, RoutedEventArgs e)
        {
            Datenverwaltung.GesamteAuswahlLoeschen();
            DisplayDefault();
        }
    }
}
