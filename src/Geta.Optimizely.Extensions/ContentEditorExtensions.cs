﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Web;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Geta.Optimizely.Extensions
{
    /// <summary>
    ///     <see cref="HtmlHelper"/> extension methods for improved content editor user experience.
    /// </summary>
    public static class ContentEditorExtensions
    {
        /// <summary>
        ///     Render help hint for current <see cref="IContentData"/> instance resolved from lambda expression.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expr"></param>
        /// <param name="helpText"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditorHelp<TModel>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, IContentData>> expr,
            string helpText)
        {
            var modelMetadata = CreateContentModelMetadata(helper, expr);
            // ReSharper disable once SuspiciousTypeConversion.Global
            var content = modelMetadata as IContentData;
            return EditorHelp(helper, content, helpText);
        }

        /// <summary>
        ///     Render help hint for current <see cref="IContentData"/> instance.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="helpText"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditorHelp<TModel>(this HtmlHelper<TModel> helper, string helpText) where TModel : IContentData
        {
            var content = helper.ViewData.Model as IContentData;
            return EditorHelp(helper, content, helpText);
        }

        /// <summary>
        ///     Render help hint for <see cref="IContentData"/> instance.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="content"></param>
        /// <param name="helpText"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditorHelp<TModel>(this HtmlHelper<TModel> helper, IContentData content, string helpText)
        {
            if (!PageIsInEditMode(helper))
            {
                return null;
            }

            if (IsBlockAndNotInPreview(helper, content.GetOriginalType()))
            {
                return null;
            }

            return new HtmlString(GetHintsTag(helpText).ToString());
        }

        /// <summary>
        ///     Render help hint for HelpTextPropertyName resolved from lambda expression.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expr"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditorHelpFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expr)
        {
            if (!PageIsInEditMode(helper))
            {
                return null;
            }

            var modelMetadata = CreatePropertyModelMetadata(helper, expr);
            if (IsBlock(modelMetadata.ContainerType) && !IsBlockPreviewTemplate(helper))
            {
                return null;
            }

            if (!modelMetadata.AdditionalValues.ContainsKey(MetadataConstants.EditorHelp.HelpTextPropertyName))
            {
                return null;
            }

            var hint = modelMetadata.AdditionalValues[MetadataConstants.EditorHelp.HelpTextPropertyName] as string;
            if (string.IsNullOrWhiteSpace(hint))
            {
                return null;
            }

            var tag = GetHintsTag(hint);
            return new HtmlString(tag.ToString());
        }

        /// <summary>
        ///     Render editor help summary for current <see cref="IContentData"/> instance.
        /// </summary>
        /// <param name="helper"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditorHelpSummary<TModel>(this HtmlHelper<TModel> helper) where TModel : IContentData
        {
            return EditorHelpSummary(helper, helper.ViewData.Model);
        }

        /// <summary>
        ///     Render editor help summary for current <see cref="IContentData"/> instance resolved from lambda expression.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expr"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditorHelpSummary<TModel>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, IContentData>> expr)
        {
            if (!PageIsInEditMode(helper))
            {
                return null;
            }

            var modelMetadata = CreateContentModelMetadata(helper, expr);
            // ReSharper disable once SuspiciousTypeConversion.Global
            return EditorHelpSummary(helper, modelMetadata as IContentData);
        }

        /// <summary>
        ///     Render editor help summary for <see cref="IContentData"/> instance.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="content"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditorHelpSummary<TModel>(this HtmlHelper<TModel> helper, IContentData content)
        {
            if (!PageIsInEditMode(helper))
            {
                return null;
            }

            var contentType = content.GetOriginalType();

            if (IsBlockAndNotInPreview(helper, contentType))
            {
                return null;
            }

            var propertiesMeta = new EmptyModelMetadataProvider().GetMetadataForType(contentType).Properties;

            bool ShowInSummary(ModelMetadata metadata)
            {
                return metadata.AdditionalValues.TryGetValue(MetadataConstants.EditorHelp.ShowInSummaryPropertyName,
                                                             out var showInSummaryObj)
                       && ((bool?)showInSummaryObj).GetValueOrDefault(true);
            }

            bool HasHelpText(ModelMetadata metadata)
            {
                return metadata.AdditionalValues.TryGetValue(MetadataConstants.EditorHelp.HelpTextPropertyName,
                                                             out var hintObj)
                       && !string.IsNullOrEmpty(hintObj as string);
            }

            string HelpText(ModelMetadata metadata)
            {
                return metadata.AdditionalValues[MetadataConstants.EditorHelp.HelpTextPropertyName] as string;
            }

            var hints = propertiesMeta
                .Where(x => ShowInSummary(x) && HasHelpText(x))
                .Select(HelpText)
                .ToList();

            if (hints.Count <= 0)
            {
                return null;
            }

            var tag = GetHintsTag(hints);
            return new HtmlString(tag.ToString());
        }

        /// <summary>
        ///     Render edit button for property resolved from lambda expression./> instance.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expr"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditButtonFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expr)
        {
            if (!PageIsInEditMode(helper))
            {
                return null;
            }

            var modelMetadata = CreatePropertyModelMetadata(helper, expr);

            if (IsBlockAndNotInPreview(helper, modelMetadata.ContainerType))
            {
                return null;
            }

            string iconCssClass = null;

            if (modelMetadata.AdditionalValues.ContainsKey(MetadataConstants.EditButton.IconCssClassPropertyName))
            {
                iconCssClass = modelMetadata.AdditionalValues[MetadataConstants.EditButton.IconCssClassPropertyName] as string;
            }

            var tag = GetEditButtonTag(helper,
                                       expr,
                                       modelMetadata.AdditionalValues[MetadataConstants.EditButton.ButtonLabel] as string
                                       ?? modelMetadata.DisplayName,
                                       iconCssClass);
            return new HtmlString(tag);
        }

        /// <summary>
        ///     Render edit buttons for current <see cref="IContentData"/> instance.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="includeBuiltInProperties">If true, also renders edit button for built-in Category property.</param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditButtonsGroup<TModel>(this HtmlHelper<TModel> helper, bool includeBuiltInProperties = false)
            where TModel : IContentData
        {
            return EditButtonsGroup(helper, helper.ViewData.Model, includeBuiltInProperties);
        }

        /// <summary>
        ///     Render edit buttons for <see cref="IContentData"/> instance resolved from lambda expression.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expr"></param>
        /// <param name="includeBuiltInProperties">If true, also renders edit button for built-in Category property.</param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditButtonsGroup<TModel>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, IContentData>> expr,
            bool includeBuiltInProperties = false)
        {
            if (!PageIsInEditMode(helper))
            {
                return null;
            }

            var modelMetadata = CreateContentModelMetadata(helper, expr);
            // ReSharper disable once SuspiciousTypeConversion.Global
            return EditButtonsGroup(helper, modelMetadata as IContentData, includeBuiltInProperties);
        }

        /// <summary>
        ///     Render edit buttons for <see cref="IContentData"/> instance.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="content"></param>
        /// <param name="includeBuiltInProperties">If true, also renders edit button for built-in Category property.</param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlContent EditButtonsGroup<TModel>(
            this HtmlHelper<TModel> helper,
            IContentData content,
            bool includeBuiltInProperties = false)
        {
            if (!PageIsInEditMode(helper))
            {
                return null;
            }

            var contentType = content.GetOriginalType();

            if (IsBlockAndNotInPreview(helper, contentType))
            {
                return null;
            }

            var propertiesMeta = new EmptyModelMetadataProvider().GetMetadataForType(contentType).Properties;

            bool ShowInGroup(ModelMetadata metadata)
            {
                return metadata.AdditionalValues.TryGetValue(MetadataConstants.EditButton.ShowInGroupPropertyName,
                                                             out var showInGroupObj)
                       && ((bool?)showInGroupObj).GetValueOrDefault(true);
            }

            bool CanTriggerFullRefresh(ModelMetadata metadata)
            {
                return metadata.TryGetAdditionalValue(MetadataConstants.EditButton.TriggerFullRefreshPropertyName, out bool _);
            }

            var visibleMeta = propertiesMeta.Where(x => x.ShowForEdit && ShowInGroup(x)).ToList();
            var iconCssDivs = visibleMeta.Select(x => GetEditButtonTag(helper, x)).ToList();
            var fullRefreshPropertyNames = visibleMeta.Where(CanTriggerFullRefresh).Select(x => x.PropertyName).ToList();

            IncludeBuiltinProperties(content, includeBuiltInProperties, iconCssDivs, fullRefreshPropertyNames);

            if (iconCssDivs.Count <= 0)
            {
                return null;
            }

            var container = new TagBuilder("div");
            container.AddCssClass("editor-buttons");

            foreach (var iconCssDiv in iconCssDivs)
            {
                container.InnerHtml.Append(iconCssDiv);
            }

            return new HtmlString(container.ToString() +
                                  helper.FullRefreshPropertiesMetaData(fullRefreshPropertyNames.ToArray()));
        }

        private static void IncludeBuiltinProperties(
            IContentData content,
            bool includeBuiltInProperties,
            ICollection<string> iconCssDivs,
            ICollection<string> fullRefreshPropertyNames)
        {
            if (!includeBuiltInProperties)
            {
                return;
            }

            if (content is ICategorizable)
            {
                iconCssDivs.Add(GetSpecialEditButtonTag("Category",
                                                        LocalizationService.Current.GetString(
                                                            "/contenttypes/icontentdata/properties/icategorizable_category/caption"),
                                                        null));
                fullRefreshPropertyNames.Add("icategorizable_category");
            }

            if (content is PageData)
            {
                iconCssDivs.Add(GetSpecialEditButtonTag("PageExternalURL",
                                                        LocalizationService.Current.GetString(
                                                            "/contenttypes/icontentdata/properties/pageexternalurl/caption"),
                                                        null));
                iconCssDivs.Add(GetSpecialEditButtonTag("PageVisibleInMenu",
                                                        LocalizationService.Current.GetString(
                                                            "/contenttypes/icontentdata/properties/pagevisibleinmenu/caption"),
                                                        null));
            }

            if (content is IRoutable)
            {
                iconCssDivs.Add(GetSpecialEditButtonTag("iroutable_routesegment",
                                                        LocalizationService.Current.GetString(
                                                            "/contenttypes/icontentdata/properties/pageurlsegment/caption"),
                                                        null));
            }

            if (content is IVersionable)
            {
                iconCssDivs.Add(GetSpecialEditButtonTag("iversionable_startpublish",
                                                        LocalizationService.Current.GetString(
                                                            "/contenttypes/icontentdata/properties/iversionable_startpublish/caption"),
                                                        null));
            }

            if (content is IChangeTrackable)
            {
                iconCssDivs.Add(GetSpecialEditButtonTag("ichangetrackable_setchangedonpublish",
                                                        LocalizationService.Current.GetString(
                                                            "/contenttypes/icontentdata/properties/ichangetrackable_setchangedonpublish/caption"),
                                                        null));
            }
        }

        private static string GetEditButtonTag<TModel>(HtmlHelper<TModel> helper, ModelMetadata metadata)
        {
            metadata.AdditionalValues.TryGetValue(MetadataConstants.EditButton.IconCssClassPropertyName, out var iconCssClassObj);
            var iconCssClass = iconCssClassObj as string;
            return GetEditButtonTag(helper,
                                    metadata.PropertyName,
                                    metadata.AdditionalValues[MetadataConstants.EditButton.ButtonLabel] as string ??
                                    metadata.DisplayName ?? metadata.PropertyName,
                                    iconCssClass);
        }

        private static string GetEditButtonTag<TModel, TProperty>(
            HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expr,
            string displayName,
            string iconCssClass)
        {
            var withIconCssClass = !string.IsNullOrWhiteSpace(iconCssClass)
                ? "with-icon "
                : null;

            var html = new StringBuilder();
            html.Append("<div ");
            html.AppendFormat("class=\"editor-button-wrapper {1}{0}\" ", iconCssClass, withIconCssClass);
            html.AppendFormat(">{0}", displayName);
            html.Append("<span class=\"editor-button\"");
            html.Append(helper.EditAttributes(expr));
            html.Append("></span>");
            html.Append("</div>");
            return html.ToString();
        }

        private static string GetEditButtonTag<TModel>(
            HtmlHelper<TModel> helper,
            string propertyName,
            string displayName,
            string iconCssClass)
        {
            var withIconCssClass = !string.IsNullOrWhiteSpace(iconCssClass)
                ? "with-icon "
                : null;

            var html = new StringBuilder();
            html.Append("<div ");
            html.AppendFormat("class=\"editor-button-wrapper {1}{0}\" ", iconCssClass, withIconCssClass);
            html.AppendFormat(">{0}", displayName);
            html.Append("<span class=\"editor-button\"");
            html.Append(helper.EditAttributes(propertyName));
            html.Append("></span>");
            html.Append("</div>");
            return html.ToString();
        }

        private static string GetSpecialEditButtonTag(string propertyName, string displayName, string iconCssClass)
        {
            var withIconCssClass = !string.IsNullOrWhiteSpace(iconCssClass)
                ? "with-icon "
                : null;

            var html = new StringBuilder();
            html.Append("<div ");
            html.AppendFormat("class=\"editor-button-wrapper {1}{0}\" ", iconCssClass, withIconCssClass);
            html.AppendFormat(">{0}", displayName);
            html.Append("<span class=\"editor-button\"");
            html.AppendFormat(" data-epi-property-name=\"{0}\" data-epi-use-mvc=\"True\"", propertyName);
            html.Append("></span>");
            html.Append("</div>");
            return html.ToString();
        }

        private static TagBuilder GetHintsTag(string hint)
        {
            return GetHintsTag(new[] { hint });
        }

        private static TagBuilder GetHintsTag(IEnumerable<string> hints)
        {
            var tag = new TagBuilder("div");
            tag.AddCssClass("editor-help");
            var ul = new TagBuilder("ul");

            foreach (var hint in hints)
            {
                var li = new TagBuilder("li");
                li.InnerHtml.Append(hint);

                ul.InnerHtml.Append(li.ToString() ?? string.Empty);
            }

            tag.InnerHtml.Append(ul.ToString() ?? string.Empty);
            return tag;
        }

        private static bool IsBlock(Type contentType)
        {
            return contentType != null && typeof(BlockData).IsAssignableFrom(contentType);
        }

        private static bool IsBlockPreviewTemplate<TModel>(HtmlHelper<TModel> helper)
        {
            return helper.ViewContext.IsBlockPreview();
        }

        private static bool IsBlockAndNotInPreview<TModel>(this HtmlHelper<TModel> helper, Type contentType)
        {
            return IsBlock(contentType) && !IsBlockPreviewTemplate(helper);
        }

        private static bool PageIsInEditMode<TModel>(IHtmlHelper<TModel> helper)
        {
            var mode = helper.ViewContext.HttpContext.RequestServices.GetRequiredService<IContextModeResolver>().CurrentMode;
            return mode == ContextMode.Edit;
        }

        private static ModelMetadata CreateContentModelMetadata<TModel>(
            HtmlHelper<TModel> helper,
            Expression<Func<TModel, IContentData>> expr)
        {
            var expressionProvider = GetModelExpressionProvider(helper);
            return expressionProvider?.CreateModelExpression(helper.ViewData, expr).Metadata;
        }

        private static ModelMetadata CreatePropertyModelMetadata<TModel, TProperty>(
            HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expr)
        {
            var expressionProvider = GetModelExpressionProvider(helper);
            return expressionProvider?.CreateModelExpression(helper.ViewData, expr).Metadata;
        }

        private static ModelExpressionProvider GetModelExpressionProvider<TModel>(HtmlHelper<TModel> helper)
        {
            return helper.ViewContext.HttpContext.RequestServices
                .GetService(typeof(ModelExpressionProvider)) as ModelExpressionProvider;
        }
    }
}
