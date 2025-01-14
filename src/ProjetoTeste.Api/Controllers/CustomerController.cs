﻿using Microsoft.AspNetCore.Mvc;
using ProjetoTeste.Arguments.Arguments;
using ProjetoTeste.Arguments.Arguments.Base;
using ProjetoTeste.Arguments.Arguments.Customer;
using ProjetoTeste.Infrastructure.Interface.Service;
using ProjetoTeste.Infrastructure.Interface.UnitOfWork;

namespace ProjetoTeste.Api.Controllers;

public class CustomerController : BaseController
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService, IUnitOfWork _unitOfWork) : base(_unitOfWork)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<OutputCustomer>>> GetAll()
    {
        var get = await _customerService.GetAll();
        return Ok(get);
    }

    [HttpGet("Id")]
    public async Task<ActionResult> Get(long id)
    {
        var get = await _customerService.Get(id);
        return Ok(get);
    }

    [HttpGet("Id/Multiple")]
    public async Task<ActionResult> GetListByListId(List<long> idList)
    {
        var get = await _customerService.GetListByListId(idList);
        return Ok(get);
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<List<OutputCustomer>>>> Create(InputCreateCustomer inputCreateCustomer)
    {
        var createClient = await _customerService.Create(inputCreateCustomer);
        if (!createClient.Success)
        {
            return BadRequest(createClient);
        }
        return Ok(createClient);
    }

    [HttpPost("Multiple")]
    public async Task<ActionResult<BaseResponse<List<OutputCustomer>>>> Create(List<InputCreateCustomer> listInputCreateCustomer)
    {
        var createClient = await _customerService.CreateMultiple(listInputCreateCustomer);
        if (!createClient.Success)
        {
            return BadRequest(createClient);
        }
        return Ok(createClient);
    }

    [HttpPut]
    public async Task<ActionResult> Update(InputIdentityUpdateCustomer inputIdentityUpdateCustomer)
    {
        BaseResponse<bool> update = await _customerService.Update(inputIdentityUpdateCustomer);
        if (!update.Success)
        {
            return BadRequest(update);
        }
        return Ok(update);
    }

    [HttpPut("Multiple")]
    public async Task<ActionResult> Update( List<InputIdentityUpdateCustomer> listInputIdentityUpdateCustomer)
    {
        BaseResponse<bool> update = await _customerService.UpdateMultiple(listInputIdentityUpdateCustomer);
        if (!update.Success)
        {
            return BadRequest(update);
        }
        return Ok(update);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(long id)
    {
        BaseResponse<bool> delete = await _customerService.Delete(id);
        if (!delete.Success)
        {
            return BadRequest(delete);
        }
        return Ok(delete);
    }

    [HttpDelete("Multiple")]
    public async Task<ActionResult> Delete(List<long> listId)
    {
        BaseResponse<bool> delete = await _customerService.DeleteMultiple(listId);
        if (!delete.Success)
        {
            return BadRequest(delete);
        }
        return Ok(delete);
    }
}