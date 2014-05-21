﻿using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;
using SharedClasses.Message;

namespace SharedClasses.Serialiser
{
    /// <summary>
    /// Used to Serialise and Deserialise a <see cref="ConversationSnapshotRequest" /> object.
    /// </summary>
    internal sealed class ConversationSnapshotRequestSerialiser : ISerialiser<ConversationSnapshotRequest>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConversationSnapshotRequestSerialiser));

        private readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        private readonly MessageIdentifierSerialiser messageIdentifierSerialiser = new MessageIdentifierSerialiser();

        public void Serialise(ConversationSnapshotRequest message, NetworkStream stream)
        {
            messageIdentifierSerialiser.SerialiseMessageIdentifier(message.Identifier, stream);

            binaryFormatter.Serialize(stream, message);
            Log.Info(message.Identifier + " serialised and sent to network stream");
        }

        public void Serialise(IMessage message, NetworkStream stream)
        {
            Serialise((ConversationSnapshotRequest)message, stream);
        }

        public IMessage Deserialise(NetworkStream networkStream)
        {
            var conversationSnapshotRequest = (ConversationSnapshotRequest)binaryFormatter.Deserialize(networkStream);
            Log.Info("Network stream has received data and deserialised to a" + conversationSnapshotRequest.Identifier + " object");
            return conversationSnapshotRequest;
        }
    }
}
