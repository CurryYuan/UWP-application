using App2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using App2.Service;
using SQLitePCL;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using System.Diagnostics;


//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace App2
{
    public sealed partial class MyUserControl1 : UserControl
    {
        public ObservableCollection<Work> Works;
        public MyUserControl1()
        {
            this.InitializeComponent();
            Works = Curry.Get_instance();
        }
        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            Work temp = new Work(TextBox1.Text, TextBox2.Text, DatePicker1.Date, picture.Source);
            var dialog = new ContentDialog()
            {
                Title = "提示",
                Content = "",
                PrimaryButtonText = "确定",
                FullSizeDesired = false,
            };
            if (Button1.Content.ToString() == "Update")
            {
                temp.Id = ((Work)DataContext).Id;

                DataService.UpdateDB(temp);
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
                    DataService.InsertDB(temp);

                    // Create the tile notification
                    TitleService titleService = new TitleService();
                    TitleService.Item = temp;
                    TileNotification tileNotif = new TileNotification(titleService.tileContent.GetXml());
                    tileNotif.Tag = TextBox1.Text;

                    // And send the notification to the primary tile
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotif);

                    dialog.Content = "创建成功";

                    titleService = new TitleService();
                    TitleService.Item = temp;
                    tileNotif = new TileNotification(titleService.tileContent.GetXml());
                    tileNotif.Tag = TextBox1.Text;
                    // And send the notification to the primary tile
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotif);

                    Curry.Reload();
                }
            }
            await dialog.ShowAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new Work();
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
                BitmapDecoder bitmapDecoder = await BitmapDecoder.CreateAsync(ir);
                WriteableBitmap b = new WriteableBitmap((int)bitmapDecoder.PixelWidth,(int)bitmapDecoder.PixelHeight);
                b.SetSource(ir);
                picture.Source = b;
            }
        }

        private void Grid_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (MainPage.update)
                Button1.Content = "Update";
            else
                Button1.Content = "Create";
        }

        private void AppBarButton_Click(System.Object sender, RoutedEventArgs e)
        {

        }

        private void AppBarButton_Click(System.Object sender, RoutedEventArgs e)
        {

        }
    }
}
