using Modele_Controleur;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Vue
{
    class ExceptionManager
    {
        public static void DirectoryNotFound(DirectoryNotFoundException ex)
        {
            MessageBox.Show(ex.Message);
        }
        public static void EmptyBook(FileNotFoundException ex)
        {
            MessageBox.Show(ex.Message);
        }
        public static void SaveNotFound(FileNotFoundException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
