// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModelDocumentationProvider.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



using System;
using System.Reflection;

namespace KitchenHelperServer.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}