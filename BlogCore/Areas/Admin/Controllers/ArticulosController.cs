using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using BlogCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;
        private ServiceArticulo serviceArticulo;
        private ServiceCategorias serviceCategorias;
        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostEnvironment, ServiceArticulo serviceArticulo, ServiceCategorias serviceCategorias)
        {
            this._contenedorTrabajo = contenedorTrabajo;
            this._hostEnvironment = hostEnvironment;
            this.serviceArticulo = serviceArticulo;
            this.serviceCategorias = serviceCategorias;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Categoria> categorias = await this.serviceCategorias.GetCategoriasAsync();
            var select = categorias.Select(i => new SelectListItem()
            {
                Text = i.Nombre,
                Value = i.Id.ToString()
            });
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new Articulo(),
                ListaCategorias = select

            };
            return View(articuloVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticuloVM articuloVm)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (articuloVm.Articulo.Id == 0)
                {
                    //nuevo Articulo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    articuloVm.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    articuloVm.Articulo.FechaCreacion = DateTime.Now.ToString();

                    await this.serviceArticulo.Add(articuloVm.Articulo.Nombre, articuloVm.Articulo.FechaCreacion, articuloVm.Articulo.UrlImagen,
                        articuloVm.Articulo.Descripcion, articuloVm.Articulo.CategoriaId, articuloVm.Articulo.Categoria);
                    //_contenedorTrabajo.Articulo.Add(articuloVm.Articulo);
                    //_contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            List<Categoria> categorias = await this.serviceCategorias.GetCategoriasAsync();
            var select = categorias.Select(i => new SelectListItem()
            {
                Text = i.Nombre,
                Value = i.Id.ToString()
            });
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new Articulo(),
                ListaCategorias = select

            };
            if (id != null)
            {
                articuloVM.Articulo = await this.serviceArticulo.FindArticulo(id.GetValueOrDefault()); //_contenedorTrabajo.Articulo.get(id.GetValueOrDefault());
            }
            return View(articuloVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticuloVM articuloVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var articuloDb = await this.serviceArticulo.FindArticulo(articuloVM.Articulo.Id); //_contenedorTrabajo.Articulo.get(articuloVM.Articulo.Id);

                if (archivos.Count() > 0)
                {
                    //Editamos imagen
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDb.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }
                    //SUbimos nuevo

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + nuevaExtension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    articuloVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + nuevaExtension;
                    articuloVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Articulo.Update(articuloVM.Articulo);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Aqui es cuando la imagen ya existe y no se remplaza ,
                    //se debe conservar la que ya esta en la BD
                    articuloVM.Articulo.UrlImagen = articuloDb.UrlImagen;
                    articuloVM.Articulo.FechaCreacion = articuloDb.FechaCreacion;
                }
                await this.serviceArticulo.Update(articuloVM.Articulo.Id, articuloVM.Articulo.Nombre, articuloVM.Articulo.FechaCreacion, articuloVM.Articulo.UrlImagen,
                         articuloVM.Articulo.Descripcion, articuloVM.Articulo.CategoriaId, articuloVM.Articulo.Categoria);
                //_contenedorTrabajo.Articulo.Update(articuloVM.Articulo);
                //_contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var articuloDesdeDb = /*await this.serviceArticulo.FindArticulo(id); */_contenedorTrabajo.Articulo.get(id);
            string rutaDirectorioPrincipal = _hostEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }
            if (articuloDesdeDb == null)
            {
                return Json(new { success = false, message = "Error Borrando articulo" });
            }
            _contenedorTrabajo.Articulo.Remove(articuloDesdeDb);
            _contenedorTrabajo.Save();
            //await this.serviceArticulo.Delete(articuloDesdeDb.Id);
            return Json(new { success = true, message = "Artículo borrado!" });

        }

        #region LLamadas a la API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data1 = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria");
            var data2 = await this.serviceArticulo.GetArticulosAsync();
            //return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
            return Json(new { data = await serviceArticulo.GetArticulosAsync() });

        }
        #endregion
    }
}