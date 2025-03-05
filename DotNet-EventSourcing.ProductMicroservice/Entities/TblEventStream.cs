using System;
using System.Collections.Generic;

namespace DotNet_EventSourcing.ProductMicroservice.Entities;

public partial class TblEventStream
{
    public Guid StreamId { get; set; }

    public string AggregateType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<TblEvent> TblEvents { get; set; } = new List<TblEvent>();
}
