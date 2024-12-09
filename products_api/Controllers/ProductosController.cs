using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using products_api.Entities;
using products_api.Entities.DTOs;

namespace products_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : Controller
    {

        private readonly CrudContext _context;

        public ProductosController(CrudContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Producto>>> getAll()
        {
            List<Producto> products = new List<Producto>();
            try
            {
                var res = await _context.Productos.ToListAsync();
                if (res.Count > 0)
                {
                    products = res;
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor");
            }
        }

        [HttpGet("{id:int}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> getById(int id)
        {
            Producto producto = new Producto();

            try
            {
                var res = await _context.Productos.FirstOrDefaultAsync(x => x.ProductoId == id);

                if (res != null) producto = res;

                return Ok(producto);

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> create(ProductoDTO producto)
        {
            try
            {
                Producto newProduct = new Producto()
                {
                    Nombre = producto.Nombre,
                    Precio = producto.Precio
                };
                await _context.Productos.AddAsync(newProduct);
                _context.SaveChanges(); ;
                return StatusCode(StatusCodes.Status201Created, true);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor");
            }

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> edit(int id, [FromBody]ProductoDTO producto)
        {
            try
            {
                Producto oldProduct = await _context.Productos.FirstOrDefaultAsync(x => x.ProductoId == id);

                if (oldProduct != null)
                {
                    if(producto.Nombre != null)
                    {
                        oldProduct.Nombre = producto.Nombre;
                    }
                    if(producto.Precio != 0)
                    {
                        oldProduct.Precio = producto.Precio;
                    }
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, true);
                }
                return StatusCode(StatusCodes.Status400BadRequest, false);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor");
            }

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> delete(int id)
        {
            try
            {
                Producto product = await _context.Productos.FirstOrDefaultAsync(x => x.ProductoId == id);

                if (product != null)
                {
                    _context.Productos.Remove(product);
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, true);
                }
                return StatusCode(StatusCodes.Status400BadRequest, false);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor");
            }

        }

    }
}
