using Blazored.LocalStorage;
using BlazorShop.Models.DTOs;

namespace BlazorShop.Web.Services;

public class GerenciaProdutosLocalStorageService : IGerenciaProdutosLocalStorageService
{
    const string key = "ProdutoCollection";

    private readonly ILocalStorageService _localStorageService;
    private readonly IProdutoService _produtoService;

    public GerenciaProdutosLocalStorageService(
        ILocalStorageService localStorageService,
        IProdutoService produtoService)
    {
        _localStorageService = localStorageService;
        _produtoService = produtoService;
    }

    public async Task<IEnumerable<ProdutoDto>> GetCollection()
    {
        return await _localStorageService
            .GetItemAsync<IEnumerable<ProdutoDto>>(key) ?? await AddCollection();
    }

    public async Task RemoveCollection()
    {
        await _localStorageService.RemoveItemAsync(key);
    }

    private async Task<IEnumerable<ProdutoDto>> AddCollection()
    {
        var produtoCollection = await _produtoService.GetItens();
        if (produtoCollection != null)
        {
            await _localStorageService.SetItemAsync(key, produtoCollection);
        }
        return produtoCollection;
    }
}