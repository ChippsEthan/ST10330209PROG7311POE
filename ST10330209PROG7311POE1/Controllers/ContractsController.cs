using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10330209PROG7311POE1.Data;
using ST10330209PROG7311POE1.Models;
using ST10330209PROG7311POE1.Patterns.Observer;
using ST10330209PROG7311POE1.Patterns.Factory;

namespace ST10330209PROG7311POE1.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly List<IContractObserver> _observers = new();

        public ContractsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            
            _observers.Add(new EmailNotifierObserver());
        }

        private void NotifyObservers(Contract contract, string oldStatus, string newStatus)
        {
            foreach (var observer in _observers)
            {
                observer.Update(contract, oldStatus, newStatus);
            }
        }

        
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? status)
        {
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();
            if (startDate.HasValue) query = query.Where(c => c.StartDate >= startDate.Value);
            if (endDate.HasValue) query = query.Where(c => c.EndDate <= endDate.Value);
            if (!string.IsNullOrEmpty(status)) query = query.Where(c => c.Status == status);
            var contracts = await query.ToListAsync();
            return View(contracts);
        }

        
        public IActionResult Create()
        {
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile pdfFile)
        {
            if (pdfFile != null && pdfFile.Length > 0)
            {
                var extension = Path.GetExtension(pdfFile.FileName).ToLower();
                if (extension != ".pdf")
                {
                    ModelState.AddModelError("pdfFile", "Only PDF files are allowed.");
                    ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name", contract.ClientId);
                    return View(contract);
                }
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + ".pdf";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }
                contract.SignedAgreementPath = "/uploads/" + uniqueFileName;
            }
            if (ModelState.IsValid)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name", contract.ClientId);
            return View(contract);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name", contract.ClientId);
            return View(contract);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract, string newStatus, IFormFile pdfFile)
        {
            if (id != contract.Id) return NotFound();
            var existing = await _context.Contracts.FindAsync(id);
            if (existing == null) return NotFound();

            
            if (pdfFile != null && pdfFile.Length > 0)
            {
                var extension = Path.GetExtension(pdfFile.FileName).ToLower();
                if (extension != ".pdf")
                {
                    ModelState.AddModelError("pdfFile", "Only PDF files are allowed.");
                    ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name", contract.ClientId);
                    return View(contract);
                }
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + ".pdf";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }
                existing.SignedAgreementPath = "/uploads/" + uniqueFileName;
            }

            
            existing.ClientId = contract.ClientId;
            existing.StartDate = contract.StartDate;
            existing.EndDate = contract.EndDate;
            existing.ServiceLevel = contract.ServiceLevel;

            
            string oldStatus = existing.Status;
            if (!string.IsNullOrEmpty(newStatus) && oldStatus != newStatus)
            {
                existing.Status = newStatus;
                NotifyObservers(existing, oldStatus, newStatus);
            }

            if (ModelState.IsValid)
            {
                _context.Update(existing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name", contract.ClientId);
            return View(contract);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var contract = await _context.Contracts.Include(c => c.Client).FirstOrDefaultAsync(m => m.Id == id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract != null)
            {
                
                if (!string.IsNullOrEmpty(contract.SignedAgreementPath))
                {
                    var filePath = Path.Combine(_env.WebRootPath, contract.SignedAgreementPath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}