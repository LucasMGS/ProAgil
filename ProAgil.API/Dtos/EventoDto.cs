using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.API.Dtos
{
    public class EventoDto
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage="Campo obrigatório")]
        [StringLength (100,MinimumLength = 3, ErrorMessage="Local precisa ter entre 3 e 100 caracteres")]
        public string Local { get; set; }
        public string Data { get; set; }
        public string Tema { get; set; }
        public int QntdPessoas { get; set; }
        public string ImagemURL { get; set; }
        [Required]
        public string Telefone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public List<LoteDto> Lotes { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<PalestranteDto> Palestrantes { get; set; }
    }
}