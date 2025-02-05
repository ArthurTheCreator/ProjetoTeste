﻿using ProjetoTeste.Arguments.Arguments.Base;
using ProjetoTeste.Arguments.Arguments.Customer;
using System.Text.Json.Serialization;

namespace ProjetoTeste.Arguments.Arguments;

[method: JsonConstructor]
public class InputIdentityUpdateCustomer(long id, InputUpdateCustomer inputUpdateCustomer) : BaseInputIdentityUpdate<InputIdentityUpdateCustomer>
{
    public long Id { get; private set; } = id;
    public InputUpdateCustomer InputUpdateCustomer { get; private set; } = inputUpdateCustomer;
}
