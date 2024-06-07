﻿using CairoDesktop.Application.Interfaces;
using CairoDesktop.Application.Structs;
using CairoDesktop.Common;
using CairoDesktop.Common.Localization;
using ManagedShell.Common.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CairoDesktop.Commands
{
    public class HibernateCommand : ICairoCommand
    {
        public ICairoCommandInfo Info => _info;

        private readonly Settings _settings;
        private readonly HibernateCommandInfo _info = new HibernateCommandInfo();

        public HibernateCommand(Settings settings)
        {
            _settings = settings;
            _settings.PropertyChanged += Settings_PropertyChanged;
            SetAvailable();
        }

        private void SetAvailable()
        {
            _info.IsAvailable = _settings.ShowHibernate && PowerHelper.CanHibernate();
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ShowHibernate")
            {
                SetAvailable();
            }
        }

        public void Setup() { }

        public bool Execute(params object[] parameters)
        {
            PowerHelper.Hibernate();

            return true;
        }

        public void Dispose()
        {
            _settings.PropertyChanged -= Settings_PropertyChanged;
        }
    }

    public class HibernateCommandInfo : ICairoCommandInfo, INotifyPropertyChanged
    {
        public string Identifier => "Hibernate";

        public string Description => "Hibernates the system.";

        public string Label => DisplayString.sCairoMenu_Hibernate;

        private bool _isAvailable;
        public bool IsAvailable
        {
            get => _isAvailable;
            internal set
            {
                if (value == _isAvailable) return;
                _isAvailable = value;
                OnPropertyChanged();
            }
        }

        public List<CairoCommandParameter> Parameters => new List<CairoCommandParameter>();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
