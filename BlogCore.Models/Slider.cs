using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlogCore.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="El estado es obligatorio")]
        public bool Estado { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name ="Nombre de la imagen")]
        public string UrlImagen { get; set; }
    }
}
