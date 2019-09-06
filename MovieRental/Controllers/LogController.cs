using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Core.Interfaces;
using MovieRental.Core.Movie;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieRental.Controllers
{
    [Route("logs")]
    public class LogController : BaseController
    {
        private readonly IRepository<PriceUpdateLogEntity> _priceUpdateLogRepository;
        private readonly IRepository<PurchaseLogEntity> _purchaseLogRepository;
        private readonly IRepository<RentalLogEntity> _rentalLogRepository;

        public LogController(IRepository<PurchaseLogEntity> purchaseLogRepository, IRepository<PriceUpdateLogEntity> priceUpdateLogRepository, IRepository<RentalLogEntity> rentalLogRepository)
        {
            _priceUpdateLogRepository = priceUpdateLogRepository;
            _purchaseLogRepository = purchaseLogRepository;
            _rentalLogRepository = rentalLogRepository;
        }

        [HttpGet]
        [Route("price")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPriceUpdateLogs()
        {
            return Ok(_priceUpdateLogRepository.FindAll());
        }

        [HttpGet]
        [Route("purchase")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPurchaseLogs()
        {
            return Ok(_purchaseLogRepository.FindAll());
        }

        [HttpGet]
        [Route("rental")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRentalLogs()
        {
            return Ok(_rentalLogRepository.FindAll());
        }
    }
}
