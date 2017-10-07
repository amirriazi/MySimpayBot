using System;
using System.Collections.Generic;
using Shared.WebService;
using Newtonsoft.Json;
using System.Data;

namespace Models.TrainTicket
{
    public class Manager
    {
        public long chatId { get; set; }
        public bool hasServiceGo { get; set; }
        public bool hasServiceReturn { get; set; }


        public TrainTicketData data { get; set; }
        public Service serviceGo { get; set; }
        public Service serviceReturn { get; set; }

        public OptionalService optionalServiceGo { get; set; }
        public OptionalService optionalServiceReturn { get; set; }
        public GeneralResultAction resultAction { get; set; }

        public Passenger passenger { get; set; }
        public PrintTicket printTicketGo { get; set; }
        public PrintTicket printTicketReturn { get; set; }


        public Manager(long thisChatId)
        {
            resultAction = new GeneralResultAction();
            chatId = thisChatId;
            data = new TrainTicketData { id = 0 };
        }

        public Manager(long thisChatId, long thisId = 0)
        {
            resultAction = new GeneralResultAction();
            chatId = thisChatId;
            data = new TrainTicketData { id = thisId };
            if (thisId != 0)
            {
                getInfo();
            }

        }
        public void getInfo(string passedSaleKey = "")
        {
            getHeader(passedSaleKey);

            getServiceInfo();
            getOptionalServiceInfo();
            getPassengerInfo();
            getPrintInfo();

        }

