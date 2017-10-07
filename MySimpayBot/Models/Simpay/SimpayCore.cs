using System;
using System.Collections.Generic;
using Shared.WebService;
using Newtonsoft.Json;
using System.Text;
using System.Data;

namespace Models
{
    public class SimpayCore
    {
        public static int SESSION_ID = 1119376;
        public static string APPLICATION_TYPE = "TelegramBot";
        public static string TOKEN = "eyJmZHQiOiIyMDE3LTAyLTIxVDA5OjU5OjA0LjgyIiwidGR0IjoiMjAzNi0wMi0yNlQyMDozOTowNC44MiIsInNlYSI6MSwicGpuIjoiaW50ZXJmYWNlIiwicHJhIjpmYWxzZSwic2lkIjoxMTE5Mzc2fQ==.cDJoKzVzRXF5bjR6VTh6VEdnR0MzYWYrQ3MwY2hoU1J0a1JqL09SdFJaV3g2L2tEMGRHNlFJdjB3TUNDaE03RA==.eyJ2bHUiOiJGRjU5QzVDQUYxMEJEMTlCRUNBRTE1NjI4NUEyRTNDQjU3RTU0ODAyRTYyMDg2RjQ4Nzk0NTBBODM4NzQ4OEEyIn0=";

        public static string API_URL = "http://interface.simpay.ir/WebServices/Core.svc/GetData";

        public static long chatId { get; set; }

        public static GeneralResultAction resultAction { get; set; }

