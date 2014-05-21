﻿using System;
using System.Collections.Generic;
using SharedClasses.Message;
using SharedClasses.Serialiser;

namespace SharedClasses
{
    /// <summary>
    /// Defines various relationships of message types, identifiers and serialisers
    /// </summary>
    public static class SerialiserRegistry
    {
        /// <summary>
        /// A readonly version of an Serialiser by Message Identifier dictionary. No one can alter this dictionary after compiling.
        /// </summary>
        public static readonly IReadOnlyDictionary<MessageNumber, ISerialiser> SerialisersByMessageIdentifier =
            new Dictionary<MessageNumber, ISerialiser>
            {
                {MessageNumber.ContributionRequest, new ContributionRequestSerialiser()},
                {MessageNumber.ContributionNotification, new ContributionNotificationSerialiser()},
                {MessageNumber.LoginRequest, new LoginRequestSerialiser()},
                {MessageNumber.UserNotification, new UserNotificationSerialiser()},
                {MessageNumber.UserSnapshotRequest, new UserSnapshotRequestSerialiser()},
                {MessageNumber.UserSnapshot, new UserSnapshotSerialiser()},
                {MessageNumber.ConversationSnapshotRequest, new ConversationSnapshotRequestSerialiser()},
                {MessageNumber.ConversationSnapshot, new ConversationSnapshotSerialiser()},
                {MessageNumber.ParticipationSnapshotRequest, new ParticipationSnapshotRequestSerialiser()},
                {MessageNumber.ParticipationSnapshot, new ParticipationSnapshotSerialiser()},
                {MessageNumber.ConversationRequest, new ConversationRequestSerialiser()},
                {MessageNumber.ConversationNotification, new ConversationNotificationSerialiser()},
                {MessageNumber.LoginResponse, new LoginResponseSerialiser()}
            };

        /// <summary>
        /// A readonly version of an Serialiser by Message Type dictionary. No one can alter this dictionary after compiling.
        /// </summary>
        public static readonly IReadOnlyDictionary<Type, ISerialiser> SerialisersByMessageType =
            new Dictionary<Type, ISerialiser>
            {
                {typeof (ContributionRequest), new ContributionRequestSerialiser()},
                {typeof (ContributionNotification), new ContributionNotificationSerialiser()},
                {typeof (LoginRequest), new LoginRequestSerialiser()},
                {typeof (UserNotification), new UserNotificationSerialiser()},
                {typeof (UserSnapshotRequest), new UserSnapshotRequestSerialiser()},
                {typeof (UserSnapshot), new UserSnapshotSerialiser()},
                {typeof (ConversationSnapshotRequest), new ConversationSnapshotRequestSerialiser()},
                {typeof (ConversationSnapshot), new ConversationSnapshotSerialiser()},
                {typeof (ParticipationSnapshotRequest), new ParticipationSnapshotRequestSerialiser()},
                {typeof (ParticipationSnapshot), new ParticipationSnapshotSerialiser()},
                {typeof (ConversationRequest), new ConversationRequestSerialiser()},
                {typeof (ConversationNotification), new ConversationNotificationSerialiser()},
                {typeof (LoginResponse), new LoginResponseSerialiser()}
            };
    }
}