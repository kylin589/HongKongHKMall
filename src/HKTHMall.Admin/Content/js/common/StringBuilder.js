// 用来连接字符串,提高字符串的拼接速度
function StringBuilder() {
    this.buffer = new Array();
}
StringBuilder.prototype.Append = function Append(string) {
    if ((string == null) || (typeof (string) == 'undefined'))
        return;
    if ((typeof (string) == 'string') && (string.length == 0))
        return;
    this.buffer.push(string);
};
StringBuilder.prototype.AppendLine = function AppendLine(string) {
    this.Append(string);
    this.buffer.push("\r\n");
};
StringBuilder.prototype.Clear = function Clear() {
    if (this.buffer.length > 0) {
        this.buffer.splice(0, this.buffer.length);
    }
};
StringBuilder.prototype.IsEmpty = function IsEmpty() {
    //    return (this.buffer.length == 0);
};
StringBuilder.prototype.ToString = function ToString() {
    return this.buffer.join("");
};
//处理日期字符串
function FormatDateType(date) {
    if (date) {
        return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
    }
    else {
        return "";
    }
}

//查找数据组中是否存在某值
Array.prototype.Contains = function (element) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == element) {
            return true;
        }
    }
    return false;

};

//使用:

//var arrayTemp = [1, 2, 3, 4, 5];

//arrayTemp.Contains(3); 返回:true

//arrayTemp.Contains(7); 返回:false
