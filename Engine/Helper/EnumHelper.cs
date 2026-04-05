using System;
using System.Collections.Generic;
using System.Linq;
//using RockmanEXEX.Game;

namespace STG.Engine.Helper {
    public class EnumHelper<T> {
        public static List<T> CastToEnumList() => Enum.GetValues(typeof(T)).Cast<T>().ToList();

        public static T ToEnum(string value) {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static string ToString(object value) {
            //Type type = typeof(T);

            //if (type == typeof(ChipData.ChipCode)) {
            //    string name = Enum.GetName(type, value);
            //    if (name == Enum.GetName(type, ChipData.ChipCode.Asterisk)) {
            //        return "*";
            //    } else {
            //        return name;
            //    }
            //}
            //return Enum.GetName(typeof(T), value);
            throw new NotImplementedException();
        }

    }

}
