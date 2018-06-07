using App2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace App2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        public ObservableCollection<Work> Works;

        public NewPage()
        {
            this.InitializeComponent();
            Works = Curry.Get_instance();

        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "提示",
                Content= "",
                PrimaryButtonText = "确定",
                FullSizeDesired = false,
            };
            if (Button1.Content.ToString() == "Update")
            {
                dialog.Content = "修改成功";

            }
            else
            {
                if (TextBox1.Text == "")
                    dialog.Content = "Title 不能为空\n";
                if (TextBox2.Text == "")
                    dialog.Content += "Detail 不能为空\n";
                if (DatePicker1.Date < DateTime.Now.Date)
                    dialog.Content += "Due Date 不能早于当前日期\n";
                if (dialog.Content.ToString() == "")
                {
                    dialog.Content = "创建成功";
                    Curry.Get_instance().Add(new Work(TextBox1.Text, TextBox2.Text, DatePicker1.Date, picture.Source));
                }
            }
            await dialog.ShowAsync();

            Frame root = Window.Current.Content as Frame;
            root.Navigate(typeof(MainPage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
            DatePicker1.Date = DateTime.Now.Date;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                IRandomAccessStream ir = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage bi = new BitmapImage();
                await bi.SetSourceAsync(ir);
                picture.Source = bi;
            }
        }

        private void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Works.Remove((Work)this.DataContext);
            Frame root = Window.Current.Content as Frame;

            root.Navigate(typeof(MainPage));
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.update = false;
            Frame root = Window.Current.Content as Frame;

            root.Navigate(typeof(NewPage));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            bool suspending = ((App)App.Current).isSuspending;
            if (suspending)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["title"] = TextBox1.Text;
                composite["detail"] = TextBox2.Text;
                composite["date"] = DatePicker1.Date;
                //composite["image"] = Con.SaveToBytesAsync(picture.Source).Result;
                int[] s = new int[2];
                int i = 0;
                foreach (Work item in Works)
                {
                    if (item.IsChecked)
                    {
                        s[i] = 1;
                    }
                    else
                        s[i] = 0;
                    ++i;
                }
                composite["isChecked"] = s;
                
                ApplicationData.Current.LocalSettings.Values["newPage"] = composite;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                this.DataContext = e.Parameter;
                if (MainPage.update)
                    Button1.Content = "Update";
                else
                    Button1.Content = "Create";
                ApplicationData.Current.LocalSettings.Values.Remove("newPage");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("newPage"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["newPage"] as ApplicationDataCompositeValue;
                    TextBox1.Text = (string)composite["title"];
                    TextBox2.Text = (string)composite["detail"];
                    DatePicker1.Date = (DateTimeOffset)composite["date"];
                    //picture.Source = Con.SaveToImageSource((byte[])composite["image"]).Result;
                    int[] s = (int[])composite["isChecked"];
                    for (int i = 0; i < s.Length; ++i)
                        if (s[i] == 1)
                            Works[i].IsChecked = true;
                    ApplicationData.Current.LocalSettings.Values.Remove("newPage");
                }
            }
        }

        
    }
}
