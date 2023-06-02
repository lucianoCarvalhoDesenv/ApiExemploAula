using ApiAula.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAula.Controllers
{
    [ApiController]

    [Route("api/aula/aluno")]

    public class AlunoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AlunoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]//Verbo HTTP
        public async Task<ActionResult<int>> Post(Aluno aluno)
        {
            //Verifica se nome é vazio ou nulo e se numero de caracteres é > 2
            if (String.IsNullOrEmpty(aluno.Nome) || aluno.Nome.Length < 2)
                return BadRequest("Nome invalido!");



            _context.Alunos.Add(aluno); // insere aluno no DbSet
            await _context.SaveChangesAsync();// Aplica alterações no Banco de Dados (SGBD)
            return aluno.Id;
        }

        [HttpGet]//Verbo HTTP
        public async Task<ActionResult<List<Aluno>>> Get()
        {
            List<Aluno> listAlunos;
            listAlunos = await _context.Alunos.ToListAsync();// Realiza leitura da lista de aluno no BANCO
            return listAlunos;
        }

        //Leitura de um Elmento do banco dados pelo ID
        [HttpGet("{id}")]//Verbo HTTP com parametros 
        public async Task<ActionResult<Aluno>> GetProdutos(int id)
        {
            var aluno = await _context.Alunos.FirstOrDefaultAsync(p => p.Id == id); // busca aluno pelo ID
            if (aluno == null)
                return NotFound();//caso id do aluno se nao for encontrado 

            return aluno;// retorna aluno
        }

        [HttpDelete]//Verbo HTTP
        public async Task<ActionResult> Delete(int id)
        {
            var aluno = await _context.Alunos.FirstOrDefaultAsync(p => p.Id == id);// busca aluno pelo ID

            //Se nao for encontrado -> sinaliza elemento nao encontrado
            if (aluno == null)
                return NotFound();

            //se for remove o elemento
            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();// Aplica alterações no Banco de Dados (SGBD)

            return NoContent();
        }

        //Atualizar Conteudo no SGBD
        [HttpPut]//Verbo HTTP
        public async Task<ActionResult> Atualizar(Aluno aluno)
        {
            //adiciona versao alterada do aluno E sinaliza para o EntityFramework que Status do aluno é Modificado
            // é obrigatorio sinalizar o ' EntityState.Modified' para que o EntityFramework entenda que exista alteração a ser aplicada no Banco de dados
            _context.Attach(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();// Aplica alterações no Banco de Dados (SGBD)

            return NoContent();
        }

        [HttpGet("ordenadoPorNota")]//Verbo HTTP com parametro de rota
        public async Task<ActionResult<List<Aluno>>> GetOrdenado()
        {
            List<Aluno> listAlunos;
            listAlunos = await _context.Alunos.OrderBy(a => a.Nota).ToListAsync();
            return listAlunos;
        }

        [HttpGet("alfabeto/{strBegin}")]//Verbo HTTP com parametros 
        public async Task<ActionResult<List<Aluno>>> GetByAlfabeto(string strBegin)
        {
            List<Aluno> listAlunos;
            listAlunos =
                await _context.Alunos.AsQueryable()
                .Where(a => a.Nome.StartsWith(strBegin)).ToListAsync();
            return listAlunos;
        }

        [HttpGet("MaiorOuMenorQue/ordem/{maioroumenor}/{valor}")]//Verbo HTTP com parametros 
        public async Task<ActionResult<List<Aluno>>> GetByAlfabeto(float valor, string maioroumenor)
        {
            List<Aluno> listAlunos;
            //se opcao for MENOR <=
            if (maioroumenor == "<" || maioroumenor.ToUpper().Equals("MENOR"))
            {
                //pega lista de alunos ordenada pelo valor da nota
                listAlunos =
                               await _context.Alunos.AsQueryable()
                               .Where(a => a.Nota <= valor).ToListAsync();
            }
            else//se opcao for MAIOR >=
            {
                //pega lista de alunos ordenada pelo valor da nota
                listAlunos = await _context.Alunos.AsQueryable().Where(a => a.Nota >= valor).ToListAsync();
            }

            return listAlunos;
        }

        //Fechar semestre Aprovado nota >=6 falta < 4
        [HttpPut("fechamentoDeSemestre")]//Verbo HTTP com parametro de Rota
        public async Task<ActionResult<List<Aluno>>> FecharSemetres()
        {

            List<Aluno> listAlunos;
            listAlunos = await _context.Alunos.ToListAsync();// Realiza leitura da lista de aluno no BANCO

            foreach (Aluno aluno in listAlunos) //Itera por todos Alunos do SGBD
            {
                if ((aluno.Nota) >= 6 && (aluno.NumeroFaltas < 4))
                    aluno.EstaAprovado = true;
                else
                    aluno.EstaAprovado = false;

                _context.Attach(aluno).State = EntityState.Modified;//Marca Aluno como Alterado para o EntityFramework poder atualizar
            }
            await _context.SaveChangesAsync();// Aplica alterações no Banco de Dados (SGBD)


            return await _context.Alunos.ToListAsync(); 
        }




    }
}
