using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ServiceProcess;

namespace UTMLauncher
{
    class Checks
    {
        public bool internetConnection = false;
        public bool utmConnection = false;
        public bool Transport = false;
        public bool TransportMonitoring = false;
        public bool TransportUpdater = false;

        public Checks()
        {
            ;
        }


        /// <param name="loadString">Строка для преобразования в тукущий статус</param>
        public Checks(string loadString)
        {
            if (loadString[0] == '1')
            {
                internetConnection = true;
            }
            else
            {
                internetConnection = false;
            }
            if (loadString[1] == '1')
            {
                utmConnection = true;
            }
            else
            {
                utmConnection = false;
            }
            if (loadString[2] == '1')
            {
                Transport = true;
            }
            else
            {
                Transport = false;
            }
            if (loadString[3] == '1')
            {
                TransportMonitoring = true;
            }
            else
            {
                TransportMonitoring = false;
            }
            if (loadString[4] == '1')
            {
                TransportUpdater = true;
            }
            else
            {
                TransportUpdater = false;
            }
        }

        public override string ToString()
        {
            string result = "";
            if (internetConnection)
            {
                result += "1";
            }
            else
            {
                result += "0";
            }
            if (utmConnection)
            {
                result += "1";
            }
            else
            {
                result += "0";
            }
            if (Transport)
            {
                result += "1";
            }
            else
            {
                result += "0";
            }
            if (TransportMonitoring)
            {
                result += "1";
            }
            else
            {
                result += "0";
            }
            if (TransportUpdater)
            {
                result += "1";
            }
            else
            {
                result += "0";
            }
            return result;
        }

        public static bool CheckIntenetConnectionAsync(string adress)
        {            
                try
                {
                    HttpWebRequest reqFP = (HttpWebRequest)HttpWebRequest.Create(adress);

                    WebResponse rspFP1 = reqFP.GetResponse();
                    HttpWebResponse rspFP = (HttpWebResponse)rspFP1;
                    if (HttpStatusCode.OK == rspFP.StatusCode)
                    {
                        // HTTP = 200 - Интернет безусловно есть! 
                        rspFP.Close();
                        return true;
                    }
                    else
                    {
                        // сервер вернул отрицательный ответ, возможно что инета нет
                        rspFP.Close();
                        return false;
                    }
                }
                catch (WebException)
                {
                    // Ошибка, значит интернета у нас нет. Плачем :'(
                    return false;
                }
            
        }

        public static bool CheckTransport(ServiceController Service)
        {
            if (Service == null) return false;
            if (Service.Status == ServiceControllerStatus.Running) return true;
            return false; 
        }

        public static bool CheckMonitoring(ServiceController Service)
        {
            if (Service == null) return false;
            if (Service.Status == ServiceControllerStatus.Running) return true;
            return false;
        }

        public static bool CheckUpdate(ServiceController Service)
        {
            if (Service == null) return false;
            if (Service.Status == ServiceControllerStatus.Running) return true;
            return false;
        }
    }
}
