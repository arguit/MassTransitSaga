using System;
using Automatonymous;
using Contracts;
using MassTransit;

namespace Components.StateMachines
{
    public class OrderStateMachine :
        MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; private set; }
        
        public Event<SubmitOrder> SubmitOrderRequested { get; private set; }
        public Event<CheckOrder> CheckOrderRequested { get; private set; }

        public OrderStateMachine()
        {
            Event(() => SubmitOrderRequested, n => 
                n.CorrelateById(m => m.Message.OrderId));
                
            Event(() => CheckOrderRequested, n =>
            {
                n.CorrelateById(m => m.Message.OrderId);                
                n.OnMissingInstance(m => m.ExecuteAsync(async context =>
                {
                    if (context.RequestId.HasValue)
                    {
                        await context.RespondAsync<OrderNotFound>(new() { OrderId = context.Message.OrderId });
                    }
                }));
            });

            InstanceState(n => n.CurrentState);

            Initially(
                When(SubmitOrderRequested)
                    .Then(context =>
                    {
                        context.Instance.SubmitDate = context.Data.Timestamp;
                        context.Instance.CustomerNumber = context.Data.CustomerNumber;
                        context.Instance.Updated = DateTime.UtcNow;
                    })
                    .TransitionTo(Submitted));

            During(Submitted,
                Ignore(SubmitOrderRequested));

            DuringAny(
                When(CheckOrderRequested)
                    .RespondAsync(n => n.Init<OrderStatus>(new OrderStatus()
                    {
                        OrderId = n.Instance.CorrelationId,
                        State = n.Instance.CurrentState
                    })));

            DuringAny(
                When(SubmitOrderRequested)
                    .Then(context =>
                    {
                        context.Instance.SubmitDate ??= context.Data.Timestamp;
                        context.Instance.CustomerNumber ??= context.Data.CustomerNumber;
                    }));
        }
    }
}