using BlazorShop.Api.Mappings;
using BlazorShop.Api.Repositories;
using BlazorShop.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlazorShop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarrinhoCompraController : ControllerBase
{
    private readonly ICarrinhoCompraRepository _carrinhoCompraRepo;
    private readonly IProdutoRepository _produtoRepo;
    private ILogger<CarrinhoCompraController> _logger;

    public CarrinhoCompraController(
        ICarrinhoCompraRepository carrinhoCompraRepo,
        IProdutoRepository produtoRepo,
        ILogger<CarrinhoCompraController> logger)
    {
        _carrinhoCompraRepo = carrinhoCompraRepo;
        _produtoRepo = produtoRepo;
        _logger = logger;
    }

    [HttpGet]
    [Route("{usuarioId}/GetItens")]
    public async Task<ActionResult<IEnumerable<CarrinhoItemDto>>> GetItens(int usuarioId)
    {
        try
        {
            var carrinhoItens = await _carrinhoCompraRepo.GetItensByUsuario(usuarioId);
            if (carrinhoItens == null)
            {
                return NoContent(); // 204 Status Code
            }

            var produtos = await _produtoRepo.GetItens();
            if (produtos == null)
            {
                throw new Exception("Não existem produtos...");
            }

            var carrinhoItensDto = carrinhoItens.ConverterCarrinhoItensParaDto(produtos);
            return Ok(carrinhoItensDto);
        }
        catch (Exception ex)
        {
            _logger.LogError("## Erro ao obter itens do carrinho");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<CarrinhoItemDto>> GetItem(int id)
    {
        try
        {
            var carrinhoItem = await _carrinhoCompraRepo.GetItem(id);
            if (carrinhoItem == null)
            {
                return NotFound($"Item não encontrado"); //404 status code
            }

            var produto = await _produtoRepo.GetItem(carrinhoItem.ProdutoId);
            if (produto == null)
            {
                return NotFound($"Item não existe na fonte de dados");
            }

            var cartItemDto = carrinhoItem.ConverterCarrinhoItemParaDto(produto);
            return Ok(cartItemDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"## Erro ao obter o item ={id} do carrinho");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CarrinhoItemDto>> PostItem([FromBody]
    CarrinhoItemAdicionaDto carrinhoItemAdicionaDto)
    {
        try
        {
            var novoCarrinhoItem = await _carrinhoCompraRepo.AdicionaItem(carrinhoItemAdicionaDto);
            if (novoCarrinhoItem == null)
            {
                return NoContent(); //Status 204
            }

            var produto = await _produtoRepo.GetItem(novoCarrinhoItem.ProdutoId);
            if (produto == null)
            {
                throw new Exception($"Produto não localizado (Id:({carrinhoItemAdicionaDto.ProdutoId})");
            }

            var novoCarrinhoItemDto = novoCarrinhoItem.ConverterCarrinhoItemParaDto(produto);

            return CreatedAtAction(nameof(GetItem), new { id = novoCarrinhoItemDto.Id },
                novoCarrinhoItemDto);
        }
        catch (Exception ex)
        {
            _logger.LogError("## Erro criar um novo item no carrinho");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CarrinhoItemDto>> DeleteItem(int id)
    {
        try
        {
            var carrinhoItem = await _carrinhoCompraRepo.DeletaItem(id);
            if (carrinhoItem is null)
            {
                return NotFound();
            }

            var produto = await _produtoRepo.GetItem(carrinhoItem.ProdutoId);
            if (produto is null)
                return NotFound();

            var carrinhoItemDto = carrinhoItem.ConverterCarrinhoItemParaDto(produto);
            return Ok(carrinhoItemDto);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<CarrinhoItemDto>> AtualizaQuantidade(int id,
        CarrinhoItemAtualizaQuantidadeDto carrinhoItemAtualizaQuantidadeDto)
    {
        try
        {
            var carrinhoItem = await _carrinhoCompraRepo.AtualizaQuantidade(id, carrinhoItemAtualizaQuantidadeDto);
            if (carrinhoItem is null)
            {
                return NotFound();
            }

            var produto = await _produtoRepo.GetItem(carrinhoItem.ProdutoId);
            var carrinhoItemDto = carrinhoItem.ConverterCarrinhoItemParaDto(produto);
            return Ok(carrinhoItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}
