using Newtonsoft.Json;
using System;
using System.Net;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Dynamic;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace Shared.WebService
{
    class Utils
    {
        public static string standardizeMobileNumber(string MobileNumber)
        {
            if (String.IsNullOrEmpty(MobileNumber)) return "";
            string Ret = MobileNumber;
            while (!((Ret.IndexOf("0") != 0) | Ret == ""))
            {
                Ret = Ret.Substring(1);
            }
            return Ret;
        }
        public static string ConvertClassToJson(object obj)
        {
            var settings = new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            return JsonConvert.SerializeObject(obj, settings);
        }
        public static T ConvertJsonToClass<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }

        public static string FileExtention(string filePath)
        {
            var lastIndexOfDot = filePath.LastIndexOf('.');
            var ans = lastIndexOfDot < 0 ? "" : filePath.Substring(lastIndexOfDot + 1);
            return ans;
        }
        public static bool FolderExists(string folderPath, bool forceToCreate = false)
        {
            var ans = false;
            do
            {
                try
                {
                    if (!Directory.Exists(folderPath))
                    {
                        ans = false;
                        if (forceToCreate)
                        {
                            Directory.CreateDirectory(folderPath);
                            ans = true;
                        }

                    }
                    else
                    {
                        ans = true;
                    }


                }
                catch (Exception)
                {

                    ans = false;
                }
            } while (false);
            return ans;
        }
        public static string callURL(string theURL, string Method, string PostData = "", string HeaderData = "", bool isJSON = false)
        {
            string responsebody = "0";
            var reqparm = new NameValueCollection();

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                if (Method.ToUpper() == "POST")
                {
                    if (PostData != "" & !isJSON)
                    {
                        string[] arrP = PostData.Split('&');
                        for (Int32 I = arrP.GetLowerBound(0); I <= arrP.GetUpperBound(0); I++)
                        {
                            string[] arrV = arrP[I].Split('=');
                            if (arrV.GetUpperBound(0) == 1)
                            {
                                reqparm.Add(arrV[0], arrV[1]);
                            }
                            arrV = null;
                        }
                        arrP = null;
                    }
                }
                try
                {
                    if (HeaderData != "")
                    {
                        System.Collections.Specialized.NameValueCollection headerparam = new System.Collections.Specialized.NameValueCollection();
                        string[] arrP = HeaderData.Split('&');
                        for (Int32 I = arrP.GetLowerBound(0); I <= arrP.GetUpperBound(0); I++)
                        {
                            string[] arrV = arrP[I].Split('=');
                            headerparam.Add(arrV[0], arrV[1]);
                            arrV = null;
                        }
                        arrP = null;
                        client.Headers.Add(headerparam);

                    }
                    byte[] responsebytes;
                    client.Encoding = System.Text.Encoding.UTF8;
                    if (Method == "GET")
                    {
                        //client.Headers.Add("Content-Type", "application/json")
                        responsebody = client.DownloadString(theURL);
                    }
                    else
                    {
                        if (isJSON)
                        {
                            client.Headers.Add("Content-Type", "application/json");
                            responsebody = client.UploadString(theURL, Method, PostData);
                        }
                        else
                        {
                            responsebytes = client.UploadValues(theURL, Method, reqparm);
                            responsebody = (new System.Text.UTF8Encoding()).GetString(responsebytes);
                        }
                    }

                }
                catch (Exception ex)
                {
                    //WriteLog("URL:" & theURL & " ; " & ex.Message)
                    responsebody = "-" + ex.Message;
                }
                return responsebody;

            }




        }

        public static HTTPResponse WebRequestByUrl(string theURL, string PostData = null, NameValueCollection headerparam = null)
        {
            HTTPResponse responseBody = new HTTPResponse();
            using (WebClient client = new WebClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
                try
                {
                    if (headerparam != null)
                    {
                        client.Headers.Clear();
                        client.Headers.Add(headerparam);

                    }


                    client.Encoding = System.Text.Encoding.UTF8;

                    if (String.IsNullOrEmpty(PostData))
                        responseBody.responseText = client.DownloadString(theURL); //.UploadString(theURL, "POST", PostData);
                    else
                        responseBody.responseText = client.UploadString(theURL, "POST", PostData);

                }
                catch (WebException ex)
                {
                    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    responseBody.status = ex.Status;
                    responseBody.statusMessage = ex.Message;
                    responseBody.responseText = resp;
                    Log.Error(ex.Message + " " + ex.Status + " - Response: " + responseBody.responseText, 0);
                }
                return responseBody;
            }
        }

        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        public static bool isShamsiDate(string shamsDate)
        {
            var ans = true;
            do
            {

            } while (false);
            return ans;
        }

        public static string Shamsi(string MDate)
        {
            if (MDate == "") return "";
            DateTime MiladiDate = Convert.ToDateTime(MDate);
            System.Globalization.PersianCalendar S = new System.Globalization.PersianCalendar();
            string Ret = "";
            Ret = S.GetYear(Convert.ToDateTime(MiladiDate)) + "/" + S.GetMonth(Convert.ToDateTime(MiladiDate)).ToString("00") + "/" + S.GetDayOfMonth(Convert.ToDateTime(MiladiDate)).ToString("00");

            if (MiladiDate.ToString().Trim().Length > 11)
            {
                string sTime = S.GetHour(Convert.ToDateTime(MiladiDate)).ToString("00") + ":" + S.GetMinute(Convert.ToDateTime(MiladiDate)).ToString("00");
                if (sTime != "00:00")
                {
                    Ret += " " + sTime;
                }
            }
            return Ret;
        }
        public static string Miladi(string shamsiDate)
        {
            string ans = "";
            string CleanShamsiDate = shamsiDate.Replace("/", "");
            object YYYY = CleanShamsiDate.Substring(0, 4);
            object MM = CleanShamsiDate.Substring(4, 2);
            object DD = CleanShamsiDate.Substring(6, 2);
            DateTime PersianDate = new DateTime(Convert.ToInt32(YYYY), Convert.ToInt32(MM), Convert.ToInt32(DD), new System.Globalization.PersianCalendar());
            System.Globalization.GregorianCalendar M = new System.Globalization.GregorianCalendar();
            ans = M.GetYear(PersianDate) + "/" + M.GetMonth(PersianDate).ToString("00") + "/" + M.GetDayOfMonth(PersianDate).ToString("00");
            return ans;
        }

        public sealed class HttpHelper
        {
            private HttpHelper()
            {
            }
            /// <summary>
            /// This method prepares an Html form which holds all data in hidden field in the addetion to form submitting script.
            /// </summary>
            /// <param name="url">The destination Url to which the post and redirection will occur, the Url can be in the same App or ouside the App.</param>
            /// <param name="data">A collection of data that will be posted to the destination Url.</param>
            /// <returns>Returns a string representation of the Posting form.</returns>
            /// <Author>Samer Abu Rabie</Author>
            private static String PreparePOSTForm(string url, NameValueCollection data)
            {
                //Set a name for the form
                string formID = "PostForm";

                //Build the form using the specified data to be posted.
                StringBuilder strForm = new StringBuilder();
                strForm.Append((Convert.ToString((Convert.ToString((Convert.ToString("<form id=\"") + formID) + "\" name=\"") + formID) + "\" action=\"") + url) + "\" method=\"POST\">");
                foreach (string key in data)
                {
                    strForm.Append((Convert.ToString("<input type=\"hidden\" id=\"" + key + "\"  name=\"") + key) + "\" value=\"" + data[key] + "\">");
                }
                strForm.Append("</form>");

                //Build the JavaScript which will do the Posting operation.
                StringBuilder strScript = new StringBuilder();
                strScript.Append("<script language='javascript'>");
                strScript.Append((Convert.ToString((Convert.ToString("var v") + formID) + " = document.") + formID) + ";");
                strScript.Append((Convert.ToString("v") + formID) + ".submit();");
                strScript.Append("</script>");

                //Return the form and the script concatenated. (The order is important, Form then JavaScript)
                return strForm.ToString() + strScript.ToString();
            }
            /// <summary>
            /// POST data and Redirect to the specified url using the specified page.
            /// </summary>
            /// <param name="page">The page which will be the referrer page.</param>
            /// <param name="destinationUrl">The destination Url to which the post and redirection is occuring.</param>
            /// <param name="data">The data should be posted.</param>
            /// <Author>Samer Abu Rabie</Author>
            public static void RedirectAndPOST(Page page, string destinationUrl, NameValueCollection data)
            {
                //Prepare the Posting form
                string strForm = PreparePOSTForm(destinationUrl, data);

                //Add a literal control the specified page holding the Post Form, this is to submit the Posting form with the request.
                page.Controls.Add(new LiteralControl(strForm));
            }
        }


        public static bool IsValidNationalCode(string nationalCode)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                if (string.IsNullOrWhiteSpace(nationalCode))
                    return false;

                if (nationalCode.Length != 10)
                    return false;

                var regex = new Regex(@"\d{10}");
                if (!regex.IsMatch(nationalCode))
                    return false;

                var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
                if (allDigitEqual.Contains(nationalCode))
                    return false;

                var chArray = nationalCode.ToCharArray();
                var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
                var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
                var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
                var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
                var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
                var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
                var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
                var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
                var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
                var a = Convert.ToInt32(chArray[9].ToString());
                var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
                var c = b % 11;
                var isValid = (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));

                //Log.Info("validating national code successfully completed.", sw.Elapsed.TotalMilliseconds);
                return isValid;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return false;
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static bool isInteger(string Number)
        {
            int n = 0;
            bool isNumeric = int.TryParse(Number, out n);
            return isNumeric;
        }

        public static bool isLong(string Number)
        {
            long n = 0;
            bool isNumeric = long.TryParse(Number, out n);
            return isNumeric;
        }
        public static bool isDouble(string Number)
        {
            double n = 0;
            bool isNumeric = double.TryParse(Number, out n);
            return isNumeric;
        }


        public static int getMonthId(string month)
        {
            var monthId = 0;
            while (true)
            {
                if (isInteger(month))
                {
                    Convert.ToInt32(month);
                    break;
                }
                string[] months = { "فروردین", "اردیبهشت", "مرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
                for (var i = 0; i < months.Length; i++)
                {
                    if (months[i] == month)
                    {
                        monthId = i + 1;
                        break;
                    }
                }
                break;
            }
            return monthId;
        }
        //public static string GetStringNumber(int number)
        //{
        //    string[] Ones = new[] { "اول", "دوم" };

        //}
        public static Type GetDataTableType(SqlDbType sqlDataType)
        {
            switch (sqlDataType)
            {
                case SqlDbType.BigInt:
                    return typeof(long?);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return typeof(bool?);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    return typeof(DateTime?);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return typeof(decimal?);

                case SqlDbType.Float:
                    return typeof(double?);

                case SqlDbType.Int:
                    return typeof(int?);

                case SqlDbType.Real:
                    return typeof(float?);

                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid?);

                case SqlDbType.SmallInt:
                    return typeof(short?);

                case SqlDbType.TinyInt:
                    return typeof(byte?);

                case SqlDbType.Variant:
                case SqlDbType.Udt:
                    return typeof(object);

                case SqlDbType.Structured:
                    return typeof(DataTable);

                case SqlDbType.DateTimeOffset:
                    return typeof(DateTimeOffset?);

                default:
                    throw new ArgumentOutOfRangeException("sqlDataType");
            }
        }
        public static int getRandomNumber(int min, int max)
        {
            var rnd = new Random();
            return rnd.Next(min, max);
        }
        public static bool isItOnlyEnglishCharecter(string textToCheck)
        {
            var englishMatch = new Regex(@"^[a-zA-Z]+$");
            return englishMatch.IsMatch(textToCheck);
        }
        public static string removeWhiteSpace(string text)
        {
            return Regex.Replace(text, @"\s+", "");
        }


        public class HTTPResponse
        {
            public WebExceptionStatus status { get; set; }
            public string statusMessage { get; set; }
            public string responseText { get; set; }
        }


        public static Image DrawText(String text, Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);
            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }
        public static Stream ImageToStream(Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }
        public static string testStackTrace()
        {
            return getLastFrame();
        }

        public static string getLastFrame()
        {
            var ans = "";
            var stackTrace = new StackTrace(1, false);
            ans = $"{stackTrace.GetFrame(2).GetMethod().DeclaringType}.{stackTrace.GetFrame(1).GetMethod().Name}";
            return ans;


        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }



        public static bool DownloadRemoteImageFile(string uri, string fileName)
        {
            var ans = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect))
            {

                using (Stream inputStream = response.GetResponseStream())
                {
                    using (Stream outputStream = File.OpenWrite(fileName))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        do
                        {
                            ans = true;
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                }


                // if the remote file was found, download oit
            }
            return ans;
        }

        public static bool WriteTextToFile(string textToBeSaved, string filePath, bool removeExistingFile = true)
        {

            var ans = false;
            do
            {
                try
                {
                    if (removeExistingFile)
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                    else
                    {
                        if (File.Exists(filePath))// if file exists, abort the mission!
                        {
                            break;
                        }
                    }
                    var file = new System.IO.StreamWriter(filePath, false);
                    file.WriteLine(textToBeSaved);
                    file.Close();

                    ans = true;
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex.Message, 0);
                }

            } while (false);
            // Write the string to a file.
            return ans;
        }
        public static string ReadFromTextFile(string filePath)
        {
            var text = "";
            try
            {
                text = System.IO.File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, 0);
            }

            return text;
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


    }


    /// <summary> /// DateTime Iso Extensions. /// </summary>

}
