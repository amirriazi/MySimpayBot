using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Def
    {
        public static MyDbLoggerModel MyDbLogger { get; set; }
    }

    public class MyDbLoggerModel
    {
        public bool logging { get; set; }
        public string method { get; set; }
        public string action { get; set; }
        public string requestUid { get; set; }
        public string UserUID { get; set; }
        public string requestUri { get; set; }
        public string playLoad { get; set; }
        public MyDbLoggerModel()
        {
            method = "";
            action = "";
            requestUid = "";
            UserUID = "";
            requestUri = "";
            playLoad = "";
        }

        public void reportLog()
        {

            if (!logging) return;
            DataBase.MyDBLogger_ReportSimpayBotLog(method, action, requestUid, UserUID, requestUri, playLoad);

        }

    }
}
