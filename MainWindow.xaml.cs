using System;
using System.CodeDom;
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

namespace Scrabble
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private char[] _playerHand;
        public char[] playerHand
        {
            get { return _playerHand; }
            set
            {
                if (validWords != null) {
                    _playerHand = value;
                    updateWords(); }
            }
        }
        public string[] validWords;
        private DataExport export;
        public MainWindow()
        {
            InitializeComponent();
            playerHand = new char[7];
            export = new DataExport("export.txt");
            validWords = pullWordsFromWeb();
            //MessageBox.Show(validWords.Length.ToString());
            
            ///Sets the inital tiles, if needed.
            //txtInput.Text = "zzzzzzz";
            txtInput.Text = "eninej ";
        }

        private string[] pullWordsFromWeb()
        {
            List<string> Words = new List<string>();
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                System.IO.StreamReader sr = new System.IO.StreamReader(wc.OpenRead("http://darcy.rsgc.on.ca/ACES/ICS4U/SourceCode/Words.txt"));
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Length <= 7) { Words.Add(line); export.writeLine(line); }
                }
                sr.Close();
            }
            catch (Exception) { MessageBox.Show("Error reading words from web-based file"); throw; }

            //returns the list, converted to an array.
            return Words.ToArray();
        }

        private bool checkWord(string word)
        {
            for (int i = 0; i < 7; i++)
            {
                if (word.Contains(playerHand[i]))
                {
                    word = word.Remove(word.IndexOf(playerHand[i]), 1);
                }
                else
                {
                    break;
                }
            }
            if (word == "") { return true; }

            //returns false if all letters couldnt be removed by tiles
            return false;
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtInput.Text.Length == 7)
            {
                //copies the first seven letters of the input textbox to the player hand.
                playerHand = txtInput.Text.ToCharArray();
            }
        }

        private void updateWords()
        {
            DateTime timerStart = DateTime.Now;

            int validWordCounter = 0;
            string wordOut = "";

            for (int i = 0; i < validWords.Length; i++)
            {
                if (checkWord(validWords[i]))
                {
                    wordOut += validWords[i] + '\r';
                    validWordCounter++;
                }
            }
            TimeSpan timer = DateTime.Now - timerStart;

            lblOutput.Content = validWordCounter.ToString() + " playable words\rTime to run: " + timer.TotalMilliseconds + " ms\r" + wordOut;
            ///shows a second timer
            //MessageBox.Show("updateWords() time: " + (DateTime.Now - timerStart).TotalMilliseconds);
        }

        private void _menuOpenExport_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("export.txt");
        }
    }
}
