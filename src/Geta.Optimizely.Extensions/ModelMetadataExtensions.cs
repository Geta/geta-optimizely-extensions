using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq.Expressions;

namespace Foundation.Core.Extensions
{
    public static class ModelMetadataExtensions
    {
        public static ModelMetadata GetModelMetadata<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expr)
        {
            var expressionProvider = html.ViewContext.HttpContext.RequestServices
                .GetService(typeof(ModelExpressionProvider)) as ModelExpressionProvider;

            return expressionProvider?.CreateModelExpression(html.ViewData, expr).Metadata;
        }

        public static bool TryGetAdditionalValue<T>(this ModelMetadata modelMetaData, string key, out T value)
        {
            value = default(T);

            if (modelMetaData == null)
            {
                return false;
            }

            if (!modelMetaData.AdditionalValues.ContainsKey(key))
            {
                return false;
            }

            value = (T)modelMetaData.AdditionalValues[key];
            return true;
        }

        public static T GetAdditionalValue<T>(this ModelMetadata modelMetaData, string key, T defaultValue)
        {
            T value;

            if (!TryGetAdditionalValue(modelMetaData, key, out value))
            {
                return defaultValue;
            }

            return value;
        }
    }
}