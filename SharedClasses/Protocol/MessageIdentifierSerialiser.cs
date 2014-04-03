﻿using System;
using System.Net.Sockets;
using log4net;

namespace SharedClasses.Protocol
{
    /// <summary>
    /// This static class is used define what message gets what identifier,
    /// and used to serialise and deserialise Message Identifiers to their related Typed
    /// </summary>
    public static class MessageIdentifierSerialiser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (MessageIdentifierSerialiser));

        public static void SerialiseMessageIdentifier(int messageIdentifier, NetworkStream stream)
        {
            stream.Write(BitConverter.GetBytes(messageIdentifier), 0, 4);
            Log.Debug("Sent Message Identifier: " + messageIdentifier + " to stream");
        }

        public static int DeserialiseMessageIdentifier(NetworkStream stream)
        {
            var messageTypeBuffer = new byte[4];
            stream.Read(messageTypeBuffer, 0, 4);
            int messageIdentifier = BitConverter.ToInt32(messageTypeBuffer, 0);
            Log.Debug("Message Identifier " + messageIdentifier + " received from client");
            return messageIdentifier;
        }
    }
}