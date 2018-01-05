using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangZhouTran
{
    public class ServerHelper
    {
        static ServiceReference1.yServiceSoapClient client = new ServiceReference1.yServiceSoapClient();
        public static DataSet GetOutputData(string wbNo)
        {

            var data = client.GetData(wbNo);
            return data;
        }

        public static void putData(string wbNo, string opEr, string opType, DateTime opTime)
        {

            client.PutData(wbNo, opEr, opType, opTime);

        }
    }
}
