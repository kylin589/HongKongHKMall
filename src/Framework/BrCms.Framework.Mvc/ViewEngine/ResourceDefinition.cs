using System;

namespace BrCms.Framework.Mvc.ViewEngine
{
    internal class ResourceDefinition
    {
        internal static string GetBasePathFromViewPath(ResourceType resourceType, string viewPath)
        {
            if (String.IsNullOrEmpty(viewPath))
            {
                return null;
            }
            string basePath = null;
            var viewsPartIndex = viewPath.IndexOf("/Views", StringComparison.OrdinalIgnoreCase);
            if (viewsPartIndex >= 0)
            {
                basePath = viewPath.Substring(0, viewsPartIndex + 1);
            }
            return basePath.Replace("~","");
        }
    }
}
