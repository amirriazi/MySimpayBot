using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.AirplaneTicket
{
    public class Passenger
    {
        public long id { get; set; }
        public List<PassengerData> data { get; set; }

        public Passenger(long passedId, bool readFromCache = false)
        {
            id = passedId;
            data = new List<PassengerData>();
            if (readFromCache)
                getInfo();
        }


        public void getInfo()
        {
            var list = new List<PassengerData>();
            do
            {
                var result = DataBase.GetAirplaneTicketPassengerInfo(id);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    list.Add(new PassengerData()
                    {
                        row = 1,
                        isIranian = true
                    });
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                foreach (DataRow DR in DS.Tables[0].Rows)
                {
                    list.Add(new PassengerData()
                    {
                        row = (int)DR["row"],
                        isIranian = (bool)DR["isIranian"],
                        title = (string)DR["title"],
                        firstName = (string)DR["firstName"],
                        lastName = (string)DR["lastName"],
                        nationalCode = (string)DR["nationalCode"],
                        passportNumber = (string)DR["passportNumber"],
                        dateOfBirth = (DateTime?)DR["dateOfBirth"],
                        passengerTypeCode = (string)DR["passengerTypeCode"],
                        passengerTypeShowName = (string)DR["passengerTypeShowName"],
                        passportCountry = (string)DR["passportCountry"],
                        passportExpireDate = (DateTime?)DR["passportExpireDate"],
                        htmlGo = (string)DR["htmlGo"],
                        htmlReturn = (string)DR["htmlReturn"],

                    });
                }

            } while (false);
            data.AddRange(list);
        }


        public void setInfo(int row = 0)
        {
            do
            {
                QueryResult result = new QueryResult();
                if (row != 0)
                {
                    result = DataBase.SetAirplaneTicketPassenger(id, row, data[row - 1].isIranian, data[row - 1].title, data[row - 1].firstName, data[row - 1].lastName, data[row - 1].nationalCode, data[row - 1].passportNumber, data[row - 1].passportCountry, data[row - 1].passportExpireDate, data[row - 1].passengerTypeCode, data[row - 1].passengerTypeShowName, data[row - 1].dateOfBirth, data[row - 1].htmlGo, data[row - 1].htmlReturn);
                    if (result.ReturnCode != 1 || result.SPCode != 1)
                    {
                        Log.Fatal(result.Text, DateTime.Now.Millisecond);
                        break;
                    }

                }
                else
                {
                    foreach (var info in data)
                    {
                        result = DataBase.SetAirplaneTicketPassenger(id, row, info.isIranian, info.title, info.firstName, info.lastName, info.nationalCode, info.passportNumber, info.passportCountry, info.passportExpireDate, info.passengerTypeCode, info.passengerTypeShowName, info.dateOfBirth, info.htmlGo, info.htmlReturn);
                        if (result.ReturnCode != 1 || result.SPCode != 1)
                        {
                            Log.Fatal(result.Text, DateTime.Now.Millisecond);
                            break;
                        }
                    }

                }

            } while (false);

        }

        public PassengerData getPassenger(int row)
        {
            var selectedResult = data.Find(delegate (PassengerData sd)
            {
                return (sd.row == row);
            });
            return selectedResult;
        }
        public PassengerData getPassenger(string firstName, string lastName)
        {
            var selectedResult = data.Find(delegate (PassengerData sd)
            {
                return (sd.firstName.ToLower() == firstName.ToLower() && sd.lastName.ToLower() == lastName.ToLower());
            });
            return selectedResult;
        }



        public void updateInfo(string nationalCode, string firstName, string lastName)
        {
            var result = DataBase.UpdateAirplaneTicketPassenger(id, nationalCode, firstName, lastName);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }
        }

    }

    public class PassengerData
    {

        public int row { get; set; }

        public bool isIranian { get; set; }

        public string title { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string nationalCode { get; set; }

        public string passportNumber { get; set; }

        public string passportCountry { get; set; }

        public DateTime? passportExpireDate { get; set; }

        public string passengerTypeCode { get; set; }

        public string passengerTypeShowName { get; set; }

        public DateTime? dateOfBirth { get; set; }
        public string htmlGo { get; set; }
        public string htmlReturn { get; set; }

    }
}