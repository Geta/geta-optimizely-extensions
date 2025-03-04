﻿using System.Collections.Generic;
using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;

namespace Geta.Optimizely.Extensions
{
    /// <summary>
    ///     Category extensions.
    /// </summary>
    public static class CategoryExtensions
    {
        #pragma warning disable 649
        private static Injected<CategoryRepository> CategoryRepository;
        #pragma warning restore 649

        /// <summary>
        ///     Returns strongly typed child categories of provided parent category ID.
        /// </summary>
        /// <param name="categoryRootId">Parent category ID.</param>
        /// <returns>Enumeration of child categories.</returns>
        public static IEnumerable<Category> GetChildCategories(this int categoryRootId)
        {
            var root = CategoryRepository.Service.Get(categoryRootId);
            return root?.Categories ?? Enumerable.Empty<Category>();
        }
    }
}
