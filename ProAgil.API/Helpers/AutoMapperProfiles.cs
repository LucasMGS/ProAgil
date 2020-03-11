using System.Linq;
using AutoMapper;
using ProAgil.API.Dtos;
using ProAgil.Domain;

namespace ProAgil.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoDto>().ForMember
            (
                dto => dto.Palestrantes, 
                opt => 
                {
                    opt.MapFrom(dominio => dominio.PalestranteEventos.Select(pe => pe.Palestrante).ToList());
                }).ReverseMap();
                
            CreateMap<Palestrante, PalestranteDto>().ForMember
            (
                dto => dto.Eventos,
                opt => 
                {
                    opt.MapFrom(domain => domain.PalestranteEventos.Select(pe => pe.Evento).ToList());
                }).ReverseMap();
                
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
        }
    }
}