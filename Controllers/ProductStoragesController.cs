using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using KitchenHelperServer.EF;
using KitchenHelperServer.Models;

namespace KitchenHelperServer.Controllers
{
    public class ProductStoragesController : ApiController
    {
        private EfContext db = new EfContext();

        // GET: api/ProductStorages
        public IQueryable<ProductStorage> GetProductStorages()
        {
            return db.ProductStorages;
        }

        // GET: api/ProductStorages/5
        [ResponseType(typeof(ProductStorage))]
        public IHttpActionResult GetProductStorage(int id)
        {
            ProductStorage productStorage = db.ProductStorages.Find(id);
            if (productStorage == null)
            {
                return NotFound();
            }

            return Ok(productStorage);
        }

        // PUT: api/ProductStorages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProductStorage(int id, ProductStorage productStorage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productStorage.UserGroupId)
            {
                return BadRequest();
            }

            db.Entry(productStorage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductStorageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProductStorages
        [ResponseType(typeof(ProductStorage))]
        public IHttpActionResult PostProductStorage(string appToken, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storage = db.ProductStorages.SingleOrDefault(_ => _.UserGroup.AppToken == appToken);

            if (storage == null && product != null)
            {
                return NotFound();
            }

            storage.Products.Add(product);

            return Ok();
        }

        // DELETE: api/ProductStorages/5
        [ResponseType(typeof(ProductStorage))]
        public IHttpActionResult DeleteProductStorage(int id)
        {
            ProductStorage productStorage = db.ProductStorages.Find(id);
            if (productStorage == null)
            {
                return NotFound();
            }

            db.ProductStorages.Remove(productStorage);
            db.SaveChanges();

            return Ok(productStorage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductStorageExists(int id)
        {
            return db.ProductStorages.Count(e => e.UserGroupId == id) > 0;
        }
    }
}