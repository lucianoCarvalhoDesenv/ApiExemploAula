using ApiAula.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAula.Controllers
{
    [ApiController]

    [Route("teste/aula")]

    public class ProdutoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProdutoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("hello/{msg}")]
        public string HelloApi(string msg)
        {
            return "Hello Api Message: " + msg;
        }

        //Cadastro
        [HttpPost]
        public async Task<ActionResult<int>> Post(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto.Id;
        }

        //Leitura todos Elementos da Tabela do Banco
        [HttpGet]
        public async Task<ActionResult<List<Produto>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        //Leitura de um Elmento do banco dados pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProdutos(int id)
        {
            Produto produto;
            produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null)
                return NotFound();

            return produto;
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            Produto produto;
            produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            //Se nao for encontrado -> sinaliza elemento nao encontrado
            if (produto == null)
                return NotFound();

            //se for remove o elemento
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Atualizar Conteudo no SGBD
        [HttpPut]
        public async Task<ActionResult> Atualizar(Produto produto)
        {
            _context.Attach(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
