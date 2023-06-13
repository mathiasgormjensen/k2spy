using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("K2Spy")]
[assembly: AssemblyDescription("")]
#if RELEASE && AnyCPU
[assembly: AssemblyConfiguration("Release AnyCPU")]
#elif RELEASE && x86
[assembly: AssemblyConfiguration("Release 32 bit")]
#elif RELEASE && x64
[assembly: AssemblyConfiguration("Release 64 bit")]
#elif DEBUG && AnyCPU
[assembly: AssemblyConfiguration("Debug AnyCPU")]
#elif DEBUG && x86
[assembly: AssemblyConfiguration("Debug 32 bit")]
#elif DEBUG && x64
[assembly: AssemblyConfiguration("Debug 64 bit")]
#else
// TODO, consider whether we should introduce a compilation error to avoid this case?
[assembly: AssemblyConfiguration("Unexpected configuration")]
#endif



[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("K2Spy")]
[assembly: AssemblyCopyright("Copyright © 2022")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("2dd1cbec-2828-4c5a-b2be-3ff27cbf2408")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
