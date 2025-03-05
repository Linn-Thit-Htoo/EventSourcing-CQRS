using System;
using System.Collections.Generic;

namespace DotNet_EventSourcing.EventDispatcher.Entities;

public partial class TblStock
{
    public Guid StockId { get; set; }

    public Guid ProductId { get; set; }

    public long Stock { get; set; }

    public virtual TblProduct Product { get; set; } = null!;
}
