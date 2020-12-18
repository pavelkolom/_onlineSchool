using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using HTTPMediaPlayerCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RobokassaLibCore;

namespace HTTPMediaPlayerCore.Controllers
{
  public class InvoicesController : Controller
  {
    private readonly DuwaysContext _context;
    private readonly IConfiguration _configuration;
    public InvoicesController(DuwaysContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
    }

    [ServiceFilter(typeof(AuthorCheck))]
    // GET: Invoices
    public async Task<IActionResult> Index()
    {
      int? userId = HttpContext.Session.GetString("UserID") != null ? Convert.ToInt32(HttpContext.Session.GetString("UserID")) : null;

      if (userId != null)
      {
        var duwaysContext = _context.Order.Where(i => i.CourseId == null && i.CreatedByUserId == userId).OrderByDescending(d => d.PaymentDateTime);
        return View(await duwaysContext.ToListAsync());
      }
      else
        return RedirectToAction("Index", "Dashboard");
    }

    // GET: Invoices/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var invoice = await _context.Order
          .Include(i => i.User)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (invoice == null)
      {
        return NotFound();
      }

      return View(invoice);
    }

    // GET: Invoices/Create
    public IActionResult Create()
    {
      ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
      return View();
    }

    // POST: Invoices/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Text,CreationDateTime,IsPaid,PaymentDateTime,ResultResponse,Sum,IncSum,Fee,SignatureValue,PaymentMethod,IsTest,EMail,UserId,CreatedByUserId")] Order invoice)
    {
      if (ModelState.IsValid)
      {
        //added
        string myUserId = HttpContext.Session.GetString("UserID");
        invoice.IsPaid = false;
        invoice.CreationDateTime = DateTime.Now;
        invoice.CreatedByUserId = Convert.ToInt32(myUserId);
        //added
        _context.Add(invoice);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", invoice.UserId);
      return View(invoice);
    }

    // GET: Invoices/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var invoice = await _context.Order.FindAsync(id);
      if (invoice == null)
      {
        return NotFound();
      }
      ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", invoice.UserId);
      return View(invoice);
    }

    // GET: Invoices/Pay/5
    public async Task<IActionResult> Pay(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var invoice = await _context.Order.FindAsync(id);
      if (invoice == null)
      {
        return NotFound();
      }

      Robokassa rk = new Robokassa(_configuration);

      string url = rk.GetRedirectUrl((int)invoice.Sum, invoice.Id, 0, invoice.Text, 0, 0, "0", "");
      return Redirect(url);

      //ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", invoice.UserId);
      //return View(invoice);
    }

    // POST: Invoices/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Text,CreationDateTime,IsPaid,PaymentDateTime,ResultResponse,Sum,IncSum,Fee,SignatureValue,PaymentMethod,IsTest,EMail,UserId,CreatedByUserId")] Order invoice)
    {
      if (id != invoice.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(invoice);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!InvoiceExists(invoice.Id))
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
      ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", invoice.UserId);
      return View(invoice);
    }

    // GET: Invoices/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var invoice = await _context.Order
          .Include(i => i.User)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (invoice == null)
      {
        return NotFound();
      }

      return View(invoice);
    }

    // POST: Invoices/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var invoice = await _context.Order.FindAsync(id);
      _context.Order.Remove(invoice);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool InvoiceExists(int id)
    {
      return _context.Order.Any(e => e.Id == id);
    }
  }
}
