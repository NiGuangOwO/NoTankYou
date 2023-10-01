using KamiLib.Interfaces;
using NoTankYou.System;

namespace NoTankYou.UserInterface.Tabs;

public class PartyListOverlayConfigurationTab : ITabItem
{
    public string TabName => "小队列表覆盖层";
    public bool Enabled => true;
    public void Draw() => NoTankYouSystem.PartyListController.DrawConfig();
}