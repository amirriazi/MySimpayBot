using System;
using System.Collections.Generic;
using Shared.WebService;
using System.Data;
using Newtonsoft.Json;

namespace Models.FixedLineBill
{
    public class Manager
    {
        public long chatId { get; set; }
        public FixedLineBillData data { get; set; }
        public GeneralResultAction resultAction { get; set; }

        public Manager(long thisChatId, long thisId = 0)
        {
            chatId = thisChatId;
            data = new FixedLineBillData { id = thisId };
            if (thisId != 0)
                getInfo();
        }
        public void getInfo()
        {
            do
            {
                var result = DataBase.GetFixedLineTransaction(data.id);
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
                    data = new FixedLineBillData
                    {
                        id = (long)DR["id"],
                        fixedLineNumber = (string)DR["fixedLineNumber"],
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
                var result = DataBase.SetFixedLineTransaction(chatId, data.id, data.fixedLineNumber, data.amount, data.billId, data.paymentId, data.status);
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
        public List<string> GetFixedLineLast(int count = 5)
        {

            var list = new List<string>();
            do
            {
                var result = DataBase.GetFixedLineLast(chatId, count);
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
                    list.Add((string)DR["fixedLineNumber"]);
                }

            } while (false);
            return list;
        }

        public string Inquiry()
        {
            var ans = "";
            do
            {
                var wsInput = new wsFixedLine.BillInquiry_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "fixedlinephonebill",
                        ActionName = "BillInquiry"

                    },
                    Parameters = new wsFixedLine.BillInquiry_Input_Parameters()
                    {
                        FixedLineNumber = data.fixedLineNumber,
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

                var serviceResult = JsonConvert.DeserializeObject<wsFixedLine.BillInquiry_Output>(result);


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


    }
}