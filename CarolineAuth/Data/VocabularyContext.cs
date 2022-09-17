using CarolineAuth.Models;
using Microsoft.EntityFrameworkCore;

namespace CarolineAuth.Data;

public sealed class VocabularyContext : DbContext
{
    public VocabularyContext(DbContextOptions<VocabularyContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Vocabulary>? Vocabularies { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VocabularyConfiguration());
    }
}