using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Cli.Sample;

public class SampleGui : GuiDefinition
{
    public override string Id => "sample_gui";

    protected override void Define(GuiFileScope s)
    {
        s.Window("exa_custom_window", w => w
            .Position(0, 0)
            .Size(500, 400)
            .Moveable(true)
            .Orientation("center")
            .Background("bg", "GFX_tiled_window_transparent")
            .Text("title_text", t => t
                .Text("EXA_CUSTOM_WINDOW_TITLE")
                .Position(20, 10)
                .Font("hoi_24header")
            )
            .Button("close_button", b => b
                .Sprite("GFX_closebutton")
                .Position(470, 10)
            )
            .Icon("flag_icon", i => i
                .Sprite("GFX_flag_small")
                .Position(20, 50)
            )
        );
    }
}

public class SampleMusic : MusicDefinition
{
    public override string Id => "exampleia";

    protected override void Define(MusicFileScope s)
    {
        s.Song("exa_main_theme", "music/exa_main_theme.ogg", chance: 1.0)
         .Song("exa_war_drums", "music/exa_war_drums.ogg", chance: 0.5);
    }
}

public class SampleSound : SoundDefinition
{
    public override string Id => "exampleia";

    protected override void Define(SoundFileScope s)
    {
        s.Effect("exa_click", "sound/exa_click.wav", volume: 0.8)
         .Effect("exa_notification", "sound/exa_notification.wav", volume: 0.6);
    }
}
