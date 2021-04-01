using System;
using EPiServer.Shell.ObjectEditing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Geta.EPi.Extensions.EditorDescriptors
{
    /// <summary>
    /// EPiServer Editor descriptor for creating custom properties with enum types as options
    /// Source: http://world.episerver.com/Blogs/Linus-Ekstrom/Dates/2014/5/Enum-properties-for-EPiServer-75/
    /// </summary>
    public class EnumAttribute : SelectOneAttribute
    {
        /// <summary>
        /// Enum type in EnumAttribute
        /// </summary>
        public Type EnumType { get; set; }

        /// <summary>
        /// Enum Attribute
        /// </summary>
        /// <param name="enumType">Type of enum</param>
        public EnumAttribute(Type enumType)
        {
            EnumType = enumType;
        }

        public void OnMetadataCreated(ExtendedMetadata metadata)
        {
            SelectionFactoryType = typeof(EnumSelectionFactory<>).MakeGenericType(EnumType);
            var key = ModelMetadataIdentity.ForType(typeof(Enum));
            var displayMetadataProvider = new DisplayMetadataProviderContext(key, metadata.Attributes as ModelAttributes);
            base.CreateDisplayMetadata(displayMetadataProvider);
        }
    }

}