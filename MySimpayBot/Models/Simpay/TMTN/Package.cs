using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Models.TMTN
{
    public class Package
    {
        public List<PackageData> data { get; set; }


        public Package(int simpayId, int categoryId, bool readFromData = false)
        {
            data = new List<PackageData>();
            if (readFromData)
            {
                getInfo(simpayId, categoryId);
            }
        }
        public void getInfo(int simpayId, int categoryId)
        {

            do
            {
                var list = new List<PackageData>();
                var result = DataBase.GetTMTNPackage(simpayId, categoryId);
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
                data.Clear();
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    list.Add(new PackageData()
                    {
                        packageId = (int)record["packageId"],
                        packageName = (string)record["packageName"],
                        packageShowName = (string)record["packageShowName"],
                        packageDescription = (string)record["packageDescription"],
                        packageAmount = (int)record["packageAmount"],
                    });
                }
                data.AddRange(list);

            } while (false);

        }
        public void setInfo(int simTypeId, int categoryId)
        {
            var result = DataBase.SetTMTNPackage(simTypeId, categoryId, data);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }

        }
        public string GetPackageShowName(int packageId)
        {
            var ans = "";
            var selectedResult = data.Find(delegate (PackageData sd)
            {
                return (sd.packageId == packageId);
            });
            if (selectedResult != null)
                ans = selectedResult.packageShowName;

            return ans;
        }
        public string GetPackageDescription(int packageId)
        {
            var ans = "";
            var selectedResult = data.Find(delegate (PackageData sd)
            {
                return (sd.packageId == packageId);
            });
            if (selectedResult != null)
                ans = selectedResult.packageDescription;

            return ans;
        }
        public int GetPackageAmount(int packageId)
        {
            int ans = 0;
            var selectedResult = data.Find(delegate (PackageData sd)
            {
                return (sd.packageId == packageId);
            });
            if (selectedResult != null)
                ans = selectedResult.packageAmount;

            return ans;
        }


    }
    public class PackageData
    {
        public int packageId { get; set; }

        public string packageName { get; set; }

        public string packageShowName { get; set; }

        public string packageDescription { get; set; }

        public int packageAmount { get; set; }


    }
}