        public static string GetJsonToken()
        {
            var Token = "";
            do
            {
                var user = new UserModel(chatId);
                Token = user.activated ? user.JsonWebToken : TOKEN;
            } while (false);
            return Token;
        }
        public static int getSessionId()
        {

            var sessionId = 0;
            do
            {
                try
                {
                    var user = new UserModel(chatId);
                    if (!user.activated)
                    {
                        sessionId = SESSION_ID;
                        break;
                    }
                    var dotIndex = user.JsonWebToken.IndexOf('.');
                    if (dotIndex < 1)
                    {
                        Log.Error("SESSION_ID annot resolve from " + user.JsonWebToken + " ", 0);

                        sessionId = SESSION_ID;
                        break;
                    }
                    var WebTokenPart = user.JsonWebToken.Split('.');
                    var headerToken = WebTokenPart[0];// token.Substring(0, dotIndex);
                    byte[] data = Convert.FromBase64String(headerToken);
                    var token = Encoding.UTF8.GetString(data);
                    var header = JsonConvert.DeserializeObject<Core.wsCore.TokenHeader>(token);
                    sessionId = header.sid;

                    //Log.Info($"Session For {user.mobileNumber} = {sessionId} ", 0);

                }
                catch (Exception ex)
                {

                    Log.Fatal("getSessionId: " + ex.Message, 0);
                    sessionId = 0;
                }
            } while (false);

            return sessionId;

        }
        public static string InterfaceApiCall(object Param)
        {
            var RES = "";
            var header = new System.Collections.Specialized.NameValueCollection();
            header.Add("Content-Type", "application/json; charset=utf-8");
            header.Add("user-agent", "ApplicationType:TelegramBot-Name:mySimpayBot-Version:1");
            try
            {
                var result = Utils.WebRequestByUrl(SimpayCore.API_URL, Utils.ConvertClassToJson(Param), header);
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

        public static Core.CallBackInfo ResolveCallBackData(string data)
        {
            var arrInfo = data.Split('-');
            var callBack = new Core.CallBackInfo
            {
                saleKey = arrInfo[0] + "-" + arrInfo[1] + "-" + arrInfo[2] + "-" + arrInfo[3] + "-" + arrInfo[4],
                status = Convert.ToInt16(arrInfo[5]),
                productId = (SimpaySectionEnum)Convert.ToInt16(arrInfo[6]),
                description = arrInfo[7]
            };
            return callBack;
        }
        public static string getPaymentLink(string saleKey)
        {
            resultAction = new GeneralResultAction();
            var ans = "";

            do
            {

                var ipgId = 0;
                var ipgList = getIPGlist(saleKey);
                if (ipgList.Count == 0)
                {
                    resultAction = new GeneralResultAction("getPaymentLink", true, "متاسفانه هم اکنون درگاه بانکی دچار مشکل است. خواهشمند است ساعاتی دیگر مجددا سعی فرمایید.");
                    break;
                }

                foreach (var ipgItem in ipgList)
                {
                    if (ipgItem.Default)
                    {
                        ipgId = ipgItem.ID;
                        break;
                    }
                }
                if (ipgId == 0)
                {
                    resultAction = new GeneralResultAction("getPaymentLink", true, "متاسفانه هم اکنون درگاه بانکی دچار مشکل است. خواهشمند است ساعاتی دیگر مجددا سعی فرمایید.");
                    break;

                }
                try
                {
                    var ws = new Core.wsCore.GetSalePaymentLink_Input
                    {
                        Identity = new Core.wsInterface.Identity()
                        {
                            JsonWebToken = SimpayCore.GetJsonToken(),
                            ServiceName = "core",
                            ActionName = "GetSalePaymentLink"
                        },
                        Parameters = new Core.wsCore.GetSalePaymentLink_Input_Parameters()
                        {
                            IPGID = ipgId,
                            SaleKey = saleKey,
                            SessionID = getSessionId()

                        }
                    };
                    var result = InterfaceApiCall(ws);
                    if (String.IsNullOrEmpty(result))
                    {
                        resultAction = new GeneralResultAction("getPaymentLink", true, "Cannot read the response!");
                        break;
                    }
                    var resultLink = JsonConvert.DeserializeObject<Core.wsCore.GetSalePaymentLink_Output>(result);
                    if (resultLink.Status.Code == "G00000")
                    {
                        ans = resultLink.Parameters.PaymentLink;
                        break;
                    }
                    else if (resultLink.Status.Code == "G00002")
                    {
                        resultAction = new GeneralResultAction("getPaymentLink", true, resultLink.Status.Description);
                        break;
                    }
                    else
                    {
                        resultAction = new GeneralResultAction("getPaymentLink", true, resultLink.Status.Description);
                        break;
                    }



                }
                catch (Exception ex)
                {

                    resultAction = new GeneralResultAction("getPaymentLink", true, "Error: " + ex.Message);
                    Log.Error(ex.Message, 0);
                    break;
                }
            } while (false);

            return ans;

        }

        public static Core.wsCore.GetSalePaymentInfo_Output_Parameters GetSalePaymentInfo(string saleKey, string discountCode = "")
        {
            var RES = new Core.wsCore.GetSalePaymentInfo_Output_Parameters();
            resultAction = new GeneralResultAction();
            do
            {
                var wsInput = new Core.wsCore.GetSalePaymentInfo_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = GetJsonToken(),
                        ServiceName = "core",
                        ActionName = "GetSalePaymentInfo"
                    },
                    Parameters = new Core.wsCore.GetSalePaymentInfo_Input_Parameters
                    {
                        DiscountCode = discountCode,
                        SaleKey = saleKey,
                        SessionID = getSessionId()
                    }
                };
                var wsOutputResult = InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    resultAction = new GeneralResultAction("GetSalePaymentInfo", true, "غیر قابل دسترس");
                    RES = null;
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<Core.wsCore.GetSalePaymentInfo_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    resultAction = new GeneralResultAction("GetSalePaymentInfo", true, wsOutput.Status.Description);
                    RES = null;
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetSalePaymentInfo", true, wsOutput.Status.Description);
                    RES = null;
                    break;
                }

                RES = wsOutput.Parameters;
            } while (false);
            return RES;
        }

