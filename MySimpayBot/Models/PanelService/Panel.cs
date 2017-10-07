using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shared.WebService;
using System.Data;
using MySimpayBot;
using System.Threading.Tasks;

namespace Models
{
    public class Panel
    {
        public GeneralResultAction resultAction { get; set; }
        public void sendMessageToMobile(string fileId, Telegram.Bot.Types.Enums.MessageType messageType, string message, string[] mobiles, string StartDateTime)
        {
            resultAction = new GeneralResultAction();
            var chatIds = new List<string>();
            do
            {
                for (int i = 0; i < mobiles.Length; i++)
                {
                    var mobileTest = new Mobile() { Number = mobiles[i] };
                    if (mobileTest.IsNumberContentValid())
                    {
                        mobiles[i] = mobileTest.InternationalNumber;
                    }
                    else
                    {
                        mobiles[i] = "";
                    }
                }

                var result = DataBase.GetChatIdByMobile(mobiles);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    resultAction = new GeneralResultAction("Panel", true, result.Text);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    break;
                }

                var DS = Converter.DBNull(result.DataSet);
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    chatIds.Add(record["chatId"].ToString());
                    //Log.Info($"chatId:{record["chatId"].ToString()}", 0);
                }
                var tmpDateTime = new DateTime();
                if (String.IsNullOrEmpty(StartDateTime) || !DateTime.TryParse(StartDateTime, out tmpDateTime))
                {
                    StartDateTime = DateTime.Now.ToString("s");
                }
                if (messageType == 0)
                {
                    messageType = Telegram.Bot.Types.Enums.MessageType.TextMessage;
                }
                var wsNotifyToAll = new wsPanel.SendNotificationToAll_Input_Params
                {
                    FileID = fileId,
                    MessageType = messageType,
                    Message = message,
                    SecondBetweenBatches = 10,
                    UserPerDay = 6000,
                    UserPerBatch = 2,
                    StartDateTime = StartDateTime,
                    TestMode = false
                };

                SendNotificationToAll(wsNotifyToAll, "MessagetoMobile", chatIds);
                return;


                //var BodyJson = new BodyDefines.sendMessageToMobile
                //{
                //    FileID = fileId,
                //    MessageType = messageType,
                //    Message = message,
                //    ChatIds = chatIds.ToArray()
                //};


                //var jobHistory = new JobHistory() { description = "sendMessageToMobile Job which has been called from Panel" };
                //jobHistory.setInfo();



                //var param = new MySimpayBot.JobManagementService.WebServiceReportJobInput()
                //{
                //    Identity = new MySimpayBot.JobManagementService.WebServiceIdentity
                //    {
                //        JsonWebToken = JobManagementModel.JsonWebToken
                //    },
                //    Parameters = new MySimpayBot.JobManagementService.WebServiceReportJobInputParams
                //    {
                //        EndPoint = "http://tg.Simpay.ir/v1/api/Bot/SendMessageToChat",
                //        Body = Utils.ConvertClassToJson(BodyJson),
                //        BusinessID = JobManagementModel.BusinessID,
                //        StartDateTime = StartDateTime, //DateTime.Now.ToString("yyyy-MM-dd") + @"T13:01:00Z",
                //        TypeName = "MessagetoMobile",
                //        DeadLineDateTime = DateTime.Now.AddHours(1).ToString("s"),
                //        FetchExpirationAction = "queue",
                //        FetchExpirationTime = 30000,
                //        IntervalTime = 10000,
                //        MaxFetchNumber = 2,
                //        UID = jobHistory.jobUID.ToString(),
                //        Description = "Send Message to chat or chats!"
                //    }
                //};

