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
        bool EditPlace(User CurrentUser, EditPlaceJSON modifiedPlace);
        bool AddPlace(User CurrentUser, NewPlaceJSON newPlace);
        bool DeletePlace(User CurrentUser, RemoveIdJSON removeId);

        List<RouteJSON> GetRoutes(User CurrentUser);
        bool EditRoute(User CurrentUser, EditRouteJSON modifiedRoute);
        bool AddRoute(User CurrentUser, NewRouteJSON newRoute);
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

        public bool EditPlace(User CurrentUser, EditPlaceJSON modifiedPlace)
        {
            var place = _context.Places
                .FirstOrDefault(r => r.PlaceId == modifiedPlace.PlaceId && r.Region.UserId == CurrentUser.UserId);


            if (place != null)
            {
                try
                {
                    place.PlaceName = modifiedPlace.PlaceName;
                    place.Latitude = modifiedPlace.Latitude;
                    place.Longitude = modifiedPlace.Longitude;
                    place.PlaceType = modifiedPlace.PlaceType;
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

        public bool AddPlace(User CurrentUser, NewPlaceJSON newPlace)
        {
            var ifPlaceExist = _context.Places
                .Count(p => p.PlaceName.Equals(newPlace.PlaceName) && p.Region.UserId == CurrentUser.UserId);

            var ifRegionExist = _context.Regions
                .Count(r => r.RegionId == newPlace.BelongRegionId && r.UserId == CurrentUser.UserId);

            if (ifPlaceExist == 0 && ifRegionExist == 1)
            {
                try
                {
                    var place = new Place
                    {
                        PlaceName = newPlace.PlaceName,
                        Latitude = newPlace.Latitude,
                        Longitude = newPlace.Longitude,
                        PlaceType = newPlace.PlaceType,
                        RegionId = newPlace.BelongRegionId,
                    };
                    _context.Places.Add(place);
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

        public bool DeletePlace(User CurrentUser, RemoveIdJSON removeId)
        {
            var place = _context.Places
                .FirstOrDefault(r => r.PlaceId == removeId.Id && r.Region.UserId == CurrentUser.UserId);

            if (place != null)
            {
                try
                {
                    _context.Places.Remove(place);
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

        public bool EditRoute(User CurrentUser, EditRouteJSON modifiedRoute)
        {
            var route = _context.Routes
                .FirstOrDefault(r => r.RouteId == modifiedRoute.RouteId && r.Place.Region.UserId == CurrentUser.UserId);


            if (route != null)
            {
                try
                {
                    route.RouteName = modifiedRoute.RouteName;
                    route.RouteType = modifiedRoute.RouteType;
                    route.Length = modifiedRoute.Length;
                    route.HeightDifference = modifiedRoute.HeightDifference;
                    route.Accomplish = modifiedRoute.Accomplish;
                    route.Material = modifiedRoute.Material;
                    route.Scale = modifiedRoute.Scale;
                    route.Rating = modifiedRoute.Rating;
                    route.DescentPosition = modifiedRoute.DescentPosition;
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

        public bool AddRoute(User CurrentUser, NewRouteJSON newRoute)
        {
            var ifRouteExist = _context.Routes
                .Count(r => r.RouteName.Equals(newRoute.RouteName) && r.Place.Region.UserId == CurrentUser.UserId);

            var ifPlaceExist = _context.Places
                .Count(p => p.PlaceId == newRoute.BelongPlaceId && p.Region.UserId == CurrentUser.UserId);

            if (ifRouteExist == 0 && ifPlaceExist == 1)
            {
                try
                {
                    var route = new Route
                    {
                        RouteName = newRoute.RouteName,
                        RouteType = newRoute.RouteType,
                        Length = newRoute.Length,
                        HeightDifference = newRoute.HeightDifference,
                        Accomplish = newRoute.Accomplish,
                        Material = newRoute.Material,
                        Scale = newRoute.Scale,
                        Rating = newRoute.Rating,
                        DescentPosition = newRoute.DescentPosition,
                        PlaceId = newRoute.BelongPlaceId,
                    };
                    _context.Routes.Add(route);
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

        public bool DeleteRoute(User CurrentUser, RemoveIdJSON removeId)
        {
            var route = _context.Routes
                .FirstOrDefault(r => r.RouteId == removeId.Id && r.Place.Region.UserId == CurrentUser.UserId);

            if (route != null)
            {
                try
                {
                    _context.Routes.Remove(route);
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
    }
}
