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

        [HttpPost]
        public async Task<ActionResult<int>> Post(Aluno aluno)
        {
            if (String.IsNullOrEmpty(aluno.Nome) || aluno.Nome.Length <2)
                return BadRequest("Nome invalido!");

  

            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
            return aluno.Id;
        }

        [HttpGet]
        public async Task<ActionResult<List<Aluno>>> Get()
        {
            List<Aluno> listAlunos;
            listAlunos = await _context.Alunos.ToListAsync();
            return listAlunos;
        }

        [HttpGet("alfabeto/{strBegin}")]
        public async Task<ActionResult<List<Aluno>>> GetByAlfabeto(string strBegin)
        {
            List<Aluno> listAlunos;
            listAlunos = 
                await _context.Alunos.AsQueryable()
                .Where( a => a.Nome.StartsWith(strBegin) ).ToListAsync();
            return listAlunos;
        }

    }
}
