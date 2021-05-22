using System;

namespace Contracts
{
    public record OrderCloseAccepted
    {
        public Guid OrderId { get; init; }

        public DateTime Timestamp { get; init; }
    }
}
