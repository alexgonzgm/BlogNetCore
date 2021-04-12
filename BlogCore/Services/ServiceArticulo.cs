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
    public class ServiceArticulo
    {
        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;
        public ServiceArticulo(string url)
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

        public async Task<List<Articulo>> GetArticulosAsync()
        {
            string request = "api/Articulos";
            List<Articulo> articulos = await this.CallApi<List<Articulo>>(request);
            return articulos;
        }
        public async Task<Articulo> FindArticulo(int id)
        {
            string request = "/api/Articulos/" + id;
            Articulo articulo = await this.CallApi<Articulo>(request);
            return articulo;
        }

        public async Task Add(string nombre, string FechaCreacion, string UrlImagen, string descripcion, int CategoriaId, Categoria categoria)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Articulos";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Articulo articulo = new Articulo();
                articulo.Nombre = nombre;
                articulo.FechaCreacion = FechaCreacion;
                articulo.Descripcion = descripcion;
                articulo.UrlImagen = UrlImagen;
                articulo.CategoriaId = CategoriaId;
                articulo.Categoria = categoria;
                string json = JsonConvert.SerializeObject(articulo);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);

            }
        }

        public async Task Delete(int id)
        {
            string request = "/api/Articulos/" + id;
            await this.CallApi<string>(request);
        }

        public async Task Update(int id, string nombre, string FechaCreacion, string UrlImagen, string descripccion, int CategoriaId, Categoria categoria)
        {
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Articulo articulo = new Articulo();
                articulo.Id = id;
                articulo.Nombre = nombre;
                articulo.FechaCreacion = FechaCreacion;
                articulo.UrlImagen = UrlImagen;
                articulo.Descripcion = descripccion;
                articulo.CategoriaId = CategoriaId;
                articulo.Categoria = categoria;
                string json = JsonConvert.SerializeObject(articulo);
                string request = "/api/Articulos/" + id;
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);

            }
        }

    }
}
