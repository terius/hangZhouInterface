using Common;
using DAL;
using HangZhouTran;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
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
            string file = "<LOGISTICS_LIBRARY_RETURN><RETURNSTATUS>1</RETURNSTATUS><RETURNTIME>20191205220502153</RETURNTIME><RETURNINFO>成功</RETURNINFO></LOGISTICS_LIBRARY_RETURN>";
          //  var info = new ResultXML();
          //  info.returnStatus
           // var str = XmlHelper.Serializer(info);
            var info2 = XmlHelper.Deserialize<ResultXML>(file);
        }



        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }


        readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;
        private void button1_Click(object sender, EventArgs e)
        {
            action.BeginRun();
        }
    }



}
