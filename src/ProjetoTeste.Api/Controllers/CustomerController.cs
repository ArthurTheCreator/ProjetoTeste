﻿using ProjetoTeste.Api.Controllers.Base;
using ProjetoTeste.Arguments.Arguments;
using ProjetoTeste.Arguments.Arguments.Customer;
using ProjetoTeste.Domain.DTO;
using ProjetoTeste.Domain.Interface.Service;
using ProjetoTeste.Infrastructure.Interface.UnitOfWork;
using ProjetoTeste.Infrastructure.Persistence.Entity;

namespace ProjetoTeste.Api.Controllers;

public class CustomerController(ICustomerService customerService, IUnitOfWork unitOfWork) : BaseController<ICustomerService, CustomerDTO, Customer, InputCreateCustomer, InputUpdateCustomer, InputIdentityUpdateCustomer, InputIdentityDeleteCustomer, InputIdentifyViewCustomer, OutputCustomer>(unitOfWork, customerService) { }
