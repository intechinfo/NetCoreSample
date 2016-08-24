(function () {
    var Countdown = function (el) {
        var end = -1,
            intervalId;

        var start = function (e) {
            if (isStarted()) return;
            end = e;
            refresh();
            intervalId = setInterval(refresh, 1000);
        };

        var refresh = function () {
            var now = moment(),
                e = moment(end),
                diff = e.diff(now);

            if (diff < 0) {
                stop();
            } else {
                display(diff);
            }
        };

        var display = function (duration) {
            duration = moment.duration(duration);

            el.innerHTML = '';
            el.appendChild(document.createTextNode(duration.format("Y-M-D h:m:s")));
        };

        var isStarted = function () {
            return end !== -1;
        };

        var stop = function () {
            if (!isStarted()) return;
            clearInterval(intervalId);
            end = -1;
        };

        this.start = start;
        this.stop = stop;
    };

    this.ITI = this.ITI || {};
    this.ITI.PrimarySchool = this.ITI.PrimarySchool || {};
    this.ITI.PrimarySchool.Countdown = Countdown;
})();