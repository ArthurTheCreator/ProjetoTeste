﻿using ProjetoTeste.Arguments.Arguments;
using ProjetoTeste.Arguments.Arguments.Base;
using ProjetoTeste.Arguments.Arguments.Order;
using ProjetoTeste.Arguments.Arguments.ProductOrder;
using ProjetoTeste.Infrastructure.Application.Service.Order;
using ProjetoTeste.Infrastructure.Conversor;
using ProjetoTeste.Infrastructure.Interface.Repositories;
using ProjetoTeste.Infrastructure.Interface.Service;
using ProjetoTeste.Infrastructure.Persistence.Entity;

namespace ProjetoTeste.Infrastructure.Application;

public class OrderService : IOrderService
{
    #region Dependency Injection
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductOrderRepository _productOrderRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly OrderValidateService _orderValidateService;

    public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IProductRepository productRepository, IProductOrderRepository productOrderRepository, IBrandRepository brandRepository, OrderValidateService orderValidateService)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _productOrderRepository = productOrderRepository;
        _brandRepository = brandRepository;
        _orderValidateService = orderValidateService;
    }
    #endregion

    #region Get
    public async Task<BaseResponse<List<OutputOrder>>> GetAll()
    {
        var listOrder = await _orderRepository.GetProductOrders();
        var outputOrder = listOrder.Select(o => new OutputOrder(o.Id, o.CustomerId, (from i in o.ListProductOrder select i.ToOuputProductOrder()).ToList(), o.Total, o.OrderDate)).ToList();
        return new BaseResponse<List<OutputOrder>> { Success = true, Content = outputOrder };
    }

    public async Task<BaseResponse<List<OutputOrder>>> Get(InputIdentifyViewOrder inputIdentifyViewOrder)
    {
        var order = await _orderRepository.GetProductOrdersId(inputIdentifyViewOrder.Id);
        return new BaseResponse<List<OutputOrder>> { Success = true, Content = order.Select(o => new OutputOrder(o.Id, o.CustomerId, (from i in o.ListProductOrder select i.ToOuputProductOrder()).ToList(), o.Total, o.OrderDate)).ToList() };
    }

    public async Task<BaseResponse<List<OutputOrder>>> GetListByListId(List<InputIdentifyViewOrder> listInputIdentifyViewOrder)
    {
        var listOrder = await _orderRepository.GetProductOrdersByListId(listInputIdentifyViewOrder.Select(i => i.Id).ToList());
        return new BaseResponse<List<OutputOrder>> { Success = true, Content = listOrder.Select(i => i.ToOutputOrder()).ToList() };
    }
    #endregion

    #region "Relatorio"
    public async Task<OutputMaxSaleValueProduct> BestSellerProduct()
    {
        return await _orderRepository.BestSellerProduct();
    }

    public async Task<OutputMaxSaleValueProduct> LeastSoldProduct()
    {
        return await _orderRepository.LeastSoldProduct();
    }

    public async Task<List<OutputMaxSaleValueProduct>> GetMostOrderedProduct()
    {
        return await _orderRepository.GetMostOrderedProduct();
    }

    public async Task<OutputCustomerOrder> BiggestBuyer()
    {
        var buyer = await _orderRepository.BiggestBuyer();
        var customer = await _customerRepository.Get(buyer.CustomerId);
        return new OutputCustomerOrder(buyer.CustomerId, customer.Name, buyer.TotalOrders, buyer.QuantityPurchased, buyer.TotalPrice);
    }

    public async Task<OutputBrandBestSeller> BrandBestSeller()
    {
        var bestSeller = await _orderRepository.BrandBestSeller();
        var brand = await _brandRepository.Get(bestSeller.Id);
        return new OutputBrandBestSeller(brand.Id, brand.Name, brand.Code, brand.Description, bestSeller.TotalSell, bestSeller.TotalPrice);
    }

    public async Task<HighestAverageSalesValue> HighestAverageSalesValue()
    {
        return await _orderRepository.HighestAverageSalesValue();
    }

    public async Task<string> Total()
    {
        var total = await _orderRepository.Total();
        return $"O Total vendido: R$: {total}";
    }
    #endregion

    #region Create Order
    public async Task<BaseResponse<OutputOrder>> Create(InputCreateOrder inputCreateOrder)
    {
        var response = new BaseResponse<OutputOrder>();
        var createValidate = await CreateMultiple([inputCreateOrder]);
        response.Success = createValidate.Success;
        response.Message = createValidate.Message;
        if (!response.Success)
            return response;
        response.Content = createValidate.Content.FirstOrDefault();
        return response;
    }

    public async Task<BaseResponse<List<OutputOrder>>> CreateMultiple(List<InputCreateOrder> listinputCreateOrder)
    {
        var response = new BaseResponse<List<OutputOrder>>();
        var listCustomerId = (await _customerRepository.GetListByListId(listinputCreateOrder.Select(i => i.CustomerId).ToList())).Select(j => j.Id).ToList();
        var listCreate = (from i in listinputCreateOrder
                          select new
                          {
                              InputCreateOrder = i,
                              CustomerId = listCustomerId.FirstOrDefault(j => j == i.CustomerId)
                          });
        List<OrderValidate> listOrderValidate = listCreate.Select(i => new OrderValidate().CreateValidate(i.InputCreateOrder, i.CustomerId)).ToList();
        var create = await _orderValidateService.CreateValidateOrder(listOrderValidate);
        response.Success = create.Success;
        response.Message = create.Message;
        if (!response.Success)
            return response;

        var listCreateOrder = (from i in create.Content
                               select new Order(i.InputCreateOrder.CustomerId, DateTime.Now, default)).ToList();

        var listNewOrder = await _orderRepository.Create(listCreateOrder);
        response.Content = listNewOrder.Select(i => i.ToOutputOrder()).ToList();
        return response;
    }
    #endregion

    #region Create ProductOrder
    public async Task<BaseResponse<OutputProductOrder>> CreateProductOrder(InputCreateProductOrder inputCreateProductOrder)
    {
        var response = new BaseResponse<OutputProductOrder>();
        var createValidate = await CreateProductOrderMultiple([inputCreateProductOrder]);
        response.Success = createValidate.Success;
        response.Message = createValidate.Message;
        if (!response.Success)
            return response;
        response.Content = createValidate.Content.FirstOrDefault();
        return response;
    }

    public async Task<BaseResponse<List<OutputProductOrder>>> CreateProductOrderMultiple(List<InputCreateProductOrder> listinputCreateProductOrder)
    {
        var response = new BaseResponse<List<OutputProductOrder>>();
        var listOrder = await _orderRepository.GetListByListId(listinputCreateProductOrder.Select(i => i.OrderId).ToList());
        var listProduct = await _productRepository.GetListByListId(listinputCreateProductOrder.Select(i => i.ProductId).ToList());
        var listCreate = (from i in listinputCreateProductOrder
                          select new
                          {
                              InputCreateProductOrder = i,
                              OrderId = listOrder.Select(j => j.Id).FirstOrDefault(j => j == i.OrderId),
                              Product = listProduct.FirstOrDefault(k => k.Id == i.ProductId).ToProductDTO(),
                          });
        List<ProductOrderValidate> listProductOrderValidate = listCreate.Select(i => new ProductOrderValidate().CreateValidate(i.InputCreateProductOrder, i.OrderId, i.Product)).ToList();
        var create = await _orderValidateService.CreateValidateProductOrder(listProductOrderValidate);
        response.Success = create.Success;
        response.Message = create.Message;
        if (!response.Success)
            return response;

        var createValidate = (from i in create.Content
                              select new ProductOrder(i.OrderId, i.Product.Id, i.InputCreateProductOrder.Quantity, i.Product.Price, (i.Product.Price * i.InputCreateProductOrder.Quantity))).ToList();
        var listNewProductOrder = await _productOrderRepository.Create(createValidate);

        var listUpdateProduct = await _productRepository.GetListByListId(create.Content.Select(i => i.Product.Id).ToList());
        var listUpdateOrder = await _orderRepository.GetListByListId(create.Content.Select(i => i.OrderId).ToList());
        for (var i = 0; i < listUpdateProduct.Count; i++)
        {
            listUpdateProduct[i].Stock = create.Content[i].Product.Stock;
        }

        var totalOrder = (from i in listUpdateOrder
                          from j in createValidate
                          where i.Id == j.OrderId
                          let total = i.Total += j.SubTotal
                          select i).ToList();

        var updateProduct = await _productRepository.Update(listUpdateProduct);
        var updateOrder = await _orderRepository.Update(totalOrder);
        if (!updateProduct || !updateOrder)
        {
            response.AddErrorMessage("Não foi possivél efetuar o pedido");
            return response;
        }

        response.Content = listNewProductOrder.Select(i => i.ToOuputProductOrder()).ToList();
        return response;
    }
    #endregion

}