        public static Core.wsCore.GetInvitationInfo_Output_Parameters GetInvitationInfo()
        {
            var RES = new Core.wsCore.GetInvitationInfo_Output_Parameters();
            do
            {
                var wsInput = new Core.wsCore.GetInvitationInfo_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "core",
                        ActionName = "GetInvitationInfo"
                    },

                    Parameters = new Core.wsCore.GetInvitationInfo_Input_Parameters
                    {
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    RES = null;
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<Core.wsCore.GetInvitationInfo_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    RES = null;
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    RES = null;
                    break;
                }
                RES = wsOutput.Parameters;
            } while (false);
            return RES;

        }

        public static Core.wsCore.ReportInvitation_Output_Parameters ReportInvitation(string refInvitationCode, string thisUserMobileNumber)
        {
            var RES = new Core.wsCore.ReportInvitation_Output_Parameters();
            do
            {
                var wsInput = new Core.wsCore.ReportInvitation_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "core",
                        ActionName = "ReportInvitation"
                    },

                    Parameters = new Core.wsCore.ReportInvitation_Input_Parameters
                    {
                        InvitationCode = refInvitationCode,
                        MobileNumber = thisUserMobileNumber
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    RES = null;
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<Core.wsCore.ReportInvitation_Output>(wsOutputResult);
                //Log.Info(Utils.ConvertClassToJson(wsOutput), 0);
                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    RES = null;
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    RES = null;
                    break;
                }
                RES = wsOutput.Parameters;
            } while (false);
            return RES;

        }
        public static Core.wsCore.GetInvitationStatus_Output_Parameters GetInvitationStatus()
        {
            var RES = new Core.wsCore.GetInvitationStatus_Output_Parameters();
            do
            {
                var wsInput = new Core.wsCore.GetInvitationStatus_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "core",
                        ActionName = "GetInvitationStatus"
                    },
                    Parameters = new Core.wsCore.GetInvitationStatus_Input_Parameters
                    {
                        SessionID = getSessionId(),
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    RES = null;
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<Core.wsCore.GetInvitationStatus_Output>(wsOutputResult);
                //Log.Info(Utils.ConvertClassToJson(wsOutput), 0);
                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    RES = null;
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    RES = null;
                    break;
                }
                RES = wsOutput.Parameters;
            } while (false);
            return RES;

        }


        public static string getRedirectPaymentUrl(SimpaySectionEnum product)
        {
            var url = "";
            switch (product)
            {
                case SimpaySectionEnum.BillPaymentProduct:
                case SimpaySectionEnum.MciMobileBill:
                case SimpaySectionEnum.FixedLineBill:
                case SimpaySectionEnum.GasBill:
                case SimpaySectionEnum.ElectricityBill:
                    url = "http://billpayment.simpay.ir/WebServices/Redirect.aspx";
                    break;
                case SimpaySectionEnum.TrafficFinesProduct:
                    url = "http://trafficfines.simpay.ir/WebServices/Redirect.aspx";
                    break;
                default:
                    break;
            }
            return url;
        }

        public static List<Core.wsCore.GetPurchaseHistoryProductsList_Output_Parameters> GetPurchaseHistoryProductsList()
        {
            var Res = new List<Core.wsCore.GetPurchaseHistoryProductsList_Output_Parameters>();
            do
            {
                //data = new TrafficFineData.header { saleKey = saleKey };
                var wsInput = new Core.wsCore.GetPurchaseHistoryProductsList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = GetJsonToken(),
                        ServiceName = "core",
                        ActionName = "GetPurchaseHistoryProductsList"
                    },
                    Parameters = new Core.wsCore.GetPurchaseHistoryProductsList_Input_Parameters()
                    {
                        FromDateTime = new DateTime(2005, 1, 1, 0, 0, 0, DateTimeKind.Local),
                        ToDateTime = new DateTime(2025, 12, 30, 0, 0, 0, DateTimeKind.Local),
                        SessionID = getSessionId(),
                    }
                };
                var resultCore = SimpayCore.InterfaceApiCall(wsInput);
                if (String.IsNullOrEmpty(resultCore))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<Core.wsCore.GetPurchaseHistoryProductsList_Output>(resultCore);


                if (wsOutput.Status.Code == "G00002")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    break;
                }
                if (wsOutput.Parameters == null)
                {
                    break;
                }
                if (wsOutput.Parameters.Length < 1)
                {
                    break;
                }

                Res.AddRange(wsOutput.Parameters);

            } while (false);
            return Res;

        }

        public static List<Core.wsCore.GetUnfinishedPaymentsInfo_Output_Parameters> GetUnfinishedPaymentsInfo()
        {
            var Res = new List<Core.wsCore.GetUnfinishedPaymentsInfo_Output_Parameters>();
            do
            {
                //data = new TrafficFineData.header { saleKey = saleKey };
                var wsInput = new Core.wsCore.GetUnfinishedPaymentsInfo_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = GetJsonToken(),
                        ServiceName = "core",
                        ActionName = "GetUnfinishedPaymentsInfo"
                    },
                    Parameters = new Core.wsCore.GetUnfinishedPaymentsInfo_Input_Parameters()
                    {
                        SessionID = getSessionId(),
                    }
                };
                var resultCore = SimpayCore.InterfaceApiCall(wsInput);
                if (String.IsNullOrEmpty(resultCore))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<Core.wsCore.GetUnfinishedPaymentsInfo_Output>(resultCore);


                if (wsOutput.Status.Code == "G00002")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    break;
                }
                if (wsOutput.Parameters == null)
                {
                    break;
                }
                if (wsOutput.Parameters.Length < 1)
                {
                    break;
                }

                Res.AddRange(wsOutput.Parameters);

            } while (false);
            return Res;

        }


        public static Core.wsHistoryData.History_Output_Parameters GetPurchaseProductsHistory(long productId, long offset = 0)
        {
            var Res = new Core.wsHistoryData.History_Output_Parameters();
            do
            {
                //data = new TrafficFineData.header { saleKey = saleKey };
                var wsInput = new Core.wsHistoryData.History_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = GetJsonToken(),
                        ServiceName = "core",
                        ActionName = "GetPurchaseHistory"
                    },
                    Parameters = new Core.wsHistoryData.History_Input_Parameters()
                    {
                        SessionID = getSessionId(),
                        FromDateTime = new DateTime(2005, 1, 1, 0, 0, 0, DateTimeKind.Local),
                        ToDateTime = new DateTime(2025, 12, 30, 0, 0, 0, DateTimeKind.Local),
                        Limit = 4,
                        Offset = offset,
                        ProductID = productId,
                    }
                };
                var resultCore = SimpayCore.InterfaceApiCall(wsInput);
                if (String.IsNullOrEmpty(resultCore))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    break;
                }
                Log.Warn(resultCore, 0);

                var wsOutput = JsonConvert.DeserializeObject<Core.wsHistoryData.History_Output>(resultCore);


                if (wsOutput.Status.Code == "G00002")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    break;
                }
                Res = wsOutput.Parameters;

            } while (false);
            return Res;

        }

        public static List<IPGInfo> getIPGlist(string saleKey)
        {
            var Res = new List<IPGInfo>();
            do
            {
                //data = new TrafficFineData.header { saleKey = saleKey };
                var wsInput = new Core.wsCore.GetIPGsList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = GetJsonToken(),
                        ServiceName = "core",
                        ActionName = "GetIPGsList"
                    },
                    Parameters = new Core.wsCore.GetIPGsList_Input_Parameters()
                    {
                        SessionID = getSessionId(),
                        SaleKey = saleKey
                    }
                };
                var resultCore = SimpayCore.InterfaceApiCall(wsInput);
                if (String.IsNullOrEmpty(resultCore))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    break;
                }
                Log.Warn(resultCore, 0);

                var wsOutput = JsonConvert.DeserializeObject<Core.wsCore.GetIPGsList_Output>(resultCore);


                if (wsOutput.Status.Code == "G00002")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    break;
                }
                foreach (var item in wsOutput.Parameters)
                {
                    Res.Add(new IPGInfo()
                    {
                        Default = item.Default,
                        ID = item.ID,
                        IPGThumbnail = item.IPGThumbnail,
                        ShowName = item.ShowName
                    });
                }

            } while (false);
            return Res;
        }

        public static long GetChatIdBySimpay(string saleKey)
        {
            long saleKeyChatId = 0;
            do
            {
                var result = DataBase.GetChatIdBySaleKey(saleKey);
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
                    saleKeyChatId = (long)DR["chatId"];
                }


            } while (false);

            return saleKeyChatId;
        }

        public static void resetAllCurrentState()
        {
            var result = DataBase.ResetAllCurrentStatus();
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }
        }

        public static void SetPaymentFinished(string saleKey)
        {
            var result = DataBase.SetPaymentFinished(saleKey);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }

        }

        public static string GeneralBillpaymentRedeem(string saleKey)
        {
            var ans = "";
            do
            {
                var wsInput = new BillPayment.wsBillPayment.Redeem_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "billpayment",
                        ActionName = "GetPurchaseInfo"
                    },
                    Parameters = new BillPayment.wsBillPayment.Redeem_Input_Parameters()
                    {
                        SaleKey = saleKey,
                        SessionID = SimpayCore.getSessionId(),
                    }
                };
                var resultCore = SimpayCore.InterfaceApiCall(wsInput);
                if (String.IsNullOrEmpty(resultCore))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    ans = "اطلاعاتی یافت نشد!";
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<BillPayment.wsBillPayment.Redeem_Output>(resultCore);


                if (serviceResult.Status.Code == "G00002")
                {
                    Log.Error("Error: " + serviceResult.Status.Description, 0);
                    ans = serviceResult.Status.Description;
                    break;

                }
                else if (serviceResult.Status.Code != "G00000")
                {
                    ans = serviceResult.Status.Description;
                    break;
                }
                var info = serviceResult.Parameters;
                ans += $" وضعیت پرداخت قبض {info.BillType} به شرح زیر میباشد:";
                ans += "\n \n ";
                ans += $"کد رهگیری:  {info.PaymentTraceNumber} ";
                ans += "\n ";
                ans += $" شناسه قبض: {info.BillID} ";
                ans += "\n ";
                ans += $" شناسه پرداخت: {info.PaymentID} ";
                ans += "\n ";
                ans += $" مبلغ:  {info.Amount.ToString("##,##")} ریال ";
                ans += "\n ";
                ans += "\n - ";

            } while (false);
            return ans;

        }
    }


    public enum BankEnum
    {
        Saman = 1,
        SamanMobile = 2
    }

    public enum TransactionStatusEnum
    {
        NotCompeleted = 0,
        Completed = 100
    }

    public enum SimpaySectionEnum
    {
        Unknown = 0,
        TrainTicket = 1,
        BusTicket = 2,
        CinemaTicket = 3,
        TrafficFinesProduct = 4,
        BillPaymentProduct = 5,
        AutoCharge = 6,
        PinCharge = 7,
        TMTNServices = 8,
        MTNServices = 800,
        MciMobileBill = 10,
        AirplaneTicket = 11,
        GasBill = 15,
        FixedLineBill = 16,
        ElectricityBill = 17,
        Drama = 41,
        XpinProduct = 100,
        Activation = 900,
        Unactivation = 901,
        Payment = 905,
        History = 910,
        Calendar = 920,
        Help = 930

    }

    public class IPGInfo
    {
        public bool Default { get; set; }
        public int ID { get; set; }
        public string IPGThumbnail { get; set; }
        public string ShowName { get; set; }


    }




}