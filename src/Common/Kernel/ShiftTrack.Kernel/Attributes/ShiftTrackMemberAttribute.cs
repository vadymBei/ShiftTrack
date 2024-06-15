using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTrack.Kernel.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    /// <summary>
    /// Registers the current assembly for use in the kernel libraries
    /// </summary>
    public class ShiftTrackMemberAttribute : Attribute
    {
    }
}
