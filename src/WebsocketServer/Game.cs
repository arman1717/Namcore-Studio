﻿using System;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace TouchTableServer
{
    public class Game : WebSocketBehavior
    {
        private string _name;
        private static int _number = 0;
        private string _prefix;

        public Game()
          : this(null)
        {
        }

        public Game(string prefix)
        {
            _prefix = !prefix.IsNullOrEmpty() ? prefix : "anon#";
        }

        private string getName()
        {
            var name = Context.QueryString["name"];
            return !name.IsNullOrEmpty() ? name : _prefix + getNumber();
        }

        private static int getNumber()
        {
            return Interlocked.Increment(ref _number);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Sessions.Broadcast(String.Format("{0} got logged off...", _name));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.Broadcast(String.Format("{0}: {1}", _name, e.Data));
        }

        protected override void OnOpen()
        {
            _name = getName();
        }
    }
}