namespace ProjetoVectra.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pessoa")]
    public partial class Pessoa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "CPF deve ser preenchido!")]
        [StringLength(11, ErrorMessage = "CPF deve conter 11 dígitos!", MinimumLength = 11)]
        [Display(Name = "CPF")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "O CPF deve conter apenas números")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Nome deve ser preenchido!")]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\u00C0-\u017F´]+\s+[a-zA-Z\u00C0-\u017F´]{0,}$", ErrorMessage = "O campo deve conter nome e sobrenome (composto por letras)")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Idade deve ser preenchida!")]
        public int Idade { get; set; }

        [Display(Name = "Peso")]
        [Required(ErrorMessage = "Peso deve ser preenchido!")]
         public double Peso { get; set; }

        [Display(Name = "Altura (cm)")]
        [Required(ErrorMessage = "Altura deve ser preenchida!")]
        public double Altura { get; set; }

        [Display(Name = "IMC")]
        public double Imc { get; set; }
    }
}
