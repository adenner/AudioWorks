using System.Diagnostics.CodeAnalysis;
using System.IO;
using AudioWorks.Common.Tests.DataSources;
using AudioWorks.TestUtilities;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace AudioWorks.Common.Tests
{
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public sealed class CoverArtTests
    {
        public CoverArtTests([NotNull] ITestOutputHelper outputHelper)
        {
            LoggingManager.LoggerFactory.AddProvider(new XUnitLoggerProvider(outputHelper));
        }

        [Theory(DisplayName = "CoverArt has the expected Width property value")]
        [MemberData(nameof(ValidImageFileDataSource.FileNamesAndWidth),
            MemberType = typeof(ValidImageFileDataSource))]
        public void HasExpectedWidth([NotNull] string fileName, int expectedWidth)
        {
            Assert.Equal(expectedWidth, CoverArtFactory.GetOrCreate(
                    Path.Combine(
                        new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent?.FullName,
                        "TestFiles",
                        "Valid",
                        fileName))
                .Width);
        }

        [Theory(DisplayName = "CoverArt has the expected Height property value")]
        [MemberData(nameof(ValidImageFileDataSource.FileNamesAndHeight),
            MemberType = typeof(ValidImageFileDataSource))]
        public void HasExpectedHeight([NotNull] string fileName, int expectedHeight)
        {
            Assert.Equal(expectedHeight, CoverArtFactory.GetOrCreate(
                    Path.Combine(
                        new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent?.FullName,
                        "TestFiles",
                        "Valid",
                        fileName))
                .Height);
        }

        [Theory(DisplayName = "CoverArt has the expected ColorDepth property value")]
        [MemberData(nameof(ValidImageFileDataSource.FileNamesAndColorDepth),
            MemberType = typeof(ValidImageFileDataSource))]
        public void HasExpectedColorDepth([NotNull] string fileName, int expectedColorDepth)
        {
            Assert.Equal(expectedColorDepth, CoverArtFactory.GetOrCreate(
                    Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent?.FullName,
                "TestFiles",
                "Valid",
                fileName))
                .ColorDepth);
        }

        [Theory(DisplayName = "CoverArt has the expected Lossless property value")]
        [MemberData(nameof(ValidImageFileDataSource.FileNamesAndLossless),
            MemberType = typeof(ValidImageFileDataSource))]
        public void HasExpectedLossless([NotNull] string fileName, bool expectedLossless)
        {
            Assert.Equal(expectedLossless, CoverArtFactory.GetOrCreate(
                    Path.Combine(
                        new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent?.FullName,
                        "TestFiles",
                        "Valid",
                        fileName))
                .Lossless);
        }

        [Theory(DisplayName = "CoverArt has the expected MimeType property value")]
        [MemberData(nameof(ValidImageFileDataSource.FileNamesAndMimeType),
            MemberType = typeof(ValidImageFileDataSource))]
        public void HasExpectedMimeType([NotNull] string fileName, [NotNull] string mimeType)
        {
            Assert.Equal(mimeType, CoverArtFactory.GetOrCreate(
                    Path.Combine(
                        new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent?.FullName,
                        "TestFiles",
                        "Valid",
                        fileName))
                .MimeType);
        }

        [Theory(DisplayName = "GetData returns the expected value")]
        [MemberData(nameof(ValidImageFileDataSource.FileNamesAndDataHash),
            MemberType = typeof(ValidImageFileDataSource))]
        public void GetDataReturnsExpectedValue([NotNull] string fileName, [NotNull] string expectedHash)
        {
            Assert.Equal(expectedHash, HashUtility.CalculateHash(CoverArtFactory.GetOrCreate(
                    Path.Combine(
                        new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent?.FullName,
                        "TestFiles",
                        "Valid",
                        fileName))
                .Data.ToArray()));
        }
    }
}
