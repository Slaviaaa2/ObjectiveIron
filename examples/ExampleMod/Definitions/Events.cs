using ObjectiveIron.Builders;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models;
using ObjectiveIron.Core.Models.Primitives;

namespace ExampleMod;

public class IndustrialBoomEvent : EventDefinition
{
    public override string Id => "exampleia_industry.1";
    public override EventType Type => EventType.Country;
    public override LocalizedText Title => new()
    {
        En = "Industrial Boom",
        Ja = "産業ブーム"
    };
    public override LocalizedText Description => new()
    {
        En = "Our industrial investments are paying off!",
        Ja = "産業への投資が実を結んでいる！"
    };

    protected override void Options(EventOptionScope o)
    {
        o.Add("exampleia_industry.1.a", new LocalizedText
        {
            En = "Excellent news!",
            Ja = "素晴らしいニュースだ！"
        }, opt =>
        {
            opt.AddPoliticalPower(50);
            opt.AddStability(0.05);
        });
    }
}
