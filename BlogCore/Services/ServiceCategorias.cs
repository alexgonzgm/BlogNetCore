using BlogCore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Services
{
    public class ServiceCategorias
    {
        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;
        public ServiceCategorias(string url)
        {
            this.UriApi = new Uri(url);
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<T> CallApi<T>(String request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }

        }

        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            string request = "api/Categorias";

            List<Categoria> categorias = await this.CallApi<List<Categoria>>(request);
            return categorias;
        }

        public async Task Add(string nombre, int orden)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Categorias";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Categoria categoria = new Categoria();
                categoria.Nombre = nombre;
                categoria.Orden = orden;
                string json = JsonConvert.SerializeObject(categoria);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);



            }
        }

        public async Task Delete(int id)
        {
            string request = "/api/Categorias/" + id;
            await this.CallApi<string>(request);
        }

        public async Task Update(int id, string nombre, int orden)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Categorias/" + id;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Categoria categoria = await this.Categoria(id);
                categoria.Id = id;
                categoria.Nombre = nombre;
                categoria.Orden = orden;
                string json = JsonConvert.SerializeObject(categoria);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);



            }
        }

        public async Task<Categoria> Categoria(int id)
        {
            string request = "/api/Categorias/" + id;
            Categoria categoria = await this.CallApi<Categoria>(request);
            return categoria;
        }



    }
}
