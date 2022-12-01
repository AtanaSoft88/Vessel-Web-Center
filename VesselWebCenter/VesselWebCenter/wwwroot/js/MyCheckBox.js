function ckChange(ckType) {
    var ckName = document.getElementsByName(ckType.name);
    var checked = document.getElementById(ckType.id);

    if (checked.checked) {
        for (var i = 0; i < ckName.length; i++) {

            if (!ckName[i].checked) {
                ckName[i].disabled = true;
            } else {
                ckName[i].disabled = false;
            }
        }
    }
    else {
        for (var i = 0; i < ckName.length; i++) {
            ckName[i].disabled = false;
        }
    }
}