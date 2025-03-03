var FormHandler = {
    settings: {
        method: 'POST',
        actionUrl: undefined,
        selector: undefined,
        containerSelector: undefined
    },

    init: function (settings, successCallback) {
        if (!settings)
            throw new Error('parameter settings is required!');

        if (!settings.selector)
            throw new Error('selector is not defined in settings');

        if (!settings.actionUrl)
            throw new Error('actionUrl is not defined in settings');

        if (!settings.containerSelector)
            throw new Error('containerSelector is not defined in settings');

        var formSettings = $.extend({}, this.settings, settings);
        var $formEl = $(formSettings.selector);

        // Ensure Unobtrusive Validation is applied
        if ($.validator && $.validator.unobtrusive) {
            $.validator.unobtrusive.parse($formEl);
        }
        debugger;
/*        $formEl.validate();*/
        $formEl.on('submit', function (e) {
            e.preventDefault();
            debugger;

            var $form = $(this);
            if (!$form.is("form")) {
                console.error("Valid() called on a non-form element.");
                return false;
            }
            if (!$form.valid()) {
                return false;
            }

            $.ajax({
                cache: false,
                url: formSettings.actionUrl,
                type: 'POST',
                data: new FormData(this),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (successCallback && typeof successCallback === 'function') {
                        successCallback();
                    }
                },
                beforeSend: function () {
                    $formEl.find('button[type="submit"]').prop("disabled", true);
                    setLoadWaiting(1);
                    $('.loader').show();
                },
                error: function (err) {
                    switch (err.status) {
                        case 400:
                            $(formSettings.containerSelector).html(err.responseText);
                            break;
                        case 404:
                            console.log("Not Found!");
                            break;
                        default:
                            console.log(err.responseText);
                    }
                },
                complete: function () {
                    $formEl.find('button[type="submit"]').prop("disabled", false);
                    setLoadWaiting(0);
                    $('.loader').hide();
                }
            });
        });
    }
};



function setLoadWaiting(enable) {
    var $busyEl = $('.ajax-loading-busy');

    if (enable) {
        $busyEl.removeClass('display-none');
    } else {
        $busyEl.addClass('display-none');
    }
}
