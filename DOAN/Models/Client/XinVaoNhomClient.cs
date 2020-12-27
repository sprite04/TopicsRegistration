using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace DOAN.Models.Client
{
    public class XinVaoNhomClient
    {
        private string Base_URL = "https://localhost:44398/api/";
        public IEnumerable<XINVAONHOM> findAll()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("xinvaonhoms").Result;
                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsAsync<IEnumerable<XINVAONHOM>>().Result;
                return null;
            }
            catch
            {
                return null;
            }
        }
        public XINVAONHOM find(int idDT, int idSV)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(string.Format("xinvaonhoms??idDT={0}&&idSV={1}", idDT, idSV)).Result;
                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsAsync<XINVAONHOM>().Result;
                return null;
            }
            catch
            {
                return null;
            }
        }
        public bool Create(XINVAONHOM xinvaonhom)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync("xinvaonhoms", xinvaonhom).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public bool Edit(XINVAONHOM xinvaonhom)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PutAsJsonAsync(string.Format("xinvaonhoms??idDT={0}&&idSV={1}", xinvaonhom.DeTai, xinvaonhom.NguoiGui),xinvaonhom).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(int idDT, int idSV)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.DeleteAsync(string.Format("xinvaonhoms??idDT={0}&&idSV={1}", idDT, idSV)).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}