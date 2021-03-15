using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhamacyViewer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PhamacyViewer
{
    static public class SmartHospitalAPI
    {
        static public HttpStatusCode Authorization(string email, string password, out string jwtToken)
        {
            try
            {
                JObject jObject = new JObject();
                jObject.Add("email", email);
                jObject.Add("password", password);
                byte[] data = Encoding.Default.GetBytes(jObject.ToString()); // note: choose appropriate encoding

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://pharmvstu.azurewebsites.net/api/auth");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = data.Length;

                var newStream = httpWebRequest.GetRequestStream(); // get a ref to the request body so it can be modified
                newStream.Write(data, 0, data.Length);
                newStream.Close();

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string jResponse = reader.ReadToEnd();
                            jwtToken = JObject.Parse(jResponse)["access_token"].ToString();

                        }
                    }
                }
                else { jwtToken = null; }

                return httpWebResponse.StatusCode;
            }
            catch (Exception ex)
            {
                var webEx = (WebException)ex;
                jwtToken = null;
                return (HttpStatusCode)webEx.Status; // httpWebResponse.StatusCode;
            }

        }
        static public HttpStatusCode Registration(string email, string password, string fio, bool isDoctor, out string jwtToken)
        {
            try
            {
                JObject jObject = new JObject();
                jObject.Add("email", email);
                jObject.Add("password", password);
                jObject.Add("fio", fio);
                jObject.Add("doctor", isDoctor);
                byte[] data = Encoding.Default.GetBytes(jObject.ToString()); // note: choose appropriate encoding

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://pharmvstu.azurewebsites.net/api/reg");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = data.Length;

                var newStream = httpWebRequest.GetRequestStream(); // get a ref to the request body so it can be modified
                newStream.Write(data, 0, data.Length);
                newStream.Close();

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string jResponse = reader.ReadToEnd();
                            jwtToken = JObject.Parse(jResponse)["access_token"].ToString();
                        }
                    }
                }
                else { jwtToken = null; }

                return httpWebResponse.StatusCode;
            }
            catch (Exception ex)
            {
                var webEx = (WebException)ex;
                jwtToken = null;
                return (HttpStatusCode)webEx.Status; // httpWebResponse.StatusCode;
            }
        }
        static public HttpStatusCode GetUser(string jwtToken, out JObject user)
        {
            try
            {
                var baseAddress = new Uri("https://pharmvstu.azurewebsites.net/api/");

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {jwtToken}");

                    using (var response =  httpClient.GetAsync("user").Result)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string responseData = response.Content.ReadAsStringAsync().Result;
                            user = JObject.Parse(responseData);
                        }
                        else user = null;
                        return response.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                var webEx = (WebException)ex;
                user = null;
                return (HttpStatusCode)webEx.Status; // httpWebResponse.StatusCode;
            }
        }
        static public HttpStatusCode GetCard(string jwtToken, int cardId, out JObject card)
        {
            try
            {
                var baseAddress = new Uri("https://pharmvstu.azurewebsites.net/api/");
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {jwtToken}");
                    using (var response = httpClient.GetAsync($"patient/card?cardId={cardId}").Result)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string responseData = response.Content.ReadAsStringAsync().Result;
                            JArray jArr = JArray.Parse(responseData);
                            card = new JObject();
                            card.Add("History", jArr);
                        }
                        else card = null;
                        return response.StatusCode;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                var webEx = (WebException)ex;
                card = null;
                return (HttpStatusCode)webEx.Status; // httpWebResponse.StatusCode;
            }
        }
        static public HttpStatusCode GetDoctor(string jwtToken, int id, out Doctor doctor)
        {
            try
            {
                var baseAddress = new Uri("https://pharmvstu.azurewebsites.net/api/");

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    using (var response =  httpClient.GetAsync($"doctor/info?doctorId={id})").Result)
                    {
                        string responseData = response.Content.ReadAsStringAsync().Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            doctor = JsonConvert.DeserializeObject<Doctor>(responseData);
                        }else { doctor = null; }
                        return response.StatusCode;
                    } 
                }  
            }
            catch (Exception ex)
            {
                var webEx = (WebException)ex;
                doctor = null;
                return (HttpStatusCode)webEx.Status; // httpWebResponse.StatusCode;
            }  
        }
        static public HttpStatusCode GetInfo(string jwtToken, out JObject info)
        {
            try
            {
                var baseAddress = new Uri("https://pharmvstu.azurewebsites.net/api/");

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {jwtToken}");

                    using (var response = httpClient.GetAsync($"patient/info").Result)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string responseData = response.Content.ReadAsStringAsync().Result;
                            info = JObject.Parse(responseData);
                        }
                        else info = null;
                        return response.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                var webEx = (WebException)ex;
                info = null;
                return (HttpStatusCode)webEx.Status; // httpWebResponse.StatusCode;
            }
        }
    }
}
