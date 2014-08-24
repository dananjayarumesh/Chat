﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;
using SharedClasses.Message;

namespace SharedClasses.Domain
{
    public abstract class Repository<T> : IRepository<T> where T : IEntity
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof (Repository<T>));

        private readonly ConcurrentDictionary<int, T> entitiesIndexedById = new ConcurrentDictionary<int, T>();

        public Type EnclosedEntityType
        {
            get { return typeof (T); }
        }

        public event EventHandler<EntityChangedEventArgs<T>> EntityAdded;

        public event EventHandler<EntityChangedEventArgs<T>> EntityUpdated;

        public event EventHandler<EntityChangedEventArgs<T>> EntityRemoved;

        /// <summary>
        /// Adds an <see cref="IEntity"/> to the repository.
        /// </summary>
        /// <param name="entity">The <see cref="IEntity"/> to add.</param>
        public void AddEntity(T entity)
        {
            entitiesIndexedById.TryAdd(entity.Id, entity);

            Log.DebugFormat("Entity with Id {0} added.", entity.Id);

            OnEntityAdded(entity);
        }

        /// <summary>
        /// Retrieves an <see cref="IEntity"/> entity from the repository.
        /// </summary>
        /// <param name="entityId">The <see cref="IEntity"/> entity Id to find.</param>
        /// <returns>The <see cref="IEntity"/> which matches the ID. If no <see cref="IEntity"/> is found, return null.</returns>
        public T FindEntityById(int entityId)
        {
            T entity;
            entitiesIndexedById.TryGetValue(entityId, out entity);
            return entity;
        }

        /// <summary>
        /// Retrieves all <see cref="IEntity"/>s from the repository.
        /// </summary>
        /// <returns>A collection of all <see cref="IEntity"/>s in the repository.</returns>
        public IEnumerable<T> GetAllEntities()
        {
            return new List<T>(entitiesIndexedById.Values);
        }

        private void OnEntityAdded(T entity)
        {
            var entityChangedEventArgs = new EntityChangedEventArgs<T>(entity, NotificationType.Create);

            EventHandler<EntityChangedEventArgs<T>> entityChangedCopy = EntityAdded;

            if (entityChangedCopy != null)
            {
                entityChangedCopy(this, entityChangedEventArgs);
            }
        }

        protected void OnEntityUpdated(T entity, T previousEntity)
        {
            var entityChangedEventArgs = new EntityChangedEventArgs<T>(entity, previousEntity);

            EventHandler<EntityChangedEventArgs<T>> entityUpdatedCopy = EntityUpdated;

            if (entityUpdatedCopy != null)
            {
                entityUpdatedCopy(this, entityChangedEventArgs);
            }
        }
    }
}