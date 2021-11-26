﻿using ImGuiNET;
using System;
using System.Numerics;
using Dalamud.Interface.Windowing;

namespace NoTankYou
{
    internal class SettingsWindow : Window, IDisposable
    {
        public bool visible = false;

        private readonly Vector2 WindowSize = new Vector2(350, 275);

        public SettingsWindow() : 
            base("Settings Window")
        {

            SizeConstraints = new WindowSizeConstraints()
            {
                MinimumSize = new(WindowSize.X, WindowSize.Y),
                MaximumSize = new(WindowSize.X, WindowSize.Y)
            };

            Flags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoResize;
        }

        public void Dispose()
        {
            
        }

        private void DrawForceShowBannerCheckbox()
        {
            ImGui.Text("Force show 'Tank Stance' warning banner");
            ImGui.Checkbox("Force show warning banner", ref Service.Configuration.ForceShowNoTankWarning);
            ImGui.Spacing();
        }

        private void DrawEnablePluginButton()
        {
            ImGui.Checkbox("Enable Plugin", ref Service.Configuration.ShowNoTankWarning);
            ImGui.Spacing();
        }

        private void DrawInstanceLoadDelayTimeTextField()
        {
            ImGui.Text("Hide warning banner on map change for (milliseconds)");
            ImGui.InputInt("", ref Service.Configuration.InstanceLoadDelayTime, 1000, 5000);
            ImGui.Spacing();
        }

        private void DrawEnableClickThroughCheckbox()
        {
            ImGui.Checkbox("Enable Clickthrough", ref Service.Configuration.EnableClickthrough);
            ImGui.Spacing();
        }
        private void DrawDisableInAllianceRaid()
        {
            ImGui.Checkbox("Disable in Alliance Raid", ref Service.Configuration.DisableInAllianceRaid);
            ImGui.Spacing();
        }

        private void DrawSaveAndCloseButtons()
        {
            ImGui.BeginTable("SaveTable", 2);

            ImGui.TableNextColumn();
            if( ImGui.Button("Save", new(100, 25)) )
            {
                Service.Configuration.Save();
            }

            ImGui.TableNextColumn();
            if( ImGui.Button("Save & Close", new(150, 25)))
            {
                Service.Configuration.Save();
                IsOpen = false;
            }
            ImGui.EndTable();
            ImGui.Spacing();
        }

        public override void Draw()
        {
            if(!IsOpen)
            {
                return;
            }
            else
            {
                DrawEnablePluginButton();
                DrawForceShowBannerCheckbox();
                DrawEnableClickThroughCheckbox();
                DrawInstanceLoadDelayTimeTextField();
                DrawDisableInAllianceRaid();
                DrawSaveAndCloseButtons();
            }
        }

        public override void OnClose()
        {
            base.OnClose();
            Service.Configuration.Save();
        }
    }
}