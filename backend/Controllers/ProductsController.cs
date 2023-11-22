using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        IWebHostEnvironment webHostEnvironment;
        private readonly DataContext context;
        public ProductsController(DataContext _context, IWebHostEnvironment _webHostEnvironment)
        {
            context = _context;
            webHostEnvironment = _webHostEnvironment;
        }

        [HttpPost("regProd")]
        public IActionResult RegisterProduct([FromForm] ProductDto request)
        {
            
            // Obtener la extensión del archivo
            string fileExtension = Path.GetExtension(request.FileImage.FileName);

            // Generar un nombre único basado en la hora actual
            string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";

            // Ruta donde se guardarán las imágenes
            string imgFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");

            // Combinar la ruta con el nombre único del archivo
            string filePath = Path.Combine(imgFolder, uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                request.FileImage.CopyTo(stream);
            }

            var regProd = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                ImageName = uniqueFileName
            };

            context.Products.Add(regProd);
            context.SaveChanges();

            return Ok(regProd);
        }



        [HttpGet("getProd")]
        public IActionResult getAllProducts()
        {
            var products = context.Products.ToList();
            return Ok(products);
        }

        [HttpPut("{editid}")]
        public IActionResult editProduct(int editid, [FromForm]ProductDto request)
        {
            var Product = context.Products.Find(editid);
            Product!.ImageName = request.Name;
            Product!.Price = request.Price;
            Product!.Description = request.Description;
            
            
            string fileExtension = Path.GetExtension(request.FileImage.FileName);

            // Generar un nombre único basado en la hora actual
            string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";

            // Ruta donde se guardarán las imágenes
            string imgFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");

            // Combinar la ruta con el nombre único del archivo
            string filePath = Path.Combine(imgFolder, uniqueFileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                request.FileImage.CopyTo(stream);
            }
            

            
            Product!.ImageName = uniqueFileName;
            
            context.SaveChanges();

            return Ok(Product);
        }

        [HttpDelete("{deleteid}")]
        public IActionResult deleteProduct(int deleteid)
        {
            context.Products.Find(deleteid);
            context.SaveChanges();

            var response = new {
                mensaje="El producto se ha eliminado del sistema"
            };

            return Ok(response);
        }
    }
}