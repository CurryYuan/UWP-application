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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

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

            var dialog = new ContentDialog()
            {
                Title = "提示",
                Content = "",
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
                    Works.Add(new Work(TextBox1.Text, TextBox2.Text, DatePicker1.Date, picture.Source));
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
                BitmapImage bi = new BitmapImage();
                await bi.SetSourceAsync(ir);
                picture.Source = bi;
            }
        }

        private void Grid_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (MainPage.update)
                Button1.Content = "Update";
            else
                Button1.Content = "Create";
        }
    }
}
