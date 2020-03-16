using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RegistryUtils;
using Microsoft.Win32;
using System.Diagnostics;
using iTunesLib;

namespace LyricsFinder
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public string var_Song, artist, song, song_key;
        private System.Windows.Threading.DispatcherTimer TimerClock;

        public Window1()
        {
            InitializeComponent();
            #region Code for the Timer event
            TimerClock = new System.Windows.Threading.DispatcherTimer();
            TimerClock.Interval = new TimeSpan(0, 0, 5);
            TimerClock.IsEnabled = true;
            TimerClock.Tick += new EventHandler(TimerClock_Tick);
            #endregion
        }

        void TimerClock_Tick(object sender, EventArgs e)
        {
            if ((IsProcessOpen("iTunes")) && (IsProcessOpen("Zune")))
            {
                MessageBox.Show("Zune & iTunes both are open. Please exit any one player and try again");
                return;
            }
            else
                if (IsProcessOpen("iTunes") == true)
                {

                    //lblApp.Content = "Itunes";

                    Find_iTunes();

                }
                else if (IsProcessOpen("Zune") == true)
                {
                    if (IsProcessOpen("ZuneNowPlaying") == false)
                    {

                        Process.Start("ZuneNowPlaying.exe");
                        //lblApp.Content = "Zune";
                        Find_Zune();

                    }
                    else
                    {

                        Find_Zune();
                    }
                }

        }

        private void Find_Zune()
        {
             RegistryKey regkey;
            int status;
            try
            {
            regkey = Registry.CurrentUser.OpenSubKey(@"Software\ZuneNowPlaying");    
            lblSong.Content = regkey.GetValue("Title") as string;
            lblArtist.Content = regkey.GetValue("Artist") as string;
            //txtLyrics.Text = "";
            
                    
                    status = (int)regkey.GetValue("Playing");
                    // Do nothing if no track is being played
                    if ((status == 0))
                    {
                        regkey.Close();
                     
                        return;
                    }
                    if (var_Song == lblSong.Content.ToString())
                    {
                        //MessageBox.Show("Doing nothing");
                        return;
                    }

                    
                   
                    artist = regkey.GetValue("Artist") as string;
                    song = regkey.GetValue("Title") as string;
                    var_Song = lblSong.Content.ToString();
                    Check_XML();
                   
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "LyricsFinder", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }

        private void Find_iTunes()
        {
            try
            {

                iTunesApp iTSong = new iTunesLib.iTunesApp();
                lblArtist.Content = iTSong.CurrentTrack.Artist;
                lblSong.Content = iTSong.CurrentTrack.Name;
                if (var_Song == lblSong.Content.ToString())
                {
                    //MessageBox.Show("Doing nothing");
                    return;
                }

                var_Song = lblSong.Content.ToString();
                Check_XML();
            }
            catch (NullReferenceException)
            {
                txtLyrics.Text = "No Song is playing";
                lblArtist.Content = "Artist";
                lblSong.Content = "Song";
            }
            

        }

        private void Check_XML()
        {


            string UR;
            UR = "http://api.leoslyrics.com/api_search.php?auth=clatonh&artist=" + lblArtist.Content.ToString() + "&songtitle=" + lblSong.Content.ToString();
            //MessageBox.Show(UR);
            //txtLyrics.Text = UR;


            XmlTextReader xmlread = new XmlTextReader(UR);
            xmlread.ReadToFollowing("result");
            string ss = xmlread.GetAttribute("exactMatch");
            //MessageBox.Show(ss);
            if (ss == "true")
            {
                //MessageBox.Show("Here is where I start to search for data");
                song_key = xmlread.GetAttribute("hid");
                //MessageBox.Show(song_key);
                Get_XML_Lyrics();
                    
            }
            else
            {
                //MessageBox.Show("we dont search");
                txtLyrics.Text = "Lyrics not found in Database";
            }
        }

        private void Get_XML_Lyrics()
        {
            string U = "http://api.leoslyrics.com/api_lyrics.php?auth=clatonh&hid=" + song_key;
            XmlTextReader xmlread1 = new XmlTextReader(U);

            while (xmlread1.Read())
            {
                XmlNodeType nType = xmlread1.NodeType;
                if (nType == XmlNodeType.Element)
                {
                    if (xmlread1.Name == "text")
                    {
                        txtLyrics.Text = xmlread1.ReadString();
                    }
                    
                }
            }
        }

        public bool IsProcessOpen(string name)
        {

            foreach (Process clsProcess in Process.GetProcesses())
            {
                Debug.WriteLine(clsProcess.Id.ToString());

                if (clsProcess.ProcessName == name)
                {
                    //Debug.WriteLine(clsProcess.ProcessName.ToString());
                    //MessageBox.Show(clsProcess.ProcessName.ToString());
                    return true;

                }
            }
            //otherwise we return a false
            return false;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void chkTop_Click(object sender, RoutedEventArgs e)
        {
            if (chkTop.IsChecked == true)
            {
                this.Topmost = true;

            }
            else
            {
                this.Topmost = false;
            }
        } 

            

    }
}
