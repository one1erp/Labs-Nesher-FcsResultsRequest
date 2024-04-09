using System;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using RestSharp;
using System.Net;
using System.Windows.Forms;

namespace FcsLogic
{

    public class SendToMSB
    {

        public responseReq SendRequest(string url)
        {
            responseReq res = new responseReq(false, null);
            try
            {

                var client = new RestClient(url);
                var request = new RestRequest(Method.Get);

                IRestResponse response = client.Execute(request);

                if (response != null && (int)response.StatusCode == 200)
                {
                    if ((int)response.StatusCode == 200)
                    {
                        res.success = true;
                        res.str = response.Content;
                    }
                    else
                        res.success = false;

                    foreach (var h in response.Headers)
                    {
                        if (h.Name == "message")
                        {
                            string err = h.Value.ToString();
                            err.Replace(";", "/n");
                            Console.WriteLine(err);

                        }
                    }
                }


                return res;
            }
            catch (Exception e)
            {
                //throw the exeption
          //      MessageBox.Show("Err at request: " + e.Message);
                return res;
            }


        }
    }
}









