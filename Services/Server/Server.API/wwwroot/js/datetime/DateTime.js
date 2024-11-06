function convertToJavaScriptDate(value) {
    if (value != null) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    } else
        return '';
}

//Convert:   19 Tuesday 10 / November / 2020 02: 29: 25 PM   Then
function ConvertDinhDangChuanNgay(value) {
    if (value != null) {
        var dt = new Date(value);
        var mth = dt.getMonth() + 1;
        var day = dt.getDate();
        if (mth < 10)
            mth = "0" + mth;
        if (day < 10)
            day = "0" + day;

        return dt.getFullYear() + '-' + mth + '-' + day;
    } else
        return null;
}

function ConvertDinhDangNgay(value) {
    if (value != null) {
        var dt = new Date(value);
        var mth = dt.getMonth() + 1;
        var day = dt.getDate();
        if (mth < 10)
            mth = "0" + mth;
        if (day < 10)
            day = "0" + day;

        return day + '/' + mth + '/' + dt.getFullYear();
    } else
        return null;
}


function ConvertDinhDangChuanNgayFormat(value) {
    if (value != null) {
        var dt = new Date(value);
        var mth = dt.getMonth() + 1;
        var day = dt.getDate();
        if (mth < 10)
            mth = "0" + mth;
        if (day < 10)
            day = "0" + day;

        return day + '/' + mth + '/' + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    } else
        return null;
}

function formatDateTimeFormatPicker(date, from = 0) {
    if (date != null) {
        var mth = date.getMonth() + 1;
        var day = date.getDate();
        if (mth < 10)
            mth = "0" + mth;
        if (day < 10)
            day = "0" + day;
        if (from == 0)
            return day + '/' + mth + '/' + date.getFullYear() + " " + "00:00:00";
        else
            return day + '/' + mth + '/' + date.getFullYear() + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
    } else
        return null;
}

function formatCompat(date) {
    var ms = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    return date.getDate() + ' ' + ms[date.getMonth()] + ' ' + date.getFullYear();
}

function formatDate(date) {
    if (date != null) {
        var mth = date.getMonth() + 1;
        var day = date.getDate();
        var hour = date.getHours();
        var minute = date.getMinutes();
        var second = date.getSeconds();
        return date.getFullYear() + '-' + mth + '-' + day;
    } else
        return null;
}

function formatDateTime(date) {
    if (date != null) {
        var mth = date.getMonth() + 1;
        var day = date.getDate();

        var hour = date.getHours();
        var minute = date.getMinutes();
        var second = date.getSeconds();

        mth = mth < 10 ? "0" + mth : mth;
        day = day < 10 ? "0" + day : day;
        hour = hour < 10 ? "0" + hour : hour;
        minute = minute < 10 ? "0" + minute : minute;
        second = second < 10 ? "0" + second : second;

        return date.getFullYear() + '-' + mth + '-' + day + "T" + hour + ":" + minute + ":" + second + "Z";
    } else
        return null;
}

function formatDateTimePickerFull(date) {
    if (date != null) {
        var mth = date.getMonth() + 1;
        var day = date.getDate();
        if (mth < 10)
            mth = "0" + mth;
        if (day < 10)
            day = "0" + day;
        return day + '/' + mth;
    } else
        return '';
}


function formatDateTimePicker(date) {

    if (date != null) {
        var mth = date.getMonth() + 1;
        var day = date.getDate();
        if (mth < 10)
            mth = "0" + mth;
        if (day < 10)
            day = "0" + day;
        return day + '/' + mth + '/' + date.getFullYear();
    } else
        return '';
}

function formatDateTimePickerFirstDayOfMonth(date) {

    if (date != null) {
        var mth = date.getMonth() + 1;
        //var day = date.getDate();
        if (mth < 10)
            mth = "0" + mth;
        //if (day < 10)
        //    day = "0" + day;
        return 01 + '/' + mth + '/' + date.getFullYear();
    } else
        return '';
}

function formatDateTimeFormatPickerDefault(date, from = 0) {
    if (date != null) {
        var mth = date.getMonth() + 1;
        var day = date.getDate();
        if (mth < 10)
            mth = "0" + mth;
        if (day < 10)
            day = "0" + day;
        if (from == 0)
            return day + '/' + mth + '/' + date.getFullYear() + " " + "00:00:00";
        else
            return day + '/' + mth + '/' + date.getFullYear() + " " + "23:59:59";
    } else
        return '';
}


