﻿using ProjetoTeste.Infrastructure.Persistence.Entity;

namespace ProjetoTeste.Infrastructure.Interface.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<bool> Exist(string code);
}
