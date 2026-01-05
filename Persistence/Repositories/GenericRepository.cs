using Domain.Contracts;
using Domain.Models.Common;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private MaintenanceTicketSysDbContext _context;

        public GenericRepository(MaintenanceTicketSysDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false)
        {
            if (typeof(TEntity) == typeof(Ticket))
            {
                return trackChanges ?
                    await _context.Tickets.Include(T => T.Attachments)
                                          .Include(T => T.Comments)
                                          .ToListAsync() as IEnumerable<TEntity>
                                          :await _context.Tickets.Include(T => T.Attachments)
                                          .Include(T => T.Comments)
                                          .AsNoTracking()
                                          .ToListAsync() as IEnumerable<TEntity>;
            }
            return trackChanges ?
                await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        public async Task<TEntity> GetAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Ticket))
            {
                return 
                    await _context.Tickets.Include(T=>T.Attachments)
                                          .Include(T=>T.Comments)
                                          .FirstOrDefaultAsync(T => T.Id.Equals(id)) as TEntity;
            }
            return await _context.Set<TEntity>().FindAsync(id);

        }
        async Task IGenericRepository<TEntity, TKey>.AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        void IGenericRepository<TEntity, TKey>.Update(TEntity entity)
        {
            _context.Update(entity);
        }

        void IGenericRepository<TEntity, TKey>.Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

    }
}
