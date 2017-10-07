using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using PhoneNumbers;

namespace Shared.WebService
{
    public partial class Mobile
    {
        private string _nationalNumber;
        private string _internationalNumber;
        public string Number { get; set; }
        //public Reason Reason { get; set; }
        public bool NumberContentIsValid { get; set; }
        public string NationalNumber { set { _nationalNumber = value; } get { return _nationalNumber ?? (_nationalNumber = InitiateNationalNumber()); } }
        public string InternationalNumber { set { _internationalNumber = value; } get { return _internationalNumber ?? (_internationalNumber = InitiateInternationalNumber()); } }


        public bool IsNumberContentValid()
        {
            var sw = Stopwatch.StartNew();
            var len = new Regex(@"^.{2,20}$");
            var digit = new Regex(@"^[0-9]*$");
            Number = Number.StartsWith(@"+") ? Number.Substring(1) : Number;
            Number = Number.Replace(" ", string.Empty);

            try
            {
                if (!len.IsMatch(Number))
                {
                    NumberContentIsValid = false;
                    //Reason = new Reason("GC0001");
                }
                else if (!digit.IsMatch(Number))
                {
                    NumberContentIsValid = false;
                    //Reason = new Reason("GC0001");
                }
                else
                {
                    var phoneUtil = PhoneNumberUtil.GetInstance();
                    var mobile = phoneUtil.Parse(Number, "IR");
                    //if (phoneUtil.IsValidNumber(mobile) != true)
                    //{
                    //    NumberContentIsValid = false;
                    //    //Reason = new Reason("GC0001");
                    //}
                    //else if (phoneUtil.GetNumberType(mobile) != PhoneNumberType.MOBILE)
                    //{
                    //    NumberContentIsValid = false;
                    //    //Reason = new Reason("GC0001");
                    //}
                    //else
                    NumberContentIsValid = true;
                }
                //Log.Info("mobile content validation completed successfully.", sw.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                NumberContentIsValid = false;
                //Reason = ProjectValues.ReasonStatusError;
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
            }
            return NumberContentIsValid;
        }
        private string InitiateNationalNumber()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();
                var phone = phoneUtil.Parse(Number ?? _internationalNumber, "IR");
                var nationalNumber = phoneUtil.Format(phone, PhoneNumberFormat.INTERNATIONAL);
                nationalNumber = "0" + nationalNumber.Replace(" ", string.Empty).Remove(0, 3);

                Log.Info("calculating national number completed successfully.", sw.Elapsed.TotalMilliseconds);
                return nationalNumber;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }

        }
        private string InitiateInternationalNumber()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();
                var phone = phoneUtil.Parse(Number ?? _nationalNumber, "IR");
                var internationalNumber = phoneUtil.Format(phone, PhoneNumberFormat.INTERNATIONAL);

                Log.Info("calculating international number completed successfully.", sw.Elapsed.TotalMilliseconds);
                return internationalNumber;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
    }
}