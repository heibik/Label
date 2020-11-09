using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaktionslogik für ProMod.xaml
    /// </summary>
    public partial class ProMod : UserControl
    {
        private Dictionary<string, UcBasisModul> _module = new Dictionary<string, UcBasisModul>();
        UcDrucken uiDruck;


        public ProMod(UcDrucken druck)
        {
            InitializeComponent();

            uiDruck = druck;

            ModulArtikel uiArtikel = new ModulArtikel();
            Grid.SetRow(uiArtikel, 1);
            uiArtikel.Margin = new Thickness(10, 10, 10, 10);
            Grid.SetColumnSpan(uiArtikel, 1);
            Grid.SetColumn(uiArtikel, 0);
            uiArtikel.HorizontalAlignment = HorizontalAlignment.Stretch;
            uiArtikel.VerticalAlignment = VerticalAlignment.Top;
            uiArtikel.Width = 590;
            uiGrSuchergebnisse.Children.Add(uiArtikel);
            _module["Artikel_art"] = uiArtikel;
            uiArtikel.AddLabel2Druck += AddLabel2Druck;


            ModulFarbe uiFarbe = new ModulFarbe();
            Grid.SetRow(uiFarbe, 1);
            uiFarbe.Margin = new Thickness(10, 10, 10, 10);
            Grid.SetColumn(uiFarbe, 0);
            uiFarbe.HorizontalAlignment = HorizontalAlignment.Stretch;
            uiFarbe.VerticalAlignment = VerticalAlignment.Top;
            uiFarbe.Width = 298;
            uiGrSuchkriterien.Children.Add(uiFarbe);
            _module["Farbe_fab"] = uiFarbe;


            ModulGroesse uiGroesse = new ModulGroesse();
            Grid.SetRow(uiGroesse, 1);
            uiGroesse.Margin = new Thickness(10, 10, 10, 10);
            Grid.SetColumn(uiGroesse, 1);
            uiGroesse.HorizontalAlignment = HorizontalAlignment.Stretch;
            uiGroesse.VerticalAlignment = VerticalAlignment.Top;
            uiGroesse.Width = 298;
            uiGrSuchkriterien.Children.Add(uiGroesse);
            _module["Groesse_gro"] = uiGroesse;



            ModulModell uiModell = new ModulModell();
            Grid.SetRow(uiModell, 2);
            uiModell.Margin = new Thickness(10, 10, 10, 10);
            Grid.SetColumnSpan(uiModell, 2);
            Grid.SetColumn(uiModell, 0);
            uiModell.Width = 618;
            uiGrSuchkriterien.Children.Add(uiModell);
            _module["Modell_mod"] = uiModell;




            ModulTyp uiTyp = new ModulTyp();
            Grid.SetRow(uiTyp, 3);
            uiTyp.Margin = new Thickness(10, 10, 10, 10);
            Grid.SetColumn(uiTyp, 0);
            uiTyp.HorizontalAlignment = HorizontalAlignment.Stretch;
            uiTyp.VerticalAlignment = VerticalAlignment.Top;
            uiTyp.Width = 298;
            uiGrSuchkriterien.Children.Add(uiTyp);
            _module["Typ_typ"] = uiTyp;


            ModulMaterial uiMaterial = new ModulMaterial();
            Grid.SetRow(uiMaterial, 3);
            uiMaterial.Margin = new Thickness(10, 10, 10, 10);
            Grid.SetColumn(uiMaterial, 1);
            uiMaterial.HorizontalAlignment = HorizontalAlignment.Stretch;
            uiMaterial.VerticalAlignment = VerticalAlignment.Top;
            uiMaterial.Width = 298;
            uiGrSuchkriterien.Children.Add(uiMaterial);
            _module["Material_mat"] = uiMaterial;



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



        public void AddDruck2Ui()
        {
            Grid.SetRow(uiDruck, 4);
            uiGrSuchergebnisse.Children.Add(uiDruck);
        }


        public void RemoveDruckFromUi()
        {
            uiGrSuchergebnisse.Children.Remove(uiDruck);
        }


        private void DisplaySearchResult(string tabellenName)
        {
            foreach (var modul in _module)
            {
                if (modul.Key != tabellenName)
                {
                    modul.Value.DisplaySearchResult();
                    modul.Value.ChangeButton();
                }
            }
        }

        private void DisplayDefault()
        {
            foreach (var modul in _module)
            {
                modul.Value.AlleAnzeigen();
            }
        }

        private void AddLabel2Druck(List<Artikel_art> artikelListe, int zahl)
        {
            uiDruck.GenerateFixedDocument(artikelListe, zahl);
        }


        public void DisplaySearchResults()
        {
            foreach (var modul in _module)
            {
                modul.Value.DisplaySearchResult();
                modul.Value.ChangeButton();
            }
        }
    }
}
