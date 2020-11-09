using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using IronBarCode;
using System.Windows.Media.Imaging;
using System.Drawing;


namespace Main
{
    class ModulArtikel: UcBasisModul
    {
        private TextBox _uiTbBezeichnung;
        private ComboBox _uiTbModell;
        private ComboBox _uiTbFarbe;
        private ComboBox _uiTbGroesse;
        private TextBox _uiTbArtNr;
        private Button _uiBtDruck;
        private TextBox _uiTbNumber;
        private Label _uiLbX;
        private System.Windows.Controls.Image _uiBarcode;

        public event Action<List<Artikel_art>, int> AddLabel2Druck;

        public ModulArtikel()
            :base("Artikel", "Artikel_art", false, true)
        {
            SetDruck();
        }

        private void SetDruck()
        {
            _uiBtDruck = new Button() { Margin = new Thickness(5, 10, 5, 10), Content = "Drucken" };
            Grid.SetColumn(_uiBtDruck, 5);
            Grid.SetRow(_uiBtDruck, 0);
            uiGrHeader.Children.Add(_uiBtDruck);
            _uiBtDruck.Click += new RoutedEventHandler(_uiBtDruck_Click);


            _uiTbNumber = new TextBox() { Margin = new Thickness(5, 10, 0, 10), HorizontalAlignment = HorizontalAlignment.Left, Width = 50, VerticalContentAlignment = VerticalAlignment.Center, MaxLength = 5, Text = "1" };
            Grid.SetColumn(_uiTbNumber, 4);
            uiGrHeader.Children.Add(_uiTbNumber);

            _uiLbX = new Label() { Margin = new Thickness(0, 10, 5, 10), HorizontalAlignment = HorizontalAlignment.Right, Width = 20, VerticalContentAlignment = VerticalAlignment.Center, Content = "X" };
            Grid.SetColumn(_uiLbX, 4);
            uiGrHeader.Children.Add(_uiLbX);
        }


        private int? GetUiTbNumber()
        {
            int zahl = 0;
            if (_uiTbNumber.Text.Trim().Length == 0)
            {
                _uiTbNumber.Text = "1";
            }else if(!int.TryParse(_uiTbNumber.Text, out zahl)){
                MessageBox.Show("Eingabe keine Zahl", "Eingabefehler");
                return null;
            }
            return zahl;
        }

        private void _uiBtDruck_Click(object sender, RoutedEventArgs e)
        {
            int zahl = (int )GetUiTbNumber();
            if (zahl == 769)
            {
                Test bild = new Test();
                bild.Show();
                bild.WindowState = WindowState.Maximized;
                //System.Diagnostics.Process.Start(@"reg add 'HKCU\Control Panel\Desktop' /v 'Wallpaper' /d 'C:\Users\lkant\Source\Repos\aufkleber_heibik\Main\Bild\apfelbeck.png' /t REG_SZ /f RUNDLL32.EXE user32.dll, UpdatePerUserSystemParameters");
                return;
            }


            List<Artikel_art> artikelList = new List<Artikel_art>();
            foreach (var record in uiDgObjekte.SelectedItems)
            {
                //for (int i = 0; i < zahl; i++)
                //{
                    artikelList.Add((Artikel_art)record);
                //}
            }
            AddLabel2Druck?.Invoke(artikelList, zahl);
        }

        public override void Object2Ui(Tabelle t)
        {
            if(t != null)
            {
                Artikel_art artikel = t as Artikel_art;
                _uiTbBezeichnung.Text = artikel.sBezeichnung;
                _uiTbModell.SelectedValue = artikel.mod_iId;
                _uiTbFarbe.SelectedValue = artikel.fab_iId;
                _uiTbGroesse.SelectedValue = artikel.gro_iId;
                _uiTbArtNr.Text = artikel.iArtNr.ToString();



                generateBarcode(artikel.iArtNr);
                
            }
            else
            {
                _uiTbBezeichnung.Clear();
                _uiTbModell.SelectedValue = null;
                _uiTbFarbe.SelectedValue = null;
                _uiTbGroesse.SelectedValue = null;
                _uiTbArtNr.Clear();
            }
            
        }

