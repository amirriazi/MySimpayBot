using System;
using System.Collections.Generic;
using Shared.WebService;
using Newtonsoft.Json;
using System.Data;
namespace Models.Payment
{
    public class Manager
    {
        public long chatId { get; set; }
        public PaymentData data { get; set; }

        public Manager(long reqChatId)
        {
            chatId = reqChatId;
            data = new PaymentData { id = 0 };

        }
        public Manager(long reqChatId, long currentId)
        {
            chatId = reqChatId;
            data = new PaymentData { id = currentId };
            getInfo();
        }
        public Manager(string passSaleKey)
        {

            data = new PaymentData { saleKey = passSaleKey };
            getInfoBySaleKey();
        }


        public void getInfo()
        {
            do
            {
                var result = DataBase.GetPayment(chatId, data.id);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    break;
                }
                FillObjectFromDataSet(result.DataSet);
            } while (false);

        }
        public void getInfoBySaleKey()
        {
            do
            {
                var result = DataBase.GetPaymentBySaleKey(data.saleKey);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    break;
                }
                FillObjectFromDataSet(result.DataSet, true);
            } while (false);

        }

        private void FillObjectFromDataSet(DataSet ds, bool fillChatId = false)
        {
            var DS = Converter.DBNull(ds);
            foreach (DataRow record in DS.Tables[0].Rows)
            {
                if (fillChatId)
                {
                    chatId = (long)record["chatId"];
                }

                data = new PaymentData()
                {
                    id = (long)record["id"],
                    productId = (int)record["productId"],
                    productName = (string)record["productName"],
                    saleKey = (string)record["saleKey"],
                    discountCode = (string)record["discountCode"],
                    discountAmount = (int)record["discountAmount"],
                    amount = (int)record["amount"],
                    description = (string)record["description"],
                    paymentIsPossible = (bool)record["paymentIsPossible"],
                    paymentFinished = (bool)record["paymentFinished"],
                    status = (string)record["status"],
                };
            }

        }
        public void setInfo()
        {
            do
            {
                var result = DataBase.SetPayment(chatId, data.id, data.productId, data.productName, data.saleKey, data.discountCode, data.discountAmount, data.amount, data.description, data.paymentIsPossible, data.status);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                data.id = (long)DS.Tables[0].Rows[0]["id"];
            } while (false);
        }
    }
}
