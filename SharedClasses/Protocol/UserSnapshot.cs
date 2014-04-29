﻿using System;
using System.Collections.Generic;
using SharedClasses.Domain;

namespace SharedClasses.Protocol
{
    /// <summary>
    /// Sends a list of currently connected users to the recently logged in Client
    /// </summary>
    [Serializable]
    public sealed class UserSnapshot : IMessage
    {
        public UserSnapshot(IEnumerable<User> users)
        {
            Users = users;
            Identifier = MessageNumber.UserSnapshot;
        }

        public IEnumerable<User> Users { get; private set; }
        public int Identifier { get; private set; }
    }
}