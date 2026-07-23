using System;
using System.Collections.Generic;
using System.Linq;

namespace STG.Engine.Helper {
    public class EnumHelper<T> {
        public static List<T> CastToEnumList() => Enum.GetValues(typeof(T)).Cast<T>().ToList();

        public static T ToEnum(string value) {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static string? ToString(object value) {
            Type type = typeof(T);
            return Enum.GetName(typeof(T), value);
        }

    }

}
