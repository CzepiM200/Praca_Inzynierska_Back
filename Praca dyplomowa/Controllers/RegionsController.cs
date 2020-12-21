using Microsoft.AspNetCore.Mvc;
using Praca_dyplomowa.Context;
using Praca_dyplomowa.Entities;
using Praca_dyplomowa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Praca_dyplomowa.Services;


namespace Praca_dyplomowa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Helpers.Authorize]
    public class RegionsController : ControllerBase
    {
        public User CurrentUser => (User)HttpContext.Items["User"];
        private IRegionService _regionService;

        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        [Route("regions")]
        public IActionResult GetRegions()
        {
            var returnData = _regionService.GetRegions(CurrentUser);

            if (returnData.Count() == 0)
                return NoContent();

            return Ok(returnData);
        }

        [HttpPut("regions/edit")]
        public void EditRegion([FromBody] string value)
        {

        }

        [HttpPost("regions/add")]
        public void AddRegion([FromBody] string value)
        {

        }

        [HttpDelete("regions/remove")]
        public void DeleteRegion([FromBody] RemoveIdJSON id)
        {

        }

        [HttpGet]
        [Route("places")]
        public IActionResult GetPlaces()
        {
            var returnData = _regionService.GetPlaces(CurrentUser);

            if (returnData.Count() == 0)
                return NoContent();

            return Ok(returnData);
        }

        [HttpGet]
        [Route("routes")]
        public IActionResult GetRoutes()
        {
            var returnData = _regionService.GetRoutes(CurrentUser);

            if (returnData.Count() == 0)
                return NoContent();

            return Ok(returnData);
        }
    }
}
