using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using KitchenHelperServer.EF;
using KitchenHelperServer.Models;
using WebGrease.Css.Extensions;

namespace KitchenHelperServer.Controllers
{
    public class FridgesController : ApiController
    {
        private EfContext db = new EfContext();

        // GET: api/Fridges
        public IQueryable<Fridge> GetFridges()
        {
            return db.Fridges;
        }

        // GET: api/Fridges/5
        [ResponseType(typeof(Fridge))]
        public IHttpActionResult GetFridge(string appToken)
        {
            var fridge = db.Fridges.SingleOrDefault(_ => _.UserGroup.AppToken == appToken);
            if (fridge == null)
            {
                return NotFound();
            }

            return Ok(fridge);
        }

        // PUT: api/Fridges/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFridge(string appToken, Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var storage = db.ProductStorages.SingleOrDefault(_ => _.UserGroup.AppToken == appToken);
            var fridge = db.Fridges.SingleOrDefault(_ => _.UserGroup.AppToken == appToken);

            if (fridge == null && recipe != null && storage != null)
            {
                return NotFound();
            }

            var resultproducts = new List<Product>();

            recipe.Ingredients.Select(_ => _.Product).ForEach(p =>
            {
                if (storage.Products.Contains(p))
                {
                    resultproducts.Add(p);
                };
            });

          return Ok(resultproducts);
        }


        // POST: api/Fridges
        [ResponseType(typeof(Fridge))]
        public IHttpActionResult PostFridge(string appToken, Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fridge = db.Fridges.SingleOrDefault(_ => _.UserGroup.AppToken == appToken);

            if (fridge == null && recipe != null)
            {
                return NotFound();
            }

            fridge.Dishes.Add(recipe);

            return Ok();
        }

        // DELETE: api/Fridges/5
        [ResponseType(typeof(Fridge))]
        public IHttpActionResult DeleteFridge(int id)
        {
            Fridge fridge = db.Fridges.Find(id);
            if (fridge == null)
            {
                return NotFound();
            }

            db.Fridges.Remove(fridge);
            db.SaveChanges();

            return Ok(fridge);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FridgeExists(int id)
        {
            return db.Fridges.Count(e => e.UserGroupId == id) > 0;
        }
    }
}