﻿using ProjetoTeste.Arguments.Arguments.Base;
using ProjetoTeste.Arguments.Arguments.Brand;
using ProjetoTeste.Infrastructure.Interface.Repositories;
using ProjetoTeste.Infrastructure.Persistence.Entity;

namespace ProjetoTeste.Infrastructure.Application;

public class BrandValidateService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IProductRepository _productRepository;

    public BrandValidateService(IBrandRepository brandRepository, IProductRepository productRepository)
    {
        _brandRepository = brandRepository;
        _productRepository = productRepository;
    }

    public async Task<BaseResponse<List<InputCreateBrand>>> ValidateCreate(List<InputCreateBrand> listInputCreateBrand)
    {
        var response = new BaseResponse<List<InputCreateBrand>>();
        if (!listInputCreateBrand.Any())
        {
            response.Success = false;
            response.AddErrorMessage(" >>> Dados Inseridos Inválidos <<<");
            return response;
        }

        // validar tamanhos por exemplo code, nome description...

        //var repeatedCode = (from i in input
        //                    where input.Count(j => j.Code == i.Code) > 1
        //                    select i).ToList();

        var repeatedCode = (from i in (from j in listInputCreateBrand
                                       group j by j.Code into g
                                       where g.Count() > 1
                                       select g).ToList()
                            from k in i
                            select k).ToList();

        foreach (var brand in repeatedCode)
        {
            response.AddErrorMessage($" >>> Erro - A marca de nome: {brand.Name} o código: {brand.Code} não pode ser cadastrado, por ser repetido <<<");
            listInputCreateBrand.Remove(brand);
        }

        if (!listInputCreateBrand.Any())
        {
            response.Success = false;
            return response;
        }

        var codeExists = (from i in listInputCreateBrand
                          where _brandRepository.CodeExists(i.Code) == true
                          select i).ToList();
        foreach (var item in codeExists)
        {
            response.AddErrorMessage($" >>> Erro - A marca de nome: {item.Name} o código: {item.Code} não pode ser cadastrado, por já estar em uso <<<");
        }

        if (codeExists.Count == listInputCreateBrand.Count)
        {
            response.Success = false;
            return response;
        }

        var validateCreate = (listInputCreateBrand.Except(codeExists)).ToList();
        response.Content = validateCreate;
        return response;
    }

    public async Task<BaseResponse<List<Brand>>> ValidateUpdate(List<long> ids, List<InputUpdateBrand> input)
    {
        var response = new BaseResponse<List<Brand>>();
        if (ids.Count() != input.Count)
        {
            response.Success = false;
            response.AddErrorMessage(" >>> ERRO - A Quantidade de Id's Digitados é Diferente da Quantidade de Marcas <<<");
            return response;
        }

        var notIdExist = (from i in ids
                          where _brandRepository.BrandExists(i) == false
                          select ids.IndexOf(i)).ToList();
        if (notIdExist.Any())
        {
            for (int i = 0; i < notIdExist.Count; i++)
            {
                response.AddErrorMessage($" >>> Marca com id: {ids[notIdExist[i]]} não encontrada <<<");
                ids.Remove(ids[notIdExist[i]]);
                input.Remove(input[notIdExist[i]]);
            }
        }
        if (!input.Any())
        {
            response.Success = false;
            return response;
        }

        var repeatedCode = (from i in input
                            where input.Count(j => j.Code == i.Code) > 1
                            select input.IndexOf(i)).ToList();
        if (repeatedCode.Any())
        {
            for (int i = 0; i < repeatedCode.Count; i++)
            {
                response.AddErrorMessage($" >>> Erro - A marca de nome: {input[repeatedCode[i]].Name} o código: {input[repeatedCode[i]].Code} não pode ser cadastrado, por ser repetido <<<");
                ids.Remove(ids[repeatedCode[i]]);
                input.Remove(input[repeatedCode[i]]);
            }
        }
        if (!input.Any())
        {
            response.Success = false;
            return response;
        }

        var brandList = await _brandRepository.GetListByListId(ids);

        var codeExists = (from i in input
                          let index = input.IndexOf(i)
                          where _brandRepository.CodeExists(i.Code) == true && i.Code != brandList[index].Code
                          select input.IndexOf(i)).ToList();

        if (codeExists.Any())
        {
            for (int i = 0; i < codeExists.Count; i++)
            {
                response.AddErrorMessage($" >>> Marca: {input[codeExists[i]].Name} o código: {input[codeExists[i]].Code} não pode ser alterado - Em uso por outra marca <<<");
                ids.Remove(ids[codeExists[i]]);
                input.Remove(input[codeExists[i]]);
                brandList.Remove(brandList[codeExists[i]]);
            }
        }
        if (!input.Any())
        {
            response.Success = false;
            return response;
        }

        for (int i = 0; i < brandList.Count(); i++)
        {
            brandList[i].Name = input[i].Name;
            brandList[i].Code = input[i].Code;
            brandList[i].Description = input[i].Description;
        }

        response.Content = brandList;
        return response;
    }

    public async Task<BaseResponse<List<long>>> ValidadeDelete(List<long> ids)
    {
        var response = new BaseResponse<List<long>>();
        var idExist = (from i in ids
                       where _brandRepository.BrandExists(i) == false
                       select i).ToList();

        if (idExist.Any())
        {
            foreach (var id in idExist)
            {
                response.AddErrorMessage($" >>> Marca com Id: {id} não Existe <<<");
            }
            ids = (ids.Except(idExist)).ToList();
        }

        var withProduct = await _productRepository.BrandId(ids);
        if (withProduct.Any())
        {
            foreach (var id in withProduct)
            {
                response.AddErrorMessage($" >>> Marca com id: {id} não pode ser deletada - Possui produtos <<<");
            }
        }

        List<long> withoutProduct = (ids.Except(withProduct)).ToList();
        if (!withoutProduct.Any())
        {
            response.Success = false;
            return response;
        }

        response.Content = withoutProduct;
        return response;
    }
}