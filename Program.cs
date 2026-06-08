var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

// API endpoint รับตัวเลข แล้วเช็คว่าเป็นจำนวนเฉพาะไหม
app.MapPost("/api/prime/check", (PrimeRequest request) =>
{
    if (request.Number < 2)
        return Results.Ok(new PrimeResponse(request.Number, false, $"{request.Number} ไม่ใช่จำนวนเฉพาะ"));

    bool isPrime = IsPrime(request.Number);
    string message = isPrime
        ? $"{request.Number} เป็นจำนวนเฉพาะ"
        : $"{request.Number} ไม่ใช่จำนวนเฉพาะ";

    return Results.Ok(new PrimeResponse(request.Number, isPrime, message));
});

app.Run();

// ฟังก์ชันเช็คจำนวนเฉพาะ
static bool IsPrime(long n)
{
    if (n < 2) return false;
    if (n == 2) return true;
    if (n % 2 == 0) return false;

    for (long i = 3; i <= Math.Sqrt(n); i += 2)
    {
        if (n % i == 0) return false;
    }
    return true;
}

record PrimeRequest(long Number);
record PrimeResponse(long Number, bool IsPrime, string Message);