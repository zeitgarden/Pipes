using System;

namespace Pipes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SkipForAutoScanningAttribute : Attribute
    {
        
    }
}