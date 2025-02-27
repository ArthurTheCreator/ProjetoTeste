﻿using ProjetoTeste.Arguments.Arguments;
using ProjetoTeste.Arguments.Arguments.Base;
using ProjetoTeste.Arguments.Enum.Validate;
using ProjetoTeste.Domain.Helper;
using ProjetoTeste.Domain.Service.Base;
using ProjetoTeste.Infrastructure.Interface.ValidateService;

namespace ProjetoTeste.Infrastructure.Application;

public class ProductValidateService : BaseValidate<ProductValidateDTO>, IProductValidateService
{
    #region Create
    public void ValidateCreate(List<ProductValidateDTO> listProductValidate)
    {
        var response = new BaseResponse<List<ProductValidateDTO>>();

        NotificationHelper.CreateDict();

        (from i in RemoveIgnore(listProductValidate)
         where i.InputCreateProduct == null
         let setIgnore = i.SetIgnore()
         select InvalidNull(listProductValidate.IndexOf(i))).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.OriginalCode != null
         let setInvalid = i.SetInvalid()
         select AlreadyExists(i.InputCreateProduct.Code, "Código")).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.RepeteCode != null
         let setInvalid = i.SetInvalid()
         select RepeatedIdentifier(i.InputCreateProduct.Code, "Código")).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.Original != null
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage($"O Produto: {i.InputCreateProduct.Name} com o código: {i.InputCreateProduct.Code} não pode ser cadastrado, por já estar em uso")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         let resultInvalidLenght = InvalidLenght(i.InputCreateProduct.Code, 1, 6)
         where resultInvalidLenght != EnumValidateType.Valid
         let setInvalid = resultInvalidLenght == EnumValidateType.Invalid ? i.SetInvalid() : i.SetIgnore()
         select InvalidLenght(i.InputCreateProduct.Code, i.InputCreateProduct.Code, 1, 6, resultInvalidLenght, "Código")).ToList();

        (from i in RemoveIgnore(listProductValidate)
         let resultInvalidLenght = InvalidLenght(i.InputCreateProduct.Name, 1, 24)
         where resultInvalidLenght != EnumValidateType.Valid
         let setInvalid = resultInvalidLenght == EnumValidateType.Invalid ? i.SetInvalid() : i.SetIgnore()
         select InvalidLenght(i.InputCreateProduct.Code, i.InputCreateProduct.Name, 1, 24, resultInvalidLenght, "Nome")).ToList();

        (from i in RemoveIgnore(listProductValidate)
         let resultInvalidLenght = InvalidLenght(i.InputCreateProduct.Description, 0, 100)
         where resultInvalidLenght != EnumValidateType.Valid
         let setInvalid = resultInvalidLenght == EnumValidateType.Invalid ? i.SetInvalid() : i.SetIgnore()
         select InvalidLenght(i.InputCreateProduct.Code, i.InputCreateProduct.Description, 0, 100, resultInvalidLenght, "Descrição")).ToList();

        //(from i in RemoveIgnore(listProductValidate)
        // where i.BrandId == 0
        // let setInvalid = i.SetInvalid()
        // select InvalidRelatedProperty(i.InputCreateProduct.Code, i.InputCreateProduct.Name)).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.InputCreateProduct.Price < 0
         let setInvalid = i.SetInvalid()
         select NegativePrice(i.InputCreateProduct.Code, i.InputCreateProduct.Price)).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.InputCreateProduct.Stock < 0
         let setInvalid = i.SetInvalid()
         select NegativeStock(i.InputCreateProduct.Code, i.InputCreateProduct.Stock)).ToList();

        (from i in RemoveInvalid(listProductValidate)
         select SuccessfullyRegistered(i.InputCreateProduct.Code, "Produto")).ToList();

    }
    #endregion

    #region Update
    public void ValidateUpdate(List<ProductValidateDTO> listProductValidate)
    {
        NotificationHelper.CreateDict();

        var response = new BaseResponse<List<ProductValidateDTO>>();

        (from i in RemoveIgnore(listProductValidate)
         where i.RepeteIdentity == default
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage($"O Produto com Id: {i.InputIdentityUpdateProduct.Id} não pode ser atualizado, por ser repetido")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.Original == null
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage($"O Produto com Id: {i.InputIdentityUpdateProduct.Id} não pode ser atualizado, por não existir")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.RepeteCode != null
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage($"O Produto com Id: {i.InputIdentityUpdateProduct.Id} com o código: {i.InputIdentityUpdateProduct.InputUpdateProduct.Code} não pode ser atualizado, por ser repetido")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.OriginalCode != null && i.Original.Code != i.InputIdentityUpdateProduct.InputUpdateProduct.Code
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage($"O Produto com Id: {i.InputIdentityUpdateProduct.Id} com o código: {i.InputIdentityUpdateProduct.InputUpdateProduct.Code} não pode ser atualizado, por já estar em uso")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.BrandId == default
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage($"O Produto com Id: {i.InputIdentityUpdateProduct.Id} com o código de marca: {i.InputIdentityUpdateProduct.InputUpdateProduct.BrandId} não pode ser atualizado, por não existir")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.InputIdentityUpdateProduct.InputUpdateProduct.Price < 0
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage($"O Produto com Id: {i.InputIdentityUpdateProduct.Id} não pode ser atualizado, Com preço Negativo")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.InputIdentityUpdateProduct.InputUpdateProduct.Stock < 0
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage($"O Produto com Id: {i.InputIdentityUpdateProduct.Id} não pode ser atualizado, Com Stock Negativo")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         let resultInvalidLenght = InvalidLenght(i.InputIdentityUpdateProduct.InputUpdateProduct.Name, 1, 24)
         where resultInvalidLenght != EnumValidateType.Valid
         let setInvalid = resultInvalidLenght == EnumValidateType.Invalid ? i.SetInvalid() : i.SetIgnore()
         select InvalidLenght(i.InputIdentityUpdateProduct.InputUpdateProduct.Code, i.InputIdentityUpdateProduct.InputUpdateProduct.Code, 1, 24, resultInvalidLenght, "Nome")).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.InputIdentityUpdateProduct.InputUpdateProduct.Code.Length > 6 || string.IsNullOrEmpty(i.InputIdentityUpdateProduct.InputUpdateProduct.Code) || string.IsNullOrWhiteSpace(i.InputIdentityUpdateProduct.InputUpdateProduct.Code)
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage(i.InputIdentityUpdateProduct.InputUpdateProduct.Code.Length > 6 ? $"Produto com Id {i.InputIdentityUpdateProduct.Id} o código: '{i.InputIdentityUpdateProduct.InputUpdateProduct.Name}' possui um código com mais de 6 caracteres e não pode ser cadastrado."
         : $"Produto com Id {i.InputIdentityUpdateProduct.Id} o código: '{i.InputIdentityUpdateProduct.InputUpdateProduct.Name}' possui um código vazio e não pode ser cadastrado.")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.InputIdentityUpdateProduct.InputUpdateProduct.Description.Length > 100 || string.IsNullOrEmpty(i.InputIdentityUpdateProduct.InputUpdateProduct.Description) || string.IsNullOrWhiteSpace(i.InputIdentityUpdateProduct.InputUpdateProduct.Description)
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage(i.InputIdentityUpdateProduct.InputUpdateProduct.Description.Length > 100 ? $"Produto com Id {i.InputIdentityUpdateProduct.Id} a descrição: '{i.InputIdentityUpdateProduct.InputUpdateProduct.Name}' possui uma descrição com mais de 100 caracteres e não pode ser cadastrado."
         : $"Produto com Id {i.InputIdentityUpdateProduct.Id} a descrição: '{i.InputIdentityUpdateProduct.InputUpdateProduct.Name}' possui uma descrição vazia e não pode ser cadastrado.")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where !i.Invalid
         select i).ToList();
    }
    #endregion

    #region Delete
    public void ValidateDelete(List<ProductValidateDTO> listProductValidate)
    {
        NotificationHelper.CreateDict();

        var response = new BaseResponse<List<ProductValidateDTO>>();

        (from i in RemoveIgnore(listProductValidate)
         where i.RepetedIdentity != 0
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage($"O Id: {i.RepetedIdentity} foi digitado repetidas vezes, não é possível deletar a marca com esse Id")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where i.Original == null
         let setInvalid = i.SetInvalid()
         let message = response.AddErrorMessage($"Produto com Id: {i.InputIdentifyDeleteProduct.Id} não foi encontrado")
         select i).ToList();

        (from i in RemoveIgnore(listProductValidate)
         where !i.Invalid && i.Original.Stock > 0
         let setInvald = i.SetInvalid()
         let message = response.AddErrorMessage($"O Produto: {i.Original.Name} com Id: {i.InputIdentifyDeleteProduct.Id} não pode ser deletado pois possui estoque: {i.Original.Stock}")
         select i).ToList();

        (from i in RemoveInvalid(listProductValidate)
         select SuccessfullyDeleted(i.InputIdentifyDeleteProduct.Id.ToString(), "Produto")).ToList();
    }
    #endregion

}