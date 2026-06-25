using System.Reflection;
using System.Runtime.CompilerServices;

namespace UncappedSpire.UncappedSpireCode.Util;

// TODO: Use this everywhere
public static class MethodInfoExtensions
{
    public static MethodInfo GetAsyncInnerMethodIfExists(this MethodInfo methodInfo)
    {
        var attr = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>();
        if (attr == null)
        {
            return methodInfo;
        }
        var moveNextMethod = attr.StateMachineType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
        if (moveNextMethod == null)
        {
            throw new NullReferenceException($"{methodInfo.Name} AsyncStateMachineAttribute state machine method not found");
        }
        return moveNextMethod;
    }
}