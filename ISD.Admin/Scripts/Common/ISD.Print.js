//Hàm gán dữ liệu vào kendo template và hiển thị trên web
//params: 
//	1. tmpl: Mẫu in kendo template
//	2. obj: Dữ liệu hiển thị lên mẫu in
//	3. divHtml: html hiển thị trên web
function previewPrint(tmpl, obj, divHtml) {
	var content = kendo.template(tmpl)(obj);
	$(divHtml).html(content);
}

//Hàm in html
function openPrintHtml(html) {
	var dummyContent = "<html><body style='width:270px;height:auto'>" + html + "</body></html>";
	var printWindow = window.open('', '', 'height=0,width=0');
	printWindow.document.write(dummyContent);
	printWindow.print();
	printWindow.close();
	return dummyContent;
}

function getHtmlTemplate(tmpl, obj) {
    var content = kendo.template(tmpl)(obj);
    var dummyContent = "<html><body style='width:270px;height:auto'>" + content + "</body></html>";
    return dummyContent;
}

