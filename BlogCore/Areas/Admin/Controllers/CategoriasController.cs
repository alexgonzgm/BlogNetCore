using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using BlogCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private ServiceCategorias serviceCategorias;

        public CategoriasController(IContenedorTrabajo contenedorTrabajo, ServiceCategorias serviceCategorias)
        {
            this._contenedorTrabajo = contenedorTrabajo;
            this.serviceCategorias = serviceCategorias;
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
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                //_contenedorTrabajo.Categoria.Add(categoria);
                await this.serviceCategorias.Add(categoria.Nombre, categoria.Orden);


                //_contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Categoria categoria = new Categoria();
            categoria = await this.serviceCategorias.Categoria(id);//_contenedorTrabajo.Categoria.get(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                //_contenedorTrabajo.Categoria.Update(categoria);
                //_contenedorTrabajo.Save();
                await this.serviceCategorias.Update(categoria.Id, categoria.Nombre, categoria.Orden);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }



        #region LLamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Categoria.GetAll() });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = /*await this.serviceCategorias.Categoria(id);*/_contenedorTrabajo.Categoria.get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando la categoría" });
            }
            _contenedorTrabajo.Categoria.Remove(objFromDb);
            _contenedorTrabajo.Save();
            //await this.serviceCategorias.Delete(objFromDb.Id);
            return Json(new { success = true, message = "Categoria borrada correctammente!" });
        }

        #endregion
    }
}