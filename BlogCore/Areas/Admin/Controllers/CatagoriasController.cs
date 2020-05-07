﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {
            this._contenedorTrabajo = contenedorTrabajo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region LLamadas a la API
       [HttpGet]
       public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Categoria.GetAll() });

        }
        #endregion
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}