using MusicManager.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MusicManager.View
{
    /// <summary>
    /// Interaction logic for MusicPlayerView.xaml
    /// </summary>
    public partial class MusicPlayerView : UserControl
    {
        private DispatcherTimer dispatcherTimer;
        public MusicPlayerView()
        {
            InitializeComponent();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Tick += TimerTickMedia;
            App.InvokePlayMusic += InitSong;
            App.DiscloseMediaPlayer += DiscloseMediaPlayer;
        }

        private void TimerTickMedia(object? sender, EventArgs e)
        {
            if(_MediaPlayer != null)
            {
                sliderMusic.Value = _MediaPlayer.Position.TotalSeconds;

            }
        }

        private void InitSong(Song song)
        {
            App.DiscloseMediaPlayer?.Invoke();

            BindingSongToUI(song);
            _MediaPlayer = new MediaPlayer();
            _MediaPlayer.MediaOpened += TrackingMusic;
            _MediaPlayer.Open(new Uri(song.SongURL));
            IsPlaying = true;
        }

        public  void DiscloseMediaPlayer()
        {
            if (_MediaPlayer != null)
            {
                dispatcherTimer.Stop();
                _MediaPlayer.Pause();
                _MediaPlayer.Close();
            }
        }

        private bool isPlaying;
        private bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
            set
            {
                isPlaying = value;
                if(isPlaying)
                {
                    _MediaPlayer?.Play();
                    symbolPlay.Symbol = Wpf.Ui.Common.SymbolRegular.Pause16;
                }
                else
                {
                    _MediaPlayer?.Pause();
                    symbolPlay.Symbol = Wpf.Ui.Common.SymbolRegular.Play16;

                }
            }
        }
        
        private void TrackingMusic(object? sender, EventArgs e)
        {
            if(_MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                sliderMusic.Maximum = _MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                dispatcherTimer.Start();
                sliderMusic.Value = 0;
            }
        }

        private void BindingSongToUI(Song song)
        {
            txbActorname.Text = string.Join(", ", song.Artists.Select(p => p.Name));
            txbMusicname.Text = song.Title;
        }

        private MediaPlayer _mediaPlayer;
        private MediaPlayer _MediaPlayer
        {
            set { _mediaPlayer = value; }
            get { return _mediaPlayer; }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(_MediaPlayer == null)
            {
                MessageBox.Show($"Song selecteed is null");
                return;
            }
            IsPlaying = !IsPlaying;
                
        }

        private void btnMute_Click(object sender, RoutedEventArgs e)
        {
            if(_MediaPlayer == null)
            {
                return;
            }
            if(_MediaPlayer.IsMuted == false)
            {
                _MediaPlayer.IsMuted = true;
                symbolMute.Symbol = Wpf.Ui.Common.SymbolRegular.SpeakerMute28;
            }
            else
            {
                _MediaPlayer.IsMuted = false;
                symbolMute.Symbol = Wpf.Ui.Common.SymbolRegular.Speaker220;
            }
        }

        private void sliderMusic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(_MediaPlayer != null)
            {
                _MediaPlayer.Position = TimeSpan.FromSeconds(sliderMusic.Value); 

            }
        }
    }
}
