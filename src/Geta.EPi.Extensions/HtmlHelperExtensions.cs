using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;

namespace Geta.EPi.Extensions
{
    /// <summary>
    /// HtmlHelper extensions.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Renders the link to the action if has permissions.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlContent AuthorizedActionLink(
            this HtmlHelper helper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues,
            object htmlAttributes)
        {
            if (HasActionPermission(helper))
            {
                return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }

            return HtmlString.Empty;
        }

        /// <summary>
        /// Renders the link to the action if has permissions.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static IHtmlContent AuthorizedActionLink(
            this HtmlHelper helper,
            string linkText,
            string actionName,
            string controllerName)
        {
            if (HasActionPermission(helper))
            {
                return helper.ActionLink(linkText, actionName, controllerName);
            }

            return HtmlString.Empty;
        }

        /// <summary>
        /// Renders the link to the action if has permissions.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlContent AuthorizedActionLink(
            this HtmlHelper helper,
            string linkText,
            string actionName,
            string controllerName,
            RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes)
        {
            if (HasActionPermission(helper))
            {
                return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }

            return HtmlString.Empty;
        }

        /// <summary>
        /// Checks if a user has permission to the controller's action.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static bool HasActionPermission(this HtmlHelper htmlHelper)
        {
            return ActionIsAuthorized(htmlHelper.ViewContext.ActionDescriptor, htmlHelper.ViewContext);
        }

        private static bool ActionIsAuthorized(ActionDescriptor actionDescriptor, ActionContext actionContext)
        {
            if (actionDescriptor == null)
                return false;

            var filters = actionDescriptor.FilterDescriptors.Select(x => x.Filter).ToList();
            var authContextHandler = new AuthorizationFilterContext(actionContext, filters);
            if (authContextHandler.Result != null)
            {
                return false;
            }

            return true;
        }
    }
}