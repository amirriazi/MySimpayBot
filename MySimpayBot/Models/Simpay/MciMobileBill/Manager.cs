using System;
using System.Collections.Generic;
using Shared.WebService;
using System.Data;
using Newtonsoft.Json;

namespace Models.MciMobileBill
{
    public class Manager
    {
        public long chatId { get; set; }

        public MciMobileBillData data { get; set; }
        public GeneralResultAction resultAction { get; set; }

        public Manager(long thisChatId, long thisId = 0)
        {
            chatId = thisChatId;
            data = new MciMobileBillData { id = thisId };
            if (thisId != 0)
                getInfo();
        }
        public void getInfo()
        {
            do
            {
                var result = DataBase.GetMciMobileBillTransaction(data.id);
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
                    data = new MciMobileBillData
                    {
                        id = (long)DR["id"],
                        mobileNumber = new Mobile() { NationalNumber = (string)DR["mobileNumber"] },
                        billId = (string)DR["billId"],
                        paymentId = (string)DR["paymentId"],
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
                var result = DataBase.SetMciMobileBillTransaction(chatId, data.id, data.mobileNumber.InternationalNumber, data.amount, data.final, data.billId, data.paymentId, data.status);
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
        public List<string> getLastChargedMobile(int count = 5)
        {

            var list = new List<string>();
            do
            {
                var result = DataBase.GetMciMobileBillLastMobile(chatId, count);
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
                    list.Add((string)DR["mobileNumber"]);
                }

            } while (false);
            return list;
        }
        public string Inquiry()
        {
            var ans = "";
            do
            {
                var wsInput = new wsMciMobileBill.BillInquiry_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "mcimobilebill",
                        ActionName = "BillInquiry"

                    },
                    Parameters = new wsMciMobileBill.BillInquiry_Input_Parameters()
                    {
                        Final = data.final,
                        MobileNumber = data.mobileNumber.InternationalNumber,
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

                var serviceResult = JsonConvert.DeserializeObject<wsMciMobileBill.BillInquiry_Output>(result);


                if (serviceResult.Status.Code == "G00002")
                {
                    Log.Error("Error: " + serviceResult.Status.Description, 0);
                    resultAction = new GeneralResultAction("BillInquiry", true, serviceResult.Status.Description);
                    break;

                }
                else if (serviceResult.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("BillInquiry", true, serviceResult.Status.Description);
                    break;
                }

                data.billId = serviceResult.Parameters.BillID;
                data.paymentId = serviceResult.Parameters.PaymentID;
                data.amount = Convert.ToInt32(serviceResult.Parameters.Amount);
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
                var wsInput = new wsMciMobileBill.Redeem_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "billpayment",
                        ActionName = "GetPurchaseInfo"
                    },
                    Parameters = new wsMciMobileBill.Redeem_Input_Parameters()
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

                var serviceResult = JsonConvert.DeserializeObject<wsMciMobileBill.Redeem_Output>(resultCore);


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
                ans += " وضعیت پرداخت قبض موبایل همراه اول  به شرح زیر میباشد:";
                var info = serviceResult.Parameters;
                ans += "\n \n ";
                ans += $" شناسه قبض: {info.BillID} ";
                ans += "\n ";
                ans += $" شناسه پرداخت: {info.PaymentID} ";
                ans += "\n ";
                ans += $" نوع: {info.BillType} ";
                ans += "\n ";
                ans += $" مبلغ:  {info.Amount} ";
                ans += "\n ";
                ans += $" شناسه پرداخت:  {info.PaymentTraceNumber} ";
                ans += "\n ";



                resultAction = new GeneralResultAction();
            } while (false);
            return ans;

        }

    }
}