using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [HttpPost]
    public IActionResult CreateProduct([FromBody] ProductModel product)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("LOG: CreateProduct invalid model");
            return BadRequest();
        }
        else
        {
            Console.WriteLine("LOG: MenuController: CreateProduct");

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

        return Ok(result);
    }
}
