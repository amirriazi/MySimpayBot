using Shared.WebService;
using Models;
using System.Web.Http;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Controllers
{
    public class PanelController : ApiController
    {
        [ActionName("sendMessage")]
        [HttpPost()]
        public wsPanel.SendMessage_Output sendMessage(wsPanel.SendMessage_Input ws)
        {
            do
            {
                try
                {
                    if (ws == null)
                    {
                        return new wsPanel.SendMessage_Output() { Status = new WebServiceStatus { Code = "GC0009", Description = "اطلاعات ارسالی از طرف شما خالی دریافت گردیده است!" } };
                    }
                    var panel = new Panel();
                    Task.Run(() => panel.sendMessageToMobile(ws.Parameters.FileID, ws.Parameters.MessageType, ws.Parameters.Message, ws.Parameters.Mobile, ws.Parameters.StartDateTime));
                    return new wsPanel.SendMessage_Output() { Status = new WebServiceStatus { Code = "GC0000", Description = "Successfull" } };
                    //if (panel.resultAction.hasError)
                    //{
                    //    return new wsPanel.SendMessage_Output() { Status = new WebServiceStatus { Code = "GC0009", Description = panel.resultAction.message } };
                    //}
                    //else
                    //{
                    //    return new wsPanel.SendMessage_Output() { Status = new WebServiceStatus { Code = "GC0000", Description = "Successfull" } };
                    //}

                }
                catch (System.Exception ex)
                {
                    return new wsPanel.SendMessage_Output() { Status = new WebServiceStatus { Code = "GC0009", Description = ex.Message } };
                }


            } while (false);

        }

        [ActionName("SendNotification")]
        [HttpPost()]
        public wsPanel.SendNotification_Output SendNotification(wsPanel.SendNotification_Input ws)
        {
            do
            {
                try
                {
                    if (ws == null)
                    {
                        return new wsPanel.SendNotification_Output() { Status = new WebServiceStatus { Code = "GC0009", Description = "اطلاعات ارسالی از طرف شما خالی دریافت گردیده است!" } };
                    }
                    var panel = new Panel();
                    panel.SendNotification(ws.Parameters.Message, ws.Parameters.UserCount, Convert.ToDateTime(ws.Parameters.StartDateTime));
                    if (panel.resultAction.hasError)
                    {
                        return new wsPanel.SendNotification_Output() { Status = new WebServiceStatus { Code = "GC0009", Description = panel.resultAction.message } };
                    }
                    else
                    {
                        return new wsPanel.SendNotification_Output() { Status = new WebServiceStatus { Code = "GC0000", Description = "Successfull" } };
                    }

                }
                catch (System.Exception ex)
                {
                    return new wsPanel.SendNotification_Output() { Status = new WebServiceStatus { Code = "GC0009", Description = ex.Message } };
                }


            } while (false);

        }

        [ActionName("SendNotificationToAll")]
        [HttpPost()]
        public wsPanel.SendNotificationToAll_Output SendNotificationToAll(wsPanel.SendNotificationToAll_Input ws)
        {
            do
            {
                try
                {
                    if (ws == null)
                    {
                        return new wsPanel.SendNotificationToAll_Output() { Status = new WebServiceStatus { Code = "GC0009", Description = "اطلاعات ارسالی از طرف شما خالی دریافت گردیده است!" } };
                    }
                    var panel = new Panel();
                    Task.Run(() => panel.SendNotificationToAll(ws.Parameters));
                    if (panel.resultAction.hasError)
                    {
                        return new wsPanel.SendNotificationToAll_Output() { Status = new WebServiceStatus { Code = "GC0009", Description = panel.resultAction.message } };
                    }
                    else
                    {
                        return new wsPanel.SendNotificationToAll_Output() { Status = new WebServiceStatus { Code = "GC0000", Description = "Successfull" } };
                    }

                }
                catch (System.Exception ex)
                {
                    return new wsPanel.SendNotificationToAll_Output() { Status = new WebServiceStatus { Code = "GC0009", Description = ex.Message } };
                }


            } while (false);

        }




        [ActionName("Test")]
        [HttpPost()]
        public string Test(string param)
        {
            var t = "PT20H20M";
            var date = DateTime.Now;
            var time = t.FromEpochToTimeEx();


            var ans = "";//Utils.Shamsi(date.ToString("d"));
            //var TM = new TelegramMessage();
            //TM.telegramAPI = new myTelegramApplication.TelegramAPI(ProjectValues.adminChatId);
            //TM.PaymentStartProcess("");
            //ans += $" {Environment.NewLine} {Utils.testStackTrace()}";
            Uri uriAddress = new Uri("http://www.contoso.com/index.htm#search");
            ans += $"Fragment={uriAddress.Fragment} {Environment.NewLine}";
            ans += $"AbsolutePath={uriAddress.AbsolutePath} {Environment.NewLine}";
            ans += $"AbsoluteUri={uriAddress.AbsoluteUri} {Environment.NewLine}";
            ans += $"Host={uriAddress.Host} {Environment.NewLine}";
            ans += $"LocalPath={uriAddress.LocalPath} {Environment.NewLine}";
            ans += $"OriginalString={uriAddress.OriginalString} {Environment.NewLine}";
            ans += $"Scheme={uriAddress.Scheme} {Environment.NewLine}";
            ans += $"UserInfo={uriAddress.UserInfo} {Environment.NewLine}";


            string txt = param;

            string re1 = ".*?"; // Non-greedy match on filler
            string re2 = "(?:[a-z][a-z]+)"; // Uninteresting: word
            string re3 = ".*?"; // Non-greedy match on filler
            string re4 = "((?:[a-z][a-z]+))";   // Word 1

            Regex r = new Regex(re1 + re2 + re3 + re4, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(txt);
            ans = $"{re1 + re2 + re3 + re4}  - > ";
            if (m.Success)
            {
                var word1 = m.Groups[1].ToString();
                ans += $"groups:{m.Groups.Count}";
                ans += word1.ToString();
            }
            return ans;
        }

        [ActionName("RegTest")]
        [HttpPost()]
        public string RegTest(string reg, string param)
        {
            var ans = "";//Utils.Shamsi(date.ToString("d"));

            string txt = param;

            //string re1 = @".\(?"; // Non-greedy match on filler
            //string re2 = "(?:[a-z][a-z]+)"; // Uninteresting: word
            //string re3 = @".\)?"; // Non-greedy match on filler
            //string re4 = "((?:[a-z][a-z]+))";   // Word 1

            Regex r = new Regex(reg, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(txt);
            ans = $"{reg}  - > ";
            if (m.Success)
            {
                for (int i = 0; i < m.Groups.Count; i++)
                {
                    ans += $"Groups[{i}]:{m.Groups[i].ToString()} {Environment.NewLine}";
                }
            }
            return ans;
        }

        [ActionName("boom")]
        [HttpGet()]
        public void boom(string code, string state)
        {
            Log.Fatal($"BOOM: code={code}  state={state}", 0);

        }


    }

}
