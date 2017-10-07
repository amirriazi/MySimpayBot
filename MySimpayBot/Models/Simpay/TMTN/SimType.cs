using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Models.TMTN
{
    public class SimType
    {
        public List<SimTypeData> data { get; set; }


        public SimType(bool readFromData = false)
        {
            data = new List<SimTypeData>();
            if (readFromData)
            {
                getInfo();
            }
        }
        public void getInfo()
        {

            do
            {
                var list = new List<SimTypeData>();
                var result = DataBase.GetTMTNSimType();
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
                    list.Add(new SimTypeData()
                    {
                        simTypeId = (int)record["simTypeId"],
                        simTypeName = (string)record["simTypeName"],
                        simTypeShowName = (string)record["simTypeShowName"],
                        simTypeThumbnail = (string)record["simTypeThumbnail"],
                    });
                }
                data.AddRange(list);

            } while (false);

        }
        public void setInfo()
        {
            var result = DataBase.SetTMTNSimType(data);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }

        }

        public string GetSimTypeShowName(int simTypeId)
        {
            var ans = "";
            var selectedResult = data.Find(delegate (SimTypeData sd)
            {
                return (sd.simTypeId == simTypeId);
            });
            if (selectedResult != null)
                ans = selectedResult.simTypeShowName;

            return ans;
        }


    }
    public class SimTypeData
    {
        public int simTypeId { get; set; }
        public string simTypeName { get; set; }
        public string simTypeShowName { get; set; }
        public string simTypeThumbnail { get; set; }

    }

}