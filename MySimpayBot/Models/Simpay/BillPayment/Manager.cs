using System;
using System.Collections.Generic;
using Shared.WebService;
using System.Data;
using Newtonsoft.Json;

namespace Models.BillPayment
{
    public class Manager
    {
        public long chatId { get; set; }

        public BillPaymentData data { get; set; }
        public GeneralResultAction resultAction { get; set; }

        public Manager(long thisChatId, long thisId = 0)
        {
            chatId = thisChatId;
            data = new BillPaymentData { id = thisId };
            if (thisId != 0)
                getInfo();
        }
        public void getInfo()
        {
            do
            {
                var result = DataBase.GetBillPaymentTransaction(data.id);
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
                    data = new BillPaymentData
                    {
                        id = (long)DR["id"],
                        billId = (string)DR["billId"],
                        paymentId = (string)DR["paymentId"],
                        billType = (string)DR["billType"],
                        saleKey = (string)DR["saleKey"],
                        status = (TransactionStatusEnum)DR["status"],
                        amount = (int)DR["amount"],
                    };
                }


            } while (false);

        }
        public void setInfo()
        {
            do
            {
                var result = DataBase.SetBillPaymentTransaction(chatId, data.id, data.amount, data.billId, data.paymentId, data.billType, data.saleKey, data.status);
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
        public List<BillPaymentLast> getLastChargedMobile(int count = 5)
        {

            var list = new List<BillPaymentLast>();
            do
            {
                var result = DataBase.GetBillPaymentLastMobile(chatId, count);
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
                    list.Add(new BillPaymentLast()
                    {
                        billId = (string)DR["billId"],
                        id = (long)DR["id"],
                        billType = (string)DR["billType"]
                    });

                }

            } while (false);
            return list;
        }

        public string SingleBillPayment()
        {
            var ans = "";
            do
            {
                var wsInput = new wsBillPayment.SingleBillPayment_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "billpayment",
                        ActionName = "SingleBillPayment"

                    },
                    Parameters = new wsBillPayment.SingleBillPayment_Input_Parameters()
                    {
                        BillID = data.billId,
                        PaymentID = data.paymentId,
                        SessionID = SimpayCore.getSessionId()
                    }
                };

                var result = SimpayCore.InterfaceApiCall(wsInput);
                if (String.IsNullOrEmpty(result))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    ans = "اطلاعاتی یافت نشد!";
                    resultAction = new GeneralResultAction("BillInquiry", true, "Error: Cannot read request message");
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<wsBillPayment.SingleBillPayment_Output>(result);


                if (serviceResult.Status.Code == "G00002")
                {
                    Log.Error(serviceResult.Status.Description, 0);
                    resultAction = new GeneralResultAction("BillInquiry", true, serviceResult.Status.Description);
                    break;

                }
                else if (serviceResult.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("BillInquiry", true, serviceResult.Status.Description);
                    break;
                }
                data.amount = Convert.ToInt32(serviceResult.Parameters.Amount);
                data.saleKey = serviceResult.Parameters.SaleKey;
                ans = serviceResult.Parameters.PaymentLink;
                setInfo();
                resultAction = new GeneralResultAction();

            } while (false);
            return ans;
        }

        public string Inquiry()
        {
            var ans = "";
            do
            {
                var wsInput = new wsBillPayment.GetBillInfo_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "billpayment",
                        ActionName = "GetBillInfo"

                    },
                    Parameters = new wsBillPayment.GetBillInfo_Input_Parameters()
                    {
                        BillID = data.billId,
                        PaymentID = data.paymentId,
                        SessionID = SimpayCore.getSessionId()
                    }
                };

                var result = SimpayCore.InterfaceApiCall(wsInput);
                if (String.IsNullOrEmpty(result))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    ans = "اطلاعاتی یافت نشد!";
                    resultAction = new GeneralResultAction("BillInquiry", true, "Error: Cannot read request message");
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<wsBillPayment.GetBillInfo_Output>(result);


                if (serviceResult.Status.Code == "G00002")
                {
                    Log.Error(serviceResult.Status.Description, 0);
                    resultAction = new GeneralResultAction("BillInquiry", true, serviceResult.Status.Description);
                    break;

                }
                else if (serviceResult.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("BillInquiry", true, serviceResult.Status.Description);
                    break;
                }
                if (!serviceResult.Parameters.BillPaymentIsValid)
                {
                    resultAction = new GeneralResultAction("BillInquiry", true, "این نوع قبض قابل پرداخت در این بخش نمیباشد.");
                    break;
                }
                if (serviceResult.Parameters.BillStatus != "قابل پرداخت می باشد")
                {
                    resultAction = new GeneralResultAction("BillInquiry", true, serviceResult.Parameters.BillStatus);
                    break;
                }
                data.amount = Convert.ToInt32(serviceResult.Parameters.BillAmount);
                data.billType = serviceResult.Parameters.BillType;
                setInfo();
                resultAction = new GeneralResultAction();

            } while (false);
            return ans;
        }

        public string Redeem(string saleKey)
        {
            var ans = "";
            do
            {
                //data = new TrafficFineData.header { saleKey = saleKey };
                var wsInput = new wsBillPayment.Redeem_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "billpayment",
                        ActionName = "GetPurchaseInfo"
                    },
                    Parameters = new wsBillPayment.Redeem_Input_Parameters()
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

                var serviceResult = JsonConvert.DeserializeObject<wsBillPayment.Redeem_Output>(resultCore);


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

                data.status = TransactionStatusEnum.Completed;
                setInfo();


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
}