using EPiServer.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     ViewContext extension method.
    /// </summary>
    public static class ViewContextExtensions
    {
        /// <summary>
        /// Checks if currently in Block preview controller.
        /// </summary>
        /// <param name="viewContext"></param>
        /// <returns></returns>
        public static bool IsBlockPreview(this ViewContext viewContext)
        {
            return viewContext.HttpContext.IsBlockPreview();
        }

        /// <summary>
        /// Checks if block is in edit mode.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsBlockInEditMode(this ViewContext context)
        {
            var mode = context.HttpContext.RequestServices.GetRequiredService<IContextModeResolver>().CurrentMode;
            return mode == ContextMode.Edit && context.IsBlockPreview();
        }
    }
}