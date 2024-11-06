using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes {
    public static class EstudantesRotas {

        public static void AddRotasEstudantes(this WebApplication app)
        {
            var rotasestudantes = app.MapGroup("estudantes");

            //Add novo estudante
            rotasestudantes.MapPost("",
                async (AddEstudanteRequest request, AppDbContext context) =>
                {
                    var jaExiste = await context.Estudantes
                    .AnyAsync(estudante => estudante.Nome == request.Nome);

                    if (jaExiste)
                        return Results.Conflict("Já existe!");

                    var novoEstudante = new Estudante(request.Nome);
                    await context.Estudantes.AddAsync(novoEstudante);
                    await context.SaveChangesAsync();

                    var estudanteRetorno = new EstudanteDto(novoEstudante.Id, novoEstudante.Nome);
                    return Results.Ok(estudanteRetorno);

                });

            //Pegar todos os estudantes
            rotasestudantes.MapGet("",
                async (AppDbContext context) =>
                {
                    var estudantes = await context
                        .Estudantes
                        .Where(estudante => estudante.Ativo)
                        .Select(estudante => new EstudanteDto(estudante.Id, estudante.Nome))
                        .ToListAsync();

                    return Results.Ok(estudantes);
                });

            //Atualizar nome dos estudantes
            rotasestudantes.MapPut("{id:guid}", 
                async (Guid id, UpdateEstudanteRequest request, AppDbContext context) =>
                {
                    var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id);
                    if (estudante == null)
                        return Results.NotFound();
                    
                    estudante.AtualizarNome(request.nome);

                    await context.SaveChangesAsync();
                    return Results.Ok(new EstudanteDto(estudante.Id, estudante.Nome));
                });

            //Deletar um registro - soft delete(inativa o registro)
            rotasestudantes.MapDelete("{id}",
                async (Guid id, AppDbContext context) =>
                {
                    var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id);
                    if (estudante == null)
                        return Results.NotFound();

                    estudante.DesativarEstudante();
                    await context.SaveChangesAsync();
                    return Results.Ok();

                   
                });
        }

        

    }
}
