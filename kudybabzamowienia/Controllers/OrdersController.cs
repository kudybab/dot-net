using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kudybabzamowienia.Models;

namespace kudybabzamowienia.Controllers
{
    public class OrdersController : Controller
    {
        private readonly kudybabBAZAContext _context;

        public OrdersController(kudybabBAZAContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var kudybabBAZAContext = _context.Orders.Include(o => o.Client).Include(pg=>pg.ProductOrders).ThenInclude(p=>p.Product).OrderByDescending(o=>o.Data);
            return View(await kudybabBAZAContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Nazwa_firmy");
            GetProductList();
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Kwota,Zrealizowane,ClientId")] Order order)
        {
            if (ModelState.IsValid)
            {
                var lista = HttpContext.Request.Form["selectedProducts"];
                _context.Add(order);
                await _context.SaveChangesAsync();
                var money = 0.0;
                foreach(var l in lista)
                {
                    var pg = new ProductOrder();
                    var szt = HttpContext.Request.Form[l];
                    pg.ProductId = int.Parse(l);
                    pg.ilosc_sztuk = int.Parse(szt);
                    var p = _context.Products.Where(p => p.Id == pg.ProductId).First();
                    var cena = p.Cena;
                    money += cena * Int32.Parse(szt);
                    pg.OrderId = order.Id;
                    if (pg.ilosc_sztuk != 0)
                        _context.Add(pg);
                    await _context.SaveChangesAsync();
                }
                order.Kwota = money;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Nazwa_firmy", order.ClientId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            GetSeletedProductList(order);
            GetProductOrderList(order);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Nazwa_firmy", order.ClientId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Kwota,Zrealizowane,ClientId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach(var poold in _context.ProductOrders.Where(po => po.OrderId == order.Id))
                    {
                        _context.Remove(poold);
                    }
                    var lista = HttpContext.Request.Form["selectedProducts"];
                    var money = 0.0;
                    foreach (var l in lista)
                    {
                        var pg = new ProductOrder();
                        var szt = HttpContext.Request.Form[l];
                        pg.ProductId = int.Parse(l);
                        pg.ilosc_sztuk = int.Parse(szt);
                        var p = _context.Products.Where(p => p.Id == pg.ProductId).First();
                        var cena = p.Cena;
                        money += cena * Int32.Parse(szt);
                        pg.OrderId = order.Id;
                        if(pg.ilosc_sztuk!=0)
                            _context.Add(pg);
                        await _context.SaveChangesAsync();
                    }
                    _context.Update(order);
                    order.Kwota = money;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Nazwa_firmy", order.ClientId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private void GetProductList()
        {
            var allproducts = _context.Products;
            var Produkty = new List<P>();
            foreach (var p in allproducts)
            {
                Produkty.Add(new P
                {
                    ProductId = p.Id,
                    Nazwa = p.Nazwa,
                    Cena=p.Cena,
                    Checked=""

                });
            }
            ViewData["products"] = Produkty;
        }
        private void GetSeletedProductList(Order order)
        {
            var allproducts = _context.Products;
            var selectproducts = _context.ProductOrders.Where(po => po.OrderId == order.Id).ToList();
            var Produkty = new List<P>();
            foreach (var p in allproducts)
            {
                Produkty.Add(new P
                {
                    ProductId = p.Id,
                    Nazwa = p.Nazwa,
                    Cena = p.Cena,
                    Checked = ""

                });
            }
            foreach(var p in Produkty)
            {
                if (selectproducts.Exists(po => po.ProductId == p.ProductId))
                {
                    p.Checked = "checked";
                }
            }
            ViewData["products"] = Produkty;

        }
        private void GetProductOrderList(Order order)
        {
            ViewData["productorders"] = _context.ProductOrders.Where(po => po.OrderId == order.Id).ToList();  
        }
    }
}
