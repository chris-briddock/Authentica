namespace Authentica.TestClient;
public class Program
{
    private Program(){}
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        builder.Services.AddBearerAuthentication();
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthentication();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.MapGet("/callback", (HttpContext context) =>
        {
            var state = context.Request.Query["state"].ToString();
            var code = context.Request.Query["code"].ToString();

            var result = new { State = state, Code = code };
            return TypedResults.Ok(result);
        });

        app.MapGet("/", () =>
        {
            return TypedResults.Ok("Hello, from /");
        });

        app.Run();
    }
}