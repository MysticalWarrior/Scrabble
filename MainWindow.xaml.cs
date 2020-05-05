using System;
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
        public char[] playerHand;
        public string[] validWords;
        public MainWindow()
        {
            InitializeComponent();
            playerHand = new char[7];
            validWords = pullDataFromWeb();
            MessageBox.Show(validWords.Length.ToString());

        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            int validWordCounter = 0;
            try
            {
                //copies the first seven letters of the input textbox to the player hand.
                playerHand = txtInput.Text.Substring(0, 7).ToCharArray();

                for (int i = 0; i < validWords.Length; i++)
                {
                    if (validWords[i].Contains(playerHand[0].ToString()))
                    {
                        lblOutput.Content += validWords[i] + '\r';
                        validWordCounter++;
                    }
                }
                lblOutput.Content = validWordCounter.ToString() + " playable words\r" + lblOutput.Content;
            }
            catch (Exception)
            {

                throw new Exception("unable to obtain tiles from input");
            }
        }

        private string[] pullDataFromWeb()
        {
            List<string> temp = new List<string>();
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                System.IO.StreamReader sr = new System.IO.StreamReader(wc.OpenRead("http://darcy.rsgc.on.ca/ACES/ICS4U/SourceCode/Words.txt"));
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    temp.Add(line);
                    //MessageBox.Show(line + " " + temp[(temp.Count() - 1)]);
                }
                sr.Close();
            }
            catch (Exception) { MessageBox.Show("Error reading words from web-based file"); throw; }

            return temp.ToArray();
        }
    }
}
