﻿namespace ProjetoTeste.Arguments.Arguments;

public class ProductDTO
{
    public ProductDTO()
    {
    }
    public long Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public long BrandId { get; set; }
    public long Stock { get; set; }

    public BrandDTO? Brand { get; set; }
    public List<ProductOrderDTO>? ListProductOrder { get; set; }

    public ProductDTO(long id, string name, string code, string description, decimal price, long brandId, long stock)
    {
        Id = id;
        Name = name;
        Code = code;
        Description = description;
        Price = price;
        BrandId = brandId;
        Stock = stock;
    }
}
