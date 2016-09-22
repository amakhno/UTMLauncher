using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UTMLauncher
{
    static class dbCheck
    {
        public static bool isRightCard(Settings settings, String serial)
        {
            if (!Directory.Exists(settings.Path + "\\transporter\\transportDB"))
            {
                throw new Exception("База данных отсутствует\nОна будет извлечена или создана новая");
            }
            if (!File.Exists(settings.Path + "\\transporter\\transportDB\\serial.cfg"))
            {
                throw new Exception("База не подписана!\nПри полной загрузке УТМ будет добавлена подпись");
            }
            string dbSerial = File.ReadAllLines(settings.Path + "\\transporter\\transportDB\\serial.cfg")[0];
            if (dbSerial != serial)
            {
                throw new Exception("Вставлена карта, не соответствующая базе.\nПытаюсь найти базу для этого токена");
            }
            return true;
        }

        public static bool SwapBase(Settings settings, bool currentBaseIsExist, String currentSerial)
        {
            if (currentBaseIsExist)
            {
                String serialInBase = File.ReadAllLines(settings.Path + "\\transporter\\transportDB\\serial.cfg")[0];
                Directory.Move(settings.Path + "\\transporter\\transportDB", settings.Path + "\\transporter\\" + serialInBase + "transportDB");
            }            
            if (!Directory.Exists(settings.Path + "\\transporter\\" + currentSerial + "transportDB"))
            {
                throw new Exception("База данных данной карты отсутствует.\nПри запуске УТМ будет создана подпись");
            }
            else
            {
                Directory.Move(settings.Path + "\\transporter\\" + currentSerial + "transportDB", settings.Path + "\\transporter\\transportDB");
            }
            
            return true;
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
            if (File.Exists(settings.Path + "\\transporter\\transportDB\\serial.cfg"))
            { 
                string baseSerial =  File.ReadAllLines(settings.Path + "\\transporter\\transportDB\\serial.cfg")[0];
                Directory.Move(settings.Path + "\\transporter\\transportDB", settings.Path + "\\transporter\\" + baseSerial + "transportDB");
            }
        }
    }
}
