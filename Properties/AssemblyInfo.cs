#region Using directives

using System;
using System.Reflection;
using System.Runtime.InteropServices;

#endregion

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("RoverLib")]
[assembly: AssemblyDescription("Mars Rover Controller Library")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("ThoughtWorks Inc.")]
[assembly: AssemblyProduct("Rover")]
[assembly: AssemblyCopyright("Copyright 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// This sets the default COM visibility of types in the assembly to invisible.
// If you need to expose a type to COM, use [ComVisible(true)] on that type.
[assembly: ComVisible(false)]

// The assembly version has following format :
//
// Major.Minor.Build.Revision
//
// You can specify all the values or you can use the default the Revision and 
// Build Numbers by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.*")]

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("RoverTest")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("UnitTests")]
