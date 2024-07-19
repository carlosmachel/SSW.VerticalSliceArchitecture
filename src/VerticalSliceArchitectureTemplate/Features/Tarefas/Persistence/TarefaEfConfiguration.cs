using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain;

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Persistence;

public class TarefaEfConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Text)
            .IsRequired();
    }
}