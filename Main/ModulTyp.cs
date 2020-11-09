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
    class ModulTyp : UcBasisModul
    {
        private TextBox _uiTbBezeichnung;

        public ModulTyp()
            : base("Typ", "Typ_typ", true, false)
        {

        }



        public override void Object2Ui(Tabelle t)
        {


            if (t != null)
            {
                Typ_typ modell = t as Typ_typ;
                _uiTbBezeichnung.Text = modell.sBezeichnung;
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
            Label uiLbBezeichnung = new Label() { Content = "Bezeichnung", Margin = new Thickness(5, 10, 5, 10) };
            Grid.SetRow(uiLbBezeichnung, 0);
            uiGrTextBoxes.Children.Add(uiLbBezeichnung);

            _uiTbBezeichnung = new TextBox() { Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center, MaxLength= 30 };
            Grid.SetColumn(_uiTbBezeichnung, 1);
            Grid.SetRow(_uiTbBezeichnung, 0);
            uiGrTextBoxes.Children.Add(_uiTbBezeichnung);

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

        public override Tabelle Ui2Object(Tabelle record)
        {
            Typ_typ t;
            if (record == null)
            {
                t = new Typ_typ();
            }
            else
            {
                t = record as Typ_typ;
            }


            if (_uiTbBezeichnung.Text.Length == 0)
            {
                _uiTbBezeichnung.Focus();
                MessageBox.Show("Bezeichnung darf nicht leer sein!", "Fehlermeldung");
                return null;
                
            }
            else
            {
                string bez = _uiTbBezeichnung.Text;
                foreach (Typ_typ item in Datenverwaltung.GetAllRecordsFromTable("Typ_typ"))
                {
                    if (item.sBezeichnung == bez)
                    {
                        MessageBox.Show("Bezeichnung muss eindeutig sein", "Fehlermeldung");
                        return null;
                    }
                }
                t.sBezeichnung = _uiTbBezeichnung.Text;

            }



            return t;
        }
    }
}
