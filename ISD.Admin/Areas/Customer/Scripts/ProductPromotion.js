/*Begin-ProductPromotion*/

/*Reload data*/
ReloadProductPromotionList = function () {
    var profileId = $("#frmEdit input[name='ProfileId']").val();
    var requestUrl = "/Customer/ProductPromotion/_List/" + profileId + "?isLoadContent=true";
    $("#contentProductPromotion table tbody").load(requestUrl);
}

function reloadPage() {
    location.reload();
}
/*End-ProductPromotion*/