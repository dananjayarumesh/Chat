﻿using System.Net.Sockets;
using SharedClasses.Message;

namespace SharedClasses.Serialiser.MessageSerialiser
{
    /// <summary>
    /// Used to Serialise and Deserialise a <see cref="ParticipationSnapshotRequest" /> object.
    /// </summary>
    internal sealed class ParticipationSnapshotRequestSerialiser : Serialiser<ParticipationSnapshotRequest>
    {
        private readonly ISerialisationType serialiser = new BinarySerialiser();

        protected override void Serialise(NetworkStream networkStream, ParticipationSnapshotRequest message)
        {
            serialiser.Serialise(networkStream, message);
        }

        public override IMessage Deserialise(NetworkStream networkStream)
        {
            var participationSnapshotRequest = (ParticipationSnapshotRequest) serialiser.Deserialise(networkStream);
            Log.InfoFormat($"Network stream has received data and deserialised to a {participationSnapshotRequest.MessageIdentifier} object");
            return participationSnapshotRequest;
        }
    }
}