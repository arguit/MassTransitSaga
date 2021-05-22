using System;

namespace Contracts
{
    public record CloseOrder
    {
        public Guid OrderId { get; init; }
        public DateTime Timestamp { get; init; }
    }
}
