using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;

namespace Models.TrainTicket
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
                var result = DataBase.GetTrainTicketPassengerInfo(id);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    list.Add(new PassengerData()
                    {
                        row = 1
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
                        isCar = (bool)DR["isCar"],
                        firstName = (string)DR["firstName"],
                        lastName = (string)DR["lastName"],
                        nationalCode = (string)DR["nationalCode"],
                        passportNumber = (string)DR["passportNumber"],
                        dateOfBirth = (DateTime?)DR["dateOfBirth"],
                        optionalServiceGo = (int)DR["optionalServiceGo"],
                        optionalServiceReturn = (int)DR["optionalServiceReturn"],
                        passengerTypeCode = (TicketPassengerTypeEnum)DR["passengerTypeCode"],
                        passengerTypeShowName = (string)DR["passengerTypeShowName"],
                        personel = (string)DR["personel"]
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
                    result = DataBase.SetTrainTicketPassenger(id, row, data[row - 1].isIranian, data[row - 1].isCar, data[row - 1].firstName, data[row - 1].lastName, data[row - 1].nationalCode, data[row - 1].passportNumber, data[row - 1].passengerTypeCode, data[row - 1].passengerTypeShowName, data[row - 1].dateOfBirth, data[row - 1].personel, data[row - 1].optionalServiceGo, data[row - 1].optionalServiceReturn);
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
                        result = DataBase.SetTrainTicketPassenger(id, row, info.isIranian, info.isCar, info.firstName, info.lastName, info.nationalCode, info.passportNumber, info.passengerTypeCode, info.passengerTypeShowName, info.dateOfBirth, info.personel, info.optionalServiceGo, info.optionalServiceReturn);
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
        public PassengerData getPassenger(string nationalCode)
        {
            var selectedResult = data.Find(delegate (PassengerData sd)
            {
                return (sd.nationalCode == nationalCode);
            });
            return selectedResult;
        }



        public void updateInfo(string nationalCode, string firstName, string lastName)
        {
            var result = DataBase.UpdateTrainTicketPassenger(id, nationalCode, firstName, lastName);
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
        public bool isCar { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string nationalCode { get; set; }
        public string passportNumber { get; set; }
        public TicketPassengerTypeEnum passengerTypeCode { get; set; }
        public string passengerTypeShowName { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string personel { get; set; }
        public int optionalServiceGo { get; set; }
        public int optionalServiceReturn { get; set; }


    }
}