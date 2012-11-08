$(function () {
    $("input:button").click(function () {
        $.get("/sayhello", { Name: $("input:text").val() }, function (res) {
            var messages = res.Messages.split("\r\n");
            var length = messages.length;
            var results = $("#results");
            results.html("");
            for (var i = 0; i < length; i++) {
                var message = $("<div></div>").html(messages[i]);
                results.append(message);
            }
        });
    });
});