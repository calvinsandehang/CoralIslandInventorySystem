public interface IButtonVfx
{
    bool SelectedVfxActive { get; }
    bool HoveredVfxActive { get; }

    void ToggleSelectedVfx();
    void ToggleHoveredVfx();
}