        private void getHeader(string passedSaleKey = "")
        {
            do
            {
                if (!string.IsNullOrEmpty(passedSaleKey))
                {
                    data.id = 0;
                }
                var result = DataBase.GetTrainTicketTransaction(data.id, passedSaleKey);
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
                foreach (DataRow DR in DS.Tables[0].Rows)
                {
                    data = new TrainTicketData
                    {
                        id = (long)DR["id"],
                        sourceStationCode = (int)DR["sourceStationCode"],
                        sourceStationShowName = (string)DR["sourceStationShowName"],
                        destinationStationCode = (int)DR["destinationStationCode"],
                        destinationStationShowName = (string)DR["destinationStationShowName"],
                        justCompartment = (bool)DR["justCompartment"],
                        seatCount = (int)DR["seatCount"],
                        ticketTypeCode = (TicketTypeEnum)DR["ticketTypeCode"],
                        twoWay = (bool)DR["twoWay"],
                        wayGoDateTime = (DateTime)DR["wayGoDateTime"],
                        wayReturnDateTime = (DateTime)DR["wayReturnDateTime"],
                        saleKey = (string)DR["saleKey"],
                        status = (TransactionStatusEnum)DR["status"],
                        amount = (int)DR["amount"],
                        goRow = (int)DR["goRow"],
                        returnRow = (int)DR["returnRow"],
                        currentPassengerRow = (int)DR["currentPassengerRow"]

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
        private void getOptionalServiceInfo()
        {
            optionalServiceGo = new OptionalService(data.id, "go", true);
            optionalServiceReturn = new OptionalService(data.id, "return", true);

        }
        private void getPassengerInfo()
        {
            passenger = new Passenger(data.id, true);
            for (var i = passenger.data.Count; i < data.seatCount; i++)
            {
                passenger.data.Add(new PassengerData
                {
                    row = i
                });
            }

        }

        private void getPrintInfo()
        {
            printTicketGo = new PrintTicket(data.id, "go", true);
            printTicketReturn = new PrintTicket(data.id, "return", true);
        }
        public void setInfo()
        {
            do
            {
                var result = DataBase.SetTrainTicketTransaction(chatId, data.id, data.sourceStationCode, data.sourceStationShowName, data.destinationStationCode, data.destinationStationShowName, data.justCompartment, data.seatCount, data.ticketTypeCode, data.twoWay, data.wayGoDateTime, data.wayReturnDateTime, data.amount, data.saleKey, data.status, data.goRow, data.returnRow, data.currentPassengerRow, data.lockedRowNumberGo, data.lockedWagonNumberGo, data.lockedRowNumberReturn, data.lockedWagonNumberReturn);
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


        public List<StationData> GetListOfSourceStation()
        {
            var station = new Station(false);
            do
            {
                var wsInput = new wsTrainTicket.GetSourceStationsList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trainticket",
                        ActionName = "GetSourceStationsList"
                    },
                    Parameters = new wsTrainTicket.GetSourceStationsList_Input_Parameters()
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

                var wsOutput = JsonConvert.DeserializeObject<wsTrainTicket.GetSourceStationsList_Output>(wsOutputResult);

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
                    station.data.Add(new StationData()
                    {
                        stationCode = wsOutput.Parameters[i].StationCode,
                        stationShowName = wsOutput.Parameters[i].StationShowName
                    });
                }

                station.CacheList();

                resultAction = new GeneralResultAction();
            } while (false);
            return station.data;
        }
        public List<StationData> GetListOfDestinationStation()
        {
            var station = new Station(false);
            do
            {
                var wsInput = new wsTrainTicket.GetDestinationStationsList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trainticket",
                        ActionName = "GetDestinationStationsList"
                    },
                    Parameters = new wsTrainTicket.GetDestinationStationsList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SourceStationCode = data.sourceStationCode
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("getListOfSourceState", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTrainTicket.GetDestinationStationsList_Output>(wsOutputResult);

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
                    station.data.Add(new StationData()
                    {
                        stationCode = wsOutput.Parameters[i].StationCode,
                        stationShowName = wsOutput.Parameters[i].StationShowName
                    });
                }

                station.CacheList();

                resultAction = new GeneralResultAction();
            } while (false);
            return station.data;
        }

        public List<LastPath> getLastPath()
        {

            var list = new List<LastPath>();
            do
            {
                var result = DataBase.GetTrainTicketLastPath(chatId, 5);
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
                foreach (DataRow DR in DS.Tables[0].Rows)
                {
                    list.Add(new LastPath()
                    {
                        destinationStationShowName = (string)DR["destinationStationShowName"],
                        sourceStationShowName = (string)DR["sourceStationShowName"],
                        id = (long)DR["id"]
                    });
                }


            } while (false);
            return list;

        }

        public void getServices()
        {
            serviceGo = new Service(data.id, "go");
            serviceReturn = new Service(data.id, "return");
            do
            {
                var wsInput = new wsTrainTicket.GetServicesList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trainticket",
                        ActionName = "GetServicesList"
                    },
                    Parameters = new wsTrainTicket.GetServicesList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SourceStationCode = data.sourceStationCode,
                        DestinationStationCode = data.destinationStationCode,
                        TwoWay = data.twoWay,
                        WayGoDateTime = data.wayGoDateTime,
                        WayReturnDateTime = (data.twoWay ? data.wayReturnDateTime : DateTime.Now),
                        JustCompartment = data.justCompartment,
                        SeatCount = data.seatCount,
                        TicketTypeCode = Convert.ToInt16(data.ticketTypeCode)
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("getListOfSourceState", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTrainTicket.GetServicesList_Output>(wsOutputResult);

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
                if (String.IsNullOrEmpty(wsOutput.Parameters.SaleKey))
                {
                    resultAction = new GeneralResultAction("getListOfSourceState", true, "اطلاعاتی یافت نشد");
                    break;

                }
                if (wsOutput.Parameters.WayGoServicesList == null && wsOutput.Parameters.WayReturnServicesList == null)
                {
                    resultAction = new GeneralResultAction("getListOfSourceState", true, "اطلاعاتی یافت نشد");
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
                            airConditioning = service.AirConditioning,
                            arrivalTime = service.ArrivalTime,
                            availableCapacity = service.AvailableCapacity,
                            departureDateTime = service.DepartureDateTime,
                            discountedAmount = service.DiscountedAmount,
                            isCompartment = service.IsCompartment,
                            media = service.Media,
                            realAmount = service.RealAmount,
                            serviceUniqueIdentifier = service.ServiceUniqueIdentifier,
                            trainName = service.TrainName,
                            trainNumber = service.TrainNumber,
                            trainType = service.TrainType
                        });
                    }
                    serviceGo.cacheList();
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
                            airConditioning = service.AirConditioning,
                            arrivalTime = service.ArrivalTime,
                            availableCapacity = service.AvailableCapacity,
                            departureDateTime = service.DepartureDateTime,
                            discountedAmount = service.DiscountedAmount,
                            isCompartment = service.IsCompartment,
                            media = service.Media,
                            realAmount = service.RealAmount,
                            serviceUniqueIdentifier = service.ServiceUniqueIdentifier,
                            trainName = service.TrainName,
                            trainNumber = service.TrainNumber,
                            trainType = service.TrainType
                        });
                    }
                    serviceReturn.cacheList();
                }


