using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;

namespace BrCms.Framework.Mvc.ViewEngine
{
    public class ResourceRegister
    {
        private readonly string _viewVirtualPath;
        public ResourceRegister(IViewDataContainer container, ResourceType resourceType)
        {
            var templateControl = container as TemplateControl;
            if (templateControl != null)
            {
                this._viewVirtualPath = templateControl.AppRelativeVirtualPath;
            }
            else
            {
                var webPage = container as WebPageBase;
                if (webPage != null)
                {
                    this._viewVirtualPath = webPage.VirtualPath;
                }
            }
            this.ResourceType = resourceType;
        }

        protected ResourceType ResourceType { get; private set; }
        public MvcHtmlString Include(string resourceName)
        {
            StringBuilder sb = new StringBuilder();
            switch(this.ResourceType)
            {
                case ResourceType.Script:
                    sb.Append(
                        string.Format(@"<script type='text/javascript' src='{0}'></script>", Path.Combine(ResourceDefinition.GetBasePathFromViewPath(this.ResourceType, this._viewVirtualPath), resourceName)
                        ));
                    break;
                case ResourceType.Css:
                    sb.Append(
                        string.Format(@"<link rel='Stylesheet' type='text/css' src='{0}'></script>", Path.Combine(ResourceDefinition.GetBasePathFromViewPath(this.ResourceType, this._viewVirtualPath), resourceName)
                        ));
                    break;
            }
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
