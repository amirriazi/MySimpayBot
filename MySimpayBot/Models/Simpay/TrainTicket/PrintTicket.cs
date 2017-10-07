using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.TrainTicket
{
    public class PrintTicket
    {
        public long id { get; set; }
        public string way { get; set; }
        public List<PrintTicketData> data { get; set; }

        public PrintTicket(long passedId, string passedWay, bool readFromCache = false)
        {
            id = passedId;
            way = passedWay;
            data = new List<PrintTicketData>();
            if (readFromCache)
                getInfo();
        }


        public void getInfo()
        {
            var list = new List<PrintTicketData>();
            do
            {
                var result = DataBase.GetTrainTicketWayPrint(id, way);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    list.Add(new PrintTicketData()
                    {
                        row = 1
                    });
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                foreach (DataRow DR in DS.Tables[0].Rows)
                {
                    list.Add(new PrintTicketData()
                    {
                        row = (int)DR["row"],
                        barCodeImage = (string)DR["barCodeImage"]
                    });
                }

            } while (false);
            data.AddRange(list);
        }


        public void setInfo()
        {
            do
            {
                foreach (var info in data)
                {
                    var result = DataBase.SetTrainTicketWayPrint(id, way, info.row, info.barCodeImage);
                    if (result.ReturnCode != 1 || result.SPCode != 1)
                    {
                        Log.Fatal(result.Text, DateTime.Now.Millisecond);
                        break;
                    }
                }

            } while (false);

        }


    }

    public class PrintTicketData
    {
        public int row { get; set; }
        public string barCodeImage { get; set; }
    }
}