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
using System.ComponentModel;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls.Primitives;
using System.Threading;

namespace Main
{
    public partial class MainWindow : Window
    {
        private ProMod _proMod;

        private EasyMod _easyMod;

        private enum modusSatus { easy, pro};
        private modusSatus status;


        public MainWindow()
        {
            Datenverwaltung.Initialize();
            InitializeComponent();

            UcDrucken druck = new UcDrucken();
            _proMod = new ProMod(druck);
            Grid.SetRow(_proMod, 1);

            _easyMod = new EasyMod(druck);
            Grid.SetRow(_easyMod, 1);
            _easyMod.AddDruck2Ui();
            uiGrMain.Children.Add(_easyMod);
            status = modusSatus.easy;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowTest win2 = new WindowTest();
            win2.Show();

            e.Handled = true;
            Datenverwaltung.GesamteAuswahlLoeschen();
            if (status == modusSatus.easy)
            {
                _easyMod.DisplaySearchResults();
                _easyMod.RemoveDruckFromUi();
                //uiGrMain.Children.Remove(_easyMod);

                _proMod.AddDruck2Ui();
                //uiGrMain.Children.Add(_proMod);
                //uiLbModus.Content = "Pro Mode";
                status = modusSatus.pro;

            }
            else
            {
                _proMod.DisplaySearchResults();
                _proMod.RemoveDruckFromUi();
                //uiGrMain.Children.Remove(_proMod);
                _easyMod.AddDruck2Ui();
                //uiGrMain.Children.Add(_easyMod);
                //uiLbModus.Content = "Easy Mode";
                status = modusSatus.easy;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string toDo = null;

            toDo += "# Simon Biedrowski\n";
            toDo += "# Florian Heider\n";
            toDo += "# Lukas Kant\n";
            

            MessageBox.Show(toDo, "Credits");
        }

        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
        }
            

        private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async Task MenuPopupButton_OnClickAsync(object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new OkMessageDialog
            {
                Message = { Text = ((ButtonBase)sender).Content.ToString() }
            };

            await DialogHost.Show(sampleMessageDialog, "RootDialog");
        }

        private void MenueRefresh(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Datenverwaltung.Initialize();
            MessageBox.Show("Refreshed", "Refresh", MessageBoxButton.OK);
        }

        private void SideMenueEasyMode(object sender, RoutedEventArgs e)
        {
            e.Handled = true;


        }

        private void SideMenueExpertMode(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

        }


        private void MainMenueRefresh(object sender, RoutedEventArgs e)
        {


            var progress = new Progress();
            DialogHost.Show(progress);

            Thread laden = new Thread(RefreshData);
            laden.Start();

            
        }


        public void LadenBeenden()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }





        private void RefreshData()
        {
            Datenverwaltung.Initialize();

            // Von der Stackoverflow
            // https://stackoverflow.com/questions/11625208/accessing-ui-main-thread-safely-in-wpf
            Application.Current.Dispatcher.Invoke(new Action(() => { DialogHost.CloseDialogCommand.Execute(null, null); }));
        }




        private async void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new OkMessageDialog
            {
                Message = { Text = ((ButtonBase)sender).Content.ToString() }
            };

            await DialogHost.Show(sampleMessageDialog, "RootDialog");
        }

        private void MainMenueSettings(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Settings");
        }

        private void MainMenueInfo(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Info");
        }

        private void MainMenueCredits(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Credits");
        }

        private void MainMenueExit(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
