using System;
using System.Collections.Generic;
using System.Reflection;

namespace WebApp {
    public static class Extensions {
        public static Target MapTo<Target, Source>(this Source source, Dictionary<String, String> maps)
            where Target : class {
            Type sType = source.GetType();
            Type tType = typeof(Target);
            var t = (Target) Activator.CreateInstance(tType);

            Dictionary<String, String>.KeyCollection keys = maps.Keys;
            foreach (String sMemberName in keys) {
                String tMemberName = maps[sMemberName];
                PropertyInfo sProp = sType.GetProperty(sMemberName);
                FieldInfo sField = sType.GetField(sMemberName);
                if (sProp == null || sField == null)
                    throw new MissingMemberException(sType.Name, sMemberName);

                PropertyInfo tProp = tType.GetProperty(tMemberName);
                FieldInfo tField = tType.GetField(tMemberName);
                if (tProp == null || tField == null)
                    throw new MissingMemberException(tType.Name, tMemberName);

                if (tProp != null) {
                    Type tPropType = tProp.PropertyType;
                    tProp.SetValue(t, Convert.ChangeType(sProp.GetValue(sType, null), tPropType), null);
                    continue;
                }

                Type tFieldType = tField.FieldType;
                tField.SetValue(t, Convert.ChangeType(sField.GetValue(sType), tFieldType));
            }

            return t;
        }
    }
}