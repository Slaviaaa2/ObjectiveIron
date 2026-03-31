using ObjectiveIron.Builders.Definitions;

namespace ObjectiveIron.Emitter.Emitters;

public static class BookmarkEmitter
{
    public static void Emit(BookmarkBuildResult bookmark, ClausewitzWriter writer)
    {
        writer.BeginBlock("bookmarks");
        writer.BeginBlock("bookmark");

        writer.WriteProperty("name", $"\"{bookmark.Name}\"");
        writer.WriteProperty("desc", $"\"{bookmark.Desc}\"");
        writer.WriteProperty("date", $"\"{bookmark.Date}\"");
        if (bookmark.Picture != null) writer.WriteProperty("picture", $"\"{bookmark.Picture}\"");
        if (bookmark.DefaultCountry != null) writer.WriteProperty("default_country", $"\"{bookmark.DefaultCountry}\"");
        writer.WriteBlankLine();

        foreach (var country in bookmark.Countries)
        {
            writer.BeginBlock(country.Tag);

            if (country.History != null) writer.WriteProperty("history", $"\"{country.History}\"");
            if (country.Ideology != null) writer.WriteProperty("ideology", country.Ideology);

            if (country.Ideas is { Count: > 0 })
            {
                writer.BeginBlock("ideas");
                foreach (var idea in country.Ideas)
                    writer.WriteUnquoted(idea);
                writer.EndBlock();
            }

            if (country.Focuses is { Count: > 0 })
            {
                writer.BeginBlock("focuses");
                foreach (var focus in country.Focuses)
                    writer.WriteUnquoted(focus);
                writer.EndBlock();
            }

            writer.EndBlock(); // country tag
        }

        writer.EndBlock(); // bookmark
        writer.EndBlock(); // bookmarks
    }
}
