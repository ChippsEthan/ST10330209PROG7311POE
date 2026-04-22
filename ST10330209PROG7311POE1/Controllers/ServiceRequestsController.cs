using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10330209PROG7311POE1.Data;
using ST10330209PROG7311POE1.Models;
using ST10330209PROG7311POE1.Services;

namespace ST10330209PROG7311POE1.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CurrencyService _currencyService;

        public ServiceRequestsController(ApplicationDbContext context, CurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        
        public async Task<IActionResult> Index()
        {
            var requests = _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client);
            return View(await requests.ToListAsync());
        }

       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (request == null) return NotFound();
            return View(request);
        }

        
        public async Task<IActionResult> Create()
        {
            
            var activeContracts = await _context.Contracts
                .Include(c => c.Client)
                .Where(c => c.Status != "Expired" && c.Status != "OnHold")
                .ToListAsync();
            ViewBag.Contracts = new SelectList(activeContracts, "Id", "Client.Name");
            var rate = await _currencyService.GetUsdToZarRateAsync();
            ViewBag.CurrentRate = rate;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            var contract = await _context.Contracts.FindAsync(serviceRequest.ContractId);
            if (contract == null)
            {
                ModelState.AddModelError("ContractId", "Invalid contract");
                await RepopulateCreateView(serviceRequest.ContractId);
                return View(serviceRequest);
            }

            
            if (contract.Status == "Expired" || contract.Status == "OnHold")
            {
                ModelState.AddModelError("", "Cannot create service request: Contract is Expired or On Hold.");
                await RepopulateCreateView(serviceRequest.ContractId);
                return View(serviceRequest);
            }

            
            decimal rate = await _currencyService.GetUsdToZarRateAsync();
            serviceRequest.CostInZAR = serviceRequest.CostInUSD * rate;
            serviceRequest.Status = "Open";

            if (ModelState.IsValid)
            {
                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await RepopulateCreateView(serviceRequest.ContractId);
            return View(serviceRequest);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.ServiceRequests
                .Include(r => r.Contract)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (request == null) return NotFound();

            var allContracts = await _context.Contracts.Include(c => c.Client).ToListAsync();
            ViewBag.Contracts = new SelectList(allContracts, "Id", "Client.Name", request.ContractId);
            ViewBag.CurrentRate = await _currencyService.GetUsdToZarRateAsync();
            return View(request);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceRequest request)
        {
            if (id != request.Id) return NotFound();

            var existing = await _context.ServiceRequests.FindAsync(id);
            if (existing == null) return NotFound();

           
            if (existing.ContractId != request.ContractId)
            {
                var newContract = await _context.Contracts.FindAsync(request.ContractId);
                if (newContract == null || newContract.Status == "Expired" || newContract.Status == "OnHold")
                {
                    ModelState.AddModelError("ContractId", "Cannot assign to an Expired or On Hold contract.");
                    var allContracts = await _context.Contracts.Include(c => c.Client).ToListAsync();
                    ViewBag.Contracts = new SelectList(allContracts, "Id", "Client.Name", request.ContractId);
                    ViewBag.CurrentRate = await _currencyService.GetUsdToZarRateAsync();
                    return View(request);
                }
                existing.ContractId = request.ContractId;
            }

            existing.Description = request.Description;
            existing.CostInUSD = request.CostInUSD;
            existing.Status = request.Status;

            
            decimal rate = await _currencyService.GetUsdToZarRateAsync();
            existing.CostInZAR = existing.CostInUSD * rate;

            if (ModelState.IsValid)
            {
                _context.Update(existing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var allContracts2 = await _context.Contracts.Include(c => c.Client).ToListAsync();
            ViewBag.Contracts = new SelectList(allContracts2, "Id", "Client.Name", request.ContractId);
            ViewBag.CurrentRate = rate;
            return View(request);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (request == null) return NotFound();
            return View(request);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request != null)
            {
                _context.ServiceRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task RepopulateCreateView(int? selectedContractId = null)
        {
            var activeContracts = await _context.Contracts
                .Include(c => c.Client)
                .Where(c => c.Status != "Expired" && c.Status != "OnHold")
                .ToListAsync();
            ViewBag.Contracts = new SelectList(activeContracts, "Id", "Client.Name", selectedContractId);
            ViewBag.CurrentRate = await _currencyService.GetUsdToZarRateAsync();
        }
    }
}