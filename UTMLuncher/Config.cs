using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace UTMLuncher
{
    public partial class Config : Form
    {
        Settings tempSett;
        ClassOfMyDelegate.MyDelegate d;
        public Config(ClassOfMyDelegate.MyDelegate sender, Settings sett)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            textBox1.Text = sett.Adress;            //Выскрываем старые настройки
            textBox2.Text = sett.Path;
            textBox3.Text = sett.AdressUtm;
            tempSett = sett;
            d = sender;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            tempSett.Adress = textBox1.Text;
            tempSett.Path = textBox2.Text;
            tempSett.AdressUtm = textBox3.Text;
            tempSett.Save();
            d(tempSett);           
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result != DialogResult.OK) return;
                if (Settings.CheckPath(folderBrowserDialog1.SelectedPath))
                {
                    textBox2.Text = folderBrowserDialog1.SelectedPath;                    
                }
                else throw new Exception("Неверный путь УТМ!");
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message, "Ошибка");
            }

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
