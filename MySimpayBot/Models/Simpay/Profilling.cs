using Shared.WebService;
using Newtonsoft.Json;

namespace Models
{
    public class Profilling
    {

        public static GeneralResultAction resultAction { get; set; }
        public static string API_URL = "http://profile.simpay.ir/WebServices/App.svc";
        public static Core.wsProfilling.SendActivationCode_Output_Parameters SendActivationCode(string mobileNumber)
        {
            var ans = new Core.wsProfilling.SendActivationCode_Output_Parameters();
            resultAction = new GeneralResultAction();

            do
            {
                var wsInput = new Core.wsProfilling.SendActivationCode_Input()
                {
                    Parameters = new Core.wsProfilling.SendActivationCode_Input_Parameters { MobileNumber = mobileNumber }
                };

                var header = new System.Collections.Specialized.NameValueCollection();
                header.Add("Content-Type", "application/json; charset=utf-8");
                header.Add("user-agent", "ApplicationType:TelegramBot-Name:mySimpayBot-Version:1");

                var url = $"{API_URL}/SendActivationCode";
                var result = Utils.WebRequestByUrl(url, Utils.ConvertClassToJson(wsInput), header);

                if (result.status != System.Net.WebExceptionStatus.Success)
                {
                    Log.Error(result.statusMessage, 0);
                    resultAction = new GeneralResultAction("profilling", true, result.statusMessage);
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<Core.wsProfilling.SendActivationCode_Output>(result.responseText);
                if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error(wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("profilling", true, wsOutput.Status.Description);
                    break;
                }

                ans = wsOutput.Parameters;

            } while (false);
            return ans;
        }


        public static string CreateSession(string mobileNumber, string activationCode)
        {
            var ans = "";

            resultAction = new GeneralResultAction();
            do
            {
                var wsInput = new Core.wsProfilling.CreateSession_Input()
                {
                    Parameters = new Core.wsProfilling.CreateSession_Input_Parameters()
                    {
                        MobileNumber = mobileNumber,
                        ActivationCode = activationCode,
                        ApplicationType = "telegrambot",
                        ApplicationVersion = "1.0.0",
                        ProjectName = "interface"
                    }
                };

                var header = new System.Collections.Specialized.NameValueCollection();
                header.Add("Content-Type", "application/json; charset=utf-8");
                header.Add("user-agent", "ApplicationType:TelegramBot-Name:mySimpayBot-Version:1");

                var url = $"{API_URL}/CreateSession";
                var result = Utils.WebRequestByUrl(url, Utils.ConvertClassToJson(wsInput), header);

                if (result.status != System.Net.WebExceptionStatus.Success)
                {
                    Log.Error(result.statusMessage, 0);
                    resultAction = new GeneralResultAction("profilling", true, result.statusMessage);
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<Core.wsProfilling.CreateSession_Output>(result.responseText);
                if (wsOutput.Status.Code != "G00000")
                {
                    //Log.Error(wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("profilling", true, wsOutput.Status.Description);
                    break;
                }
                ans = wsOutput.Parameters.JsonWebToken;
            } while (false);
            return ans;
        }
    }


}