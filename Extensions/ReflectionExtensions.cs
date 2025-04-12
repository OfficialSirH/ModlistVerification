using System;
using System.Collections.Generic;
using System.Reflection;

namespace ModlistVerification.Extensions;

internal static class ReflectionExtensions
{
    /// <summary>
    /// A safe way to get all methods from a type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<MethodInfo> SafeGetMethods(this Type type)
    {
        try
        {
            return type.GetMethods();
        }
        catch /*(Exception ex)*/
        {
            // Logger.LogWarning($"Error retrieving methods for type {type.FullName}: {ex.Message}");
            return Array.Empty<MethodInfo>();
        }
    }

    /// <summary>
    /// A safe way to get a custom attribute from a method.
    /// </summary>
    /// <param name="method"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? SafeGetCustomAttribute<T>(this MethodInfo method) where T : Attribute
    {
        try
        {
            return method.GetCustomAttribute<T>();
        }
        catch /*(Exception ex)*/
        {
            // Logger.LogWarning($"Error retrieving methods for type {type.FullName}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// A safe way to get a custom attribute from a class.
    /// </summary>
    /// <param name="_class"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? SafeGetCustomAttribute<T>(this Type _class) where T : Attribute
    {
        try
        {
            return _class.GetCustomAttribute<T>();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// A safe way to check if a method has a custom attribute.
    /// </summary>
    /// <param name="method"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool HasCustomAttribute<T>(this MethodInfo method) where T : Attribute
    {
        try
        {
            return method.GetCustomAttribute<T>() != null;
        }
        catch /*(Exception ex)*/
        {
            // Logger.LogWarning($"Error retrieving methods for type {type.FullName}: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// A safe way to check if a class has a custom attribute.
    /// </summary>
    /// <param name="_class"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool HasCustomAttribute<T>(this Type _class) where T : Attribute
    {
        try
        {
            return _class.GetCustomAttribute<T>() != null;
        }
        catch
        {
            return false;
        }
    }
}
