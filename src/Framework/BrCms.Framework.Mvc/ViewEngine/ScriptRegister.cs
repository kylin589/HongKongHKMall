using System.Web.Mvc;

namespace BrCms.Framework.Mvc.ViewEngine
{
    public abstract class ScriptRegister:ResourceRegister
    {
        protected ScriptRegister(IViewDataContainer container,ResourceType resourceType)
            : base(container, resourceType)
        {
        }
    }
}
