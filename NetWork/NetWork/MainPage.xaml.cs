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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static NetWork.WeatherFromXml;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace NetWork
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var position = await LocationManager.GetPosition();
            RootObject myWeather = await OpenWeatherMapProxy.GetWeather(position.Coordinate.Point.Position.Latitude,
                position.Coordinate.Point.Position.Longitude);

            string icon = string.Format("ms-appx:///Assets/Weather/{0}.png", myWeather.weather[0].icon);
            ResultImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
            ResultTextBlock.Text = myWeather.name + " - " + myWeather.main.temp + "℃ - " + myWeather.weather[0].description;
            ResultTextBlock.Text += " - " + myWeather.sys.country + " - " + myWeather.coord.lat + " - " + myWeather.coord.lon;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RootObject myWeather = await OpenWeatherMapProxy.GetWeather(locationName.Text);

            string icon = string.Format("ms-appx:///Assets/Weather/{0}.png", myWeather.weather[0].icon);
            ResultImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
            ResultTextBlock.Text = myWeather.name + " - " + myWeather.main.temp + "℃ - " + myWeather.weather[0].description;
            ResultTextBlock.Text += " - " + myWeather.sys.country + " - " + myWeather.coord.lat + " - " + myWeather.coord.lon;
                        
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var position = await LocationManager.GetPosition();
            Current weather = await GetWeather(position.Coordinate.Point.Position.Latitude,
                position.Coordinate.Point.Position.Longitude);
            string icon = string.Format("ms-appx:///Assets/Weather/{0}.png", weather.Weather.Icon);
            JsonImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
            JsonBlock.Text = weather.City.Name + " - " + weather.City.Country + " - " + weather.Temperature.Value + "℃ ";
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Current weather = await GetWeather(JsonBox.Text);
            string icon = string.Format("ms-appx:///Assets/Weather/{0}.png", weather.Weather.Icon);
            JsonImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
            JsonBlock.Text = weather.City.Name + " - " + weather.City.Country + " - " + weather.Temperature.Value + "℃ ";
        }
    }
}
