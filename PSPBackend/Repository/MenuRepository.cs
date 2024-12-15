using PSPBackend.Model;

namespace PSPBackend.Repository
{
    public class MenuRepository
    {
        private readonly AppDbContext _context;

        public MenuRepository(AppDbContext context)
        {
            _context = context;
        }

        public ProductModel? GetProduct(int productId)
        {
            Console.WriteLine("LOG: repository returns Product by id:" + productId);
            try
            {
                ProductModel? product = _context.Product.FirstOrDefault(d => d.Id == productId);
                return product;
            } 
            catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    return null;
            }
        }

        public int CreateProduct(ProductModel product)
        {
            Console.WriteLine("LOG: Repository creates Product with name: " + product.ProductName);

            _context.Product.Add(product);
            int rowsAffected = _context.SaveChanges();

            Console.WriteLine("LOG: Saved Product with name: " + product.ProductName);
            return rowsAffected;
        }

        public int GetNewProductId()
        {
            int id = _context.Product.Select(o => o.Id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
            if (id == 0) { id++; }
            return id;
        }
    }
}
