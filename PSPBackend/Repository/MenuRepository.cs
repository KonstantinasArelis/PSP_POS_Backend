using PSPBackend.Dto;
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
        public int UpdateProduct(int productId, ProductModel product)
        {
            Console.WriteLine("LOG: UpdateProduct with id: " + productId);
            ProductModel? updateProduct = GetProduct(productId);

            if (updateProduct != null)
            {
                updateProduct.ProductName = product.ProductName;
                updateProduct.BusinessId = product.BusinessId;
                updateProduct.TaxId = product.TaxId;
                updateProduct.Price = product.Price;
                updateProduct.ProductType = product.ProductType;
                updateProduct.IsForSale = product.IsForSale;
                updateProduct.CategoryId = product.CategoryId;
                updateProduct.CanDiscountBeApplied = product.CanDiscountBeApplied;
                updateProduct.StockQuantity = product.StockQuantity;
                updateProduct.Variations = product.Variations;

                int rowsAffected = _context.SaveChanges();
                return rowsAffected;
            }
            return 0;
        }

        public int DeleteProduct(int productId)
        {
            Console.WriteLine("LOG: DeleteProduct with id: " + productId);
            ProductModel? product = GetProduct(productId);
            if (product != null)
            {
                _context.Product.Remove(product);
                int rowsAffected = _context.SaveChanges();
                return rowsAffected;
            }
            return 0;
        }

        public IQueryable<ProductModel> GetProducts(ProductGetDto arguments)
        {
            var query = _context.Product.AsQueryable();

            if (arguments.business_id.HasValue)
                query = query.Where(p => p.BusinessId == arguments.business_id.Value);

            if (arguments.min_price.HasValue)
                query = query.Where(p => p.Price >= arguments.min_price.Value);

            if (arguments.max_price.HasValue)
                query = query.Where(p => p.Price <= arguments.max_price.Value);

            if (arguments.is_for_sale.HasValue)
                query = query.Where(p => p.IsForSale == arguments.is_for_sale.Value);

            if (arguments.tax_id.HasValue)
                query = query.Where(p => p.TaxId == arguments.tax_id.Value);

            if (arguments.category_id.HasValue)
                query = query.Where(p => p.CategoryId == arguments.category_id.Value);

            if (arguments.can_discount_be_applied.HasValue)
                query = query.Where(p => p.CanDiscountBeApplied == arguments.can_discount_be_applied.Value);

            if (!string.IsNullOrEmpty(arguments.product_type))
                query = query.Where(p => p.ProductType.Contains(arguments.product_type));

            if (!string.IsNullOrEmpty(arguments.variations))
                query = query.Where(p => p.Variations.Contains(arguments.variations));
            


            // Pagination
            if (arguments.page_nr.HasValue && arguments.limit.HasValue)
            {
                int skip = (arguments.page_nr.Value - 1) * arguments.limit.Value;
                query = query.Skip(skip).Take(arguments.limit.Value);
            }

            return query;
        }

    }
}
