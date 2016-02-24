//add by liujc
function GetLanguageName(obj) {
    var languageName = "Hongkong";
    switch (obj) {
        case "1":
            languageName = "Chinese";
            break;
        case "2":
            languageName = "English";
            break;
        case "3":
            languageName = "Thai";
            break;
        case "4":
            languageName = "Hongkong";
            break;
        default:
            break;

    }
    return languageName;
}