using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;

namespace Main
{
    class ModulModell : UcBasisModul
    {
        private TextBox _uiTbBezeichnung;
        private ComboBox _uiTbGeschlecht;
        private ComboBox _uiTbTyp;
        private string _uiTbSymbolBildPfad;
        private Button _uiBtSymbolBildAuswahl;
        private Image _uiImBildAnzeige;
        private ListBox _uiLiMaterial;
        private byte[] _bild;

        public ModulModell()
            : base("Modell", "Modell_mod", false, false)
        {

        }



        public override void Object2Ui(Tabelle t)
        {
            _uiLiMaterial.Items.Clear();
            foreach (var record in Datenverwaltung.GetAllRecordsFromTable("Material_mat"))
            {
                _uiLiMaterial.Items.Add(new CheckBox() { DataContext = (Material_mat)record, Content = ((Material_mat)record).sBezeichnung });
            }



            if (t != null)
            {
                Modell_mod modell = t as Modell_mod;
                _uiTbBezeichnung.Text = modell.sBezeichnung;
                _uiTbGeschlecht.SelectedValue = modell.sGeschlecht;
                _uiImBildAnzeige.Source = Byte2Pic(modell.bSymbolBild);
                _uiTbTyp.SelectedValue = modell.typ_iId;
                _bild = modell.bSymbolBild;

                foreach (var item in _uiLiMaterial.Items)
                {

                    if (modell.Materialien.Contains(((CheckBox)item).DataContext))
                    {
                        ((CheckBox)item).IsChecked = true;
                    }
                    else
                    {
                        ((CheckBox)item).IsChecked = false;
                    }
                }
            }
            else
            {
                _uiTbBezeichnung.Clear();
                _uiTbGeschlecht.SelectedValue = null;
                _uiImBildAnzeige.Source = null;
                _uiTbTyp.SelectedValue = null;
                _bild = null;

                foreach (var item in _uiLiMaterial.Items)
                {
                    ((CheckBox)item).IsChecked = false;
                }
            }
        }

        public override void SetDgColumns()
        {
            DataGridTextColumn bezeichnung = new DataGridTextColumn();
            bezeichnung.Width = new DataGridLength(50, DataGridLengthUnitType.Star);
            bezeichnung.Header = "Bezeichnung";
            bezeichnung.Binding = new Binding("sBezeichnung");
            uiDgObjekte.Columns.Add(bezeichnung);

            DataGridComboBoxColumn geschlecht = new DataGridComboBoxColumn();
            geschlecht.Width = new DataGridLength(50, DataGridLengthUnitType.Star);
            geschlecht.Header = "Geschlecht";
            geschlecht.SelectedValueBinding = new Binding("sGeschlecht");
            geschlecht.SelectedValuePath = "sAbkuerzung";
            geschlecht.DisplayMemberPath = "sGeschlecht";
            dataGridComboBoxes["Geschlecht_gs"] = geschlecht;
            uiDgObjekte.Columns.Add(geschlecht);

            DataGridComboBoxColumn typ = new DataGridComboBoxColumn();
            typ.Width = new DataGridLength(50, DataGridLengthUnitType.Star);
            typ.Header = "Typ";
            typ.SelectedValueBinding = new Binding("typ_iId");
            typ.SelectedValuePath = "iId";
            typ.DisplayMemberPath = "sBezeichnung";
            dataGridComboBoxes["Typ_typ"] = typ;
            uiDgObjekte.Columns.Add(typ);
        }


