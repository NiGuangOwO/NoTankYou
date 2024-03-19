﻿using KamiLib.AutomaticUserInterface;

namespace DailyDuty.Models;

[Category("GeneralOptions")]
public class SystemConfig
{
    public string CharacterName { get; set; } = string.Empty;
    public string CharacterWorld { get; set; } = string.Empty;

    [BoolConfig("WaitForDutyStart", "WaitForDutyStartHelp")]
    public bool WaitUntilDutyStart { get; set; } = true;

    public bool AutoSuppress { get; set; } = false;
    public int AutoSuppressTime { get; set; } = 60;
}