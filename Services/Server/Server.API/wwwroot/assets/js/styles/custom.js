/* ------------------------------------------------------------------------------
 *
 *  # Custom JS code
 *
 *  Place here all your custom js. Make sure it's loaded after app.js
 *
/* ------------------------------------------------------------------------------
 * // 1. Menu Save Sate
 * ---------------------------------------------------------------------------- */
// ------------------------------
try {
    const toggle = document.getElementById('btn-sidebarControl');
    const navbar = document.getElementById('menuSidebar');

    if (toggle && navbar) {
        toggle.addEventListener('click', () => {
            navbar.classList.toggle('sidebar-main-resized')

            if (navbar.classList.contains('sidebar-main-resized')) {
                navbar.classList.remove('sidebar-main-resized');
                localStorage.setItem("expanderState", false);
            } else {
                navbar.classList.add('sidebar-main-resized');
                localStorage.setItem("expanderState", true);
            }
        })
    };

    const expanderState = localStorage.getItem("expanderState");
    if (expanderState === 'false')
        navbar.classList.add('sidebar-main-resized');
    else
        navbar.classList.remove('sidebar-main-resized');

}
catch (e) { }

/* ------------------------------------------------------------------------------
 * // 2. Thư viện DateTime
 * ---------------------------------------------------------------------------- */
// ------------------------------
//https://mariomka.github.io/vue-datetime/
var DateTime = luxon.DateTime;

/* ------------------------------------------------------------------------------
 *  # 3. Form validation
 * ---------------------------------------------------------------------------- */
// Setup module
// ------------------------------
const FormValidationStyles = function () {

    // Config
    const _componentValidationCustom = function () {

        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.querySelectorAll('.needs-validation');

        // Loop over them and prevent submission
        forms.forEach(function (form) {
            form.addEventListener('submit', function (e) {
                if (!form.checkValidity()) {
                    e.preventDefault();
                    e.stopPropagation();
                    form.classList.add('invalid');
                } else {
                    form.classList.remove('invalid');
                }
                form.classList.add('was-validated');
            }, false);
        });
    };

    // Return objects assigned to module
    return {
        init: function () {
            _componentValidationCustom();
        }
    }
}();

// Initialize module 
document.addEventListener('DOMContentLoaded', function () {
    FormValidationStyles.init();
});


function resetFormJs() {
    var forms = document.querySelectorAll('.needs-validation');
    forms.forEach(function (form) {
        form.classList.remove('invalid');
        form.classList.remove('was-validated');
    });
}

function validateForm(querySelectorAll) {
    let field = true
    try {
        document.querySelectorAll(querySelectorAll + '.needs-validation').forEach(function (form) {
            if (!form.checkValidity())
                form.classList.add('invalid');
            else
                form.classList.remove('invalid');
            form.classList.add('was-validated');
        });

        $(querySelectorAll + ' :required').each(function (i) {
            if (this.checkValidity() == false)
                field = false
        });

    } catch (e) { }
    return field
}

function base64(file, callback) {
    var coolFile = {};
    function readerOnload(e) {
        var base64 = btoa(e.target.result);
        coolFile.base64 = base64;
        callback(coolFile)
    };

    var reader = new FileReader();
    reader.onload = readerOnload;

    var file = file[0].files[0];
    coolFile.filetype = file.type;
    coolFile.size = file.size;
    coolFile.filename = file.name;
    reader.readAsBinaryString(file);
}
function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}
function removeCharacterEnd(str) {
    var value = "";
    if (str != null || str != "") {
        value = str.substring(0, str.length - 2);
    }
    return value;
}
function showSuccessful() {
    try {
        var forms = document.querySelectorAll('.needs-validation');
        forms.forEach(function (form) {
            if (form.classList.contains("was-validated")) {
                setTimeout(function () {
                    form.classList.remove("was-validated");
                }, 600);
            }
        });
    } catch (e) { }
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function setCookie(name, value, daysToLive) {
    // Encode value in order to escape semicolons, commas, and whitespace
    //var cookie = name + "=" + encodeURIComponent(value);
    var cookie = name + "=" + value;

    if (typeof daysToLive === "number") {
        /* Sets the max-age attribute so that the cookie expires after the specified number of days */
        cookie += "; max-age=" + (daysToLive * 24 * 60 * 60) + "; path=/";
        document.cookie = cookie;
    }
}

// Creating a cookie
//document.cookie = "firstName=Christopher; path=/; max-age=" + 30 * 24 * 60 * 60;

//// Updating the cookie
//document.cookie = "firstName=Alexander; path=/; max-age=" + 365 * 24 * 60 * 60;

//  End Setup module
// ------------------------------


/* ------------------------------------------------------------------------------
 *  # 4. . Menu Actived
 * ---------------------------------------------------------------------------- */
$(document).ready(function () {
    var urlPage = window.location.pathname,
        urlHref = window.location.href,
        urlRegExp = new RegExp(urlPage.replace(/\/$/, '') + "$");
    $('.nav-item a').each(function () {
        if (urlHref == this.href) {
            $(this).addClass('active');
            $(this).parents(".nav-group-sub").addClass(" show");
            $(this).parents(".nav-item-submenu").addClass(" nav-item-open");
        }

        if (urlRegExp.test(this.href.replace(/\/$/, '')) && urlRegExp != "/$/") {
            $(this).addClass('active');
            $(this).parents(".nav-group-sub").addClass(" show");
            $(this).parents(".nav-item-submenu").addClass(" nav-item-open");
        }
    });
    //$('.sidebar-control').click(function () {
    //    if ($('.sidebar').hasClass("sidebar-main-resized"))
    //        localStorage.setItem("sidebar", 'closed');
    //    else
    //        localStorage.setItem("sidebar", 'open');
    //})

    //if (typeof (Storage) !== "undefined") {
    //    if (localStorage.getItem("sidebar") == 'closed') {
    //        $('.sidebar').addClass('sidebar-main-resized');
    //    } else if (localStorage.getItem("sidebar") == 'open') {
    //        $('.sidebar').removeClass('sidebar-main-resized');
    //    }
    //}
});
//  End menu
// ------------------------------

