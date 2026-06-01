# Changelog

All notable changes to this project will be documented in this file.

## [3.0.0]

### Changed

- Upgraded to Optimizely CMS 13 (`EPiServer.CMS.AspNetCore.HtmlHelpers` and `EPiServer.CMS.UI.Core` 13.0.2).
- Upgraded target framework to .NET 10.
- Upgraded `Geta.Net.Extensions` to 3.0.1.
- `ContentAreaExtensions.HasContent` no longer relies on the removed `ContentArea.FilteredItems`. It now filters `Items` using `IPublishedStateAssessor` and `IContentAccessEvaluator`, so unpublished, expired and access-restricted items are still excluded.
- `JsonExtensions.ToJson` now reuses cached `JsonSerializerOptions` instances per `includeNull` value to keep the `System.Text.Json` metadata cache warm and avoid per-call allocations.
- `StringExtensions.StripHtml` now passes a regex match timeout (ReDoS guard) and decodes HTML entities via `WebUtility.HtmlDecode` before truncating, restoring the entity-decoding behavior of the old `TextIndexer.StripHtml`.
- Sample web host and test project upgraded to .NET 10; test dependencies modernized (xUnit, FluentAssertions, Moq, coverlet, Microsoft.NET.Test.Sdk, Castle.Core).

### Removed

- Support for Optimizely CMS 12 and earlier (now requires CMS 13).
- Support for .NET versions prior to .NET 10.

## [1.0.2]

- Small refactorings

## [1.0.1]

- Fix: Only remove default port when adding host [#18](https://github.com/Geta/geta-optimizely-extensions/pull/18)

## [1.0.0]

### Changed

- Upgraded to Optimizely 12
- Removed XFormHelper's (EPiServer CMS version 12 doesnt support XForms)
