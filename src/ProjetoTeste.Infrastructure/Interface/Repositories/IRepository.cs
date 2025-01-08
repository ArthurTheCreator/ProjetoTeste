﻿using ProjetoTeste.Infrastructure.Persistence.Entity.Base;

namespace ProjetoTeste.Infrastructure.Interface.Repositories;
//Trocar os create update e delete pra list
public interface IRepository<TEntity> where TEntity : BaseEntity, new()
{
    Task<List<TEntity?>> GetAllAsync();
    List<TEntity?> GetAll();
    Task<List<TEntity>> Get(List<long> ids);
    Task<List<TEntity>?> Create(List<TEntity> entityList);
    Task<bool> Update(List<TEntity> entityList);
    Task<bool> Delete(List<TEntity> entityList);
}