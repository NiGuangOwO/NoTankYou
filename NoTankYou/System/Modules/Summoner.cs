﻿using NoTankYou.Abstracts;
using NoTankYou.Localization;
using NoTankYou.Models;
using NoTankYou.Models.Enums;
using NoTankYou.Models.Interfaces;
using NoTankYou.Models.ModuleConfiguration;

namespace NoTankYou.System.Modules;

public class Summoner : ModuleBase
{
    public override ModuleName ModuleName => ModuleName.Summoner;
    protected override string DefaultWarningText { get; } = Strings.SummonerPet;
    public override IModuleConfigBase ModuleConfig { get; protected set; } = new GenericBattleConfiguration();

    private const uint SummonCarbuncleActionId = 25798;
    private const byte MinimumLevel = 2;
    private const byte ArcanistJobId = 26;
    private const byte SummonerJobId = 27;

    private readonly Debouncer petDebouncer = new();
    
    protected override bool ShouldEvaluate(IPlayerData playerData)
    {
        if (playerData.MissingClassJob(ArcanistJobId, SummonerJobId)) return false;
        if (playerData.GetLevel() < MinimumLevel) return false;
        if (!playerData.IsTargetable()) return false;

        return true;
    }
    
    protected override void EvaluateWarnings(IPlayerData playerData)
    {
        petDebouncer.Update(playerData.GetObjectId(), playerData.HasPet());
        if (petDebouncer.IsLockedOut(playerData.GetObjectId())) return;
        
        if (!playerData.HasPet())
        {
            AddActiveWarning(SummonCarbuncleActionId, playerData);
        }
    }
}