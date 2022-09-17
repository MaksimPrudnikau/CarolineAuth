using System.ComponentModel.DataAnnotations.Schema;
using CarolineAuth.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarolineAuth.Models;

[Table("Vocabularies")]
public sealed class Vocabulary
{
    private string? data;
    
    public int Id { get; set; }
    public string UserId { get; set; }

    public List<Row> Data { get; set; }
}

public class VocabularyConfiguration : IEntityTypeConfiguration<Vocabulary>
{
    public void Configure(EntityTypeBuilder<Vocabulary> builder)
    {
        builder.Property(v => v.Data).HasJsonConversion();
    }
}