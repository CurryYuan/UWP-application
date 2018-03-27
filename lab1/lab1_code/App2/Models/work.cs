using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace App2.Models
{
    public class Work: INotifyPropertyChanged
    {
        string title, detail;
        DateTimeOffset date;
        ImageSource image;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        public Work()
        {
            this.date = DateTime.Now.Date;
        }
        public Work(string title, string detail,DateTimeOffset date, ImageSource image)
        {
            this.title = title;
            this.detail = detail;
            this.image = image;
            this.date = date;
        }


        public override string ToString()
        {
            return Date.ToString();
        }
    }

    public class Curry
    {
        private static ObservableCollection<Work> Works = new ObservableCollection<Work>();
        private Curry()
        {
        }
        public static ObservableCollection<Work> Get_instance()
        {
            return Works;
        }
    }
}
