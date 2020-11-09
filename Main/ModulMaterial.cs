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
    class ModulMaterial : UcBasisModul
    {
        private TextBox _uiTbBezeichnung;
        private TextBox _uiTbNummer;

        public ModulMaterial()
            : base("Material", "Material_mat", true, false)
        {
        }

        public override void Object2Ui(Tabelle t)
        {
            if (t != null)
            {
                Material_mat modell = t as Material_mat;
                _uiTbBezeichnung.Text = modell.sBezeichnung;
                _uiTbNummer.Text = modell.sNummer;
            }
            else
            {
                _uiTbBezeichnung.Clear();
                _uiTbNummer.Clear();
            }
        }

        public override void SetDgColumns()
        {

            DataGridTextColumn nummer = new DataGridTextColumn();
            nummer.Width = new DataGridLength(50, DataGridLengthUnitType.Star);
            nummer.Header = "Nummer";
            nummer.Binding = new Binding("sNummer");
            uiDgObjekte.Columns.Add(nummer);


            DataGridTextColumn bezeichnung = new DataGridTextColumn();
            bezeichnung.Width = new DataGridLength(50, DataGridLengthUnitType.Star);
            bezeichnung.Header = "Bezeichnung";
            bezeichnung.Binding = new Binding("sBezeichnung");
            uiDgObjekte.Columns.Add(bezeichnung);

        }

        public override void SetTextFields()
        {
            AddHorizontalGridLine();
            Label uiLbNummer = new Label() { Content = "Nummer", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetRow(uiLbNummer, 0);
            uiGrTextBoxes.Children.Add(uiLbNummer);

            _uiTbNummer = new TextBox() {Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center, MaxLength = 10 };
            Grid.SetColumn(_uiTbNummer, 1);
            Grid.SetRow(_uiTbNummer, 0);
            uiGrTextBoxes.Children.Add(_uiTbNummer);


            AddHorizontalGridLine();
            Label uiLbBezeichnung = new Label() { Content = "Bezeichnung", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetRow(uiLbBezeichnung, 1);
            uiGrTextBoxes.Children.Add(uiLbBezeichnung);

            _uiTbBezeichnung = new TextBox() {Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center };
            Grid.SetColumn(_uiTbBezeichnung, 1);
            Grid.SetRow(_uiTbBezeichnung, 1);
            uiGrTextBoxes.Children.Add(_uiTbBezeichnung);

        }

        public override Tabelle Ui2Object(Tabelle record)
        {
            Material_mat m;
            if (record == null)
            {
                m = new Material_mat();
            }
            else
            {
                m = record as Material_mat;
            }



            if (_uiTbBezeichnung.Text.Trim().Length == 0)
            {
                _uiTbBezeichnung.Focus();
                MessageBox.Show("Bezeichnung darf nicht leer sein!", "Fehlermeldung");
                return null;
            }
            else
            {
                m.sBezeichnung = _uiTbBezeichnung.Text;

            }


            if (_uiTbNummer.Text.Trim().Length == 0)
            {
                _uiTbNummer.Focus();
                MessageBox.Show("Nummer darf nicht leer sein!", "Fehlermeldung");
                return null;
            }
            else if (_uiTbNummer.Text.Trim().Length != 10)
            {
                _uiTbNummer.Focus();
                MessageBox.Show("Nummer muss 10-stellig sein!", "Fehlermeldung");
                return null;
            }
            else
            {
                m.sNummer = _uiTbNummer.Text.Trim();
            }



            return m;
        }

        internal override void CheckDeleteButton()
        {
            if (Datenverwaltung.GetSearchResultsFromTable("Modell_mod").Count == 0)
            {
                uiBtLoeschen.IsEnabled = true;
            }
            else
            {
                uiBtLoeschen.IsEnabled = false;
            }
        }
    }
}
