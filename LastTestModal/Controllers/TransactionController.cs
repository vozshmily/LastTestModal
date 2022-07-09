using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LastTestModal.Models;

namespace LastTestModal.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionDbContext _context;

        public TransactionController(TransactionDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
              return _context.Transactions != null ? 
                          View(await _context.Transactions.ToListAsync()) :
                          Problem("Entity set 'TransactionDbContext.Transactions'  is null.");
        }

        //// GET: Transaction/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Transactions == null)
        //    {
        //        return NotFound();
        //    }

        //    var transactionModel = await _context.Transactions
        //        .FirstOrDefaultAsync(m => m.TransactionId == id);
        //    if (transactionModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(transactionModel);
        //}

        // GET: Transaction/Create
        public async Task<IActionResult> AddorEdit(int id=0)
        {
            if(id == 0)
                return View(new TransactionModel());
            var transactionModel = await _context.Transactions.FindAsync(id);
            if (transactionModel == null)
            {
                return NotFound();
            }
            return View(transactionModel);
        }

        //// POST: Transaction/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("TransactionId,AccountNumber,BeneficiaryName,BankName,SWIFTCode,Amount")] TransactionModel transactionModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(transactionModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(transactionModel);
        //}

        //NOTES put async

        // GET: Transaction/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Transactions == null)
        //    {
        //        return NotFound();
        //    }

        //    var transactionModel = await _context.Transactions.FindAsync(id);
        //    if (transactionModel == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(transactionModel);
        //}

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddorEdit(int id, [Bind("TransactionId,AccountNumber,BeneficiaryName,BankName,SWIFTCode,Amount")] TransactionModel transactionModel)
        {
            //if (id != transactionModel.TransactionId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    _context.Add(transactionModel);
                    await _context.SaveChangesAsync();
                }
                else {
                    try
                    {
                        _context.Update(transactionModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionModelExists(transactionModel.TransactionId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                
                }


                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this,"_ViewAll",_context.Transactions.ToList()) });
                //return RedirectToAction(nameof(Index));
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddorEdit", transactionModel) });
            //return View(transactionModel);
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transactionModel = await _context.Transactions
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transactionModel == null)
            {
                return NotFound();
            }

            return View(transactionModel);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'TransactionDbContext.Transactions'  is null.");
            }
            var transactionModel = await _context.Transactions.FindAsync(id);
            if (transactionModel != null)
            {
                _context.Transactions.Remove(transactionModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionModelExists(int id)
        {
          return (_context.Transactions?.Any(e => e.TransactionId == id)).GetValueOrDefault();
        }
    }
}
