using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;
using PSPBackend.Repository;

namespace PSPBackend.Service
{
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
    }
}
