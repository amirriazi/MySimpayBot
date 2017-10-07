using System;
using System.Collections.Generic;
using Shared.WebService;
using Newtonsoft.Json;
using System.Data;

namespace Models.AirplaneTicket
{
    public class Manager
    {
        public long chatId { get; set; }
        public bool hasServiceGo { get; set; }
        public bool hasServiceReturn { get; set; }
        public AirplaneTicketData data { get; set; }
        public Service serviceGo { get; set; }
        public Service serviceReturn { get; set; }

        public Passenger passenger { get; set; }
        public GeneralResultAction resultAction { get; set; }

        public int seatCount
        {
            get { return data.adultCount + data.childCount + data.infantCount; }
        }


        public Manager(long thisChatId)
        {
            resultAction = new GeneralResultAction();
            chatId = thisChatId;
            data = new AirplaneTicketData { id = 0 };
        }

        public Manager(long thisChatId, long thisId = 0)
        {
            resultAction = new GeneralResultAction();
            chatId = thisChatId;
            data = new AirplaneTicketData { id = thisId };
            if (thisId != 0)
            {
                getInfo();
            }

        }

        public void getInfo(string passedSaleKey = "")
        {
            getHeader(passedSaleKey);
            getServiceInfo();
            getPassengerInfo();

        }

        private void getHeader(string passedSaleKey = "")
        {
            do
            {
                if (!string.IsNullOrEmpty(passedSaleKey))
                {
                    data.id = 0;
                }
                var result = DataBase.GetAirplaneTicketTransaction(data.id, passedSaleKey);
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
                    data = new AirplaneTicketData
                    {
                        id = (long)record["id"],
                        userId = (int)record["userId"],
                        dateEntered = (DateTime)record["dateEntered"],
                        sourceAirportCode = (string)record["sourceAirportCode"],
                        sourceAirportShowName = (string)record["sourceAirportShowName"],
                        destinationAirportCode = (string)record["destinationAirportCode"],
                        destinationAirportShowName = (string)record["destinationAirportShowName"],
                        twoWay = (bool)record["twoWay"],
                        wayGoDateTime = (DateTime)record["wayGoDateTime"],
                        wayReturnDateTime = (DateTime)record["wayReturnDateTime"],
                        adultCount = (int)record["adultCount"],
                        childCount = (int)record["childCount"],
                        infantCount = (int)record["infantCount"],
                        amount = (int)record["amount"],
                        saleKey = (string)record["saleKey"],
                        goRow = (int)record["goRow"],
                        returnRow = (int)record["returnRow"],
                        status = (TransactionStatusEnum)record["status"],
                        currentPassengerRow = (int)record["currentPassengerRow"],

                    };
                    if (data.wayGoDateTime == DateTime.MinValue)
                    {
                        data.wayGoDateTime = null;
                    }
                    if (data.wayReturnDateTime == DateTime.MinValue)
                    {
                        data.wayReturnDateTime = null;
                    }
                }

            } while (false);


        }
        private void getServiceInfo()
        {

            serviceGo = new Service(data.id, "go", true);
            serviceReturn = new Service(data.id, "return", true);

        }

        private void getPassengerInfo()
        {
            passenger = new Passenger(data.id, true);
            for (var i = passenger.data.Count; i < seatCount; i++)
            {
                passenger.data.Add(new PassengerData
                {
                    row = i
                });
            }
        }


        public void setInfo()
        {
            do
            {
                var result = DataBase.SetAirplaneTicketTransaction(chatId, data.id, data.sourceAirportCode, data.sourceAirportShowName, data.destinationAirportCode, data.destinationAirportShowName, data.twoWay, data.wayGoDateTime, data.wayReturnDateTime, data.adultCount, data.childCount, data.infantCount, data.amount, data.saleKey, data.goRow, data.returnRow, data.currentPassengerRow, data.status);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                var DR = DS.Tables[0].Rows[0];
                data.id = Convert.ToInt32(DR["id"]);
            } while (false);


        }

