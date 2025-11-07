using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ozkandaglihangman
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadSound();
            StartGame();
        }

        int vie;
        string GuessWord;
        string MotIntern;
        private MediaPlayer errorSound;
        private MediaPlayer correctSound;
        private MediaPlayer clickSound;
        public MediaPlayer winningSound;
        public MediaPlayer losingSound;

        public void StartGame()
        {
            // 🔁 Réinitialisation
            vie = 0;
            GuessWord = "";
            MotIntern = "";

            // 🔤 Liste de mots intégrée
            List<string> listMot = new List<string>
            {
                "noble","connu","ordre","abime","zeste","zonal","table","envie","façon","fleur",
                "amour","poule","pièce","globe","arbre","ruban","ouvre","heure","clair","coure",
                "banal","sirop","pendu","melon","suite","ruche","quand","nager","poche","salle",
                "corne","foule","haine","piste","vague","terre","jouer","bijou","vieux","donne",
                "garde","chaud","votre","usine","nièce","herbe","xenon","valet","boule","wagon",
                "lourd","ombre","union","verre","gros","ferme","aimer","klaxon","quasi","force",
                "navet","rival","gazon","filin","mouet","soupe","faute","petit","clown","reine",
                "beret","durée","zèbre","mettre","livre","neige"
            };

            // 🎲 Choix aléatoire du mot
            Random rand = new Random();
            int i = rand.Next(listMot.Count);
            GuessWord = listMot[i].ToUpper();

            // 🔠 Mot masqué
            MotIntern = new string('*', GuessWord.Length);
            TB_Display.Text = MotIntern;

            // 🖼️ Image initiale
            Uri ressource = new Uri("Assets/character/character_empty.png", UriKind.Relative);
            ImgPendu.Source = new BitmapImage(ressource);

            // ✅ Réactivation des boutons
            ActivateBtn();
        }

        private void BTN_Click(object sender, RoutedEventArgs e)
        {
            clickSound.Position = TimeSpan.Zero;
            clickSound.Play();

            Button btn = sender as Button;
            string lett = btn.Name.ToUpper();
            btn.IsEnabled = false;

            bool correct = false;

            // 🔍 Vérification de la lettre
            for (int i = 0; i < GuessWord.Length; i++)
            {
                if (GuessWord[i].ToString() == lett)
                {
                    MotIntern = MotIntern.Remove(i, 1).Insert(i, lett);
                    correct = true;
                }
            }

            TB_Display.Text = MotIntern;

            if (correct)
            {
                correctSound.Position = TimeSpan.Zero;
                correctSound.Play();

                // 🎉 Victoire
                if (MotIntern == GuessWord)
                {
                    Uri ressource = new Uri("Assets/character/win.png", UriKind.Relative);
                    ImgPendu.Source = new BitmapImage(ressource);
                    winningSound.Position = TimeSpan.Zero;
                    winningSound.Play();

                    MessageBox.Show("🎉 Vous avez gagné !");
                    StartGame();
                }
            }
            else
            {
                errorSound.Position = TimeSpan.Zero;
                errorSound.Play();
                vie++;

                if (vie <= 6)
                {
                    Uri ressource = new Uri($"Assets/character/character_{vie}.png", UriKind.Relative);
                    ImgPendu.Source = new BitmapImage(ressource);
                }

                // 💀 Défaite
                if (vie >= 6)
                {
                    Uri ressource = new Uri("Assets/character/lose.png", UriKind.Relative);
                    ImgPendu.Source = new BitmapImage(ressource);
                    LosingEffect();
                }
            }
        }

        private async void LosingEffect()
        {
            losingSound.Position = TimeSpan.Zero;
            losingSound.Play();

            await Task.Delay(2000);
            MessageBox.Show($"💀 Vous avez perdu ! Le mot était : {GuessWord}");
            StartGame();
        }

        public void LoadSound()
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

        public void ActivateBtn()
        {
            foreach (var elm in Grd_Keypad.Children)
            {
                if (elm is Button btn)
                    btn.IsEnabled = true;
            }
        }

       
    }
}
