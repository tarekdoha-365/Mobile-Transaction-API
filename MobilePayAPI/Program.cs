using Microsoft.EntityFrameworkCore;
using MobilePayAPI.Data;
using MobilePayAPI.Interfaces;
using MobilePayAPI.Repositories;
using Hangfire;
using MobilePayAPI.Services;


    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("TransactionConn")));
    builder.Services.AddHangfireServer();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("TransactionConn"))
                    );
    builder.Services.AddScoped<IMerchantService, MerchantServiceRepo>();
    builder.Services.AddScoped<IFeeService, FeeService>();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHangfireDashboard();
    app.UseHttpsRedirection();



    app.UseAuthorization();

    app.MapControllers();

    app.Run();
