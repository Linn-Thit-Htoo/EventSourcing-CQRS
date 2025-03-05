using System;
using System.Collections.Generic;

namespace DotNet_EventSourcing.ProductMicroservice.Entities;

public partial class SchemaMigration
{
    public long Version { get; set; }

    public DateTime? InsertedAt { get; set; }
}
