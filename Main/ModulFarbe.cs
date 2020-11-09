using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Main
{
    class ModulFarbe: UcBasisModul
    {
        private TextBox _uiTbBezeichnung;

        public ModulFarbe()
            : base("Farbe", "Farbe_fab", true, false)
        {

        }


        public override void Object2Ui(Tabelle t)
        {
            

            if (t != null)
            {
                Farbe_fab farbe = t as Farbe_fab;
                _uiTbBezeichnung.Text = farbe.sBezeichnung;
            }
            else
            {
                _uiTbBezeichnung.Clear();
            }
        }

        public override void SetDgColumns()
        {
            DataGridTextColumn bezeichnung = new DataGridTextColumn();
            bezeichnung.Width = new DataGridLength(50, DataGridLengthUnitType.Star);
            bezeichnung.Header = "Bezeichnung";
            bezeichnung.Binding = new Binding("sBezeichnung");
            uiDgObjekte.Columns.Add(bezeichnung);

        }


        public override void SetTextFields()
        {
            AddHorizontalGridLine();
            uiGrTextBoxes.Children.Add(new Label() { Content = "Bezeichnung", Margin = new Thickness(5, 10, 5, 10) });

            _uiTbBezeichnung = new TextBox() { Text = "test", Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center };
            Grid.SetColumn(_uiTbBezeichnung, 1);

            uiGrTextBoxes.Children.Add(_uiTbBezeichnung);
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
            Farbe_fab f;
            if (record == null)
            {
                f = new Farbe_fab();
            }
            else
            {
                f = record as Farbe_fab;
            }
            if (true)
            {

            }
            if (_uiTbBezeichnung.Text.Trim().Length == 0)
            {
                _uiTbBezeichnung.Focus();
                MessageBox.Show("Bezeichnung darf nicht leer sein!", "Fehlermeldung");
                return null;
            }
            f.sBezeichnung = _uiTbBezeichnung.Text;

            return f;
        }

    }
}
