using ObjectiveIron.Core.Models.Primitives;

namespace ObjectiveIron.Emitter;

/// <summary>
/// Low-level writer for Clausewitz script format.
/// Handles indentation, key = value output, block nesting, and boolean (yes/no) conversion.
/// 
/// Output example:
/// <code>
/// focus_tree = {
///     id = my_tree
///     country = {
///         factor = 0
///         modifier = {
///             add = 10
///             tag = GER
///         }
///     }
/// }
/// </code>
/// </summary>
public sealed class ClausewitzWriter : IDisposable
{
    private readonly TextWriter _writer;
    private readonly WriterOptions _options;
    private int _indentLevel;
    private bool _isLineStart = true;

    public ClausewitzWriter(TextWriter writer, WriterOptions? options = null)
    {
        _writer = writer;
        _options = options ?? new WriterOptions();
    }

    /// <summary>Write a key = string_value property.</summary>
    public void WriteProperty(string key, string value)
    {
        WriteIndent();
        _writer.Write(key);
        _writer.Write(" = ");
        _writer.WriteLine(value);
        _isLineStart = true;
    }

    /// <summary>Write a key = int_value property.</summary>
    public void WriteProperty(string key, int value)
    {
        WriteIndent();
        _writer.Write(key);
        _writer.Write(" = ");
        _writer.WriteLine(value);
        _isLineStart = true;
    }

    /// <summary>Write a key = float_value property.</summary>
    public void WriteProperty(string key, double value)
    {
        WriteIndent();
        _writer.Write(key);
        _writer.Write(" = ");
        _writer.WriteLine(value.ToString("0.####"));
        _isLineStart = true;
    }

    /// <summary>Write a key = yes/no property.</summary>
    public void WriteProperty(string key, bool value)
    {
        WriteProperty(key, value ? "yes" : "no");
    }

    /// <summary>Write a key op value property with a specific operator.</summary>
    public void WriteProperty(string key, string value, Operator op)
    {
        WriteIndent();
        _writer.Write(key);
        _writer.Write($" {op.ToClausewitz()} ");
        _writer.WriteLine(value);
        _isLineStart = true;
    }

    /// <summary>Write a Property model entry.</summary>
    public void WriteProperty(Property property)
    {
        var value = property.Value;
        var key = property.Key;
        var op = property.Operator;

        switch (value)
        {
            case PropertyValue.StringValue:
            case PropertyValue.QuotedStringValue:
                WriteProperty(key, value.ToClausewitz(), op);
                break;
            case PropertyValue.IntValue i:
                WriteIndent();
                _writer.Write(key);
                _writer.Write($" {op.ToClausewitz()} ");
                _writer.WriteLine(i.Value);
                _isLineStart = true;
                break;
            case PropertyValue.FloatValue f:
                WriteIndent();
                _writer.Write(key);
                _writer.Write($" {op.ToClausewitz()} ");
                _writer.WriteLine(f.Value.ToString("0.####"));
                _isLineStart = true;
                break;
            case PropertyValue.BoolValue b:
                WriteProperty(key, b.Value ? "yes" : "no", op);
                break;
        }
    }

    /// <summary>Begin a named block: key = {</summary>
    public void BeginBlock(string key)
    {
        WriteIndent();
        _writer.Write(key);
        _writer.WriteLine(" = {");
        _isLineStart = true;
        _indentLevel++;
    }

    /// <summary>End the current block: }</summary>
    public void EndBlock()
    {
        _indentLevel = Math.Max(0, _indentLevel - 1);
        WriteIndent();
        _writer.WriteLine("}");
        _isLineStart = true;
    }

    /// <summary>Write a comment line: # text</summary>
    public void WriteComment(string comment)
    {
        WriteIndent();
        _writer.Write("# ");
        _writer.WriteLine(comment);
        _isLineStart = true;
    }

    /// <summary>Write a bare identifier on its own line (no key=value, no quotes).</summary>
    public void WriteUnquoted(string value)
    {
        WriteIndent();
        _writer.WriteLine(value);
        _isLineStart = true;
    }

    /// <summary>Write an empty line for readability.</summary>
    public void WriteBlankLine()
    {
        _writer.WriteLine();
        _isLineStart = true;
    }

    private void WriteIndent()
    {
        if (!_isLineStart) return;
        for (int i = 0; i < _indentLevel; i++)
            _writer.Write(_options.IndentString);
        _isLineStart = false;
    }

    public void Dispose()
    {
        if (_options.DisposeWriter)
            _writer.Dispose();
    }
}

/// <summary>Configuration options for ClausewitzWriter.</summary>
public sealed class WriterOptions
{
    /// <summary>String used for each indentation level. Default: tab.</summary>
    public string IndentString { get; init; } = "\t";

    /// <summary>Whether to dispose the underlying TextWriter. Default: true.</summary>
    public bool DisposeWriter { get; init; } = true;

    /// <summary>Whether to add a generation header comment. Default: true.</summary>
    public bool AddGenerationHeader { get; init; } = true;
}
