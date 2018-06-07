using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App4
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void pick_Click(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
        }

        async private System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");

            var file = await openPicker.PickSingleFileAsync();

            // mediaPlayer is a MediaElement defined in XAML
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                player.SetSource(stream, file.ContentType);

            }
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            EllStoryboard.Begin();
            player.Play();
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            player.Pause();
            EllStoryboard.Pause();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            EllStoryboard.Stop();
        }

        private void fullWindow_Click(object sender, RoutedEventArgs e)
        {
            player.IsFullWindow = !player.IsFullWindow;
        }

        private void player_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            player.IsFullWindow = !player.IsFullWindow;
        }

        private void player_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
            if (!player.IsAudioOnly)
            {
                player.Visibility = Visibility.Visible;
                ellipse.Visibility = Visibility.Collapsed;
            }
        }


    }

    class MusicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((TimeSpan)value).TotalSeconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.FromSeconds((double)value);
        }
    }
}