                resultAction = new GeneralResultAction();
            } while (false);


        }
        public void GetServiceDetailInfo()
        {
            optionalServiceGo = new OptionalService(data.id, "go");
            optionalServiceReturn = new OptionalService(data.id, "return");

            var rowGo = 0;
            var rowReturn = 0;
            do
            {
                var wsInput = new wsTrainTicket.GetServiceDetailInfo_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trainticket",
                        ActionName = "GetServiceDetailInfo"
                    },
                    Parameters = new wsTrainTicket.GetServiceDetailInfo_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey,
                        WayGoServiceUniqueIdentifier = serviceGo.getService(data.goRow).serviceUniqueIdentifier,
                        WayReturnServiceUniqueIdentifier = data.twoWay ? serviceReturn.getService(data.returnRow).serviceUniqueIdentifier : null,
                    }


                };

                if (data.twoWay)
                {
                    //wsInput.Parameters.WayReturnServiceUniqueIdentifier = serviceReturn.getService(data.returnRow).serviceUniqueIdentifier;
                }

                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTrainTicket.GetServiceDetailInfo_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, wsOutput.Status.Description);
                    break;
                }

                if (wsOutput.Parameters.WayGoOptionalServicesList != null)
                {
                    foreach (var optionalService in wsOutput.Parameters.WayGoOptionalServicesList)
                    {
                        rowGo++;
                        optionalServiceGo.data.Add(new OptionalServiceData
                        {
                            row = rowGo,
                            code = optionalService.Code,
                            amount = optionalService.Amount,
                            description = optionalService.Description,
                            name = optionalService.Name,
                            optionalServiceUniqueIdentifier = optionalService.OptionalServiceUniqueIdentifier
                        });
                    }
                    optionalServiceGo.cacheList();

                }
                if (wsOutput.Parameters.WayReturnOptionalServicesList != null)
                {
                    foreach (var optionalService in wsOutput.Parameters.WayReturnOptionalServicesList)
                    {
                        rowReturn++;
                        optionalServiceReturn.data.Add(new OptionalServiceData
                        {
                            row = rowReturn,
                            code = optionalService.Code,
                            amount = optionalService.Amount,
                            description = optionalService.Description,
                            name = optionalService.Name,
                            optionalServiceUniqueIdentifier = optionalService.OptionalServiceUniqueIdentifier
                        });
                    }
                    optionalServiceReturn.cacheList();

                }

                resultAction = new GeneralResultAction();

            } while (false);

        }

        public void SetTicketInfo()
        {
            do
            {
                var wsInput = new wsTrainTicket.SetTicketInfo_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trainticket",
                        ActionName = "SetTicketInfo"
                    },
                    Parameters = new wsTrainTicket.SetTicketInfo_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey,
                        WayGoPassengersInfo = getPassengerInfoString("go"),
                        WayReturnPassengersInfo = getPassengerInfoString("return"),
                    }
                };

                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTrainTicket.SetTicketInfo_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, wsOutput.Status.Description);
                    break;
                }

                if (wsOutput.Parameters != null)
                {
                    var nationalCodeIndex = 0;// in case it is car inof there is no NationalCode we put this index instead!
                    data.amount = wsOutput.Parameters.TotalAmount;
                    setInfo();
                    foreach (var Info in wsOutput.Parameters.WayGoPassengersList)
                    {

                        if (data.ticketTypeCode == TicketTypeEnum.Car)
                        {
                            nationalCodeIndex++;
                            Info.NationalCode = nationalCodeIndex.ToString();
                        }

                        passenger.updateInfo(Info.NationalCode, Info.FirstName, Info.LastName);
                    }
                    if (data.twoWay)
                    {
                        nationalCodeIndex = 0;
                        foreach (var Info in wsOutput.Parameters.WayReturnPassengersList)
                        {

                            if (data.ticketTypeCode == TicketTypeEnum.Car)
                            {
                                nationalCodeIndex++;
                                Info.NationalCode = nationalCodeIndex.ToString();
                            }
                            passenger.updateInfo(Info.NationalCode, Info.FirstName, Info.LastName);
                        }

                    }
                }



                resultAction = new GeneralResultAction();

            } while (false);

        }

        public void LockSeat()
        {
            do
            {
                var wsInput = new wsTrainTicket.LockSeat_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trainticket",
                        ActionName = "LockSeat"
                    },
                    Parameters = new wsTrainTicket.LockSeat_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey
                    }
                };

                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTrainTicket.LockSeat_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("DetailServiceInfo", true, wsOutput.Status.Description);
                    break;
                }

                if (wsOutput.Parameters != null)
                {
                    data.lockedRowNumberGo = wsOutput.Parameters.WayGo.LockedRowNumber;
                    data.lockedWagonNumberGo = wsOutput.Parameters.WayGo.LockedWagonNumber;
                    if (wsOutput.Parameters.WayReturn != null)
                    {
                        data.lockedRowNumberReturn = wsOutput.Parameters.WayReturn.LockedRowNumber;
                        data.lockedWagonNumberReturn = wsOutput.Parameters.WayReturn.LockedWagonNumber;
                    }


                    setInfo();
                }

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
                    Log.Error(resultAction.message, 0);
                    break;
                }
                printTicket(saleKey);

            } while (false);

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

                var wsInput = new wsTrainTicket.RedeemTicket_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trainticket",
                        ActionName = "RedeemTicket"
                    },
                    Parameters = new wsTrainTicket.RedeemTicket_Input_Parameters()
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

                var wsOutput = JsonConvert.DeserializeObject<wsTrainTicket.RedeemTicket_Output>(wsOutputResult);

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
        private bool printTicket(string saleKey)
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

                var wsInput = new wsTrainTicket.PrintTickets_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trainticket",
                        ActionName = "PrintTickets"
                    },
                    Parameters = new wsTrainTicket.PrintTickets_Input_Parameters()
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

                var wsOutput = JsonConvert.DeserializeObject<wsTrainTicket.PrintTickets_Output>(wsOutputResult);

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
                        var p = passenger.getPassenger(item.NationalCode);
                        if (p == null)
                        {
                            resultAction = new GeneralResultAction("DetailServiceInfo", true, "متاسفانه اطلاعات مشتریان با بلیط صادر شده همخوانی ندارد!");
                            ans = false;
                            break;

                        }
                        printTicketGo.data.Add(new PrintTicketData()
                        {
                            row = p.row,
                            barCodeImage = item.BarCodeImage
                        });
                    }
                    printTicketGo.setInfo();
                }
                if (data.twoWay)
                {
                    if (wsOutput.Parameters.WayReturnTicketsList.Length > 0)
                    {
                        foreach (var item in wsOutput.Parameters.WayReturnTicketsList)
                        {
                            var p = passenger.getPassenger(item.NationalCode);
                            if (p == null)
                            {
                                resultAction = new GeneralResultAction("DetailServiceInfo", true, "متاسفانه اطلاعات مشتریان با بلیط صادر شده همخوانی ندارد!");
                                ans = false;
                                break;

                            }
                            printTicketReturn.data.Add(new PrintTicketData()
                            {
                                row = p.row,
                                barCodeImage = item.BarCodeImage
                            });
                        }
                        printTicketReturn.setInfo();
                    }


                }

                resultAction = new GeneralResultAction();

            } while (false);
            return ans;
        }
        public string[] getPassengerInfoString(string way)
        {
            var infoList = new List<string>();
            do
            {
                if (way == "return" && !data.twoWay)
                {
                    break;
                }
                foreach (var info in passenger.data)
                {
                    var firstName = "";
                    var lastName = "";
                    if (info.passengerTypeCode == TicketPassengerTypeEnum.CarCarriers)
                    {
                        firstName = $"{info.firstName}";
                        lastName = $"{info.lastName}";
                    }
                    else
                    {
                        firstName = $"نام کوچک مسافر {info.row}";
                        lastName = $"نام فامیل مسافر {info.row}";
                    }
                    var serviceUniqKey = "";
                    if (way == "go")
                    {
                        serviceUniqKey = optionalServiceGo.getService(info.optionalServiceGo).optionalServiceUniqueIdentifier;
                    }
                    else if (way == "return" && info.optionalServiceReturn != 0)
                    {
                        serviceUniqKey = optionalServiceReturn.getService(info.optionalServiceReturn).optionalServiceUniqueIdentifier;
                    }
                    DateTime DOB = Convert.ToDateTime(info.dateOfBirth);

                    int passengerType = Convert.ToInt32(info.passengerTypeCode);
                    var item = "";
                    if (info.passengerTypeCode == TicketPassengerTypeEnum.CarCarriers)
                    {
                        item = $"{passengerType};{firstName};{lastName};{serviceUniqKey}";
                    }
                    else
                    {
                        item = $"{passengerType};{firstName};{lastName};{info.nationalCode};{DOB.ToString("MM/dd/yyyy")};{serviceUniqKey};{info.personel}";
                    }

                    infoList.Add(item);
                }

            } while (false);
            return infoList.ToArray();
        }

        public string getTrainInfo(string wayRequest, int serviceRowRequest = 0)
        {
            var msg = "";
            do
            {
                var serviceRow = 0;

                if (wayRequest == "go")
                {
                    if (serviceRowRequest != 0)
                    {
                        serviceRow = serviceRowRequest;
                    }
                    else
                    {
                        serviceRow = data.goRow;
                    }
                }
                else if (wayRequest == "return")
                {
                    if (serviceRowRequest != 0)
                    {
                        serviceRow = serviceRowRequest;
                    }
                    else
                    {
                        serviceRow = data.goRow;
                    }
                }
                else
                {
                    msg = "رفت یا برگشت تعیین نشده است!";
                    break;
                }
                var service = new Service(data.id, wayRequest, true);
                var serviceData = service.data[serviceRow - 1];
                var fc = new FarsiCalendar(serviceData.departureDateTime);
                var hasCompartment = serviceData.isCompartment ? "بله " : " خیر ";
                var hasMedia = serviceData.media ? "بله " : " خیر ";
                var hasAC = serviceData.airConditioning ? "بله " : " خیر ";

                msg += " \n ";
                msg += $"شماره قطار:  {serviceData.trainNumber}  \n \n";
                msg += " نام قطار: \n ";
                msg += $"{serviceData.trainName}  \n \n";
                msg += " نوع قطار: \n ";
                msg += $"{serviceData.trainType}  \n \n";

                msg += $" کوپه دارد: {hasCompartment }  \n \n";
                msg += $" صوت و تصویر: {hasCompartment }  \n \n";
                msg += $" تهویه مطبوع دارد: {hasCompartment }  \n \n";


                msg += " تاریخ و ساعت حرکت: \n ";
                msg += $"{fc.pDate }  \n \n ";

                msg += $" ساعت رسیدن {serviceData.arrivalTime}: \n \n ";
                msg += $" تعداد مسافرین{data.seatCount}: \n \n ";
                foreach (var p in passenger.data)
                {
                    msg += $"مسافر شماره {p.row}: \n";
                    msg += $"نام : {p.firstName} {p.lastName} \n";
                    msg += $"نوع مسافر: {p.passengerTypeShowName} \n";
                    if (p.passengerTypeCode == TicketPassengerTypeEnum.Martyr || p.passengerTypeCode == TicketPassengerTypeEnum.Veteran)
                    {
                        msg += $"کد ایثارگری: {p.personel} \n";
                    }
                }

                msg += $" بهای بلیط : {data.amount.ToString("#,##")} ریال \n \n ";


                msg += " \n ";
                msg += " \n ";





            } while (false);



            return msg;

        }

        public string getTicketTypeName(TicketTypeEnum ticketType)
        {
            var ans = "";
            switch (ticketType)
            {
                case TicketTypeEnum.Unknown:
                    ans = "نا مشخص";
                    break;
                case TicketTypeEnum.Normal:
                    ans = "عادی";
                    break;
                case TicketTypeEnum.Men:
                    ans = "ویژه برادران";
                    break;
                case TicketTypeEnum.Women:
                    ans = "ویژه خواهران";
                    break;
                case TicketTypeEnum.Car:
                    ans = "ویژه حمل خودرو";
                    break;
                default:
                    break;
            }
            return ans;
        }
        public string GetPassengerType(TicketPassengerTypeEnum typeId)
        {
            var ans = "";
            switch (typeId)
            {
                case TicketPassengerTypeEnum.Unknown:
                    ans = "نا مشخص";
                    break;
                case TicketPassengerTypeEnum.Adult:
                    ans = "بزرگسال";
                    break;
                case TicketPassengerTypeEnum.Child:
                    ans = "خردسال";
                    break;
                case TicketPassengerTypeEnum.Martyr:
                    ans = "شاهد";
                    break;
                case TicketPassengerTypeEnum.Veteran:
                    ans = "ایثارگر";
                    break;
                case TicketPassengerTypeEnum.Fraction:
                    ans = "کسر کوپه";
                    break;
                case TicketPassengerTypeEnum.Baby:
                    ans = "نوزاد";
                    break;
                case TicketPassengerTypeEnum.CarCarriers:
                    ans = "حمل خودرو";
                    break;
                case TicketPassengerTypeEnum.Foreigners:
                    ans = "مسافر خارجی";
                    break;
                default:
                    ans = $"{typeId}";
                    break;
            }
            return ans;
        }

    }
    public enum TicketTypeEnum
    {
        Unknown = 0,
        Normal = 1,
        Men = 2,
        Women = 3,
        Car = 4
    }
    public enum TicketPassengerTypeEnum
    {
        Unknown = 0,
        Adult = 1,
        Child = 2,
        Martyr = 3, //شاهد
        Veteran = 4, //جانباز
        Fraction = 5, //کسر کوپه
        Baby = 6,
        CarCarriers = 7,
        Foreigners = 8
    }
}