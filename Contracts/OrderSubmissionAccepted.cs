using System;

namespace Contracts
{
    public record OrderSubmissionAccepted
    {
        public Guid OrderId { get; init; }

        public DateTime Timestamp { get; init; }

        public string CustomerNumber { get; init; }
    }
}
