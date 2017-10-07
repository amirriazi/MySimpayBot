using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.AirplaneTicket
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
                var result = DataBase.GetAirplaneTicketService(id, way);
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
                    list.Add(new ServiceData()
                    {
                        row = (int)record["row"],
                        systemId = (int)record["systemId"],
                        sourceAirportCode = (string)record["SourceAirportCode"],
                        destinationAirportCode = (string)record["DestinationAirportCode"],
                        aircraft = (string)record["Aircraft"],
                        airlineCode = (string)record["AirlineCode"],
                        flightID = (string)record["FlightID"],
                        flightNumber = (string)record["FlightNumber"],
                        Class = (string)record["Class"],
                        classType = (string)record["ClassType"],
                        classTypeName = (string)record["ClassTypeName"],
                        amountAdult = (int)record["AmountAdult"],
                        amountChild = (int)record["AmountChild"],
                        amountInfant = (int)record["AmountInfant"],
                        arrivalTime = (string)record["ArrivalTime"],
                        dayOfWeek = (string)record["DayOfWeek"],
                        departureDateTime = (DateTime)record["DepartureDateTime"],
                        description = (string)record["Description"],
                        isCharter = (bool)record["isCharter"],
                        sellerId = (string)record["SellerId"],
                        sellerReference = (string)record["SellerReference"],
                        serviceUniqueIdentifier = (string)record["ServiceUniqueIdentifier"],
                        status = (string)record["Status"],
                        statusName = (string)record["StatusName"],
                        systemKey = (string)record["SystemKey"],

                    });
                }

            } while (false);
            data.AddRange(list);
        }

        public void setInfo()
        {
            var result = DataBase.SetAirplaneTicketService(id, way, data);
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
                var result = DataBase.GetAirplaneTicketServiceDayTimeGroup(id, way);
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

                var result = DataBase.GetAirplaneTicketServiceByDayTime(id, way, dayTimeId, page, 1);
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
                var record = DS.Tables[0].Rows[0];
                service = new ServiceData()
                {
                    row = (int)record["row"],
                    systemId = (int)record["systemId"],
                    sourceAirportCode = (string)record["sourceAirportCode"],
                    destinationAirportCode = (string)record["DestinationAirportCode"],
                    aircraft = (string)record["Aircraft"],
                    airlineCode = (string)record["AirlineCode"],
                    flightID = (string)record["FlightID"],
                    flightNumber = (string)record["FlightNumber"],
                    Class = (string)record["Class"],
                    classType = (string)record["ClassType"],
                    classTypeName = (string)record["ClassTypeName"],
                    amountAdult = (int)record["AmountAdult"],
                    amountChild = (int)record["AmountChild"],
                    amountInfant = (int)record["AmountInfant"],
                    arrivalTime = (string)record["ArrivalTime"],
                    dayOfWeek = (string)record["DayOfWeek"],
                    departureDateTime = (DateTime)record["DepartureDateTime"],
                    description = (string)record["Description"],
                    isCharter = (bool)record["isCharter"],
                    sellerId = (string)record["SellerId"],
                    sellerReference = (string)record["SellerReference"],
                    serviceUniqueIdentifier = (string)record["ServiceUniqueIdentifier"],
                    status = (string)record["Status"],
                    statusName = (string)record["StatusName"],
                    systemKey = (string)record["SystemKey"],

                };
                maxp = (int)record["maxPage"];

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

        public long systemId { get; set; }

        public string sourceAirportCode { get; set; }

        public string destinationAirportCode { get; set; }

        public string aircraft { get; set; }

        public string airlineCode { get; set; }

        public string flightID { get; set; }

        public string flightNumber { get; set; }

        public string Class { get; set; }

        public string classType { get; set; }

        public string classTypeName { get; set; }

        public int amountAdult { get; set; }

        public int amountChild { get; set; }

        public int amountInfant { get; set; }

        public string arrivalTime { get; set; }

        public string dayOfWeek { get; set; }

        public DateTime? departureDateTime { get; set; }

        public string description { get; set; }

        public bool isCharter { get; set; }

        public string sellerId { get; set; }

        public string sellerReference { get; set; }

        public string serviceUniqueIdentifier { get; set; }

        public string status { get; set; }

        public string statusName { get; set; }

        public string systemKey { get; set; }

    }

    public class ServiceDataDayTimePeriodSummary
    {
        public int dayTimeId { get; set; }
        public string dayTimeName { get; set; }
        public int countOfRecords { get; set; }

    }

}