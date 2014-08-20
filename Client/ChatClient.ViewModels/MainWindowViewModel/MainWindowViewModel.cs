﻿using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using ChatClient.ViewModels.Commands;
using ChatClient.ViewModels.Properties;
using SharedClasses;
using SharedClasses.Domain;

namespace ChatClient.ViewModels.MainWindowViewModel
{
    public sealed class MainWindowViewModel : ViewModel
    {
        private readonly int userId;
        private readonly UserRepository userRepository;
        public EventHandler OpenUserSettingsWindowRequested;
        private Image userAvatar = Resources.DefaultUserImage;

        public MainWindowViewModel()
        {
            if (!IsInDesignMode)
            {
                userRepository = ClientService.RepositoryManager.UserRepository;
                userRepository.EntityUpdated += OnUserUpdated;
                userId = ClientService.ClientUserId;
            }
        }

        public Image UserAvatar
        {
            get { return userAvatar; }
            set
            {
                userAvatar = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get { return userRepository.FindEntityById(userId).Username; }
        }

        public ICommand OpenUserSettings
        {
            get { return new RelayCommand(OpenUserSettingsWindow); }
        }

        private void OpenUserSettingsWindow()
        {
            Application.Current.Dispatcher.Invoke(OnOpenUserSettingsWindowRequested);
        }

        private void OnUserUpdated(object sender, EntityChangedEventArgs<User> e)
        {
            if (e.Entity.Id == userId && !e.PreviousEntity.Avatar.Equals(e.Entity.Avatar))
            {
                UserAvatar = e.Entity.Avatar.UserAvatar;
            }
        }

        private void OnOpenUserSettingsWindowRequested()
        {
            EventHandler openUserSettingsWindowRequestedCopy = OpenUserSettingsWindowRequested;
            if (openUserSettingsWindowRequestedCopy != null)
            {
                openUserSettingsWindowRequestedCopy(this, EventArgs.Empty);
            }
        }
    }
}