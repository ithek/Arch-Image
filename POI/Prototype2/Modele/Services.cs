using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using msvipConnexionDLL.implementations;
using msvipConnexionDLL.interfaces;
using Commun;

namespace Modele
{
    public class Services
    {
        public static Services instance = new Services();
       
        

        private Services()
        {}


        //renvoie la liste des éléments contenu dans le dossier chemin
        public String[] getContenu(string chemin, Types t)
        {
            String[] fichiers;
            switch (t)
            {
                case (Types.image):
                    String[] fichiersjpg = Directory.GetFiles(chemin, "*.jpg");
                    String[] fichierspng = Directory.GetFiles(chemin, "*.png");
                    //on rend la somme des deux types de fichiers
                    fichiers = new String[fichiersjpg.Length + fichierspng.Length];
                    fichiersjpg.CopyTo(fichiers, 0);
                    fichierspng.CopyTo(fichiers, fichiersjpg.Length);
                    break;
                case (Types.video):
                    String[] fichiersmp4 = Directory.GetFiles(chemin, "*.mp4");
                    String[] fichiersavi = Directory.GetFiles(chemin, "*.avi");
                    //on rend la somme des deux types de fichiers
                    fichiers = new String[fichiersmp4.Length + fichiersavi.Length];
                    fichiersmp4.CopyTo(fichiers, 0);
                    fichiersavi.CopyTo(fichiers, fichiersmp4.Length);
                    break;
                case (Types.diaporama):
                    fichiers = Directory.GetDirectories(chemin, "*.diaporama");
                    break;
                case (Types.poi):
                    fichiers = Directory.GetDirectories(chemin, "*.poi");
                    break;
                case (Types.vitrine):
                    fichiers = Directory.GetDirectories(chemin, "*.vitrine");
                    break;
                default:
                    fichiers = null;
                    break;
            }
            return fichiers;
        }

        //Renvoie le VideoModele de la vidéo d'aide
        public VideoModele getVideoAide()
        {
            String chemin = "..\\..\\..\\Aide";
            String[] video = Directory.GetFiles(chemin, "AideVideo.*");
            VideoModele vm = null;
            if( video.Length == 1)
            {
                vm = new VideoModele(video[0]);
            }
            return vm;
        }

        //Renvoie le VideoModele de la vidéo d'aide
        public DiaporamaModele getDiapoAide()
        {
            String chemin = "..\\..\\..\\Aide";
            String[] video = Directory.GetDirectories(chemin, "aide.diaporama");
            DiaporamaModele vm = null;
            if (video.Length == 1)
            {
                vm = new DiaporamaModele(video[0]);
            }
            return vm;
        }

        //renvoie le chemin vers la carte de fond
        // -> peut être dans n'importe quel format d'image supporté par C#
        public String getCarteDeFond(String chemin)
        {
            String[] carte = Directory.GetFiles(chemin, "carte.*");
            return carte[0];
        }

        //Lire le fichier "position.pos" présent a l'adresse chemin pour réccupérer la position du poi
        public Point getPosition(string chemin)
        {
            int[] xy = new int[2];
            string line;
            int i = 0;
            System.IO.StreamReader file = new System.IO.StreamReader(chemin + "\\position.txt");

            while ((line = file.ReadLine()) != null)
            {
                xy[i] = Convert.ToInt32(line);
                i++;
            }
            file.Close();
            Point p = new Point(xy[0], xy[1]);
            return p;
        }

        public void creerDossier(string c)
        {
            if (!Directory.Exists(c))
                Directory.CreateDirectory(c);
        }

        public void renommerDossier(string c1,string c2)
        {
            if (c1 != c2)
            {
                if (Directory.Exists(c1))
                {
                    if (Directory.Exists(c2))
                    {
                        IEnumerable<string> fichiers = Directory.EnumerateFiles(c2);
                        foreach (string s in fichiers)
                        {
                            File.Delete(s);
                        }
                        Directory.Delete(c2);
                    }

                    Directory.Move(c1, c2);
                }
            }
        }

        public void creerFichier(string c)
        {
            if (!File.Exists(c))
            {
                using (FileStream fs = File.Create(c)){}
            }

        }

        public bool caractereValide(char c)
        {
            int numero = Convert.ToInt32(c);
            if ((c >= 97 && c <= 122) || (c >= 65 && c <= 90) || (c >= 48 && c <= 57) || c == 95)
                return true;
            else
                return false;
        }

        public void setPosition(Point p, double ratio, string chemin)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(chemin + "\\position.txt");
            file.WriteLine((int)(p.X/ratio));
            file.WriteLine((int)(p.Y/ratio));
            file.Close();
        }
    }
}
