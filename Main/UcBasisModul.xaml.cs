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
    /// Interaktionslogik für UcBasisModul.xaml
    /// </summary>
    public abstract partial class UcBasisModul : UserControl
    {
        internal Tabelle currentRecord = null;

        public string tabellenName;

        public event Action<string> SelectedObjekt;

        public event Action DisplayDefault;

        internal enum OpenState { closed, editRow, newRow }

        internal OpenState status = OpenState.editRow;

        private LinearGradientBrush notSelected = new LinearGradientBrush();

        private LinearGradientBrush selected = new LinearGradientBrush();

        internal Dictionary<string, DataGridComboBoxColumn> dataGridComboBoxes = new Dictionary<string, DataGridComboBoxColumn>();

        internal Dictionary<string, ComboBox> comboBoxes = new Dictionary<string, ComboBox>();

        private bool _resultModul;

        private bool _halfSize;

        public UcBasisModul(string modulName, string tabellenName, bool halfSize,  bool resultModul)
        {
            InitializeComponent();
            _resultModul = resultModul;

            ChangeModul();

            

            uiLbModulName.Content = modulName;
            this.tabellenName = tabellenName;

            _halfSize = halfSize;

            if (_halfSize)
            {
                uiLbModulName.FontSize = 20;
                uiLbModulName.FontWeight = FontWeights.Medium;
            }

            SetVerticalGridLines();

            SetDgColumns();
            SetTextFields();
            SetComboBoxes();

            CloseUiGrEdit();
            status = OpenState.closed;


            uiDgObjekte.ItemsSource = Datenverwaltung.GetSearchResultsFromTable(tabellenName);

        }

        private void ChangeModul()
        {
            if (_resultModul)
            {
                uiGrHeader.Children.Remove(uiBtAll);
                //uiBoModul.CornerRadius = new CornerRadius(0);
                //uiStatusLeiste.CornerRadius = new CornerRadius(0);

                notSelected.GradientStops.Add(new GradientStop(Color.FromRgb(143, 188, 255), 0.0));
                selected.GradientStops.Add(new GradientStop(Color.FromRgb(143, 188, 255), 0.0));
                //uiBoModul.Background = Brushes.LightBlue;
            }
            else
            {
                notSelected.GradientStops.Add(new GradientStop(Color.FromRgb(184, 192, 204), 0.0));
                selected.GradientStops.Add(new GradientStop(Color.FromRgb(214, 255, 216), 0.0));
            }
        }


        private void SetComboBoxes()
        {
            foreach (var dataGridComboBox in dataGridComboBoxes)
            {
                dataGridComboBox.Value.ItemsSource = Datenverwaltung.GetAllRecordsFromTable(dataGridComboBox.Key);
            }

            foreach (var comboBox in comboBoxes)
            {
                comboBox.Value.ItemsSource = Datenverwaltung.GetAllRecordsFromTable(comboBox.Key);
            }
        }

        public void ChangeButton()
        {

            if (Datenverwaltung.GetSelectedObjects(tabellenName).Count == 0 && status != OpenState.newRow)
            {
                uiBtAnzeigen.IsEnabled = false;
                uiBtLoeschen.IsEnabled = false;
                
                //uiStatusLeiste.Background = notSelected;
            }
            else if (Datenverwaltung.GetSelectedObjects(tabellenName).Count == 1)
            {
                CheckDeleteButton();
                uiBtAnzeigen.IsEnabled = true;

                //uiStatusLeiste.Background = selected;
            }
            else if(Datenverwaltung.GetSelectedObjects(tabellenName).Count > 1)
            {
                CheckDeleteButton();
                uiBtAnzeigen.IsEnabled = false;

                //uiStatusLeiste.Background = selected;
            }
        }

        private void SetVerticalGridLines()
        {
            SetHalfVerticalGridLines();
            if (!_halfSize)
            {
                uiGrTextBoxes.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(35, GridUnitType.Pixel) });
                SetHalfVerticalGridLines();
            }
        }

        private void SetHalfVerticalGridLines()
        {
            uiGrTextBoxes.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(95, GridUnitType.Pixel) });
            uiGrTextBoxes.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }

        public void AddHorizontalGridLine()
        {
            uiGrTextBoxes.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Pixel) });
        }

        public void CloseUiGrEdit()
        {
            uiBtNeu.IsEnabled = true;
            uiBtAnzeigen.Content = "Bearbeiten";
            uiGrAnzeige.Height = 0;
            status = OpenState.closed;
            Object2Ui(null);
            ChangeButton();
        }

        private void OpenUiGrEdit()
        {
            uiGrAnzeige.Height = Double.NaN;
            uiBtAnzeigen.Content = "Zurück";
        }

        private void OpenUiGrEditRow()
        {
            OpenUiGrEdit();
            uiBtSafe.Content = "Ändern";
            status = OpenState.editRow;
            ChangeButton();
        }

        private void OpenUiGrEditNew()
        {
            OpenUiGrEdit();
            uiBtAnzeigen.IsEnabled = true;
            uiBtSafe.Content = "Neu Erstellen";
            uiBtNeu.IsEnabled = false;
            status = OpenState.newRow;
            ChangeButton();
        }

        private void UiDgArtikel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CheckDiffrentSelection())
            {
                StartSearch();
            }
        }

        private bool CheckDiffrentSelection()
        {
            if (Datenverwaltung.GetSelectedObjects(tabellenName).Count == uiDgObjekte.SelectedItems.Count)
            {
                if (uiDgObjekte.SelectedItems.Count == 0)
                {
                    return false;
                }

                foreach (var oldSelectedRecord in Datenverwaltung.GetSelectedObjects(tabellenName))
                {
                    if (uiDgObjekte.SelectedItems.Contains(oldSelectedRecord))
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        private void UiBtAll_Click(object sender, RoutedEventArgs e)
        {
            Datenverwaltung.GesamteAuswahlLoeschen();
            DisplayDefault?.Invoke();
        }

        public void AlleAnzeigen()
        {
            uiDgObjekte.SelectedItem = null;
            DisplaySearchResult();
        }

        private void UiBtAuswahlLoeschen_Click(object sender, RoutedEventArgs e)
        {
            uiDgObjekte.SelectedItem = null;
            Datenverwaltung.SetSelectedObjekte(null, tabellenName);
            SelectedObjekt?.Invoke(tabellenName);
            CloseUiGrEdit();
            uiDgObjekte.Items.Refresh();
        }

        private void UiBtAnzeigen_Click(object sender, RoutedEventArgs e)
        {
            if(status == OpenState.closed)
            {
                OpenUiGrEditRow();
                Object2Ui(uiDgObjekte.SelectedItem as Tabelle);
            }else if(status == OpenState.newRow || status == OpenState.editRow)
            {
                CloseUiGrEdit();
            }
        }

        private void UiBtNeu_Click(object sender, RoutedEventArgs e)
        {
            if(uiDgObjekte.SelectedItems.Count > 0)
            {
                uiDgObjekte.SelectedItem = null;
                Datenverwaltung.SetSelectedObjekte(null, tabellenName);
                SelectedObjekt?.Invoke(tabellenName);
                ChangeButton();
            }
            OpenUiGrEditNew();
        }

        private void UiBtLoeschen_Click(object sender, RoutedEventArgs e)
        {
            string meldung;

            if(uiDgObjekte.SelectedItems.Count > 1)
            {
                meldung = "Datensätze löschen";
            }
            else
            {
                meldung = "Datensatz löschen";
            }

            if(MessageBox.Show(meldung, "Überprüfung", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
            {
                return;
            }

            if (!Datenverwaltung.DeleteRecordFromTable(tabellenName, out string errmsg))
            {
                MessageBox.Show(errmsg, "DB-Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                uiDgObjekte.SelectedItem = null;
                Datenverwaltung.SetSelectedObjekte(null, tabellenName);
                SelectedObjekt?.Invoke(tabellenName);
                CloseUiGrEdit();
                uiDgObjekte.Items.Refresh();
            }
        }

        public abstract void Object2Ui(Tabelle t);

        

        public abstract void SetDgColumns();

        internal abstract void CheckDeleteButton();

        public void DisplaySearchResult()
        {
            foreach (var column in uiDgObjekte.Columns)
            {
                column.SortDirection = null;// System.ComponentModel.ListSortDirection.;
            }

            uiDgObjekte.ItemsSource = null;
            uiDgObjekte.ItemsSource = Datenverwaltung.GetSearchResultsFromTable(tabellenName);

            uiDgObjekte.Items.Refresh();
            
            uiDgObjekte.Items.Refresh();
            CloseUiGrEdit();
            uiDgObjekte.SelectedItems.Clear();
            foreach (var record in Datenverwaltung.GetSelectedObjects(tabellenName))
            {
                uiDgObjekte.SelectedItems.Add(record);
            }
            
        }

        public abstract void SetTextFields();

        private void UiDgObjekte_KeyUp(object sender, KeyEventArgs e)
        {
            if (CheckDiffrentSelection())
            {
                StartSearch();
            }
        }

        private void StartSearch()
        {

           // if (!_resultModul)
            //{
                Datenverwaltung.SetSelectedObjekte(uiDgObjekte.SelectedItems, tabellenName);
                SelectedObjekt?.Invoke(tabellenName);
            //}


                if (status == OpenState.editRow && uiDgObjekte.SelectedItems.Count == 1)
                {
                    Object2Ui(uiDgObjekte.SelectedItem as Tabelle);
                }
                else
                {
                    CloseUiGrEdit();
                }
            
            
        }

        private void UiBtSafe_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Speichern();
        }


        public void Speichern()
        {


            Tabelle record = Ui2Object(uiDgObjekte.SelectedItem as Tabelle);
            if (record == null)
            {
                return;
            }


            // ---------- Hier kommt das Speichern in der DB ---------------
            
            if (!Datenverwaltung.SafeObjectInDb(record, tabellenName, out string fehler))
            {
                MessageBox.Show(fehler, "DB-Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }





            if (status == OpenState.editRow)
            {
                uiDgObjekte.Items.Refresh();
                Object2Ui(uiDgObjekte.SelectedItem as Tabelle);
            }
            else
            {
                Datenverwaltung.AddRecord(record, tabellenName);
                uiDgObjekte.Items.Refresh();
                CloseUiGrEdit();
                uiDgObjekte.SelectedItem = record;
                OpenUiGrEditRow();
                Object2Ui(uiDgObjekte.SelectedItem as Tabelle);
                
                StartSearch();
                ChangeButton();
            }

            SelectedObjekt?.Invoke(tabellenName);
        }


        public abstract Tabelle Ui2Object(Tabelle record);



    }
}



