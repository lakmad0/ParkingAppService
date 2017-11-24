using ParkingDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ParkingAppService.Controllers
{
    [Authorize]
    public class PlacesController : ApiController
    {
        // GET: api/Places
        public IEnumerable<Place> Get()
        {
            using (ParkingDBEntities entities = new ParkingDBEntities())
            {
                return entities.Places.ToList();
            }
        }

        // GET: api/Places/5
        public HttpResponseMessage Get(int id)
        {
            using (ParkingDBEntities entities = new ParkingDBEntities())
            {
                var entity = entities.Places.FirstOrDefault<Place>(p => p.Id == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Place with id " + id + " not found.");
                }
            }
        }

        // POST: api/Places
        public HttpResponseMessage Post([FromBody]Place place)
        {
            try
            {
                using (ParkingDBEntities entities = new ParkingDBEntities())
                {
                    entities.Places.Add(place);
                    entities.SaveChanges();

                    //To add slots entry
                    Slot slot = new Slot();
                    slot.PlaceId = place.Id;
                    slot.FreeSlots = place.MaxSlots;

                    entities.Slots.Add(slot);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, place);
                    message.Headers.Location = new Uri(Request.RequestUri + place.Id.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/Places/5
        public HttpResponseMessage Put(int id, [FromBody]Place place)
        {
            try
            {
                using (ParkingDBEntities entities = new ParkingDBEntities())
                {
                    var entity = entities.Places.FirstOrDefault<Place>(p => p.Id == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Place with id " + id + " not found to update.");
                    }
                    else
                    {
                        entity.PlaceName = place.PlaceName;
                        entity.Address = place.Address;
                        entity.Longitude = place.Longitude;
                        entity.Longitude = place.Latitude;
                        entity.MaxSlots = place.MaxSlots;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE: api/Places/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (ParkingDBEntities entities = new ParkingDBEntities())
                {
                    var place = entities.Places.FirstOrDefault<Place>(p => p.Id == id);
                    var slot = entities.Slots.FirstOrDefault<Slot>(p => p.PlaceId == id);

                    if (place == null || slot == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Place with id " + id + " not found to Delete.");
                    }
                    else
                    {
                        entities.Places.Remove(place);
                        entities.Slots.Remove(slot);
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
