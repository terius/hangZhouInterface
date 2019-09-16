using Common;
using System;
using System.IO;
using System.Threading;
using System.Web.Services;

namespace WebApplication1
{
    /// <summary>
    /// yService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class yService : System.Web.Services.WebService
    {

        string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
        ///<summary>
        ///获取出口清单或进口个人物品申报单数据,没有找到数据返回null
        ///</summary>
        ///<param name="wbNo">运单号</param>
        ///<returns>出口清单或进口个人物品申报单</returns>
        [WebMethod]
        public string GetInfo(string logistics_No, string app_No)
        {
           // Thread.Sleep(5000);
            string aa = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dir = new DirectoryInfo(Path.Combine(Server.MapPath("/"), "files"));
            foreach (var file in dir.GetFiles())
            {
                if (file.Name.Contains(logistics_No))
                {
                    var str = XmlHelper.FileToXMLString(file.FullName);
                    return str;
                }
            }
            return "";
          

        }

        /// <summary>
        /// 返回操作结果
        /// </summary>
        /// <param name="logistics_No"></param>
        /// <param name="app_No"></param>
        /// <param name="checkWay"></param>
        /// <param name="checkResult"></param>
        /// <param name="inspectionStatus"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [WebMethod]
        public String SetExam(String logistics_No, String app_No, int checkWay, int checkResult, int inspectionStatus, string result)
        {
            var str = XmlHelper.FileToXMLString(Path.Combine(filePath, "2.txt"));
            return str;
        }
    }


}
