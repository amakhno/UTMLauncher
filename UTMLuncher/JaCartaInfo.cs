using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.Pkcs11Interop.HighLevelAPI;

namespace UTMLauncher
{
    public class JaCartaInfo
    {
        private Pkcs11 Library;

        public JaCartaInfo(string path)
        {
            Library = new Pkcs11(path, false);           //"jcPKCS11-2.dll"
        }

        public string GetSerial()
        {
            string result = "";

            List<Slot> listOfSlots = Library.GetSlotList(true);
            try
            {
                result = listOfSlots[0].GetTokenInfo().SerialNumber;
            }
            catch
            {
                this.Dispose();
                throw new Exception("Не могу получить информацию о токене.\nУбедитесь, что JaCarta установлена");
            }

            return result;
        }

        public void Dispose()
        {
            Library.Dispose();
        }
    }
}
