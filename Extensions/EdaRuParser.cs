using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using KitchenHelperServer.EF;
using KitchenHelperServer.Models;
using WebGrease.Css.Extensions;

namespace KitchenHelperServer.Extensions
{
    public class EdaRuParser
    {
        private static readonly EfContext Context = new EfContext();
        private static readonly Encoding Encoding = Encoding.GetEncoding("utf-8");
        private static WebResponse _response;
        private static WebRequest _request;

        public static IEnumerable<string> GetAllRecipeLinksByCategory(string category, int maxPage)
        {
            var allRecipeLinks = new List<string>();
            for (var i = 1; i <= maxPage; i++)
            {
                var initialUrl = "http://eda.ru/recepty/" + category + "/page" + i;
                var html = new HtmlDocument();
                html.LoadHtml(GetHtmlString(initialUrl));
                var root = html.DocumentNode;
                var currentpageRecipeLinks = root.SelectNodes("/html/body/div[4]/div[4]/div/div[1]/div/div/div[2]" +
                                                              "/div[1]/div/div/div/div/h3/a/@href")
                    .Select(_ => _.GetAttributeValue("href", string.Empty))
                    .ToList();
                allRecipeLinks = allRecipeLinks.Concat(currentpageRecipeLinks).ToList();
            }

            return allRecipeLinks;
        }

        public static void SaveAllIngredientsToDb(IEnumerable<string> recipeLinks)
        {
            var ingredientNodes = new List<HtmlNode>();
            var ingredients = new List<Ingredient>();

            recipeLinks.ForEach(recipelink =>
            {
                var html = new HtmlDocument();
                html.LoadHtml(GetHtmlString(recipelink));
                var root = html.DocumentNode;

                var currentPageIngredientNodes = root.SelectNodes("/html/body/div/div/div/div/div" +
                                                                  "/article/div/div/div/div/table/tbody/tr").ToList();

                ingredientNodes = ingredientNodes.Concat(currentPageIngredientNodes).ToList();
            });

            foreach (var productLink in ingredientNodes)
            {
                var productName = productLink.SelectSingleNode(".//*[@class=\"name\"]").InnerText.Replace("&#171;", "\"").Replace("&#187;", "\"");
                var ingredientQuantity = productLink.SelectSingleNode(".//*[@class=\"amount\"]").InnerText.Replace("&frac12;", "0,5").Replace("&frac14;", "0,25").Replace("&frac34;", "0,75");
                var product = Context.Products.SingleOrDefault(_ => _.Name == productName);
                ingredients.Add(new Ingredient
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = ingredientQuantity
                });
            }

            ingredients.ForEach(ingredient =>
            {
                Context.Ingredients.Add(ingredient);
            });
            Context.SaveChanges();
        }

        public static void SaveAllProductsToDb(IEnumerable<string> recipeLinks)
        {
            var productLinks = new List<string>();
            recipeLinks.ForEach(recipelink =>
            {
                var html = new HtmlDocument();
                html.LoadHtml(GetHtmlString(recipelink));
                var root = html.DocumentNode;

                var currentPageProductLinks =
                    root.SelectNodes("/html/body/div/div/div/div/div/article/div/div/div/div/table/tbody/tr//*[@class=\"name\"]")
                        .Select(_ => _.InnerText)
                        .ToList();
                productLinks = productLinks.Concat(currentPageProductLinks).ToList();
            });

            foreach (var productLink in productLinks.Distinct())
            {
                var product = new Product
                {
                    Name = productLink.Replace("&#171;", "\"").Replace("&#187;", "\"")
                };

                Context.Products.Add(product);
            }

            Context.SaveChanges();
        }

        public static void SaveAllRecipeStepsToDb(IEnumerable<string> recipeLinks)
        {
            var resipeSteps = new List<string>();
            recipeLinks.ForEach(recipelink =>
            {
                var html = new HtmlDocument();
                html.LoadHtml(GetHtmlString(recipelink));
                var root = html.DocumentNode;

                var currentPageRecipeSteps =
                    root.SelectNodes("/html/body/div/div/div/div/div/article/div/div/div/div/ol/li")
                        .Select(_ => _.InnerText)
                        .ToList();
                resipeSteps = resipeSteps.Concat(currentPageRecipeSteps).ToList();
            });

            foreach (var step in resipeSteps)
            {
                if (step.Contains("Adf.banner.show"))
                {
                    continue;
                }

                var recipeStep = new RecipeStep
                {
                    Description = step.Replace("\n", string.Empty)
                                        .Replace("\r", string.Empty)
                                        .Replace("\t", string.Empty)
                                        .Replace("&nbsp;", " ")
                };

                Context.RecipeSteps.Add(recipeStep);
            }

            Context.SaveChanges();
        }

        public static void SaveAllRecipesToDb(IEnumerable<string> recipeLinks)
        {
            var resipeNodes = new List<HtmlNode>();
            recipeLinks.ForEach(recipelink =>
            {
                var html = new HtmlDocument();
                html.LoadHtml(GetHtmlString(recipelink));
                var root = html.DocumentNode;

                var currentPageRecipe =
                    root.SelectSingleNode("/html/body/div/div/div/div/div/article/div/div/div");
                resipeNodes.Add(currentPageRecipe);
            });

            foreach (var step in resipeNodes)
            {
                var recipeStepNodes = step.SelectNodes("//*[@class=\"instruction\"]").Select(_ => _.InnerText
                                        .Replace("\n", string.Empty)
                                        .Replace("\r", string.Empty)
                                        .Replace("\t", string.Empty)
                                        .Replace("&nbsp;", " ")).ToList();

                var recipeSteps = new List<RecipeStep>();

                recipeStepNodes.ForEach(_ => recipeSteps.Add(Context.RecipeSteps.First(s => s.Description == _)));

                var ingredientNodes = step.SelectNodes("//*[@class=\"ingredient\"]").Select(_ => new
                {
                    Names = _.SelectNodes("/html/body/div/div/div/div/div/article/div/div/div/div/table/tbody/tr//*[@class=\"name\"]").Select(n => n.InnerText).ToList(),
                    Quentities = _.SelectNodes("//*[@class=\"amount\"]").Select(q => q.InnerText).ToList()
                }).ToList();

                var ingredients = new List<Ingredient>();

                var i = 0;
                foreach (var node in ingredientNodes)
                {
                    var currentName = node.Names[i].Replace("&#171;", "\"").Replace("&#187;", "\"");
                    var currentQuantity = node.Quentities[i].Replace("&frac12;", "0,5").Replace("&frac14;", "0,25").Replace("&frac34;", "0,75");
                    var ing = Context.Ingredients.First(g => g.Product.Name == currentName & g.Quantity == currentQuantity);
                    ingredients.Add(ing);
                    if (i < node.Names.Count)
                    {
                        i++;
                    }
                }

                var recipe = new Recipe()
                {
                    Name = step.SelectSingleNode("//*[@class=\"fn s-recipe-name\"]").InnerText.Replace("&nbsp;", " "),
                    RecipeSteps = recipeSteps,
                    Ingredients = ingredients
                };

                Context.Recipes.Add(recipe);
            }

            Context.SaveChanges();
        }

        private static string GetHtmlString(string url)
        {
            _request = WebRequest.Create(url);
            _request.Proxy = null;
            _response = _request.GetResponse();

            using (var streamReader = new StreamReader(_response.GetResponseStream(), Encoding))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}