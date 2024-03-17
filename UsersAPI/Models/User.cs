using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El primer nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El primer nombre no puede tener más de 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El primer nombre no puede contener números")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "El segundo nombre no puede tener más de 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El segundo nombre no puede contener números")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [StringLength(50, ErrorMessage = "El primer apellido no puede tener más de 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El primer apellido no puede contener números")]
        public string FirstLastName { get; set; }

        [StringLength(50, ErrorMessage = "El segundo apellido no puede tener más de 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El segundo apellido no puede contener números")]
        public string SecondLastName { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "El sueldo es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El sueldo debe ser mayor que cero")]
        public double Salary { get; set; }
    }
}
