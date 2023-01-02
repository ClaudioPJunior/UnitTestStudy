using Bogus;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Util;
using ExpectedObjects;
using Xunit;
using Xunit.Abstractions;

namespace CursoOnline.DominioTest.Cursos
{
    public class CursoTest : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly string _nome;
        private readonly double _cargaHoraria;
        private readonly PublicoAlvo _publicoAlgo;
        private readonly double _valor;
        private readonly string _descricao;

        public CursoTest(ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine("Construtor sendo excecutado");
            var faker = new Faker();

            _nome = faker.Random.Word();
            _cargaHoraria = faker.Random.Double(50,1000);
            _publicoAlgo = PublicoAlvo.Estudante;
            _valor = faker.Random.Double(100,1000);
            _descricao = faker.Lorem.Paragraph();
        }

        public void Dispose()
        {
            _output.WriteLine("Disposable sendo excecutado");
        }

        [Fact(DisplayName = "DeveCriarCurso")]
        public void DeveCriarCurso()
        {
            //Organização
            var cursoEsperado = new
            {
                Nome = _nome,
                CargaHoraria = _cargaHoraria,
                PublicoAlvo = _publicoAlgo,
                Valor = _valor,
                Descricao = _descricao
            };

            //Ação
            var curso = new Curso(cursoEsperado.Nome, cursoEsperado.Descricao, cursoEsperado.CargaHoraria, cursoEsperado.PublicoAlvo, cursoEsperado.Valor);

            //Assert
            cursoEsperado.ToExpectedObject().ShouldMatch(curso);
        }

        [Theory(DisplayName = "NomeDeveCursoTerNomeInvalido")]
        [InlineData("")]
        [InlineData(null)]
        public void NomeDeveCursoTerNomeInvalido(string nomeInvalido)
        {
            //Assert
            Assert.Throws<ArgumentException>(() =>
                   CursoBuilder.New().WithNome(nomeInvalido).Build())
                  .ComMensagem("Nome inválido");

        }

        [Theory(DisplayName = "NomeDeveCursoTerCargaHorariaMenorQue1")]
        [InlineData(0)]
        [InlineData(-50)]
        [InlineData(-200)]
        public void NomeDeveCursoTerCargaHorariaMenorQue1(double cargaHoraria)
        {
            //Assert
            Assert.Throws<ArgumentException>(() =>
                   CursoBuilder.New().WithCargaHoraria(cargaHoraria).Build())
                  .ComMensagem("Carga horária inválida");
        }

        [Theory(DisplayName = "NomeDeveCursoTerValorMenorQue1")]
        [InlineData(0)]
        [InlineData(-9)]
        [InlineData(-100)]
        public void NomeDeveCursoTerValorMenorQue1(double valor)
        {
            //Assert
            Assert.Throws<ArgumentException>(() =>
                   CursoBuilder.New().WithValor(valor).Build())
                  .ComMensagem("Valor inválido");
        }
    }
}
