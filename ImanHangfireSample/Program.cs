using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SampleHangfire.Data;
using Hangfire;
using Hangfire.SqlServer;
using SampleHangfire.Entities;
using SampleHangfire.Infrastrucures;
using SampleHangfire;
using Hangfire.Dashboard;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<StartedRecurringJobClass>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer(options =>
{
    options.SchedulePollingInterval = TimeSpan.FromSeconds(1);
    options.Queues = new string[] { "default" , "send-sms" };
    options.WorkerCount = 1; //Default ==> Environment.ProcessorCount * 5
});

builder.Services.AddSingleton<SmsService>();
builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<IEmailService,EmailService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
{
    Attempts = 3,
    DelaysInSeconds = new[] { 10, 20, 30 }
});

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(StartRecurringJobs);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    //Authorization = new[] { new HangfireAuthorizationFilter() },
    DashboardTitle = "Hanfire Dashboard Sample",
    //DisplayNameFunc=(ctx,job)=>$"{job.Type.Name} -> {job.Method.Name}", // global for all jobs
    //IsReadOnlyFunc = (DashboardContext context) => true,
});

app.Run();

static void StartRecurringJobs()
{
    RecurringJob.AddOrUpdate<IEmailService>("RecurringJob_With_LifeTime", p => p.SendNews(), Cron.Minutely());
}