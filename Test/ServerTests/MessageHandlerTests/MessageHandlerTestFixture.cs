﻿using System.Collections.Generic;
using NUnit.Framework;
using Server;
using SharedClasses;
using SharedClasses.Domain;
using SharedClasses.Message;

namespace ServerTests.MessageHandlerTests
{
    public abstract class MessageHandlerTestFixture
    {
        protected User DefaultUser;

        protected int DefaultConversationDefaultUserIsIn;

        protected MockClientHandler ConnectedUserClientHandler { get; private set; }

        protected IServiceRegistry ServiceRegistry { get; private set; }

        [SetUp]
        public virtual void BeforeEachTest()
        {
            ServiceRegistry = new ServiceRegistry();

            EntityIdAllocatorFactory entityIdAllocatorFactory = new EntityIdAllocatorFactory();

            int userId= entityIdAllocatorFactory.AllocateEntityId<User>();
            DefaultUser = new User("user", userId, new ConnectionStatus(userId, ConnectionStatus.Status.Connected));

            ServiceRegistry.RegisterService<EntityIdAllocatorFactory>(entityIdAllocatorFactory);

            PopulateClientManager();
            PopulateRepositoryManager(entityIdAllocatorFactory);

        }

        private void PopulateClientManager()
        {
            var clientManager = new ClientManager();
            ConnectedUserClientHandler = new MockClientHandler();
            clientManager.AddClientHandler(DefaultUser.Id, ConnectedUserClientHandler);
            ServiceRegistry.RegisterService<IClientManager>(clientManager);
        }

        private void PopulateRepositoryManager(EntityIdAllocatorFactory idAllocator)
        {
            var repositoryManager = new RepositoryManager();
          
            repositoryManager.AddRepository<User>(new UserRepository());

            repositoryManager.AddRepository<Conversation>(new ConversationRepository());

            repositoryManager.AddRepository<Participation>(new ParticipationRepository());

            ServiceRegistry.RegisterService<RepositoryManager>(repositoryManager);

            int userId2 = idAllocator.AllocateEntityId<User>();
            int userId3 = idAllocator.AllocateEntityId<User>();

            List<int> usersToAddToConversation = new List<int> {DefaultUser.Id, userId2, userId3};

            UserRepository userRepository = (UserRepository)repositoryManager.GetRepository<User>();
            ParticipationRepository participationRepository = (ParticipationRepository)repositoryManager.GetRepository<Participation>();

            foreach (int userId in usersToAddToConversation)
            {
                User user = new User("user" + userId, userId, new ConnectionStatus(userId, ConnectionStatus.Status.Connected));
                userRepository.AddEntity(user);
            }

            SetUpMultiUserConversation(usersToAddToConversation, repositoryManager, idAllocator);

            DefaultConversationDefaultUserIsIn = participationRepository.GetConversationIdByParticipantsId(usersToAddToConversation);
        }

        public static void SetUpMultiUserConversation(IEnumerable<int> userIds, RepositoryManager repositoryManager, EntityIdAllocatorFactory idAllocator)
        {
            ConversationRepository conversationRepository = (ConversationRepository)repositoryManager.GetRepository<Conversation>();
            ParticipationRepository participationRepository = (ParticipationRepository)repositoryManager.GetRepository<Participation>();

            Conversation conversation  = new Conversation(idAllocator.AllocateEntityId<Conversation>());
            conversationRepository.AddEntity(conversation);

            foreach (int userId in userIds)
            {
                Participation participation = new Participation(idAllocator.AllocateEntityId<Participation>(), userId, conversation.Id);
                participationRepository.AddEntity(participation);
            }


        }

        public abstract void HandleMessage(IMessage message);
    }
}