using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
         private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;
        public SliderController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostEnvironment)
        {
            this._contenedorTrabajo = contenedorTrabajo;
            this._hostEnvironment = hostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(); 
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {

            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (slider.Id == 0)
                {
                    //nuevo Articulo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;
                   

                    _contenedorTrabajo.Slider.Add(slider);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var slider = _contenedorTrabajo.Slider.get(id.GetValueOrDefault());
                return View(slider);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                var rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivo = HttpContext.Request.Form.Files;

                var sliderDesdeDb = _contenedorTrabajo.Slider.get(slider.Id);

                if (archivo.Count()>0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var RutaSubidas = Path.Combine(rutaPrincipal, @"imagenes/sliders");
                    var extension = Path.GetExtension(archivo[0].FileName);

                    var rutaimagenOld = Path.Combine(RutaSubidas, sliderDesdeDb.UrlImagen.TrimEnd('\\'));
                    if (System.IO.File.Exists(rutaimagenOld))
                    {
                        System.IO.File.Delete(rutaimagenOld);
                    }

                    using (var fileS = new FileStream(Path.Combine(RutaSubidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivo[0].CopyTo(fileS);
                    }
                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;

                    _contenedorTrabajo.Slider.Update(slider);
                    _contenedorTrabajo.Save();
                }
                else
                {
                    slider.UrlImagen = sliderDesdeDb.UrlImagen;
                }
                _contenedorTrabajo.Slider.Update(slider);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objDb = _contenedorTrabajo.Slider.get(id);
            if (objDb == null)
            {
                return Json(new { message = false, messagge = "Error borrando slider" });
            }
            _contenedorTrabajo.Slider.Remove(objDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Slider Borrado" });
        }


        #region LLamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Slider.GetAll() });

        }
        #endregion
    }
}