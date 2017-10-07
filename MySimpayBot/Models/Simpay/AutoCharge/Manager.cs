using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;


namespace Models.AutoCharge
{
    public class Manager
    {
        public long chatId { get; set; }
        public Transaction transaction { get; set; }
        public GeneralResultAction actionResult { get; set; }
        public Manager(string saleKey)
        {

        }

        public Manager(long theChatId)
        {
            chatId = theChatId;
        }

        public List<Operator> GetAutoChargeOperatorList()
        {
            var list = new List<Operator>();
            do
            {
                var result = DataBase.GetAutoChargeOperatorList();
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
                    list.Add(new Operator()
                    {
                        chargeName = (string)DR["chargeName"],
                        chargeTypeId = (int)DR["chargeTypeId"],
                        operatorName = (string)DR["operatorName"]

                    });
                }


            } while (false);
            return list;

        }
        public List<ChargeList> GetAutoChargeList(int chargeTypeId)
        {
            var list = new List<ChargeList>();
            do
            {
                var result = DataBase.GetAutoChargeList(chargeTypeId);
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
                    list.Add(new ChargeList()
                    {
                        amount = (int)DR["amount"]

                    });
                }


            } while (false);
            return list;

        }

        public List<LastChargedMobiles> getLastChargedMobile()
        {

            var list = new List<LastChargedMobiles>();
            do
            {
                var result = DataBase.GetAutoChargeLastMobile(chatId, 5);
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
                    list.Add(new LastChargedMobiles()
                    {
                        mobileNumber = (string)DR["mobileNumber"],
                        id = (int)DR["id"]
                    });
                }


            } while (false);
            return list;

        }
        public void getTransaction(int id = 0, string saleKey = "")
        {
            do
            {
                var result = DataBase.GetAutoChargeTransaction(id, saleKey);
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
                    transaction = new Transaction()
                    {
                        id = (int)DR["id"],
                        chargeTypeId = (int)DR["chargeTypeId"],
                        amount = (int)DR["amount"],
                        mobileNumber = (string)DR["mobileNumber"] != string.Empty ? new Mobile() { Number = (string)DR["mobileNumber"] } : null,
                        saleKey = (string)DR["saleKey"],
                        status = (TransactionStatusEnum)DR["status"],
                        transactionId = (string)DR["transactionId"],
                    };
                }


            } while (false);

        }
        public void setTransaction(int id = 0)
        {
            do
            {
                var result = DataBase.SetAutoChargeTransaction(chatId, id, transaction.mobileNumber.InternationalNumber, transaction.chargeTypeId, transaction.amount, transaction.saleKey, transaction.status, transaction.transactionId);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                var DR = DS.Tables[0].Rows[0];
                transaction.id = (int)DR["id"];
            } while (false);


        }
        public string getSaleKey()
        {
            var ans = "";
            actionResult = new GeneralResultAction();
            do
            {
                var ws = new wsAutoCharge.Charge_Input
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "autocharge",
                        ActionName = "charge"
                    },
                    Parameters = new wsAutoCharge.Charge_Input_Parameters
                    {
                        Amount = transaction.amount,
                        ChargeTypeID = transaction.chargeTypeId,
                        MobileNumber = transaction.mobileNumber.InternationalNumber,
                        SessionID = SimpayCore.getSessionId()
                    }
                };

                var result = SimpayCore.InterfaceApiCall(ws);

                Def.MyDbLogger.action = "AutoCharge.GetSaleKey.Get";
                Def.MyDbLogger.playLoad = $" Get={Utils.ConvertClassToJson(ws.Parameters)}";
                Def.MyDbLogger.reportLog();

                Def.MyDbLogger.action = "AutoCharge.GetSaleKey.Ret";
                Def.MyDbLogger.playLoad = $" Return={Utils.ConvertClassToJson(result)}";
                Def.MyDbLogger.reportLog();

                //Log.Info($"getSaleKey: {Utils.ConvertClassToJson(result)}", 0);
                if (String.IsNullOrEmpty(result))
                {

                    ans = "متاسفانه سامانه پاسخگو نیست!";
                    actionResult = new GeneralResultAction("getSaleKey", true, ans);
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<wsAutoCharge.Charge_Output>(result);
                if (serviceResult.Status.Code == "G00000")
                {
                    ans = serviceResult.Parameters.SaleKey;

                    break;
                }
                else if (serviceResult.Status.Code == "G00002")
                {
                    ans = "Error: " + serviceResult.Status.Description;
                    actionResult = new GeneralResultAction("getSaleKey", true, serviceResult.Status.Description);
                    break;

                }
                else
                {
                    ans = "Error: " + serviceResult.Status.Description;
                    actionResult = new GeneralResultAction("getSaleKey", true, serviceResult.Status.Description);
                    break;
                }


            } while (false);

            return ans;

        }
        public string Redeem(string saleKey)
        {
            var ans = "";
            actionResult = new GeneralResultAction();
            do
            {
                var ws = new wsAutoCharge.Redeem_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "autocharge",
                        ActionName = "RedeemCharge"
                    },
                    Parameters = new wsAutoCharge.Redeem_Input_Parameters
                    {
                        SaleKey = saleKey,
                        SessionID = SimpayCore.getSessionId()
                    }
                };

                var result = SimpayCore.InterfaceApiCall(ws);
                if (String.IsNullOrEmpty(result))
                {
                    ans = "Error: Cannot read request message!";
                    actionResult = new GeneralResultAction("getSaleKey", true, ans);
                    break;
                }

                var serviceResult = JsonConvert.DeserializeObject<wsAutoCharge.Redeem_Output>(result);
                if (serviceResult.Status.Code == "G00000")
                {

                    setTransactionSuccess(saleKey);

                    ans += " با تشکر از اعتماد شما به سیم پی، جزییات شارژ خریداری شده بشرح زیر، به اطلاع شما میرسد:" + "\n \n";
                    ans += " شماره موبایل: \n ";
                    ans += serviceResult.Parameters.MobileNumber;
                    ans += "\n \n ";
                    ans += " نوع شارژ: \n ";
                    ans += serviceResult.Parameters.ChargeTypeShowName + "\n";
                    ans += "\n \n ";
                    ans += " مبلغ شارژ:  \n ";
                    ans += serviceResult.Parameters.Amount + " ریال \n ";
                    ans += "\n \n -";

                    break;
                }
                else if (serviceResult.Status.Code == "G00002")
                {
                    ans = "Error: " + serviceResult.Status.Description;
                    break;

                }
                else
                {
                    ans = "Error: " + serviceResult.Status.Description;
                    break;
                }

            } while (false);
            return ans;
        }
        private void setTransactionSuccess(string saleKey)
        {
            getTransaction(saleKey: saleKey);
            if (transaction.id != 0)
            {
                transaction.status = TransactionStatusEnum.Completed;
                setTransaction(transaction.id);
            }
        }


    }
}