using System;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Shared.WebService
{
    public static partial class Converter
    {
        public static string GetHostName()
        {
            var hostName = string.Empty;
            var sw = Stopwatch.StartNew();

            try
            {
                hostName = HttpContext.Current.Request.UserHostName;

                Log.Info("get consumer host name successfully completed.", sw.Elapsed.TotalMilliseconds);
                return hostName;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return hostName;
            }
        }
        public static string UniqueString()
        {
            var sw = Stopwatch.StartNew();
            var uniqueString = string.Empty;

            try
            {
                uniqueString = (Guid.NewGuid()).ToString();

                Log.Info("generating unique string completed successfully.", sw.Elapsed.TotalMilliseconds);
                return uniqueString;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return uniqueString;
            }
        }
        public static string GetIpAddress()
        {
            var ipAddress = string.Empty;
            var sw = Stopwatch.StartNew();

            try
            {
                ipAddress = HttpContext.Current.Request.UserHostAddress;

                Log.Info("get consumer IP address successfully completed.", sw.Elapsed.TotalMilliseconds);
                return ipAddress;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return ipAddress;
            }
        }
        public static string GetCalledUrl()
        {
            var calledUrl = string.Empty;
            var sw = Stopwatch.StartNew();

            try
            {
                calledUrl = HttpContext.Current.Request.HttpMethod + " " + HttpContext.Current.Request.Url.OriginalString;

                Log.Info("get consumer called URL successfully completed.", sw.Elapsed.TotalMilliseconds);
                return calledUrl;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return calledUrl;
            }
        }
        public static string GetUserAgent()
        {
            var userAgent = string.Empty;
            var sw = Stopwatch.StartNew();

            try
            {
                userAgent = HttpContext.Current.Request.UserAgent;

                Log.Info("get consumer user agent successfully completed.", sw.Elapsed.TotalMilliseconds);
                return userAgent;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return userAgent;
            }
        }
        public static string ToString(Enum enumValue)
        {
            var enumString = string.Empty;
            var sw = Stopwatch.StartNew();

            try
            {
                enumString = Enum.GetName(enumValue.GetType(), enumValue);

                Log.Info("converting enum to string successfully completed.", sw.Elapsed.TotalMilliseconds);
                return enumString;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return enumString;
            }
        }
        public static DataSet DBNull(DataSet dataSet)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                foreach (DataTable dataTable in dataSet.Tables)
                    foreach (DataRow dataRow in dataTable.Rows)
                        foreach (DataColumn dataColumn in dataTable.Columns)
                            if (dataRow.IsNull(dataColumn))
                            {
                                if (dataColumn.DataType.IsValueType) dataRow[dataColumn] = Activator.CreateInstance(dataColumn.DataType);
                                else if (dataColumn.DataType == typeof(string)) dataRow[dataColumn] = string.Empty;
                                else if (dataColumn.DataType == typeof(bool)) dataRow[dataColumn] = false;
                                else if (dataColumn.DataType == typeof(DateTime)) dataRow[dataColumn] = DateTime.MinValue;
                                else if (dataColumn.DataType == typeof(int) || dataColumn.DataType == typeof(byte) || dataColumn.DataType == typeof(short) || dataColumn.DataType == typeof(long) || dataColumn.DataType == typeof(float) || dataColumn.DataType == typeof(double)) dataRow[dataColumn] = 0;
                                else dataRow[dataColumn] = null;
                            }

                Log.Info("converting the data base null values successfully completed.", sw.Elapsed.TotalMilliseconds);
                return dataSet;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return dataSet;
            }
        }
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Encrypt(string plainText)
        {
            var sw = Stopwatch.StartNew();
            var encryptedString = string.Empty;
            var vectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                using (var password = new PasswordDeriveBytes(ProjectValues.CryptographyKey, null))
                {
                    var keyBytes = password.GetBytes(32);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, vectorBytes))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    var cipherTextBytes = memoryStream.ToArray();
                                    encryptedString = Convert.ToBase64String(cipherTextBytes);
                                }
                            }
                        }
                    }
                }

                Log.Info("encrypting successfully completed.", sw.Elapsed.TotalMilliseconds);
                return encryptedString;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return encryptedString;
            }
        }
        public static T ToEnum<T>(string stringValue) where T : struct
        {
            var enumValue = default(T);
            var sw = Stopwatch.StartNew();

            try
            {
                enumValue = (T)Enum.Parse(typeof(T), stringValue, true);

                Log.Info("converting string to enum successfully completed.", sw.Elapsed.TotalMilliseconds);
                return enumValue;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return enumValue;
            }
        }
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Decrypt(string cipherText)
        {
            var sw = Stopwatch.StartNew();
            var decryptedString = string.Empty;
            var vectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

            try
            {
                var cipherTextBytes = Convert.FromBase64String(cipherText);
                using (var password = new PasswordDeriveBytes(ProjectValues.CryptographyKey, null))
                {
                    var keyBytes = password.GetBytes(32);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, vectorBytes))
                        {
                            using (var memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[cipherTextBytes.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    decryptedString = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }

                Log.Info("decrypting successfully completed.", sw.Elapsed.TotalMilliseconds);
                return decryptedString;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return decryptedString;
            }
        }
        public static string GetUserName(string identity)
        {
            var userName = string.Empty;
            var sw = Stopwatch.StartNew();

            try
            {
                //userName = string.IsNullOrEmpty(identity) ? ProjectValues.Consumer : JsonWebToken.Verification(identity, ProjectValues.SecretKey, true) ? JsonWebToken.Decode(identity, ProjectValues.SecretKey).Payload?.UserName : identity;
                userName = ProjectValues.Consumer;

                Log.Info("get consumer user name successfully completed.", sw.Elapsed.TotalMilliseconds);
                return userName;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return userName;
            }
        }
        public static string ToPersianString(string number)
        {
            var persianString = string.Empty;
            var sw = Stopwatch.StartNew();

            try
            {
                //persianString = PersianWord.ToPersianString(number);

                Log.Info("converting to persian string successfully completed.", sw.Elapsed.TotalMilliseconds);
                return persianString;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return persianString;
            }
        }
        public static string ToEnglishString(string number)
        {
            var sw = Stopwatch.StartNew();
            var englishString = string.Empty;
            var Arabic = new string[10] { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩" };
            var persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            try
            {
                if (string.IsNullOrEmpty(number) == false)
                    for (int i = 0; i < persian.Length; i++)
                    {
                        number = number.Replace(persian[i], i.ToString());
                        number = number.Replace(Arabic[i], i.ToString());
                    }
                englishString = number;

                //Log.Info("converting to english string successfully completed.", sw.Elapsed.TotalMilliseconds);
                return englishString;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return englishString;
            }
        }
        public static string ToSHA256Hash(string plainTextValue)
        {
            var hashValue = string.Empty;
            var sw = Stopwatch.StartNew();

            try
            {
                var data = Encoding.UTF8.GetBytes(plainTextValue);
                using (var shaM = new SHA256Managed())
                {
                    var result = shaM.ComputeHash(data);
                    hashValue = BitConverter.ToString(result).Replace("-", "");
                }

                //Log.Info("converting to sha256 hash value completed successfully.", sw.Elapsed.TotalMilliseconds);
                return hashValue;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            var instance = default(T[]);
            var sw = Stopwatch.StartNew();

            try
            {
                instance = new T[source.Length - 1];
                if (index > 0)
                    Array.Copy(source, 0, instance, 0, index);

                if (index < source.Length - 1)
                    Array.Copy(source, index + 1, instance, index, source.Length - index - 1);

                Log.Info("removing from array member successfully completed.", sw.Elapsed.TotalMilliseconds);
                return instance;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return instance;
            }
        }
    }
}