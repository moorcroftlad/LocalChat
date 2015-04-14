using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace LocalChat.SignalR
{
    public class ChatHub : Hub
    {
        public void Send(string username, string message)
        {
            var chatRoom = RetrieveChatRoom();
            var chatMessage = new ChatMessage(username, message);
            chatRoom.AddMessage(chatMessage);
            UpdateChatRoom(chatRoom);
            Clients.All.broadcastMessage(username, message);
        }

        public void UpdateChatHistory()
        {
            var chatRoom = RetrieveChatRoom();
            Clients.Caller.updateChatHistory(chatRoom.RetrieveAll());
        }

        private ChatRoom RetrieveChatRoom()
        {
            var ip = GetClientIp(Context.Request);
            return HttpRuntime.Cache[ip] as ChatRoom ?? new ChatRoom();
        }

        private void UpdateChatRoom(ChatRoom chatRoom)
        {
            var ip = GetClientIp(Context.Request);
            HttpRuntime.Cache.Insert(ip, chatRoom);
        }

        private static string GetClientIp(IRequest request)
        {
            return Get<string>(request.Environment, "server.RemoteIpAddress");
        }

        private static T Get<T>(IDictionary<string, object> env, string key)
        {
            object value;
            return env.TryGetValue(key, out value) ? (T)value : default(T);
        }
    }

    public class ChatRoom
    {
        private readonly List<ChatMessage> _messages;

        public ChatRoom()
        {
            _messages = new List<ChatMessage>();
        }

        public void AddMessage(ChatMessage chatMessage)
        {
            _messages.Add(chatMessage);
        }

        public List<ChatMessage> RetrieveAll()
        {
            return _messages;
        }
    }

    public class ChatMessage
    {
        public ChatMessage(string username, string message)
        {
            Username = username;
            Message = message;
            Timestamp = DateTime.Now;
        }

        public string Username { get; private set; }
        public string Message { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}