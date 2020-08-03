
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      // configure strongly typed settings objects
      var appSettingsSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingsSection);
      var appSettings = appSettingsSection.Get<AppSettings>();

      // ===== Add CORS ========
      services.AddCors();


      services.AddDbContext<DataContext>(options => options.UseSqlServer(appSettings.DefaultConnection));

      services.AddControllers().AddNewtonsoftJson();
      //services.AddMvc().AddNewtonsoftJson();

      // ===== Add Mapper ========
      // Auto Mapper Configurations
      var mappingConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new AutoMapperProfile());
      });

      IMapper mapper = mappingConfig.CreateMapper();
      services.AddSingleton(mapper);


      // ===== Add jwt authentication ========
      var key = Encoding.ASCII.GetBytes(appSettings.Secret);
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });

      // configure DI for application services
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IMenuService, MenuService>();
      services.AddScoped<IRoleService, RoleService>();
      services.AddScoped<IApplicationService, ApplicationService>();
      services.AddScoped<IOtpService, OtpService>();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<AuthorizationService>();
      services.AddScoped<IAgenciesServices, AgenciesServices>();
      services.AddScoped<ICampaignsService, CampaignsService>();
      services.AddScoped<IBonaFidesServices, BonaFidesServices>();
      services.AddScoped<IChapterServices, ChapterServices>();
      services.AddScoped<IQualifyingEventsSerivie, QualifyingEventsSerivie>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

      app.UseStaticFiles();
      app.UseRouting();

      // global cors policy
      app.UseCors(builder =>
      {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
      });

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });


      //Sedding Database
      using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
      {
        if (!serviceScope.ServiceProvider.GetService<DataContext>().AllMigrationsApplied())
        {
          serviceScope.ServiceProvider.GetService<DataContext>().Database.Migrate();
          serviceScope.ServiceProvider.GetService<DataContext>().EnsureSeeded();
        }
        else
        {
          serviceScope.ServiceProvider.GetService<DataContext>().EnsureSeeded();
        }
      }

    }
  }
}
