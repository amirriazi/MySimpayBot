using NLog;
using System.Linq;
using System.Runtime.CompilerServices;
using Models;

namespace Shared.WebService
{
    public static class Log
    {
        private static readonly Logger logger;

        public static Logger Logger
        {
            get { return logger; }
        }


        static Log()
        {
            LogManager.GetCurrentClassLogger();
            //LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("nLog.config", true);
            logger = LogManager.GetCurrentClassLogger();
        }
        private static string Body(string message)
        {
            return "[" + message + "]";
        }
        private static string ExecutionTime(double time)
        {
            return "[" + time + "]";
        }
        private static string Consumer(string userName = "")
        {
            if (string.IsNullOrEmpty(userName) == true)
                return "[]";

            var wsun = "(Web Service User Name: " + userName + ")";
            var iP = "(IP Address: " + Converter.GetIpAddress() + ")";
            var hostName = "(Host Name: " + Converter.GetHostName() + ")";
            var userAgent = "(User Agent: " + Converter.GetUserAgent() + ")";
            var calledUrl = "(Called URL: " + Converter.GetCalledUrl() + ")";

            return "[" + wsun + iP + hostName + userAgent + calledUrl + "]";
        }
        private static string CurrentMethod(string sourceFilePath, string memberName, int sourceLineNumber)
        {
            sourceFilePath = sourceFilePath.Contains('\\') ? sourceFilePath.Substring(sourceFilePath.LastIndexOf('\\') + 1) : sourceFilePath;
            sourceFilePath = sourceFilePath.Contains('.') ? sourceFilePath.Substring(0, sourceFilePath.IndexOf('.')) : sourceFilePath;

            return "[" + sourceFilePath + "." + memberName + " line:" + sourceLineNumber + "]";
        }
        private static string Fromat(string message, double time, string userName, string sourceFilePath, string memberName, int sourceLineNumber)
        {
            var currentMethod = CurrentMethod(sourceFilePath, memberName, sourceLineNumber);
            var executiontime = ExecutionTime(time);
            var consumer = Consumer(userName);
            var body = Body(message);

            return executiontime + ";" + currentMethod + ";" + consumer + ";" + body;
        }

        public static void Trace(string message, double time, string userName = "", [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {

            if (ProjectValues.LogMode <= 0)
            {
                Logger.Trace(Fromat(message, time, userName, sourceFilePath, memberName, sourceLineNumber));
            }

        }
        public static void Debug(string message, double time, string userName = "", [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (ProjectValues.LogMode <= 1)
            {
                Logger.Debug(Fromat(message, time, userName, sourceFilePath, memberName, sourceLineNumber));
            }

        }
        public static void Info(string message, double time, string userName = "", [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (ProjectValues.LogMode <= 2)
            {
                Logger.Info(Fromat(message, time, userName, sourceFilePath, memberName, sourceLineNumber));
            }

        }
        public static void Warn(string message, double time, string userName = "", [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (ProjectValues.LogMode <= 3)
            {
                Logger.Warn(Fromat(message, time, userName, sourceFilePath, memberName, sourceLineNumber));
            }

        }
        public static void Error(string message, double time, string userName = "", [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (ProjectValues.LogMode <= 4)
            {
                Logger.Error(Fromat(message, time, userName, sourceFilePath, memberName, sourceLineNumber));
                Def.MyDbLogger.action += $":error";
                Def.MyDbLogger.playLoad = $"{message}";
                Def.MyDbLogger.reportLog();

            }

        }
        public static void Fatal(string message, double time, string userName = "", [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (ProjectValues.LogMode <= 5)
            {
                Logger.Fatal(Fromat(message, time, userName, sourceFilePath, memberName, sourceLineNumber));
                Def.MyDbLogger.action += $":fatal";
                Def.MyDbLogger.playLoad = $"{message}";
                Def.MyDbLogger.reportLog();
            }

        }
    }
}