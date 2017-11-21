using ParkingDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ParkingAppService.Controllers
{
    public class SlotsViewController : ApiController
    {
        // GET: api/SlotsView
        public IEnumerable<SlotsView> Get()
        {
            using (ParkingDBEntities entities = new ParkingDBEntities())
            {
                return entities.SlotsViews.ToList();
            }
        }

        // GET: api/SlotsView/5
        public HttpResponseMessage Get(int id)
        {
            using (ParkingDBEntities entities = new ParkingDBEntities())
            {
                var entity = entities.SlotsViews.FirstOrDefault<SlotsView>(p => p.PlaceId == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parking place with id " + id + " not found.");
                }
            }
        }
    }
}
