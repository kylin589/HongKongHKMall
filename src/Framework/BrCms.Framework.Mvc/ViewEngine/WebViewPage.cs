namespace BrCms.Framework.Mvc.ViewEngine
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        private ScriptRegister _scriptRegister;
        public ScriptRegister Script
        {
            get
            {
                return this._scriptRegister ?? (this._scriptRegister = new WebViewScriptRegister(this, this.Html.ViewDataContainer, ResourceType.Script));
            }
        }
        private ResourceRegister _stylesheetRegister;
        public ResourceRegister Resource
        {
            get
            {
                return this._stylesheetRegister ?? (this._stylesheetRegister = new ResourceRegister(this.Html.ViewDataContainer, ResourceType.Css));
            }
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}
