using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using Xunit;

namespace Geta.Optimizely.Extensions.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void MemberOf_SourceListIsNull_ThenReturnFalse()
        {
            IEnumerable<ContentReference> sourceList = null;
            
            var result = sourceList.MemberOf(new ContentReference(1));

            Assert.False(result);
        }

        [Fact]
        public void MemberOf_SourceListDoesNotContainRef_ThenReturnFalse()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2)
            };

            var result = sourceList.MemberOf(new ContentReference(3));

            Assert.False(result);
        }

        [Fact]
        public void MemberOf_SourceListContainsRef_ThenReturnTrue()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2)
            };

            var result = sourceList.MemberOf(new ContentReference(2));

            Assert.True(result);
        }

        [Fact]
        public void MemberOf_SourceListContainsRefWithDifferentWorkingVersion_ThenReturnTrue()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2, 3)
            };

            var result = sourceList.MemberOf(new ContentReference(2, 2));

            Assert.True(result);
        }

        [Fact]
        public void MemberOfAny_TargetListIsNull_ThenReturnTrue()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2)
            };
            IEnumerable<ContentReference> targetList = null;

            var result = sourceList.MemberOfAny(targetList);

            Assert.True(result);
        }

        [Fact]
        public void MemberOfAny_TargetListIsEmpty_ThenReturnTrue()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2)
            };
            IEnumerable<ContentReference> targetList = new List<ContentReference>() {};

            var result = sourceList.MemberOfAny(targetList);

            Assert.True(result);
        }

        [Fact]
        public void MemberOfAny_SourceListIsNull_ThenReturnFalse()
        {
            IEnumerable<ContentReference> sourceList = null;
            IEnumerable<ContentReference> targetList = new List<ContentReference>() { new ContentReference(2) };

            var result = sourceList.MemberOfAny(targetList);

            Assert.False(result);
        }

        [Fact]
        public void MemberOfAny_SourceListContainsRefWithDifferentWorkingVersion_ThenReturnTrue()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2, 3)
            };
            IEnumerable<ContentReference> targetList = new List<ContentReference>() { new ContentReference(2) };

            var result = sourceList.MemberOfAny(targetList);

            Assert.True(result);
        }

        [Fact]
        public void MemberOfAny_SourceListContainsAnyMatches_ThenReturnTrue()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2)
            };
            IEnumerable<ContentReference> targetList = new List<ContentReference>() {
                new ContentReference(2),
                new ContentReference(3)
            };

            var result = sourceList.MemberOfAny(targetList);

            Assert.True(result);
        }

        [Fact]
        public void MemberOfAny_SourceListDoesNotContainAnyMatches_ThenReturnFalse()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2)
            };
            IEnumerable<ContentReference> targetList = new List<ContentReference>() {
                new ContentReference(3),
                new ContentReference(4)
            };

            var result = sourceList.MemberOfAny(targetList);

            Assert.False(result);
        }

        [Fact]
        public void MemberOfAll_TargetListIsNull_ThenReturnTrue()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2)
            };
            IEnumerable<ContentReference> targetList = null;

            var result = sourceList.MemberOfAll(targetList);

            Assert.True(result);
        }

        [Fact]
        public void MemberOfAll_TargetListIsEmpty_ThenReturnTrue()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2)
            };
            IEnumerable<ContentReference> targetList = new List<ContentReference>() { };

            var result = sourceList.MemberOfAll(targetList);

            Assert.True(result);
        }

        [Fact]
        public void MemberOfAll_SourceListIsNull_ThenReturnFalse()
        {
            IEnumerable<ContentReference> sourceList = null;
            IEnumerable<ContentReference> targetList = new List<ContentReference>() { new ContentReference(2) };

            var result = sourceList.MemberOfAll(targetList);

            Assert.False(result);
        }

        [Fact]
        public void MemberOfAll_SourceListContainsRefWithDifferentWorkingVersion_ThenReturnTrue()
        {
            IEnumerable<ContentReference> sourceList = new List<ContentReference>() {
                new ContentReference(1),
                new ContentReference(2, 3)
            };
            IEnumerable<ContentReference> targetList = new List<ContentReference>() { new ContentReference(2) };

            var result = sourceList.MemberOfAll(targetList);

            Assert.True(result);
        }

        [Theory]
        [InlineData(new[] { 1, 2 }, new[] { 3 })]
        [InlineData(new[] { 1, 2 }, new[] { 1, 2, 3 })]
        public void xMemberOfAll_SourceListDoesNotContainAllTargets_ThenReturnFalse(int[] source, int[] target)
        {
            var sourceList = source.Select(x => new ContentReference(x));
            var targetList = target.Select(x => new ContentReference(x));

            var result = sourceList.MemberOfAll(targetList);

            Assert.False(result);
        }

        [Theory]
        [InlineData(new[] { 1 }, new[] { 1 })]
        [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
        [InlineData(new[] { 1, 2 }, new[] { 1 })]
        public void xMemberOfAll_SourceListIsNull_ThenReturnTrue(int[] source, int[] target)
        {
            var sourceList = source.Select(x => new ContentReference(x));
            var targetList = target.Select(x => new ContentReference(x));

            var result = sourceList.MemberOfAll(targetList);

            Assert.True(result);
        }
    }
}
