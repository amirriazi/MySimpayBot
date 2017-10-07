using Newtonsoft.Json;
using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.XPIN
{
    public class Manager
    {
        public long chatId { get; set; }
        public XpinData data { get; set; }
        public List<ProductData> product { get; set; }
        public List<SubProductData> subProduct { get; set; }

        public GeneralResultAction resultAction { get; set; }

        public Manager()
        {
            data = new XpinData();
        }

        public Manager(long thisChatId, long thisId = 0)
        {
            chatId = thisChatId;
            data = new XpinData() { id = thisId };
            if (thisId != 0)
            {
                getInfo();
                getProducts();
                getSubProducts();

            }
        }
        public Manager(long thisChatId, XpinCategoryEnum theCategoryId = XpinCategoryEnum.GiftCard, long thisId = 0)
        {
            chatId = thisChatId;
            data = new XpinData() { id = thisId, categoryId = theCategoryId };
            if (thisId != 0)
            {

                getInfo();
                getProducts();
                getSubProducts();
            }
            else
            {
                setInfo();
            }
        }

        public Manager(long thisChatId, XpinCategoryEnum theCategoryId = XpinCategoryEnum.GiftCard)
        {
            chatId = thisChatId;
            data = new XpinData() { id = 0, categoryId = theCategoryId };
            setInfo();

        }

        public void getInfo()
        {
            do
            {
                var result = DataBase.GetXpinTransaction(data.id);
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
                    data = new XpinData()
                    {
                        id = (long)DR["id"],
                        categoryId = (XpinCategoryEnum)DR["categoryId"],
                        productID = (int)DR["productID"],
                        productName = (string)DR["productName"],
                        productThumbnail = (string)DR["productThumbnail"],
                        subProductID = (int)DR["subProductID"],
                        subProductName = (string)DR["subProductName"],
                        status = (TransactionStatusEnum)DR["status"],
                        amount = (int)DR["amount"],
                    };
                }


            } while (false);

        }
        public void getProducts()
        {
            product = new List<ProductData>();
            do
            {
                var result = DataBase.GetXpinProduct(data.id);
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
                    product.Add(new ProductData()
                    {
                        productId = (int)record["productId"],
                        productName = (string)record["productName"],
                        productHintsLink = (string)record["productHintsLink"],
                        productIcon = (string)record["productIcon"],
                        productThumbnail = (string)record["productThumbnail"],
                    });

                }


            } while (false);

        }
        public void getSubProducts()
        {
            subProduct = new List<SubProductData>();
            do
            {
                var result = DataBase.GetXpinSubProduct(data.id);
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
                    subProduct.Add(new SubProductData()
                    {
                        subProductId = (int)record["subProductId"],
                        subProductName = (string)record["subProductName"],
                        subProductAmount = (int)record["subProductAmount"],
                        subProductHints = (string)record["subProductHints"],
                        subProductIsActive = (bool)record["subProductIsActive"],

                    });
                }


            } while (false);

        }

        public void setInfo()
        {
            do
            {
                var result = DataBase.SetXpinTransaction(chatId, data.id, data.amount, data.categoryId, data.productID, data.productName, data.productThumbnail, data.subProductID, data.subProductName, data.saleKey, data.status);
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
        public void setProductInfo()
        {
            do
            {
                var result = DataBase.SetXpinProduct(data.id, product);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
            } while (false);

        }
        public void setSubProductInfo()
        {
            do
            {
                var result = DataBase.SetXpinSubProduct(data.id, subProduct);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
            } while (false);

        }

        public ProductData getProduct(int productId)
        {
            var selectedResult = product.Find(delegate (ProductData sd)
            {
                return (sd.productId == productId);
            });
            return selectedResult;
        }
        public SubProductData getSubProduct(int subProductId)
        {
            var selectedResult = subProduct.Find(delegate (SubProductData sd)
            {
                return (sd.subProductId == subProductId);
            });
            return selectedResult;
        }

        public void GetProductList()
        {
            var productList = new List<ProductData>();
            do
            {
                var wsInput = new wsXpin.GetProductsList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "xpin",
                        ActionName = "GetProductsList"
                    },
                    Parameters = new wsXpin.GetProductsList_Input_Parameters()
                    {
                        CategoryID = Convert.ToInt32(data.categoryId),
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsXpin.GetProductsList_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters.Length < 1)
                {
                    resultAction = new GeneralResultAction("callSimpayService", true, "محصولی پیدا نشد");
                    break;
                }

                product = new List<ProductData>();
                for (var i = 0; i < wsOutput.Parameters.Length; i++)
                {
                    product.Add(new ProductData()
                    {
                        productId = wsOutput.Parameters[i].ProductID,
                        productName = wsOutput.Parameters[i].ProductName,
                        productThumbnail = wsOutput.Parameters[i].ProductThumbnail,
                        productIcon = wsOutput.Parameters[i].ProductIcon,
                        productHintsLink = wsOutput.Parameters[i].ProductHintsLink
                    });
                }
                setProductInfo();
                resultAction = new GeneralResultAction();
            } while (false);
        }

        public void GetSubProductList()
        {
            subProduct = new List<SubProductData>();
            do
            {
                var wsInput = new wsXpin.GetSubProductsList_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "xpin",
                        ActionName = "GetSubProductsList"
                    },
                    Parameters = new wsXpin.GetSubProductsList_Input_Parameters()
                    {
                        ProductID = data.productID,
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsXpin.GetSubProductsList_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("callSimpayService", true, wsOutput.Status.Description);
                    break;
                }
                if (wsOutput.Parameters.Length < 1)
                {
                    resultAction = new GeneralResultAction("callSimpayService", true, "محصولی پیدا نشد");
                    break;
                }

                for (var i = 0; i < wsOutput.Parameters.Length; i++)
                {
                    subProduct.Add(new SubProductData()
                    {
                        subProductId = wsOutput.Parameters[i].SubProductID,
                        subProductName = wsOutput.Parameters[i].SubProductName,
                        subProductAmount = wsOutput.Parameters[i].SubProductAmount,
                        subProductIsActive = wsOutput.Parameters[i].SubProductIsActive,
                        subProductHints = wsOutput.Parameters[i].SubProductHints
                    });
                }
                setSubProductInfo();
                resultAction = new GeneralResultAction();
            } while (false);

        }

        public void ReservePin()
        {
            do
            {
                var wsInput = new wsXpin.ReservePin_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "xpin",
                        ActionName = "ReservePin"
                    },
                    Parameters = new wsXpin.ReservePin_Input_Parameters()
                    {
                        SubProductID = data.subProductID,
                        SessionID = SimpayCore.getSessionId()
                    }
                };
                var wsOutputResult = SimpayCore.InterfaceApiCall(wsInput);

                if (String.IsNullOrEmpty(wsOutputResult))
                {
                    Log.Error("Error: Cannot read request message!", 0);
                    resultAction = new GeneralResultAction("ReservePin", true, "result is empty");
                    break;
                }

                var wsOutput = JsonConvert.DeserializeObject<wsXpin.ReservePin_Output>(wsOutputResult);

                if (wsOutput.Status.Code == "G00002")
                {

                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("ReservePin", true, wsOutput.Status.Description);
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    resultAction = new GeneralResultAction("ReservePin", true, wsOutput.Status.Description);
                    break;
                }
                data.saleKey = wsOutput.Parameters.SaleKey;
                setInfo();
                resultAction = new GeneralResultAction();

            } while (false);

        }
        public string Redeem(string saleKey)
        {
            var ans = "";
            do
            {

                var wsInput = new wsXpin.Redeem_Input()
                {
                    Identity = new Core.wsInterface.Identity
                    {
                        JsonWebToken = SimpayCore.GetJsonToken(),
                        ServiceName = "xpin",
                        ActionName = "RedeemPin"
                    },
                    Parameters = new wsXpin.Redeem_Input_Parameters()
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

                var wsOutput = JsonConvert.DeserializeObject<wsXpin.Redeem_Output>(resultCore);


                if (wsOutput.Status.Code == "G00002")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    ans = wsOutput.Status.Description;
                    break;

                }
                else if (wsOutput.Status.Code != "G00000")
                {
                    Log.Error("Error: " + wsOutput.Status.Description, 0);
                    ans = wsOutput.Status.Description;
                    break;
                }

                ans += " پین خریداری شده بدین شرح میباشد:";

                var info = wsOutput.Parameters;
                ans += "\n \n ";
                ans += $" شماره پین: {info.PinCode} ";
                ans += "\n ";
                ans += $" توضیحات {info.Description} ";
                ans += "\n ";
                ans += "\n -";

                data.status = TransactionStatusEnum.Completed;
                setInfo();


            } while (false);
            return ans;

        }


    }
    public enum XpinCategoryEnum
    {
        GiftCard = 1,
        TeleCom = 2,
        Univirus = 3,
        Other = 9
    }
}