using First_Web_Api_Project.Data;
using First_Web_Api_Project.Models;
using First_Web_Api_Project.Models.DTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace First_Web_Api_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public MedicineController(PharmacyContext context)
        {
                _context = context;
        }

        [HttpGet]
        public ActionResult GetMedicine()
        {
            return Ok(_context.Medicines.Include(m => m.Company).ToList());
        }

        [HttpPost]
        public IActionResult AddMedicine(MedicineDTO m)
        {
            if(m != null)
            {
                var med = new Medicine()
                {
                    Name = m.Name,
                    Description = m.Description,
                    CompanyId = m.CompanyId,
                    Price = m.Price,
                    Formula = m.Formula,
                    Power = m.Power,
                    Expairy = m.Expairy,
                };

                var newAddMedicine = _context.Medicines.Add(med);
                _context.SaveChanges();
                return Ok(newAddMedicine.Entity);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult EditMedicine(int id,MedicineDTO m)
        {
            if (m != null && id != null)
            {
                var medicine = _context.Medicines.Find(id);
                medicine.Name = m.Name;
                medicine.Description = m.Description;
                medicine.Price = m.Price;
                medicine.Power = m.Power;
                medicine.Formula = m.Formula;
                medicine.CompanyId = m.CompanyId;
                medicine.Expairy = m.Expairy;

                var updateCompany = _context.Medicines.Update(medicine);
                _context.SaveChanges();
                return Ok(updateCompany.Entity);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteMedicine(int id)
        {
            if (id != null)
            {
                var med = _context.Medicines.Find(id);
                if (med != null)
                {
                    var DelMedicine = _context.Medicines.Remove(med);
                    _context.SaveChanges();
                    return Ok(DelMedicine.Entity);
                }
                else
                {
                    return NotFound();
                }
            }else {

                return BadRequest(); 
            }
        }

        //[HttpGet("{q}")]
        //public IActionResult SearchMedicines(string q)
        //{
        //    var data = _context.Medicines.Include(x => x.Company).Where(x => x.Name == q || x.Description == q || x.Formula == q || x.Power == q || x.Company.CompanyName == q || x.Company.Address == q).ToList();

        //    if (q != null && data != null)
        //    {
        //        return Ok(data);
        //    }
        //    else
        //    {
        //       return BadRequest();
        //    }
        //}

        [HttpGet("{q}/{PageNo}/{PageSize}")]
        public IActionResult SearchMedicines(string q, int PageNo, int PageSize)
        {
            int pageNumber = PageNo;
            int pageSize = PageSize;
            if (PageNo < 0) { pageNumber = 1; };
            if (PageSize < 0) { pageSize = 1; };

            var data = _context.Medicines.Include(x => x.Company).Where(x => x.Name.Contains(q) || x.Description.Contains(q) || x.Formula.Contains(q) || x.Power.Contains(q) || x.Company.CompanyName.Contains(q) || x.Company.Address.Contains(q)).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            if (q != null && data != null)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }

        //Pagination Api:


        [HttpGet("{PageNo}/{PageSize}")]
        public IActionResult GetMedicine(int PageNo, int PageSize)
        {
            int pageNumber = PageNo;
            int pageSize = PageSize;
            if(PageNo < 0) { pageNumber = 1; };
            if(PageSize < 0) { pageSize = 1; };

            var medicines = _context.Medicines.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            if(medicines != null && medicines.Count > 0)
            {
                return Ok(medicines);

            }else { return BadRequest(); }
        }
    }
}
