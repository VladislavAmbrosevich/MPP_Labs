using System;

namespace Lab6.Serialization
{
    public static class AccessModifiersExtensions
    {
        public static string GetString(this AccessModifiers accessModifier)
        {
            switch (accessModifier)
            {
                case AccessModifiers.Public:
                {
                    return AccessModifierNames.Public;
                }
                case AccessModifiers.Internal:
                {
                    return AccessModifierNames.Internal;
                }
                case AccessModifiers.Protected:
                {
                    return AccessModifierNames.Protected;
                }
                case AccessModifiers.ProtectedInternal:
                {
                    return AccessModifierNames.ProtectedInternal;
                }
                case AccessModifiers.Private:
                {
                    return AccessModifierNames.Private;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(accessModifier), accessModifier, null);
                }
            }
        }
    }
}