using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.TrainTicket
{
    public class Station
    {
        public List<StationData> data { get; set; }

        public Station(bool Chache = false)
        {
            data = new List<StationData>();
            if (Chache)
                setDataList();
        }


        public string getShowName(int queryId)
        {
            var ans = "";
            do
            {
                var selectedResult = data.Find(delegate (StationData sd)
                {
                    return (sd.stationCode == queryId);
                });

                if (selectedResult != null)
                    ans = selectedResult.stationShowName;
            } while (false);

            return ans;
        }
        public void setDataList(long code = 0, string keyword = "")
        {
            var list = new List<StationData>();
            do
            {
                var result = DataBase.GetTrainTicketStation(code, keyword);
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
                    list.Add(new StationData()
                    {
                        stationCode = (int)DR["stationCode"],
                        stationShowName = (string)DR["stationName"]
                    });
                }

            } while (false);
            data.AddRange(list);

        }

        public void CacheList()
        {
            var result = DataBase.SetTrainTicketStation(data);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }
        }

    }
    public class StationData
    {
        public int stationCode { get; set; }
        public string stationShowName { get; set; }
    }


}

