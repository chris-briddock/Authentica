Param(
    [string]$WorkingDir 
)

dotnet user-secrets set "ConnectionStrings:Default" "YourSecretHere"
dotnet user-secrets set "ConnectionStrings:Redis" "YourSecretHere"
dotnet user-secrets set "ConnectionStrings:AzureServiceBus" "YourSecretHere"
dotnet user-secrets set "ApplicationInsights:InstrumentationKey" "YourSecretHere"

dotnet user-secrets set "Jwt:Issuer" "YourSecretHere"
dotnet user-secrets set "Jwt:Audience" "YourSecretHere"
dotnet user-secrets set "Jwt:Secret" "YourSecretHere"
dotnet user-secrets set "Jwt:Expires" "3600"

dotnet user-secrets set "RabbitMQ:Hostname" "YourSecretHere"
dotnet user-secrets set "RabbitMQ:Username" "YourSecretHere"
dotnet user-secrets set "RabbitMQ:Password" "YourSecretHere"


