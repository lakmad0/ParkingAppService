using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ParkingDataAccess;

namespace ParkingAppService.Controllers
{
    public class SlotsController : ApiController
    {
        // GET: api/Slots
        public IEnumerable<Slot> Get()
        {
            using (ParkingDBEntities entities = new ParkingDBEntities())
            {
                return entities.Slots.ToList();
            }
        }

        // GET: api/Slots/5
        public HttpResponseMessage Get(int id)
        {
            using (ParkingDBEntities entities = new ParkingDBEntities())
            {
                var entity = entities.Slots.FirstOrDefault<Slot>(p => p.PlaceId == id);

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

        // POST: api/Slots
        public HttpResponseMessage Post([FromBody] Slot slot)
        {
            try
            {
                using (ParkingDBEntities entities = new ParkingDBEntities())
                {
                    entities.Slots.Add(slot);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, slot);
                    message.Headers.Location = new Uri(Request.RequestUri + slot.Id.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/Slots/5
        public HttpResponseMessage Put(int id, [FromBody]Slot slot)
        {
            try
            {
                using (ParkingDBEntities entities = new ParkingDBEntities())
                {
                    var entity = entities.Slots.FirstOrDefault<Slot>(p => p.PlaceId == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parking place with id " + id + " not found to update.");
                    }
                    else
                    {
                        entity.FreeSlots= slot.FreeSlots;                  

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

        // DELETE: api/Slots/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (ParkingDBEntities entities = new ParkingDBEntities())
                {
                    var entity = entities.Slots.FirstOrDefault<Slot>(p => p.PlaceId == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parking place with id " + id + " not found to Delete.");
                    }
                    else
                    {
                        entities.Slots.Remove(entity);
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
