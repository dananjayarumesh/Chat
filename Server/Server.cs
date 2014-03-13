﻿using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using SharedClasses;
using SharedClasses.Serialisation;

namespace Server
{
    public class Server : ISubject
    {
        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static TcpListener listener;

        private const int ConcurrentSockets = 5;

        private readonly List<IObserver> observers;
        private Message clientMessage;

        // Use Strategy pattern to chose what TCP Serialisation method to use
        private ITcpSendBehaviour sendBehaviour;

        public const int PortNumber = 5004;

        public Server(ITcpSendBehaviour sendBehaviour)
        {
            SetSerialiseMethod(sendBehaviour);

            observers = new List<IObserver>();

            StartTcpInput();
        }

        public void SetSerialiseMethod(ITcpSendBehaviour wantedSendBehaviour)
        {
            sendBehaviour = wantedSendBehaviour;
        }

        public void RegisterObserver(IObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            observers.Remove(o);
        }

        public void NotifyObserversOfClientChange()
        {
            foreach (var observer in observers)
            {
                if (clientMessage != null)
                {
                    observer.Update(clientMessage);
                }
            }
        }

        private void StartTcpInput()
        {
            listener = new TcpListener(PortNumber);
            listener.Start();

            Log.Debug("Start the socket listener");

            for (int i = 0; i < ConcurrentSockets; i++)
            {
                var tcpInstance = new Thread(ListenForIncomingData) { Name = "Listener thread " + (i + 1) };


                tcpInstance.Start();
            }
        }

        private void ListenForIncomingData()
        {
            Log.Debug("Create listener thread");

            while (true)
            {
                Socket socket = listener.AcceptSocket();

                Stream networkStream = new NetworkStream(socket);

                Log.Debug("Deserialise Data");
                clientMessage = sendBehaviour.Deserialise(networkStream);

                ParseClientData();
                Log.Debug("Notify Clients of change");
                NotifyObserversOfClientChange();
            }
        }

        private void ParseClientData()
        {
            Log.Info("User: " + clientMessage + " - " + clientMessage);
        }
    }
}