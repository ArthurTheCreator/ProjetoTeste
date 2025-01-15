﻿using ProjetoTeste.Arguments.Arguments;
using ProjetoTeste.Arguments.Arguments.Base;
using ProjetoTeste.Arguments.Arguments.Product;

namespace ProjetoTeste.Infrastructure.Interface.Service;

public interface IProductService
{
    Task<List<OutputProduct>> GetAll();
    Task<OutputProduct> Get(long id);
    Task<List<OutputProduct>> GetListByListId(List<long> idList);
    Task<List<OutputProduct>> GetListByBrandId(long id);
    Task<BaseResponse<OutputProduct>> Create(InputCreateProduct inputCreateProduct);
    Task<BaseResponse<List<OutputProduct>>> CreateMultiple(List<InputCreateProduct> listinputCreateProduct);
    Task<BaseResponse<bool>> Update(InputIdentityUpdateProduct inputIdentityUpdateProduct);
    Task<BaseResponse<bool>> UpdateMultiple(List<InputIdentityUpdateProduct> listInputIdentityUpdateProduct);
    Task<BaseResponse<bool>> Delete(long id);
    Task<BaseResponse<bool>> DeleteMultiple(List<long> idList);
}