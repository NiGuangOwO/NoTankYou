﻿using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Party;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using ImGuiNET;
using ImGuiScene;
using System;
using System.IO;
using System.Numerics;

namespace NoTankYou.DisplaySystem
{
    public abstract class WarningBanner : Window, IDisposable
    {
        public const ImGuiWindowFlags MoveWindowFlags =
                    ImGuiWindowFlags.NoScrollbar |
                    ImGuiWindowFlags.NoScrollWithMouse |
                    ImGuiWindowFlags.NoTitleBar |
                    ImGuiWindowFlags.NoCollapse |
                    ImGuiWindowFlags.NoBringToFrontOnFocus |
                    ImGuiWindowFlags.NoFocusOnAppearing |
                    ImGuiWindowFlags.NoNavFocus |
                    ImGuiWindowFlags.NoResize;

        public const ImGuiWindowFlags IgnoreInputFlags =
                    ImGuiWindowFlags.NoScrollbar |
                    ImGuiWindowFlags.NoTitleBar |
                    ImGuiWindowFlags.NoCollapse |
                    ImGuiWindowFlags.NoResize |
                    ImGuiWindowFlags.NoBackground |
                    ImGuiWindowFlags.NoBringToFrontOnFocus |
                    ImGuiWindowFlags.NoFocusOnAppearing |
                    ImGuiWindowFlags.NoNavFocus |
                    ImGuiWindowFlags.NoInputs;

        protected TextureWrap ImageLarge;
        protected TextureWrap ImageMedium;
        protected TextureWrap ImageSmall;
        protected TextureWrap SelectedImage;

        public bool Visible { get; set; } = false;
        public bool Paused { get; set; } = false;
        public bool Forced { get; set; } = false;
        public bool Disabled { get; set; } = false;

        protected abstract ref bool RepositionModeBool { get; }
        protected abstract ref bool ForceShowBool { get; }
        protected abstract ref bool ModuleEnabled { get; }

        protected abstract void UpdateInPartyInDuty();
        protected abstract void UpdateSoloInDuty();
        protected abstract void UpdateSoloEverywhere();

        public enum ImageSize
        {
            Small,
            Medium,
            Large
        }

        protected WarningBanner(string windowName, string imageName) : base(windowName)
        {
            var assemblyLocation = Service.PluginInterface.AssemblyLocation.DirectoryName!;
            var smallPath = Path.Combine(assemblyLocation, $@"images\{imageName}_Small.png");
            var mediumPath = Path.Combine(assemblyLocation, $@"images\{imageName}_Medium.png");
            var largePath = Path.Combine(assemblyLocation, $@"images\{imageName}_Large.png");

            ImageSmall = Service.PluginInterface.UiBuilder.LoadImage(smallPath);
            ImageMedium = Service.PluginInterface.UiBuilder.LoadImage(mediumPath);
            ImageLarge = Service.PluginInterface.UiBuilder.LoadImage(largePath);

            switch (Service.Configuration.ImageSize)
            {
                case ImageSize.Small:
                    SelectedImage = ImageSmall;
                    break;

                case ImageSize.Medium:
                    SelectedImage = ImageMedium;
                    break;

                case ImageSize.Large:
                    SelectedImage = ImageLarge;
                    break;

                default:
                    SelectedImage = ImageLarge;
                    break;
            }

            SizeConstraints = new WindowSizeConstraints()
            {
                MinimumSize = new(this.SelectedImage.Width, this.SelectedImage.Height),
                MaximumSize = new(this.SelectedImage.Width, this.SelectedImage.Height)
            };
        }

        protected void PreUpdate()
        {
            IsOpen = ModuleEnabled;
        }

        public void Update()
        {
            PreUpdate();

            if (!IsOpen) return;

            Forced = ForceShowBool || RepositionModeBool;

            // If we are in a party, and in a duty
            if (Service.PartyList.Length > 0 && Service.Condition[ConditionFlag.BoundByDuty] && Service.Configuration.ProcessingMainMode == Configuration.MainMode.Party)
            {
                UpdateInPartyInDuty();
            }

            // If we are in a duty, and have solo mode enabled
            else if (Service.Configuration.ProcessingSubMode == Configuration.SubMode.OnlyInDuty && Service.Condition[ConditionFlag.BoundByDuty] && Service.Configuration.ProcessingMainMode == Configuration.MainMode.Solo)
            {
                UpdateSoloInDuty();
            }

            else if (Service.Configuration.ProcessingSubMode == Configuration.SubMode.Everywhere && Service.Configuration.ProcessingMainMode == Configuration.MainMode.Solo)
            {
                UpdateSoloEverywhere();
            }

            else
            {
                Visible = false;
            }
        }

        public override void PreDraw()
        {
            base.PreDraw();

            Flags = RepositionModeBool ? MoveWindowFlags : IgnoreInputFlags;
        }

        public override void Draw()
        {
            if (!IsOpen) return;

            if (Forced)
            {
                ImGui.SetCursorPos(new Vector2(5, 0));
                ImGui.Image(SelectedImage.ImGuiHandle, new Vector2(SelectedImage.Width, SelectedImage.Height));
                return;
            }

            if (Visible && !Disabled && !Paused)
            {
                ImGui.SetCursorPos(new Vector2(5, 0));
                ImGui.Image(SelectedImage.ImGuiHandle, new Vector2(SelectedImage.Width, SelectedImage.Height));
                return;
            }
        }

        public static unsafe bool IsTargetable(PartyMember partyMember)
        {
            var playerGameObject = partyMember.GameObject;
            if (playerGameObject == null) return false;

            var playerTargetable = ((GameObject*)playerGameObject.Address)->GetIsTargetable();

            return playerTargetable;
        }

        public static unsafe bool IsTargetable(Dalamud.Game.ClientState.Objects.Types.GameObject gameObject)
        {
            var playerTargetable = ((GameObject*)gameObject.Address)->GetIsTargetable();

            return playerTargetable;
        }

        public void ChangeImageSize(ImageSize size)
        {
            switch (size)
            {
                case ImageSize.Small:
                    SelectedImage = ImageSmall;
                    break;

                case ImageSize.Medium:
                    SelectedImage = ImageMedium;
                    break;

                case ImageSize.Large:
                    SelectedImage = ImageLarge;
                    break;
            }

            SizeConstraints = new WindowSizeConstraints()
            {
                MinimumSize = new(this.SelectedImage.Width, this.SelectedImage.Height),
                MaximumSize = new(this.SelectedImage.Width, this.SelectedImage.Height)
            };
        }

        public void Dispose()
        {
            ImageSmall.Dispose();
            ImageMedium.Dispose();
            ImageLarge.Dispose();
        }
    }
}
