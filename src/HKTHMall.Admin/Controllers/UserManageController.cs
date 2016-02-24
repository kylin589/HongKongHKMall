using System.Web.Mvc;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models.User;
using HKTHMall.Services.Users;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class UserManageController : HKBaseController
    {
        private readonly IAC_UserService _acUserService;
        public UserManageController(IAC_UserService acUserService)
        {
            this._acUserService = acUserService;
        }
        //
        // GET: /AC_User/
        public PartialViewResult Index()
        {
            return this.PartialView();
        }

        public JsonResult Search(SearchUsersModel model)
        {
            var result = this._acUserService.GetPagingAC_Users(model);
            return this.Json(new {rows = result.Data, total = result.Data.TotalCount}, JsonRequestBehavior.AllowGet);
        }
	}
}