        public void updateAirportList()
        {
            var airport = new Airport(false);
            do
            {
                var wsInput = new wsAirplane.GetSourceStationsList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "airplaneticket",
                        ActionName = "GetSourceAirportsList"
                    },
                    Parameters = new wsAirplane.GetSourceAirportsList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("getListOfSourceState", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsAirplane.GetSourceAirportsList_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("getListOfSourceState", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("getListOfSourceState", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters.Length < 1)
                {
                    resultAction = new GeneralResultAction("getListOfSourceState", true, "پیدا نشد");
                    break;
                }


                for (var i = 0; i < wsOutput.Parameters.Length; i++)
                {
                    if (wsOutput.Parameters[i].AirportCode == "THR")
                    {
                        wsOutput.Parameters[i].AirportShowName += " تهران ";
                    }
                    airport.data.Add(new AirportData()
                    {
                        airportCode = wsOutput.Parameters[i].AirportCode,
                        airportShowName = wsOutput.Parameters[i].AirportShowName.Replace("بین‌المللی", "").Replace("فرودگاه", "")
                    });

                }

                airport.CacheList();

                resultAction = new GeneralResultAction();
            } while (false);
        }

        public void getServices()
        {
            serviceGo = new Service(data.id, "go");
            serviceReturn = new Service(data.id, "return");
            do
            {
                var wsInput = new wsAirplane.GetServicesList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "airplaneticket",
                        ActionName = "GetServicesList"
                    },
                    Parameters = new wsAirplane.GetServicesList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        TwoWay = data.twoWay,
                        AdultCount = data.adultCount,
                        ChildCount = data.childCount,
                        InfantCount = data.infantCount,
                        SourceAirportCode = data.sourceAirportCode,
                        DestinationAirportCode = data.destinationAirportCode,
                        WayGoDateTime = Convert.ToDateTime(data.wayGoDateTime),
                        WayReturnDateTime = (data.twoWay ? Convert.ToDateTime(data.wayReturnDateTime) : DateTime.Now),

                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsAirplane.GetServicesList_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;
                }
                if (String.IsNullOrEmpty(wsOutput.Parameters.SaleKey))
                {
                    resultAction = new GeneralResultAction("getListOfSourceState", true, "اطلاعاتی یافت نشد");
                    break;

                }
                if (wsOutput.Parameters.WayGoServicesList == null && wsOutput.Parameters.WayReturnServicesList == null)
                {
                    resultAction = new GeneralResultAction("", true, "اطلاعاتی یافت نشد");
                    break;

                }
                data.saleKey = wsOutput.Parameters.SaleKey;
                setInfo();

                if (wsOutput.Parameters.WayGoServicesList != null)
                {
                    hasServiceGo = true;
                    var rowGo = 0;
                    foreach (var service in wsOutput.Parameters.WayGoServicesList)
                    {
                        rowGo++;
                        serviceGo.data.Add(new ServiceData
                        {
                            row = rowGo,
                            aircraft = service.Aircraft,
                            airlineCode = service.AirlineCode,
                            sourceAirportCode = service.SourceAirportCode,
                            amountAdult = service.AmountAdult,
                            amountChild = service.AmountChild,
                            amountInfant = service.AmountInfant,
                            Class = service.Class,
                            classType = service.ClassType,
                            classTypeName = service.ClassTypeName,
                            dayOfWeek = service.DayOfWeek,
                            description = service.Description,
                            destinationAirportCode = service.DestinationAirportCode,
                            flightID = service.FlightID,
                            flightNumber = service.FlightNumber,
                            isCharter = service.IsCharter,
                            sellerId = service.SellerID,
                            sellerReference = service.SellerReference,
                            status = service.Status,
                            statusName = service.StatusName,
                            systemId = service.ID,
                            systemKey = service.SystemKey,
                            arrivalTime = service.ArrivalTime,
                            departureDateTime = service.DepartureDateTime,
                            serviceUniqueIdentifier = service.ServiceUniqueIdentifier,
                        });
                    }
                    serviceGo.setInfo();
                }


                if (wsOutput.Parameters.WayReturnServicesList != null)
                {
                    var rowReturn = 0;
                    foreach (var service in wsOutput.Parameters.WayReturnServicesList)
                    {
                        hasServiceReturn = true;
                        rowReturn++;
                        serviceReturn.data.Add(new ServiceData
                        {
                            row = rowReturn,
                            aircraft = service.Aircraft,
                            airlineCode = service.AirlineCode,
                            sourceAirportCode = service.SourceAirportCode,
                            amountAdult = service.AmountAdult,
                            amountChild = service.AmountChild,
                            amountInfant = service.AmountInfant,
                            Class = service.Class,
                            classType = service.ClassType,
                            classTypeName = service.ClassTypeName,
                            dayOfWeek = service.DayOfWeek,
                            description = service.Description,
                            destinationAirportCode = service.DestinationAirportCode,
                            flightID = service.FlightID,
                            flightNumber = service.FlightNumber,
                            isCharter = service.IsCharter,
                            sellerId = service.SellerID,
                            sellerReference = service.SellerReference,
                            status = service.Status,
                            statusName = service.StatusName,
                            systemId = service.ID,
                            systemKey = service.SystemKey,
                            arrivalTime = service.ArrivalTime,
                            departureDateTime = service.DepartureDateTime,
                            serviceUniqueIdentifier = service.ServiceUniqueIdentifier,

                        });
                    }
                    serviceReturn.setInfo();
                }


                resultAction = new GeneralResultAction();
            } while (false);

        }



        public void ReserveTicket()
        {
            var airport = new Airport(false);
            do
            {
                var passengerInfo = new List<string>();

                foreach (var info in passenger.data)
                {
                    //var dob = Convert.ToDateTime(info.dateOfBirth).ToString("dd/MM/yyyy");
                    var dob = Convert.ToDateTime(info.dateOfBirth);
                    passengerInfo.Add($"{info.passengerTypeCode.ToUpper()};{info.title.ToUpper()};{info.firstName};{info.lastName};{dob};{info.nationalCode};;;");
                }
                //AgeType;Title;FirstName;LastName;BirthDate;NationalCode;PassportNumber;PassportExpiryDate;PassportPlaceOfIssue
                var wsInput = new wsAirplane.ReserveTicket_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "airplaneticket",
                        ActionName = "ReserveTicket"
                    },
                    Parameters = new wsAirplane.ReserveTicket_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        PassengersInfo = passengerInfo.ToArray(),
                        SaleKey = data.saleKey,
                        WayGoServiceUniqueIdentifier = serviceGo.getService(data.goRow).serviceUniqueIdentifier,
                        WayReturnServiceUniqueIdentifier = data.twoWay ? serviceReturn.getService(data.returnRow).serviceUniqueIdentifier : null,
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsAirplane.ReserveTicket_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;
                }
                data.amount = wsOutput.Parameters.TotalAmount;
                setInfo();
                resultAction = new GeneralResultAction();
            } while (false);
        }

        public void Redeem(string saleKey)
        {
            do
            {
                redeemTicket(saleKey);
                if (resultAction.hasError)
                {
                    break;
                }
                PrintTicket(saleKey);
                if (resultAction.hasError)
                {
                    break;
                }
            } while (false);
            //return ans;

        }
        private bool redeemTicket(string saleKey)
        {
            var ans = true;
            do
            {
                getInfo(saleKey);
                if (data.status == TransactionStatusEnum.Completed)
                {
                    break;
                }

                var wsInput = new wsAirplane.RedeemTicket_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "airplaneticket",
                        ActionName = "RedeemTicket"
                    },
                    Parameters = new wsAirplane.RedeemTicket_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = saleKey
                    }
                };

                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    ans = false;
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsAirplane.RedeemTicket_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    ans = false;
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, wsOutput.Status.Description);
                    ans = false;
                    break;
                }

                data.status = TransactionStatusEnum.Completed;
                setInfo();
                resultAction = new GeneralResultAction();

            } while (false);
            return ans;
        }

        private bool PrintTicket(string saleKey)
        {
            var ans = true;
            do
            {
                getInfo(saleKey);
                if (data.status != TransactionStatusEnum.Completed)
                {
                    ans = false;
                    break;
                }

                var wsInput = new wsAirplane.PrintTickets_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "airplaneticket",
                        ActionName = "PrintTickets"
                    },
                    Parameters = new wsAirplane.PrintTickets_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = saleKey
                    }
                };

                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    ans = false;
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsAirplane.PrintTickets_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    ans = false;
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, wsOutput.Status.Description);
                    ans = false;
                    break;
                }

                if (wsOutput.Parameters.WayGoTicketsList.Length > 0)
                {
                    foreach (var item in wsOutput.Parameters.WayGoTicketsList)
                    {
                        var p = passenger.getPassenger(item.FirstName, item.LastName);
                        if (p != null)
                        {
                            passenger.data[p.row - 1].htmlGo = item.Html;
                            passenger.setInfo(p.row);
                        }

                    }
                }
                if (data.twoWay)
                {
                    if (wsOutput.Parameters.WayReturnTicketsList.Length > 0)
                    {
                        foreach (var item in wsOutput.Parameters.WayReturnTicketsList)
                        {
                            var p = passenger.getPassenger(item.FirstName, item.LastName);
                            if (p != null)
                            {
                                passenger.data[p.row - 1].htmlReturn = item.Html;
                                passenger.setInfo(p.row);
                            }

                        }
                    }


                }

                resultAction = new GeneralResultAction();

            } while (false);
            return ans;

        }

        public string GetPassengerTypeShowName(string passengerTypeCode)
        {
            var ans = "";
            switch (passengerTypeCode.ToUpper())
            {
                case "ADL":
                    ans = "بزرگسال";
                    break;
                case "CHD":
                    ans = "کودک";
                    break;
                case "INF":
                    ans = "نوزاد";
                    break;
                default:
                    break;
            }
            return ans;
        }
        public string GetPassengerTitleShowName(string TitleCode)
        {
            var ans = "";
            switch (TitleCode.ToUpper())
            {
                case "MR":
                    ans = "آقا";
                    break;
                case "MS":
                    ans = "خانم";
                    break;
                default:
                    break;
            }
            return ans;
        }
        public string GetPassengerTitleCode(string TitleShowName)
        {
            var ans = "";
            switch (TitleShowName)
            {
                case "آقا":
                    ans = "MR";
                    break;
                case "خانم":
                    ans = "MS";
                    break;
                default:
                    break;
            }
            return ans;
        }


    }


}