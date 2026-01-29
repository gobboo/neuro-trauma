using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Runtime;
using Barotrauma;
using NeuroSDKCsharp;
using System.Runtime.CompilerServices;
using NeuroSDKCsharp.Messages.Outgoing;
using NeuroSDKCsharp.Messages.API;
using NeuroSDKCsharp.Actions;
using Microsoft.Xna.Framework;

[assembly: IgnoresAccessChecksTo("Barotrauma")]
[assembly: IgnoresAccessChecksTo("DedicatedServer")]
[assembly: IgnoresAccessChecksTo("BarotraumaCore")]

namespace AgnesControl
{
    public partial class Plugin : IAssemblyPlugin
    {
    }
}
