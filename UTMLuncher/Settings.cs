using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace UTMLuncher
{
    public class Settings
    {
        string adress;
        string path;
        string adressUTM;

        public Settings()
        {
            if (!File.Exists("config.ini"))
            {
                Default();
            }
            else
            {
                StreamReader reader = new StreamReader("config.ini");
                try
                {
                    adress = reader.ReadLine().Substring(9);
                    path = reader.ReadLine().Substring(7);
                    adressUTM = reader.ReadLine().Substring(12);
                    reader.Close();
                }
                catch(FileNotFoundException)
                {
                    reader.Close();
                    MessageBox.Show("Не могу получить доступ к файлу", "Ошибка");
                    return;
                }
                catch
                {
                    reader.Close();
                    MessageBox.Show("Не могу разобрать файл настроек\nУстновлены настройки по-умолчанию", "Ошибка");
                    Default();                }
            }
        }

        private void Default()
        {
            adress = "http://google.com";
            path = "C:\\UTM";
            adressUTM = "http://127.0.0.1:8080";
            this.Save();
        }

        public void Save()
        {
            try
            {
                StreamWriter writer = new StreamWriter("config.ini");
                writer.WriteLine("Adress = " + adress);
                writer.WriteLine("Path = " + path);
                writer.WriteLine("AdressUTM = " + adressUTM);
                writer.Close();
            }
            catch
            {
                MessageBox.Show("Не могу сохранить настройки", "Ошибка");
            }
        }

        public string Adress
        {
            get
            {
                return adress;
            }
            set
            {
                adress = value;
            }
        }

        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        public string AdressUtm
        {
            get
            {
                return adressUTM;
            }
            set
            {
                adressUTM = value;
            }
        }

        public static bool CheckPath(string tempPath)
        {
            return File.Exists(tempPath + "\\transporter\\bin\\RunDaemon.bat");
        }
    }
}
