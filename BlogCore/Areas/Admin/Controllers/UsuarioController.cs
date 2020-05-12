using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsuarioController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public UsuarioController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_contenedorTrabajo.Usuario.GetAll(i => i.Id != usuarioActual.Value));
        }

        public IActionResult Bloquear(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            _contenedorTrabajo.Usuario.BloquearUsuario(Id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Desbloquear(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            _contenedorTrabajo.Usuario.DesbloquearUsuario(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}