using System.Reflection;
using ObjectiveIron.Builders.Definitions;
using ObjectiveIron.Core.Models;

namespace ObjectiveIron.Builders.Compilers;

public static class TechnologyCompiler
{
    public static IEnumerable<Technology> Compile(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(TechnologyDefinition)))
            .Select(t => ((TechnologyDefinition)Activator.CreateInstance(t)!).Build());
    }
}
