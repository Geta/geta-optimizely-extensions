using EPiServer.Shell.ObjectEditing;
using System;

namespace Geta.Optimizely.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EditorHelpAttribute : Attribute
    {
        public string Hint { get; set; }
        public bool ShowInSummary { get; set; }

        public EditorHelpAttribute(string hint) : this(hint, true)
        {
        }

        public EditorHelpAttribute(string hint, bool showInSummary)
        {
            Hint = hint;
            ShowInSummary = showInSummary;
        }

        public void OnMetadataCreated(ExtendedMetadata metadata)
        {
            metadata.AdditionalValues[MetadataConstants.EditorHelp.HelpTextPropertyName] = Hint;
            metadata.AdditionalValues[MetadataConstants.EditorHelp.ShowInSummaryPropertyName] = ShowInSummary;
        }
    }
}