using System;

namespace Contracts
{
    public record OrderSubmissionRejected
    {
        public Guid OrderId { get; init; }

        public DateTime Timestamp { get; init; }

        public string CustomerNumber { get; init; }

        public string Reason { get; init; }
    }
}
