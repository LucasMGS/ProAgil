namespace ProAgil.Domain
{
    public class PalestranteEvento
    {
        public int PalestranteId { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; }
        public Palestrante Palestrante { get; }
    }
}