using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoanLogics;

namespace LoanLogics.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class ProductsController : Controller
  {
    private readonly AdventureWorks2014Context _context;

    public ProductsController(AdventureWorks2014Context context)
    {
      _context = context;
    }

    // GET: api/Products
    [HttpGet]
    public IEnumerable<Product> GetProduct()
    {
      return _context.Product;
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct([FromRoute] int id)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var product = await _context.Product.FindAsync(id);

      if (product == null)
      {
        return NotFound();
      }

      return Ok(product);
    }


    [HttpGet, Route("Search")]
    public IActionResult GetProductIds(string ids)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var products = ProductResult.ToProductResult(ids.Split(',', StringSplitOptions.None));




      var results = _context.Product.Where(p => products.Any(pr => pr.productId == p.ProductId))
        .Select(f => new ProductResult()
        {
          exists = true,
          averageStandardCost = f.ProductCostHistory.Count > 0 ? f.ProductCostHistory.GroupBy(g => g.StartDate.Year).Select(ch => new ASC()
          {
            year = ch.First().StartDate.Year,
            average = ch.Average(avg => avg.StandardCost).ToString("C")
          }) : (IEnumerable<ASC>)new List<ASC>(),
          productName = f.Name,
          productNumber = f.ProductNumber,
          productId = f.ProductId,
          locationName = f.ProductInventory.Count > 0 ? f.ProductInventory.OrderByDescending(q => q.Quantity).First().Location.Name : "No Inventory"
        })
        .Concat(products.Where(p => !_context.Product.Any(pro => pro.ProductId == p.productId)));
        




      /*
      var results = products.GroupBy(k => k.exists = _context.Product.Any(p => p.ProductId == k.productId))
        .SelectMany(g => g.Select(m => new ProductResult()
        {
          exists = m.exists,
          averageStandardCost =
          _context.Product.First(f => f.ProductId == m.productId).ProductCostHistory.Count > 0 ? _context.Product.First(f => f.ProductId == m.productId).ProductCostHistory.GroupBy(y => y.StartDate.Year).Select(ch => new ASC()
          {
            year = ch.First().StartDate.Year,
            average = ch.Average(avg => avg.StandardCost).ToString("C")
          }) : m.averageStandardCost,
          productName = _context.Product.First(f => f.ProductId == m.productId).Name,
          productNumber = _context.Product.First(f => f.ProductId == m.productId).ProductNumber,
          productId = _context.Product.First(f => f.ProductId == m.productId).ProductId,
          locationName = _context.Product.First(f => f.ProductId == m.productId).ProductInventory.Count > 0 ? _context.Product.First(f => f.ProductId == m.productId).ProductInventory.OrderByDescending(q => q.Quantity).First().Location.Name : "No Inventory"
        }));
        */





      /*

      var results = products.GroupJoin(
        _context.Product.Where(p => products.Any(pr => pr.productId == p.ProductId)),
        o => o.productId,
        i => i.ProductId,
                (nf, fg) => fg.Select(f => new ProductResult()
                {
                  exists = true,
                  averageStandardCost = f.ProductCostHistory.Count > 0 ? f.ProductCostHistory.GroupBy(g => g.StartDate.Year).Select(ch => new ASC()
                  {
                    year = ch.First().StartDate.Year,
                    average = ch.Average(avg => avg.StandardCost).ToString("C")
                  }) : nf.averageStandardCost,
                  productName = f.Name,
                  productNumber = f.ProductNumber,
                  productId = f.ProductId,
                  locationName = f.ProductInventory.Count > 0 ? f.ProductInventory.OrderByDescending(q => q.Quantity).First().Location.Name : "No Inventory"
                })
                .DefaultIfEmpty(new ProductResult()
                {
                  exists = false,
                  averageStandardCost = (IEnumerable<ASC>)new List<ASC>(),
                  productName = "",
                  productNumber = "",
                  productId = nf.productId,
                  locationName = ""
                }))
                    .SelectMany(g => g);

  */


      if (results == null)
      {
        return NotFound();
      }

      return Ok(results);
    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != product.ProductId)
      {
        return BadRequest();
      }

      _context.Entry(product).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ProductExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Products
    [HttpPost]
    public async Task<IActionResult> PostProduct([FromBody] Product product)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      _context.Product.Add(product);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var product = await _context.Product.FindAsync(id);
      if (product == null)
      {
        return NotFound();
      }

      _context.Product.Remove(product);
      await _context.SaveChangesAsync();

      return Ok(product);
    }

    private bool ProductExists(int id)
    {
      return _context.Product.Any(e => e.ProductId == id);
    }
  }


  public class ASC
  {
    public int year;
    public string average;
  }


  public class ProductResult
  {
    public bool exists;
    public IEnumerable<ASC> averageStandardCost;
    public string productName;
    public string productNumber;
    public int productId;
    public string locationName;


    public static IQueryable<ProductResult> ToProductResult(string[] ids)
    {
      var results = new List<ProductResult>();


      foreach (var i in ids)
      {
        results.Add(new ProductResult()
        {
          exists = false,
          averageStandardCost = (IEnumerable<ASC>)new List<ASC>(),
          productName = "",
          productNumber = "",
          productId = Int32.Parse(i),
          locationName = ""
        });
      }
      return results.AsQueryable();

    }
  }

}
