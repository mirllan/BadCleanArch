using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Order
{
    // aqui puse propiedades porque sonar decia que no se deben usar campos publicos y asi queda mejor
    public int Id { get; set; }

    // este es el nombre del cliente y lo deje asi para que sea facil de usar en otras partes
    public string CustomerName { get; set; }

    // este es el nombre del producto que se esta pidiendo en la orden
    public string ProductName { get; set; }

    // esta es la cantidad del producto que se pidio y se usa para calcular el total
    public int Quantity { get; set; }

    // este es el precio por unidad y sirve para calcular el total de la orden
    public decimal UnitPrice { get; set; }

    public void CalculateTotalAndLog()
    {
        // aqui se calcula el total multiplicando la cantidad por el precio y luego se manda a un log
        var total = Quantity * UnitPrice;
        Infrastructure.Logging.Logger.Log("Total (maybe): " + total);
    }
}
