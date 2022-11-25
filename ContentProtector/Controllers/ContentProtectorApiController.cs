using System;
using System.Collections.Generic;
using System.Linq;
using ContentProtector.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;
using Umbraco.Web.Editors;

namespace ContentProtector.Controllers
{
    public class ContentProtectorApiController : UmbracoAuthorizedJsonController
    {
        private readonly IScopeProvider _scopeProvider;
        public ContentProtectorApiController(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public IEnumerable<ActionModel> GetAll()
        {
            try
            {
                IEnumerable<ActionModel> value;
                using (IScope scope = _scopeProvider.CreateScope(autoComplete: true))
                {
                    NPoco.Sql<ISqlContext> sql = scope.SqlContext.Sql().Select("*").From<ActionModel>().Where<ActionModel>(x => x.id != 2 && x.id != 5);
                    value = scope.Database.Fetch<ActionModel>(sql);
                }
                return value;
            }
            catch (Exception ex)
            {
                Logger.Error<ActionModel>("Failed to get Content Protector settings", ex.Message);
            }
            return null;
        }
        public ActionModel GetById(int id)
        {
            try
            {
                ActionModel value;
                using (IScope scope = _scopeProvider.CreateScope(autoComplete: true))
                {
                    NPoco.Sql<ISqlContext> sql = scope.SqlContext.Sql().Select("*").From<ActionModel>()
                    .Where<ActionModel>(x => x.id == id);
                    value = scope.Database.Fetch<ActionModel>(sql).FirstOrDefault();
                }
                return value;
            }
            catch (Exception ex)
            {
                Logger.Error<ActionModel>("Failed to get Content Protector settings", ex.Message);
            }
            return null;
        }

        public ActionModel Save(ActionModel model)
        {

            try
            {
                using (IScope scope = _scopeProvider.CreateScope(autoComplete: true))
                {

                    NPoco.Sql<ISqlContext> sql = scope.SqlContext.Sql().Select("*").From<ActionModel>()
                                    .Where<ActionModel>(x => x.id == model.id);

                    ActionModel value = scope.Database.Fetch<ActionModel>(sql).FirstOrDefault();
                    value.disableAction = model.disableAction;
                    value.lastEdited = DateTime.Now;
                    value.lastEditedBy = UmbracoContext.Security.CurrentUser.Name;
                    value.nodes = model.nodes;
                    value.userExceptions = model.userExceptions;
                    scope.Database.Update(value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error<ActionModel>("Failed to save Content Protector settings for the" + model.name + " action", ex.Message);
            }
            return model;
        }
    }
}