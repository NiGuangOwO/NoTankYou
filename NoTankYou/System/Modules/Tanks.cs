﻿using System;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.Game.Group;
using FFXIVClientStructs.Interop;
using KamiLib.Game;
using Lumina.Excel.GeneratedSheets;
using NoTankYou.Abstracts;
using NoTankYou.Localization;
using NoTankYou.Models;
using NoTankYou.Models.Enums;
using NoTankYou.Models.Interfaces;
using NoTankYou.Models.ModuleConfiguration;

namespace NoTankYou.System.Modules;

public unsafe class Tanks : ModuleBase
{
    public override ModuleName ModuleName => ModuleName.Tanks;
    public override IModuleConfigBase ModuleConfig { get; protected set; } = new TankConfiguration();
    protected override string DefaultWarningText { get; } = Strings.TankStance;

    private readonly uint[] tankClassJobArray = LuminaCache<ClassJob>.Instance
        .Where(job => job.Role is 1)
        .Select(r => r.RowId)
        .ToArray();

    private readonly uint[] tankStanceIdArray = LuminaCache<Status>.Instance
        .Where(status => status is { InflictedByActor: true, CanStatusOff: true, IsPermanent: true, ParamModifier: 500, PartyListPriority: 0})
        .Select(status => status.RowId)
        .ToArray();

    private const byte MinimumLevel = 10;
    
    protected override bool ShouldEvaluate(IPlayerData playerData)
    {
        if (GetConfig<TankConfiguration>().DisableInAlliance && IsInAllianceRaid()) return false;
        if (!IsTank(playerData)) return false;
        
        return true;
    }
    
    protected override void EvaluateWarnings(IPlayerData playerData)
    {
        if (GroupManager.Instance()->MemberCount is 0)
        {
            if (playerData.MissingStatus(tankStanceIdArray))
            {
                AddActiveWarning(GetActionIdForClass(playerData.GetClassJob()), playerData);
            }
        }
        else
        {
            if (GetConfig<TankConfiguration>().CheckAllianceTanks && IsInAllianceRaid())
            {
                if (AllianceHasStance()) return;
            }

            if (!PartyHasStance())
            {
                AddActiveWarning(GetActionIdForClass(playerData.GetClassJob()), playerData);
            }
        }
    }
    
    private bool PartyHasStance()
    {
        foreach (var partyMember in PartyMemberSpan.PointerEnumerator())
        {
            IPlayerData playerData = new PartyMemberPlayerData(partyMember);

            if (!IsTank(playerData)) continue;
            if (playerData.HasStatus(tankStanceIdArray)) return true;
        }

        return false;
    }

    private bool AllianceHasStance()
    {
        foreach (var partyMember in GroupManager.Instance()->AllianceMembersSpan.PointerEnumerator())
        {
            if (partyMember->ObjectID is 0xE0000000) continue;
            
            IPlayerData playerData = new PartyMemberPlayerData(partyMember);

            if (!IsTank(playerData)) continue;
            if (playerData.GameObjectHasStatus(tankStanceIdArray)) return true;
        }

        return false;
    }

    private bool IsTank(IPlayerData playerData)
    {
        if (playerData.MissingClassJob(tankClassJobArray)) return false;
        if (playerData.GetLevel() < MinimumLevel) return false;
        
        return true;
    }
    
    private static bool IsInAllianceRaid() => DutyLists.Instance.IsType(Service.ClientState.TerritoryType, DutyType.Alliance);

    private static uint GetActionIdForClass(byte classJob) => classJob switch
    {
        1 or 19 => 28u,
        3 or 21 => 48u,
        32 => 3629u,
        37 => 16142u,
        _ => throw new ArgumentOutOfRangeException(nameof(classJob), classJob, null),
    };
}