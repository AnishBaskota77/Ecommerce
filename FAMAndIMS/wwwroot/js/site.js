ShowPopUp = (Url, title = '', aclass = null) => {

    var decodeurl = decodeURIComponent(Url);

    $.ajax({
        type: "GET",
        url: decodeurl,
        success: function (res) {
            aclass == null ? $("#add-new .modal-dialog").addClass("modal-lg") : $("#add-new .modal-dialog").removeClass().addClass(aclass);
            $("#add-new .modal-body").html(res);
            $("#add-new .modal-title").html(title);
            $("#add-new").modal({ backdrop: 'static', keyboard: false });
            $("#add-new").modal('show');
        }, error: function (err) {
            console.log(err, "error");
        }
    })
}

//read image url
function readURL(input, id) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {

            var data = e.target.result;

            $('#' + id).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
};

//validate image file
function validateImageFile(fileInput, id) {
    var file = fileInput.files[0];

    if (file) {
        var allowedExtensions = ["jpg", "jpeg", "png", "svg", "gif", "bmp"];
        var fileExtension = file.name.split(".").pop().toLowerCase();

        if (!allowedExtensions.includes(fileExtension)) {
            fileInput.value = ""; // Clear the file input
            $(id + "0").html("Invalid image format. Only JPEG, PNG, SVG, GIF, and BMP formats are allowed.");

            alert("Invalid image format. Only JPEG, PNG, SVG, GIF, and BMP formats are allowed.");
            return false;
        }
        // Check file size
        var fileSize = file.size / 1024; // Size in KB
        var maxFileSize = 2000; // Maximum file size in KB

        if (fileSize > maxFileSize) {
            fileInput.value = ""; // Clear the file input
            $(id + "0").html("File size exceeds the maximum limit of 2000 KB.");

            alert("File size exceeds the maximum limit of 2000 KB.");
            return false;
        }
    }
    return true;
}


showImage = (data, id) => {
    debugger;
    var isValid = validateImageFile(data, id);
    if (isValid) {
        $('#' + id).removeAttr('hidden');
        readURL(data, id);
    }
}

function changeStatus(id, isActive, controllerName, actionName) {
   
    $.ajax({
        url: '/' + controllerName + '/' + actionName, 
        type: 'POST',
        data: {
            Id: id,
            IsActive: isActive
        },
        success: function (result) {
            Pagination.getData();
        },
        error: function (xhr, status, error) {
            alert('Error: ' + error);
        }
    });
}

function togglePasswordVisibility(passwordId, eyeIconId) {
  
    var passwordInput = document.getElementById(passwordId);
    var eyeIcon = document.getElementById(eyeIconId);

    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        eyeIcon.classList.remove("ri-eye-line");
        eyeIcon.classList.add("ri-eye-off-fill");
    } else {
        passwordInput.type = "password";
        eyeIcon.classList.remove("ri-eye-off-fill");
        eyeIcon.classList.add("ri-eye-line");
    }
}


