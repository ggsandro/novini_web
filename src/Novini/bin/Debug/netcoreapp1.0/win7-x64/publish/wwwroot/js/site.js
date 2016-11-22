var overlay = document.getElementById('overlay');
var contacts = document.getElementById('contacts');
var contactsButton = document.getElementById('contactsbutton');
var form = document.getElementById("addNewsItem");
var okMessage = document.getElementById('okmessage');
var errorMessage = document.getElementById('errormessage');
var addNewsButton = document.getElementById('addNewsButton');
var moreNewsButton = document.getElementById('moreNewsButton');
var contentBody = document.getElementById('contentbody');
var elementsOnPage = document.getElementById('elementsOnPage');
var spiner = document.getElementById('spiner');

var addNewsHeader = document.getElementById('add-news-header');
var addNewsContent = document.getElementById('add-news-content');
var addNewsUrl = document.getElementById('add-news-url');

function openModal() {
    overlay.classList.remove("hidden-xs-up");
}

function closeModal() {
    overlay.classList.add("hidden-xs-up");
}

function openContacts() {
    contacts.classList.remove("hidden-xs-up");
}

function closeContacts() {
    contacts.classList.add("hidden-xs-up");
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

function showSpiner() {
    spiner.classList.remove("hidden-xs-up");
}

function hideSpiner() {
    spiner.classList.add("hidden-xs-up");
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
            setTimeout(hideOkMessage, 1000);
        });
    });

    // We define what will happen in case of error
    request.addEventListener("error", function (event) {
        closeModal();
        showErrorMessage();
        var transitionEvent = whichTransitionEvent();
        transitionEvent && errorMessage.addEventListener(transitionEvent, function () {
            setTimeout(hideErrorMessage, 1000);
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

    contactsButton.addEventListener("click", function (event) {
        event.preventDefault();
        openContacts();
    });

    //More news
    moreNewsButton.addEventListener("click", function (event) {
        var request = new XMLHttpRequest();
        request.open('GET', '/home/morenews?currentElements=' + elementsOnPage.value, true);

        request.onload = function () {
            if (request.status >= 200 && request.status < 400) {
                // Success!
                var dataArray = JSON.parse(request.responseText);
                var htmlToAdd = "";
                for (var i = 0; i < dataArray.length ; i++) {
                    var htmlElem = RenderTemplate(dataArray[i].Url, dataArray[i].Title, extractDomain(dataArray[i].Url), dataArray[i].Content, dataArray[i].TimeStampString);
                    htmlToAdd += htmlElem;
                }
                contentBody.insertAdjacentHTML('beforeend', htmlToAdd);
                elementsOnPage.value += dataArray.length;
                hideSpiner();
            } else {
                // We reached our target server, but it returned an error
                hideSpiner();
            }
        };

        request.onerror = function () {
            alert("asdasd");
            hideSpiner();
            // There was a connection error of some sort
        };

        request.send();
        showSpiner();
    });
});

function RenderTemplate(url, title, urlDomain, content, timeStampString) {
    return "<div class=\"col-xs-12 col-md-custom-4\">" +
                        "<div class=\"card\">" +
                            "<div class=\"card-header\">" +
                                "<a href=\"" + url + "\" class=\"news-title\" target=\"_blank\">" + title + "</a>" +
                            "</div>" +
                            "<div class=\"card-block\">" +
                                "<label class=\"text-muted\">" + urlDomain + "</label>" +
                                "<label class=\"text-muted pull-xs-right\">" + timeStampString + "</label>" +
                                "<p class=\"card-text\">" + content + "</p>" +
                            "</div>" +
                        "</div>" +
                    "</div> ";
}

function extractDomain(url) {
    var domain;
    //find & remove protocol (http, ftp, etc.) and get domain
    if (url.indexOf("://") > -1) {
        domain = url.split('/')[2];
    }
    else {
        domain = url.split('/')[0];
    }
    //find & remove port number
    domain = domain.split(':')[0];
    return domain;
}
