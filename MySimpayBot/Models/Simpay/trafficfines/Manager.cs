using Newtonsoft.Json;
using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.TrafficFine
{
    public class Manager
    {
        public long chatId { get; set; }
        public TrafficFineData.header data { get; set; }
        public GeneralResultAction resultAction { get; set; }

        public Manager(long thisChatId)
        {
            chatId = thisChatId;
            data = new TrafficFineData.header { ticketId = 0, status = TransactionStatusEnum.NotCompeleted };
        }
        public void setInfo()
        {
            do
            {
                var result = DataBase.SetTrafficFineHeader(chatId, data.ticketId, data.barCode, data.saleKey, data.count, data.amount, data.twoPhaseInquiry, data.captchaUrl, data.captchaText, data.captchaBase64, data.status);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                var DR = DS.Tables[0].Rows[0];
                data.ticketId = (long)DR["ticketId"];

                if (data.details == null || data.details.Count <= 0)
                    break;

                result = DataBase.SetTrafficFineDetail(data.ticketId, data.details);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }


            } while (false);
        }

        public void getInfo()
        {
            getHeader();
            getDetails();
        }

        public void getHeader()
        {
            do
            {
                var result = DataBase.GetTrafficFineHeader(data.ticketId, data.saleKey);
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
                    data = new TrafficFineData.header()
                    {
                        ticketId = (long)DR["ticketId"],
                        barCode = (string)DR["barCode"],
                        saleKey = (string)DR["saleKey"],
                        count = (int)DR["count"],
                        amount = (int)DR["amount"],
                        twoPhaseInquiry = (bool)DR["twoPhaseInquiry"],
                        captchaText = (string)DR["captchaText"],
                        captchaUrl = (string)DR["captchaUrl"],
                        captchaBase64 = (string)DR["captchaBase64"],
                        status = (TransactionStatusEnum)DR["status"],
                        selectedCount = (int)DR["selectedCount"],
                        selectedAmount = (int)DR["selectedAmount"],
                    };
                }


            } while (false);
        }
        public void getDetails(int row = 0)
        {
            do
            {
                if (data?.ticketId == 0)
                {
                    break;
                }
                var result = DataBase.GetTrafficFineDetail(data.ticketId, row);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    break;
                }

                data.details = new List<TrafficFineData.Detail>();
                var DS = Converter.DBNull(result.DataSet);
                foreach (DataRow DR in DS.Tables[0].Rows)
                {
                    data.details.Add(new TrafficFineData.Detail()
                    {
                        Amount = (int)DR["Amount"],
                        City = (string)DR["City"],
                        Code = (int)DR["Code"],
                        DateTime = (DateTime)DR["DateTime"],
                        Description = (string)DR["Description"],
                        ID = (long)DR["ID"],
                        LicensePlate = (string)DR["LicensePlate"],
                        Location = (string)DR["Location"],
                        Serial = (string)DR["Serial"],
                        Type = (string)DR["Type"],
                        selected = (bool)DR["selected"],
                        row = (int)DR["row"],
                    });
                }



            } while (false);
        }
        public void getTrafficFinesInquiry()
        {
            do
            {
                var wsInput = new wsTrafficFine.TrafficFinesInquiry_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trafficfines",
                        ActionName = "TrafficFinesInquiry"
                    },
                    Parameters = new wsTrafficFine.TrafficFinesInquiry_Input_Parameters()
                    {
                        BarCode = data.barCode,
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var resultGetCount = SimpayCore.InterfaceApiCall(wsInput);


                if (String.IsNullOrEmpty(resultGetCount))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, "result is empty");
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<wsTrafficFine.TrafficFinesInquiry_Output>(resultGetCount);


                if (serviceResult.Status.Code == "G00002")
                {

                    Log.Error("Error: " + serviceResult.Status.Description, 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, serviceResult.Status.Description);
                    break;

                }
                else if (serviceResult.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("callSimpayService", true, serviceResult.Status.Description);
                    break;
                }
                data.saleKey = serviceResult.Parameters.SaleKey;
                data.twoPhaseInquiry = serviceResult.Parameters.TwoPhaseInquiry;
                setInfo();
                resultAction = new GeneralResultAction();

            } while (false);
        }
        public void getCaptcha()
        {
            do
            {
                var wsInput = new wsTrafficFine.GetCaptcha_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trafficfines",
                        ActionName = "GetCaptcha"
                    },
                    Parameters = new wsTrafficFine.GetCaptcha_Input_Parameters()
                    {
                        SaleKey = data.saleKey,
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var resultGetCount = SimpayCore.InterfaceApiCall(wsInput);


                if (String.IsNullOrEmpty(resultGetCount))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, "result is empty");
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<wsTrafficFine.GetCaptcha_Output>(resultGetCount);


                if (serviceResult.Status.Code == "G00002")
                {

                    Log.Error("Error: " + serviceResult.Status.Description, 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, serviceResult.Status.Description);
                    break;

                }
                else if (serviceResult.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("callSimpayService", true, serviceResult.Status.Description);
                    break;
                }
                data.captchaUrl = serviceResult.Parameters.CaptchaUrl;
                data.captchaBase64 = serviceResult.Parameters.CaptchaBase64;
                setInfo();
                resultAction = new GeneralResultAction();

            } while (false);
        }

        public void ResolveCaptcha()
        {
            do
            {
                var wsInput = new wsTrafficFine.SolveCaptcha_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trafficfines",
                        ActionName = "SolveCaptcha"
                    },
                    Parameters = new wsTrafficFine.SolveCaptcha_Input_Parameters()
                    {
                        SaleKey = data.saleKey,
                        SessionID = SimpayCore.getSessionId(),
                        CaptchaText = data.captchaText
                    }
                };
                var resultGetCount = SimpayCore.InterfaceApiCall(wsInput);


                if (String.IsNullOrEmpty(resultGetCount))
                {
                    data.captchaText = "";
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, "result is empty");
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<wsTrafficFine.GetCaptcha_Output>(resultGetCount);


                if (serviceResult.Status.Code == "G00002")
                {
                    data.captchaText = "";
                    Log.Error("Error: " + serviceResult.Status.Description, 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, serviceResult.Status.Description);
                    break;

                }
                else if (serviceResult.Status.Code != "G00000")
                {
                    data.captchaText = "";
                    resultAction = new GeneralResultAction("callSimpayService", true, serviceResult.Status.Description);
                    break;
                }
                resultAction = new GeneralResultAction();

            } while (false);
            setInfo();
        }


        public void GetTicketCount()
        {

            do
            {
                var wsGetCount = new wsTrafficFine.GetTicketsCount_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trafficfines",
                        ActionName = "GetTicketsCount"
                    },
                    Parameters = new wsTrafficFine.GetTicketsCount_Input_Parameters()
                    {
                        BarCode = data.barCode,
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var resultGetCount = SimpayCore.InterfaceApiCall(wsGetCount);


                if (String.IsNullOrEmpty(resultGetCount))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, "result is empty");
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<wsTrafficFine.GetTicketsCount_Output>(resultGetCount);


                if (serviceResult.Status.Code == "G00002")
                {

                    Log.Error("Error: " + serviceResult.Status.Description, 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, serviceResult.Status.Description);
                    break;

                }
                else if (serviceResult.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("callSimpayService", true, serviceResult.Status.Description);
                    break;
                }
                if (serviceResult.Parameters.TicketsCount < 1)
                {
                    resultAction = new GeneralResultAction("callSimpayService", true, "جریمه ای یافت نشد");
                    break;
                }

                data.count = Convert.ToInt32(serviceResult.Parameters.TicketsCount);
                data.amount = Convert.ToInt32(serviceResult.Parameters.TotalAmount);
                data.saleKey = serviceResult.Parameters.SaleKey;
                setInfo();

                resultAction = new GeneralResultAction();

            } while (false);

        }
        public void GetTicketDetail()
        {
            do
            {
                var wsInput = new wsTrafficFine.GetTicketsDetail_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trafficfines",
                        ActionName = "GetTicketsDetail"
                    },
                    Parameters = new wsTrafficFine.GetTicketsDetail_Input_Parameters()
                    {
                        SaleKey = data.saleKey,
                        SessionID = SimpayCore.getSessionId()
                    }
                };

                var resultGetDetail = SimpayCore.InterfaceApiCall(wsInput);
                if (String.IsNullOrEmpty(resultGetDetail))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTrafficFine.GetTicketsDetail_Output>(resultGetDetail);


                if (wsOutput.Status.Code == "G00002")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("callSimpayService", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters.TicketsCount == 0)
                {
                    resultAction = new GeneralResultAction("callSimpayService", true, "شما جریمه ای ندارید");
                    break;
                }
                data.count = wsOutput.Parameters.TicketsCount;
                data.amount = wsOutput.Parameters.TotalAmount;


                var array = wsOutput.Parameters.TicketsDetail;


                data.details = new List<TrafficFineData.Detail>();
                for (var i = 0; i < array.Length; i++)
                {
                    data.details.Add(new TrafficFineData.Detail()
                    {
                        Amount = array[i].Amount,
                        City = array[i].City,
                        Code = array[i].Code,
                        DateTime = array[i].DateTime,
                        Description = array[i].Description,
                        ID = array[i].ID,
                        LicensePlate = array[i].LicensePlate,
                        Location = array[i].Location,
                        Serial = array[i].Serial,
                        Type = array[i].Type
                    });
                }
                setInfo();

                resultAction = new GeneralResultAction();

            } while (false);

        }
        public void RowSelection(long selectedTicketId, int rowSelected, bool selected)
        {
            var result = DataBase.SetTrafficFineDetailRowSelection(selectedTicketId, rowSelected, selected);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);

            }

        }

        public bool SelectTickets()
        {
            var ans = true;
            do
            {
                if (data.selectedCount == 0)
                {
                    ans = false;
                    Log.Error("No ticket has been selected!", 0);
                    break;
                }

                getDetails();

                var selectedList = new List<long>();
                for (var i = 0; i < data.details.Count; i++)
                {
                    if (data.details[i].selected)
                        selectedList.Add(data.details[i].ID);

                }

                var wsSelection = new wsTrafficFine.SelectTickets_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trafficfines",
                        ActionName = "SelectTickets"
                    },
                    Parameters = new wsTrafficFine.SelectTickets_Input_Parameters()
                    {
                        SaleKey = data.saleKey,
                        SessionID = SimpayCore.getSessionId(),
                        TicketsList = selectedList.ToArray()
                    }
                };
                var resultGetCount = SimpayCore.InterfaceApiCall(wsSelection);
                if (String.IsNullOrEmpty(resultGetCount))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    ans = false;
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<wsTrafficFine.SelectTickets_Output>(resultGetCount);


                if (serviceResult.Status.Code == "G00002")
                {
                    Log.Error("Error: " + serviceResult.Status.Description, 0);
                    ans = false;
                    break;

                }
                else if (serviceResult.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("SelectTickets", true, serviceResult.Status.Description);
                    ans = false;
                    break;
                }
                if (serviceResult.Parameters.TicketsCount < 1)
                {
                    ans = false;
                    resultAction = new GeneralResultAction("SelectTickets", true, serviceResult.Status.Description);
                    break;
                }

                data.selectedCount = Convert.ToInt32(serviceResult.Parameters.TicketsCount);
                data.selectedAmount = Convert.ToInt32(serviceResult.Parameters.TotalAmount);

                resultAction = new GeneralResultAction();

            } while (false);
            return ans;
        }

        public void SingleTicketPayment()
        {
            do
            {
                var wsInput = new wsTrafficFine.SingleTicketPayment_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trafficfines",
                        ActionName = "SingleTicketPayment"
                    },
                    Parameters = new wsTrafficFine.SingleTicketPayment_Input_Parameters()
                    {
                        BillID = data.barCode,
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var resultGetCount = SimpayCore.InterfaceApiCall(wsInput);


                if (String.IsNullOrEmpty(resultGetCount))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTrafficFine.SingleTicketPayment_Output>(resultGetCount);


                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("callSimpayService", true, wsOutput.Status.Description);
                    break;
                }

                data.count = Convert.ToInt32(wsOutput.Parameters.Amount);
                data.saleKey = wsOutput.Parameters.SaleKey;


            } while (false);

        }
        public List<string> getLastBarcodes(int count = 5)
        {
            var list = new List<string>();
            do
            {
                var result = DataBase.GetTrafficFineLastBarcode(chatId, count);
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
                    list.Add((string)DR["barCode"]);
                }

            } while (false);
            return list;

        }
        public string Redeem(string saleKey)
        {
            var ans = "";
            do
            {
                //data = new TrafficFineData.header { saleKey = saleKey };
                var wsSelection = new wsTrafficFine.GetPurchaseInfo_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "trafficfines",
                        ActionName = "GetPurchaseInfo"
                    },
                    Parameters = new wsTrafficFine.GetPurchaseInfo_Input_Parameters()
                    {
                        SaleKey = saleKey,
                        SessionID = SimpayCore.getSessionId(),
                    }
                };
                var resultCore = SimpayCore.InterfaceApiCall(wsSelection);
                if (String.IsNullOrEmpty(resultCore))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    ans = "اطلاعاتی یافت نشد!";
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<wsTrafficFine.GetPurchaseInfo_Output>(resultCore);


                if (serviceResult.Status.Code == "G00002")
                {
                    Log.Error("Error: " + serviceResult.Status.Description, 0);
                    ans = serviceResult.Status.Description;
                    break;

                }
                else if (serviceResult.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("SelectTickets", true, serviceResult.Status.Description);
                    ans = serviceResult.Status.Description;
                    break;
                }
                if (serviceResult.Parameters.Length < 1)
                {
                    resultAction = new GeneralResultAction("SelectTickets", true, "اطلاعاتی در باره این جریمه یافت نشد");
                    ans = "اطلاعاتی در باره این جریمه یافت نشد";
                    break;
                }

                ans += " وضعیت پرداخت جریمه (ها) به شرح زیر میباشد:";
                for (var i = 0; i < serviceResult.Parameters.Length; i++)
                {
                    var info = serviceResult.Parameters[i];
                    ans += "\n \n ";
                    ans += $" شماره پلاک خودرو: {info.LicensePlate} ";
                    ans += "\n ";
                    ans += $"قبض جریمه به شماره {info.BillID} و مبلغ {info.Amount} ";
                    ans += "\n ";
                    ans += $" بابت: {info.Type} ";
                    ans += "\n ";
                    ans += $" وضعیت:  {info.PaymentStatus} ";
                    ans += "\n ";
                    ans += $"کد رهگیری:  {info.PaymentTraceNumber} ";
                    ans += "\n ";
                }

                data.status = TransactionStatusEnum.Completed;
                setInfo();


            } while (false);
            return ans;

        }
    }
}