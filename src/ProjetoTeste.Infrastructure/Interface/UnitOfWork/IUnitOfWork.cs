﻿namespace ProjetoTeste.Infrastructure.Interface.UnitOfWork;

public interface IUnitOfWork
{
    Task Commit();
}