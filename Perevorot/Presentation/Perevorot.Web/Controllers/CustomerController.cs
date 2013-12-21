using System;
using Perevorot.Domain.IServices.DomainInterfaces;
using Perevorot.Web.Models;

namespace Perevorot.Web.Controllers
{
    using System.Web.Mvc;

    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public JsonResult AddNewCustomer(CustomerModel customer)
        {            

            if (customer.Name.Length>255)
                return Json(new { Result = "Fail", Message = "Name is too long." });

            try
            {
                _customerService.AddNewCustomer(customer.Name);
            }
            catch (Exception e)
            {
                return Json(new { Result = "Fail", Message = e.Message });
            }

            return Json(new { Result = "Success" });
        }

    }
}