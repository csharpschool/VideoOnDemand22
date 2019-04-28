using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VOD.Admin.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("btn")]
    public class BtnTagHelper : AnchorTagHelper
    {
        #region Properties
        public string Icon { get; set; } = string.Empty;
        #endregion

        #region Constants
        const string btnPrimary = "btn-primary";
        const string btnDanger = "btn-danger";
        const string btnDefault = "btn-default";
        const string btnInfo = "btn-info";
        const string btnSucess = "btn-success";
        const string btnWarning = "btn-warning";
        // Google's Material Icons provider name
        const string iconProvider = "material-icons";
        #endregion

        #region This constructor is needed for AnchorTagHelper inheritance
        public BtnTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }
        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            // Changes <btn> tag to <a> tag when rendered
            output.TagName = "a";

            #region Bootstrap Button
            // Fetch page and class attribute to see if they exist
            var aspPageAttribute = context.AllAttributes.SingleOrDefault(p => p.Name.ToLower().Equals("asp-page"));
            var classAttribute = context.AllAttributes.SingleOrDefault(p => p.Name.ToLower().Equals("class"));
            var buttonStyle = btnDefault;
            if (aspPageAttribute != null)
            {
                var pageValue = aspPageAttribute.Value.ToString().ToLower();
                buttonStyle =
                    pageValue.Equals("create") ? btnPrimary :
                    pageValue.Equals("delete") ? btnDanger :
                    pageValue.Equals("edit") ? btnSucess :
                    pageValue.Equals("index") ? btnPrimary :
                    pageValue.Equals("details") ? btnInfo :
                    pageValue.Equals("/index") ? btnWarning :
                    pageValue.Equals("error") ? btnDanger :
                    btnDefault;
            }

            var bootstrapClasses = $"btn-sm {buttonStyle}";

            if (classAttribute != null)
            {
                var css = classAttribute.Value.ToString();
                if (!css.ToLower().Contains("btn-"))
                {
                    output.Attributes.Remove(classAttribute);
                    classAttribute = new TagHelperAttribute("class", $"{css} {bootstrapClasses}");
                    output.Attributes.Add(classAttribute);
                }
            }
            else
            {
                output.Attributes.Add("class", bootstrapClasses);
            }
            #endregion

            base.Process(context, output);
        }
    }
}
