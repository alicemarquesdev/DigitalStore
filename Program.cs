using DigitalStore.Data;
using DigitalStore.Helper;
using DigitalStore.Repositorio;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Configuration;

namespace DigitalStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<BancoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<FreteServicos>();

            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            builder.Services.AddScoped<IFavoritosRepositorio, FavoritosRepositorio>();
            builder.Services.AddScoped<ICarrinhoRepositorio, CarrinhoRepositorio>();
            builder.Services.AddScoped<ISiteRepositorio, SiteRepositorio>();
            builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
            builder.Services.AddScoped<IEnderecoRepositorio, EnderecoRepositorio>();
            builder.Services.AddScoped<IItensDoPedidoRepositorio, ItensDoPedidoRepositorio>();
            builder.Services.AddScoped<IPagamentoRepositorio, PagamentoRepositorio>();
            builder.Services.AddScoped<ISessao, Sessao>();
            builder.Services.AddScoped<IEmail, Email>();
            builder.Services.AddScoped<ICaminhoImagem, CaminhoImagem>();
            builder.Services.AddScoped<StripeService>();


            builder.Services.AddSession(o =>
            {
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}