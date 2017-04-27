var sso = sso || {};
(function ($) {
    sso.host = "http://localhost:58806/";

    sso.utils = {
        isEmpty: function(str) {
            if (typeof (str) === "undefined") return true;
            if (str.replace(/(^s*)|(s*$)/g, "").length === 0) return true;
            return false;
        }
    };

    /**
    * 登陆
    * @param {signInfo}登陆信息
    *   {
	        userName:"",
	        password:"",
	        rememberMe:false,
	        returnUrl:""
        }
    */
    sso.login = function(signInfo) {
        if (sso.utils.isEmpty(signInfo.userName)) {
            alert("用户名不能为空");
            return;
        }
        if (sso.utils.isEmpty(signInfo.password)) {
            alert("登陆密码不能为空");
            return;
        }
        $.ajax({
            url: sso.host + "Account/SignIn",
            dataType: 'jsonp',
            type: 'GET',
            contentType: 'application/json',
            data: signInfo
        });
    };

    /**
    * 三方登陆
    * @param {signInfo}登陆信息
    *   {
	        loginProvider:"",
	        providerKey:"",
	        rememberMe:false,
	        returnUrl:""
        }
    */
    sso.externalLogin = function(signInfo) {
        if (sso.utils.isEmpty(signInfo.loginProvider)) {
            alert("三方登陆来源不能为空");
            return;
        }
        if (sso.utils.isEmpty(signInfo.providerKey)) {
            alert("三方登陆唯一Key不能为空");
            return;
        }
        $.ajax({
            url: sso.host + "Account/ExternalSignIn",
            dataType: 'jsonp',
            type: 'GET',
            contentType: 'application/json',
            data: signInfo
        });
    };

    /**
     * 注销
     */
    sso.logOut = function() {
        $.ajax({
            url: sso.host + "Account/SignOut",
            dataType: 'jsonp',
            type: 'GET',
            contentType: 'application/json',
            data: {}
        });
    };

    /**
     * sso服务器登陆成功后jsonp回调
     * @param {string[]}需要通知的Url集合
     */
    sso.notify = function () {
        var createScript = function (src) {
            $("<script><//script>").attr("src", src).appendTo("body");
        };

        var urlList = arguments;
        for (var i = 1; i < urlList.length; i++) {
            createScript(urlList[i]);
        }

        //延时执行，避免跳转时cookie还未写入成功
        setTimeout(function () {
            if (urlList[0] === "refresh") {
                window.location.reload();
            } else {
                window.location.href = urlList[0];
            }
        }, 1000);
    };

    /**
     * sso服务器登陆失败后jsonp回调
     * @param {code}错误码
     * @param {msg}错误消息
     */
    sso.error= function(code, msg) {
        alert(msg);
    }
})(jQuery);