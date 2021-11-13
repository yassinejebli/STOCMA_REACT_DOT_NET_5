using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using STOCMA.Data;
using STOCMA.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using STOCMA.Data.Configurations;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Linq;
using STOCMA.Utils;

namespace STOCMA
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
            services.AddControllers().AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()).Select().Filter().Expand().Count().OrderBy().SetMaxTop(100));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<ODataQueryStringFixer>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseODataQueryStringFixer();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                //endpoints.MapODataRoute("odata", "odata", GetEdmModel());

            });


            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
            DatabaseInitializer.Initialize(dbContext);
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Setting>("Settings");
            builder.EntitySet<Article>("Articles");
            builder.EntitySet<Client>("Clients");
            builder.EntitySet<Fournisseur>("Fournisseurs");
            builder.EntitySet<BonLivraison>("BonLivraisons");
            builder.EntitySet<BonLivraisonItem>("BonLivraisonItems");

            builder.StructuralTypes.First(t => t.ClrType == typeof(Client)).AddProperty(typeof(Client).GetProperty("Solde"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(Client)).AddProperty(typeof(Client).GetProperty("SoldeFacture"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(Fournisseur)).AddProperty(typeof(Fournisseur).GetProperty("Solde"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(Fournisseur)).AddProperty(typeof(Fournisseur).GetProperty("SoldeFacture"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(Article)).AddProperty(typeof(Article).GetProperty("QteStockSum"));
            return builder.GetEdmModel();
        }
    }
}
