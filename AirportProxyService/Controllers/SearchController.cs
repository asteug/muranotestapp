using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using AirportProxyService.Algorithm;
using AirportProxyService.ApiIntegration;
using AirportProxyService.Helpers;
using AirportProxyService.Models;
using Serilog;

namespace AirportProxyService.Controllers
{
    public class SearchController : Controller
    {
        private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        private List<RouteExtension> SearchResultAirportExtensions { get; set; } = new List<RouteExtension>();
        public async Task<ActionResult> Index()
        {
            Log.Logger.Information($"Method type: {HttpContext.Request.RequestType} " +
                                $"\r\n Action: {ControllerContext.RouteData.Values["controller"]}" +
                                $"\r\n Action: {ControllerContext.RouteData.Values["action"]}");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SearchAirport(SearchAirportModel airportModel)
        {
            CancellationToken token = _cancelTokenSource.Token;
            try
            {
                var airports = await new AirportApiIntegrationMethods().GetAllAirportByPatternAsync(airportModel.query, token);
                return Json(airports);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, $"{nameof(SearchAirport)} exception handler\r\n {ex.Message} \r\n Inner exception: {ex.InnerException?.Message}");
            }
            finally
            {
                _cancelTokenSource.Cancel();
            }
            return Json("");
        }

        [HttpPost]
        public async Task<ActionResult> SearchDestinations(SearchDestinationModel model)
        {
            if (!ModelState.IsValid)
                return Json(new CommonMessage(string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)), ModelState.IsValid));

