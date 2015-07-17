using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLiteSetPwd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _pwd;

        public MainWindow()
        {
            InitializeComponent();

            _pwd = CustomMd5("Abiz!@#$%^&*()1234!@#$%^&*()");
            //75e6570127598c658c980b1e0f27eb1e
        }

        private void SetPwdClick(object sender, RoutedEventArgs e)
        {
            using (var sqliteConn = new SQLiteConnection(string.Format(@"Data Source={0}\Abiz.db3;UTF8Encoding=True;Version=3;Pooling=True", txt.Text)))
            {
                sqliteConn.Open();
                sqliteConn.ChangePassword(_pwd);
                sqliteConn.Close();
            }
        }

        private void NullPwdClick(object sender, RoutedEventArgs e)
        {
            using (var sqliteConn = new SQLiteConnection(string.Format(@"Data Source={0}\Abiz.db3;UTF8Encoding=True;Version=3;Pooling=True", txt.Text)))
            {
                sqliteConn.SetPassword(_pwd);
                sqliteConn.Open();
                sqliteConn.ChangePassword(string.Empty);
                sqliteConn.Close();
            }
        }

        private string CustomMd5(string str)
        {
            var md = new MD5CryptoServiceProvider();
            var ss = md.ComputeHash(UnicodeEncoding.UTF8.GetBytes(str));
            return CustomMd5Helper.ByteArrayToHexString(ss);
        }
    }

    public class CustomMd5Helper
    {
        private static readonly string[] HexCode =
            {
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e",
                "f"
            };

        public static string ByteToHexString(byte b)
        {
            int n = b;
            if (n < 0)
                n = 256 + n;
            var d1 = n / 16;
            var d2 = n % 16;
            return HexCode[d1] + HexCode[d2];
        }

        public static string ByteArrayToHexString(byte[] b)
        {
            return b.Aggregate("", (current, t) => current + ByteToHexString(t));
        }
    }
}
