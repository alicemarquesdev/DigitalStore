using DigitalStore.Data;
using DigitalStore.Helper;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Repositorio;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DigitalStore
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console() // Logs no Console
            .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day) // Logs em arquivo
            .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Adicionar Serilog como provedor de logs
            builder.Host.UseSerilog();

            // Adicionar serviços à aplicação
            ConfigureServices(builder);

            var app = builder.Build();

            // Configurar o pipeline de requisição HTTP
            ConfigureMiddleware(app);

            // Migração automática do banco de dados
            ApplyDatabaseMigrations(app);

            app.Run();
        }

        // Método para configurar os serviços
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Adicionar o IHttpContextAccessor, sessões do usuário
            builder.Services.AddHttpContextAccessor();

            // Configurar seções de configuração (StripeSettings)
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

            // Configurar seções de configuração (GoogleAPI)
            builder.Services.Configure<GoogleAPISettings>(builder.Configuration.GetSection("GoogleAPISettings"));

            // Configurar seções de configuração (EmailSettings)
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            // Configurar o Logging
            builder.Logging.ClearProviders();  // Limpa os provedores de logging padrão
            builder.Logging.AddConsole();  // Adiciona o log ao Console
            builder.Logging.AddDebug();    // Adiciona o log ao Debug (Visual Studio)

            // Serviços de Controllers e Views
            builder.Services.AddControllersWithViews();

            // Configurar o contexto de banco de dados
            builder.Services.AddDbContext<BancoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

            // Configuração de HTTP Client
            builder.Services.AddHttpClient();

            // Configuração de Repositórios
            RegisterRepositories(builder);

            // Configuração de session
            builder.Services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromMinutes(30);
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });

            // Adicionar compressão de resposta
            builder.Services.AddResponseCompression();
        }

        // Método para registrar repositórios de forma mais limpa
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
            builder.Services.AddScoped<IAlteracaoSenhaRepositorio, AlterarSenhaRepositorio>();
            builder.Services.AddScoped<ISessao, Sessao>();
            builder.Services.AddScoped<IEmail, Email>();
            builder.Services.AddScoped<StripeService>();
        }

        // Método para configurar o middleware
        private static void ConfigureMiddleware(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Captura o erro 400 (Bad Request) devido à falha no token
            app.UseStatusCodePagesWithRedirects("/Home/Error");

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            // Adicionar compressão de resposta
            app.UseResponseCompression();

            // Roteamento da aplicação
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");
        }

        // Método para aplicar migrações automáticas no banco de dados
        private static void ApplyDatabaseMigrations(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BancoContext>();
                // dbContext.Database.Migrate(); // Aplica a migração automaticamente no banco de dados
            }
        }
    }
}
