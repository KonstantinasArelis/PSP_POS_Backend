using Microsoft.EntityFrameworkCore;
using PSPBackend.Dto;
using PSPBackend.Model;
using PSPBackend.Repository;

namespace PSPBackend.Service;

public class MenuService
{
    private readonly MenuRepository _menuRepository;

    public MenuService(MenuRepository menuRepository)
    {
        _menuRepository = menuRepository;

    }

    public ProductModel? GetProduct(int productId)
    {

        Console.WriteLine("LOG: Product service GetProduct");
        var product = _menuRepository.GetProduct(productId);
        if (product is null)
        {
            return null;
        }
        else
        {
            return product;
        }
    }

    public ProductModel? CreateProduct(ProductModel product)
    {
        Console.WriteLine("LOG: MenuService: CreateProduct");
        if (product.Id == 0)
        {
            product.Id = _menuRepository.GetNewProductId();
        }

        if (_menuRepository.CreateProduct(product) > 0)
        {
            return product;
        }
        else
        {
            return null;
        }
    }

    public List<ProductModel> GetProducts(ProductGetDto arguments)
    {
        Console.WriteLine("LOG: Product Service: GetProducts");
        var query = _menuRepository.GetProducts(arguments);
        var products = query.ToList();
        return products;
    }

    public int DeleteProduct(int productId)
    {
        Console.WriteLine("LOG: Product Service: DeleteProduct");
        try
        {
            _menuRepository.DeleteProduct(productId);
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }
    public ProductModel? UpdateProduct(int productId, ProductModel product)
    {
        Console.WriteLine("LOG: Product Service: UpdateProduct");

        if (_menuRepository.UpdateProduct(productId, product) > 0)
        {
            return product;
        }
        else
        {
            return null;
        }
    }
}
