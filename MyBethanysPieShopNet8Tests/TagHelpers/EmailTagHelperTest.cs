using BethanysPieShopNet8.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;

namespace BethanysPieShopNet8Tests.TagHelpers
{
    public class EmailTagHelperTest
    {
        [Fact]
        public void Generates_Email_Link()
        {
            // Arrange
            EmailTagHelper emailTagHelper = new EmailTagHelper()
            {
                Address = "test@bethanyspieshop.com",
                Content = "Email us"
            };

            var tagHelperContext = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                string.Empty);

            var content = new Mock<TagHelperContent>();

            var tagHelperOutput = new TagHelperOutput(
                "a",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(content.Object));

            // Act
            emailTagHelper.Process(tagHelperContext, tagHelperOutput);

            // Assert
            Assert.Equal("Email us", tagHelperOutput.Content.GetContent());
            Assert.Equal("a", tagHelperOutput.TagName);
            Assert.Equal("mailto:test@bethanyspieshop.com", tagHelperOutput.Attributes[1].Value);
        }
    }
}
