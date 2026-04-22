using Xunit;
using System.IO;

namespace ST10330209PROG7311POE1.Tests.Tests
{
    public class FileValidationTests
    {
        private bool IsPdfFile(string fileName)
        {
            return Path.GetExtension(fileName).ToLower() == ".pdf";
        }

        [Fact]
        public void PdfFile_ValidExtension_ReturnsTrue()
        {
            bool result = IsPdfFile("contract.pdf");
            Assert.True(result);
        }

        [Theory]
        [InlineData("virus.exe")]
        [InlineData("document.txt")]
        [InlineData("image.png")]
        [InlineData("noextension")]
        public void NonPdfFile_InvalidExtension_ReturnsFalse(string fileName)
        {
            bool result = IsPdfFile(fileName);
            Assert.False(result);
        }
    }
}