// Guids.cs
// MUST match guids.h
using System;

namespace DST.UIGenerator
{
    static class GuidList
    {
        public const string guidUIGeneratorPkgString = "18A17763-3DE0-41B8-9507-4D49C2725F1C";
        public const string guidUIGeneratorCmdSetString = "718b4319-3834-4a64-a50d-6a2aa51443ab";
        public const string guidToolWindowPersistanceString = "C28C232B-1B38-4647-B7F0-D948C4CD0588";

        public static readonly Guid guidUIGeneratorCmdSet = new Guid(guidUIGeneratorCmdSetString);
    };
}