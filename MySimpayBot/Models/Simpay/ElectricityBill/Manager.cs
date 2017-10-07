using System;
using System.Collections.Generic;
using Shared.WebService;
using System.Data;
using Newtonsoft.Json;


namespace Models.ElectricityBill
{
    public class Manager
    {
        public long chatId { get; set; }

        public ElectricityBillData data { get; set; }
        public GeneralResultAction resultAction { get; set; }

        public Manager(long thisChatId, long thisId = 0)
        {
            chatId = thisChatId;
            data = new ElectricityBillData { id = thisId };
            if (thisId != 0)
                getInfo();
        }
        public void getInfo()
        {
            do
            {
                var result = DataBase.GetElectricityBillTransaction(data.id);
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
                    data = new ElectricityBillData
                    {
                        id = (long)record["id"],
                        dateEntered = (DateTime)record["dateEntered"],
                        electricityBillID = (string)record["electricityBillID"],
                        amount = (int)record["amount"],
                        debt = (int)record["debt"],
                        fullName = (string)record["fullName"],
                        billId = (string)record["billId"],
                        paymentId = (string)record["paymentId"],
                        fromDate = (DateTime)record["fromDate"],
                        toDate = (DateTime)record["toDate"],
                        paymentDeadLineDate = (DateTime)record["paymentDeadLineDate"],
                        status = (TransactionStatusEnum)record["status"],
                    };
                }


            } while (false);

        }
        public void setInfo()
        {
            do
            {
                var result = DataBase.SetElectricityBillTransaction(chatId, data.id, data.electricityBillID, data.amount, data.debt, data.fullName, data.billId, data.paymentId, data.fromDate, data.toDate, data.paymentDeadLineDate, data.status);
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

        public List<string> getLastBill(int count = 5)
        {

            var list = new List<string>();
            do
            {
                var result = DataBase.GetElectricityLastBill(chatId, count);
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
                    list.Add((string)DR["electricityBillID"]);
                }

            } while (false);
            return list;
        }

        public string Inquiry()
        {
            var ans = "";
            do
            {
                var wsInput = new wsElectricityBill.BillInquiry_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "electricitybill",
                        ActionName = "BillInquiry"

                    },
                    Parameters = new wsElectricityBill.BillInquiry_Input_Parameters()
                    {
                        ElectricityBillID = data.electricityBillID,
                        SessionID = SimpayCore.getSessionId(),

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

                var serviceResult = JsonConvert.DeserializeObject<wsElectricityBill.BillInquiry_Output>(result);


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
                data.debt = Convert.ToInt32(serviceResult.Parameters.Debt);
                data.fullName = serviceResult.Parameters.FullName;
                data.fromDate = serviceResult.Parameters.FromDate;
                data.toDate = serviceResult.Parameters.ToDate;
                data.paymentDeadLineDate = serviceResult.Parameters.PaymentDeadLineDate;
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
                var wsInput = new wsElectricityBill.Redeem_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "billpayment",
                        ActionName = "GetPurchaseInfo"
                    },
                    Parameters = new wsElectricityBill.Redeem_Input_Parameters()
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

                var serviceResult = JsonConvert.DeserializeObject<wsElectricityBill.Redeem_Output>(resultCore);


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
                ans += " وضعیت پرداخت قبض برق به شرح زیر میباشد:";
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