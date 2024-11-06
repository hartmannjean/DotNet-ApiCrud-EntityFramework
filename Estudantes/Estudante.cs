namespace ApiCrud.Estudantes {
    public class Estudante {
        public Guid Id { get; init; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }

        public Estudante(string nome)
        {
            Id = Guid.NewGuid();
            Ativo = true;
            Nome = nome;
        }

        public void AtualizarNome(string nome)
        {
            Nome = nome;
        }

        public void DesativarEstudante()
        {
            Ativo = false;
        }

    }
}
