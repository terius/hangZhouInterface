using Common;
using DAL;
using HangZhouTran;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        HZAction action = new HZAction();
        public Form1()
        {
            InitializeComponent();
        }


        private void btnReadFile_Click(object sender, EventArgs e)
        {
            
        }

   

        private void Form1_Load(object sender, EventArgs e)
        {
           // action.BeginRun();
        }

    

        private  void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
         
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            MyConfig.Load();
            var path = MyConfig.ScanPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var file = @"‪D:\work\MPS_IB_20190903160000.xml";
            file = @"d:\2.xml";
            //XmlDocument xDoc = new XmlDocument();
            //xDoc.Load(file);
            //XmlNodeList nodelist = xDoc.SelectNodes("awblist/awb");
            //foreach (XmlNode node in nodelist)
            //{
            //   var aaa=node.SelectSingleNode("awbnbr").InnerText;
            //}
            // var data = ExcelHelper.GetData(@"D:\work\CONs 主运单_子运单_站点 (009).xlsm", 2, 2, 3);
            //    var info = XmlHelper.DeserializeFromFile<awblist>(file);
            var list = new List<awb>();
            for (int i = 0; i < 5; i++)
            {
                //for (int j = 0; j < 3; j++)
                //{

                //}
                list.Add(new awb
                {
                    awbnbr = "1111",
                    trklist = new trklist { trknbr = new List<string> { "zzzz", "aaaaa" } }
                });
            }
            var xmlInfo = new awblist { awb = list };
            var str = XmlHelper.Serializer(xmlInfo);
        }
    }
}
