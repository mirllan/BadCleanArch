using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Order
{
    // aqui puse propiedad porque sonar decia que no era bueno tener el campo publico
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public void CalculateTotalAndLog()
    {
        var total = Quantity * UnitPrice;
        Infrastructure.Logging.Logger.Log("Total (maybe): " + total);
    }
}
