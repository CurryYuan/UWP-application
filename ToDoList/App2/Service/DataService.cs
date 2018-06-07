using App2.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace App2.Service
{
    public static class DataService
    {
        public static void InitializeDatabase()
        {
            using (var conn = new SQLiteConnection("Works.db"))
            {
                using (var statement = conn.Prepare("CREATE TABLE IF NOT EXISTS" +
                                                    " Work(Id      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                                                    " Title    VARCHAR(140)," +
                                                    " Detail    VARCHAR(140)," +
                                                    " IsChecked BOOLEAN," +
                                                    " Date DateTime," +
                                                    " Pic BLOB); "))
                {
                    statement.Step();
                }
            }
        }

        public static void DeleteDB(int id)
        {
            using (var conn = new SQLiteConnection("Works.db"))
            {
                using (var statement = conn.Prepare("DELETE FROM Work WHERE Id = ?"))
                {
                    statement.Bind(1, id);
                    statement.Step();
                }
            }
        }

        public static async void InsertDB(Work temp)
        {
            temp.Store = await Con.SaveToBytesAsync(temp.Image);
            using (var conn = new SQLiteConnection("Works.db"))
            {
                using (var custstmt = conn.Prepare("INSERT INTO Work (Title, Detail, IsChecked,Date,Pic) VALUES (?, ?, ?, ?,?)"))
                {
                    custstmt.Bind(1, temp.Title);
                    custstmt.Bind(2, temp.Detail);
                    custstmt.Bind(3, temp.IsChecked.ToString());
                    custstmt.Bind(4, temp.Date.ToString());
                    custstmt.Bind(5, temp.Store);
                    custstmt.Step();
                }
            }
        }

        public static async void UpdateDB(Work temp)
        {
            temp.Store = await Con.SaveToBytesAsync(temp.Image);
            Debug.WriteLine(temp.Id);

            using (var conn = new SQLiteConnection("Works.db"))
            {
                using(var statement=conn.Prepare("UPDATE Work SET Title=?, Detail=?,IsChecked=?, Date=?,Pic=? WHERE Id = ?"))
                {
                    statement.Bind(1, temp.Title);
                    statement.Bind(2, temp.Detail);
                    statement.Bind(3, temp.IsChecked.ToString());
                    statement.Bind(4, temp.Date.ToString());
                    statement.Bind(5, temp.Store);
                    statement.Bind(6, temp.Id);
                    statement.Step();
                }
            }
        }

        public static List<Work> QueryLike(string str)
        {
            List<Work> works = new List<Work>();

            using (var conn = new SQLiteConnection("Works.db"))
            {
                using (var statement = conn.Prepare("SELECT Id, Title,Detail,IsChecked,Date FROM Work WHERE Title LIKE ? OR Detail LIKE ?"))
                {
                    statement.Bind(1, "%" + str + "%");
                    statement.Bind(2, "%" + str + "%");
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        works.Add(new Work()
                        {
                            Id = (int)(long)statement[0],
                            Title = (string)statement[1],
                            Detail = (string)statement[2],
                            IsChecked = Convert.ToBoolean(statement[3].ToString()),
                            Date = Convert.ToDateTime(statement[4].ToString())
                        });

                    }
                }
            }

            return works;
        }

        public static List<Work> GetData()
        {
            List<Work> works = new List<Work>();
            
            using (var conn = new SQLiteConnection("Works.db"))
            {
               using(var statement=conn.Prepare("SELECT Id, Title,Detail,IsChecked,Date,Pic FROM Work "))
                {
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        works.Add(new Work()
                        {
                            Id = (int)(long)statement[0],
                            Title = (string)statement[1],
                            Detail = (string)statement[2],
                            IsChecked = Convert.ToBoolean(statement[3].ToString()),
                            Date = Convert.ToDateTime(statement[4].ToString()),
                            Store = (byte[])statement[5]
                        });
    
                    }
                }
            }

            return works;
        }
    }
}
