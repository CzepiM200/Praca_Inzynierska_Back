using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Praca_dyplomowa.Entities;
using Praca_dyplomowa.Models;
using Praca_dyplomowa.Services;
using Praca_dyplomowa.Helpers;


namespace Praca_dyplomowa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

            if (returnData == null)
                return NoContent();

            return Ok(returnData);
        }

        [HttpPut("regions/edit")]
        public IActionResult EditRegion([FromBody] RegionJSON modifiedRegion)
        {
            bool result = _regionService.EditRegion(CurrentUser, modifiedRegion);

            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpPost("regions/add")]
        public IActionResult AddRegion([FromBody] NewRegionJSON newRegion)
        {
            bool result = _regionService.AddRegion(CurrentUser, newRegion);

            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("regions/remove")]
        public IActionResult DeleteRegion([FromBody] RemoveIdJSON id)
        {
            bool result = _regionService.DeleteRegion(CurrentUser, id);

            if (result)
                return Ok();
            return BadRequest();
        }


        [HttpGet]
        [Route("places")]
        public IActionResult GetPlaces()
        {
            var returnData = _regionService.GetPlaces(CurrentUser);

            if (returnData == null)
                return NoContent();

            return Ok(returnData);
        }

        [HttpPut("places/edit")]
        public IActionResult EditPlace([FromBody] EditPlaceJSON modifiedPlace)
        {
            bool result = _regionService.EditPlace(CurrentUser, modifiedPlace);

            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpPost("places/add")]
        public IActionResult AddPlace([FromBody] NewPlaceJSON newPlace)
        {
            bool result = _regionService.AddPlace(CurrentUser, newPlace);

            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("places/remove")]
        public IActionResult DeletePlace([FromBody] RemoveIdJSON id)
        {
            bool result = _regionService.DeletePlace(CurrentUser, id);

            if (result)
                return Ok();
            return BadRequest();
        }


        [HttpGet]
        [Route("routes")]
        public IActionResult GetRoutes()
        {
            var returnData = _regionService.GetRoutes(CurrentUser);

            if (returnData == null)
                return NoContent();

            return Ok(returnData);
        }

        [HttpPut("routes/edit")]
        public IActionResult EditRoute([FromBody] EditRouteJSON modifiedRoute)
        {
            bool result = _regionService.EditRoute(CurrentUser, modifiedRoute);

            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpPost("routes/add")]
        public IActionResult AddRoute([FromBody] NewRouteJSON newRoute)
        {
            bool result = _regionService.AddRoute(CurrentUser, newRoute);

            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("routes/remove")]
        public IActionResult DeleteRoute([FromBody] RemoveIdJSON id)
        {
            bool result = _regionService.DeleteRoute(CurrentUser, id);

            if (result)
                return Ok();
            return BadRequest();
        }
    }
}
