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

namespace Praca_dyplomowa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Helpers.Authorize]
    public class RegionsController : ControllerBase
    {
        public User CurrentUser => (User)HttpContext.Items["User"];
        private readonly ProgramContext _context;

        public RegionsController(ProgramContext context)
        {
            _context = context;
        }

        // GET: api/regions/regions
        [HttpGet]
        [Route("regions")]
        public IActionResult GetRegions([FromBody] UserIdModel userId)
        {
            var userRegions = _context.Regions
                .Where(r => r.UserId == userId.userId);

            var returnData = new List<RegionJSON>();
            foreach (var region in userRegions)
            {
                returnData.Add(new RegionJSON{ 
                    RegionId = region.RegionId, 
                    RegionName = region.RegionName
                });
            }

            return Ok(returnData);
        }

        [HttpGet]
        [Route("places")]
        public IActionResult GetPlaces([FromBody] UserIdModel userId)
        {
            var userPlaces = _context.Places
                .Include(r => r.Region)
                .Where(p => p.Region.UserId == userId.userId);

            var returnData = new List<PlaceJSON>();
            foreach (var place in userPlaces)
            {
                returnData.Add(new PlaceJSON
                {
                    PlaceId = place.PlaceId,
                    PlaceName = place.PlaceName,
                    Latitude = place.Latitude,
                    Longitude = place.Longitude,
                    PlaceType = place.PlaceType,
                    BelongRegion = new RegionJSON { 
                        RegionId = place.Region.RegionId, 
                        RegionName = place.Region.RegionName 
                    },
                });
            }

            return Ok(returnData);
        }

        [HttpGet]
        [Route("routes")]
        public IActionResult GetRoutes([FromBody] UserIdModel userId)
        {
            var userRoutes = _context.Routes
                .Include(r => r.Place)
                .Include(p => p.Place.Region)
                .Where(r => r.Place.Region.UserId == CurrentUser.UserId);

            var returnData = new List<RouteJSON>();
            foreach (var route in userRoutes)
            {
                returnData.Add(new RouteJSON
                {
                    RouteId = route.RouteId,
                    RouteName = route.RouteName,
                    RouteType = route.RouteType,
                    Length = route.Length,
                    HeightDifference = route.HeightDifference,
                    Accomplish = route.Accomplish,
                    Material = route.Material,
                    Scale = route.Scale,
                    Rating = route.Rating,
                    Rings = route.Rings,
                    BelongPlace = new PlaceJSON
                    {
                        PlaceId = route.Place.PlaceId,
                        PlaceName = route.Place.PlaceName,
                        Latitude = route.Place.Latitude,
                        Longitude = route.Place.Longitude,
                        PlaceType = route.Place.PlaceType,
                        BelongRegion = new RegionJSON
                        {
                            RegionId = route.Place.Region.RegionId,
                            RegionName = route.Place.Region.RegionName
                        },
                    },
            });
            }

            return Ok(returnData);
        }


        //// GET api/<RegionsController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<RegionsController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<RegionsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<RegionsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
