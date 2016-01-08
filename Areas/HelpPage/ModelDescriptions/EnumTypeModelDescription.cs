// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumTypeModelDescription.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



using System.Collections.ObjectModel;

namespace KitchenHelperServer.Areas.HelpPage.ModelDescriptions
{
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}