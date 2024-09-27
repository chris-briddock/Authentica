namespace Application.Activities;

public sealed class CreateDeviceCodeActivity : ActivityBase<string>
{
    public override string Payload { get; set; } = default!;
}