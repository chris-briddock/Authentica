using Api.Requests;

namespace Domain.Events;

public class RegisterAdminEvent : EventBase<RegisterRequest>
{
    public override RegisterRequest Payload { get; set; }
}
