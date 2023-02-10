using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres.")]
        [Display(Name = "País")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = string.Empty;
    }
}
