﻿using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using KamiLib.Command;
using KamiLib.Interfaces;
using KamiLib.System;
using KamiLib.UserInterface;
using KamiLib.Utility;

namespace NoTankYou.Views.Windows;

public class ConfigurationWindow : TabbedSelectionWindow
{
    private readonly List<ISelectionWindowTab> tabs;
    private readonly List<ITabItem> regularTabs;
    
    public ConfigurationWindow() : base("NoTankYou - 配置窗口", 0.0f, 150.0f)
    {
        tabs = new List<ISelectionWindowTab>(Reflection.ActivateOfInterface<ISelectionWindowTab>());
        regularTabs = new List<ITabItem>(Reflection.ActivateOfInterface<ITabItem>());
        
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(550, 350),
            MaximumSize = new Vector2(9999,9999),
        };
        
        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;
        
        CommandController.RegisterCommands(this);
    }
    
    protected override IEnumerable<ISelectionWindowTab> GetTabs() => tabs;
    protected override IEnumerable<ITabItem> GetRegularTabs() => regularTabs;

    public override bool DrawConditions()
    {
        if (Service.ClientState.IsPvP) return false;
        if (!Service.ClientState.IsLoggedIn) return false;

        return true;
    }

    [BaseCommandHandler("OpenConfigWindow")]
    public void OpenConfigWindow()
    {
        if (!Service.ClientState.IsLoggedIn) return;
        if (Service.ClientState.IsPvP) return;
            
        Toggle();
    }
}
