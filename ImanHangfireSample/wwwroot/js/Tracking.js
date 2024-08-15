var connection = new signalR.HubConnectionBuilder().withUrl("/TrackingHub").build();

connection.start().then(function () {
    console.log("connect .......!");
}).catch(function (err) {
    return console.error(err.toString());
});

function connectToGroup(id) {
    connection.invoke("SubscribeSendMail", id);
    console.log("connect to group .... " + id);
}

connection.on("ShowStatus", function (percent,sendMailId,email) {
    $('#theprogressbar-' + sendMailId).attr('aria-valuenow', percent).css('width', percent + '%');
    $('#theprogressbar-' + sendMailId).text(percent + '%');
    $('#sendmaillogs').append('Email was sent to ' + email + '\n');

    var psconsole = $('#sendmaillogs');
    if (psconsole.length)
        psconsole.scrollTop(psconsole[0].scrollHeight - psconsole.height());
});