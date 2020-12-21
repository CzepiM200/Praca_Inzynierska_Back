using Microsoft.EntityFrameworkCore;
using Praca_dyplomowa.Context;
using Praca_dyplomowa.Entities;
using Praca_dyplomowa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Praca_dyplomowa.Services
{
    public interface IRegionService
    {
        List<RegionJSON> GetRegions(User CurrentUser);
        bool EditRegion(User CurrentUser, RegionJSON modifiedRegion);
        bool AddRegion(User CurrentUser, NewRegionJSON newRegion);
        bool DeleteRegion(User CurrentUser, RemoveIdJSON removeId);

        List<PlaceJSON> GetPlaces(User CurrentUser);
        bool EditPlace(User CurrentUser, RegionJSON modifiedPlace);
        bool AddPlace(User CurrentUser, NewRegionJSON newPlace);
        bool DeletePlace(User CurrentUser, RemoveIdJSON removeId);

        List<RouteJSON> GetRoutes(User CurrentUser);
        bool EditRoute(User CurrentUser, RegionJSON modifiedRoute);
        bool AddRoute(User CurrentUser, NewRegionJSON newRoute);
        bool DeleteRoute(User CurrentUser, RemoveIdJSON removeId);


    }

    public class RegionService : IRegionService
    {
        private ProgramContext _context;

        public RegionService(ProgramContext context)
        {
            _context = context;
        }

        public List<RegionJSON> GetRegions(User CurrentUser)
        {
            var userRegions = _context.Regions
                .Where(r => r.UserId == CurrentUser.UserId);

            if (userRegions.Count() == 0)
                return null;

            var returnData = new List<RegionJSON>();
            foreach (var region in userRegions)
            {
                returnData.Add(new RegionJSON
                {
                    RegionId = region.RegionId,
                    RegionName = region.RegionName
                });
            }

            return returnData;
        }

        public bool EditRegion(User CurrentUser, RegionJSON modifiedRegion)
        {
            var region = _context.Regions
                .FirstOrDefault(r => r.RegionId == modifiedRegion.RegionId && r.UserId == CurrentUser.UserId);

            if (region != null)
            {
                try
                {
                    region.RegionName = modifiedRegion.RegionName;
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool AddRegion(User CurrentUser, NewRegionJSON newRegion)
        {
            var ifExist = _context.Regions
                .Count(r => r.RegionName.Equals(newRegion.RegionName) && r.UserId == CurrentUser.UserId);

            if(ifExist == 0)
            {
                try
                {
                    var region = new Region
                    {
                        RegionName = newRegion.RegionName,
                        UserId = CurrentUser.UserId,
                        User = CurrentUser,
                    };
                    _context.Regions.Add(region);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool DeleteRegion(User CurrentUser, RemoveIdJSON removeId)
        {
            var region = _context.Regions
                .FirstOrDefault(r => r.RegionId == removeId.Id && r.UserId == CurrentUser.UserId);

            if (region != null)
            {
                try
                {
                    _context.Regions.Remove(region);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public List<PlaceJSON> GetPlaces(User CurrentUser)
        {
            var userPlaces = _context.Places
                .Include(r => r.Region)
                .Where(p => p.Region.UserId == CurrentUser.UserId);

            if (userPlaces.Count() == 0)
                return null;

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
                    BelongRegion = new RegionJSON
                    {
                        RegionId = place.Region.RegionId,
                        RegionName = place.Region.RegionName
                    },
                });
            }
            return returnData;
        }

        public bool EditPlace(User CurrentUser, PlaceJSON modifiedPlace)
        {
            var place = _context.Places
                .FirstOrDefault(r => r.PlaceId == modifiedPlace.PlaceId && r.Region.UserId == CurrentUser.UserId);

            if (place != null)
            {
                try
                {
                    //place.
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool AddPlace(User CurrentUser, NewRegionJSON region)
        {
            throw new NotImplementedException();
        }

        public bool DeletePlace(User CurrentUser, RemoveIdJSON removeId)
        {
            throw new NotImplementedException();
        }

        public List<RouteJSON> GetRoutes(User CurrentUser)
        {
            var userRoutes = _context.Routes
                .Include(r => r.Place)
                .Include(p => p.Place.Region)
                .Where(r => r.Place.Region.UserId == CurrentUser.UserId);

            if (userRoutes.Count() == 0)
                return null;

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

            return returnData;
        }

        public bool EditRoute(User CurrentUser, RegionJSON region)
        {
            throw new NotImplementedException();
        }

        public bool AddRoute(User CurrentUser, NewRegionJSON region)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRoute(User CurrentUser, RemoveIdJSON removeId)
        {
            throw new NotImplementedException();
        }
    }
}
