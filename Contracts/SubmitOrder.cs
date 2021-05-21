using System;

namespace Contracts
{
    public record SubmitOrder
    {
        public Guid OrderId { get; init; }
        public DateTime Timestamp { get; init; }
        public string CustomerNumber { get; init; }
    }
}
