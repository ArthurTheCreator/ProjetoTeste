﻿using Microsoft.EntityFrameworkCore;
using ProjetoTeste.Infrastructure.Interface.Repositories;
using ProjetoTeste.Infrastructure.Persistence.Context;
using ProjetoTeste.Infrastructure.Persistence.Entity.Base;

namespace ProjetoTeste.Infrastructure.Persistence.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity?>();
        }
        public async Task<List<TEntity?>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<TEntity>> GetListByListId(List<long> listId)
        {
            return await _dbSet.Where(i => listId.Contains(i.Id)).ToListAsync();
        }

        public async Task<TEntity?> Get(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        //public async Task<TEntity>? Create(TEntity entity)
        //{
        //    await _dbSet.AddAsync(entity);
        //    await _context.SaveChangesAsync();
        //    return entity;
        //}

        public async Task<List<TEntity>?> Create(List<TEntity>? entityList)
        {
            await _dbSet.AddRangeAsync(entityList);
            await _context.SaveChangesAsync();
            return entityList;
        }

        //public async Task<bool> Update(TEntity entity)
        //{
        //    _dbSet.Update(entity);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> Update(List<TEntity>? entityList)
        {
            _dbSet.UpdateRange(entityList);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> Delete(TEntity entity)
        //{
        //     _dbSet.Remove(entity);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
        public async Task<bool> Delete(List<TEntity> entityList)
        {
            _dbSet.RemoveRange(entityList);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}