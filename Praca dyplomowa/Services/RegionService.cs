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
        List<RegionJSON> GetRegions(User CurrentUser, PageJSON page);
        bool EditRegion(User CurrentUser, RegionJSON modifiedRegion);
        bool AddRegion(User CurrentUser, NewRegionJSON newRegion);
        bool DeleteRegion(User CurrentUser, RemoveIdJSON removeId);

        List<PlaceJSON> GetPlaces(User CurrentUser, PageJSON page);
        PlaceJSON GetPlaceDetails(User CurrentUser, int id);
        bool EditPlace(User CurrentUser, EditPlaceJSON modifiedPlace);
        bool AddPlace(User CurrentUser, NewPlaceJSON newPlace);
        bool DeletePlace(User CurrentUser, RemoveIdJSON removeId);

        List<RouteJSON> GetRoutes(User CurrentUser, PageJSON page);
        RouteJSON GetRouteDetails(User CurrentUser, int id);
        List<SimpleRouteJSON> GetRoutesByPlaceId(User CurrentUser, int placeId);
        bool EditRoute(User CurrentUser, EditRouteJSON modifiedRoute);
        bool AddRoute(User CurrentUser, NewRouteJSON newRoute);
        bool DeleteRoute(User CurrentUser, RemoveIdJSON removeId);

        ListsOfSimpleItems GetAllSimpleItems(User CurrentUser);
    }

    public class RegionService : IRegionService
    {
        private ProgramContext _context;

        public RegionService(ProgramContext context)
        {
            _context = context;
        }

        public List<RegionJSON> GetRegions(User CurrentUser, PageJSON page)
        {
            var userRegions = _context.Regions
                .Where(r => r.UserId == CurrentUser.UserId);
                

            if(page.Number != 0 && page.Page != 0)
                userRegions.Skip((page.Page - 1) * page.Number)
                .Take(page.Number);

            if (userRegions.Count() == 0)
                return null;

            var userPlaces = GetPlaces(CurrentUser, new PageJSON { Page = 1, Number = 100 });

            if (userPlaces == null)
                userPlaces = new List<PlaceJSON>();

            var returnData = new List<RegionJSON>();
            foreach (var region in userRegions)
            {
                var placesInRegion = new List<PlaceJSON>();
                foreach (var place in userPlaces)
                {
                    if (place.BelongRegion.RegionId == region.RegionId)
                    {
                        placesInRegion.Add(place);
                    }
                }
                returnData.Add(new RegionJSON
                {
                    RegionId = region.RegionId,
                    RegionName = region.RegionName,
                    Places = placesInRegion
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

            var co = _context.Regions
                .Where(r => r.RegionName.Equals(newRegion.RegionName) && r.UserId == CurrentUser.UserId);

            if (ifExist == 0)
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

        public List<PlaceJSON> GetPlaces(User CurrentUser, PageJSON page)
        {
            var userPlaces = _context.Places
                .Include(r => r.Region)
                .Where(p => p.Region.UserId == CurrentUser.UserId);
                

            if (page.Number != 0 && page.Page != 0)
                userPlaces.Skip((page.Page - 1) * page.Number)
                .Take(page.Number);

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

        public PlaceJSON GetPlaceDetails(User CurrentUser, int id)
        {
            var userPlace = _context.Places
                .Include(r => r.Region)
                .FirstOrDefault(p => p.Region.UserId == CurrentUser.UserId && p.PlaceId == id);

            if (userPlace == null)
                return null;

            var returnData = new PlaceJSON
            {
                PlaceId = userPlace.PlaceId,
                PlaceName = userPlace.PlaceName,
                Latitude = userPlace.Latitude,
                Longitude = userPlace.Longitude,
                PlaceType = userPlace.PlaceType,
                BelongRegion = new RegionJSON
                {
                    RegionId = userPlace.Region.RegionId,
                    RegionName = userPlace.Region.RegionName
                },
            };

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

        public List<RouteJSON> GetRoutes(User CurrentUser, PageJSON page)
        {
            var userRoutes = _context.Routes
                .Include(r => r.Place)
                .Include(p => p.Place.Region)
                .Where(r => r.Place.Region.UserId == CurrentUser.UserId);
                

            if (page.Number != 0 && page.Page != 0)
                userRoutes.Skip((page.Page - 1) * page.Number)
                .Take(page.Number);

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
                    DescentPosition = route.DescentPosition,
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

        public List<SimpleRouteJSON> GetRoutesByPlaceId(User CurrentUser, int  placeId)
        {
            var userRoutes = _context.Routes
                .Include(r => r.Place)
                .Include(p => p.Place.Region)
                .Where(r => r.Place.Region.UserId == CurrentUser.UserId && r.PlaceId == placeId);

            if (userRoutes.Count() == 0)
                return null;

            var returnData = new List<SimpleRouteJSON>();
            foreach (var route in userRoutes)
            {
                returnData.Add(new SimpleRouteJSON
                {
                    RouteId = route.RouteId,
                    RouteName = route.RouteName,
                });
            }

            return returnData;
        }

        public RouteJSON GetRouteDetails(User CurrentUser, int id)
        {
            var userRoute = _context.Routes
                .Include(r => r.Place)
                .Include(p => p.Place.Region)
                .FirstOrDefault(r => r.Place.Region.UserId == CurrentUser.UserId && r.RouteId == id);

            if (userRoute == null)
                return null;

            var returnData = new RouteJSON{
                RouteId = userRoute.RouteId,
                RouteName = userRoute.RouteName,
                RouteType = userRoute.RouteType,
                Length = userRoute.Length,
                HeightDifference = userRoute.HeightDifference,
                Accomplish = userRoute.Accomplish,
                Material = userRoute.Material,
                Scale = userRoute.Scale,
                Rating = userRoute.Rating,
                Rings = userRoute.Rings,
                DescentPosition = userRoute.DescentPosition,
                BelongPlace = new PlaceJSON
                {
                    PlaceId = userRoute.Place.PlaceId,
                    PlaceName = userRoute.Place.PlaceName,
                    Latitude = userRoute.Place.Latitude,
                    Longitude = userRoute.Place.Longitude,
                    PlaceType = userRoute.Place.PlaceType,
                    BelongRegion = new RegionJSON
                    {
                        RegionId = userRoute.Place.Region.RegionId,
                        RegionName = userRoute.Place.Region.RegionName
                    },
                }
            };

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
                catch (Exception e)
                {
                    // Gry trasy już istnieją
                    return false;
                }
            }
            else
                return false;
        }

        public ListsOfSimpleItems GetAllSimpleItems(User CurrentUser)
        {
            var userRegions = _context.Regions
                .Where(r => r.UserId == CurrentUser.UserId);

            var userPlaces = _context.Places
                .Include(r => r.Region)
                .Where(p => p.Region.UserId == CurrentUser.UserId);

            var userRoutes = _context.Routes
                .Include(r => r.Place)
                .Include(p => p.Place.Region)
                .Where(r => r.Place.Region.UserId == CurrentUser.UserId);

            var list = new ListsOfSimpleItems();
            list.Regions = new List<SimpleRegionJSON>();
            list.Places = new List<SimplePlaceJSON>();
            list.Routes = new List<SimpleRouteJSON>();

            foreach (var region in userRegions)
            {
                list.Regions.Add(new SimpleRegionJSON
                {
                    RegionId = region.RegionId,
                    RegionName = region.RegionName
                });
            }

            foreach (var place in userPlaces)
            {
                list.Places.Add(new SimplePlaceJSON
                {
                    PlaceId = place.PlaceId,
                    PlaceName = place.PlaceName,
                    BelongRegionId = place.RegionId
                });
            }

            foreach (var route in userRoutes)
            {
                list.Routes.Add(new SimpleRouteJSON
                {
                    RouteId = route.RouteId,
                    RouteName = route.RouteName,
                    BelongPlaceId = route.PlaceId,
                });
            }

            return list;
        }

        
    }
}
