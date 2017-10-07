using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.TrainTicket
{
    public class Service
    {
        public long id { get; set; }
        public string way { get; set; }
        public List<ServiceData> data { get; set; }

        public Service(long passedId, string passedWay, bool readFromCache = false)
        {
            id = passedId;
            way = passedWay;
            data = new List<ServiceData>();
            if (readFromCache)
                getInfo();
        }

        public void getInfo()
        {
            var list = new List<ServiceData>();
            do
            {
                var result = DataBase.GetTrainTicketService(id, way);
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
                        airConditioning = (bool)DR["airConditioning"],
                        arrivalTime = (string)DR["arrivalTime"],
                        availableCapacity = (int)DR["availableCapacity"],
                        discountedAmount = (int)DR["discountedAmount"],
                        isCompartment = (bool)DR["isCompartment"],
                        media = (bool)DR["media"],
                        realAmount = (int)DR["realAmount"],
                        trainName = (string)DR["trainName"],
                        trainNumber = (int)DR["trainNumber"],
                        trainType = (string)DR["trainType"],
                        serviceUniqueIdentifier = (string)DR["serviceUniqueIdentifier"],
                        departureDateTime = (DateTime)DR["departureDateTime"],
                    });
                }

            } while (false);
            data.AddRange(list);
        }
        public void cacheList()
        {
            var result = DataBase.SetTrainTicketService(id, way, data);
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
                var result = DataBase.GetTrainTicketServiceDayTimeGroup(id, way);
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

                var result = DataBase.GetTrainTicketServiceByDayTime(id, way, dayTimeId, page, 1);
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
                    airConditioning = (bool)DR["airConditioning"],
                    arrivalTime = (string)DR["arrivalTime"],
                    availableCapacity = (int)DR["availableCapacity"],
                    discountedAmount = (int)DR["discountedAmount"],
                    isCompartment = (bool)DR["isCompartment"],
                    media = (bool)DR["media"],
                    realAmount = (int)DR["realAmount"],
                    trainName = (string)DR["trainName"],
                    trainNumber = (int)DR["trainNumber"],
                    trainType = (string)DR["trainType"],
                    serviceUniqueIdentifier = (string)DR["serviceUniqueIdentifier"],
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
        public bool airConditioning { get; set; }
        public string arrivalTime { get; set; }
        public int availableCapacity { get; set; }
        public DateTime departureDateTime { get; set; }
        public int discountedAmount { get; set; }
        public bool isCompartment { get; set; }
        public bool media { get; set; }
        public int realAmount { get; set; }
        public string serviceUniqueIdentifier { get; set; }
        public string trainName { get; set; }
        public int trainNumber { get; set; }
        public string trainType { get; set; }
    }

    public class ServiceDataDayTimePeriodSummary
    {
        public int dayTimeId { get; set; }
        public string dayTimeName { get; set; }
        public int countOfRecords { get; set; }

    }
}