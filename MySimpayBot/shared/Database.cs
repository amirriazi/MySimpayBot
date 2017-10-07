
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Shared.WebService
{
    public static partial class DataBase
    {
        #region Commons
        public static QueryResult ReportUpdate(long UpdateID, int MessageID, int messageType, long ChatID, string U)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_reportUpdate";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@UpdateID", Value = UpdateID });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@MessageID", Value = MessageID });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@messageType", Value = messageType });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = ChatID });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@U", Value = U });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult getMenu(long chatId, int parentId = 0)
        {
            var parameters = new List<SqlParameter>();
            string query = "dbo.ud_prc_getMenu";

            parameters.Add(new SqlParameter { ParameterName = "@chatId", SqlDbType = SqlDbType.BigInt, Value = chatId });
            parameters.Add(new SqlParameter { ParameterName = "@parentId", SqlDbType = SqlDbType.Int, Value = parentId });
            return ExecuteStoredProcedure(query, parameters);

        }

        public static QueryResult SetPreviousMenu(long chatId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_setPreviousMenu";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetUser(long chatId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getUser";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult ReportUser(long chatId, string firstName, string lastName, string userName, long introducedBy = 0, string mobileNumber = "", bool activated = false, string activationCode = "", string JsonWebToken = "", string invitationCode = "", string linkAction = "")
        {
            var parameters = new List<SqlParameter>();
            string query = "dbo.ud_prc_reportUser";

            parameters.Add(new SqlParameter { ParameterName = "@chatId", SqlDbType = SqlDbType.BigInt, Value = chatId });
            parameters.Add(new SqlParameter { ParameterName = "@firstName", SqlDbType = SqlDbType.NVarChar, Value = firstName ?? "N/A" });
            parameters.Add(new SqlParameter { ParameterName = "@lastName", SqlDbType = SqlDbType.NVarChar, Value = lastName ?? "N/A" });
            parameters.Add(new SqlParameter { ParameterName = "@userName", SqlDbType = SqlDbType.NVarChar, Value = userName ?? "N/A" });
            parameters.Add(new SqlParameter { ParameterName = "@" + nameof(introducedBy), SqlDbType = SqlDbType.Int, Value = introducedBy });
            parameters.Add(new SqlParameter { ParameterName = "@mobileNumber", SqlDbType = SqlDbType.VarChar, Value = mobileNumber });
            parameters.Add(new SqlParameter { ParameterName = "@activated", SqlDbType = SqlDbType.Bit, Value = activated });
            parameters.Add(new SqlParameter { ParameterName = "@activationCode", SqlDbType = SqlDbType.VarChar, Value = activationCode });
            parameters.Add(new SqlParameter { ParameterName = "@JsonWebToken", SqlDbType = SqlDbType.VarChar, Value = JsonWebToken });
            parameters.Add(new SqlParameter { ParameterName = "@invitationCode", SqlDbType = SqlDbType.VarChar, Value = invitationCode });
            parameters.Add(new SqlParameter { ParameterName = "@linkAction", SqlDbType = SqlDbType.VarChar, Value = linkAction });

            return ExecuteStoredProcedure(query, parameters);

        }

        public static QueryResult ReportUserBlocked(long chatId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_reportUserBlocked";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult reportUserSelection(long chatId, int selectedMenuId)
        {

            var parameters = new List<SqlParameter>();
            string query = "dbo.ud_prc_reportUserSelection";


            parameters.Add(new SqlParameter { ParameterName = "@chatId", SqlDbType = SqlDbType.BigInt, Value = chatId });
            parameters.Add(new SqlParameter { ParameterName = "@selectedMenuId", SqlDbType = SqlDbType.Int, Value = selectedMenuId });

            return ExecuteStoredProcedure(query, parameters);

        }


        public static QueryResult getUserSelection(long chatId)
        {

            var parameters = new List<SqlParameter>();
            string query = "dbo.ud_prc_getUserSelection";

            parameters.Add(new SqlParameter { ParameterName = "@chatId", SqlDbType = SqlDbType.BigInt, Value = chatId });

            return ExecuteStoredProcedure(query, parameters);

        }

        public static QueryResult GetSelectedMenu(long chatID)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getSelectedMenu";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatID", Value = chatID });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetCurrentAction(long chatId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getCurrentAction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult RemoveCurrentAction(long chatId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_removeCurrentAction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult ReportCurrentAction(long chatId, SimpaySectionEnum section, string action, string parameter = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_reportCurrentAction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@section", Value = section });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@action", Value = action });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@parameter", Value = parameter });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        #endregion

        #region AUTOCHARGE
        public static QueryResult GetAutoChargeOperatorList()
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetAutoChargeOperatorList";

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetAutoChargeList(int chargeTypeId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetAutoChargeList";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@chargeTypeId", Value = chargeTypeId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetAutoChargeTransaction(int id = 0, string saleKey = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetAutoChargeTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult SetAutoChargeTransaction(long chatId, int id, string mobileNumber, int chargeTypeId, int amount, string saleKey, TransactionStatusEnum status, string transactionId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetAutoChargeTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@mobileNumber", Value = mobileNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@chargeTypeId", Value = chargeTypeId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@transactionId", Value = transactionId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetAutoChargeLastMobile(long chatId, int count)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getAutoChargeLastMobile";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@count", Value = count });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion

        #region TRAFFICFINE
        public static QueryResult SetTrafficFineHeader(long chatId, long ticketId = 0, string barCode = "", string saleKey = "", int count = 0, int amount = 0, bool twoPhaseInquiry = false, string captchaUrl = "", string captchaText = "", string captchaBase64 = "", TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTrafficFineHeader";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@ticketId", Value = ticketId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@barCode", Value = barCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@count", Value = count });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@twoPhaseInquiry", Value = twoPhaseInquiry });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@captchaUrl", Value = captchaUrl });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@captchaText", Value = captchaText });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@captchaBase64", Value = captchaBase64 });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetTrafficFineDetail(long ticketId, List<Models.TrafficFine.TrafficFineData.Detail> details)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTrafficFineDetail";

            DataTable dtDetail = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())
            var F = details[0].GetType().GetProperties()[0];

            dtDetail.Columns.Add("ID", typeof(Int32)).AllowDBNull = false;
            dtDetail.Columns.Add("Amount", typeof(Int32));
            dtDetail.Columns.Add("City", typeof(string));
            dtDetail.Columns.Add("Code", typeof(Int32));
            dtDetail.Columns.Add("DateTime", typeof(DateTime));
            dtDetail.Columns.Add("Description", typeof(string));
            dtDetail.Columns.Add("LicensePlate", typeof(string));
            dtDetail.Columns.Add("Location", typeof(string));
            dtDetail.Columns.Add("Serial", typeof(string));
            dtDetail.Columns.Add("Type", typeof(string));


            for (var i = 0; i < details.Count; i++)
            {
                DataRow dr = dtDetail.NewRow();
                dr["ID"] = details[i].ID;
                dr["Amount"] = details[i].Amount;
                dr["City"] = details[i].City;
                dr["Code"] = details[i].Code;
                dr["DateTime"] = details[i].DateTime;
                dr["Description"] = details[i].Description;
                dr["LicensePlate"] = details[i].LicensePlate;
                dr["Location"] = details[i].Location;
                dr["Serial"] = details[i].Serial;
                dr["Type"] = details[i].Type;
                dtDetail.Rows.Add(dr);
            }



            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@ticketId", Value = ticketId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@details", Value = dtDetail });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetTrafficFineHeader(long ticketId = 0, string saleKey = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTrafficFineHeader";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@ticketId", Value = ticketId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetTrafficFineDetail(long ticketId, int row = 0)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTrafficFineDetail";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@ticketId", Value = ticketId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@row", Value = row });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetTrafficFineDetailRowSelection(long ticketId, int row, bool selected)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTrafficFineDetailRowSelection";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@ticketId", Value = ticketId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@row", Value = row });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@selected", Value = selected });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetTrafficFineLastBarcode(long chatId, int count)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getTrafficFineLastBarcode";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@count", Value = count });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        #endregion

        #region MciMobileBill
        public static QueryResult SetMciMobileBillTransaction(long chatId, long id, string mobileNumber, int amount = 0, bool final = false, string billId = "", string paymentId = "", TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetMciMobileChargeTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@mobileNumber", Value = mobileNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@final", Value = final });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@billId", Value = billId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@paymentId", Value = paymentId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetMciMobileBillTransaction(long id = 0)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetMciMobileBillTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetMciMobileBillLastMobile(long chatId, int count)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getMciMobileBillLastMobile";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@count", Value = count });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion

        #region BillPayment

        public static QueryResult SetBillPaymentTransaction(long chatId, long id, int amount, string billId, string paymentId, string billType, string saleKey = "", TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetBillPaymentTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@billId", Value = billId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@paymentId", Value = paymentId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@billType", Value = billType });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetBillPaymentTransaction(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetBillPaymentTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetBillPaymentLastMobile(long chatId, int count)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getBillPaymentLastMobile";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@count", Value = count });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion



        #region PinCharge
        public static QueryResult SetPinChargeTransaction(long chatId, long id, int amount = 0, string name = "", string operatorName = "", string pinCode = "", int typeId = 0, string saleKey = "", TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetPinChargeTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@name", Value = name });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@operatorName", Value = operatorName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@pinCode", Value = pinCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@typeId", Value = typeId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetPinChargeTransaction(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetPinChargeTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion

        #region BusTicket
        public static QueryResult SetBusTicketTransaction(long chatId, long id, int sourceStateCode = 0, string sourceStateShowName = "", int sourceTerminalCode = 0, string sourceTerminalShowName = "", int destinationStateCode = 0, string destinationStateShowName = "", int destinationTerminalCode = 0, string destinationTerminalShowName = "", DateTime? dateTime = null, int amount = 0, string serviceUniqueIdentifier = "", string fullName = "", int seatCount = 0, int selectedServiceRow = 0, string saleKey = "", TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted, string ticketNumber = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetBusTicketTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@sourceStateCode", Value = sourceStateCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@sourceStateShowName", Value = sourceStateShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@sourceTerminalCode", Value = sourceTerminalCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@sourceTerminalShowName", Value = sourceTerminalShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@destinationStateCode", Value = destinationStateCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@destinationStateShowName", Value = destinationStateShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@destinationTerminalCode", Value = destinationTerminalCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@destinationTerminalShowName", Value = destinationTerminalShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.SmallDateTime, ParameterName = "@dateTime", Value = dateTime });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@serviceUniqueIdentifier", Value = serviceUniqueIdentifier });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@fullName", Value = fullName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@seatCount", Value = seatCount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@selectedServiceRow", Value = selectedServiceRow });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@ticketNumber", Value = ticketNumber });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetBusTicketTransaction(long id = 0, string saleKey = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetBusTicketTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetBusTicketState(List<Models.BusTicket.StateData> stateList)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetBusTicketState";

            DataTable dtDetail = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dtDetail.Columns.Add("code", typeof(Int32));
            dtDetail.Columns.Add("name", typeof(string));


            for (var i = 0; i < stateList.Count; i++)
            {
                DataRow dr = dtDetail.NewRow();
                dr["code"] = stateList[i].stateCode;
                dr["name"] = stateList[i].stateShowName;
                dtDetail.Rows.Add(dr);
            }
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dtDetail });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }


        public static QueryResult SetBusTicketTerminal(List<Models.BusTicket.TerminalData> TerminalList)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetBusTicketTerminal";

            DataTable dtDetail = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dtDetail.Columns.Add("code", typeof(Int32));
            dtDetail.Columns.Add("name", typeof(string));


            for (var i = 0; i < TerminalList.Count; i++)
            {
                DataRow dr = dtDetail.NewRow();
                dr["code"] = TerminalList[i].terminalCode;
                dr["name"] = TerminalList[i].terminalShowName;
                dtDetail.Rows.Add(dr);
            }
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dtDetail });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }


        public static QueryResult GetBusTicketTerminal()
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetBusTicketTerminal";

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetBusTicketState()
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetBusTicketState";

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetBusTicketService(long id, List<Models.BusTicket.ServiceData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetBusTicketService";

            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dataTable.Columns.Add("row", typeof(Int32));
            dataTable.Columns.Add("amount", typeof(Int32));
            dataTable.Columns.Add("capacity", typeof(Int32));
            dataTable.Columns.Add("busType", typeof(string));
            dataTable.Columns.Add("corporationName", typeof(string));
            dataTable.Columns.Add("departureDateTime", typeof(DateTime));
            dataTable.Columns.Add("sourceTerminalShowName", typeof(string));
            dataTable.Columns.Add("destinationTerminalShowName", typeof(string));
            dataTable.Columns.Add("serviceUniqueIdentifier", typeof(string));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["row"] = i + 1;
                dr["amount"] = list[i].amount;
                dr["capacity"] = list[i].capacity;
                dr["busType"] = list[i].busType;
                dr["corporationName"] = list[i].corporationName;
                dr["departureDateTime"] = list[i].departureDateTime;
                dr["sourceTerminalShowName"] = list[i].sourceTerminalShowName;
                dr["destinationTerminalShowName"] = list[i].destinationTerminalShowName;
                dr["serviceUniqueIdentifier"] = list[i].serviceUniqueIdentifier;
                dataTable.Rows.Add(dr);
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetBusTicketService(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetBusTicketService";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetBusTicketServiceDayTimeGroup(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetBusTicketServiceDayTimeGroup";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }



        public static QueryResult GetBusTicketServiceByDayTime(long id, int dayTimeId, int PageNumber = 1, int RecordsInPage = 0)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetBusTicketServiceByDayTime";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@dayTimeId", Value = dayTimeId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@PageNumber", Value = PageNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@RecordsInPage", Value = RecordsInPage });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetBusTicketInitializeSeat(long id, int capacity, int columnNumber, int floor, int rowNumber, int space, List<Models.BusTicket.SeatMap> list)
        {

            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetBusTicketInitializeSeat";

            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())
            dataTable.Columns.Add("mapIndex", typeof(Int32));
            dataTable.Columns.Add("seatNumber", typeof(Int32));
            dataTable.Columns.Add("occupiedBy", typeof(Int32));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["mapIndex"] = list[i].mapIndex;
                dr["seatNumber"] = list[i].seatNumber;
                dr["occupiedBy"] = list[i].occupiedBy;
                dataTable.Rows.Add(dr);
            }

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@capacity", Value = capacity });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@columnNumber", Value = columnNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@floor", Value = floor });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@rowNumber", Value = rowNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@space", Value = space });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetBusTicketSeat(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetBusTicketSeatMap";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetBusTicketSeat(long id, int mapIndex, bool selectedByUser, int selectedGender = 0)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_setBusTicketSeat";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@mapIndex", Value = mapIndex });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@selectedByUser", Value = selectedByUser });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@selectedGender", Value = selectedGender });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetBusTicketLastPath(long chatId, int count)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getBusTicketLastPath";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@count", Value = count });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetTrainTicketLastPath(long chatId, int count)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getTrainTicketLastPath";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@count", Value = count });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        #endregion




        #region ADMIN
        public static QueryResult Admin(string keyword, string extra = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_Admin";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@keyword", Value = keyword });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@extra", Value = extra });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult AdminGetLinkCounter(bool ExcludeBlockedUser = false)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_RptLinkCounter";


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@ExcludeBlockedUser", Value = ExcludeBlockedUser });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion

        #region TRAIN
        public static QueryResult SetTrainTicketStation(List<Models.TrainTicket.StationData> List)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTrainTicketStation";

            DataTable dtDetail = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dtDetail.Columns.Add("code", typeof(Int32));
            dtDetail.Columns.Add("name", typeof(string));


            for (var i = 0; i < List.Count; i++)
            {
                DataRow dr = dtDetail.NewRow();
                dr["code"] = List[i].stationCode;
                dr["name"] = List[i].stationShowName;
                dtDetail.Rows.Add(dr);
            }
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dtDetail });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetTrainTicketStation(long code = 0, string keyword = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTrainTicketStation";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@code", Value = code });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@keyword", Value = keyword });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetTrainTicketTransaction(long id, string saleKey = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTrainTicketTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult SetTrainTicketTransaction(long chatId, long id = 0, int sourceStationCode = 0, string sourceStationShowName = "", int destinationStationCode = 0, string destinationStationShowName = "", bool JustCompartment = false, int seatCount = 0, Models.TrainTicket.TicketTypeEnum ticketTypeCode = Models.TrainTicket.TicketTypeEnum.Unknown, bool twoWay = false, DateTime? wayGoDateTime = null, DateTime? wayReturnDatetime = null, int amount = 0, string saleKey = "", TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted, int goRow = 0, int returnRow = 0, int currentPassengerRow = 0, int lockedRowNumberGo = 0, int lockedWagonNumberGo = 0, int lockedRowNumberReturn = 0, int lockedWagonNumberReturn = 0)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTrainTicketTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@sourceStationCode", Value = sourceStationCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@sourceStationShowName", Value = sourceStationShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@destinationStationCode", Value = destinationStationCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@destinationStationShowName", Value = destinationStationShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@JustCompartment", Value = JustCompartment });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@seatCount", Value = seatCount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@ticketTypeCode", Value = ticketTypeCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@twoWay", Value = twoWay });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.SmallDateTime, ParameterName = "@wayGoDateTime", Value = wayGoDateTime });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.SmallDateTime, ParameterName = "@wayReturnDatetime", Value = wayReturnDatetime });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@goRow", Value = goRow });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@returnRow", Value = returnRow });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@currentPassengerRow", Value = currentPassengerRow });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@lockedRowNumberGo", Value = lockedRowNumberGo });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@lockedWagonNumberGo", Value = lockedWagonNumberGo });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@lockedRowNumberReturn", Value = lockedRowNumberReturn });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@lockedWagonNumberReturn", Value = lockedWagonNumberReturn });


            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetTrainTicketService(long id, string way, List<Models.TrainTicket.ServiceData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTrainTicketService";
            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dataTable.Columns.Add("row", typeof(Int32));
            dataTable.Columns.Add("airConditioning", typeof(Int32));
            dataTable.Columns.Add("arrivalTime", typeof(string));
            dataTable.Columns.Add("availableCapacity", typeof(Int32));
            dataTable.Columns.Add("departureDateTime", typeof(DateTime));
            dataTable.Columns.Add("discountedAmount", typeof(Int32));
            dataTable.Columns.Add("isCompartment", typeof(bool));
            dataTable.Columns.Add("media", typeof(bool));
            dataTable.Columns.Add("realAmount", typeof(Int32));
            dataTable.Columns.Add("serviceUniqueIdentifier", typeof(string));

            dataTable.Columns.Add("trainName", typeof(string));
            dataTable.Columns.Add("trainNumber", typeof(Int32));
            dataTable.Columns.Add("trainType", typeof(string));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["row"] = i + 1;
                dr["airConditioning"] = list[i].airConditioning;
                dr["arrivalTime"] = list[i].arrivalTime;
                dr["availableCapacity"] = list[i].availableCapacity;
                dr["departureDateTime"] = list[i].departureDateTime;
                dr["discountedAmount"] = list[i].discountedAmount;
                dr["departureDateTime"] = list[i].departureDateTime;
                dr["isCompartment"] = list[i].isCompartment;
                dr["media"] = list[i].media;
                dr["realAmount"] = list[i].realAmount;
                dr["serviceUniqueIdentifier"] = list[i].serviceUniqueIdentifier;
                dr["trainName"] = list[i].trainName;
                dr["trainNumber"] = list[i].trainNumber;
                dr["trainType"] = list[i].trainType;

                dataTable.Rows.Add(dr);
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);

        }

        public static QueryResult GetTrainTicketServiceDayTimeGroup(long id, string way)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTrainTicketServiceDayTimeGroup";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetTrainTicketServiceByDayTime(long id, string way, int dayTimeId, int PageNumber = 1, int RecordsInPage = 1)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTrainTicketServiceByDayTime";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@dayTimeId", Value = dayTimeId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@PageNumber", Value = PageNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@RecordsInPage", Value = RecordsInPage });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetTrainTicketService(long id, string way)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTrainTicketService";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetTrainTicketOptionalService(long id, string way, List<Models.TrainTicket.OptionalServiceData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTrainTicketOptionalService";
            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dataTable.Columns.Add("row", typeof(Int32));
            dataTable.Columns.Add("code", typeof(Int32));
            dataTable.Columns.Add("amount", typeof(Int32));
            dataTable.Columns.Add("name", typeof(string));
            dataTable.Columns.Add("description", typeof(string));
            dataTable.Columns.Add("optionalServiceUniqueIdentifier", typeof(string));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["row"] = i + 1;
                dr["code"] = list[i].code;
                dr["amount"] = list[i].amount;
                dr["name"] = list[i].name;
                dr["description"] = list[i].description;
                dr["optionalServiceUniqueIdentifier"] = list[i].optionalServiceUniqueIdentifier;

                dataTable.Rows.Add(dr);
            }

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);

        }
        public static QueryResult GetTrainTicketOptionalService(long id, string way)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTrainTicketOptionalService";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetTrainTicketPassenger(long id, int row, bool isIranian = true, bool isCar = false, string firstName = "", string lastName = "", string nationalCode = "", string passportNumber = "", Models.TrainTicket.TicketPassengerTypeEnum passengerTypeCode = Models.TrainTicket.TicketPassengerTypeEnum.Unknown, string passengerTypeShowName = "", DateTime? dateOfBirth = null, string personel = "", int optionalServiceGo = 0, int optionalServiceReturn = 0)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTrainTicketPassenger";

            if (dateOfBirth != null && dateOfBirth == DateTime.MinValue)
            {
                dateOfBirth = null;
            }

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@row", Value = row });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@isIranian", Value = isIranian });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@isCar", Value = isCar });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@firstName", Value = firstName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@lastName", Value = lastName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@nationalCode", Value = nationalCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@passportNumber", Value = passportNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@passengerTypeCode", Value = passengerTypeCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@passengerTypeShowName", Value = passengerTypeShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@dateOfBirth", Value = dateOfBirth });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@personel", Value = personel });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@optionalServiceGo", Value = optionalServiceGo });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@optionalServiceReturn", Value = optionalServiceReturn });


            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetTrainTicketPassengerInfo(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTrainTicketPassengerInfo";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult UpdateTrainTicketPassenger(long id, string nationalCode, string firstName, string lastName)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_UpdateTrainTicketPassenger";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@nationalCode", Value = nationalCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@firstName", Value = firstName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@lastName", Value = lastName });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult SetTrainTicketWayPrint(long id, string way, int row, string barCodeImage)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTrainTicketWayPrint";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@row", Value = row });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@barCodeImage", Value = barCodeImage });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetTrainTicketWayPrint(long id, string way)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTrainTicketWayPrint";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion


        #region AirplaneTicket
        public static QueryResult GetAirplaneTicketAirports(string code = "", string keyword = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetAirplaneTicketAirports";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@code", Value = code });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@keyword", Value = keyword });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }


        public static QueryResult SetAirplaneTicketAirport(List<Models.AirplaneTicket.AirportData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetAirplaneTicketAirport";

            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dataTable.Columns.Add("airportCode", typeof(string));
            dataTable.Columns.Add("airportShowName", typeof(string));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["airportCode"] = list[i].airportCode;
                dr["airportShowName"] = list[i].airportShowName;

                dataTable.Rows.Add(dr);
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetAirplaneTicketTransaction(long chatId, long id = 0, string sourceAirportCode = "", string sourceAirportShowName = "", string destinationAirportCode = "", string destinationAirportShowName = "", bool twoWay = false, DateTime? wayGoDateTime = null, DateTime? wayReturnDateTime = null, int adultCount = 0, int childCount = 0, int infantCount = 0, int amount = 0, string saleKey = "", int goRow = 0, int returnRow = 0, int currentPassengerRow = 0, TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetAirplaneTicketTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@sourceAirportCode", Value = sourceAirportCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@sourceAirportShowName", Value = sourceAirportShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@destinationAirportCode", Value = destinationAirportCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@destinationAirportShowName", Value = destinationAirportShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@twoWay", Value = twoWay });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@wayGoDateTime", Value = wayGoDateTime });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@wayReturnDateTime", Value = wayReturnDateTime });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@adultCount", Value = adultCount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@childCount", Value = childCount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@infantCount", Value = infantCount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@goRow", Value = goRow });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@returnRow", Value = returnRow });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@currentPassengerRow", Value = currentPassengerRow });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetAirplaneTicketTransaction(long id = 0, string saleKey = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetAirplaneTicketTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetAirplaneTicketService(long id, string way)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetAirplaneTicketService";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetAirplaneTicketService(long id, string way, List<Models.AirplaneTicket.ServiceData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetAirplaneTicketService";

            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dataTable.Columns.Add("row", typeof(Int32));
            dataTable.Columns.Add("systemId", typeof(Int32));
            dataTable.Columns.Add("SourceAirportCode", typeof(string));
            dataTable.Columns.Add("DestinationAirportCode", typeof(string));
            dataTable.Columns.Add("Aircraft", typeof(string));
            dataTable.Columns.Add("AirlineCode", typeof(string));
            dataTable.Columns.Add("FlightID", typeof(string));
            dataTable.Columns.Add("FlightNumber", typeof(string));
            dataTable.Columns.Add("Class", typeof(string));
            dataTable.Columns.Add("ClassType", typeof(string));
            dataTable.Columns.Add("ClassTypeName", typeof(string));
            dataTable.Columns.Add("AmountAdult", typeof(int));
            dataTable.Columns.Add("AmountChild", typeof(int));
            dataTable.Columns.Add("AmountInfant", typeof(int));
            dataTable.Columns.Add("ArrivalTime", typeof(string));
            dataTable.Columns.Add("DayOfWeek", typeof(string));
            dataTable.Columns.Add("DepartureDateTime", typeof(DateTime));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("isCharter", typeof(bool));
            dataTable.Columns.Add("SellerId", typeof(string));
            dataTable.Columns.Add("SellerReference", typeof(string));
            dataTable.Columns.Add("ServiceUniqueIdentifier", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("StatusName", typeof(string));
            dataTable.Columns.Add("SystemKey", typeof(string));


            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["row"] = i + 1;
                dr["systemId"] = list[i].systemId;
                dr["SourceAirportCode"] = list[i].sourceAirportCode;
                dr["DestinationAirportCode"] = list[i].destinationAirportCode;
                dr["Aircraft"] = list[i].aircraft;
                dr["AirlineCode"] = list[i].airlineCode;
                dr["FlightID"] = list[i].flightID;
                dr["FlightNumber"] = list[i].flightNumber;
                dr["Class"] = list[i].Class;
                dr["ClassType"] = list[i].classType;
                dr["ClassTypeName"] = list[i].classTypeName;
                dr["AmountAdult"] = list[i].amountAdult;
                dr["AmountChild"] = list[i].amountChild;
                dr["AmountInfant"] = list[i].amountInfant;
                dr["ArrivalTime"] = list[i].arrivalTime;
                dr["DayOfWeek"] = list[i].dayOfWeek;
                dr["DepartureDateTime"] = list[i].departureDateTime;
                dr["Description"] = list[i].description;
                dr["isCharter"] = list[i].isCharter;
                dr["SellerId"] = list[i].sellerId;
                dr["SellerReference"] = list[i].sellerReference;
                dr["ServiceUniqueIdentifier"] = list[i].serviceUniqueIdentifier;
                dr["Status"] = list[i].status;
                dr["StatusName"] = list[i].statusName;
                dr["SystemKey"] = list[i].systemKey;



                dataTable.Rows.Add(dr);
            }

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);

        }

        public static QueryResult GetAirplaneTicketServiceByDayTime(long id, string way, int dayTimeId, int PageNumber = 1, int RecordsInPage = 1)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetAirplaneTicketServiceByDayTime";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@dayTimeId", Value = dayTimeId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@PageNumber", Value = PageNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@RecordsInPage", Value = RecordsInPage });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetAirplaneTicketServiceDayTimeGroup(long id, string way)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetAirplaneTicketServiceDayTimeGroup";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@way", Value = way });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult SetAirplaneTicketPassenger(long id, int row, bool isIranian = true, string title = "", string firstName = "", string lastName = "", string nationalCode = "", string passportNumber = "", string passportCountry = "", DateTime? passportExpireDate = null, string passengerTypeCode = "", string passengerTypeShowName = "", DateTime? dateOfBirth = null, string htmlGo = "", string htmlReturn = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetAirplaneTicketPassenger";

            if (passportExpireDate != null && passportExpireDate == DateTime.MinValue)
            {
                passportExpireDate = null;
            }

            if (dateOfBirth != null && dateOfBirth == DateTime.MinValue)
            {
                dateOfBirth = null;
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@row", Value = row });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@isIranian", Value = isIranian });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@title", Value = title });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@firstName", Value = firstName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@lastName", Value = lastName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@nationalCode", Value = nationalCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@passportNumber", Value = passportNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@passportCountry", Value = passportCountry });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@passportExpireDate", Value = passportExpireDate });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@passengerTypeCode", Value = passengerTypeCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@passengerTypeShowName", Value = passengerTypeShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@dateOfBirth", Value = dateOfBirth });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@htmlGo", Value = htmlGo });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@htmlReturn", Value = htmlReturn });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetAirplaneTicketPassengerInfo(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetAirplaneTicketPassengerInfo";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }


        public static QueryResult UpdateAirplaneTicketPassenger(long id, string nationalCode = "", string firstName = "", string lastName = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_UpdateAirTicketPassenger";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@nationalCode", Value = nationalCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@firstName", Value = firstName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@lastName", Value = lastName });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        #endregion



        #region XPIN

        public static QueryResult SetXpinTransaction(long chatId, long id = 0, int amount = 0, Models.XPIN.XpinCategoryEnum categoryId = Models.XPIN.XpinCategoryEnum.GiftCard, int productId = 0, string productName = "", string productThumbnail = "", int subProductId = 0, string subProductName = "", string saleKey = "", TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetXpinTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@categoryId", Value = categoryId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@productId", Value = productId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@productName", Value = productName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@productThumbnail", Value = productThumbnail });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@subProductId", Value = subProductId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@subProductName", Value = subProductName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetXpinTransaction(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetXpinTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetXpinProduct(long id, List<Models.XPIN.ProductData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetXpinProduct";

            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())
            dataTable.Columns.Add("productId", typeof(int));
            dataTable.Columns.Add("productName", typeof(string));
            dataTable.Columns.Add("productHintsLink", typeof(string));
            dataTable.Columns.Add("productIcon", typeof(string));
            dataTable.Columns.Add("productThumbnail", typeof(string));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["productId"] = list[i].productId;
                dr["productName"] = list[i].productName;
                dr["productHintsLink"] = list[i].productHintsLink;
                dr["productIcon"] = list[i].productIcon;
                dr["productThumbnail"] = list[i].productThumbnail;
                dataTable.Rows.Add(dr);
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetXpinProduct(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetXpinProduct";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetXpinSubProduct(long id, List<Models.XPIN.SubProductData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetXpinSubProduct";
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("subProductId", typeof(int));
            dataTable.Columns.Add("subProductName", typeof(string));
            dataTable.Columns.Add("subProductAmount", typeof(int));
            dataTable.Columns.Add("subProductHints", typeof(string));
            dataTable.Columns.Add("subProductIsActive", typeof(bool));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["subProductId"] = list[i].subProductId;
                dr["subProductName"] = list[i].subProductName;
                dr["subProductAmount"] = list[i].subProductAmount;
                dr["subProductHints"] = list[i].subProductHints;
                dr["subProductIsActive"] = list[i].subProductIsActive;
                dataTable.Rows.Add(dr);
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);

        }
        public static QueryResult GetXpinSubProduct(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetXpinSubProduct";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion

        #region GasBill
        public static QueryResult SetGasBillTransaction(long chatId, long id, string gasParticipateCode = "", long amount = 0, string billId = "", string paymentId = "", DateTime? FromDate = null, DateTime? toDate = null, DateTime? paymentDeadLineDate = null, TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetGasBillTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@gasParticipateCode", Value = gasParticipateCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@billId", Value = billId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@paymentId", Value = paymentId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@FromDate", Value = FromDate });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@toDate", Value = toDate });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@paymentDeadLineDate", Value = paymentDeadLineDate });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetGasBillTransaction(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetGasBillTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetGasLastBill(long chatId, int count = 5)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getGasLastBill";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@count", Value = count });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }


        #endregion

        #region TMTN
        public static QueryResult SetTMTNSimType(List<Models.TMTN.SimTypeData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTMTNSimType";
            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())
            dataTable.Columns.Add("simTypeId", typeof(int));
            dataTable.Columns.Add("simTypeName", typeof(string));
            dataTable.Columns.Add("simTypeShowName", typeof(string));
            dataTable.Columns.Add("simTypeThumbnail", typeof(string));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["simTypeId"] = list[i].simTypeId;
                dr["simTypeName"] = list[i].simTypeName;
                dr["simTypeShowName"] = list[i].simTypeShowName;
                dr["simTypeThumbnail"] = list[i].simTypeThumbnail;
                dataTable.Rows.Add(dr);
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult SetTMTNSimTypeCategory(int simTypeId, List<Models.TMTN.CategoryData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTMTNSimTypeCategory";
            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dataTable.Columns.Add("categoryId", typeof(int));
            dataTable.Columns.Add("categoryName", typeof(string));
            dataTable.Columns.Add("categoryShowName", typeof(string));
            dataTable.Columns.Add("categoryThumbnail", typeof(string));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["categoryId"] = list[i].categoryId;
                dr["categoryName"] = list[i].categoryName;
                dr["categoryShowName"] = list[i].categoryShowName;
                dr["categoryThumbnail"] = list[i].categoryThumbnail;
                dataTable.Rows.Add(dr);
            }

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@simTypeId", Value = simTypeId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });


            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetTMTNSimType()
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTMTNSimType";

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetTMTNSimTypeCategory(int simTypeId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTMTNSimTypeCategory";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@simTypeId", Value = simTypeId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetTMTNTransaction(long chatId, long id = 0, int simTypeId = 0, string simTypeShowName = "", int categoryId = 0, string categoryShowName = "", int packageId = 0, string packageShowName = "", string packageDescription = "", int amount = 0, string mobileNumber = "", string saleKey = "", TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTMTNTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@simTypeId", Value = simTypeId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@simTypeShowName", Value = simTypeShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@categoryId", Value = categoryId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@categoryShowName", Value = categoryShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@packageId", Value = packageId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@packageShowName", Value = packageShowName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@packageDescription", Value = packageDescription });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@mobileNumber", Value = mobileNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetTMTNTransaction(long id = 0, string saleKey = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTMTNTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetTMTNPackage(int simTypeId, int categoryId, List<Models.TMTN.PackageData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetTMTNPackage";
            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dataTable.Columns.Add("packageId", typeof(int));
            dataTable.Columns.Add("packageName", typeof(string));
            dataTable.Columns.Add("packageShowName", typeof(string));
            dataTable.Columns.Add("packageDescription", typeof(string));
            dataTable.Columns.Add("packageAmount", typeof(int));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["packageId"] = list[i].packageId;
                dr["packageName"] = list[i].packageName;
                dr["packageShowName"] = list[i].packageShowName;
                dr["packageDescription"] = list[i].packageDescription;
                dr["packageAmount"] = list[i].packageAmount;
                dataTable.Rows.Add(dr);
            }

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@simTypeId", Value = simTypeId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@categoryId", Value = categoryId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });


            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetTMTNPackage(int simTypeId, int categoryId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetTMTNPackage";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@simTypeId", Value = simTypeId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@categoryId", Value = categoryId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion

        #region FixedLineNumber
        public static QueryResult SetFixedLineTransaction(long chatId, long id, string fixedLineNumber = "", int amount = 0, string billId = "", string paymentId = "", TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetFixedLineTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@fixedLineNumber", Value = fixedLineNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@billId", Value = billId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@paymentId", Value = paymentId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetFixedLineTransaction(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetFixedLineTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetFixedLineLast(long chatId, int count)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getFixedLineLast";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@count", Value = count });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion

        #region ElectricityBill
        public static QueryResult SetElectricityBillTransaction(long chatId, long id, string electricityBillID = "", int amount = 0, int debt = 0, string fullName = "", string billId = "", string paymentId = "", DateTime? fromDate = null, DateTime? toDate = null, DateTime? paymentDeadLineDate = null, TransactionStatusEnum status = TransactionStatusEnum.NotCompeleted)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetElectricityBillTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@electricityBillID", Value = electricityBillID });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@debt", Value = debt });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@fullName", Value = fullName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@billId", Value = billId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@paymentId", Value = paymentId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@fromDate", Value = fromDate });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@toDate", Value = toDate });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.DateTime, ParameterName = "@paymentDeadLineDate", Value = paymentDeadLineDate });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetElectricityBillTransaction(long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetElectricityBillTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }


        public static QueryResult GetElectricityLastBill(long chatId, int count)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getElectricityLastBill";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@count", Value = count });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion


        public static QueryResult GetChatIdBySaleKey(string saleKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetChatIdBySaleKey";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@saleKey", Value = saleKey });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult ResetAllCurrentStatus()
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_ResetAllCurrentStatus";
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }



        #region Panel

        public static QueryResult GetChatIdByMobile(string[] Mobiles)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetChatIdByMobile";

            DataTable dtDetail = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())

            dtDetail.Columns.Add("mobileNumber", typeof(string));


            for (var i = 0; i < Mobiles.Length; i++)
            {
                DataRow dr = dtDetail.NewRow();
                dr["mobileNumber"] = Mobiles[i];
                dtDetail.Rows.Add(dr);
            }



            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@Mobile", Value = dtDetail });

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetJob(long jobUID, string description = "", string result = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetJobHistory";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@jobUID", Value = jobUID });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@description", Value = description });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@result", Value = result });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetJobHistory(long jobUID)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetJobHistory";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@jobUID", Value = jobUID });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetRandomChatId(int returnNumber = 0)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetRandomChatId";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@returnNumber", Value = returnNumber });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion


        #region Payment


        public static QueryResult SetPayment(long chatId, long id, int productId = 0, string productName = "", string saleKey = "", string discountCode = "", int discountAmount = 0, int amount = 0, string description = "", bool paymentIsPossible = true, string status = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetPayment";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@productId", Value = productId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@productName", Value = productName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@discountCode", Value = discountCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@discountAmount", Value = discountAmount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@description", Value = description });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@paymentIsPossible", Value = paymentIsPossible });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetPayment(long chatId, long id)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetPayment";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetPaymentBySaleKey(string saleKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetPaymentBySaleKey";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult SetPaymentFinished(string saleKey)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetPaymentFinished";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion

        #region CinemaTicket

        public static QueryResult SetCinemaTicketOnScreen(bool success, int messageType, string message, string exception, string info, List<Models.CinemaTicket.FilmData> filmList, List<Models.CinemaTicket.CinemaData> cinemaList)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetCinemaTicketOnScreen";


            DataTable dataTable = new DataTable();
            //foreach (var item in details[0].GetType().GetProperties())
            DataTable filmData = new DataTable();


            filmData.Columns.Add("filmCode", typeof(int));
            filmData.Columns.Add("filmName", typeof(string));
            filmData.Columns.Add("filmId", typeof(int));
            filmData.Columns.Add("categoryId", typeof(int));
            filmData.Columns.Add("categoryName", typeof(string));
            filmData.Columns.Add("profileLink", typeof(string));
            filmData.Columns.Add("rating", typeof(string));
            filmData.Columns.Add("director", typeof(string));
            filmData.Columns.Add("producer", typeof(string));
            filmData.Columns.Add("genre", typeof(string));
            filmData.Columns.Add("summary", typeof(string));
            filmData.Columns.Add("releaseDate", typeof(string));
            filmData.Columns.Add("runningTime", typeof(string));
            filmData.Columns.Add("casting", typeof(string));
            filmData.Columns.Add("distribution", typeof(string));
            filmData.Columns.Add("filmImageUrl", typeof(string));
            filmData.Columns.Add("filmTrailer", typeof(string));

            for (var i = 0; i < filmList.Count; i++)
            {
                DataRow dr = filmData.NewRow();

                dr["filmCode"] = filmList[i].filmCode;
                dr["filmName"] = filmList[i].filmName.TruncateEx(500);
                dr["filmId"] = filmList[i].filmId;
                dr["categoryId"] = filmList[i].categoryId;
                dr["categoryName"] = filmList[i].categoryName.TruncateEx(500);
                dr["profileLink"] = filmList[i].profileLink.TruncateEx(1000);
                dr["rating"] = filmList[i].rating.TruncateEx(10);
                dr["director"] = filmList[i].director.TruncateEx(200);
                dr["producer"] = filmList[i].producer.TruncateEx(200);
                dr["genre"] = filmList[i].genre.TruncateEx(500);
                dr["summary"] = filmList[i].summary.TruncateEx(1500);
                dr["releaseDate"] = filmList[i].releaseDate.TruncateEx(20);
                dr["runningTime"] = filmList[i].runningTime.TruncateEx(500);
                dr["casting"] = filmList[i].casting.TruncateEx(500);
                dr["distribution"] = filmList[i].distribution.TruncateEx(1000);
                dr["filmImageUrl"] = filmList[i].filmImageUrl.TruncateEx(1000);
                dr["filmTrailer"] = filmList[i].filmTrailer.TruncateEx(1000);
                filmData.Rows.Add(dr);
            }

            DataTable cinemaData = new DataTable();

            cinemaData.Columns.Add("cinemaCode", typeof(int));
            cinemaData.Columns.Add("cinemaName", typeof(string));
            cinemaData.Columns.Add("city", typeof(string));
            cinemaData.Columns.Add("address", typeof(string));
            cinemaData.Columns.Add("latitude", typeof(string));
            cinemaData.Columns.Add("longitude", typeof(string));
            cinemaData.Columns.Add("phone", typeof(string));
            cinemaData.Columns.Add("photoUrl", typeof(string));
            cinemaData.Columns.Add("buyTicketOnline", typeof(bool));



            for (var i = 0; i < cinemaList.Count; i++)
            {
                DataRow dr = cinemaData.NewRow();
                dr["cinemaCode"] = cinemaList[i].cinemaCode;
                dr["cinemaName"] = cinemaList[i].cinemaName;
                dr["city"] = cinemaList[i].city;
                dr["address"] = cinemaList[i].address;
                dr["latitude"] = cinemaList[i].latitude;
                dr["longitude"] = cinemaList[i].longitude;
                dr["phone"] = cinemaList[i].phone;
                dr["photoUrl"] = cinemaList[i].photoUrl;
                dr["buyTicketOnline"] = cinemaList[i].buyTicketOnline;
                cinemaData.Rows.Add(dr);
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Bit, ParameterName = "@success", Value = success });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@messageType", Value = messageType });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@message", Value = message });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@exception", Value = exception });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@info", Value = info });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@films", Value = filmData });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@cinemas", Value = cinemaData });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetCinemaTicketOnScreen()
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetCinemaTicketOnScreen";

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult HasCinemaTicketOnScreenExpired()
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_HasCinemaTicketOnScreenExpired";

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult SetCinemaTicketOnScreenBlank()
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetCinemaTicketOnScreenBlank";

            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetCinemaTicketOnScreenData(long chatId, int filmCode = 0, int PageNumber = 1, int RecordsInPage = 1)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetCinemaTicketOnScreenData";
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@filmCode", Value = filmCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@PageNumber", Value = PageNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@RecordsInPage", Value = RecordsInPage });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }


        public static QueryResult SetCinemaTicketTransaction(long chatId, long id = 0, int filmCode = 0, int cinemaCode = 0, int sansCode = 0, string seats = "", string firstName = "", string lastName = "", int amount = 0, string saleKey = "", string status = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetCinemaTicketTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@filmCode", Value = filmCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@cinemaCode", Value = cinemaCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@sansCode", Value = sansCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@seats", Value = seats });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@firstName", Value = firstName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@lastName", Value = lastName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@status", Value = status });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetCinemaTicketTransaction(long id = 0, string saleKey = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetCinemaTicketTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetCinemaTicketTransactionByChatId(long chatId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetCinemaTicketTransactionByChatId";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult SetCinemaTicketSans(long id, List<Models.CinemaTicket.SansData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetCinemaTicketSans";

            //foreach (var item in details[0].GetType().GetProperties())
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("sansCode", typeof(string));
            dataTable.Columns.Add("showDate", typeof(string));
            dataTable.Columns.Add("showDay", typeof(string));
            dataTable.Columns.Add("filmCode", typeof(int));
            dataTable.Columns.Add("filmName", typeof(string));
            dataTable.Columns.Add("sansHour", typeof(string));
            dataTable.Columns.Add("salon", typeof(string));
            dataTable.Columns.Add("sansPrice", typeof(int));
            dataTable.Columns.Add("discount", typeof(string));
            dataTable.Columns.Add("sansPriceDiscount", typeof(int));
            dataTable.Columns.Add("discountRemain", typeof(int));




            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["sansCode"] = list[i].sansCode;
                dr["showDate"] = list[i].showDate;
                dr["showDay"] = list[i].showDay;
                dr["filmCode"] = list[i].filmCode;
                dr["filmName"] = list[i].filmName;
                dr["sansHour"] = list[i].sansHour;
                dr["salon"] = list[i].salon;
                dr["sansPrice"] = list[i].sansPrice;
                dr["discount"] = list[i].discount;
                dr["sansPriceDiscount"] = list[i].sansPriceDiscount;
                dr["discountRemain"] = list[i].discountRemain;
                dataTable.Rows.Add(dr);
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetCinemaTicketSans(long id, int PageNumber = 1, int RecordsInPage = 1)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetCinemaTicketSans";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@PageNumber", Value = PageNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@RecordsInPage", Value = RecordsInPage });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }


        public static QueryResult SetCinemaTicketSeat(long id, int maxSeatsColumnsCount, List<Models.CinemaTicket.SeatData> list)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetCinemaTicketSeat";

            //foreach (var item in details[0].GetType().GetProperties())
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("seatNumber", typeof(int));
            dataTable.Columns.Add("realRowNumber", typeof(string));
            dataTable.Columns.Add("realSeatNumber", typeof(string));
            dataTable.Columns.Add("rowNumber", typeof(string));
            dataTable.Columns.Add("state", typeof(int));
            dataTable.Columns.Add("selected", typeof(bool));



            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["seatNumber"] = list[i].seatNumber;
                dr["realRowNumber"] = list[i].realRowNumber;
                dr["realSeatNumber"] = list[i].realSeatNumber;
                dr["rowNumber"] = list[i].rowNumber;
                dr["state"] = list[i].state;
                dr["selected"] = list[i].selected;
                dataTable.Rows.Add(dr);
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@maxSeatsColumnsCount", Value = maxSeatsColumnsCount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult GetCinemaTicketSeats(long id, int row, int colSection = 1)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetCinemaTicketSeats";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@row", Value = row });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@colSection", Value = colSection });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetCinemaTicketQuery(long chatId, string query = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetCinemaTicketQuery";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@query", Value = query });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        #endregion


        #region MyDbLogger

        public static QueryResult MyDBLogger_ReportSimpayBotLog(string method = "", string action = "", string requestUid = "", string UserUID = "", string requestUri = "", string playLoad = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "MyDBLogger.dbo.ud_prc_ReportSimpayBotLog";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@method", Value = method });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@action", Value = action });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@requestUid", Value = requestUid });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@UserUID", Value = UserUID });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@requestUri", Value = requestUri });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@playLoad", Value = playLoad });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        #endregion


        #region EventsTicket

        public static QueryResult GetEventsTicketTransaction(long id = 0, string saleKey = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetEventsTicketTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult SetEventsTicketTransaction(long chatId, string category, long id, string eventUID = "", string instanceUID = "", string emailAddress = "", string fullName = "", string seats = "", string attachmentURL = "", string instanceTitle = "", long reserveID = 0, string venueAddress = "", string venueTitle = "", long venueCode = 0, int amount = 0, string saleKey = "", int status = 0, string query = "")
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetEventsTicketTransaction";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@chatId", Value = chatId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@category", Value = category });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@eventUID", Value = eventUID });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@instanceUID", Value = instanceUID });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@emailAddress", Value = emailAddress });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@fullName", Value = fullName });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@seats", Value = seats });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@attachmentURL", Value = attachmentURL });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@instanceTitle", Value = instanceTitle });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@reserveID", Value = reserveID });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@venueAddress", Value = venueAddress });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@venueTitle", Value = venueTitle });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@venueCode", Value = venueCode });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@amount", Value = amount });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@saleKey", Value = saleKey });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@status", Value = status });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@query", Value = query });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        public static QueryResult SetEventsTicketEvent(long id, List<Models.EventsTicket.EventInfo> list)
        {

            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_SetEventsTicketEvent";

            //foreach (var item in details[0].GetType().GetProperties())
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("uniqueIdentifier", typeof(string));
            dataTable.Columns.Add("availableForSale", typeof(bool));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("ShortDescription", typeof(string));
            dataTable.Columns.Add("StartDate", typeof(DateTime));
            dataTable.Columns.Add("TimesText", typeof(string));
            dataTable.Columns.Add("EndDate", typeof(DateTime));
            dataTable.Columns.Add("ImageThumbnailURL", typeof(string));
            dataTable.Columns.Add("ImageURL", typeof(string));
            dataTable.Columns.Add("Method", typeof(string));
            dataTable.Columns.Add("AmountsText", typeof(string));
            dataTable.Columns.Add("VenueTitle", typeof(string));

            for (var i = 0; i < list.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["uniqueIdentifier"] = list[i].uniqueIdentifier;
                dr["availableForSale"] = list[i].availableForSale;
                dr["Title"] = list[i].title;
                dr["ShortDescription"] = list[i].shortDescription;
                dr["StartDate"] = list[i].startDate;
                dr["TimesText"] = list[i].timesText;
                dr["EndDate"] = list[i].endDate;
                dr["ImageThumbnailURL"] = list[i].imageThumbnailURL;
                dr["ImageURL"] = list[i].imageURL;
                dr["Method"] = list[i].method;
                dr["AmountsText"] = list[i].amountsText;
                dr["VenueTitle"] = list[i].venueTitle;
                dataTable.Rows.Add(dr);
            }


            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Structured, ParameterName = "@data", Value = dataTable });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);

        }
        public static QueryResult GetEventsTicketEvents(long id, int PageNumber, int RecordsInPage = 1)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_GetEventsTicketEvents";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@id", Value = id });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@PageNumber", Value = PageNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@RecordsInPage", Value = RecordsInPage });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }

        #endregion

    }
}
