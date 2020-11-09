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
    class ModulGroesse : UcBasisModul
    {
        private TextBox _uiTbGroesse;

        public ModulGroesse()
            : base("Größe", "Groesse_gro", true, false)
        {

        }



        public override void Object2Ui(Tabelle t)
        {
            

            if (t != null)
            {
                Groesse_gro groesse = t as Groesse_gro;
                _uiTbGroesse.Text = groesse.rGroesse.ToString();
            }
            else
            {
                _uiTbGroesse.Clear();
            }
        }

        public override void SetDgColumns()
        {
            DataGridTextColumn groesse = new DataGridTextColumn();
            groesse.Width = new DataGridLength(50, DataGridLengthUnitType.Star);
            groesse.Header = "Größe";
            groesse.Binding = new Binding("rGroesse");
            uiDgObjekte.Columns.Add(groesse);
        }


        public override void SetTextFields()
        {
            AddHorizontalGridLine();
            uiGrTextBoxes.Children.Add(new Label() { Content = "Größe", Margin = new Thickness(5, 10, 5, 10) });

            _uiTbGroesse = new TextBox() { Text = "test", Margin = new Thickness(5, 10, 5, 10), VerticalContentAlignment = VerticalAlignment.Center };
            Grid.SetColumn(_uiTbGroesse, 1);

            uiGrTextBoxes.Children.Add(_uiTbGroesse);
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
            Groesse_gro g;
            if (record == null)
            {
                g = new Groesse_gro();
            }
            else
            {
                g = record as Groesse_gro;
            }


            decimal groesse;
            if (_uiTbGroesse.Text.Trim().Length == 0)
            {
                MessageBox.Show("Größe braucht einen Wert", "Eingabefehler");
                _uiTbGroesse.Focus();
                return null;
            }
            else if (!decimal.TryParse(_uiTbGroesse.Text.Trim(), out groesse))
            {
                MessageBox.Show("Eingabe ist keine Zahl", "Eingabefehler");
                _uiTbGroesse.Focus();
                return null;
            }
            else if ((int)Math.Round(groesse, 2, MidpointRounding.AwayFromZero) > 999)
            {
                MessageBox.Show("Groesse darf nur 3 Vorkommastellen haben", "Eingabefehler");
                _uiTbGroesse.Focus();
                return null;
            }
            else
            {
                decimal gr = groesse;
                foreach (Groesse_gro item in Datenverwaltung.GetAllRecordsFromTable("Groesse_gro"))
                {

                    if (item.rGroesse == groesse)
                    {
                        MessageBox.Show("Diese Größe gibt es schon", "Fehlermeldung");
                        return null;

                    }
                }
                g.rGroesse = Math.Round(groesse, 2, MidpointRounding.AwayFromZero);
            }


            return g;
        }
    }
}
