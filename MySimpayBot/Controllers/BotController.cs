using System.Web.Http;
using AltonTechBotManager;
using Shared.WebService;
using Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net;

namespace Controllers
{
    public class BotController : ApiController
    {
        [ActionName("update")]
        [HttpPost()]
        public void update(Update U)
        {
            var ans = "";
            try
            {
                var TM = new TelegramMessage(U);
                Task.Run(() => TM.actionOnUpdate());
                ans = "ok";
            }
            catch (System.Exception ex)
            {
                Log.Error(ex.Message, 0);
                ans = ex.Message;
            }
        }

        [ActionName("CallBackTransaction")]
        [HttpPost()]
        public CallBackTransactionOutput CallBackTransaction(CallBackTransactionInput callback)
        {
            //if (callback.ProductId == 7)
            //{
            //    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "this item does not exist"));
            //}

            try
            {

                var TM = new TelegramMessage();
                return TM.CallBackWebhook(callback);

            }
            catch (System.Exception ex)
            {
                Log.Error(ex.Message, 0);
                return new CallBackTransactionOutput() { HasError = true, Message = ex.Message };

            }
        }


        [ActionName("CinemaTicketPlan")]
        [HttpPost()]
        public void CinemaTicketPlan(CinemaTicketCallBackInput ws)
        {

            var TM = new TelegramMessage();

            TM.CinemaTicketSeatSelectionCallBack(ws.chatId, ws.seats);
        }
        [ActionName("EventSeatPlan")]
        [HttpPost()]
        public void EventSeatPlan(EventSeatPlanInput ws)
        {

            var TM = new TelegramMessage();

            TM.DramaEventSeatPlan(ws);
        }


        [ActionName("SendMessageToChat")]
        [HttpPost()]
        public void SendMessageToChat(JobSendMessageToChat_Input ws)
        {
            //Log.Trace($"JobSendMessageToChat:{Utils.ConvertClassToJson(ws)}", 0);
            var TM = new TelegramMessage();

            var Body = JsonConvert.DeserializeObject<JobSendMessageToChat_Input_Body>(ws.Body);
            Task.Run(() => TM.sendMessageToChat(ws.UID, Body.MessageType, Body.FileID, Body.Message, Body.ChatIds));




        }


    }
}
