using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogCore.AccesoDatos.Data
{
    public class UsuarioRepository : Repository<ApplicationUser>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;
        public UsuarioRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void BloquearUsuario(string IdUsuario)
        {
            var usuarioDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == IdUsuario);
            usuarioDb.LockoutEnd = DateTime.Now.AddDays(10);
            _db.SaveChanges();
        }

        public void DesbloquearUsuario(string IdUsuario)
        {
            var usuarioDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == IdUsuario);
            usuarioDb.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }
    }
}
