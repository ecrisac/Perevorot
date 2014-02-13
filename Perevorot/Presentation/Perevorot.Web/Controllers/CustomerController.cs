﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Perevorot.Domain.IServices.DomainInterfaces;
using Perevorot.Domain.Models.DomainEntities;
using Perevorot.Web.Dtos;
using Perevorot.Web.Helpers;
using Perevorot.Web.Models;

namespace Perevorot.Web.Controllers
{
    //[Authorize(Roles = "admin")]
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
            if (customer.Name.Length > 255)
                return Json(new {Result = "Fail", Message = "Name is too long."});

            try
            {
                _customerService.AddNewCustomer(customer.Name);
            }
            catch (Exception e)
            {
                return Json(new {Result = "Fail", e.Message});
            }

            return Json(new {Result = "Success"});
        }

        [HttpPost]
        public JsonResult GetCustomers(DatatablesRequestInfo datatablesRequestInfo,CustomerModel customer)
        {
            var random = new Random();
            IList<Customer> customers = _customerService.GetCustomers();
            IEnumerable<CustomersGridResponseModel> customerDtos =
                   customers.Select(x => new CustomersGridResponseModel
                   {
                       CustomerId = x.Id,
                       CreationDate = x.Created.LocalDateTime,
                       CustomerName = x.Name,
                       Calls = random.Next(),
                       Fields = random.Next(),
                       Details = random.Next().ToString()
                   });

            var customersGridResponseModels = customerDtos as IList<CustomersGridResponseModel> ?? customerDtos.ToList();
            IEnumerable<CustomersGridResponseModel> dtos =
                customersGridResponseModels.Skip(datatablesRequestInfo.iDisplayStart)
                                           .Take(datatablesRequestInfo.iDisplayLength);
            var response = new DatatablesResponseInfo(datatablesRequestInfo)
                {
                    iTotalRecords = customersGridResponseModels.Count(),
                    iTotalDisplayRecords = customersGridResponseModels.Count(),
                    aaData = dtos
                };
            return Json(response);
        }
    }
}