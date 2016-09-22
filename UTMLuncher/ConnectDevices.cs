using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UTMLauncher
{
    static class ConnectDevices
    {
        private const int DBT_DEVICEARRIVAL = 0x8000;
        const int WM_DEVICECHANGE = 0x0219;
        const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

        public static void Connect(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                int EventCode = m.WParam.ToInt32();
                

                switch (EventCode)
                {
                    case DBT_DEVICEARRIVAL:
                        {
                            break;
                        }
                    case DBT_DEVICEREMOVECOMPLETE:
                        {
                            break;
                        }
                }
            }

        }
    }
}
