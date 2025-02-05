﻿using ProjetoTeste.Arguments.Arguments.Base;
using System.Text.Json.Serialization;
namespace ProjetoTeste.Arguments.Arguments.Brand;

[method: JsonConstructor]
public class OutputBrand(long id, string name, string code, string description) : BaseOutput<OutputBrand>
{
    public long Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string Code { get; private set; } = code;
    public string Description { get; private set; } = description;
}