            CancellationToken token = _cancelTokenSource.Token;
            try
            {
                DepthFirstSearch searchDic = new DepthFirstSearch();
                int iteration = 0;

                List<RouteEntry> routes = new List<RouteEntry>();
                List<string> allRoutes = new List<string>();
                List<Route> chain = new List<Route>();
                do
                {
                    if (iteration == 0)
                    {
                        var routesInCityAirports =
                            await new RouteApiIntegrationMethods().GetAllRoutesAsync(model.FromAirport, token);

                        routes.Add(new RouteEntry(0, model.FromAirport, false,
                            routesInCityAirports.Result.Where(w => !w.DestAirport.Contains("\\N")).ToList()));
                        allRoutes.AddRange(routesInCityAirports.Result.Where(w => !w.DestAirport.Contains("\\N")).Select(s => s.DestAirport).ToList());
                    }
                    else if (iteration < 4)
                    {
                        var iteration1 = iteration;
                        var routes1 = allRoutes;
                        var tasksCheckAirplaneRoutes = routes.Where(w => !w.IsChecked).Select(
                                                           async items =>
                                                           {
                                                               foreach (var item in items.Routes)
                                                               {
                                                                   var routesInCityAirports = await new RouteApiIntegrationMethods()
                                                                           .GetAllRoutesAsync(item.DestAirport, token);
                                                                   routes.Add(new RouteEntry((iteration1 - 1), item.SrcAirport,
                                                                       false, routesInCityAirports.Result.Where(w =>
                                                                               !w.DestAirport.Contains("\\N")).ToList(), item));
                                                                   routes1.AddRange(routesInCityAirports.Result.Where(w => !w.DestAirport.Contains("\\N")).Select(s => s.DestAirport));
                                                               }
                                                           });

                        await Task.WhenAll(tasksCheckAirplaneRoutes);
                        allRoutes = new HashSet<string>(routes1).ToList();
                        routes.Where(w => w.Iteration <= iteration - 1).Select(s => { s.IsChecked = true; return s; }).ToList();
                    }
                    else
                    {
                        break;
                    }
                    iteration++;
                } while (allRoutes.All(a => a != model.ToAirport));

                if (allRoutes.Any(a => a == model.ToAirport))
                {

                    if (StaticClass.GetChain(routes, model.FromAirport, model.ToAirport))
                    {

                    }
                    //var tasks = listOfRoute?.Routes.Select(async items =>
                    //{
                    //    var airlinesAccess = await new AirlineApiIntegrationMethods().GetAllAirlineByAliasAsync(items.Airline, token);
                    //    SearchResultAirportExtensions.Add(new RouteExtension()
                    //    {
                    //        Airline = items.Airline,
                    //        Destination = items.DestAirport,
                    //        Source = items.SrcAirport,
                    //        IsParent = items.SrcAirport == model.FromAirport,
                    //        IsWorkingAirline = airlinesAccess.Result.Any(a => a.Alias == items.Airline && a.IsActive)
                    //    });
                    //});
                    //await Task.WhenAll(tasks);
                    //_cancelTokenSource.CancelAfter(5000);
                    return Json(new CommonMessage<List<RouteExtension>>(
                        SearchResultAirportExtensions.Where(w => w.IsWorkingAirline).ToList(), ""));
                }


                //_cancelTokenSource.CancelAfter(5000);

                return Json(new CommonMessage<List<RouteExtension>>(SearchResultAirportExtensions.Where(w => w.IsWorkingAirline).ToList(), ""));



                // Step 2. Search by destination city
                #region Other Implemintation
                //  else
                //{
                //    var airportsInCityDestination = await new AirportApiIntegrationMethods().GetAllAirportByPatternAsync(model.ToCity, token);
                //    var airportsInCityDestinationList = airportsInCityDestination.Result.Where(w => w.Alias != "\\N" && w.Alias != model.ToAirport).Select(s => s.Alias);
                //    var compareAirport = routes.Result.Where(w => airportsInCityDestinationList.Any(a => a == w.DestAirport)).ToList();
                //    if (compareAirport.Count > 0)
                //    {
                //        var tasks = compareAirport.Select(async item =>
                //        {
                //            var airlinesAccess =
                //                await new AirlineApiIntegrationMethods().GetAllAirlineByAliasAsync(item.Airline, token);
                //            SearchResultAirportExtensions.Add(new RouteExtension()
                //            {
                //                Airline = item.Airline,
                //                DestAirport = item.DestAirport,
                //                SrcAirport = item.SrcAirport,
                //                IsWorkingAirline = airlinesAccess.Result.Any(a => a.Alias == item.Airline && a.IsActive)
                //            });
                //        });
                //        await Task.WhenAll(tasks);
                //        _cancelTokenSource.CancelAfter(5000);
                //        return Json(new CommonMessage<List<RouteExtension>>(
                //            SearchResultAirportExtensions.Where(w => w.IsWorkingAirline).ToList(), ""));
                //    }
                //    else
                //    {
                //        // Get all airports in city
                //        var airportsInCitySource = await new AirportApiIntegrationMethods().GetAllAirportByPatternAsync(model.FromCity, token);
                //        var routesInCitySource = new List<Route>();
                //        // Add parent routes
                //        routesInCitySource.AddRange(routes.Result);
                //        var airportsInSourceCityList = airportsInCitySource.Result.Where(w => w.Alias != "\\N" && w.Alias != model.FromAirport).Select(s => s.Alias).ToList();

                //        // Check destinations by city
                //        var tasksGetRoutesByCity = airportsInSourceCityList.Select(async item =>
                //        {
                //            var routesInCityAirports = await new RouteApiIntegrationMethods().GetAllRoutesAsync(item, token);
                //            routesInCitySource.AddRange(routesInCityAirports.Result);
                //        });
                //        await Task.WhenAll(tasksGetRoutesByCity);
                //        if (routesInCitySource.Any(a => a.DestAirport == model.ToAirport))
                //        {
                //            var selectedRoutes = routesInCitySource.Where(w => w.DestAirport == model.ToAirport)
                //                .ToList();

                //            var tasksCheckAirplaneRoutes = selectedRoutes.Select(async item =>
                //            {
                //                var airlinesAccess =
                //                    await new AirlineApiIntegrationMethods().GetAllAirlineByAliasAsync(item.Airline, token);
                //                SearchResultAirportExtensions.Add(new RouteExtension()
                //                {
                //                    Airline = item.Airline,
                //                    DestAirport = item.DestAirport,
                //                    SrcAirport = item.SrcAirport,
                //                    IsWorkingAirline = airlinesAccess.Result.Any(a => a.Alias == item.Airline && a.IsActive)
                //                }); ;
                //            });
                //            await Task.WhenAll(tasksCheckAirplaneRoutes);
                //            _cancelTokenSource.CancelAfter(5000);

                //        }
                //        // Checking destination through 2 level
                //        else
                //        {
                //            var tasksCheckAirplaneRoutes = routesInCitySource.Select(s => s.DestAirport).Distinct().ToList().Select(async item =>
                //              {
                //                  var routesInCityAirports = await new RouteApiIntegrationMethods().GetAllRoutesAsync(item, token);
                //                  routesInCitySource.AddRange(routesInCityAirports.Result);


                //              });
                //            await Task.WhenAll(tasksCheckAirplaneRoutes);
                //            if (routesInCitySource.Any(a => a.DestAirport == model.ToAirport))
                //            {
                //                var selectedRoutes = routesInCitySource.Where(w => w.DestAirport == model.ToAirport)
                //                    .ToList();
                //                var tasksCheckAirplaneAccess = selectedRoutes.Select(async item =>
                //                {
                //                    var airlinesAccess =
                //                        await new AirlineApiIntegrationMethods().GetAllAirlineByAliasAsync(item.Airline, token);
                //                    SearchResultAirportExtensions.Add(new RouteExtension()
                //                    {
                //                        Airline = item.Airline,
                //                        DestAirport = item.DestAirport,
                //                        SrcAirport = item.SrcAirport,
                //                        IsWorkingAirline = airlinesAccess.Result.Any(a => a.Alias == item.Airline && a.IsActive)
                //                    }); ;
                //                });
                //            }
                //            _cancelTokenSource.CancelAfter(5000);

                //        }
                //        return Json(new CommonMessage<List<RouteExtension>>(
                //            SearchResultAirportExtensions.Where(w => w.IsWorkingAirline).ToList(), ""));
                //    } 
                #endregion
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, $"{nameof(SearchAirport)} exception handler\r\n {ex.Message} \r\n Inner exception: {ex.InnerException?.Message}");
                return Json(new CommonMessage(ex.Message, false));
            }
            finally
            {
                _cancelTokenSource.Cancel();
            }
        }
    }
}