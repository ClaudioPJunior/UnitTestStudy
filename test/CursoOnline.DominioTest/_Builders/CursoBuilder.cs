using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest.Cursos;

namespace CursoOnline.DominioTest._Builders
{
    public class CursoBuilder
    {
        private string _nome = "Informática Básica";
        private double _cargaHoraria = 80;
        private PublicoAlvo _publicoAlgo = PublicoAlvo.Estudante;
        private double _valor = 950;
        private string _descricao = "Descrição";
        
        public static CursoBuilder New()
        {
            return new CursoBuilder();
        }

        public CursoBuilder WithNome(string nome)
        {
            _nome = nome;
            return this;
        }

        public CursoBuilder WithDescricao(string descricao)
        {
            _descricao = descricao;
            return this;
        }

        public CursoBuilder WithCargaHoraria(double cargaHoraria)
        {
            _cargaHoraria = cargaHoraria;
            return this;
        }
        public CursoBuilder WithValor(double valor)
        {
            _valor = valor;
            return this;
        }

        public CursoBuilder WithPublicAlvo(PublicoAlvo publicoAlvo)
        {
            _publicoAlgo = publicoAlvo;
            return this;
        }

        public Curso Build()
        {
            return new Curso(_nome, _descricao, _cargaHoraria, _publicoAlgo, _valor);
        }
    }
}
