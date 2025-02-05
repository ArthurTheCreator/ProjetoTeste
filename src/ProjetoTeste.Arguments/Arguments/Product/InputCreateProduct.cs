﻿using ProjetoTeste.Arguments.Arguments.Base;
using System.Text.Json.Serialization;
namespace ProjetoTeste.Arguments.Arguments.Product;

[method: JsonConstructor]
public class InputCreateProduct(string name, string code, string description, decimal price, long brandId, long stock) : BaseInputCreate<InputCreateProduct>
{
    public string Name { get; private set; } = name;
    public string Code { get; private set; } = code;
    public string Description { get; private set; } = description;
    public decimal Price { get; private set; } = price;
    public long BrandId { get; private set; } = brandId;
    public long Stock { get; private set; } = stock;
}