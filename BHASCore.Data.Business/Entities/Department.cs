using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BHASCore.Data.Business.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }// primary key - dodali [Key] atribut da bi označili da je ovo primarni ključ

        [Required(ErrorMessage ="Naziv odjela je obavezan")]
        [Display(Name = "Naziv odjela")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Šifra odjela je obavezan")]
        [Display(Name = "Šifra odjela")]
        public string DepartmentCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Budžet")]
        public decimal? Budget { get; set; }

        [Display(Name = "Datum kreiranja")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Datum izmjene")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Navigacijska svojstva koja omogućava pristup povezanim podacima o zaposlenima (Employee) koji pripadaju ovom odeljenju
        /// </summary>
        public virtual ICollection<Employee> Employees { get; set; } 
    }
}