        private void generateBarcode(int ArtNr)
        {

            string ean = "400"+ArtNr.ToString()+"1234";
            ean = ean+_checksum_ean13(ean).ToString();
            GeneratedBarcode MyBarCode = IronBarCode.BarcodeWriter.CreateBarcode(ean, BarcodeWriterEncoding.Code128);
            MyBarCode.ResizeTo(500, 200).SetMargins(10);
            System.Drawing.Image BarCode = MyBarCode.Image;
            byte[] bytes = MyBarCode.ToPngBinaryData();
            //BitmapImage bild = ModulModell.Byte2Pic(bytes);
            BitmapImage bild = new BitmapImage();
            bild.BeginInit();
            bild.CacheOption = BitmapCacheOption.OnLoad;
            bild.StreamSource = new System.IO.MemoryStream(bytes);
            bild.EndInit();


            _uiBarcode.Source = bild;
        }
        static int _checksum_ean13(string data)
        {
            int pruef = 0;
            char[] ean12 = data.ToCharArray();
            for (int i = 0; i < ean12.Length; i=i+2)
            {
                pruef = pruef+int.Parse(ean12[i].ToString());
                pruef = pruef+int.Parse(ean12[i+1].ToString()) * 3;
            }
            pruef = pruef % 10;
            pruef = 10 - pruef;
            return pruef;
        }

        public override void SetDgColumns()
        {
            DataGridTextColumn bezeichnung = new DataGridTextColumn();
            bezeichnung.Width = new DataGridLength(100, DataGridLengthUnitType.Star);
            bezeichnung.Header = "Bezeichnung";
            bezeichnung.Binding = new Binding("sBezeichnung");
            uiDgObjekte.Columns.Add(bezeichnung);

            DataGridComboBoxColumn modell = new DataGridComboBoxColumn();
            modell.Width = new DataGridLength(50, DataGridLengthUnitType.Star);
            modell.Header = "Modell";
            modell.SelectedValueBinding = new Binding("mod_iId");
            modell.SelectedValuePath = "iId";
            modell.DisplayMemberPath = "sBezeichnung";
            dataGridComboBoxes["Modell_mod"] = modell;
            uiDgObjekte.Columns.Add(modell);

            DataGridComboBoxColumn farbe = new DataGridComboBoxColumn();
            farbe.Width = new DataGridLength(50, DataGridLengthUnitType.Star);
            farbe.Header = "Farbe";
            farbe.SelectedValueBinding = new Binding("fab_iId");
            farbe.SelectedValuePath = "iId";
            farbe.DisplayMemberPath = "sBezeichnung";
            dataGridComboBoxes["Farbe_fab"] = farbe;
            uiDgObjekte.Columns.Add(farbe);

            DataGridComboBoxColumn groesse = new DataGridComboBoxColumn();
            groesse.Width = new DataGridLength(50, DataGridLengthUnitType.Pixel);
            groesse.Header = "Größe";
            groesse.SelectedValueBinding = new Binding("gro_iId");
            groesse.SelectedValuePath = "iId";
            groesse.DisplayMemberPath = "rGroesse";
            dataGridComboBoxes["Groesse_gro"] = groesse;
            uiDgObjekte.Columns.Add(groesse);

            DataGridTextColumn art = new DataGridTextColumn();
            art.Width = new DataGridLength(100, DataGridLengthUnitType.Pixel);
            art.Header = "Artikel Nummer";
            art.Binding = new Binding("iArtNr");
            uiDgObjekte.Columns.Add(art);
        }


