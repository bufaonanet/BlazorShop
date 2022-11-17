using BlazorShop.Api.Context;
using BlazorShop.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Api.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext dbContext)
    {
        _context = dbContext;
    }    

    public async Task<Produto> GetItem(int id)
    {
        var produto = await _context.Produtos
                 .Include(c => c.Categoria)
                 .SingleOrDefaultAsync(c => c.Id == id);

        return produto;
    }

    public async Task<IEnumerable<Produto>> GetItens()
    {
        var produtos = await _context.Produtos
                              .Include(p => p.Categoria)
                              .ToListAsync();
        return produtos;
    }

    public async Task<IEnumerable<Produto>> GetItensPorCategoria(int id)
    {
        var produtos = await _context.Produtos
                              .Where(p => p.CategoriaId == id)
                              .Include(p => p.Categoria)
                              .ToListAsync();
        return produtos;
    }

    public async Task<IEnumerable<Categoria>> GetCategorias()
    {
        var categorias = await _context
            .Categorias
            .ToListAsync();
        return categorias;
    }
}
