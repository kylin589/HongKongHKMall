2016-01-30 11:39:57,576 [20] ERROR 引发异常的方法：Void OnError(System.Data.SqlClient.SqlException, Boolean, System.Action`1[System.Action])
错误信息：'&' 附近有语法错误。
错误堆栈：   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteDbDataReader(CommandBehavior behavior)
   在 System.Data.Common.DbCommand.System.Data.IDbCommand.ExecuteReader()
   在 Simple.Data.RawSql.DbCommandExtensions.ToResultSets(IDbCommand command) 位置 f:\WorkSpace\src\Framework\Simple.Data.RawSql\DbCommandExtensions.cs:行号 13
   在 Simple.Data.RawSql.DbConnectionExtensions.<>c__DisplayClass1.<ToResultSets>b__0() 位置 f:\WorkSpace\src\Framework\Simple.Data.RawSql\DbConnectionExtensions.cs:行号 24
   在 Simple.Data.RawSql.DbConnectionExtensions.WithOpenConnection[T](IDbConnection connection, Func`1 func) 位置 f:\WorkSpace\src\Framework\Simple.Data.RawSql\DbConnectionExtensions.cs:行号 142
   在 Simple.Data.RawSql.DbConnectionExtensions.ToResultSets(IDbConnection connection, String sql, IDictionary`2 parameters) 位置 f:\WorkSpace\src\Framework\Simple.Data.RawSql\DbConnectionExtensions.cs:行号 24
   在 Simple.Data.RawSql.AdoAdapterExtensions.ToResultSets(AdoAdapter adapter, String sql, IDictionary`2 parameters) 位置 f:\WorkSpace\src\Framework\Simple.Data.RawSql\AdoAdapterExtensions.cs:行号 19
   在 Simple.Data.RawSql.DatabaseExtensions.ToResultSets(Database db, String sql, IDictionary`2 parameters) 位置 f:\WorkSpace\src\Framework\Simple.Data.RawSql\DatabaseExtensions.cs:行号 15
   在 Simple.Data.RawSql.DatabaseExtensions.ToResultSets(Database db, String sql, KeyValuePair`2[] parameters) 位置 f:\WorkSpace\src\Framework\Simple.Data.RawSql\DatabaseExtensions.cs:行号 21
   在 HKTHMall.Services.ShoppingCart.Impl.ShoppingCartService.<>c__DisplayClass9e.<GetShoppingCartByUserId>b__9d(Database x) 位置 f:\WorkSpace\src\HKTHMall.Services\ShoppingCart\Impl\ShoppingCartService.cs:行号 342
   在 CallSite.Target(Closure , CallSite , Func`2 , Object )
   在 BrCms.Framework.Data.DatabaseHelper.RunSqlQuery(Func`2 dbQuery) 位置 f:\WorkSpace\src\Framework\BrCms.Framework\Data\DatabaseHelper.cs:行号 37
   在 HKTHMall.Services.ShoppingCart.Impl.ShoppingCartService.GetShoppingCartByUserId(String userid, Int32 languageId) 位置 f:\WorkSpace\src\HKTHMall.Services\ShoppingCart\Impl\ShoppingCartService.cs:行号 342
   在 HKTHMall.Services.ShoppingCart.Impl.ShoppingCartService.getCartFromDb(Int32 languageId, String userid, YH_User u) 位置 f:\WorkSpace\src\HKTHMall.Services\ShoppingCart\Impl\ShoppingCartService.cs:行号 225
   在 HKTHMall.Services.ShoppingCart.Impl.ShoppingCartService.getGoodsGroupByCom(String getChecked, Int32 languageId, String userid) 位置 f:\WorkSpace\src\HKTHMall.Services\ShoppingCart\Impl\ShoppingCartService.cs:行号 422
   在 Castle.Proxies.Invocations.IShoppingCartService_getGoodsGroupByCom.InvokeMethodOnTarget()
   在 Castle.DynamicProxy.AbstractInvocation.Proceed()
   在 Castle.Proxies.IShoppingCartServiceProxy.getGoodsGroupByCom(String getChecked, Int32 languageId, String userid)
   在 HKTHMall.Web.Areas.Money.Controllers.ShoppingCartController.getGoodsGroupByCom(String getChecked) 位置 f:\WorkSpace\src\HKTHMall.Web\Areas\Money\Controllers\ShoppingCartController.cs:行号 131
   在 lambda_method(Closure , ControllerBase , Object[] )
   在 System.Web.Mvc.ActionMethodDispatcher.Execute(ControllerBase controller, Object[] parameters)
   在 System.Web.Mvc.ReflectedActionDescriptor.Execute(ControllerContext controllerContext, IDictionary`2 parameters)
   在 System.Web.Mvc.ControllerActionInvoker.InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary`2 parameters)
   在 System.Web.Mvc.Async.AsyncControllerActionInvoker.<BeginInvokeSynchronousActionMethod>b__39(IAsyncResult asyncResult, ActionInvocation innerInvokeState)
   在 System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncResult`2.CallEndDelegate(IAsyncResult asyncResult)
   在 System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncResultBase`1.End()
   在 System.Web.Mvc.Async.AsyncControllerActionInvoker.EndInvokeActionMethod(IAsyncResult asyncResult)
   在 System.Web.Mvc.Async.AsyncControllerActionInvoker.AsyncInvocationWithFilters.<InvokeActionMethodFilterAsynchronouslyRecursive>b__3d()
   在 System.Web.Mvc.Async.AsyncControllerActionInvoker.AsyncInvocationWithFilters.<>c__DisplayClass46.<InvokeActionMethodFilterAsynchronouslyRecursive>b__3f()
   在 System.Web.Mvc.Async.AsyncControllerActionInvoker.<>c__DisplayClass33.<BeginInvokeActionMethodWithFilters>b__32(IAsyncResult asyncResult)
   在 System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncResult`1.CallEndDelegate(IAsyncResult asyncResult)
   在 System.Web.Mvc.Async.AsyncResultWrapper.WrappedAsyncResultBase`1.End()
   在 System.Web.Mvc.Async.AsyncControllerActionInvoker.EndInvokeActionMethodWithFilters(IAsyncResult asyncResult)
   在 System.Web.Mvc.Async.AsyncControllerActionInvoker.<>c__DisplayClass21.<>c__DisplayClass2b.<BeginInvokeAction>b__1c()
   在 System.Web.Mvc.Async.AsyncControllerActionInvoker.<>c__DisplayClass21.<BeginInvokeAction>b__1e(IAsyncResult asyncResult)
错误发生地址：/money/ShoppingCart/getGoodsGroupByCom
 - [f:\WorkSpace\src\Framework\BrCms.Framework\Logging\Logger.cs:26]
