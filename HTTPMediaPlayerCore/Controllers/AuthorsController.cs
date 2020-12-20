using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HTTPMediaPlayerCore.Models;

namespace HTTPMediaPlayerCore.Controllers
{
  public class AuthorsController : Controller
  {
    private readonly DuwaysContext _context;

    public AuthorsController(DuwaysContext context)
    {
      _context = context;
    }

    // GET: Authors
    [ServiceFilter(typeof(AdminCheck))]
    public async Task<IActionResult> Index()
    {
      var duwaysContext = _context.Author.Include(a => a.Filter).Include(a => a.User);
      return View(await duwaysContext.ToListAsync());
    }

    // GET: Authors/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var author = await _context.Author
          .Include(a => a.Filter)
          .Include(a => a.User)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (author == null)
      {
        return NotFound();
      }

      return View(author);
    }

    // GET: Authors/Create
    [ServiceFilter(typeof(AdminCheck))]
    public IActionResult Create()
    {
      ViewData["FilterId"] = new SelectList(_context.Filter, "Id", "Id");
      ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
      return View();
    }

    // POST: Authors/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ServiceFilter(typeof(AdminCheck))]
    public async Task<IActionResult> Create([Bind("Id,AuthorUrl,WorkShopName,UserId,Title,Description,PersonalPageTitle,PersonalPageSlogan,PersonalPageHeaderPic,PersonalPageHTML,FilterId,Instagram,Facebook,VK,YouTube,HasOwnRobokassa,IsPayingRobokassaFee,RobokassaShopId,RobokassaPassword1,RobokassaPassword2,RobokassaTestPassword1,RobokassaTestPassword2,HasContactForm,ContactFormHeaderText,ContactFormButtonText,ContactFormMessageBoxText")] Author author)
    {
      if (ModelState.IsValid)
      {
        _context.Add(author);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["FilterId"] = new SelectList(_context.Filter, "Id", "Id", author.FilterId);
      ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", author.UserId);
      return View(author);
    }

    // GET: Authors/Edit/5
    [ServiceFilter(typeof(AdminCheck))]
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var author = await _context.Author.FindAsync(id);
      if (author == null)
      {
        return NotFound();
      }
      ViewData["FilterName"] = new SelectList(_context.Filter, "Id", "Name", author.Filter != null ? author.Filter.Name : null);
      ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", author.UserId);
      return View(author);
    }

    public async Task<IActionResult> EditMyAuthorProfile()
    {
      string aid = HttpContext.Session.GetString("AuthorID");
      if(string.IsNullOrEmpty(aid)) return RedirectToAction("Index", "Home");
      int? id = Convert.ToInt32(aid);
      if (id == null)
      {
        return NotFound();
      }

      var author = await _context.Author.FindAsync(id);
      if (author == null)
      {
        return NotFound();
      }
      ViewData["FilterName"] = new SelectList(_context.Filter, "Id", "Name", author.Filter != null ? author.Filter.Name : null);
      ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", author.UserId);
      return View(author);
    }

    // POST: Authors/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ServiceFilter(typeof(AdminCheck))]
    public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorUrl,WorkShopName,UserId,Title,Description,PersonalPageTitle,PersonalPageSlogan,PersonalPageHeaderPic,PersonalPageHTML,FilterId,Instagram,Facebook,VK,YouTube,HasOwnRobokassa,IsPayingRobokassaFee,RobokassaShopId,RobokassaPassword1,RobokassaPassword2,RobokassaTestPassword1,RobokassaTestPassword2,HasContactForm,ContactFormHeaderText,ContactFormButtonText,ContactFormMessageBoxText")] Author author)
    {
      if (id != author.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(author);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!AuthorExists(author.Id))
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
      ViewData["FilterName"] = new SelectList(_context.Filter, "Id", "Name", author.Filter != null ? author.Filter.Name : null);
      ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", author.UserId);
      return View(author);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditMyAuthorProfile([Bind("Id,AuthorUrl,WorkShopName,UserId,Title,Description,PersonalPageTitle,PersonalPageSlogan,PersonalPageHeaderPic,PersonalPageHTML,FilterId,Instagram,Facebook,VK,YouTube,HasOwnRobokassa,IsPayingRobokassaFee,RobokassaShopId,RobokassaPassword1,RobokassaPassword2,RobokassaTestPassword1,RobokassaTestPassword2,HasContactForm,ContactFormHeaderText,ContactFormButtonText,ContactFormMessageBoxText")] Author author)
    {
      string aid = HttpContext.Session.GetString("AuthorID");
      if (string.IsNullOrEmpty(aid)) return RedirectToAction("Index", "Home");
      int? id = Convert.ToInt32(aid);
      if (id != author.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(author);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!AuthorExists(author.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index), "Dashboard");
      }
      ViewData["FilterName"] = new SelectList(_context.Filter, "Id", "Name", author.Filter != null ? author.Filter.Name : null);
      ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", author.UserId);
      return View(author);
    }



    // GET: Authors/Delete/5
    [ServiceFilter(typeof(AdminCheck))]
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var author = await _context.Author
          .Include(a => a.Filter)
          .Include(a => a.User)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (author == null)
      {
        return NotFound();
      }

      return View(author);
    }

    // POST: Authors/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [ServiceFilter(typeof(AdminCheck))]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var author = await _context.Author.FindAsync(id);
      _context.Author.Remove(author);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool AuthorExists(int id)
    {
      return _context.Author.Any(e => e.Id == id);
    }
  }
}