        public override void SetTextFields()
        {

            AddHorizontalGridLine();

            uiGrTextBoxes.Children.Add(new Label() { Content = "Bezeichnung", Margin = new Thickness(5, 10, 5, 10) });

            _uiTbBezeichnung = new TextBox() { Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center };
            Grid.SetColumn(_uiTbBezeichnung, 1);
            uiGrTextBoxes.Children.Add(_uiTbBezeichnung);


            AddHorizontalGridLine();
            Label uiLbGeschlecht = new Label() { Content = "Geschlecht", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetColumn(uiLbGeschlecht, 0);
            Grid.SetRow(uiLbGeschlecht, 1);
            uiGrTextBoxes.Children.Add(uiLbGeschlecht);

            _uiTbGeschlecht = new ComboBox() { Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center };
            Grid.SetColumn(_uiTbGeschlecht, 1);
            Grid.SetRow(_uiTbGeschlecht, 1);
            _uiTbGeschlecht.SelectedValuePath = "sAbkuerzung";
            _uiTbGeschlecht.DisplayMemberPath = "sGeschlecht";
            comboBoxes["Geschlecht_gs"] = _uiTbGeschlecht;
            uiGrTextBoxes.Children.Add(_uiTbGeschlecht);

            AddHorizontalGridLine();
            Label uiLbTyp = new Label() { Content = "Typ", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetColumn(uiLbTyp, 0);
            Grid.SetRow(uiLbTyp, 2);
            uiGrTextBoxes.Children.Add(uiLbTyp);

            _uiTbTyp = new ComboBox() { Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center };
            Grid.SetColumn(_uiTbTyp, 1);
            Grid.SetRow(_uiTbTyp, 2);
            _uiTbTyp.SelectedValuePath = "iId";
            _uiTbTyp.DisplayMemberPath = "sBezeichnung";
            comboBoxes["Typ_typ"] = _uiTbTyp;
            uiGrTextBoxes.Children.Add(_uiTbTyp);

            AddHorizontalGridLine();
            Label uiLbMaterial = new Label() { Content = "Material", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetColumn(uiLbMaterial, 0);
            Grid.SetRow(uiLbMaterial, 3);
            uiGrTextBoxes.Children.Add(uiLbMaterial);


            AddHorizontalGridLine();
            AddHorizontalGridLine();
            _uiLiMaterial = new ListBox() { Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetColumn(_uiLiMaterial, 1);
            Grid.SetRow(_uiLiMaterial, 3);
            Grid.SetRowSpan(_uiLiMaterial, 3);
            uiGrTextBoxes.Children.Add(_uiLiMaterial);


            Label uiLbSymbolbild = new Label() { Content = "Symbolbild", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetColumn(uiLbSymbolbild, 3);
            Grid.SetRow(uiLbSymbolbild, 0);
            uiGrTextBoxes.Children.Add(uiLbSymbolbild);

            _uiBtSymbolBildAuswahl = new Button() { Margin = new Thickness(5, 10, 5, 10), Content = "Bild Auswählen" };
            Grid.SetColumn(_uiBtSymbolBildAuswahl, 4);
            Grid.SetRow(_uiBtSymbolBildAuswahl, 0);
            uiGrTextBoxes.Children.Add(_uiBtSymbolBildAuswahl);
            _uiBtSymbolBildAuswahl.Click += new RoutedEventHandler(_uiBtSymbolBildAuswahl_Click);

            _uiImBildAnzeige = new Image() { Margin = new Thickness(0, 0, 0, 0)};
            Grid.SetColumn(_uiImBildAnzeige, 3);
            Grid.SetRow(_uiImBildAnzeige, 1);
            Grid.SetColumnSpan(_uiImBildAnzeige, 2);
            Grid.SetRowSpan(_uiImBildAnzeige, 3);
            uiGrTextBoxes.Children.Add(_uiImBildAnzeige);

        }

        internal override void CheckDeleteButton()
        {
            if (Datenverwaltung.GetSearchResultsFromTable("Artikel_art").Count == 0)
            {
                uiBtLoeschen.IsEnabled = true;
            }
            else
            {
                uiBtLoeschen.IsEnabled = false;
            }
        }

        public override Tabelle Ui2Object(Tabelle record)
        {
            Modell_mod m;
            byte[] savePic;
            if (record == null)
            {
                m = new Modell_mod();
            }
            else
            {
                m = record as Modell_mod;
            }

            /*string bez = _uiTbBezeichnung.Text; ;
            foreach (Modell_mod item in Datenverwaltung.GetAllRecordsFromTable("Modell_mod"))
            {
                
                if (item.sBezeichnung == bez)
                {
                    MessageBox.Show("Bezeichnung muss eindeutig sein", "Fehlermeldung");
                    return null;
                    
                }
            }*/
            try
            {
                m.sBezeichnung = _uiTbBezeichnung.Text;
            }
            catch(Exception e)
            {
                MessageBox.Show("Bezeichnug muss eindeutig sein", "DB-Fehler");
            }



            m.sGeschlecht = _uiTbGeschlecht.SelectedValue.ToString();

            m.bSymbolBild = _bild;

            savePic = m.bSymbolBild;
            m.bSymbolBild = Link2Byte();
            if (m.bSymbolBild == null)
            {
                m.bSymbolBild = savePic;
            }

            _uiTbSymbolBildPfad = null;

            m.typ_iId = (int?)_uiTbTyp.SelectedValue;


            m.Materialien.Clear();
            foreach (var item in _uiLiMaterial.Items)
            {
                if (((CheckBox)item).IsChecked == true)
                {
                    m.Materialien.Add((Material_mat)((CheckBox)item).DataContext);
                }
            }



            return m;
        }



        private void _uiBtSymbolBildAuswahl_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "Portable Network Graphics |*.png";
            dlg.InitialDirectory = "";
            dlg.Multiselect = true;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                string a = dlg.FileName;
                _uiTbSymbolBildPfad = a;
                if (a != "")
                {
                    anzeigeBild();
                }

            }

        }



        private void anzeigeBild()
        {
            
            BitmapImage original = Byte2Pic(Link2Byte());
            //var bild = new TransformedBitmap(original, new ScaleTransform(0.5, 0.5));

            _uiImBildAnzeige.Source = original;
        }

        private byte[] Link2Byte()
        {
            if (_uiTbSymbolBildPfad == null)
            {
                return null;
            }
            byte[] bytes = File.ReadAllBytes(_uiTbSymbolBildPfad);

            return bytes;
        }

        public static BitmapImage Byte2Pic(byte[] by)
        {
            if (by == null)
            {
                return null;
            }
            BitmapImage bild = new BitmapImage();
            bild.BeginInit();
            bild.CacheOption = BitmapCacheOption.OnLoad;
            bild.StreamSource = new MemoryStream(by);
            bild.EndInit();


            return bild;
        }

    }
}
