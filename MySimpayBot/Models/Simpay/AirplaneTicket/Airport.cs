using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.AirplaneTicket
{
    public class Airport
    {
        public List<AirportData> data { get; set; }
        public Airport(bool Chache = false)
        {
            data = new List<AirportData>();
            if (Chache)
                setDataList();
        }


        public string getShowName(string queryCode)
        {
            var ans = "";
            do
            {
                var selectedResult = data.Find(delegate (AirportData sd)
                {
                    return (sd.airportCode == queryCode);
                });

                if (selectedResult != null)
                    ans = selectedResult.airportShowName;
            } while (false);

            return ans;
        }
        public void setDataList(string code = "", string keyword = "")
        {
            var list = new List<AirportData>();
            do
            {
                var result = DataBase.GetAirplaneTicketAirports(code, keyword);
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
                    list.Add(new AirportData()
                    {
                        airportCode = (string)record["airportCode"],
                        airportShowName = (string)record["airportShowName"]
                    });
                }

            } while (false);
            data.AddRange(list);

        }

        public void CacheList()
        {
            var result = DataBase.SetAirplaneTicketAirport(data);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }
        }

    }

    public class AirportData
    {
        public string airportCode { get; set; }
        public string airportShowName { get; set; }
        public string city { get; set; }
    }
}