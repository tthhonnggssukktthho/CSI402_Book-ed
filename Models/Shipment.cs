using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class Shipment
{
    public int ShipmentId { get; set; }

    public int OrderId { get; set; }

    public string ShipmentStatus { get; set; } = null!;

    public string? CarrierName { get; set; }

    public string? TrackingNo { get; set; }

    public DateTime? ShippingLabelPrintedAt { get; set; }

    public int? PackedByEmployeeId { get; set; }

    public DateTime? ShippedAt { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Employee? PackedByEmployee { get; set; }
}
