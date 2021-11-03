using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Check02.Models
{
    public class MdDono
    {
        [Key]
        public int IdDono { get; set; }

        // ########## NOME ##########
        [Display(Name ="Nome do Dono")]
        [Required]
        public String NmDono { get; set; }



        // ########## TELEFONE ##########
        [Required]
        public int Telefone { get; set; }


        // ########## DATA DE NASCIMENTO ##########
        [Display(Name ="Data de nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormatAttribute(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string Nascimento { get; set; }
    }
}