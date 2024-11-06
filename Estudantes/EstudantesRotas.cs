using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes {
    public static class EstudantesRotas {

        public static void AddRotasEstudantes(this WebApplication app)
        {
            var rotasestudantes = app.MapGroup("estudantes");


            /*
               Sobre cancellationToken:
           
                1.	Criação: Um CancellationToken é criado a partir de um CancellationTokenSource.
                2.	Passagem: O token é passado para métodos assíncronos que suportam cancelamento.
                3.	Cancelamento: Quando o cancelamento é solicitado, o CancellationTokenSource sinaliza o token, e os métodos que estão observando o token podem interromper a operação.
             
             */

            //Add novo estudante
            rotasestudantes.MapPost("",
                async (AddEstudanteRequest request, AppDbContext context, CancellationToken ct) =>
                {
                    //AnyAsync = Verifica se existe algum estudante com o mesmo nome
                    var jaExiste = await context.Estudantes
                    .AnyAsync(estudante => estudante.Nome == request.Nome, ct);

                    //Conflict = 409
                    if (jaExiste)
                        return Results.Conflict("Já existe!");

                    var novoEstudante = new Estudante(request.Nome);
                    //AddAsync = Adiciona um novo estudante
                    await context.Estudantes.AddAsync(novoEstudante, ct);
                    //SaveChangesAsync = Salva as alterações no banco de dados
                    await context.SaveChangesAsync(ct);

                    //Retorna o estudante criado
                    //Results.Ok = 200
                    var estudanteRetorno = new EstudanteDto(novoEstudante.Id, novoEstudante.Nome);
                    return Results.Ok(estudanteRetorno);

                });

            //Pegar todos os estudantes ativos
            rotasestudantes.MapGet("",
                async (AppDbContext context, CancellationToken ct) =>
                {
                    var estudantes = await context
                        .Estudantes
                        .Where(estudante => estudante.Ativo)
                        .Select(estudante => new EstudanteDto(estudante.Id, estudante.Nome))
                        .ToListAsync(ct);

                    return Results.Ok(estudantes);
                });

            //Atualizar nome dos estudantes
            rotasestudantes.MapPut("{id:guid}", 
                async (Guid id, UpdateEstudanteRequest request, AppDbContext context, CancellationToken ct) =>
                {
                    var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id, ct);
                    if (estudante == null)
                        return Results.NotFound();
                    
                    estudante.AtualizarNome(request.nome);

                    await context.SaveChangesAsync(ct);
                    return Results.Ok(new EstudanteDto(estudante.Id, estudante.Nome));
                });

            //Deletar um registro - soft delete(inativa o registro)
            rotasestudantes.MapDelete("{id}",
                async (Guid id, AppDbContext context, CancellationToken ct) =>
                {
                    var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id, ct);
                    if (estudante == null)
                        return Results.NotFound();

                    estudante.DesativarEstudante();
                    await context.SaveChangesAsync(ct);
                    return Results.Ok();

                   
                });
        }

        

    }
}
