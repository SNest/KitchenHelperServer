// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexTypeModelDescription.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



using System.Collections.ObjectModel;

namespace KitchenHelperServer.Areas.HelpPage.ModelDescriptions
{
    public class ComplexTypeModelDescription : ModelDescription
    {
        public ComplexTypeModelDescription()
        {
            Properties = new Collection<ParameterDescription>();
        }

        public Collection<ParameterDescription> Properties { get; private set; }
    }
}