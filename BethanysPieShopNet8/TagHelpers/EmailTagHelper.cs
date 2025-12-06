using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BethanysPieShopNet8.TagHelpers
{
    public class EmailTagHelper : TagHelper// Creating "<email" custom tag helper
    {
        // Properties for the email address and link content
        public string? Address { get; set; }
        public string? Content { get; set; }

        // Overriding the Process method to define the output of the custom tag helper
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("class", "btn btn-secondary");
            output.Attributes.SetAttribute("href", "mailto:" + Address);
            output.Content.SetContent(Content);
        }
    }
}
