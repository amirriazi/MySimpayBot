using Newtonsoft.Json;
using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;

namespace Models.EventsTicket
{
    public class Manager
    {
        public long chatId { get; set; }
        public GeneralResultAction resultAction { get; set; }

        public string category { get; set; }
        public EventsTicketInfo data { get; set; }
        public List<EventInfo> events { get; set; }


        public Manager()
        {
        }

        public Manager(long currentChatId, string currentCategory)
        {
            chatId = currentChatId;
            category = currentCategory;
            data = new EventsTicketInfo { id = 0 };
        }
        public Manager(long currentId)
        {
            data = new EventsTicketInfo { id = currentId };
            getInfo();
        }

        public Manager(string passSaleKey)
        {
            data = new EventsTicketInfo { id = 0, saleKey = passSaleKey };
            getInfo();
        }

        public void getInfo()
        {
            do
            {
                var result = new QueryResult();
                if (!String.IsNullOrEmpty(data.saleKey))
                {
                    result = DataBase.GetEventsTicketTransaction(0, data.saleKey);
                }
                else
                {
                    result = DataBase.GetEventsTicketTransaction(data.id);
                }

                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    break;
                }

                var DS = Converter.DBNull(result.DataSet);
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    chatId = (long)record["chatId"];
                    category = (string)record["category"];
                    data = new EventsTicketInfo
                    {
                        id = (long)record["id"],
                        eventUID = (string)record["eventUID"],
                        instanceUID = (string)record["instanceUID"],
                        attachmentURL = (string)record["attachmentURL"],
                        emailAddress = (string)record["emailAddress"],
                        fullName = (string)record["fullName"],
                        instanceTitle = (string)record["instanceTitle"],
                        reserveID = (long)record["reserveID"],
                        seats = (string)record["seats"],
                        venueAddress = (string)record["venueAddress"],
                        venueTitle = (string)record["venueTitle"],
                        venueCode = (long)record["venueCode"],
                        amount = (int)record["amount"],
                        saleKey = (string)record["saleKey"],
                        status = (int)record["status"],
                        query = (string)record["query"],
                        eventMethod = (string)record["eventMethod"],
                    };
                }

            } while (false);

        }

        public void setInfo()
        {
            do
            {
                var result = DataBase.SetEventsTicketTransaction(chatId, category, data.id, data.eventUID, data.instanceUID, data.emailAddress, data.fullName, data.seats, data.attachmentURL, data.instanceTitle, data.reserveID, data.venueAddress, data.venueTitle, data.venueCode, data.amount, data.saleKey, data.status, data.query);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                var DR = DS.Tables[0].Rows[0];
                data.id = Convert.ToInt64(DR["id"]);
            } while (false);


        }


        public void GetEventsList()
        {
            do
            {
                var wsInput = new wsEventsTicket.GetEventsList_Input()
                {

                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "eventsticket",
                        ActionName = "GetEventsList"
                    },
                    Parameters = new wsEventsTicket.GetEventsList_Input_Parameters
                    {
                        SessionID = SimpayCore.getSessionId(),
                        Category = category,
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsEventsTicket.GetEventsList_Output>(wsOutputResult);



                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;
                }
                events = new List<EventInfo>();
                foreach (var e in wsOutput.Parameters)
                {
                    if (e.AvailableForSale)
                    {
                        events.Add(new EventInfo
                        {
                            uniqueIdentifier = e.UniqueIdentifier,
                            amountsText = e.AmountsText,
                            availableForSale = e.AvailableForSale,
                            startDate = e.StartDate,
                            endDate = e.EndDate,
                            timesText = e.TimesText,
                            imageThumbnailURL = e.ImageThumbnailURL,
                            imageURL = e.ImageURL,
                            method = e.Method,
                            shortDescription = e.ShortDescription,
                            title = e.Title,
                            venueTitle = e.VenueTitle
                        });

                    }

                }

                setEvents();



                resultAction = new GeneralResultAction();

            } while (false);


        }



        private void setEvents()
        {
            do
            {
                var result = DataBase.SetEventsTicketEvent(data.id, events);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
            } while (false);



        }

        public EventPageing getEventInfo(int pageNumber, int recordsInPage = 1)
        {
            var RES = new EventPageing() { maxRecord = 0 };
            RES.data = new List<EventInfo>();
            do
            {
                var result = DataBase.GetEventsTicketEvents(data.id, pageNumber, recordsInPage);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                var row = 0;
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    row++;

                    RES.data.Add(new EventInfo()
                    {
                        row = row,
                        uniqueIdentifier = (string)record["uniqueIdentifier"],
                        availableForSale = (bool)record["availableForSale"],
                        title = (string)record["title"],
                        shortDescription = (string)record["shortDescription"],
                        startDate = (DateTime)record["startDate"],
                        timesText = (string)record["timesText"],
                        endDate = (DateTime)record["endDate"],
                        imageThumbnailURL = (string)record["imageThumbnailURL"],
                        imageURL = (string)record["imageURL"],
                        method = (string)record["method"],
                        amountsText = (string)record["amountsText"],
                        venueTitle = (string)record["venueTitle"],
                    });
                }

                RES.maxRecord = (int)DS.Tables[1].Rows[0][0];
            } while (false);
            return RES;

        }


        public void GetEventDetail()
        {
            resultAction = new GeneralResultAction();
            do
            {
                var wsInput = new wsEventsTicket.GetEventDetailInfo_Input()
                {

                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "eventsticket",
                        ActionName = "GetEventDetailInfo"
                    },
                    Parameters = new wsEventsTicket.GetEventDetailInfo_Input_Parameters
                    {
                        SessionID = SimpayCore.getSessionId(),
                        EventUniqueIdentifier = data.eventUID
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsEventsTicket.GetEventDetailInfo_Output>(wsOutputResult);



                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;
                }

                Utils.WriteTextToFile(Utils.ConvertClassToJson(wsOutput.Parameters), EventDetailInfoFilePath());

            } while (false);

        }

        public void GetEventInstanceSeatMap()
        {
            resultAction = new GeneralResultAction();
            do
            {
                var wsInput = new wsEventsTicket.GetEventInstanceSeatMap_Input()
                {

                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "eventsticket",
                        ActionName = "GetEventInstanceSeatMap"
                    },
                    Parameters = new wsEventsTicket.GetEventInstanceSeatMap_Input_Parameters
                    {
                        SessionID = SimpayCore.getSessionId(),
                        InstanceUniqueIdentifier = data.instanceUID,
                        SaleKey = data.saleKey
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetEventInstanceSeatMap", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsEventsTicket.GetEventInstanceSeatMap_Output>(wsOutputResult);



                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetEventInstanceSeatMap", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetEventInstanceSeatMap", true, wsOutput.Status.Description);
                    break;
                }
                data.venueCode = wsOutput.Parameters.VenueCode;
                setInfo();

                Utils.WriteTextToFile(Utils.ConvertClassToJson(wsOutput.Parameters), EventDetailInfoFilePath());

            } while (false);

        }
        public wsEventsTicket.GetEventDetailInfo_Output_Parameters GetEventDetailFromFile()
        {

            var textJson = Utils.ReadFromTextFile(EventDetailInfoFilePath());
            if (!String.IsNullOrEmpty(textJson))
            {
                return JsonConvert.DeserializeObject<wsEventsTicket.GetEventDetailInfo_Output_Parameters>(textJson);
            }
            else
            {
                return null;
            }
        }


        public void ReserveTicket()
        {
            resultAction = new GeneralResultAction();
            do
            {
                var wsInput = new wsEventsTicket.ReserveTicket_Input()
                {

                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "eventsticket",
                        ActionName = "ReserveTicket"
                    },
                    Parameters = new wsEventsTicket.ReserveTicket_Input_Parameters
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey,
                        EmailAddress = data.emailAddress,
                        FullName = data.fullName,
                        Seats = data.seats.Split(','),

                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsEventsTicket.ReserveTicket_Output>(wsOutputResult);



                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;
                }

                data.amount = wsOutput.Parameters.Amount;
                setInfo();

                Utils.WriteTextToFile(Utils.ConvertClassToJson(wsOutput.Parameters), ReserveTicketInfoFilePath());

            } while (false);

        }

        public void BuyTicket()
        {
            resultAction = new GeneralResultAction();
            do
            {
                var wsInput = new wsEventsTicket.BuyTicket_Input()
                {

                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "eventsticket",
                        ActionName = "BuyTicket"
                    },
                    Parameters = new wsEventsTicket.BuyTicket_Input_Parameters
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey,
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsEventsTicket.BuyTicket_Output>(wsOutputResult);



                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;
                }



            } while (false);

        }

        public string PrintTicket()
        {
            var ans = "";
            resultAction = new GeneralResultAction();
            do
            {
                var wsInput = new wsEventsTicket.PrintTicket_Input()
                {

                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "eventsticket",
                        ActionName = "PrintTicket"
                    },
                    Parameters = new wsEventsTicket.PrintTicket_Input_Parameters
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey,

                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsEventsTicket.PrintTicket_Output>(wsOutputResult);



                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;
                }

                Utils.WriteTextToFile(Utils.ConvertClassToJson(wsOutput.Parameters), PrintTicketFilePath());
                var info = wsOutput.Parameters;

                ans += $"نام برنامه: {info.InstanceTitle}{Environment.NewLine}";
                ans += $"محل اجرا: {info.VenueTitle}{Environment.NewLine}";
                ans += $"{info.VenueAddress}{Environment.NewLine}";
                ans += $"تلفن: {info.VenueTelPhone}{Environment.NewLine}";
                ans += $"{info.SaleName}{Environment.NewLine}";



            } while (false);
            return ans;
        }

        private string PrintTicketFilePath()
        {
            return $@"{ProjectValues.DataFolder}PrintTicketInfo_{chatId}.json";
        }

        private string EventDetailInfoFilePath()
        {
            return $@"{ProjectValues.DataFolder}EventDetailInfo_{chatId}.json";
        }
        private string ReserveTicketInfoFilePath()
        {
            return $@"{ProjectValues.DataFolder}ReserveTicketInfo_{chatId}.json";
        }
    }

    public class EventsTicketInfo
    {
        public long id { get; set; }
        public string eventUID { get; set; }
        public string instanceUID { get; set; }
        public string emailAddress { get; set; }

        public string fullName { get; set; }

        public string seats { get; set; }

        public string attachmentURL { get; set; }

        public string instanceTitle { get; set; }

        public long reserveID { get; set; }

        public string venueAddress { get; set; }

        public string venueTitle { get; set; }
        public long venueCode { get; set; }
        public int amount { get; set; }
        public string saleKey { get; set; }
        public int status { get; set; }
        public string query { get; set; }
        public string eventMethod { get; set; }

    }

    public class EventPageing
    {
        public int pageNumber { get; set; }
        public int maxRecord { get; set; }
        public List<EventInfo> data { get; set; }
    }

    public class EventInfo
    {
        public int row { get; set; }
        public string amountsText { get; set; }
        public bool availableForSale { get; set; }
        public DateTime endDate { get; set; }
        public string imageThumbnailURL { get; set; }
        public string imageURL { get; set; }
        public string method { get; set; }
        public string shortDescription { get; set; }
        public DateTime startDate { get; set; }
        public string timesText { get; set; }
        public string title { get; set; }
        public string uniqueIdentifier { get; set; }
        public string venueTitle { get; set; }


    }

}