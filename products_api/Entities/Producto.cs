using System;
using System.Collections.Generic;

namespace products_api.Entities;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public static implicit operator Producto(Task<Producto?> v)
    {
        throw new NotImplementedException();
    }
}
