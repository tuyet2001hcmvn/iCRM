// Usage:
//   var myStr = "le xuan tien"
//   myStr.uppercaseAllFirstLetter() => => "Le Xuan Tien"
String.prototype.uppercaseAllFirstLetter = function () {

    var str = this.split(' ');

    for (var i = 0, x = str.length; i < x; i++) {
        if (str[i]) {
            str[i] = str[i][0].toUpperCase() + str[i].substr(1);
        }
    }

    return str.join(' ');
};