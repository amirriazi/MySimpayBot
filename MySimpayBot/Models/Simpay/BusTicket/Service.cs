using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.BusTicket
{
    public class Service
    {
        public long id { get; set; }
        public List<ServiceData> data { get; set; }

        public Service(long passedId, bool readFromCache = false)
        {
            id = passedId;
            data = new List<ServiceData>();
            if (readFromCache)
                getInfo();
        }

        public void getInfo()
        {
            var list = new List<ServiceData>();
            do
            {
                var result = DataBase.GetBusTicketService(id);
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
                    list.Add(new ServiceData()
                    {
                        row = (int)DR["row"],
                        amount = (int)DR["amount"],
                        busType = (string)DR["busType"],
                        capacity = (int)DR["capacity"],
                        corporationName = (string)DR["corporationName"],
                        destinationTerminalShowName = (string)DR["destinationTerminalShowName"],
                        serviceUniqueIdentifier = (string)DR["serviceUniqueIdentifier"],
                        sourceTerminalShowName = (string)DR["sourceTerminalShowName"],
                        departureDateTime = (DateTime)DR["departureDateTime"],
                    });
                }

            } while (false);
            data.AddRange(list);
        }
        public void cacheList()
        {
            var result = DataBase.SetBusTicketService(id, data);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }
        }

        public List<ServiceDataDayTimePeriodSummary> getDayTimeSummary()
        {
            var list = new List<ServiceDataDayTimePeriodSummary>();
            do
            {
                var result = DataBase.GetBusTicketServiceDayTimeGroup(id);
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
                    if ((int)DR["countOfRecords"] > 0)
                    {
                        list.Add(new ServiceDataDayTimePeriodSummary()
                        {
                            dayTimeId = (int)DR["dayTimeId"],
                            dayTimeName = (string)DR["dayTimeName"],
                            countOfRecords = (int)DR["countOfRecords"],
                        });
                    }
                }

            } while (false);
            return list;

        }

        public ServiceData getByDayTime(int dayTimeId, int page, out int MaxPage)
        {
            var service = new ServiceData();
            var maxp = 0;
            do
            {

                var result = DataBase.GetBusTicketServiceByDayTime(id, dayTimeId, page, 1);
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
                var DR = DS.Tables[0].Rows[0];
                service = new ServiceData()
                {
                    row = (int)DR["row"],
                    amount = (int)DR["amount"],
                    busType = (string)DR["busType"],
                    capacity = (int)DR["capacity"],
                    corporationName = (string)DR["corporationName"],
                    destinationTerminalShowName = (string)DR["destinationTerminalShowName"],
                    serviceUniqueIdentifier = (string)DR["serviceUniqueIdentifier"],
                    sourceTerminalShowName = (string)DR["sourceTerminalShowName"],
                    departureDateTime = (DateTime)DR["departureDateTime"],
                };
                maxp = (int)DR["maxPage"];

            } while (false);
            MaxPage = maxp;
            return service;


        }
        public ServiceData getService(int row)
        {
            var selectedResult = data.Find(delegate (ServiceData sd)
            {
                return (sd.row == row);
            });
            return selectedResult;
        }

    }

    public class ServiceData
    {
        public int row { get; set; }
        public int amount { get; set; }
        public string busType { get; set; }
        public int capacity { get; set; }
        public string corporationName { get; set; }
        public DateTime departureDateTime { get; set; }
        public string destinationTerminalShowName { get; set; }
        public string serviceUniqueIdentifier { get; set; }
        public string sourceTerminalShowName { get; set; }
    }

    public class ServiceDataDayTimePeriodSummary
    {
        public int dayTimeId { get; set; }
        public string dayTimeName { get; set; }
        public int countOfRecords { get; set; }


    }

    

}