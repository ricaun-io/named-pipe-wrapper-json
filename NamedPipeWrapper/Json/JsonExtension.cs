﻿using System;

namespace NamedPipeWrapper.Json
{
    /// <summary>
    /// JsonExtension
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// JsonDeserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(this string value)
        {
            return JsonUtils.DeserializeObject<T>(value);
        }

        /// <summary>
        /// JsonSerialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string JsonSerialize<T>(this T value)
        {
            return JsonUtils.SerializeObject<T>(value);
        }
    }
}