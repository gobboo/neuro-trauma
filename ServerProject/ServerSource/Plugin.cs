using Barotrauma;
using System.Runtime.CompilerServices;

[assembly: IgnoresAccessChecksTo("Barotrauma")]
[assembly: IgnoresAccessChecksTo("DedicatedServer")]
[assembly: IgnoresAccessChecksTo("BarotraumaCore")]

namespace NeuroSauma;

/// <summary>
/// Server plugin.
/// </summary>
public partial class Plugin : IAssemblyPlugin
{
}
