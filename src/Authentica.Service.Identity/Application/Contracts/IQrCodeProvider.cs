namespace Application.Contracts;

public interface IQrCodeProvider
{
    string FormatKey(string unformattedKey);

    string Generate(string email, string unformattedKey);
}
