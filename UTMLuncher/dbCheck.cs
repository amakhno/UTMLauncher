using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace UTMLauncher
{
    static class dbAction
    {
        public static bool IsNeedToWrite(Settings settings, String serial, ref bool NeedToWrite)
        {
            string Message;
            NeedToWrite = false;
            if (!CurrentBaseDirectoryIsExist(settings))
            {
                Message = "База данных отсутствует\n";
                if (BaseWithSerialIsExist(settings, serial))
                {
                    Directory.Move(settings.Path + "\\transporter\\" + serial + "transportDB", settings.Path + "\\transporter\\transportDB");
                    Message += "Была загружена старая база данных";
                    throw new Exception(Message);
                }
                Message += "Подписанной базы не обнаружено. Будет создана новая";
                NeedToWrite = true;
                throw new Exception(Message);
            }
            if (!File.Exists(settings.Path + "\\transporter\\transportDB\\serial.cfg"))
            {
                Message = "База не подписана!\nВставьте правильную карту и дождитесь подписи (полного запуска)";
                NeedToWrite = true;
                throw new Exception(Message);
            }

            //Текущая карта не соответствует базе
            if (CurrentBaseSerial(settings) != serial)
            {
                if (BaseWithSerialIsExist(settings, CurrentBaseSerial(settings)))
                {
                    Message = "Обнаружено 2 одинаковых базы.\nРекомендуется удалить базу из вкладки \"Дополнительно\"";
                    throw new Exception(Message);
                }
                Message = "Вставлена карта, не соответствующая базе.\n";
                UTM.StopTransport(settings.Path);
                Directory.Move(settings.Path + "\\transporter\\transportDB", settings.Path + "\\transporter\\" + CurrentBaseSerial(settings) + "transportDB");
                Message += "Старая база перенесена\n";
                if (BaseWithSerialIsExist(settings, serial))
                {
                    Directory.Move(settings.Path + "\\transporter\\" + serial + "transportDB", settings.Path + "\\transporter\\transportDB");
                    Message += "Была загружена база данных для текущего ключа\n";
                }
                else
                {
                    Message += "База данных для текущего ключа не найдена\n";
                    Message += "Будет создана новая\n";
                }
                throw new Exception(Message);
            }

            return false;
        }

        private static bool BaseWithSerialIsExist(Settings settings, String serial)
        {
            return Directory.Exists(settings.Path + "\\transporter\\" + serial + "transportDB");
        }

        private static bool CurrentBaseDirectoryIsExist(Settings settings)
        {
            return (Directory.Exists(settings.Path + "\\transporter\\transportDB"));
        }

        private static string CurrentBaseSerial(Settings settings)
        {
            return File.ReadAllLines(settings.Path + "\\transporter\\transportDB\\serial.cfg")[0];
        }

        public static void TryWrite(Settings settings, String currentSerial)
        {
            if (!File.Exists(settings.Path + "\\transporter\\transportDB\\serial.cfg"))
            {
                File.WriteAllLines(settings.Path + "\\transporter\\transportDB\\serial.cfg", new String[] { currentSerial });
            }
        }

        public static void CloseBase(Settings settings, bool currentBaseIsExist, String currentSerial)
        {
            UTM.StopTransport(settings.Path);
            if (BaseWithSerialIsExist(settings, CurrentBaseSerial(settings)))
            {
                string Message = "Обнаружено 2 одинаковых базы.\nРекомендуется удалить базу из вкладки \"Дополнительно\"";
                throw new Exception(Message);
            }
            if (File.Exists(settings.Path + "\\transporter\\transportDB\\serial.cfg"))
            {
                UTM.StopTransport(settings.Path);
                string baseSerial =  File.ReadAllLines(settings.Path + "\\transporter\\transportDB\\serial.cfg")[0];
                Directory.Move(settings.Path + "\\transporter\\transportDB", settings.Path + "\\transporter\\" + baseSerial + "transportDB");
            }
        }      

        public static string GetCurrentSerial()
        {
            JaCartaInfo jaCartaInfo = new JaCartaInfo("jcPKCS11-2.dll");            
            string result = jaCartaInfo.GetSerial();                      
            jaCartaInfo.Dispose();                       
            return result;
        }

        public static void DeleteBase(Settings settings)
        {
            if (CurrentBaseDirectoryIsExist(settings))
            {
                UTM.StopTransport(settings.Path);
                Directory.Delete(settings.Path + "\\transporter\\transportDB", true);
                throw new Exception("База данных удалена");
            }
            else throw new Exception("База данных отсутствует");
        }
    }
}
