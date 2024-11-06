namespace ApiCrud.Professores {
    /*
        Comando para rodar toda vez que fizer uma alteração nos models:
        dotnet ef migrations add Initial e dotnet ef database update
        Sendo Initial o nome da operação, por exemplo poderia ser  dotnet ef migrations add ProfessoresAJuste
    */
    public class Professor {
        public Guid Id { get; init; }
        public string Nome { get; set; }
        public string Materia { get; set; }
        public double Salario { get; set; }
        public bool Ativo { get; set; }

        public Professor(string nome, string materia, double salario)
        {
            Id = Guid.NewGuid();
            Ativo = true;
            Nome = nome;
            Salario = salario;
            Materia = materia;
        }

        public void DesativarProfessor()
        {
            Ativo = false;
        }
    }
}
