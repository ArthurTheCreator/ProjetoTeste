﻿using ProjetoTeste.Arguments.Arguments.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetoTeste.Arguments.Arguments.Customer;

public class InputCreateCustomer : BaseInputCreate<InputCreateCustomer>
{
    public string Name { get; private set; }

    [Required(ErrorMessage = "O campo {0} é OBRIGATÓRIO - Identificador")]
    public string CPF { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }

    public InputCreateCustomer()
    {

    }

    [JsonConstructor]
    public InputCreateCustomer(string name, string cPF, string email, string phone)
    {
        Name = name;
        CPF = cPF;
        Email = email;
        Phone = phone;
    }
}