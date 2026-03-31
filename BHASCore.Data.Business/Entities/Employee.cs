using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BHASCore.Data.Business.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Display(Name = "Ime")]
        [StringLength(20, ErrorMessage ="Ime ne moze biti duze od 20 karaktera")]
        public string FirstName { get; set; }

        [Display(Name = "Prezime")]
        [StringLength(30, ErrorMessage = "Prezime ne moze biti duze od 30 karaktera")]
        public string LastName { get; set; }

        [Display(Name = "JMBG")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG mora imati tacno 13 karaktera")]
        public string? JMBG { get; set; }

        [Display(Name = "Plata")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Salary { get; set; }

        [Display(Name = "Pozicija")]
        [StringLength(50, ErrorMessage = "Pozicija ne moze biti duza od 50 karaktera")]
        public string Position { get; set; }

        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "Email adresa ne moze biti duza od 100 karaktera")]
        [EmailAddress(ErrorMessage = "Neispravan format email adrese")]
        public string? Email { get; set; }

        [Display(Name = "Datum zaposlenja")]
        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        [Display(Name = "Datum kreiranja")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Datum izmjene")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// FK-Strani ključ koji povezuje zaposlenog sa odeljenjem (Department) 
        /// </summary>
        [Required(ErrorMessage = "Odeljenje je obavezno")]
        [Display(Name = "Odeljenje")]
        public int DepartmentID { get; set; } // Foreign key

        /// <summary>
        /// Navigacijska svojstva koja omogućava pristup povezanim podacima o odeljenju (Department) kojem zaposleni pripada.
        /// </summary>
        [ForeignKey("DepartmentID")]
        public virtual Department Department { get; set; } // Navigation property
    }
}
