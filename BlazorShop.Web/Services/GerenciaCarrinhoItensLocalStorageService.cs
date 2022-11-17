using Blazored.LocalStorage;
using BlazorShop.Models.DTOs;

namespace BlazorShop.Web.Services;

public class GerenciaCarrinhoItensLocalStorageService : IGerenciaCarrinhoItensLocalStorageService
{
    const string key = "CarrinhoItemCollection";

    private readonly ILocalStorageService _localStorageService;
    private readonly ICarrinhoCompraService _carrinhoCompraService;

    public GerenciaCarrinhoItensLocalStorageService(
        ILocalStorageService localStorageService, 
        ICarrinhoCompraService carrinhoCompraService)
    {
        _localStorageService = localStorageService;
        _carrinhoCompraService = carrinhoCompraService;
    }

    public async Task<List<CarrinhoItemDto>> GetCollection()
    {
        return await _localStorageService.GetItemAsync<List<CarrinhoItemDto>>(key)
              ?? await AddCollection();
    }

    public async Task RemoveCollection()
    {
        await _localStorageService.RemoveItemAsync(key);
    }

    public async Task SaveCollection(List<CarrinhoItemDto> carrinhoItensDto)
    {
        await _localStorageService.SetItemAsync(key, carrinhoItensDto);
    }

    private async Task<List<CarrinhoItemDto>> AddCollection()
    {
        var carrinhoCompraCollection = await _carrinhoCompraService
                                          .GetItensAsync(UsuarioLogado.UsuarioId);

        if (carrinhoCompraCollection != null)
        {
            await _localStorageService.SetItemAsync(key, carrinhoCompraCollection);
        }
        return carrinhoCompraCollection;
    }
}