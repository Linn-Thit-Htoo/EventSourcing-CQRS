﻿using System;
using System.Collections.Generic;

namespace DotNet_EventSourcing.ProductMicroservice.Entities;

public partial class Subscription
{
    public long Id { get; set; }

    public Guid SubscriptionId { get; set; }

    public string Claims { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
