using System.Web.Mvc;

namespace KitchenHelperServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            //var links = EdaRuParser.GetAllRecipeLinksByCategory("vypechka-deserty", 10).ToList();

            //EdaRuParser.SaveAllProductsToDb(links);
            //EdaRuParser.SaveAllIngredientsToDb(links);
            //EdaRuParser.SaveAllRecipeStepsToDb(links);
            //EdaRuParser.SaveAllRecipesToDb(links);

            return View();
        }
    }
}