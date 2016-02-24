using System.Web.Mvc;
using System.Web.WebPages;

namespace BrCms.Framework.Mvc.ViewEngine
{
    public class WebViewScriptRegister : ScriptRegister
    {
        private readonly WebPageBase _viewPage;
        public WebViewScriptRegister(WebPageBase viewPage, IViewDataContainer container, ResourceType resourceType)
            : base(container, resourceType) 
        {
            this._viewPage = viewPage;
        }
    }
}
