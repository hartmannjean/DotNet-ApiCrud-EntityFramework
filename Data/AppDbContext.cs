using ApiCrud.Estudantes;
using ApiCrud.Professores;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Data {

    /*
        DbContext é a classe principal do Entity Framework Core que é usada para se comunicar com o banco de dados.
        Ele é responsável por configurar o modelo, rastrear as entidades, executar consultas e salvar alterações no banco de dados.
        O DbContext é uma combinação de Unit of Work e Repository Patterns
     */
    public class AppDbContext : DbContext
    {
        public DbSet<Estudante> Estudantes { get; set; }
        public DbSet<Professor> Professores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Banco.sqlite");
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

    }
}
