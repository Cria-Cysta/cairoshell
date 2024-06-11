﻿using CairoDesktop.Application.Interfaces;
using CairoDesktop.Application.Structs;
using CairoDesktop.Common.Localization;
using System.Collections.Generic;

namespace CairoDesktop.Commands
{
    public class OpenCairoSettingsCommand : ICairoCommand
    {
        public ICairoCommandInfo Info => _info;

        private readonly ISettingsUIService _settingsUIService;
        private readonly OpenCairoSettingsCommandInfo _info = new OpenCairoSettingsCommandInfo();

        public OpenCairoSettingsCommand(ISettingsUIService settingsUIService) {
            _settingsUIService = settingsUIService;
            _info.IsAvailable = _settingsUIService != null;
        }

        public void Setup() { }

        public bool Execute(List<CairoCommandParameter> parameters)
        {
            string tabIdentifier = null;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    switch (parameter.Name)
                    {
                        case "TabIdentifier":
                            if (parameter.Value is string _tabIdentifier)
                            {
                                tabIdentifier = _tabIdentifier;
                            }
                            break;
                    }
                }
            }

            if (tabIdentifier != null)
            {
                _settingsUIService?.Show(tabIdentifier);
            }
            else
            {
                _settingsUIService?.Show();
            }

            return _settingsUIService != null;
        }

        public void Dispose() { }
    }

    public class OpenCairoSettingsCommandInfo : ICairoCommandInfo
    {
        public string Identifier => "OpenCairoSettings";

        public string Description => "Opens the Cairo settings window.";

        public string Label => DisplayString.sCairoMenu_CairoSettings;

        private bool _isAvailable;
        public bool IsAvailable
        {
            get => _isAvailable;
            internal set
            {
                _isAvailable = value;
            }
        }

        private List<CairoCommandParameter> _parameters = new List<CairoCommandParameter>()
        {
            new CairoCommandParameter {
                Name = "TabIdentifier",
                Description = "Settings tab to open",
                IsRequired = false
            }
        };

        public IReadOnlyCollection<CairoCommandParameter> Parameters => _parameters;
    }
}
