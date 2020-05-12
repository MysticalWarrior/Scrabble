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
        public DateTime timerStart;
        private char[] _playerHand;
        public char[] playerHand
        {
            get { return _playerHand; }
            set
            {
                if (validWords != null) {

                    timerStart = DateTime.Now;
                    _playerHand = value;
                    updateWords();
                    //MessageBox.Show("updateWords() time: " + (DateTime.Now - timerStart).TotalMilliseconds);
                }
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
            //MessageBox.Show("Dictionary length: " + validWords.Length.ToString());
            
            ///Sets the inital tiles, if needed.
            txtInput.Text = "ALLWORd";

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
                    //filters out words that are more than 7 characters long
                    if (line.Length <= 7)
                    {
                        Words.Add(line);
                        export.writeLine(line);
                    }
                }
            sr.Close();
            }
            catch (Exception) { MessageBox.Show("Error initializing dictionary"); }

            //returns the list, converted to an array.
            return Words.ToArray();
        }

        private bool checkWord(string wordtocheck)
        {
            string word = wordtocheck.ToUpper();

            int numberRemovedByWild = 0;
            for (int i = 0; i < 7; i++)
            {
                if (word.Length == 0) { break; }
                /*if (word[0] == char.ToUpper(playerHand[i]))
                {
                    word = word.Remove(0,1);
                }*/
                else if (word.Contains(playerHand[i]))
                {
                    word = word.Remove(word.IndexOf(playerHand[i]), 1);
                }
                else if (playerHand[i] == ' ')
                {
                    numberRemovedByWild++;
                }
            }
            if (word == "") { return true; }
            else if (word.Length <= numberRemovedByWild) { return true; }
            else { return false; }
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtInput.Text.Length == 7)
            {
                //copies the first seven letters of the input textbox to the player hand.
                playerHand = txtInput.Text.ToUpper().ToCharArray();
            }
        }

        private void updateWords()
        {
            //timerStart = DateTime.Now;
            int validWordCounter = 0;
            string wordsOut = "";

            
            for (int i = 0; i < validWords.Length; i++)
            {
                if (checkWord(validWords[i]))
                {
                    wordsOut += validWords[i] + '\r';
                    validWordCounter++;
                }
                //pBar.Value = i;
            }
            if (txtInput.Text == "ALLWORD")
            {
                validWordCounter = 0;
                wordsOut = "";
                for (int i = 0; i < validWords.Length; i++)
                {
                    wordsOut += validWords[i] + '\r';
                    validWordCounter++;
                }
            }
            lblOutput.Content = validWordCounter.ToString() + " playable words\rTime to run: " + (DateTime.Now - timerStart).TotalMilliseconds + " ms\r" + wordsOut;
            //lblOutput.Text = validWordCounter.ToString() + " playable words\rTime to run: " + (DateTime.Now - timerStart).TotalMilliseconds + " ms\r" + wordsOut;
        }

        private void _menuOpenExport_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("export.txt");
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            //debugging tool for reworking stackpanel
            MessageBox.Show("stackpanel height: " + spOutput.Height.ToString() + "\rstackpanel rendered height: " + spOutput.ActualHeight.ToString());
        }
    }
}