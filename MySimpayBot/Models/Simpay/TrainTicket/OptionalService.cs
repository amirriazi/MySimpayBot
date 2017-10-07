using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.TrainTicket
{
    public class OptionalService
    {
        public long id { get; set; }
        public string way { get; set; }
        public List<OptionalServiceData> data { get; set; }

        public OptionalService(long passedId, string passedWay, bool readFromCache = false)
        {
            id = passedId;
            way = passedWay;
            data = new List<OptionalServiceData>();
            if (readFromCache)
                getInfo();
        }
        public void getInfo()
        {
            var list = new List<OptionalServiceData>();
            do
            {
                var result = DataBase.GetTrainTicketOptionalService(id, way);
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
                    list.Add(new OptionalServiceData()
                    {
                        row = (int)DR["row"],
                        code = (int)DR["code"],
                        amount = (int)DR["amount"],
                        name = (string)DR["name"],
                        description = (string)DR["description"],
                        optionalServiceUniqueIdentifier = (string)DR["optionalServiceUniqueIdentifier"]
                    });
                }

            } while (false);
            data.AddRange(list);
        }
        public void cacheList()
        {
            var result = DataBase.SetTrainTicketOptionalService(id, way, data);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }
        }
        public OptionalServiceData getService(int row)
        {
            var selectedResult = data.Find(delegate (OptionalServiceData osd)
            {
                return (osd.row == row);
            });
            return selectedResult;
        }


    }

    public class OptionalServiceData
    {
        public int row { get; set; }
        public int code { get; set; }
        public int amount { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string optionalServiceUniqueIdentifier { get; set; }

    }
}