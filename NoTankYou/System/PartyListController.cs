﻿using System;
using System.Collections.Generic;
using System.Linq;
using KamiLib.AutomaticUserInterface;
using KamiLib.Utilities;
using NoTankYou.DataModels;
using NoTankYou.Models;
using NoTankYou.Models.Enums;
using NoTankYou.Views.Components;

namespace NoTankYou.System;

public class PartyListController : IDisposable
{
    private PartyListConfig config = new();
    
    private readonly PartyMemberOverlay?[] partyMembers = new PartyMemberOverlay[8];

    private static WarningState SampleWarning => new()
    {
        Message = "NoTankYou警告样例",
        Priority = 100,
        IconId = 786,
        IconLabel = "技能样例",
        SourceObjectId = Service.ClientState.LocalPlayer?.ObjectId ?? 0xE000000,
        SourcePlayerName = "玩家样例",
        SourceModule = ModuleName.Test,
    };

    public void Update()
    {
        foreach (var member in partyMembers)
        {
            member?.Update();
        }
    }

    public void Draw(List<WarningState> warnings)
    {
        if (!config.Enabled) return;

        if (config.SampleMode)
        {
            partyMembers[0]?.DrawWarning(SampleWarning);
            return;
        }

        if (config.SoloMode)
        {
            var warning = warnings
                .Where(warning => !config.BlacklistedModules.Contains(warning.SourceModule))
                .Where(warning => warning.SourceObjectId == Service.ClientState.LocalPlayer?.ObjectId)
                .MaxBy(warning => warning.Priority);
            
            partyMembers[0]?.DrawWarning(warning);
        }
        else
        {
            foreach (var partyMember in partyMembers)
            {
                var warning = warnings
                    .Where(warning => !config.BlacklistedModules.Contains(warning.SourceModule))
                    .Where(warning => warning.SourceObjectId == partyMember?.ObjectId)
                    .MaxBy(warning => warning.Priority);
            
                partyMember?.DrawWarning(warning);
            }
        }
    }

    public void Load()
    {
        config = LoadConfig();

        foreach (var index in Enumerable.Range(0, 8))
        {
            partyMembers[index] = new PartyMemberOverlay(config, index);
        }
    }

    public void Unload()
    {
        foreach (var member in partyMembers)
        {
            member?.Reset(true);
        }
    }

    public void Dispose() => Unload();
    public void DrawConfig() => DrawableAttribute.DrawAttributes(config, SaveConfig);
    private PartyListConfig LoadConfig() => CharacterFileController.LoadFile<PartyListConfig>("PartyListOverlay.config.json", config);
    public void SaveConfig() => CharacterFileController.SaveFile("PartyListOverlay.config.json", config.GetType(), config);
}