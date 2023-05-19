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

        //[HttpGet("/hello/{msg}")]
        //public string HelloApi(string msg)
        //{
        //    return "Hello Api Message: " + msg;
        //}

        //Cadastro
        [HttpPost]
        public async Task<ActionResult<int>> Post(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto.Id;
        }

        [HttpGet]
        public async Task<ActionResult<List<Produto>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProdutos(int id)
        {
            Produto produto;
            produto= await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null)
                return NotFound();
            
            return produto;
        }


    }
}
