using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace DOAN.Models.Client
{
    public class UserType_QuyenClient
    {
        private string Base_URL = "https://localhost:44398/api/";
        public IEnumerable<USERTYPE_QUYEN> findAll()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("usertype_quyen").Result;
                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsAsync<IEnumerable<USERTYPE_QUYEN>>().Result;
                return null;
            }
            catch
            {
                return null;
            }
        }
        public USERTYPE_QUYEN find(int idUT,string quyen)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(string.Format("usertype_quyen??idUT={0}&&quyen={1}",idUT,quyen)).Result;
                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsAsync<USERTYPE_QUYEN>().Result;
                return null;
            }
            catch
            {
                return null;
            }
        }
        public bool Create(USERTYPE_QUYEN usertype_quyen)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync("usertype_quyen",usertype_quyen).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public bool Edit(USERTYPE_QUYEN usertype_quyen)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PutAsJsonAsync(string.Format("usertype_quyen??idUT={0}&&quyen={1}", usertype_quyen.IdUT, usertype_quyen.Quyen),usertype_quyen).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(int idUT,string quyen)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.DeleteAsync(string.Format("usertype_quyen??idUT={0}&&quyen={1}", idUT, quyen)).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}