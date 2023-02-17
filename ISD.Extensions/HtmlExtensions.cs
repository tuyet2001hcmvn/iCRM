using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using ISD.Constant;
using System.Collections.Generic;

namespace System.Web.Mvc.Html
{
    public static class HtmlExtensions
    {
        #region Get Current Url
        public static string GetCurrentUrl(string CurrentArea, string CurrentController, string Parameters = null)
        {
            string CurrentUrl = "";
            if (!string.IsNullOrEmpty(CurrentArea))
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    CurrentUrl = string.Format("{0}/{1}{2}", CurrentArea, CurrentController, Parameters);
                }
                else
                {
                    CurrentUrl = string.Format("{0}/{1}", CurrentArea, CurrentController);
                }
            }
            else
            {
                CurrentUrl = CurrentController;
            }
            return CurrentUrl;
        }
        #endregion Get Current Url

        #region Get Permission
        public static bool GetPermission(string PageUrl, string Function, string Parameters = null)
        {
            //Get PageUrl from user input
            string pageUrl = string.Format("/{0}", PageUrl);
            //Get PageUrl from Session["Menu"]
            PermissionViewModel permission = (PermissionViewModel)HttpContext.Current.Session["Menu"];
            var pageId = permission.PageModel.Where(p => p.PageUrl == pageUrl &&
                                                         (Parameters == null || p.Parameter.Contains(Parameters)))
                                            .Select(p => p.PageId)
                                            .FirstOrDefault();
            //Compare with PageId in PagePermission
            var pagePermission = permission.PagePermissionModel.Where(p => p.PageId == pageId && p.FunctionId == Function).FirstOrDefault();
            if (pagePermission != null)
            {
                return true;
            }
            return false;
        }
        #endregion Get Permission

        #region Create Button
        public static MvcHtmlString CreateButton(string areaName, string controlName, object htmlAttributes = null)
        {
            TagBuilder aTag = new TagBuilder("a");
            aTag.Attributes.Add("id", "btn-create");
            aTag.Attributes.Add("class", "btn bg-blue");

            string CurrentUrl = GetCurrentUrl(areaName, controlName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Create);
            if (!isHasPermission)
            {
                aTag.Attributes.Add("disabled", "disabled");
            }
            else
            {
                aTag.Attributes.Add("href", string.Format("/{0}/Create", CurrentUrl));
                aTag.Attributes.Add("target", "_blank");
                // aTag.Attributes.Add("onclick", "$(this).button('loading')");
            }
            aTag.InnerHtml += string.Format("<i class='fa fa-plus-square'></i> {0}", LanguageResource.Btn_Create);
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(aTag.ToString());
        }

        public static MvcHtmlString CreateButton(string areaName, string controlName, string paramsName, string paramsValue, object htmlAttributes = null)
        {
            TagBuilder aTag = new TagBuilder("a");
            aTag.Attributes.Add("id", "btn-create");
            aTag.Attributes.Add("class", "btn bg-blue");

            string CurrentUrl = GetCurrentUrl(areaName, controlName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Create);
            if (!isHasPermission)
            {
                aTag.Attributes.Add("disabled", "disabled");
                aTag.Attributes.Add("class", "hidden");
            }
            else
            {
                aTag.Attributes.Add("href", string.Format("/{0}/Create?{1}={2}", CurrentUrl, paramsName, paramsValue));
                aTag.Attributes.Add("target", "_blank");
                // aTag.Attributes.Add("onclick", "$(this).button('loading')");
            }
            aTag.InnerHtml += string.Format("<i class='fa fa-plus-square'></i> {0}", LanguageResource.Btn_Create);
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(aTag.ToString());
        }

        public static MvcHtmlString CreateButton(string areaName, string controlName, string parameters, object htmlAttributes = null)
        {
            TagBuilder aTag = new TagBuilder("a");
            aTag.Attributes.Add("id", "btn-create");
            aTag.Attributes.Add("class", "btn bg-blue");

            string CurrentUrl = GetCurrentUrl(areaName, controlName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Create);
            if (!isHasPermission)
            {
                aTag.Attributes.Add("disabled", "disabled");
            }
            else
            {
                aTag.Attributes.Add("href", string.Format("/{0}/Create{1}", CurrentUrl, parameters));
                aTag.Attributes.Add("target", "_blank");
                // aTag.Attributes.Add("onclick", "$(this).button('loading')");
            }
            aTag.InnerHtml += string.Format("<i class='fa fa-plus-square'></i> {0}", LanguageResource.Btn_Create);
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(aTag.ToString());
        }
        #endregion Create Button

        #region Export Button
        public static MvcHtmlString ExportButton(string areaName, string controlName, object htmlAttributes = null)
        {
            //button
            TagBuilder button = new TagBuilder("button");
            //button.Attributes.Add("id", "btn-export");
            button.Attributes.Add("class", "btn btn-success dropdown-toggle");
            button.InnerHtml += string.Format("<i class='fa fa-download'></i> {0}", LanguageResource.Btn_Export);
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                button.MergeAttributes(attributes, true);
            }
            button.ToString(TagRenderMode.SelfClosing);

            //button dropdown
            TagBuilder buttonDropdown = new TagBuilder("button");
            buttonDropdown.Attributes.Add("class", "btn btn-success dropdown-toggle");
            buttonDropdown.Attributes.Add("data-toggle", "dropdown");
            buttonDropdown.InnerHtml += "<span class='caret'></span><span class='sr-only'>&nbsp;</span>";
            buttonDropdown.ToString(TagRenderMode.SelfClosing);

            string CurrentUrl = GetCurrentUrl(areaName, controlName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Export);
            if (!isHasPermission)
            {
                button.Attributes.Add("disabled", "disabled");
                return new MvcHtmlString(button.ToString());
            }
            //ul li
            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.Attributes.Add("class", "dropdown-menu");
            ulTag.Attributes.Add("role", "menu");

            //li: Export Create
            TagBuilder liCreate = new TagBuilder("li");
            TagBuilder aTagExportCreate = new TagBuilder("a");
            aTagExportCreate.Attributes.Add("class", "btn-export");
            aTagExportCreate.Attributes.Add("href", string.Format("/{0}/ExportCreate", CurrentUrl));
            aTagExportCreate.InnerHtml += string.Format("<i class='fa fa-file-excel-o'></i> {0}", LanguageResource.Template_Add);
            liCreate.InnerHtml = aTagExportCreate.ToString();
            liCreate.ToString(TagRenderMode.SelfClosing);

            //li: Export Edit
            TagBuilder liEdit = new TagBuilder("li");
            TagBuilder aTagExportEdit = new TagBuilder("a");
            aTagExportEdit.Attributes.Add("class", "btn-export");
            aTagExportEdit.Attributes.Add("href", string.Format("/{0}/ExportEdit", CurrentUrl));
            aTagExportEdit.InnerHtml += string.Format("<i class='fa fa-file-excel-o'></i> {0}", LanguageResource.Template_Update);
            liEdit.InnerHtml = aTagExportEdit.ToString();
            liEdit.ToString(TagRenderMode.SelfClosing);

            ulTag.InnerHtml = liCreate.ToString() + liEdit.ToString();

            //div
            var div = new TagBuilder("div");
            div.AddCssClass("btn-group");
            div.InnerHtml = button.ToString() + buttonDropdown.ToString() + ulTag.ToString();
            div.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString ExportReportButton(string areaName, string controlName, object htmlAttributes = null)
        {
            //button
            TagBuilder aTag = new TagBuilder("a");
            //button.Attributes.Add("id", "btn-export");
            aTag.Attributes.Add("class", "btn btn-success");
            aTag.InnerHtml += string.Format("<i class='fa fa-download'></i> {0}", LanguageResource.Btn_Export);
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.ToString(TagRenderMode.SelfClosing);


            string CurrentUrl = GetCurrentUrl(areaName, controlName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Export);
            if (!isHasPermission)
            {
                aTag.Attributes.Add("disabled", "disabled");
            }
            else
            {
                aTag.Attributes.Add("onclick", "exportExcel($(this))");
            }

            return new MvcHtmlString(aTag.ToString());

        }
        #endregion Export Button

        #region Import Button
        public static MvcHtmlString ImportButton(string areaName, string controlName, object htmlAttributes = null)
        {
            TagBuilder button = new TagBuilder("button");
            button.Attributes.Add("id", "btn-import");
            button.Attributes.Add("class", "btn bg-olive");
            button.Attributes.Add("data-toggle", "modal");
            button.Attributes.Add("data-target", "#importexcel-window");
            button.InnerHtml += string.Format("<i class='fa fa-upload'></i> {0}", LanguageResource.Btn_Import);
            string CurrentUrl = GetCurrentUrl(areaName, controlName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Import);
            if (!isHasPermission)
            {
                button.Attributes.Add("disabled", "disabled");
                return new MvcHtmlString(button.ToString());
            }

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                button.MergeAttributes(attributes, true);
            }
            button.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(button.ToString());
        }
        #endregion Import Button

        #region Search Button
        public static MvcHtmlString SearchButton(object htmlAttributes = null)
        {
            TagBuilder aTag = new TagBuilder("a");
            aTag.Attributes.Add("id", "btn-search");
            aTag.Attributes.Add("class", "btn btn-primary btn-search");
            aTag.InnerHtml += string.Format("<i class='fa fa-search'></i> {0}", LanguageResource.Btn_Search);
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(aTag.ToString());
        }
        #endregion Search Button

        #region Edit Button in List
        public static MvcHtmlString EditButton(string areaName, string controllerName, Guid id, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Edit);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-default btn-edit");

                aTag.Attributes.Add("href", string.Format("/{0}/Edit/{1}", CurrentUrl, id));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml += string.Format("<i class='fa fa-pencil'></i> {0}", LanguageResource.Btn_Edit);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }

        //FunctionModel: FunctionId type string
        public static MvcHtmlString EditButton(string areaName, string controllerName, string id, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Edit);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-default btn-edit");
                aTag.Attributes.Add("href", string.Format("/{0}/Edit/{1}", CurrentUrl, id));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml += string.Format("<i class='fa fa-pencil'></i> {0}", LanguageResource.Btn_Edit);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }

        //CategoryModel (ACTest)
        public static MvcHtmlString EditButton(string areaName, string controllerName, string id, string compid, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Edit);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-default btn-edit");
                aTag.Attributes.Add("href", string.Format("/{0}/Edit/{1}?compid={2}", CurrentUrl, id, compid));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml += string.Format("<i class='fa fa-pencil'></i> {0}", LanguageResource.Btn_Edit);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }

        //ReceiptVoucher
        public static MvcHtmlString EditButton(string areaName, string controllerName, Guid id, int type, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Edit);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-default btn-edit");
                aTag.Attributes.Add("href", string.Format("/{0}/Edit/{1}?type={2}", CurrentUrl, id, type));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml += string.Format("<i class='fa fa-pencil'></i> {0}", LanguageResource.Btn_Edit);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }
        #endregion Edit Button in List

        #region View Button in List
        public static MvcHtmlString ViewButton(string areaName, string controllerName, Guid id, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.View);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-info btn-edit");

                aTag.Attributes.Add("href", string.Format("/{0}/View/{1}", CurrentUrl, id));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml += string.Format("<i class='fa fa-eye'></i> {0}", LanguageResource.Btn_View);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }

        //FunctionModel: FunctionId type string
        public static MvcHtmlString ViewButton(string areaName, string controllerName, string id, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.View);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-info btn-edit");
                aTag.Attributes.Add("href", string.Format("/{0}/View/{1}", CurrentUrl, id));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml += string.Format("<i class='fa fa-eye'></i> {0}", LanguageResource.Btn_View);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }

        //FunctionModel: FunctionId type int
        public static MvcHtmlString ViewButton(string areaName, string controllerName, int id, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.View);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-info btn-edit");
                aTag.Attributes.Add("href", string.Format("/{0}/View/{1}", CurrentUrl, id));
                aTag.Attributes.Add("onclick", "$(this).button('loading')");

                aTag.InnerHtml += string.Format("<i class='fa fa-eye'></i> {0}", LanguageResource.Btn_View);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }
        #endregion View Button in List

        #region Upload File 3D Button in List
        public static MvcHtmlString UploadFile3DButton(string areaName, string controllerName, string id, string compid, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Upload);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-default btn-upload");

                aTag.Attributes.Add("href", string.Format("/{0}/Upload/{1}?compid={2}", CurrentUrl, id, compid));

                aTag.InnerHtml += string.Format("<i class='fa fa-upload'></i> {0}", LanguageResource.UploadFile3D);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }
        #endregion

        #region Upload Multiple Images
        public static MvcHtmlString UploadMultipleImages(string areaName, string controllerName, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.UploadMultipleImages);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("class", "btn btn-info btn-upload-img");

                aTag.Attributes.Add("href", string.Format("/{0}/UploadMultipleImages", CurrentUrl));

                aTag.InnerHtml += string.Format("<i class='fa fa-upload'></i> {0}", LanguageResource.UploadMultipleImages);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }
        #endregion

        #region Back Button
        public static MvcHtmlString BackButton(string areaName, string controllerName, string parameters = null, object htmlAttributes = null)
        {
            //a Tag
            TagBuilder aTag = new TagBuilder("a");
            string CurrentUrl = string.Empty;
            if (!string.IsNullOrEmpty(parameters))
            {
                CurrentUrl = GetCurrentUrl(areaName, controllerName, parameters);
            }
            else
            {
                CurrentUrl = GetCurrentUrl(areaName, controllerName);
            }
            aTag.Attributes.Add("href", string.Format("/{0}", CurrentUrl));
            aTag.InnerHtml += LanguageResource.Btn_Back;

            //small Tag
            TagBuilder smallTag = new TagBuilder("small");
            smallTag.InnerHtml += string.Format("<i class='fa fa-arrow-circle-left'></i>");
            smallTag.InnerHtml += aTag.ToString();
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                smallTag.MergeAttributes(attributes, true);
            }
            smallTag.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(smallTag.ToString());
        }
        #endregion Back Button

        #region Actived Icon
        /// <summary>
        /// ActivedIcon
        /// </summary>
        /// <param name="actived">Giá trị hiển thị: true, false</param>
        /// <param name="isUnShowFalseIcon">True: không hiển thị fa-close icon</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ActivedIcon(bool? actived, bool? isUnShowFalseIcon = null, object htmlAttributes = null)
        {
            TagBuilder iIcon = new TagBuilder("i");
            if (actived == true)
            {
                iIcon.Attributes.Add("class", "fa fa-check true-icon");
            }
            else if (isUnShowFalseIcon != true)
            {
                iIcon.Attributes.Add("class", "fa fa-close false-icon");
            }
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                iIcon.MergeAttributes(attributes, true);
            }
            iIcon.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(iIcon.ToString());
        }

        //Radio Button Yes/No
        public static MvcHtmlString ActivedRadioButtonIs<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string yes = "", string no = "")
        {
            var result = new StringBuilder();
            //tag div
            result.Append("<div class='div-radio'>");
            //2 radio button
            result.AppendFormat("<label class=\"label-radio\">{0} {1}</label><label class=\"label-radio\">{2} {3}</label>",
                helper.RadioButtonFor(expression, true, new { @checked = "checked", @id = "" }),
                yes == string.Empty ? LanguageResource.Yes : yes,
                helper.RadioButtonFor(expression, false),
                no == string.Empty ? LanguageResource.No : no);
            //end tag div
            result.Append("</div>");

            return MvcHtmlString.Create(result.ToString());
        }
        #endregion Actived Icon

        #region Save Button
        public static MvcHtmlString SaveButton(string id, string btn_name, object htmlAttributes = null)
        {
            TagBuilder aTag = new TagBuilder("a");
            aTag.Attributes.Add("id", id);
            aTag.Attributes.Add("class", "btn bg-blue");
            aTag.Attributes.Add("onclick", "$(this).button('loading')");
            aTag.InnerHtml += string.Format("<i class='fa fa-floppy-o'></i> {0}", btn_name);
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(aTag.ToString());
        }

        public static MvcHtmlString SaveSubmitButton(string id, string btn_name, object htmlAttributes = null)
        {
            TagBuilder aTag = new TagBuilder("a");
            aTag.Attributes.Add("id", id);
            aTag.Attributes.Add("class", "btn bg-blue");
            //button.Attributes.Add("type", "submit");
            //aTag.Attributes.Add("style", "margin-right: 5px;");
            aTag.InnerHtml += string.Format("<i class='fa fa-floppy-o'></i> {0}", btn_name);
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                aTag.MergeAttributes(attributes, true);
            }
            aTag.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(aTag.ToString());
        }
        #endregion Save Button

        #region Required Textbox
        public static MvcHtmlString RequiredTextboxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            return RequiredTextboxBuilder(helper, expression, null, isUpperText: false, htmlAttributes: htmlAttributes);
            //return MvcHtmlString.Create(result.ToString());
        }

        //Format text: datetime, decimal,...
        public static MvcHtmlString RequiredTextboxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string format, object htmlAttributes = null)
        {
            return RequiredTextboxBuilder(helper, expression, format, isUpperText: false, htmlAttributes: htmlAttributes);
        }

        // Uppercase
        // @Html.RequiredTextboxFor(p => p.RolesCode, true, null)
        public static MvcHtmlString RequiredTextboxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool? isUpperText, object htmlAttributes)
        {
            return RequiredTextboxBuilder(helper, expression, format: null, isUpperText: true, htmlAttributes: htmlAttributes);
        }

        //Always set required for textbox
        public static MvcHtmlString RequiredExTextboxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string Required, bool? isRequired, object htmlAttributes = null)
        {
            return RequiredTextboxBuilder(helper, expression, format: null, isUpperText: false, Required: Required, isRequired: isRequired, htmlAttributes: htmlAttributes);
        }

        // AngularJS
        public static MvcHtmlString ARequiredTextboxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            return RequiredTextboxBuilder(helper, expression, format: null, isUpperText: false, isAngular: true, htmlAttributes: htmlAttributes);
        }

        private static MvcHtmlString RequiredTextboxBuilder<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string format, bool? isUpperText, bool? isAngular = false, string Required = null, bool? isRequired = false, object htmlAttributes = null)
        {
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttributes == null || (htmlAttributes != null && htmlAttributes.GetType().GetProperty("class") == null))
            {
                attributes.Add("class", "form-control text-box single-line");
            }

            if (isUpperText == true)
            {
                attributes.Add("onkeyup", "$(this).val($(this).val().toUpperCase())");
            }

            if (isAngular == true)
            {
                attributes.Add("ng-model", GetName(expression));
            }

            if (!string.IsNullOrEmpty(format))
            {
                result.AppendFormat("<div class=\"input-group input-group-required\">{0}",
               helper.TextBoxFor(expression, format, attributes));
            }
            else
            {
                result.AppendFormat("<div class=\"input-group input-group-required\">{0}",
               helper.TextBoxFor(expression, attributes));
            }

            if (metadata.IsRequired || (Required == ConstCommon.Required && isRequired == true))
            {
                //result.Append("<div class=\"input-group-btn\"><span class=\"required\">*</span></div>");
                result.Append("<div class='required-icon'><div class='text'>*</div></div>");
            }
            result.AppendFormat("{0}</div>",
                helper.ValidationMessageFor(expression, null, new { @class = "validation-text" }));

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString DateTimeTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool? isNullValueForTime = false)
        {
            var result = new StringBuilder();
            var name = (expression.Body as MemberExpression).Member.Name;
            #region get Value
            var dateValue = string.Empty;
            var timeValue = string.Empty;
            var datetimeValue = string.Empty;
            var value = ModelMetadata.FromLambdaExpression(
               expression, helper.ViewData
           ).Model;
            if (value != null)
            {
                dateValue = ((DateTime?)value)?.ToString("yyyy-MM-dd");
                if (isNullValueForTime == true)
                {
                    //timeValue = ((DateTime?)value)?.ToString("HH:mm");
                    datetimeValue = string.Format("{0}", dateValue);
                }
                else
                {
                    timeValue = ((DateTime?)value)?.ToString("HH:mm");
                    datetimeValue = string.Format("{0}T{1}", dateValue, timeValue);
                }
            }
            #endregion
            //1. Date Picker
            result.Append(string.Format("<input id='dateField{0}' type='date' class='form-control select-date-time' data-id='{0}' value='{1}' />", name, dateValue));
            //2. Time Picker
            result.Append(string.Format("<input id='timeField{0}' type='time' class='form-control select-date-time' data-id='{0}' value='{1}' />", name, timeValue));
            //3. Hidden value
            result.AppendFormat("{0}", helper.Hidden(name, datetimeValue));

            //validation
            //result.AppendFormat("{0}",helper.ValidationMessageFor(expression, null, new { @class = "validation-text" }));

            return MvcHtmlString.Create(result.ToString());
        }


        private static object GetName<TModel, TValue>(Expression<Func<TModel, TValue>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            var bodyText = body.ToString();

            return bodyText.Substring(bodyText.IndexOf(".") + 1);
        }
        #endregion

        #region TooltipLabelFor
        public static MvcHtmlString TooltipLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string hintName = null)
        {
            var result = new StringBuilder();
            //Begin Tag1
            result.Append("<div class=\"label-wrapper\">");
            //Add label text
            result.Append(helper.LabelFor(expression, new { @class = "control-label" }));
            //Get resource name to set up the tooltip
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            //Get hint Rerouce => add the tooltip
            if (string.IsNullOrEmpty(hintName))
            {
                hintName = metadata.PropertyName;
            }
            //var hintResourceName = string.Format("{0}_Hint", metadata.PropertyName) ?? "";
            var hintResourceName = string.Format("{0}_Hint", hintName) ?? "";
            var hintResourceValue = LanguageResource.ResourceManager.GetString(hintResourceName);
            if (!string.IsNullOrEmpty(hintResourceValue))
            {
                result.AppendFormat("<span class=\"ico-help\" title=\"{0}\"><i class=\"fa fa-question-circle\"></i></span>", hintResourceValue);
            }
            if (!string.IsNullOrEmpty(metadata.Description) && string.IsNullOrEmpty(hintResourceValue))
            {
                result.AppendFormat("<span class=\"ico-help\" title=\"{0}\"><i class=\"fa fa-question-circle\"></i></span>", metadata.Description);
            }
            //End Tag1
            result.Append("</div>");
            return MvcHtmlString.Create(result.ToString());
        }
        public static MvcHtmlString LabelByPropertyNameFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string PropertyName, string hintName = null)
        {
            var result = new StringBuilder();
            //Begin Tag1
            result.Append("<div class=\"label-wrapper\">");
            //Add label text
            if (string.IsNullOrEmpty(PropertyName))
            {
                PropertyName = null;
            }
            result.Append(helper.LabelFor(expression, PropertyName, new { @class = "control-label" }));
            //Get resource name to set up the tooltip
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            //Get hint Rerouce => add the tooltip
            if (string.IsNullOrEmpty(hintName))
            {
                hintName = metadata.PropertyName;
            }
            var hintResourceName = string.Format("{0}_Hint", hintName) ?? "";
            var hintResourceValue = LanguageResource.ResourceManager.GetString(hintResourceName);
            if (!string.IsNullOrEmpty(hintResourceValue))
            {
                result.AppendFormat("<span class=\"ico-help\" title=\"{0}\"><i class=\"fa fa-question-circle\"></i></span>", hintResourceValue);
            }
            if (!string.IsNullOrEmpty(metadata.Description) && string.IsNullOrEmpty(hintResourceValue))
            {
                result.AppendFormat("<span class=\"ico-help\" title=\"{0}\"><i class=\"fa fa-question-circle\"></i></span>", metadata.Description);
            }
            //End Tag1
            result.Append("</div>");
            return MvcHtmlString.Create(result.ToString());
        }
        #endregion TooltipLabelFor

        #region Radio Button
        public static MvcHtmlString ActivedRadioButton<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool? isDisabled = null)
        {
            var result = new StringBuilder();
            //tag div
            result.Append("<div class='div-radio'>");
            //2 radio button
            if (isDisabled == true)
            {
                result.AppendFormat("<label class=\"label-radio\">{0} {1}</label><label class=\"label-radio\">{2} {3}</label>",
                helper.RadioButtonFor(expression, true, new { @checked = "checked", @id = "", @disabled = "disabled"}),
                LanguageResource.Actived_True,
                helper.RadioButtonFor(expression, false, new { @id = "", @disabled = "disabled" }),
                LanguageResource.Actived_False);
            }
            else
            {
               result.AppendFormat("<label class=\"label-radio\">{0} {1}</label><label class=\"label-radio\">{2} {3}</label>",
               helper.RadioButtonFor(expression, true, new { @checked = "checked", @id = "" }),
               LanguageResource.Actived_True,
               helper.RadioButtonFor(expression, false, new { @id = "" }),
               LanguageResource.Actived_False);
            }
            //end tag div
            result.Append("</div>");

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString RadioButtonTextFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string TrueText, string FalseText)
        {
            var result = new StringBuilder();
            //tag div
            result.Append("<div class='div-radio'>");
            //2 radio button
            result.AppendFormat("<label class=\"label-radio\">{0} {1}</label><label class=\"label-radio\">{2} {3}</label>",
                helper.RadioButtonFor(expression, true, new { @checked = "checked", @id = "" }),
                TrueText,
                helper.RadioButtonFor(expression, false),
                FalseText);
            //end tag div
            result.Append("</div>");

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString RadioButtonTextFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string firstText, string secondText, object firstValue = null, object secondValue = null)
        {
            var result = new StringBuilder();
            //tag div
            result.Append("<div class='div-radio'>");
            if (firstValue != null && secondValue != null)
            {
                //2 radio button
                result.AppendFormat("<label class=\"label-radio\">{0} {1}</label><label class=\"label-radio\">{2} {3}</label>",
                    helper.RadioButtonFor(expression, firstValue, new { @checked = "checked", @id = "" }),
                    firstText,
                    helper.RadioButtonFor(expression, secondValue),
                    secondText);
            }
            else
            {
                //2 radio button
                result.AppendFormat("<label class=\"label-radio\">{0} {1}</label><label class=\"label-radio\">{2} {3}</label>",
                    helper.RadioButtonFor(expression, true, new { @checked = "checked", @id = "" }),
                    firstText,
                    helper.RadioButtonFor(expression, false),
                    secondText);
            }
            //end tag div
            result.Append("</div>");

            return MvcHtmlString.Create(result.ToString());
        }
        #endregion

        #region Delete Button
        public static MvcHtmlString DeleteButton(string areaName, string controllerName, string itemName, Guid id, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Delete);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("onclick", "$(this).button('loading')");
                aTag.Attributes.Add("class", "btn btn-danger btn-delete");

                aTag.Attributes.Add("data-id", string.Format("{0}", id));
                aTag.Attributes.Add("data-current-url", string.Format("{0}", CurrentUrl));
                //aTag.Attributes.Add("data-item-name", string.Format("{0}", itemName.ToLower()));
                aTag.Attributes.Add("data-item-name", string.Format("{0}", itemName));
                aTag.InnerHtml += string.Format("<i class=\"fa fa-trash-o\"></i> {0}", LanguageResource.Btn_Delete);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }
        //FunctionModel: FunctionId type string
        public static MvcHtmlString DeleteButton(string areaName, string controllerName, string itemName, string id, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Delete);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("onclick", "$(this).button('loading')");
                aTag.Attributes.Add("class", "btn btn-danger btn-delete");
                aTag.Attributes.Add("data-id", string.Format("{0}", id));
                aTag.Attributes.Add("data-current-url", string.Format("{0}", CurrentUrl));
                aTag.Attributes.Add("data-item-name", string.Format("{0}", itemName.ToLower()));
                aTag.InnerHtml += string.Format("<i class=\"fa fa-trash-o\"></i> {0}", LanguageResource.Btn_Delete);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }
        //NC_Material3DFileModel
        public static MvcHtmlString DeleteButton(string areaName, string controllerName, string itemName, int id, object htmlAttributes = null)
        {
            string CurrentUrl = GetCurrentUrl(areaName, controllerName);
            bool isHasPermission = GetPermission(CurrentUrl, ConstFunction.Delete);
            if (isHasPermission)
            {
                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("onclick", "$(this).button('loading')");
                aTag.Attributes.Add("class", "btn btn-danger btn-delete");

                aTag.Attributes.Add("data-id", string.Format("{0}", id));
                aTag.Attributes.Add("data-current-url", string.Format("{0}", CurrentUrl));
                aTag.Attributes.Add("data-item-name", string.Format("{0}", itemName));
                aTag.InnerHtml += string.Format("<i class=\"fa fa-trash-o\"></i> {0}", LanguageResource.Btn_Delete);
                if (htmlAttributes != null)
                {
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    aTag.MergeAttributes(attributes, true);
                }
                aTag.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(aTag.ToString());
            }
            return null;
        }
        #endregion Delete Button

        #region Required Dropdownlist
        /// <summary>
        /// Dropdownlist required
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList">The name of ViewBag in Controller MUST be DIFFERENT from the name field</param>
        /// IMPORTANT: If the name of ViewBag is same as the name filed, it's no need to add (IEnumerable<SelectListItem>)ViewBag in selectList
        /// Example: ProvinceId is the field name, the ViewBag name is ProvinceIdList
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString RequiredDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes = null)
        {
            //return RequiredDropDownListBuilder(helper, expression, selectList, optionLabel, htmlAttributes: htmlAttributes);
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttributes == null || (htmlAttributes != null && htmlAttributes.GetType().GetProperty("class") == null))
            {
                attributes.Add("class", "form-control with-search");
                attributes.Add("data-val", "true");
                attributes.Add("data-val-required", string.Format(LanguageResource.Required, metadata.DisplayName));
            }

            result.AppendFormat("<div class=\"input-group input-group-required input-group-select-required\">{0}",
               helper.DropDownList(((MemberExpression)expression.Body).Member.Name, selectList, optionLabel, attributes));

            if (metadata.IsRequired)
            {
                //result.Append("<div class=\"input-group-btn\"><span class=\"required\">*</span></div>");
                result.Append("<div class=\"required-icon\"><span class=\"text\">*</span></div>");
            }
            result.AppendFormat("{0}</div>",
                helper.ValidationMessageFor(expression, null, new { @class = "validation-text" }));

            return MvcHtmlString.Create(result.ToString());
        }


        /// <summary>
        /// Always display required icon on select
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString RequiredIfDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes = null)
        {
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttributes == null || (htmlAttributes != null && htmlAttributes.GetType().GetProperty("class") == null))
            {
                attributes.Add("class", "form-control with-search");
                attributes.Add("data-val", "true");
                attributes.Add("data-val-required", string.Format(LanguageResource.Required, metadata.DisplayName));
            }

            result.AppendFormat("<div class=\"input-group input-group-required input-group-select-required\">{0}",
               helper.DropDownList(((MemberExpression)expression.Body).Member.Name, selectList, optionLabel, attributes));

            result.Append("<div class=\"required-icon\"><span class=\"text\">*</span></div>");
            result.AppendFormat("{0}</div>",
                helper.ValidationMessageFor(expression, null, new { @class = "validation-text" }));

            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// Dropdownlist required: Name base on expression name, not base on member name in model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString RequiredExDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes = null)
        {
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttributes == null || (htmlAttributes != null && htmlAttributes.GetType().GetProperty("class") == null))
            {
                attributes.Add("class", "form-control with-search");
                attributes.Add("data-val", "true");
                attributes.Add("data-val-required", string.Format(LanguageResource.Required, metadata.DisplayName));
            }

            result.AppendFormat("<div class=\"input-group input-group-required input-group-select-required\">{0}",
               helper.DropDownListFor(expression, selectList, optionLabel, attributes));

            if (metadata.IsRequired)
            {
                //result.Append("<div class=\"input-group-btn\"><span class=\"required\">*</span></div>");
                result.Append("<div class=\"required-icon\"><span class=\"text\">*</span></div>");
            }
            result.AppendFormat("{0}</div>",
                helper.ValidationMessageFor(expression, null, new { @class = "validation-text" }));

            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// Dropdownlist required: Name base on expression name, not base on member name in model
        /// Always display required
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString RequiredExDropDownList_RequiredFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes = null)
        {
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttributes == null || (htmlAttributes != null && htmlAttributes.GetType().GetProperty("class") == null))
            {
                attributes.Add("class", "form-control with-search");
                attributes.Add("data-val", "true");
                attributes.Add("data-val-required", string.Format(LanguageResource.Required, metadata.DisplayName));
            }

            result.AppendFormat("<div class=\"input-group input-group-required input-group-select-required\">{0}",
               helper.DropDownListFor(expression, selectList, optionLabel, attributes));

            result.Append("<div class=\"required-icon\"><span class=\"text\">*</span></div>");
            result.AppendFormat("{0}</div>",
                helper.ValidationMessageFor(expression, null, new { @class = "validation-text" }));

            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// Dropdownlist required
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <param name="selectList">The name of ViewBag in Controller MUST be DIFFERENT from the name field</param>
        /// IMPORTANT: If the name of ViewBag is same as the name filed, it's no need to add (IEnumerable<SelectListItem>)ViewBag in selectList
        /// Example: ProvinceId is the field name, the ViewBag name is ProvinceIdList</param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString RequiredDropDownList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes = null)
        {
            //return RequiredDropDownListBuilder(helper, expression, selectList, optionLabel, htmlAttributes: htmlAttributes);
            var result = new StringBuilder();
            //var metadata = ModelMetadata.FromLambdaExpression(name, helper.ViewData);
            var metadata = ModelMetadata.FromStringExpression(name, helper.ViewData);

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttributes == null || (htmlAttributes != null && htmlAttributes.GetType().GetProperty("class") == null))
            {
                attributes.Add("class", "form-control with-search");
                attributes.Add("data-val", "true");
                attributes.Add("data-val-required", string.Format(LanguageResource.Required, metadata.DisplayName));
            }

            result.AppendFormat("<div class=\"input-group input-group-required input-group-select-required\">{0}",
               helper.DropDownList(name, selectList, optionLabel, attributes));
            result.Append("<div class=\"required-icon\"><span class=\"text\">*</span></div>");
            result.AppendFormat("{0}</div>",
                helper.ValidationMessage(name, null, new { @class = "validation-text" }));

            return MvcHtmlString.Create(result.ToString());
        }
        #endregion
    }
}