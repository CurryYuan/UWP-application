using App2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Diagnostics;
using Windows.Storage;
using App2.Service;
using Windows.UI.Notifications;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using SQLitePCL;
using System.Text;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    ///   

    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Work> Works;
        public static bool update = false;
        private Work temp = null;
        public MainPage()
        {
            this.InitializeComponent();
            DataService.InitializeDatabase();
            Works = Curry.Get_instance();
            Curry.Reload();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            update = false;

            if (Grid2.Visibility == Visibility.Collapsed)
            {
                Frame root = Window.Current.Content as Frame;

                root.Navigate(typeof(NewPage));
            }
            else
            {
                Second.DataContext = new Work();
            }
        }

        private void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Works.Remove(temp);
            DataService.DeleteDB(temp.Id);
            update = false;
            Second.DataContext = new Work();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            update = true;
            temp = (Work)e.ClickedItem;
            if (Grid2.Visibility == Visibility.Visible)
            {
                Second.DataContext = temp;
                Debug.WriteLine(temp.Id);
            }
            else
            {
                Frame root = Window.Current.Content as Frame;
                root.Navigate(typeof(NewPage), ((Work)temp));
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            bool suspending = ((App)App.Current).isSuspending;
            if (suspending)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                Work work = (Work)Second.DataContext;
                if (work != null)
                {
                    composite["title"] = work.Title;
                    composite["detail"] = work.Detail;
                    composite["date"] = work.Date;

                }
                ApplicationData.Current.LocalSettings.Values["mainPage"] = composite;
            }
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("mainPage");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("mainPage"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["mainPage"] as ApplicationDataCompositeValue;

                    Second.DataContext = new Work((string)composite["title"], (string)composite["detail"], (DateTimeOffset)composite["date"], null);

                    ApplicationData.Current.LocalSettings.Values.Remove("mainPage");
                }
            }
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
        }

        async void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            var request = args.Request;
            request.Data.Properties.Title = MyUserControl2.temp.Title;
            request.Data.Properties.Description = "This demonstrates how to share text to another app";
            request.Data.SetText(MyUserControl2.temp.Detail);

            var photoFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/share.jpg"));
            request.Data.SetStorageItems(new List<StorageFile> { photoFile });

            deferral.Complete();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var dialog = new ContentDialog()
            {
                Title = "查询结果",
                Content = "",
                PrimaryButtonText = "确定",
                FullSizeDesired = false,
            };
            foreach(Work it in DataService.QueryLike(SearchStr.Text))
            {
                sb.AppendFormat("{0},{1}", it.ToString(), "\n");
            }
            dialog.Content = sb.ToString();
            await dialog.ShowAsync();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Curry.Reload();
        }
    }
}
