using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogCore.Models;
using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models.ViewModels;

namespace BlogCore.Controllers
{   [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            this._contenedorTrabajo = contenedorTrabajo;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Sliders = _contenedorTrabajo.Slider.GetAll(),
                Articulos = _contenedorTrabajo.Articulo.GetAll()
            };
            return View(homeVM);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var articuloDb = _contenedorTrabajo.Articulo.GetFirstOrDefault(a=>a.Id==id);
            return View(articuloDb);
        }

    }
}
