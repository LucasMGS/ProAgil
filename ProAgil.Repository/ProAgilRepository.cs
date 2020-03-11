using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        public ProAgilContext Context { get; set; }
        public ProAgilRepository(ProAgilContext context)
        {
            this.Context = context;
            this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GERAL
        public void Add<T>(T Entity) where T : class
        {
            Context.Add(Entity);
        }

        public void Delete<T>(T Entity) where T : class
        {
            Context.Remove(Entity);
        }
        public void Update<T>(T Entity) where T : class
        {
            Context.Update(Entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        // EVENTO
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = Context.Eventos
                                       .Include(c => c.Lotes)
                                       .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(evento => evento.PalestranteEventos)
                            .ThenInclude(pe => pe.Palestrante);
            }

            return await query.OrderBy(evento => evento.Id).ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = Context.Eventos
                                    .Include(evento => evento.Lotes)
                                    .Include(evento => evento.RedesSociais)
                                    .Where(evento => evento.Tema.ToLower().Contains(tema.ToLower()));

            if (includePalestrantes)
            {
                query = query.Include(c => c.PalestranteEventos)
                            .ThenInclude(c => c.Palestrante);
            }

            return await query.OrderBy(evento => evento.Id).ToArrayAsync();
        }

        public async Task<Evento> GetEventoAsyncById(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = Context.Eventos
                                    .Include(evento => evento.Lotes)
                                    .Include(evento => evento.RedesSociais)
                                    .Where(evento => evento.Id == eventoId);

            if (includePalestrantes)
            {
                query = query.Include(c => c.PalestranteEventos)
                            .ThenInclude(c => c.Palestrante);
            }

            return await query.OrderBy(evento => evento.Id).FirstOrDefaultAsync();
        }

        public async Task<Palestrante> GetPalestranteAsync(int palestranteId, bool includeEvento = false)
        {
            IQueryable<Palestrante> query = Context.Palestrante
                                    .Include(palestrante => palestrante.RedesSociais)
                                    .Where(palestrante => palestrante.Id == palestranteId);

            if (includeEvento)
            {
                query = query.Include(c => c.PalestranteEventos)
                            .ThenInclude(c => c.Evento);
            }

            return await query.OrderBy(palestrante => palestrante.Nome).FirstOrDefaultAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string nome, bool includeEvento = false)
        {
            IQueryable<Palestrante> query = Context.Palestrante
                                    .Include(palestrante => palestrante.RedesSociais)
                                    .Where(palestrante => palestrante.Nome.ToLower().Contains(nome.ToLower()));

            if (includeEvento)
            {
                query = query.Include(c => c.PalestranteEventos)
                            .ThenInclude(c => c.Evento);
            }

            return await query.OrderBy(palestrante => palestrante.Nome).ToArrayAsync();
        }
    }
}