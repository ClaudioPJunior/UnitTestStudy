using Bogus;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Util;
using Moq;
using Xunit;

namespace CursoOnline.DominioTest.Cursos
{
    public class ArmazenadorDeCursoTest
    {
        private readonly CursoDTO _cursoDTO;
        private readonly ArmazenadorDeCurso _armazenadorDeCurso;
        private readonly Mock<ICursoRepository> _cursoRepository;
        
        
        public ArmazenadorDeCursoTest ()
        {
            var faker = new Faker();

            _cursoDTO = new CursoDTO
            {
                Nome = faker.Random.Words(),
                Descricao = faker.Lorem.Paragraph(),
                CargaHoraria = faker.Random.Double(1,100),
                Valor = faker.Random.Double(1, 1000),
                PublicoAlvo = "Estudante"
            };

            _cursoRepository = new Mock<ICursoRepository>(); // MOCK serve para gente simular comportamentos(Métodos)

            _armazenadorDeCurso = new ArmazenadorDeCurso(_cursoRepository.Object); // ArmazenadorDeCurso é minha classe de Domain Service basicamento
                                                                                         // estou injetanndo de dento da classe o meu comportamento
                                                                                         // de adicionar sem da necessidade da implementação do método em si,
                                                                                         // somente validando o comportamento e não se está persistindo no banco ou não
        }

        [Fact]
        public void DeveAdicionarCurso()
        {
            _armazenadorDeCurso.Armazenar(_cursoDTO); // Aqui eu simulo a utilização da função, usando ela fora da camada de dominio, na Controller e na camada Presentation

            _cursoRepository.Verify(x => x.Adicionar(
                It.Is<Curso>( 
                    c => c.Nome == _cursoDTO.Nome &&
                    c.Descricao == _cursoDTO.Descricao
                    
                    )
                )); // Aqui estou verificando se o meu comportamento mockado foi chamado alguma vez e se o curso que esta sendo curso é um curso válido
        }

        [Fact]
        public void NaoDeveAdicionarCursoComMesmoNome()
        {
            var cursoJaSalvo = CursoBuilder.New().WithNome(_cursoDTO.Nome).Build();

            _cursoRepository.Setup(r => r.ObterPeloNome(_cursoDTO.Nome)).Returns(cursoJaSalvo);

            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDTO))
               .ComMensagem("Nome do curso já consta no banco de dados");
        }

        [Fact]
        public void NaoDeveAdicionarComPublicAlvoInvalido()
        {
            var publicAlvoInvalido = "Médico";
            _cursoDTO.PublicoAlvo = publicAlvoInvalido;

            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDTO))
                .ComMensagem("Público alvo inválido");
        }
    }

    public interface ICursoRepository
    {
        void Adicionar(Curso curso);
        Curso ObterPeloNome(string nome);
    }

    public class ArmazenadorDeCurso
    {
        private readonly ICursoRepository _cursoRepository;

        public ArmazenadorDeCurso(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public void Armazenar(CursoDTO cursoDTO)
        {
            var cursoJaSalvo = _cursoRepository.ObterPeloNome(cursoDTO.Nome);

            if (cursoJaSalvo != null)
                throw new ArgumentException("Nome do curso já consta no banco de dados");

            if (Enum.TryParse<PublicoAlvo>(cursoDTO.PublicoAlvo, out var publicoAlvo))
                throw new ArgumentException("Público alvo inválido");
            
            var curso = new Curso(cursoDTO.Nome, cursoDTO.Descricao, cursoDTO.CargaHoraria, publicoAlvo, cursoDTO.Valor);

            _cursoRepository.Adicionar(curso);
        }
    }

    public class CursoDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double CargaHoraria { get; set; }
        public double Valor { get; set; }
        public string PublicoAlvo { get; set; }
    }
}
