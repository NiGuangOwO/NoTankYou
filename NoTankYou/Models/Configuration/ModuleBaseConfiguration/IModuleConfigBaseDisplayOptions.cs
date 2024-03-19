﻿using KamiLib.AutomaticUserInterface;

namespace NoTankYou.Abstracts;

[Category("DisplayOptions", -1)]
public interface IModuleConfigBaseDisplayOptions
{
    [BoolConfig("CustomWarning")]
    public bool CustomWarning { get; set; }

    [StringConfig("CustomWarningText")]
    public string CustomWarningText { get; set; }
}