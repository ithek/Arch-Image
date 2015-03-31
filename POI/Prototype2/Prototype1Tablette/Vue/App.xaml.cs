using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Prototype1Tablette.VueModeles;

namespace Prototype1Tablette
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private VueModele vm;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            vm = new VueModele();
            vm.lancement(false);

            /*if (vm.connexion.presenceTabChef())
            {
                vm.lancement(false);
            }
            else
            {
                var window = new Selection();
                window.DataContext = vm;
                App.Current.MainWindow = window;
                window.Show();
            }*/

        }

        protected override void OnExit(ExitEventArgs e)
        {
            //vm.connexion.stopClient();
            
            base.OnExit(e);
        }
    }
}
