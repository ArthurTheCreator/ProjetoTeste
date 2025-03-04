﻿using ProjetoTeste.Arguments.Arguments.Brand;
using ProjetoTeste.Domain.DTO;

namespace ProjetoTeste.Arguments.Arguments;

public class BrandValidateDTO : BaseValidateDTO
{
    public InputCreateBrand? InputCreate { get; private set; }
    public string? RepeatedInputCreateCode { get; private set; }
    public List<InputIdentityUpdateBrand>? ListRepeatedInputIdetityUpdate { get; private set; }
    public BrandDTO? OriginalBrandDTO { get; private set; }
    public InputIdentityUpdateBrand InputUpdate { get; private set; }
    public long RepetedInputUpdateIdentify { get; private set; }
    public string RepetedCode { get; private set; }
    public string? CodeExists { get; private set; }
    public InputIdentifyDeleteBrand InputIdentifyDeleteBrand { get; private set; }
    public InputIdentifyDeleteBrand? RepeteInputDelete { get; private set; }
    public long? BrandWithProduct { get; private set; }

    public BrandValidateDTO ValidateCreate(InputCreateBrand? inputCreate, string? repeatedInputCreate, BrandDTO? originalBrand)
    {
        InputCreate = inputCreate;
        RepeatedInputCreateCode = repeatedInputCreate;
        OriginalBrandDTO = originalBrand;
        return this;
    }
    public BrandValidateDTO ValidateUpdate(InputIdentityUpdateBrand? inputUpdate, long repetedInputUpdate, BrandDTO? originalBrand, string repetedCode, string? codeExists)
    {
        InputUpdate = inputUpdate;
        RepetedInputUpdateIdentify = repetedInputUpdate;
        OriginalBrandDTO = originalBrand;
        RepetedCode = repetedCode;
        CodeExists = codeExists;
        return this;
    }
    public BrandValidateDTO ValidateDelete(InputIdentifyDeleteBrand inputIdentifyDeleteBrand, BrandDTO? originalBrand, InputIdentifyDeleteBrand repeteInputDelete, long? brandWIthProduct)
    {
        InputIdentifyDeleteBrand = inputIdentifyDeleteBrand;
        OriginalBrandDTO = originalBrand;
        RepeteInputDelete = repeteInputDelete;
        BrandWithProduct = brandWIthProduct;
        return this;
    }
}