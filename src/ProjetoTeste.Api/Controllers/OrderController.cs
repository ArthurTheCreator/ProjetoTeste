﻿using Microsoft.AspNetCore.Mvc;
using ProjetoTeste.Api.Controllers.Base;
using ProjetoTeste.Arguments.Arguments;
using ProjetoTeste.Arguments.Arguments.Base;
using ProjetoTeste.Arguments.Arguments.Base.ApiResponse;
using ProjetoTeste.Arguments.Arguments.Base.Crud;
using ProjetoTeste.Arguments.Arguments.Order;
using ProjetoTeste.Arguments.Arguments.ProductOrder;
using ProjetoTeste.Domain.DTO;
using ProjetoTeste.Domain.Interface.Service;
using ProjetoTeste.Domain.Interface.Service.Module.ProductOrder;
using ProjetoTeste.Infrastructure.Interface.UnitOfWork;
using ProjetoTeste.Infrastructure.Persistence.Entity;

namespace ProjetoTeste.Api.Controllers;

public class OrderController : BaseController<IOrderService, OrderDTO, Order, InputCreateOrder, BaseInputUpdate_0, BaseInputIdentityUpdate_0, BaseInputIdentityDelete_0, InputIdentifyViewOrder, OutputOrder>
{
    private readonly IOrderService _orderService;
    private readonly IProductOrderService _productOrderService;

    public OrderController(IOrderService orderService, IUnitOfWork unitOfWork, IProductOrderService productOrderService) : base(unitOfWork, orderService)
    {
        _orderService = orderService;
        _productOrderService = productOrderService;
    }

    #region Get
    [HttpPost("GetById")]
    public async Task<ActionResult<List<OutputOrder>>> GetByIdWithProduct(InputIdentifyViewOrder inputIdentifyViewOrder)
    {
        return await _orderService.GetByIdWithProducts(inputIdentifyViewOrder);
    }
    #endregion

    #region Relatorio
    [HttpGet("BestSellerProduct")]
    public async Task<ActionResult<OutputMaxSaleValueProduct>> GetBestSeller()
    {
        var order = await _orderService.BestSellerProduct();
        return Ok(order);
    }

    [HttpGet("LesatSoldProduct")]
    public async Task<ActionResult<OutputMaxSaleValueProduct>> LesatSoldProduct()
    {
        var order = await _orderService.LeastSoldProduct();
        return Ok(order);
    }

    [HttpGet("GetMostOrderedProduct")]
    public async Task<ActionResult<OutputMaxSaleValueProduct>> GetBestSellers()
    {
        var order = await _orderService.GetMostOrderedProduct();
        return Ok(order);
    }

    [HttpGet("BiggestBuyer")]
    public async Task<ActionResult<OutputCustomerOrder>> BiggestBuyer()
    {
        var client = await _orderService.BiggestBuyer();
        return Ok(client);
    }

    [HttpGet("BrandBestSeller")]
    public async Task<ActionResult<OutputBrandBestSeller>> Brand()
    {
        var brand = await _orderService.BrandBestSeller();
        return Ok(brand);
    }

    [HttpGet("HighestAverageSalesValue")]
    public async Task<ActionResult> HighestAverageSalesValue()
    {
        var avarage = await _orderService.HighestAverageSalesValue();
        return Ok(avarage);
    }

    [HttpGet("Total")]
    public async Task<ActionResult> Total()
    {
        var total = await _orderService.Total();
        return Ok(total);
    }
    #endregion

    #region Create ProductOrder
    [HttpPost("Create/ProductOrder")]
    public async Task<ActionResult<BaseResult<OutputOrder>>> CreateProductOrder(InputCreateProductOrder inputCreateProductOrder)
    {
        var create = await _productOrderService.Create(inputCreateProductOrder);
        if (!create.IsSuccess)
        {
            return BadRequest(create);
        }
        return Ok(create);
    }

    [HttpPost("Create/ProductOrder/Multiple")]
    public async Task<ActionResult<BaseResult<List<OutputOrder>>>> CreateProductOrder(List<InputCreateProductOrder> listInputCreateProductOrder)
    {
        var createMultiple = await _productOrderService.CreateMultiple(listInputCreateProductOrder);
        if (!createMultiple.IsSuccess)
        {
            return BadRequest(createMultiple);
        }
        return Ok(createMultiple);
    }
    #endregion

    #region Ignore

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("Id")]
    public override async Task<ActionResult<OutputOrder>> Get(InputIdentifyViewOrder inputIdentifyView)
    {
        throw new NotImplementedException();
    }

    #region Update
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPut("Update")]
    public override async Task<ActionResult<BaseResult<bool>>> Update(BaseInputIdentityUpdate_0 inputIdentityUpdateDTO)
    {
        throw new NotImplementedException();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPut("Update/Multiple")]
    public override async Task<ActionResult<BaseResult<bool>>> Update(List<BaseInputIdentityUpdate_0> listTInputIndetityUpdate)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Delete
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("Delete")]
    public override async Task<ActionResult<BaseResponse<bool>>> Delete(BaseInputIdentityDelete_0 inputIdentifyDeleteDTO)
    {
        throw new NotImplementedException();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpDelete("Delete/Multiple")]
    public override async Task<ActionResult<BaseResponse<bool>>> Delete(List<BaseInputIdentityDelete_0> listInputIdentifyDeleteDTO)
    {
        throw new NotImplementedException();
    }
    #endregion
    #endregion
}