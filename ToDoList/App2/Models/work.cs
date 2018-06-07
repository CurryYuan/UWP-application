using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.UI.Xaml.Media;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using App2.Service;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Streams;
using System.Diagnostics;

namespace App2.Models
{
    public class Work : INotifyPropertyChanged
    {
        static int num = 0;
        int id;
        string title, detail;
        bool isChecked;
        DateTimeOffset date;
        ImageSource image;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    id = value;
                    NotifyPropertyChanged();
                }               
            }
        }
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title != value)
                {
                    title = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Detail
        {
            get
            {
                return detail;
            }
            set
            {
                if (detail != value)
                {
                    detail = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public ImageSource Image
        {
            get
            {
                return image;
            }
            set
            {
                if (image != value)
                {
                    image = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTimeOffset Date
        {
            get
            {
                return date;
            }
            set
            {
                if (date != value)
                {
                    date = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public byte[] Store { get; set; }
        public Work()
        {
            this.date = DateTime.Now.Date;
            this.isChecked = false;
        }
        public Work(string title, string detail, DateTimeOffset date, ImageSource image)
        {
            ++num;
            id = num;
            this.title = title;
            this.detail = detail;
            this.image = image;
            this.date = date;
            this.isChecked = false;
        }


        public override string ToString()
        {
            string text;
            text = "Title: " + title + " Detail: " + detail + " DueDate: " + date.ToString();

            return text;
        }
    }

    public class Curry
    {
        private static ObservableCollection<Work> Works;
        private Curry()
        {
        }
        public static ObservableCollection<Work> Get_instance()
        {
            if (Works == null)
            {
                Works = new ObservableCollection<Work>();
            }
            return Works;
        }
        public static async void Reload()
        {
            Works.Clear();
            List<Work> temp = DataService.GetData();
            foreach (Work it in temp)
            {
                it.Image = await Con.SaveToImageSource(it.Store);

                Works.Add(it);
            }
        }
    }

    public static class Con
    {
        public static async Task<byte[]> SaveToBytesAsync(this ImageSource imageSource)
        {
            byte[] imageBuffer;
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync("temp.jpg", CreationCollisionOption.ReplaceExisting);
            using (var ras = await file.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            {
                WriteableBitmap bitmap = imageSource as WriteableBitmap;
                var stream = bitmap.PixelBuffer.AsStream();
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, ras);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, 96.0, 96.0, buffer);
                await encoder.FlushAsync();

                var imageStream = ras.AsStream();
                imageStream.Seek(0, SeekOrigin.Begin);
                imageBuffer = new byte[imageStream.Length];
                var re = await imageStream.ReadAsync(imageBuffer, 0, imageBuffer.Length);

            }
            await file.DeleteAsync(StorageDeleteOption.Default);
            return imageBuffer;
        }

        public static async Task<ImageSource> SaveToImageSource(this byte[] imageBuffer)
        {
            ImageSource imageSource = null;
            using (MemoryStream stream = new MemoryStream(imageBuffer))
            {
                var ras = stream.AsRandomAccessStream();
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(BitmapDecoder.JpegDecoderId, ras);
                var provider = await decoder.GetPixelDataAsync();
                byte[] buffer = provider.DetachPixelData();
                WriteableBitmap bitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                await bitmap.PixelBuffer.AsStream().WriteAsync(buffer, 0, buffer.Length);
                imageSource = bitmap;
            }
            return imageSource;
        }

     
    }
}
