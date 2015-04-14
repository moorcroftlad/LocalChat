$(function () {
    var chat = $.connection.chatHub;

    chat.client.broadcastMessage = function (name, message) {
        var chatMessage = retrieveChatMessage(name, message);
        $('#discussion').append(chatMessage);
    };
    
    chat.client.updateChatHistory = function (messages) {
        $.each(messages, function (key, message) {
            var chatMessage = retrieveChatMessage(message.Username, message.Message);
            $('#discussion').append(chatMessage);
        });
    };
    
    $('#displayname').val(prompt('Enter your name:', ''));
    $('#message').focus();
    
    $.connection.hub.start().done(function () {
        chat.server.updateChatHistory();
        $('#sendmessage').click(function () {
            chat.server.send($('#displayname').val(), $('#message').val());
            $('#message').val('').focus();
        });
    });
});

function retrieveChatMessage(name, message) {
    var encodedName = $('<div />').text(name).html(),
        encodedMessage = $('<div />').text(message).html(),
        source = $("#message-template").html(),
        template = Handlebars.compile(source),
        context = {
            message: encodedMessage,
            name: encodedName
        };
    return $(template(context));
}