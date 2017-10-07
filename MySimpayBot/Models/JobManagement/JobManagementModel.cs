using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shared.WebService;
using Newtonsoft.Json;

namespace Models
{
    public class JobManagementModel
    {
        public static string JsonWebToken = "eyJmZHQiOiIyMDE3LTA1LTA3VDA5OjUwOjIxLjI2IiwidGR0IjoiMjAxNy0wNS0wN1QxMDo1MDoyMS4yNiIsInNlYSI6MSwicGpuIjoiTm90aWZpY2F0aW9uRkNNIiwicHJhIjpmYWxzZSwic2lkIjo0NTc1ODAzfQ==.d3JhaFYwWGdXR3ArS3o0TENjODZhcU5NZXloVEhqa252UHZFQ2lacnQzbWEzdCtpY1l4UGVlaVlYVlFKRThGcHpzVGdPRXo5Tzc3UGwvTWFJM3RXL3c9PQ==.eyJ2bHUiOiI3RjU5RjE5M0NCMTg2QzJCQUI3MkE0RTc3OTAwNkJDRTkyMzNGMkUxNzg5QjJGQTU1OTQyMDE3OTgyQUE2NEVGIn0=";
        public static int BusinessID = 10;

        public static MySimpayBot.JobManagementService.WebServiceReportJobOutput ReportJob(MySimpayBot.JobManagementService.WebServiceReportJobInput ws)
        {
            var job = new MySimpayBot.JobManagementService.CoreClient();
            var ret = new MySimpayBot.JobManagementService.WebServiceReportJobOutput();
            do
            {
                var callingResult = CallCore("ReportJob", ws);


                ret = JsonConvert.DeserializeObject<MySimpayBot.JobManagementService.WebServiceReportJobOutput>(callingResult);
            } while (false);
            return ret;
        }
        public static MySimpayBot.JobManagementService.WebServiceReportResultOutput ReportResult(MySimpayBot.JobManagementService.WebServiceReportResultInput ws)
        {
            var job = new MySimpayBot.JobManagementService.CoreClient();
            var ret = new MySimpayBot.JobManagementService.WebServiceReportResultOutput();
            do
            {
                var callingResult = CallCore("ReportResult", ws);


                ret = JsonConvert.DeserializeObject<MySimpayBot.JobManagementService.WebServiceReportResultOutput>(callingResult);
            } while (false);
            return ret;
        }

        public static string CallCore(string method, object Param)
        {
            var RES = "";
            var header = new System.Collections.Specialized.NameValueCollection();
            header.Add("Content-Type", "application/json; charset=utf-8");
            header.Add("user-agent", "ApplicationType:TelegramBot-Name:mySimpayBot-Version:1");
            try
            {
                var result = Utils.WebRequestByUrl($@"{ProjectValues.JobmManagementCoreUrl}/{method}", Utils.ConvertClassToJson(Param), header);
                Log.Info($"callCoreApi-param:{Utils.ConvertClassToJson(Param)} \n \n  response={Utils.ConvertClassToJson(result)}", 0);
                if (result.status == System.Net.WebExceptionStatus.Success)
                {
                    RES = result.responseText;
                }
                else
                {
                    Log.Fatal(result.statusMessage, 0);
                }

            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, 0);
                throw;
            }
            return RES;

        }


    }
}