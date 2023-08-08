using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bharuwa.Common.Utilities
{
    public class SendSMS
    {

        public void sendOrderSMS(string mobileNo, string sendmessage, string DLT_TE_ID)
        {
            try
            {

                if (string.IsNullOrEmpty(DLT_TE_ID)) return;


                //Your authentication key
                string authKey = "327346AvUpynEBDBO5ea69e44";
                //Multiple mobiles numbers separated by comma
                //string mobileNumber = salesOrder.CustMobile;
                string mobileNumber = mobileNo;
                //Sender ID,While using route4 sender id should be 6 characters long.
                string senderId = "ORDEME";
                //Your message to send, Add URL encoding here.
                string message = HttpUtility.UrlEncode(sendmessage);

                //Prepare you post parameters
                StringBuilder sbPostData = new StringBuilder();
                sbPostData.AppendFormat("authkey={0}", authKey);
                sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
                sbPostData.AppendFormat("&DLT_TE_ID={0}", DLT_TE_ID);
                sbPostData.AppendFormat("&message={0}", message);
                sbPostData.AppendFormat("&sender={0}", senderId);
                sbPostData.AppendFormat("&route={0}", "4");
                sbPostData.AppendFormat("&country={0}", "91");


                try
                {
                    //Call Send SMS API
                    string sendSMSUri = "http://api.msg91.com/api/sendhttp.php";
                    //Create HTTPWebrequest
                    HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                    //Prepare and Add URL Encoded data
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] data = encoding.GetBytes(sbPostData.ToString());
                    //Specify post method
                    httpWReq.Method = "POST";
                    httpWReq.ContentType = "application/x-www-form-urlencoded";
                    httpWReq.ContentLength = data.Length;
                    using (Stream stream = httpWReq.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    //Get the response
                    HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseString = reader.ReadToEnd();

                    //Close the response
                    reader.Close();
                    response.Close();
                }
                catch (SystemException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
