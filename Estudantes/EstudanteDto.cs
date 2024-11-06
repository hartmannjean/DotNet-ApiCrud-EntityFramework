namespace ApiCrud.Estudantes;
/*
 O DTO (Data Transfer Object) é um objeto que carrega dados entre processos.
    Ele é usado para minimizar o número de chamadas entre aplicativos e servidores.
    O DTO é um objeto simples que não contém lógica de negócios, mas apenas dados.
    Quando voce não quer expor a estrutura interna de seus objetos, você pode usar DTOs.
 */
public record EstudanteDto(Guid Id, string Nome);

