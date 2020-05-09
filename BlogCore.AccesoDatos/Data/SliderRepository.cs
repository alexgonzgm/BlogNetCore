using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogCore.AccesoDatos.Data
{
    class SliderRepository : Repository<Slider>, ISliderRepository
    {
        protected readonly ApplicationDbContext _db;
        public SliderRepository(ApplicationDbContext db):base(db)
        {
            this._db = db;
        }

        public void Update(Slider slider)
        {
            var sliderDb = _db.Slider.FirstOrDefault(i => i.Id == slider.Id);
            sliderDb.Nombre = slider.Nombre;
            sliderDb.Estado = slider.Estado;
            sliderDb.UrlImagen = slider.UrlImagen;
        }
    }
}
