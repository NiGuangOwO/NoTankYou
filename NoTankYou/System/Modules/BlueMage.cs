﻿using FFXIVClientStructs.FFXIV.Client.Game.Group;
using FFXIVClientStructs.Interop;
using NoTankYou.Abstracts;
using NoTankYou.Localization;
using NoTankYou.Models;
using NoTankYou.Models.Enums;
using NoTankYou.Models.Interfaces;
using NoTankYou.Models.ModuleConfiguration;

namespace NoTankYou.System.Modules;

public unsafe class BlueMage : ModuleBase
{
    public override ModuleName ModuleName => ModuleName.BlueMage;
    public override IModuleConfigBase ModuleConfig { get; protected set; } = new BlueMageConfiguration();
    protected override string DefaultWarningText { get; } = Strings.MightyGuard;

    private const uint MightyGuardStatusId = 1719;
    private const uint AetherialMimicryTankStatusId = 2124;
    private const uint MightyGuardActionId = 11417;
    private const byte BlueMageClassJob = 36;
    
    protected override bool ShouldEvaluate(IPlayerData playerData)
    {
        if (!playerData.HasClassJob(BlueMageClassJob)) return false;
        if (playerData.MissingStatus(AetherialMimicryTankStatusId)) return false;

        return true;
    }
    
    protected override void EvaluateWarnings(IPlayerData playerData)
    {
        if (GroupManager.Instance()->MemberCount is 0)
        {
            if (playerData.MissingStatus(MightyGuardStatusId))
            {
                AddActiveWarning(MightyGuardActionId, playerData);
            }
        }
        else
        {
            if (!PartyHasStance())
            {
                AddActiveWarning(MightyGuardActionId, playerData);
            }
        }
    }

    private static bool PartyHasStance()
    {
        foreach (var partyMember in PartyMemberSpan.PointerEnumerator())
        {
            IPlayerData playerData = new PartyMemberPlayerData(partyMember);

            if (playerData.MissingStatus(AetherialMimicryTankStatusId)) continue;
            if (playerData.HasStatus(MightyGuardStatusId)) return true;
        }

        return false;
    }
}