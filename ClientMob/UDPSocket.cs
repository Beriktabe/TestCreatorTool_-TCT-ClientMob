using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP
{
    public class UDPSocket
    {
        public Socket _socket;
        private const int bufSize = 8 * 1024;
        private State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;

        public delegate void onReceiveEventHandler(object sender, dataReceiveEventArgs e);
        public event onReceiveEventHandler onReceive;
        public class dataReceiveEventArgs : EventArgs 
        {
            public dataReceiveEventArgs(IPEndPoint ip, string data)
            {
                this.ip = ip;
                this.data = data;
            }

            public IPEndPoint ip { get; private set; }
            public string data { get; private set; }
        }

        protected virtual void _onRecieve(dataReceiveEventArgs e)
        {
            if (onReceive != null) onReceive(this, e);
        }

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public void Server(string address, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Receive();            
        }

        public void Client(string address, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Connect(IPAddress.Parse(address), port);
            Receive();            
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
            }, state);
        }

        private void Receive()
        {

            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                try
                {
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);


                    _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    
                    dataReceiveEventArgs z = new dataReceiveEventArgs((IPEndPoint)epFrom, Encoding.ASCII.GetString(so.buffer, 0, bytes));
                    _onRecieve(z);

                } catch { }
                
            }, state);
        }
    }
}