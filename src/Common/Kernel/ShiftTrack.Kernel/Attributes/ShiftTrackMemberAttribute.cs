namespace ShiftTrack.Kernel.Attributes;

/// <summary>
/// Registers the current assembly for use in the kernel libraries
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ShiftTrackMemberAttribute : Attribute
{
}