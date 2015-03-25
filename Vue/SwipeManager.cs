using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modele_Controleur;
using System.Windows.Input;
using System.Windows;

namespace Vue
{
    class SwipeManager
    {
        /**
         * How many pixels before a movement is considered a swipe
         */
        private const int Threshold = 100;
        private TouchPoint TouchStart
        {
            get;
            set;
        }

        private ArchImage Archimage
        {
            get;
            set;
        }

        private NavigationPage NavPage
        {
            get;
            set;
        }

        private bool AlreadySwiped;

        public SwipeManager(ArchImage arch, NavigationPage page)
        {
            this.Archimage = arch;
            this.NavPage = page;
            this.AlreadySwiped = false;
        }

        public void TouchDown(object sender, TouchEventArgs e)
        {
            TouchStart = e.GetTouchPoint(this.NavPage);
        }

        public void TouchMove(object sender, TouchEventArgs e)
        {
            if (!AlreadySwiped)
            {                
                var Touch = e.GetTouchPoint(this.NavPage);

                //Swipe Left
                if (TouchStart != null && Touch.Position.X > (TouchStart.Position.X + Threshold))
                {
                    this.Archimage.DocumentPrecedent();
                    this.NavPage.UpdateUI();

                    AlreadySwiped = true;
                }

                //Swipe Right
                if (TouchStart != null && Touch.Position.X < (TouchStart.Position.X - Threshold))
                {
                    this.Archimage.DocumentSuivant();
                    this.NavPage.UpdateUI();

                    AlreadySwiped = true;
                }
            }
            e.Handled = true;
        }

        public void HasRealeasedTouch(){
            this.AlreadySwiped = false;
            this.TouchStart = null;
        }
    }
}
