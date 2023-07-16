using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://localhost:10001";// IdentityServer address
        options.Audience = "OrdersAPI";
        //options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };//IdentityServer по умолчанию выдает заголовок типа, рекомендуется дополнительная проверка
        //options.RequireHttpsMetadata = false;//Получает или задает, требуется ли HTTPS для адреса или центра метаданных.
        //options.Events = new JwtBearerEvents()
        //{
        //    OnAuthenticationFailed = (context) =>
        //    {
        //        context.NoResult();
        //        return Task.CompletedTask;
        //    },
        //};
    });
//builder.Services.AddSwaggerGen();
//// add services CORS
//builder.Services.AddCors();

//builder.Services.AddControllersWithViews();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// add services CORS
//app.UseCors(builder => builder
//    .AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(x => x.MapDefaultControllerRoute());

app.Run();