$(document).ready(function () {
    // Add backdrop element
    $('body').append('<div class="main-backdrop"></div>');

    // Enable tooltips everywhere
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Show sidebar in mobile
    $('#menuLink').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-show');
    });

    // Hide sidebar in mobile
    $('body').on('click', '.main-backdrop', function () {
        $('body').removeClass('sidebar-show sideright-show');
    });

    // Sidebar Interaction
    const psSidebar = new PerfectScrollbar('#sidebarMenu', {
        suppressScrollX: true
    });

    $('.sidebar .nav-label').on('click', function (e) {
        e.preventDefault();
        var target = $(this).next('.nav-sidebar');
        $(target).slideToggle(function () {
            psSidebar.update();
        });
    });

    $('.sidebar .has-sub').on('click', function (e) {
        e.preventDefault();
        var target = $(this).next('.nav-sub');
        $(target).slideToggle(function () {
            psSidebar.update();
        });

        var siblings = $(this).closest('.nav-item').siblings();
        siblings.each(function () {
            var nav = $(this).find('.nav-sub');
            if (nav.is(':visible')) {
                nav.slideUp();
            }
        });
    });

    $('#sidebarFooterMenu').on('click', function (e) {
        e.preventDefault();
        $(this).closest('.sidebar').toggleClass('footer-menu-show');
    });

    // Header mobile effect on scroll
    function animateHead() {
        if ($(document).scrollTop() > 20) {
            $('.main-mobile-header').addClass('scroll');
        } else {
            $('.main-mobile-header').removeClass('scroll');
        }
    }

    $(window).scroll(function () {
        animateHead();
    });

    // Click interaction anywhere in the page
    $(document).on('click touchstart', function (e) {
        e.stopPropagation();

        // Closing sidebar footer menu
        if (!$(e.target).closest('.sidebar-footer').length) {
            $('.sidebar').removeClass('footer-menu-show');
        }
    });

    // Form search
    $('.form-search .form-control').on('focusin focusout', function () {
        $(this).parent().toggleClass('onfocus');
    });

    // Show/hide sidebar
    $('#menuSidebar').on('click', function (e) {
        e.preventDefault();
        if (window.matchMedia('(min-width: 992px)').matches) {
            $('body').toggleClass('sidebar-hide');
        } else {
            $('body').toggleClass('sidebar-show');
        }
    });

    // Show/hide sidebar offset
    $('#menuSidebarOffset').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-show');
    });

    // Load skin mode
    var skinMode = localStorage.getItem('skin-mode');
    if (skinMode) {
        $('html').attr('data-skin', 'dark');
        $('#skinMode .nav-link:last-child').addClass('active').siblings().removeClass('active');
    }

    // Set skin mode
    $('#skinMode .nav-link').on('click', function (e) {
       
        e.preventDefault();
        $(this).addClass('active').siblings().removeClass('active');

        var mode = $(this).text().toLowerCase();
        if (mode === 'dark') {
            $('html').attr('data-skin', 'dark');
            localStorage.setItem('skin-mode', mode);
        } else {
            localStorage.removeItem('skin-mode');
            $('html').attr('data-skin', '');
        }
    });

    // Load sidebar skin
    var sidebarSkin = localStorage.getItem('sidebar-skin');
    if (sidebarSkin) {
        $('.sidebar').attr('class', 'sidebar sidebar-' + sidebarSkin);

        if (sidebarSkin === 'prime') {
            $('#sidebarSkin .nav-link:nth-child(2)').addClass('active').siblings().removeClass('active');
        } else {
            $('#sidebarSkin .nav-link:last-child').addClass('active').siblings().removeClass('active');
        }
    }

    // Set sidebar skin
    $('#sidebarSkin .nav-link').on('click', function (e) {
        e.preventDefault();
        $(this).addClass('active').siblings().removeClass('active');

        var skin = $(this).text().toLowerCase();
        if (skin === 'default') {
            $('.sidebar').attr('class', 'sidebar');
            localStorage.removeItem('sidebar-skin');
        } else {
            $('.sidebar').attr('class', 'sidebar sidebar-' + skin);
            localStorage.setItem('sidebar-skin', skin);
        }
    });

    // Direction (LTR, RTL)
    $('#layoutDirection .nav-link').on('click', function (e) {
        e.preventDefault();
        var loc = window.location.href;
        var newLoc = '';

        if ($(this).is(':last-child')) {
            newLoc = loc.replace('/dashboard/', '/dashboard-rtl/')
                .replace('/apps/', '/apps-rtl/')
                .replace('/pages/', '/pages-rtl/');
        } else {
            newLoc = loc.replace('/dashboard-rtl/', '/dashboard/')
                .replace('/apps-rtl/', '/apps/')
                .replace('/pages-rtl/', '/pages/');
        }

        window.location.href = newLoc;
    });

    //toggle eye button for password and confirm password

    
});
