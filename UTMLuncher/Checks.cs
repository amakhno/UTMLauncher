using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ServiceProcess;

namespace UTMLuncher
{
    class Checks
    {
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
