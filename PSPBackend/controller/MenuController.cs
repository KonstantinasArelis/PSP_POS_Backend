using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSPBackend.Dto;
using PSPBackend.Model;
using PSPBackend.Service;
using System.ComponentModel.DataAnnotations;


[ApiController]
[Route("[controller]")]
public class MenuController : ControllerBase
{
    private readonly MenuService _menuService;

    public MenuController(MenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("LOG: GetProductById invalid model");
            return BadRequest();
        }

        ProductModel? result;
        try
        {
            result = _menuService.GetProduct(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        Console.WriteLine("LOG: MenuController: GetProductById");
        return Ok(result);
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] ProductModel product)
    {
        Console.WriteLine("LOG: MenuController: CreateProduct");
        if (!ModelState.IsValid)
        {
            Console.WriteLine("LOG: CreateProduct invalid model");
            return BadRequest();
        }
        else
        {
            ProductModel? result;
            try
            {
                result = _menuService.CreateProduct(product);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }

            return Ok(result);
        }


    }


    [HttpGet]
    public IActionResult GetProducts([FromQuery] ProductGetDto dto)
    {
        Console.WriteLine("LOG: MenuController: GetProducts");
        var products = _menuService.GetProducts(dto).ToList();

        return Ok(products);
    }

    [HttpDelete]
    [Route("{productId}")]
    public IActionResult DeleteProduct(int productId)
    {
        Console.WriteLine("LOG: MenuController: DeleteProduct");
        try
        {
            _menuService.DeleteProduct(productId);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPatch]
    [Route("{productId}")]
    public IActionResult UpdateProduct(int productId, [FromBody] ProductModel updateProduct)
    {
        Console.WriteLine("LOG: MenuController: UpdateProduct");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("LOG: UpdateProduct: invalid model");
            return BadRequest("Invalid data provided.");
        }
        try
        {
            var result = _menuService.UpdateProduct(productId, updateProduct);
            return Ok(result);
        }
        catch (Exception ex) {
            Console.WriteLine(ex.ToString());
            return NotFound($"Product with Id {productId} not found.");
        }
    }
}
