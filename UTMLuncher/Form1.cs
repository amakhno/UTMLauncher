using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.ServiceProcess;
using System.IO;
using Net.Pkcs11Interop.HighLevelAPI;

namespace UTMLauncher
{
    public partial class Form1 : Form
    {
        public Settings sett;
        public string currentSerial;
        public bool currentBaseIsExist;
        public bool internetConnection;

        public Form1()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            sett = new Settings();
            this.backgroundWorker1.RunWorkerAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckBase();
        }

        public void CheckBase()
        {
            JaCartaInfo jaCartaInfo = new JaCartaInfo("jcPKCS11-2.dll");
            currentSerial = jaCartaInfo.GetSerial();
            try
            {
                dbCheck.isRightCard(sett, currentSerial);
                currentBaseIsExist = true;
            }
            catch (Exception exc)
            {
                if (exc.Message == "Вставлена карта, не соответствующая базе.\nПытаюсь найти базу для этого токена")
                {
                    MessageBox.Show(exc.Message, "Внимание");
                    currentBaseIsExist = true;
                }

                if (exc.Message == "База не подписана!\nПри полной загрузке УТМ будет добавлена подпись")
                {
                    MessageBox.Show(exc.Message, "Внимание");
                    currentBaseIsExist = true;
                }

                if (exc.Message == "База данных отсутствует\nОна будет извлечена или создана новая")
                {
                    MessageBox.Show(exc.Message, "Внимание");
                    currentBaseIsExist = false;
                }

                UTM.StopTransport(sett.Path);
                try
                {
                    dbCheck.SwapBase(sett, currentBaseIsExist, currentSerial);
                }
                catch(Exception exception)
                {
                    if (exception.Message == "База данных данной карты отсутствует.\n Старая база перенесена.\nПри запуске УТМ будет создана подпись")
                    {
                        MessageBox.Show(exc.Message, "Внимание");
                        currentBaseIsExist = false;
                    }
                }
                
            }            
        }

        private void Checkings(ref bool Transport,ref bool TransportMonitoring,ref bool TransportUpdater)
        {
            ServiceController[] controllers = ServiceController.GetServices().Where(x => x.DisplayName.Contains("Transport")).ToArray();
          
            //Транспортный модуль
            if (Checks.CheckTransport(controllers.First(x => x.DisplayName == "Transport")))
            {
                Transport = true;
            }
            //Модуль Мониторинга
            if (Checks.CheckMonitoring(controllers.First(x => x.DisplayName == "Transport-Monitoring")))
            {
                TransportMonitoring = true;
            }
            //Модуль Обновления
            if (Checks.CheckUpdate(controllers.First(x => x.DisplayName == "Transport-Updater")))
            {
                TransportUpdater = true;
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBase();
                UTM.Run(sett.Adress, sett.Path, internetConnection);
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }
        }

        //Остановка Транспортного модуля
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {                
                UTM.StopTransport(sett.Path);
                dbCheck.CloseBase(sett, currentBaseIsExist, currentSerial);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }
        }

        //Выпадающее меню
        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form a = new Config(new ClassOfMyDelegate.MyDelegate(GetData), sett);
            a.Show();         
        }

        //Перенос настроек из старой версии
        void GetData(Settings param)
        {
            sett = param;
        }

        //Таймер
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
        }

        private Checks FullChecks(object state)
        {
            Checks currentState = new Checks();

            currentState.internetConnection = false;
            currentState.utmConnection = false;
            currentState.Transport = false;
            currentState.TransportMonitoring = false;
            currentState.TransportUpdater = false;

            Checkings(ref currentState.Transport, ref currentState.TransportMonitoring, ref currentState.TransportUpdater);

            currentState.internetConnection = Checks.CheckIntenetConnectionAsync(sett.Adress);
            currentState.utmConnection = Checks.CheckIntenetConnectionAsync(sett.AdressUtm);

            return currentState;
        }

        private void авторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Версия: 0.12\nАвтор: Артем Махно", "Информация о программе");
        }

        private void включитьТранспортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UTM.RunTransport(sett.Adress, sett.Path, internetConnection);
                //needCheck = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }
        }

        private void отключитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UTM.Stop(sett.Path);
                //needCheck = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }
        }

        private void AddToList(bool Transport, bool TransportMonitoring, bool TransportUpdater)
        {
            if (Transport)
            {
                checkedListBox1.Items.Add("Транспортный модуль", true);
            }
            else
            {
                checkedListBox1.Items.Add("Транспортный модуль", false);
            }
            if (TransportMonitoring)
            {
                checkedListBox1.Items.Add("Модуль Мониторинга", true);
            }
            else
            {
                checkedListBox1.Items.Add("Модуль Мониторинга", false);
            }
            if (TransportUpdater)
            {
                checkedListBox1.Items.Add("Модуль Обновления", true);
            }
            else
            {
                checkedListBox1.Items.Add("Модуль Обновления", false);
            }
        }

        protected override void WndProc(ref Message m)
        {
            ConnectDevices.Connect(ref m);
            base.WndProc(ref m);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Checks currentState = new Checks(e.Result.ToString());

            bool Transport = currentState.Transport;
            bool TransportMonitoring = currentState.TransportMonitoring;
            bool TransportUpdater = currentState.TransportUpdater;

            checkedListBox1.Items.Clear();          //Очистка формы

            if (currentState.internetConnection)
            {
                checkedListBox1.Items.Add("Соединение с Интернет", true);
            }
            else
            {
                checkedListBox1.Items.Add("Соединение с Интернет", false);
            }


            AddToList(Transport, TransportMonitoring, TransportUpdater);

            if (currentState.utmConnection)
            {
                checkedListBox1.Items.Add("Веб-интерфейс УТМ", true);
                dbCheck.TryWrite(sett, currentSerial);
            }
            else
            {
                checkedListBox1.Items.Add("Веб-интерфейс УТМ", false);
            }
            internetConnection = currentState.internetConnection;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            e.Result = FullChecks(null);
        }
    }
}