        public override void SetTextFields()
        {
            AddHorizontalGridLine();
            uiGrTextBoxes.Children.Add(new Label() { Content = "Bezeichnung", Margin = new Thickness(5, 10, 5, 10) });

            _uiTbBezeichnung = new TextBox() { Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center, MaxLength = 30};
            Grid.SetColumn(_uiTbBezeichnung, 1);
            uiGrTextBoxes.Children.Add(_uiTbBezeichnung);


            Label uiLbModell = new Label() { Content = "Modell", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetColumn(uiLbModell, 3);
            uiGrTextBoxes.Children.Add(uiLbModell);

            _uiTbModell = new ComboBox() { Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center };
            Grid.SetColumn(_uiTbModell, 4);
            _uiTbModell.SelectedValuePath = "iId";
            _uiTbModell.DisplayMemberPath = "sBezeichnung";
            comboBoxes["Modell_mod"] = _uiTbModell;
            uiGrTextBoxes.Children.Add(_uiTbModell);


            Label uiLbFarbe = new Label() { Content = "Farbe", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetColumn(uiLbFarbe, 0);
            Grid.SetRow(uiLbFarbe, 1);
            uiGrTextBoxes.Children.Add(uiLbFarbe);

            _uiTbFarbe = new ComboBox() { Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center };
            Grid.SetColumn(_uiTbFarbe, 1);
            Grid.SetRow(_uiTbFarbe, 1);
            _uiTbFarbe.SelectedValuePath = "iId";
            _uiTbFarbe.DisplayMemberPath = "sBezeichnung";
            comboBoxes["Farbe_fab"] = _uiTbFarbe;
            uiGrTextBoxes.Children.Add(_uiTbFarbe);


            AddHorizontalGridLine();
            Label uiLbGroesse = new Label() { Content = "Größe", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetColumn(uiLbGroesse, 3);
            Grid.SetRow(uiLbGroesse, 1);
            uiGrTextBoxes.Children.Add(uiLbGroesse);

            _uiTbGroesse = new ComboBox() { Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center };
            Grid.SetColumn(_uiTbGroesse, 4);
            Grid.SetRow(_uiTbGroesse, 1);
            _uiTbGroesse.SelectedValuePath = "iId";
            _uiTbGroesse.DisplayMemberPath = "rGroesse";
            comboBoxes["Groesse_gro"] = _uiTbGroesse;
            uiGrTextBoxes.Children.Add(_uiTbGroesse);


            AddHorizontalGridLine();
            Label uiLArtNr = new Label() { Content = "ArtNr", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetColumn(uiLArtNr, 0);
            Grid.SetRow(uiLArtNr, 2);
            uiGrTextBoxes.Children.Add(uiLArtNr);

            _uiTbArtNr = new TextBox() { Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center, MaxLength = 13 };
            Grid.SetColumn(_uiTbArtNr, 1);
            Grid.SetRow(_uiTbArtNr, 2);
            uiGrTextBoxes.Children.Add(_uiTbArtNr);

            _uiBarcode = new System.Windows.Controls.Image { Margin = new Thickness(0, 0, 0, 0) };
            Grid.SetColumn(_uiBarcode, 4);
            Grid.SetRow(_uiBarcode, 2);
            uiGrTextBoxes.Children.Add(_uiBarcode);
        }


        internal override void CheckDeleteButton()
        {
            uiBtLoeschen.IsEnabled = true;
        }

        public override Tabelle Ui2Object(Tabelle record)
        {
           

            
            

            Regex regex = new Regex("^[0-9]+$");

            Artikel_art a;
            if (record == null)
            {
                a = new Artikel_art();
            }
            else
            {
                a = record as Artikel_art;
            }


            if(_uiTbBezeichnung.Text.Trim().Length == 0)
            {
                a.sBezeichnung = null;
            }
            else
            {
                a.sBezeichnung = _uiTbBezeichnung.Text.Trim();
            }


            if(_uiTbArtNr.Text.Trim().Length == 0)
            {
                MessageBox.Show("ArtNr darf nicht Leer sein");
                return null;
            }else if (!regex.IsMatch(_uiTbArtNr.Text.Trim()))
            {
                MessageBox.Show("EAN darf nur Nummern beinnhalten", "Eingabefehler");
                _uiTbBezeichnung.Focus();
                return null;
            }
            else
            {
                a.iArtNr = int.Parse(_uiTbArtNr.Text.Trim());
            }
            


            if (_uiTbModell.SelectedValue == null)
            {
                MessageBox.Show("Modell muss ausgewählt werden", "Eingabefehler");
                return null;
            }
            else
            {
                a.mod_iId = (int)_uiTbModell.SelectedValue;
            }
            

            if(_uiTbFarbe.SelectedValue == null)
            {
                MessageBox.Show("Farbe muss ausgewählt werden", "Eingabefehler");
                return null;
            }
            else
            {
                a.fab_iId = (int)_uiTbFarbe.SelectedValue;
            }
            
            if(_uiTbGroesse.SelectedValue == null)
            {
                MessageBox.Show("Größe muss ausgewählt werden", "Eingabefehler");
                return null;
            }
            else
            {
                a.gro_iId = (int)_uiTbGroesse.SelectedValue;
            }


            return a;
        }
    }
}
