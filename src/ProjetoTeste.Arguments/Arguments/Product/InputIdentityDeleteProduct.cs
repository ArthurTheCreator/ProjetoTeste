﻿using ProjetoTeste.Arguments.Arguments.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetoTeste.Arguments.Arguments;

public class InputIdentityDeleteProduct(long id) : BaseInputIdentityDelete<InputIdentityDeleteProduct>(id) { }