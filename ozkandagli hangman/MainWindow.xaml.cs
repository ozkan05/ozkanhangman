using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ozkandaglihangman
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadsound();
            startGame();
        }


        int vie;
        string GuessWord;
        char[] HiddenTab;
        string MotIntern;
        private MediaPlayer errorSound;
        private MediaPlayer correctSound;
        private MediaPlayer clickSound;
        public MediaPlayer winningSound;
        public MediaPlayer losingSound;
        public MediaPlayer music;

        public void startGame()
        {
            List<string> listMot = new List<string> { "vive" };
            using (var MyFile = File.OpenText("../../Assets/mot.txt")) // mise en place du dictionaire
            {
                string txtcontent = MyFile.ReadLine();
                int linenb = 0;
                do
                {
                    txtcontent = MyFile.ReadLine();
                    listMot.Add(txtcontent); // ajout des mots dans la liste
                    linenb++;
                } while (txtcontent != null);
            }

            Random rand = new Random(); //choix du mot
            int i = rand.Next(listMot.Count);
            GuessWord = listMot[i].ToUpper();
            //initalisation du mot, des vie et de l'image
            MotIntern = new string('*', GuessWord.Length);
            TB_Display.Text = MotIntern;
            vie = 0;
            activateBtn();
            Uri ressource = new Uri("Assets/character/character_empty.png", UriKind.Relative);
            ImgPendu.Source = new BitmapImage(ressource);
        }

        //event click sur les boutons
        private void BTN_Click(object sender, RoutedEventArgs e)
        {
            clickSound.Play();
            clickSound.Position = TimeSpan.Zero;
            Button btn = sender as Button;
            string lett = btn.Name.ToString();
            btn.IsEnabled = false;
            int index = 0;

            if (GuessWord.Contains(lett))
            {

                foreach (var l in GuessWord)
                {
                    if (l.ToString() == lett)
                    {
                        MotIntern = MotIntern.Remove(index, 1).Insert(index, lett);
                        TB_Display.Text = MotIntern;
                    }
                    index++;
                    correctSound.Play();
                    correctSound.Position = TimeSpan.Zero;
                }
                if (GuessWord == MotIntern) // si le joueur a gagné
                {
                    Uri ressource = new Uri("Assets/character/win.png", UriKind.Relative);
                    ImgPendu.Source = new BitmapImage(ressource);
                    winningSound.Play();
                    MessageBox.Show("Vous avez gagné !");
                    startGame();

                }

            }
            else //changement de charactere si le joueur a faux
            {
                errorSound.Play();
                errorSound.Position = TimeSpan.Zero;
                vie++;
                Uri ressource = new Uri("Assets/character/character_" + vie + ".png", UriKind.Relative);
                ImgPendu.Source = new BitmapImage(ressource);
            }
            if (vie == 6) // si le joueur a perdu
            {
                Uri ressource = new Uri("Assets/character/lose.png", UriKind.Relative);
                ImgPendu.Source = new BitmapImage(ressource);
                LosingEffect();
            }
        }
        //fonction pour charger les sons

        public void loadsound()
        {
            errorSound = new MediaPlayer();
            errorSound.Open(new Uri("Assets/song/error_008.mp3", UriKind.Relative));
            correctSound = new MediaPlayer();
            correctSound.Open(new Uri("Assets/song/confirmation_004.mp3", UriKind.Relative));
            clickSound = new MediaPlayer();
            clickSound.Open(new Uri("Assets/song/click_2.mp3", UriKind.Relative));
            winningSound = new MediaPlayer();
            winningSound.Open(new Uri("Assets/song/winning.mp3", UriKind.Relative));
            losingSound = new MediaPlayer();
            losingSound.Open(new Uri("Assets/song/lose.mp3", UriKind.Relative));
        }

        private async void LosingEffect()
        {
            losingSound.MediaEnded += (s, e) => { losingSound.Position = TimeSpan.Zero; };
            losingSound.Play();
            await Task.Delay(2500);
            MessageBox.Show("Vous avez perdu !");
        }

        //fonction pour reactiver les boutons
        public void activateBtn()
        {
            foreach (var elm in Grd_Keypad.Children)
            {
                Button btn = elm as Button;

                btn.IsEnabled = true;
            }
        }
    }
}