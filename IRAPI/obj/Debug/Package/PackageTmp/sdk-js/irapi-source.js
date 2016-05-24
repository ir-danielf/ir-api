if (!window.location.origin) {
    window.location.origin = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port: '');
}
window.BrowserAlert = false;
window.BrowserChecked = false;
function IRAPI(req) {
    if(!window.BrowserChecked) {
        window.BrowserChecked = !0;
        var navi = navigator.userAgent.match(/(MSIE .+?);/i);
        if(navi != null) {
            navi = parseInt(navi[1].replace("MSIE ",""));
            if(navi < 10) {
                window.BrowserAlert = !0;
                alert('Navegador não suportado\nAtualize seu navagador ou utilize outro');
                return;
            }
        }
    } else if(window.BrowserAlert) {
        return;
    }
    var apiUrl = 'https://www.ingressorapido.com.br/services/api/v1.2';
    var dataa = null;
    function hasCookie(cookie) {
        return (document.cookie.indexOf(cookie+"=") > -1)
    }
    function setCookie(name, value) {
        var expiresDate = new Date();
        expiresDate.setFullYear(expiresDate.getFullYear()+1);
        document.cookie = name + "=" + value + "; Path=/; expires=" + expiresDate.toGMTString();
    }
    function getCookie(cookie) {
        if(hasCookie(cookie)) {
            var cookies = document.cookie.split(";");
            for(var i = cookies.length-1; i > -1; i--) {
                var cookieData = cookies[i].replace(" ","").split("=");
                if(cookieData[0] === cookie) {
                    return cookieData[1];
                }
            }
        }
        return null;
    }
    function clientInfoDetect() {
        var ua = "";
        ua = navigator.userAgent.match(/(MSIE .+?);/i)||
            navigator.userAgent.match(/(OPR\/.+)/i)||
            navigator.userAgent.match(/(Chrome\/.+?) /i)||
            navigator.userAgent.match(/(Safari\/.+)/i)||
            navigator.userAgent.match(/(Firefox\/.+)/i);
        ua.pop();
        var v = ua.pop() + " " + navigator.platform + " " + new Date().getTime();
        v = v.replace(";","");
        setCookie("iractci", v);
    }
    function enc(t) {
        var i = parseInt(t.length/4);
        t = t.substring(i*4) + t.substring(i*2,i*3) + t.substring(0,i) + t.substring(i,i*2) + t.substring(i*3,i*4);
        return t;
    }
    function dec(t) {
        var i = parseInt(t.length/4);
        var r = t.length - i*4;
        t = t.substring(i+r,(i*2)+r)+t.substring((i*2)+r,(i*3)+r)+t.substring(r,i+r)+t.substring((i*3)+r)+t.substring(0,r);
        return t;
    }
    function send(req, sucessCallback, errorCallback) {
        function parametersToObject(url) {
            var result = {};
            for(var propertie in url.split("?")[1].split("&")){
                result[propertie.split(":")[0]] = unescape(propertie.split(":")[1]);
            }
            return result;
        }
        function objectToParameters(object) {
            var result = [];
            for(var propertie in object){
                result.push(propertie + "=" + object[propertie]);
            }
            return result.join("&");
        }
        req = {
            parameters: (req.parameters ? "?"+objectToParameters(req.parameters) : ""),
            url: req.url,
            method: req.method||"GET",
            async: (typeof req.async != 'undefined')? req.async: true,
            data: req.data,
            headers: req.headers||{}
        };
        if(XMLHttpRequest)
        {
            var xmlhttp = new XMLHttpRequest();
            xmlhttp.onreadystatechange = function() {
                if (xmlhttp.readyState == 4) {
                    var data = null;
                    try {
                        data = JSON.parse(xmlhttp.responseText);
                    } catch(e) {
                        data = xmlhttp.responseText;
                    }
                    if(xmlhttp.status >= 200 && xmlhttp.status < 300) {
                        if (typeof sucessCallback != "undefined")
                            sucessCallback(data, xmlhttp.status);
                    }else {
                        if (typeof errorCallback != "undefined")
                            errorCallback(data, xmlhttp.status);
                    }
                }
            };
            xmlhttp.open(req.method, req.url + req.parameters, req.async);
            for(var propertie in req.headers) {
                xmlhttp.setRequestHeader(propertie, req.headers[propertie]);
            }
            if (req.data) { 
                xmlhttp.send((typeof req.data == "string") ? req.data : objectToParameters(req.data)) 
            } else {
                xmlhttp.send();
            }
        }
    }
    function getAuthInfo() {
        var script = document.head.querySelector("script#irapi");
        if(script !== null) {
            var dir = script.src.split("#")[1];
            if(typeof dir != "undefined") {
                dir = "/"+dir
            } else {
                dir = "";
            }
        }
        var p1 = ""; var p2 = ""; var p3 = "";
        var names = "bbeea2bc2b81541fa057ba9007643c66027475b3e419a85b2186b36a4d29f3bd331692cffe1f1388b55e04519ede6f9d";
        names = [names.substring(16, 48), names.substring(48, 80), names.substring(96, 80)+names.substring(0, 16)];
        var origin = window.location.origin + dir;
        send({
            method: 'GET',
            url: origin+'/'+names[2]+'.txt'
        }, function (data) {
            p1 = data;
            send({
                method: 'GET',
                url: origin+'/'+names[1]+'.txt'
            }, function (data) {
                p2 = data;
                send({
                    method: 'GET',
                    url: origin+'/'+names[0]+'.txt'
                }, function (data) {
                    p3 = data;
                    p1 = p2 + p3 + p1;
                    dataa = p1.split(";");
                    dataa[0] += "==";
                });
            });
        });
    }
    function loginApi() {
        getAuthInfo();
        var count = 1;
        var interval = setInterval(function() {
            if(dataa != null) {
                clearInterval(interval);
                send({
                    method: 'POST',
                    url: apiUrl + '/token',
                    data: { username: dataa[1], password: dataa[0], grant_type: 'password' },
                    headers: { 'Content-Type': "application/x-www-form-urlencoded", ClientInfo: getCookie("iractci") }
                }, function (data) {
                    var a = data[".expires"];
                    setCookie("iracte", a);
                    setCookie("iractt", enc(data.access_token));
                    setCookie("iracttt","Bearer");
                    requestApi();
                });
            } else {
                if(count == 100) {
                    clearInterval(interval);
                    throw "Auth unavailable";
                }
                count++;
            }
        }, 100);
    }
    function refreshToken(callback) {
        var clientInfo = getCookie("iractci");
        var t = getCookie("iractt");
         t = dec(t);
            send({
                method: 'POST',
                async: req.async,
                url: apiUrl + '/token',
                data: {refresh_token: t, grant_type: 'refresh_token' },
                headers: { 'Content-Type': "application/x-www-form-urlencoded", ClientInfo: clientInfo }
            }, function (data) {
                var a = data[".expires"]
                setCookie("iracte", a);
                setCookie("iractt", enc(data.access_token));
                callback();
            }, loginApi);
    }
    function requestApi() {
        function request() {
            var clientInfo = getCookie("iractci");
            var t = getCookie("iractt");
            t = dec(t);
            var token = getCookie("iracttt") + " " + t;
            if (req.path[0] != "/")
                req.path = "/" + req.path;
            send({
                parameters: (req.method == "GET" && req.data != null) ? req.data : null,
                url: apiUrl + req.path,
                method: req.method,
                async: req.async,
                data: (req.data && req.method != "GET") ? JSON.stringify(req.data) : null,
                headers: { 'Content-Type': "application/json", ClientInfo: clientInfo, Authorization: token }
            }, req.success, function(data, status) {
                if(status == 401 && data.Message == "Authorization has been denied for this request.") {
                    refreshToken();
                } else {
                    req.error(data, status);
                }
            });
        }
        if(new Date(getCookie("iracte")) <= new Date()) {
            refreshToken(request);
        } else {
            request();
        }
    }
    if(!(hasCookie("iracte") && hasCookie("iractt") && hasCookie("iracttt") && hasCookie("iractci"))) {
        if(!hasCookie("iractci")) {
            clientInfoDetect();
        }
        loginApi();
    } else {
        requestApi();
    }
}