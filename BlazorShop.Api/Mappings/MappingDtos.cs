using BlazorShop.Api.Entities;
using BlazorShop.Models.DTOs;
using System.Runtime.CompilerServices;

namespace BlazorShop.Api.Mappings;

public static class MappingDtos
{
    public static IEnumerable<CategoriaDto> ConverterCategoriasParaDto(
        this IEnumerable<Categoria> categorias)
    {
        return categorias.Select(c => new CategoriaDto
        {
            Id = c.Id,
            Nome = c.Nome,
            IconCSS = c.IconCSS
        }).ToList();
    }

    public static IEnumerable<ProdutoDto> ConverterProdutosParaDto(
                                        this IEnumerable<Produto> produtos)
    {
        return produtos.Select(p => new ProdutoDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            ImagemUrl = p.ImagemUrl,
            Preco = p.Preco,
            Quantidade = p.Quantidade,
            CategoriaId = p.Categoria.Id,
            CategoriaNome = p.Categoria.Nome
        }).ToList();
    }

    public static ProdutoDto ConverterProdutoParaDto(this Produto produto)
    {
        return new ProdutoDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            ImagemUrl = produto.ImagemUrl,
            Preco = produto.Preco,
            Quantidade = produto.Quantidade,
            CategoriaId = produto.Categoria.Id,
            CategoriaNome = produto.Categoria.Nome
        };
    }

    public static IEnumerable<CarrinhoItemDto> ConverterCarrinhoItensParaDto(
       this IEnumerable<CarrinhoItem> carrinhoItens, IEnumerable<Produto> produtos)
    {

        return (from carrinhoItem in carrinhoItens
                join produto in produtos
                on carrinhoItem.ProdutoId equals produto.Id
                select new CarrinhoItemDto
                {
                    Id = carrinhoItem.Id,
                    ProdutoId = carrinhoItem.ProdutoId,
                    ProdutoNome = produto.Nome,
                    ProdutoDescricao = produto.Descricao,
                    ProdutoImagemURL = produto.ImagemUrl,
                    Preco = produto.Preco,
                    CarrinhoId = carrinhoItem.CarrinhoId,
                    Quantidade = carrinhoItem.Quantidade,
                    PrecoTotal = produto.Preco * carrinhoItem.Quantidade
                }).ToList();
    }

    public static CarrinhoItemDto ConverterCarrinhoItemParaDto(
        this CarrinhoItem carrinhoItem, Produto produto)
    {
        return new CarrinhoItemDto
        {
            Id = carrinhoItem.Id,
            ProdutoId = carrinhoItem.ProdutoId,
            ProdutoNome = produto.Nome,
            ProdutoDescricao = produto.Descricao,
            ProdutoImagemURL = produto.ImagemUrl,
            Preco = produto.Preco,
            CarrinhoId = carrinhoItem.CarrinhoId,
            Quantidade = carrinhoItem.Quantidade,
            PrecoTotal = produto.Preco * carrinhoItem.Quantidade
        };
    }
}
