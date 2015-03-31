using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Prototype1TBI.VueModeles;

namespace Prototype1TBI
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
            /*window.DataContext = vm;
            window.Show();*/
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //vm.connexion.stopClient();

            base.OnExit(e);
        }
    }
}
