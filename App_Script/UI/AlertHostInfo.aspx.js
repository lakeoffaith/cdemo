var HCNetVideo = document.getElementById("HCNetVideo");
var HCNetVideo1 = document.getElementById("HCNetVideo1");
var obj = new Object();
var obj1 = new Object();

function loginHCNet(url, urlref) {
    obj = HCNetVideo;
    var i = obj.Login(url, 8000, "admin", "12345");

    obj1 = HCNetVideo1;
    i = obj1.Login(urlref, 8000, "admin", "12345");
}

function LoginOut() {
    var obj = new Object();
    obj = HCNetVideo;
    obj.Logout();

    var obj1 = new Object();
    obj1 = HCNetVideo1;
    obj1.Logout();
}

function StartPlay(ichanel, ichanelref) {
    var obj = new Object();
    obj = HCNetVideo;
    obj.StartRealPlay(ichanel, 0, 0);

    var obj1 = new Object();
    obj1 = HCNetVideo1;
    obj1.StartRealPlay(ichanelref, 0, 0);
}

function StopPlay() {
    var obj = new Object();
    obj = HCNetVideo;
    obj.StopRealPlay();

    var obj1 = new Object();
    obj1 = HCNetVideo1;
    obj1.StopRealPlay();
}

function AlertView(cid) {
    __PopupEventWindow.GetHCUrl(cid, function(result) {
        if ($("AlertCoordinates").value != cid) {
            if ($("activeUrl").value != result.url) {
                StopPlay();
                LoginOut();
                loginHCNet(result.url, result.urlref);
                StartPlay(result.iChanel, result.iChannelref);
                $("activeUrl").value = result.url;
            }
            else {
                StopPlay();
                StartPlay(result.iChanel);
            }

            $("AlertCoordinates").value = cid;
        }
    })
}