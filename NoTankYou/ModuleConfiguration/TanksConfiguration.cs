﻿using Dalamud.Interface;
using ImGuiNET;
using ImGuiScene;
using NoTankYou.Components;
using NoTankYou.Data.Components;
using NoTankYou.Data.Modules;
using NoTankYou.Enums;
using NoTankYou.Interfaces;
using NoTankYou.Localization;
using NoTankYou.Utilities;

namespace NoTankYou.ModuleConfiguration
{
    internal class TanksConfiguration : IConfigurable
    {
        public ModuleType ModuleType => ModuleType.Tanks;
        public string ConfigurationPaneLabel => Strings.Modules.Tank.ConfigurationPanelLabel;
        public string AboutInformationBox => Strings.Modules.Tank.Description;
        public string TechnicalInformation => Strings.Modules.Tank.TechnicalDescription;
        public TextureWrap? AboutImage { get; }
        public GenericSettings GenericSettings => Settings;
        private static TankModuleSettings Settings => Service.Configuration.ModuleSettings.Tank;

        private readonly InfoBox AdditionalOptions = new()
        {
            Label = Strings.Common.Labels.AdditionalOptions,
            ContentsAction = () =>
            {
                if (ImGui.Checkbox(Strings.Modules.Tank.DisableInAllianceRaid, ref Settings.DisableInAllianceRaid))
                {
                    Service.Configuration.Save();
                }
            }
        };

        public TanksConfiguration()
        {
            AboutImage = Image.LoadImage("Tank");
        }

        public void DrawTabItem()
        {
            ImGui.TextColored(Settings.Enabled ? Colors.SoftGreen : Colors.SoftRed, Strings.Modules.Tank.Label);
        }

        public void DrawOptions()
        {
            AdditionalOptions.DrawCentered();
        }
    }
}
