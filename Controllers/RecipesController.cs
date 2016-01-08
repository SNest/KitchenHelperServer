using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using KitchenHelperServer.EF;
using KitchenHelperServer.Models;

namespace KitchenHelperServer.Controllers
{
    public class RecipesController : ApiController
    {
        private readonly EfContext _db = new EfContext();

        // GET: api/Recipes
        public IQueryable<Recipe> GetRecipes()
        {
            var recipes = _db.Recipes.Include("Ingredients.Product").Include(_ => _.RecipeSteps).AsNoTracking();
            return recipes;
        }

        // GET: api/Recipes/5
        [ResponseType(typeof(Recipe))]
        public IHttpActionResult GetRecipe(int id)
        {
            var recipe = _db.Recipes.Include(_ => _.Ingredients).Include(_ => _.RecipeSteps).SingleOrDefault(_ => _.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        // PUT: api/Recipes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRecipe(int id, Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recipe.Id)
            {
                return BadRequest();
            }

            _db.Entry(recipe).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Recipes
        [ResponseType(typeof (Recipe))]
        public IHttpActionResult PostRecipe(Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Recipes.Add(recipe);
            _db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new {id = recipe.Id}, recipe);
        }

        // DELETE: api/Recipes/5
        [ResponseType(typeof (Recipe))]
        public IHttpActionResult DeleteRecipe(int id)
        {
            var recipe = _db.Recipes.Find(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _db.Recipes.Remove(recipe);
            _db.SaveChanges();

            return Ok(recipe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool RecipeExists(int id)
        {
            return _db.Recipes.Count(e => e.Id == id) > 0;
        }
    }
}