﻿using ProjetoTeste.Arguments.Arguments.Order;
using ProjetoTeste.Arguments.Arguments.ProductsOrder;
using ProjetoTeste.Infrastructure.Persistence.Entity;

namespace ProjetoTeste.Infrastructure.Interface.Service;

public interface IOrderService
{
    Task<BaseResponse<List<OutputOrder>>> GetAll();
    Task<BaseResponse<List<OutputOrder>>> Get(long id);
    Task<BaseResponse<OutputOrder>> Delete(long id);
    Task<BaseResponse<OutputOrder>> Create(InputCreateOrder input);
    Task<BaseResponse<OutputProductOrder>> CreateProductOrder(InputCreateProductOrder input);
    Task<BaseResponse<OutputSellProduct>> BestSellerProduct();
    Task<BaseResponse<OutputSellProduct>> LeastSoldProduct();
    Task<BaseResponse<List<OutputSellProduct>>> TopSellers();
    Task<BaseResponse<OutputCustomerOrder>> BiggestBuyer();
    Task<BaseResponse<OutputCustomerOrder>> BiggestBuyerPrice();
    Task<BaseResponse<OutputBrandBestSeller>> BrandBestSeller();
    Task<BaseResponse<decimal>> Avarege();
    Task<BaseResponse<decimal>> Total();
}