                //var jobResult = JobManagementModel.ReportJob(param);
                //Log.Info("ReportJob:" + Utils.ConvertClassToJson(jobResult), 0);

            } while (false);
        }
        public void ReportBackResult(string UID, string result)
        {
            do
            {
                var param = new MySimpayBot.JobManagementService.WebServiceReportResultInput
                {
                    Identity = new MySimpayBot.JobManagementService.WebServiceIdentity
                    {
                        JsonWebToken = JobManagementModel.JsonWebToken
                    },
                    Parameters = new MySimpayBot.JobManagementService.WebServiceReportResultInputParams
                    {
                        BusinessID = JobManagementModel.BusinessID,
                        UID = UID,
                        ReportDateTime = DateTime.Now.ToString("s"),
                        Result = result
                    }
                };

                var jobResult = JobManagementModel.ReportResult(param);
                Log.Info($"ReportResult: call{Utils.ConvertClassToJson(param) } result: {Utils.ConvertClassToJson(jobResult)} ", 0);


            } while (false);
        }

        public void SendNotification(string message, int userCount, DateTime startSendingDateTime, List<string> currentChatId = null)
        {
            resultAction = new GeneralResultAction();
            do
            {
                var chatIds = new List<string>();

                if (currentChatId != null)
                {
                    chatIds = currentChatId;
                }
                else
                {
                    var result = DataBase.GetRandomChatId(userCount);
                    if (result.ReturnCode != 1 || result.SPCode != 1)
                    {
                        Log.Fatal(result.Text, DateTime.Now.Millisecond);
                        resultAction = new GeneralResultAction("Panel", true, result.Text);
                        break;
                    }
                    if (result.DataSet.Tables[0].Rows.Count <= 0)
                    {
                        resultAction = new GeneralResultAction("Panel", true, "No User selected!");
                        break;
                    }

                    var DS = Converter.DBNull(result.DataSet);
                    foreach (DataRow record in DS.Tables[0].Rows)
                    {
                        chatIds.Add(record["chatId"].ToString());
                        //chatIds.Add("102379130");
                        //Log.Info($"chatId:{record["chatId"].ToString()}", 0);
                    }
                }

                //
                string[] arrChatId = chatIds.ToArray();
                var startIndex = 0;
                var portionLength = 5;
                var portionChatIdArray = arrChatId.SubArrayEx(startIndex, portionLength);
                var portionStartSendingDateTime = startSendingDateTime;
                while (portionChatIdArray.Length > 0)
                {
                    var BodyJson = new BodyDefines.SendNotification
                    {
                        Message = message,
                        ChatIds = portionChatIdArray
                    };

                    var jobHistory = new JobHistory() { description = "SendNotification Job which has been called from Panel" };
                    jobHistory.setInfo();

                    var param = new MySimpayBot.JobManagementService.WebServiceReportJobInput()
                    {
                        Identity = new MySimpayBot.JobManagementService.WebServiceIdentity
                        {
                            JsonWebToken = JobManagementModel.JsonWebToken
                        },
                        Parameters = new MySimpayBot.JobManagementService.WebServiceReportJobInputParams
                        {
                            EndPoint = "http://tg.Simpay.ir/v1/api/Bot/SendMessageToChat",
                            Body = Utils.ConvertClassToJson(BodyJson),
                            BusinessID = JobManagementModel.BusinessID,
                            StartDateTime = portionStartSendingDateTime.ToString("s"), //DateTime.Now.ToString("yyyy-MM-dd") + @"T13:01:00Z",
                            TypeName = "Notification",
                            DeadLineDateTime = startSendingDateTime.AddMinutes(20).ToString("s"),
                            FetchExpirationAction = "systemFailed",
                            FetchExpirationTime = 30000,
                            IntervalTime = 5000,
                            MaxFetchNumber = 2,
                            UID = jobHistory.jobUID.ToString(),
                            Description = $"Send Notification to {portionChatIdArray.Length} chat(s)."
                        }
                    };

                    //Log.Trace("ReportJob:" + Utils.ConvertClassToJson(param), 0);

                    var jobResult = JobManagementModel.ReportJob(param);


                    Log.Info("ReportJob:" + Utils.ConvertClassToJson(jobResult), 0);
                    if (jobResult.Status.Code != "G00000")
                    {
                        resultAction = new GeneralResultAction("Panel", true, $"{jobResult.Status.Description}:{jobResult.Status.Description}");
                        Log.Error($"{jobResult.Status.Description}:{jobResult.Status.Description}", 0);
                        break;
                    }


                    portionStartSendingDateTime = portionStartSendingDateTime.AddSeconds(5);
                    startIndex += portionLength;
                    portionChatIdArray = arrChatId.SubArrayEx(startIndex, portionLength);

                }


            } while (false);
            return;
        }

        public void SendNotificationToAll(wsPanel.SendNotificationToAll_Input_Params ws, string TypeName = "NotificationToAll", List<string> currentChatId = null)
        {
            resultAction = new GeneralResultAction();
            do
            {
                var chatIds = new List<string>();
                if (currentChatId != null)
                {
                    chatIds = currentChatId;
                }
                else
                {
                    var result = DataBase.GetRandomChatId();
                    if (result.ReturnCode != 1 || result.SPCode != 1)
                    {
                        Log.Fatal(result.Text, DateTime.Now.Millisecond);
                        resultAction = new GeneralResultAction("Panel", true, result.Text);
                        break;
                    }
                    if (result.DataSet.Tables[0].Rows.Count <= 0)
                    {
                        resultAction = new GeneralResultAction("Panel", true, "No User selected!");
                        break;
                    }
                    if (ws.TestMode)
                    {
                        // Just send to me!
                        chatIds.Add("102379130");
                    }
                    else
                    {
                        var DS = Converter.DBNull(result.DataSet);
                        foreach (DataRow record in DS.Tables[0].Rows)
                        {
                            chatIds.Add(record["chatId"].ToString());
                            //chatIds.Add("102379130");
                            //Log.Info($"chatId:{record["chatId"].ToString()}", 0);
                        }

                    }
                }
                //
                string[] arrChatId = chatIds.ToArray();
                //string[] arrChatId = new string[] { "102379130" };

                var indexOfUserPerDay = 0;
                var usersPerDayArray = arrChatId.SubArrayEx(indexOfUserPerDay, ws.UserPerDay);
                var sendingNotificationDateTime = Convert.ToDateTime(ws.StartDateTime);
                while (usersPerDayArray.Length > 0)
                {

                    var batchIndex = 0;
                    var batchLength = ws.UserPerBatch;
                    var usersPerBatchArray = usersPerDayArray.SubArrayEx(batchIndex, batchLength);
                    var batchStartSendingDateTime = sendingNotificationDateTime;

                    while (usersPerBatchArray.Length > 0)
                    {
                        var BodyJson = new BodyDefines.SendNotificationToAll
                        {
                            FileID = ws.FileID,
                            MessageType = ws.MessageType,
                            Message = ws.Message,
                            ChatIds = usersPerBatchArray
                        };

                        var jobHistory = new JobHistory() { description = $"Send Notification to all for {arrChatId.Length} users, starting at {ws.StartDateTime} " };
                        jobHistory.setInfo();

                        var param = new MySimpayBot.JobManagementService.WebServiceReportJobInput()
                        {
                            Identity = new MySimpayBot.JobManagementService.WebServiceIdentity
                            {
                                JsonWebToken = JobManagementModel.JsonWebToken
                            },
                            Parameters = new MySimpayBot.JobManagementService.WebServiceReportJobInputParams
                            {
                                EndPoint = "http://tg.Simpay.ir/v1/api/Bot/SendMessageToChat",
                                Body = Utils.ConvertClassToJson(BodyJson),
                                BusinessID = JobManagementModel.BusinessID,
                                StartDateTime = ws.TestMode ? DateTime.Now.ToString("s") : batchStartSendingDateTime.ToString("s"), //DateTime.Now.ToString("yyyy-MM-dd") + @"T13:01:00Z",
                                TypeName = TypeName,
                                DeadLineDateTime = batchStartSendingDateTime.AddMinutes(20).ToString("s"),
                                FetchExpirationAction = "queue",
                                FetchExpirationTime = 30000,
                                IntervalTime = 5000,
                                MaxFetchNumber = 2,
                                UID = jobHistory.jobUID.ToString(),
                                Description = $"Send Notification to {usersPerBatchArray.Length} chat(s)."
                            }
                        };

                        //Log.Trace("ReportJob:" + Utils.ConvertClassToJson(param), 0);

                        var jobResult = JobManagementModel.ReportJob(param);


                        Log.Info("ReportJob:" + Utils.ConvertClassToJson(jobResult), 0);
                        if (jobResult.Status.Code != "G00000")
                        {
                            resultAction = new GeneralResultAction("Panel", true, $"{jobResult.Status.Description}:{jobResult.Status.Description}");
                            Log.Error($"{jobResult.Status.Description}:{jobResult.Status.Description}", 0);
                            break;
                        }


                        batchStartSendingDateTime = batchStartSendingDateTime.AddSeconds(ws.SecondBetweenBatches);
                        batchIndex += batchLength;
                        usersPerBatchArray = usersPerDayArray.SubArrayEx(batchIndex, batchLength);
                    }

                    indexOfUserPerDay += ws.UserPerDay;
                    sendingNotificationDateTime = sendingNotificationDateTime.AddDays(1);
                    usersPerDayArray = arrChatId.SubArrayEx(indexOfUserPerDay, ws.UserPerDay);

                }






            } while (false);

        }



    }
}