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
                                          .ToListAsync() as IEnumerable<TEntity> :
                    await _context.Tickets.Include(T => T.Attachments)
                                          .Include(T => T.Comments)
                                          .AsNoTracking()
                                          .ToListAsync() as IEnumerable<TEntity>;
            }
            return trackChanges ?
                await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        public Task<TEntity> GetAsync(TKey id)
        {
            throw new NotImplementedException();
        }
        public Task AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
