﻿using System.Net.Sockets;
using SharedClasses.Message;
using SharedClasses.Serialiser.EntitySerialiser;

namespace SharedClasses.Serialiser.MessageSerialiser
{
    /// <summary>
    /// Used to serialise and deserialise a <see cref="ConversationRequest" /> message
    /// Uses <see cref="ConversationSerialiser" /> for its underlying serialiser
    /// </summary>
    internal sealed class ConversationNotificationSerialiser : Serialiser<ConversationNotification>
    {
        private readonly ConversationSerialiser conversationSerialiser = new ConversationSerialiser();
        private readonly NotificationTypeSerialiser notificationTypeSerialiser = new NotificationTypeSerialiser();

        protected override void Serialise(NetworkStream networkStream, ConversationNotification message)
        {
            notificationTypeSerialiser.Serialise(networkStream, message.NotificationType);
            conversationSerialiser.Serialise(networkStream, message.Conversation);
        }

        public override IMessage Deserialise(NetworkStream networkStream)
        {
            NotificationType notificationType = notificationTypeSerialiser.Deserialise(networkStream);
            var conversation = new ConversationNotification(conversationSerialiser.Deserialise(networkStream), notificationType);

            Log.InfoFormat($"{conversation.MessageIdentifier} message deserialised");

            return conversation;
        }
    }
}