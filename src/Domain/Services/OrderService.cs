using System;
using System.Collections.Generic;

namespace Domain.Services;

using Domain.Entities;

public static class OrderService
{
    // aqui dejo la lista privada para que nadie la cambie por fuera y solo se vea como lectura
    private static readonly List<Order> _lastOrders = new List<Order>();
    public static IReadOnlyList<Order> LastOrders => _lastOrders.AsReadOnly();

    public static Order CreateTerribleOrder(string customer, string product, int qty, decimal price)
    {
        var o = new Order { Id = new Random().Next(1, 9999999), CustomerName = customer, ProductName = product, Quantity = qty, UnitPrice = price };
        _lastOrders.Add(o);
        Infrastructure.Logging.Logger.Log("Created order " + o.Id + " for " + customer);
        return o;
    }
}
