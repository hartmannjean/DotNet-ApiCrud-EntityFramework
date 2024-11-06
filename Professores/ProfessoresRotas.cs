﻿using ApiCrud.Data;
using ApiCrud.Estudantes;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Professores {

    public static class ProfessoresRotas {
        public static void AddRotasProfessores(this WebApplication app)
        {
            var rotasProfessor = app.MapGroup("professores");

            //Adicionar novo professor
            rotasProfessor.MapPost("", async (AddProfessorRequest request, AppDbContext context, CancellationToken ct) =>
            {
                var novoProfessor = new Professor(request.Nome, request.Materia, request.Salario);
                await context.AddAsync(novoProfessor, ct);
                await context.SaveChangesAsync(ct);

                var professorRetorno = new ProfessorDto(novoProfessor.Id, novoProfessor.Nome, novoProfessor.Materia, novoProfessor.Salario, novoProfessor.Ativo);
                return Results.Ok(professorRetorno);
            });

            //Pega todos os professores ativos
            rotasProfessor.MapGet("", async (AppDbContext context, CancellationToken ct) =>
            {
                return Results.Ok(await context.Professores
                .Where(professor => professor.Ativo)
                .Select(professor => new ProfessorDto(professor.Id, professor.Nome, professor.Materia, professor.Salario, professor.Ativo))
                .ToListAsync());
            });

            //Pega todos os professores por nome
            rotasProfessor.MapGet("nome/{name}", async (string name, AppDbContext context, CancellationToken ct) =>
            {
                /*
                    Aqui é como se eu estivesse fazendo em sql: SELECT Id, Nome, Material, Salario, Ativo FROM Professores WHERE Nome = name
                */
                return Results.Ok(await context.Professores
                .Where(professor => professor.Nome == name)
                .Select(professor => new ProfessorDto(professor.Id, professor.Nome, professor.Materia, professor.Salario, professor.Ativo))
                .ToListAsync());
            });
            //Pega todos os professores onde o nome que o usuário passar CONTÉM no nome do professor
            rotasProfessor.MapGet("contendo/{nameContains}", async (string nameContains, AppDbContext context, CancellationToken ct) =>
            {
                /*
                    Aqui é como se eu estivesse fazendo em sql: SELECT Id, Nome, Material, Salario, Ativo FROM Professores WHERE Nome = name
                */
                return Results.Ok(await context.Professores
                .Where(professor => professor.Nome.Contains(nameContains))
                .Select(professor => new ProfessorDto(professor.Id, professor.Nome, professor.Materia, professor.Salario, professor.Ativo))
                .ToListAsync());
            });
        }
    }
}