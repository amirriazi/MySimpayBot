using Newtonsoft.Json;
using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Models.PinCharge
{
    public class Manager
    {
        public long chatId { get; set; }
        public PinChargeData data { get; set; }
        public GeneralResultAction resultAction { get; set; }

        public Manager(long thisChatId)
        {
            chatId = thisChatId;
            data = new PinChargeData();
        }
        public Manager(long thisChatId, long thisRecordId)
        {
            chatId = thisChatId;
            data = new PinChargeData() { id = thisRecordId };
            getInfo();
        }

        public void getInfo()
        {
            do
            {
                var result = DataBase.GetPinChargeTransaction(data.id);
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
                    data = new PinChargeData
                    {
                        id = (long)DR["id"],
                        name = (string)DR["name"],
                        operatorName = (string)DR["operatorName"],
                        saleKey = (string)DR["saleKey"],
                        typeId = (int)DR["typeId"],
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
                var result = DataBase.SetPinChargeTransaction(chatId, data.id, data.amount, data.name, data.operatorName, data.pinCode, data.typeId, data.saleKey, data.status);
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

        public List<OperatorList> getOperatorList()
        {
            var Res = new List<OperatorList>();
            do
            {
                var wsInput = new wsPinCharge.GetChargesList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "pincharge",
                        ActionName = "GetChargesList"
                    },
                    Parameters = new wsPinCharge.GetChargesList_Input_Parameters
                    {
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsPinCharge.GetChargesList_Output>(wsOutputResult);



                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;
                }
                for (var i = 0; i < wsOutput.Parameters.Length; i++)
                {



                    //if (!Res.Contains(new OperatorList { OperatorName = wsOutput.Parameters[i].OperatorName, TypeID = wsOutput.Parameters[i].TypeId }))
                    if (!Res.Any(item => item.TypeID == wsOutput.Parameters[i].TypeId))
                    {
                        Res.Add(new OperatorList
                        {
                            OperatorName = wsOutput.Parameters[i].OperatorName,
                            TypeID = wsOutput.Parameters[i].TypeId,
                        });

                    }
                }
                resultAction = new GeneralResultAction();

            } while (false);
            return Res;

        }
        public List<ChargesList> GetChargesList(int typeId = 0)
        {
            var Res = new List<ChargesList>();
            do
            {
                var wsInput = new wsPinCharge.GetChargesList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "pincharge",
                        ActionName = "GetChargesList"
                    },
                    Parameters = new wsPinCharge.GetChargesList_Input_Parameters
                    {
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsPinCharge.GetChargesList_Output>(wsOutputResult);



                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;
                }
                for (var i = 0; i < wsOutput.Parameters.Length; i++)
                {
                    if (wsOutput.Parameters[i].TypeId == typeId)
                    {
                        Res.Add(new ChargesList
                        {
                            Amount = wsOutput.Parameters[i].Amount,
                            Name = wsOutput.Parameters[i].Name,
                            OperatorName = wsOutput.Parameters[i].OperatorName,
                            TypeID = wsOutput.Parameters[i].TypeId,
                        });
                    }
                }

                resultAction = new GeneralResultAction();

            } while (false);
            return Res;
        }

        public string getSaleKey(int typeId, int amount)
        {
            var Res = "";
            do
            {
                var wsInput = new wsPinCharge.Charge_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "pincharge",
                        ActionName = "Charge"
                    },
                    Parameters = new wsPinCharge.Charge_Input_Parameters
                    {
                        Amount = amount,
                        ChargeTypeID = typeId,
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsPinCharge.Charge_Output>(wsOutputResult);



                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;
                }

                Res = wsOutput.Parameters.saleKey;

                resultAction = new GeneralResultAction();

            } while (false);
            return Res;

        }


        public string Redeem(string saleKey)
        {
            var Res = "";
            do
            {
                var wsInput = new wsPinCharge.Redeem_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "pincharge",
                        ActionName = "RedeemCharge"
                    },
                    Parameters = new wsPinCharge.Redeem_Input_Parameters
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = saleKey
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);
                Log.Warn(wsOutputResult, 0);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsPinCharge.Redeem_Output>(wsOutputResult);



                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("GetChargesList", true, wsOutput.Status.Description);
                    break;
                }

                data.amount = wsOutput.Parameters.Amount;
                data.ChargeTypeShowName = wsOutput.Parameters.ChargeTypeShowName;
                data.description = wsOutput.Parameters.Description;
                data.pinCode = wsOutput.Parameters.PinCode;

                var message = "";
                message += "\n \n ";
                message += " پیشن شارژ ";
                message += "\n ";
                message += $" عنوان شارژ: {data.ChargeTypeShowName} ";
                message += " \n ";
                message += $" شرح : {data.description} ";
                message += " \n ";
                message += $" مبلغ: {data.amount.ToString("#,##")} ریال ";
                message += " \n ";
                message += $" شماره پین: {data.pinCode} ";
                message += " \n ";
                message += "\n \n - ";

                Res = message;
                resultAction = new GeneralResultAction();
                setInfo();
            } while (false);
            return Res;


        }

    }

    public enum ChargeListTypeEnum
    {
        Header = 1,
        Detail = 2
    }

}