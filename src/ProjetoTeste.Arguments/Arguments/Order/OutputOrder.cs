﻿using ProjetoTeste.Arguments.Arguments.Base;
using ProjetoTeste.Arguments.Arguments.ProductOrder;
using System.Text.Json.Serialization;

namespace ProjetoTeste.Arguments.Arguments.Order;

public class OutputOrder : BaseOutput<OutputOrder>
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public List<OutputProductOrder> listProductOrders { get; set; }
    public decimal Total { get; set; }
    public DateTime OrderDate { get; set; }

    [JsonConstructor]
    public OutputOrder(long id, long customerId, List<OutputProductOrder> productOrders, decimal total, DateTime orderDate)
    {
        Id = id;
        CustomerId = customerId;
        listProductOrders = productOrders;
        Total = total;
        OrderDate = orderDate;

    }
}