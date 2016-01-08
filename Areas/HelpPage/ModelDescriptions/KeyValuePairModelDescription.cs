// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyValuePairModelDescription.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace KitchenHelperServer.Areas.HelpPage.ModelDescriptions
{
    public class KeyValuePairModelDescription : ModelDescription
    {
        public ModelDescription KeyModelDescription { get; set; }

        public ModelDescription ValueModelDescription { get; set; }
    }
}