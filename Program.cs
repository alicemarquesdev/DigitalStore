using DigitalStore.Data;
using DigitalStore.Helper;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Repositorio;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionar servi�os � aplica��o
            ConfigureServices(builder);

            var app = builder.Build();

            // Configurar o pipeline de requisi��o HTTP
            ConfigureMiddleware(app);

            // Migra��o autom�tica do banco de dados
            ApplyDatabaseMigrations(app);

            app.Run();
        }

        // M�todo para configurar os servi�os
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Adicionar o IHttpContextAccessor, sess�es do usu�rio
            builder.Services.AddHttpContextAccessor();

            // Configurar se��es de configura��o (StripeSettings)
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

            // Configurar se��es de configura��o (GoogleAPI)
            builder.Services.Configure<GoogleAPISettings>(builder.Configuration.GetSection("GoogleAPISettings"));

            // Configurar se��es de configura��o (EmailSettings)
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            // Configurar o Logging
            builder.Logging.ClearProviders();  // Limpa os provedores de logging padr�o
            builder.Logging.AddConsole();  // Adiciona o log ao Console
            builder.Logging.AddDebug();    // Adiciona o log ao Debug (Visual Studio)

            // Servi�os de Controllers e Views
            builder.Services.AddControllersWithViews();

            // Configurar o contexto de banco de dados
            builder.Services.AddDbContext<BancoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

            // Configura��o de HTTP Client
            builder.Services.AddHttpClient();

            // Configura��o de Reposit�rios
            RegisterRepositories(builder);

            // Configura��o de session
            builder.Services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromMinutes(45);
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });

            // Adicionar compress�o de resposta
            builder.Services.AddResponseCompression();
        }

        // M�todo para registrar reposit�rios de forma mais limpa
        private static void RegisterRepositories(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            builder.Services.AddScoped<IFavoritosRepositorio, FavoritosRepositorio>();
            builder.Services.AddScoped<ICarrinhoRepositorio, CarrinhoRepositorio>();
            builder.Services.AddScoped<ISiteRepositorio, SiteRepositorio>();
            builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
            builder.Services.AddScoped<IEnderecoRepositorio, EnderecoRepositorio>();
            builder.Services.AddScoped<IItensDoPedidoRepositorio, ItensDoPedidoRepositorio>();
            builder.Services.AddScoped<IPagamentoRepositorio, PagamentoRepositorio>();
            builder.Services.AddScoped<ICaminhoImagem, CaminhoImagem>();
            builder.Services.AddScoped<IEnderecoFrete, EnderecoFrete>();
            builder.Services.AddScoped<IAlteracaoSenhaRepositorio, AlteracaoSenhaRepositorio>();
            builder.Services.AddScoped<ISessao, Sessao>();
            builder.Services.AddScoped<IEmail, Email>();
            builder.Services.AddScoped<StripeService>();
        }

        // M�todo para configurar o middleware
        private static void ConfigureMiddleware(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();

            // Adicionar compress�o de resposta
            app.UseResponseCompression();

            // Roteamento da aplica��o
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }

        // M�todo para aplicar migra��es autom�ticas no banco de dados
        private static void ApplyDatabaseMigrations(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BancoContext>();
                dbContext.Database.Migrate(); // Aplica a migra��o automaticamente no banco de dados
            }
        }
    }
}
