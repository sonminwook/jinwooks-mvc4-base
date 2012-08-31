function top_subMenu_show(id) {

    var oParentObj = document.getElementById('td' + id);
    var iLeft = oParentObj.offsetParent.offsetLeft + oParentObj.offsetLeft;

    var strID = "top_subMenu" + id;
    var obj = document.getElementById(strID);

    if (obj != null && obj.style.display == 'none') {
        obj.style.display = '';
        obj.style.left = iLeft;
    }
}

function top_subMenu_hide(id) {
    var strID = "top_subMenu" + id;
    var obj = document.getElementById(strID);
    if (obj != null && obj.style.display != 'none')
        obj.style.display = 'none';
}

function title_Nomal(num, obj, menuSelected) {
    if (obj != null && !menuSelected) {
        obj.style.bgColor = "#74A1DD";
    }
}

function title_Over(num, obj) {
    if (obj != null)
        obj.style.bgColor = "#74A1DD";
}