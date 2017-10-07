using Newtonsoft.Json;
using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Models.TMTN
{
    public class Manager
    {
        public long chatId { get; set; }
        public TMTNData data { get; set; }

        public GeneralResultAction resultAction { get; set; }

        public SimType simtype { get; set; }
        public Category category { get; set; }
        public Package package { get; set; }


        public Manager(long reqChatId)
        {
            data = new TMTNData { id = 0 };
            chatId = reqChatId;

        }
        public Manager(long reqChatId, long id = 0)
        {
            chatId = reqChatId;
            data = new TMTNData { id = id };
            getInfo();
        }
        public void getInfo(string passedSaleKey = "")
        {
            getTransaction(passedSaleKey);
            getSimType();
            getCategory();
            getPackage();


        }

        private void getTransaction(string passedSaleKey = "")
        {
            do
            {
                if (!string.IsNullOrEmpty(passedSaleKey))
                {
                    data.id = 0;
                }
                var result = DataBase.GetTMTNTransaction(data.id, passedSaleKey);
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
                    data = new TMTNData
                    {
                        id = (long)record["id"],
                        simTypeId = (int)record["simTypeId"],
                        simTypeShowName = (string)record["simTypeShowName"],
                        categoryId = (int)record["categoryId"],
                        categoryShowName = (string)record["categoryShowName"],
                        packageId = (int)record["packageId"],
                        packageShowName = (string)record["packageShowName"],
                        packageDescription = (string)record["packageDescription"],
                        amount = (int)record["amount"],
                        mobileNumber = (string)record["mobileNumber"],
                        saleKey = (string)record["saleKey"],
                        status = (TransactionStatusEnum)record["status"],

                    };
                }

            } while (false);


        }
        private void getSimType()
        {
            simtype = new SimType(true);
        }
        private void getCategory()
        {
            category = new Category(data.simTypeId, true);
        }

        private void getPackage()
        {
            package = new Package(data.simTypeId, data.categoryId, true);
        }

        public void setInfo()
        {
            do
            {
                var result = DataBase.SetTMTNTransaction(chatId, data.id, data.simTypeId, data.simTypeShowName, data.categoryId, data.categoryShowName, data.packageId, data.packageShowName, data.packageDescription, data.amount, data.mobileNumber, data.saleKey, data.status);
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

        public void GetSimTypesList()
        {
            do
            {
                simtype = new SimType();
                var wsInput = new wsTMTN.GetSimTypesList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "tmtnservices",
                        ActionName = "GetSimTypesList"
                    },
                    Parameters = new wsTMTN.GetSimTypesList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTMTN.GetSimTypesList_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters.Length < 1)
                {
                    resultAction = new GeneralResultAction("", true, "فهرست خالی برگشته است!");
                    break;
                }


                for (var i = 0; i < wsOutput.Parameters.Length; i++)
                {
                    simtype.data.Add(new SimTypeData
                    {
                        simTypeId = wsOutput.Parameters[i].SimTypeID,
                        simTypeName = wsOutput.Parameters[i].SimTypeName,
                        simTypeShowName = wsOutput.Parameters[i].SimTypeShowName,
                        simTypeThumbnail = wsOutput.Parameters[i].SimTypeThumbnail
                    });
                }

                simtype.setInfo();

                resultAction = new GeneralResultAction();
            } while (false);

        }

        public void GetCategoriesList()
        {
            do
            {
                category = new Category(data.simTypeId);
                var wsInput = new wsTMTN.GetCategoriesList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "tmtnservices",
                        ActionName = "GetCategoriesList"
                    },
                    Parameters = new wsTMTN.GetCategoriesList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SimTypeID = data.simTypeId

                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTMTN.GetCategoriesList_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters.Length < 1)
                {
                    resultAction = new GeneralResultAction("", true, "فهرست خالی برگشته است!");
                    break;
                }


                for (var i = 0; i < wsOutput.Parameters.Length; i++)
                {
                    category.data.Add(new CategoryData
                    {
                        categoryId = wsOutput.Parameters[i].CategoryID,
                        categoryName = wsOutput.Parameters[i].CategoryName,
                        categoryShowName = wsOutput.Parameters[i].CategoryShowName,
                        categoryThumbnail = wsOutput.Parameters[i].CategoryThumbnail
                    });
                }

                category.setInfo(data.simTypeId);

                resultAction = new GeneralResultAction();
            } while (false);

        }

        public void GetPackageList()
        {
            do
            {
                package = new Package(data.simTypeId, data.categoryId);
                var wsInput = new wsTMTN.GetPackagesList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "tmtnservices",
                        ActionName = "GetPackagesList"
                    },
                    Parameters = new wsTMTN.GetPackagesList_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        CategoryID = data.categoryId
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTMTN.GetPackagesList_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters.Length < 1)
                {
                    resultAction = new GeneralResultAction("", true, "فهرست خالی برگشته است!");
                    break;
                }


                for (var i = 0; i < wsOutput.Parameters.Length; i++)
                {
                    package.data.Add(new PackageData
                    {

                        packageId = wsOutput.Parameters[i].PackageID,
                        packageName = wsOutput.Parameters[i].PackageName,
                        packageShowName = wsOutput.Parameters[i].PackageShowName,
                        packageDescription = wsOutput.Parameters[i].PackageDescription,
                        packageAmount = wsOutput.Parameters[i].PackageAmount
                    });
                }

                package.setInfo(data.simTypeId, data.categoryId);

                resultAction = new GeneralResultAction();
            } while (false);

        }

        public void BuyService()
        {
            do
            {
                var wsInput = new wsTMTN.BuyService_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "tmtnservices",
                        ActionName = "BuyService"
                    },
                    Parameters = new wsTMTN.BuyService_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        Amount = data.amount,
                        ServiceTypeID = data.packageId,
                        MobileNumber = data.mobileNumber
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTMTN.BuyService_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;
                }

                data.saleKey = wsOutput.Parameters.SaleKey;

                setInfo();

                resultAction = new GeneralResultAction();
            } while (false);

        }
        public void redeem(string saleKey)
        {
            do
            {
                getInfo(saleKey);
                var wsInput = new wsTMTN.RedeemService_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "tmtnservices",
                        ActionName = "RedeemService"
                    },
                    Parameters = new wsTMTN.RedeemService_Input_Parameters()
                    {
                        SessionID = SimpayCore.getSessionId(),
                        SaleKey = data.saleKey


                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsTMTN.RedeemService_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    resultAction = new GeneralResultAction("", true, wsOutput.Status.Description);
                    break;
                }


                data.status = TransactionStatusEnum.Completed;

                setInfo();

                resultAction = new GeneralResultAction();
            } while (false);

        }


    }
}