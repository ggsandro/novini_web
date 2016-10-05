var overlay = document.getElementById('overlay');
var form = document.getElementById("addNewsItem");
var okMessage = document.getElementById('okmessage');
var errorMessage = document.getElementById('errormessage');
var addNewsButton = document.getElementById('addNewsButton');
var moreNewsButton = document.getElementById('moreNewsButton');
var contentBody = document.getElementById('contentbody');

function openModal() {
    overlay.classList.remove("hidden-xs-up");
}

function closeModal() {
    overlay.classList.add("hidden-xs-up");
}

function showOkMessage() {
    okMessage.classList.add("show-message");
}

function hideOkMessage() {
    okMessage.classList.remove("show-message");
}

function showErrorMessage() {
    errorMessage.classList.add("show-message");
}

function hideErrorMessage() {
    errorMessage.classList.remove("show-message");
}

//catch end of transition
function whichTransitionEvent() {
    var t;
    var el = document.createElement('fakeelement');
    var transitions = {
        'transition': 'transitionend',
        'OTransition': 'oTransitionEnd',
        'MozTransition': 'transitionend',
        'WebkitTransition': 'webkitTransitionEnd'
    }

    for (t in transitions) {
        if (el.style[t] !== undefined) {
            return transitions[t];
        }
    }
}

function serializeForm() {
    var elems = form.elements;
    var serialized = [], i, len = elems.length, str = '';
    for (i = 0; i < len; i += 1) {
        var element = elems[i];
        var type = element.type;
        var name = element.name;
        var value = element.value;

        switch (type) {
            case 'text':
            case 'radio':
            case 'checkbox':
            case 'textarea':
            case 'select-one':
                str = name + '=' + value;
                serialized.push(str);
                break;
            default:
                break;
        }
    }
    return serialized.join('&');
}

function newPost() {
    var request = new XMLHttpRequest();

    // We define what will happen if the data are successfully sent
    request.addEventListener("load", function (event) {
        closeModal();
        showOkMessage();
        var transitionEvent = whichTransitionEvent();
        transitionEvent && okMessage.addEventListener(transitionEvent, function () {
            setTimeout(hideOkMessage, 4000);
        });
    });

    // We define what will happen in case of error
    request.addEventListener("error", function (event) {
        closeModal();
        showErrorMessage();
        var transitionEvent = whichTransitionEvent();
        transitionEvent && okMessage.addEventListener(transitionEvent, function () {
            setTimeout(hideOkMessage, 4000);
        });
    });

    request.open('POST', '/home/addnewsitem', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
    request.send(serializeForm());
}

//Send new NewsItem
window.addEventListener("load", function () {
    form.addEventListener("submit", function (event) {
        event.preventDefault();
        newPost();
    });

    addNewsButton.addEventListener("click", function (event) {
        event.preventDefault();
        openModal();
    });

    //More news
    moreNewsButton.addEventListener("click", function (event) {
        var request = new XMLHttpRequest();
        request.open('GET', '/home/morenews', true);

        request.onload = function () {
            if (request.status >= 200 && request.status < 400) {
                // Success!
                contentBody.appendChild(request.responseText);
            } else {
                // We reached our target server, but it returned an error
            }
        };

        request.onerror = function () {
            alert("asdasd");
            // There was a connection error of some sort
        };

        request.send();
    });
});

function RenderTemplate(url, title, urlDomain, content) {
    var newsTemplate = "<div class=\"col-md-4\">" +
                        "<div class=\"card\">" +
                            "<div class=\"card-header\">" +
                                "<a href=\"" + url + "\" class=\"news-title\" target=\"_blank\">" + title + "</a>" +
                            "</div>" +
                            "<div class=\"card-block\">" +
                                "<label class=\"text-muted\">" + urlDomain + "</label>" +
                                "<p class=\"card-text\">" + content + "</p>" +
                            "</div>" +
                        "</div>" +
                    "</div>";
}

