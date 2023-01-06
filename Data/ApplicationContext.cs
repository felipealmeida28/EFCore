using EF_Dev.io.Data.Configurations;
using EF_Dev.io.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF_Dev.io.Data;

public class ApplicationContext : DbContext
{
    private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());
    
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLoggerFactory(_logger)
            .EnableSensitiveDataLogging()
            .UseSqlServer(
                // EnableRetryOnFailure para tentar novamente quando o acesso ao banco da falha
            "Server=localhost;User Id=sa;Pwd=9Pkg7*74Hk3SrF&;Database=CursoEFCore;Trusted_Connection=False;Encrypt=False;", p 
                => p.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        MapearPropriedadesEsquecidas(modelBuilder);
    }

    
    //verifica se não foi configurado o tamanho da coluna e seta um padrão 
    private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

            foreach (var property in properties)
            {
                if (string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue) 
                {
                    property.SetColumnType("VARCHAR(100)");
                }
            }
        }
    }
    
}