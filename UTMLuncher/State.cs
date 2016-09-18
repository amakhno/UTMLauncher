using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTMLuncher
{
    class State
    {
        public bool internetConnection = false;
        public bool utmConnection = false;
        public bool Transport = false;
        public bool TransportMonitoring = false;
        public bool TransportUpdater = false;

        public State()
        {
            ;
        }

        public State(string loadString)
        {
            if (loadString[0]=='1')
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
    }
}
