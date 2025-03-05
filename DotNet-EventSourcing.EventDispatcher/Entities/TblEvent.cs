using System;
using System.Collections.Generic;

namespace DotNet_EventSourcing.EventDispatcher.Entities;

public partial class TblEvent
{
    public Guid EventId { get; set; }

    public Guid StreamId { get; set; }

    public string EventType { get; set; } = null!;

    public string EventData { get; set; } = null!;

    public long Version { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual TblEventStream Stream { get; set; } = null!;
}
