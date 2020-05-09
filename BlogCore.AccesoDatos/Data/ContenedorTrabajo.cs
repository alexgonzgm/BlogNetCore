using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.AccesoDatos.Data
{
    public class ContenedorTrabajo : IContenedorTrabajo
    { 
        public ICategoriaRepository Categoria { get; private set; }
        public IArticuloRepository Articulo { get; private set; }
        public ISliderRepository Slider { get; private set; }

        private ApplicationDbContext _db;
        public ContenedorTrabajo(ApplicationDbContext db) 
        {
            _db = db;
            Categoria = new CategoriaRepository(_db);
            Articulo = new ArticuloRepository(_db);
            Slider = new SliderRepository(_db);
        }
       

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
