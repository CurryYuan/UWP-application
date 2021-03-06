﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using App2.Models;
using Windows.ApplicationModel.Email;
using App2.Service;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace App2
{
    public sealed partial class MyUserControl2 : UserControl
    {
        public static Work temp;
        public MyUserControl2()
        {
            this.InitializeComponent();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

            temp = (Work)DataContext;

            DataTransferManager.ShowShareUI();

        }

        private void CheckBox1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataService.UpdateDB((Work)DataContext);
        }
    }
}
