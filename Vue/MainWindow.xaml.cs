using Modele_Controleur;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MahApps.Metro.Controls;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Vue
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ArchImage archimage;

        public MainWindow()
        {
            InitializeComponent();
            this.archimage = new ArchImage();
            this.Content = new MainMenuPage(this.archimage);
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            if (archimage.DocumentCourant != null)
            {
                //Opens a file and serializes the object into it in binary format.
                Stream stream = File.Open(ArchImage.PATH_TO_SESSION_SAVE, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    formatter.Serialize(stream, archimage.DocumentCourant);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                stream.Close();
            }
        }
    }
}
