using Newtonsoft.Json;
using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.BusTicket
{
    public class Manager
    {
        public long chatId { get; set; }
        public BusTicketData data { get; set; }
        public GeneralResultAction resultAction { get; set; }

        public Manager()
        {
            resultAction = new GeneralResultAction();
            data = new BusTicketData { id = 0, dateTime = DateTime.Now };
        }

        public Manager(long thisChatId)
        {
            resultAction = new GeneralResultAction();
            chatId = thisChatId;
            data = new BusTicketData { id = 0, dateTime = DateTime.Now };
        }

        public Manager(long thisChatId, long thisId = 0)
        {
            resultAction = new GeneralResultAction();
            chatId = thisChatId;
            data = new BusTicketData { id = thisId, dateTime = DateTime.Now };
            if (thisId != 0)
            {
                getInfo();
            }

        }

        public void getInfo(string passedSaleKey = "")
        {
            do
            {
                if (!string.IsNullOrEmpty(passedSaleKey))
                {
                    data.id = 0;
                }
                var result = DataBase.GetBusTicketTransaction(data.id, passedSaleKey);
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
                    data = new BusTicketData
                    {
                        id = (long)DR["id"],
                        sourceStateCode = (int)DR["sourceStateCode"],
                        sourceStateShowName = (string)DR["sourceStateShowName"],
                        sourceTerminalCode = (int)DR["sourceTerminalCode"],
                        sourceTerminalShowName = (string)DR["sourceTerminalShowName"],
                        destinationStateCode = (int)DR["destinationStateCode"],
                        destinationStateShowName = (string)DR["destinationStateShowName"],
                        destinationTerminalCode = (int)DR["destinationTerminalCode"],
                        destinationTerminalShowName = (string)DR["destinationTerminalShowName"],
                        dateTime = (DateTime)DR["dateTime"],
                        serviceUniqueIdentifier = (string)DR["serviceUniqueIdentifier"],
                        fullName = (string)DR["fullName"],
                        saleKey = (string)DR["saleKey"],
                        selectedServiceRow = (int)DR["selectedServiceRow"],
                        seatCount = (int)DR["seatCount"],
                        status = (TransactionStatusEnum)DR["status"],
                        amount = (int)DR["amount"],
                        ticketNumber = (string)DR["ticketNumber"]
                    };
                }


            } while (false);

        }
        public void setInfo()
        {
            do
            {
                var result = DataBase.SetBusTicketTransaction(chatId, data.id, data.sourceStateCode, data.sourceStateShowName, data.sourceTerminalCode, data.sourceTerminalShowName, data.destinationStateCode, data.destinationStateShowName, data.destinationTerminalCode, data.destinationTerminalShowName, data.dateTime, data.amount, data.serviceUniqueIdentifier, data.fullName, data.seatCount, data.selectedServiceRow, data.saleKey, data.status, data.ticketNumber);
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



        public List<StateData> GetListOfSourceState()
        {
            var state = new State(false);
            do
            {
                var wsInput = new wsBusTicket.GetSourceStatesList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "busticket",
                        ActionName = "GetSourceStatesList"
                    },
                    Parameters = new wsBusTicket.GetSourceStatesList_Input_Parameters()
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

                var wsOutput = JsonConvert.DeserializeObject<wsBusTicket.GetSourceStatesList_Output>(wsOutputResult);

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
                    state.data.Add(new StateData()
                    {
                        stateCode = wsOutput.Parameters[i].StateCode,
                        stateShowName = wsOutput.Parameters[i].StateShowName
                    });
                }

                state.CacheList();

                resultAction = new GeneralResultAction();
            } while (false);
            return state.data;
        }

        public List<TerminalData> GetListOfSourceTerminal()
        {
            var terminal = new Terminal(false);
            do
            {
                var wsInput = new wsBusTicket.GetSourceTerminalsList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "busticket",
                        ActionName = "GetSourceTerminalsList"
                    },
                    Parameters = new wsBusTicket.GetSourceTerminalsList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        StateCode = data.sourceStateCode
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);



                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("getListOfSourceState", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsBusTicket.GetSourceTerminalsList_Output>(wsOutputResult);

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
                    terminal.data.Add(new TerminalData()
                    {
                        terminalCode = wsOutput.Parameters[i].TerminalCode,
                        terminalShowName = wsOutput.Parameters[i].TerminalShowName
                    });
                }
                terminal.CacheList();
                resultAction = new GeneralResultAction();
            } while (false);
            return terminal.data;
        }


        public List<StateData> GetListOfDestinationState()
        {
            var state = new State(false);
            do
            {
                var wsInput = new wsBusTicket.GetDestinationStatesList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "busticket",
                        ActionName = "GetDestinationStatesList"
                    },
                    Parameters = new wsBusTicket.GetDestinationStatesList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SourceTerminalCode = data.sourceTerminalCode
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetListOfDestinationState", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsBusTicket.GetDestinationStatesList_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetListOfDestinationState", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetListOfDestinationState", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters.Length < 1)
                {
                    resultAction = new GeneralResultAction("GetListOfDestinationState", true, "پیدا نشد");
                    break;
                }


                for (var i = 0; i < wsOutput.Parameters.Length; i++)
                {
                    state.data.Add(new StateData()
                    {
                        stateCode = wsOutput.Parameters[i].StateCode,
                        stateShowName = wsOutput.Parameters[i].StateShowName
                    });
                }

                state.CacheList();

                resultAction = new GeneralResultAction();
            } while (false);
            return state.data;
        }

        public List<TerminalData> GetListOfDestinationTerminal()
        {
            var terminal = new Terminal(false);
            do
            {
                var wsInput = new wsBusTicket.GetDestinationTerminalsList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "busticket",
                        ActionName = "GetDestinationTerminalsList"
                    },
                    Parameters = new wsBusTicket.GetDestinationTerminalsList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        StateCode = data.destinationStateCode,
                        SourceTerminalCode = data.sourceTerminalCode
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);



                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("getListOfSourceState", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsBusTicket.GetDestinationTerminalsList_Output>(wsOutputResult);

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
                    terminal.data.Add(new TerminalData()
                    {
                        terminalCode = wsOutput.Parameters[i].TerminalCode,
                        terminalShowName = wsOutput.Parameters[i].TerminalShowName
                    });
                }
                terminal.CacheList();
                resultAction = new GeneralResultAction();
            } while (false);
            return terminal.data;
        }

        public List<ServiceData> GetListOfServices()
        {
            var list = new Service(data.id);
            do
            {
                var wsInput = new wsBusTicket.GetServicesList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "busticket",
                        ActionName = "GetServicesList"
                    },
                    Parameters = new wsBusTicket.GetServicesList_Input_Parameters()
                    {

                        SessionID = SimpayCore.getSessionId(),
                        SourceTerminalCode = data.sourceTerminalCode,
                        DestinationTerminalCode = data.destinationTerminalCode,
                        DateTime = data.dateTime

                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetListOfServices", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsBusTicket.GetServicesList_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetListOfServices", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetListOfServices", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters.Detail == null)
                {
                    resultAction = new GeneralResultAction("GetListOfServices", true, "سرویسی برای این مسیر و تاریخ پیدا نشد.");
                    break;
                }
                if (wsOutput.Parameters.Detail.Length < 1)
                {
                    resultAction = new GeneralResultAction("GetListOfServices", true, "پیدا نشد");
                    break;
                }



                for (var i = 0; i < wsOutput.Parameters.Detail.Length; i++)
                {
                    list.data.Add(new ServiceData()
                    {
                        amount = wsOutput.Parameters.Detail[i].Amount,
                        busType = wsOutput.Parameters.Detail[i].BusType,
                        capacity = wsOutput.Parameters.Detail[i].Capacity,
                        corporationName = wsOutput.Parameters.Detail[i].CorporationName,
                        departureDateTime = wsOutput.Parameters.Detail[i].DepartureDateTime,
                        destinationTerminalShowName = wsOutput.Parameters.Detail[i].DestinationTerminalShowName,
                        serviceUniqueIdentifier = wsOutput.Parameters.Detail[i].ServiceUniqueIdentifier,
                        sourceTerminalShowName = wsOutput.Parameters.Detail[i].SourceTerminalShowName,
                    });
                }
                list.cacheList();

                data.saleKey = wsOutput.Parameters.SaleKey;
                resultAction = new GeneralResultAction();
            } while (false);
            return list.data;
        }
        public void setSelectedServiceInfo(int serviceRow)
        {
            do
            {
                var service = new Service(data.id, true).getService(serviceRow);
                if (service == null)
                {
                    Log.Error($"Could not retrive Service row #{serviceRow}", 0);
                    break;
                }
                data.selectedServiceRow = serviceRow;
                data.amount = service.amount;
                data.serviceUniqueIdentifier = service.serviceUniqueIdentifier;

                setInfo();

            } while (false);


        }
        public void setSelectedSeatInfo()
        {
            do
            {
                var seatInfo = getSeatInfo();
                data.seatCount = seatInfo.seatCount();
                setInfo();

            } while (false);


        }

        public SeatInfo GetServiceSeats(int serviceRow)
        {
            var seatInfo = new SeatInfo();
            do
            {
                var service = new Service(data.id, true).getService(serviceRow);
                if (service == null)
                {
                    break;
                }
                var wsInput = new wsBusTicket.GetServiceSeats_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "busticket",
                        ActionName = "GetServiceSeats"
                    },
                    Parameters = new wsBusTicket.GetServiceSeats_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey,
                        ServiceUniqueIdentifier = service.serviceUniqueIdentifier
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetServiceSeats", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsBusTicket.GetServiceSeats_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetServiceSeats", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetServiceSeats", true, wsOutput.Status.Description);
                    break;
                }

                var seats = wsOutput.Parameters.Seats;
                var seatMapList = new List<SeatMap>();
                for (var i = 0; i < seats.Length; i++)
                {
                    seatMapList.Add(new SeatMap()
                    {
                        mapIndex = Convert.ToInt16(seats[i].Split('/')[0]),
                        seatNumber = Convert.ToInt16(seats[i].Split('/')[1]),
                        occupiedBy = Convert.ToInt16(seats[i].Split('/')[2]),
                    });
                }

                seatInfo = new SeatInfo()
                {
                    capacity = wsOutput.Parameters.Capacity,
                    columnNumber = wsOutput.Parameters.ColumnNumber,
                    floor = wsOutput.Parameters.Floor,
                    rowNumber = wsOutput.Parameters.RowNumber,
                    space = wsOutput.Parameters.Space,
                    seats = seatMapList
                };
                InitialSeatMap(seatInfo);
                resultAction = new GeneralResultAction();
            } while (false);
            return seatInfo;
        }

        private void InitialSeatMap(SeatInfo seatInfo)
        {
            var result = DataBase.SetBusTicketInitializeSeat(data.id, seatInfo.capacity, seatInfo.columnNumber, seatInfo.floor, seatInfo.rowNumber, seatInfo.space, seatInfo.seats);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }

        }

        public SeatInfo getSeatInfo()
        {
            var seatInfo = new SeatInfo();
            do
            {
                var result = DataBase.GetBusTicketSeat(data.id); // 2 record should be returned 1.seat info 2.seat map list
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

                var DRInfo = DS.Tables[0].Rows[0];
                var info = new SeatInfo
                {
                    capacity = (int)DRInfo["capacity"],
                    columnNumber = (int)DRInfo["columnNumber"],
                    floor = (int)DRInfo["floor"],
                    rowNumber = (int)DRInfo["rowNumber"],
                    space = (int)DRInfo["space"],
                    seats = new List<SeatMap>()

                };

                foreach (DataRow DR in DS.Tables[1].Rows)
                {
                    info.seats.Add(new SeatMap()
                    {
                        mapIndex = (int)DR["mapIndex"],
                        seatNumber = (int)DR["seatNumber"],
                        occupiedBy = (int)DR["occupiedBy"],
                        selectedByUser = (bool)DR["selectedByUser"],
                        selectedGender = (int)DR["selectedGender"],
                    });
                }
                seatInfo = info;
            } while (false);
            return seatInfo;


        }
        public void setSeatSelection(int mapIndex, bool selectedByUser)
        {
            var result = DataBase.SetBusTicketSeat(data.id, mapIndex, selectedByUser);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }
        }

        public void setSeatGender(int mapIndex, int gender)
        {
            var result = DataBase.SetBusTicketSeat(data.id, mapIndex, true, gender);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }
        }

        public void ReserveSeat()
        {
            do
            {
                var seatInfo = getSeatInfo();

                var wsInput = new wsBusTicket.ReserveSeats_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "busticket",
                        ActionName = "ReserveSeats"
                    },
                    Parameters = new wsBusTicket.ReserveSeats_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey,
                        FullName = data.fullName,
                        Seats = seatInfo.getSelectedSeat(),
                        ServiceUniqueIdentifier = data.serviceUniqueIdentifier,
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetServiceSeats", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsBusTicket.ReserveSeats_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetServiceSeats", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetServiceSeats", true, wsOutput.Status.Description);
                    break;
                }

                data.amount = wsOutput.Parameters.TotalAmount;
                setInfo();



            } while (false);
        }

        public string getTicketInfo()
        {

            var serviceData = new Service(data.id, true).getService(data.selectedServiceRow);
            var seatInfo = getSeatInfo();


            var fc = new FarsiCalendar(serviceData.departureDateTime);

            var msg = "";
            msg += " تاریخ و ساعت حرکت: \n ";
            msg += $"{fc.pDate }  \n \n ";
            msg += " مبدا: \n ";
            msg += $"{data.sourceTerminalShowName}  \n  ";
            msg += " مقصد: \n ";
            msg += $"{data.destinationTerminalShowName}  \n  ";
            msg += " نام شرکت: \n ";
            msg += $"{serviceData.corporationName}  \n  ";
            msg += " نوع اتوبوس: \n ";
            msg += $"{serviceData.busType}  \n \n";
            msg += " شماره صندلی: \n ";
            msg += $" {seatInfo.getSelectedSeatName()}  \n \n ";
            msg += $" بهای بلیط: {data.amount.ToString("#,##")} ریال \n \n ";


            return msg;
        }
        public string Redeem(string saleKey)
        {
            var msg = "";
            do
            {
                getInfo(saleKey);
                var seatInfo = getSeatInfo();

                var wsInput = new wsBusTicket.Redeem_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "busticket",
                        ActionName = "RedeemTicket"
                    },
                    Parameters = new wsBusTicket.Redeem_Input_Parameters
                    {
                        SaleKey = saleKey,
                        SessionID = SimpayCore.getSessionId()
                    }
                };

                var result = SimpayCore.InterfaceApiCall(wsInput);
                if (String.IsNullOrEmpty(result))
                {
                    msg = "Error: Cannot read request message!";
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsBusTicket.Redeem_Output>(result);
                if (wsOutput.Status.Code == "G00000")
                {

                    data.ticketNumber = wsOutput.Parameters.TicketNumber;

                    var fc = new FarsiCalendar(wsOutput.Parameters.DepartureDateTime);

                    msg += " با تشکر از اعتماد شما به سیم پی، جزییات بلیط خریداری شده بشرح زیر، به اطلاع شما میرسد: \n \n";
                    msg += $" شماره بلیط: {wsOutput.Parameters.TicketNumber} \n \n ";
                    msg += " نام مسافر: \n ";
                    msg += $"{wsOutput.Parameters.PassengerName}  \n \n ";
                    msg += " تاریخ و ساعت حرکت: \n ";
                    msg += $"{fc.pDate }  \n \n ";
                    msg += " مبدا: \n ";
                    msg += $"{wsOutput.Parameters.SourceTerminalShowName}  \n  ";
                    msg += " مقصد: \n ";
                    msg += $"{wsOutput.Parameters.DestinationTerminalShowName}  \n  ";
                    msg += " نام شرکت: \n ";
                    msg += $"{wsOutput.Parameters.CorporationName}  \n  ";
                    msg += $" تعداد صندلی:  {wsOutput.Parameters.SeatsCount}  \n  ";
                    msg += " شماره صندلی: \n ";
                    msg += $" {seatInfo.getSelectedSeatName()}  \n  ";
                    msg += $" بهای کل: {wsOutput.Parameters.TotalAmount.ToString("#,##")} ریال \n \n ";


                    break;
                }
                else if (wsOutput.Status.Code == "G00002")
                {
                    msg = "Error: " + wsOutput.Status.Description;
                    break;

                }
                else
                {
                    msg = "Error: " + wsOutput.Status.Description;
                    break;
                }

            } while (false);
            return msg;

        }

        public List<LastPath> getLastPath()
        {

            var list = new List<LastPath>();
            do
            {
                var result = DataBase.GetBusTicketLastPath(chatId, 5);
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
                        destinationTerminalShowName = (string)DR["destinationTerminalShowName"],
                        sourceTerminalShowName = (string)DR["sourceTerminalShowName"],
                        id = (long)DR["id"]
                    });
                }


            } while (false);
            return list;

        }


    }
}