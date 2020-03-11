using System.ComponentModel.DataAnnotations;

namespace ProAgil.API.Dtos
{
    public class LoteDto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Nome { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public string DataInicio { get; set; }

        [Required]
        public string DataFim { get; set; }

        [Required]
        [Range (5,120000, ErrorMessage="A quantidade deve ser entre 5 e 120.000")]
        public int Quantidade { get; set; }
    }
}