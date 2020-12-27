using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace DOAN.Models.Client
{
    public class SinhVien_DeTaiClient
    {
        private string Base_URL = "https://localhost:44398/api/";
        public IEnumerable<SINHVIEN_DETAI> findAll()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("sinhvien_detai").Result;
                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsAsync<IEnumerable<SINHVIEN_DETAI>>().Result;
                return null;
            }
            catch
            {
                return null;
            }
        }
        public SINHVIEN_DETAI find(int idDT, int idSV)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(string.Format("sinhvien_detai??idDT={0}&&idSV={1}", idDT, idSV)).Result;
                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsAsync<SINHVIEN_DETAI>().Result;
                return null;
            }
            catch
            {
                return null;
            }
        }
        public bool Create(SINHVIEN_DETAI sinhvien_detai)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync("sinhvien_detai", sinhvien_detai).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public bool Edit(SINHVIEN_DETAI sinhvien_detai)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PutAsJsonAsync(string.Format("sinhvien_detai??idDT={0}&&idSV={1}",sinhvien_detai.DeTai,sinhvien_detai.SinhVien), sinhvien_detai).Result;
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
                HttpResponseMessage response = client.DeleteAsync(string.Format("sinhvien_detai??idDT={0}&&idSV={1}", idDT, idSV)).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}