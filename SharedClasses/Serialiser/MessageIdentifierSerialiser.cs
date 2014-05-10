﻿using System;
using System.Diagnostics.Contracts;
using System.Net.Sockets;
using log4net;

namespace SharedClasses.Serialiser
{
    /// <summary>
    /// This static class is used define what message gets what identifier,
    /// and used to serialise and deserialise Message Identifiers to their related Typed
    /// </summary>
    public sealed class MessageIdentifierSerialiser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (MessageIdentifierSerialiser));

        #region Serialise

        public void SerialiseMessageIdentifier(int messageIdentifier, NetworkStream stream)
        {
            Contract.Requires(stream != null);

            stream.Write(BitConverter.GetBytes(messageIdentifier), 0, 4);
            Log.Debug("Sent Message Identifier: " + messageIdentifier + " to stream");
        }

        #endregion

        #region Deserialise

        public int DeserialiseMessageIdentifier(NetworkStream stream)
        {
            Contract.Requires(stream != null);

            var messageTypeBuffer = new byte[4];
            stream.Read(messageTypeBuffer, 0, 4);
            int messageIdentifier = BitConverter.ToInt32(messageTypeBuffer, 0);
            Log.Debug("Message Identifier " + messageIdentifier + " received from client");
            return messageIdentifier;
        }

        #endregion
    }
}