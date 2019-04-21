﻿namespace DSInternals.Common
{
    using DSInternals.Common.Interop;
    using DSInternals.Common.Properties;
    using System;
    using System.Security;
    using System.Security.AccessControl;

    public static class StringExtensions
    {
        private const char DBWithBinarySeparator = ':';
        private const string DBWithBinaryPrefix = "B:";
        private const string DNWithBinaryFormat = "B:{0}:{1}:{2}";

        public static string TrimEnd(this string input, string suffix)
        {
            if(! string.IsNullOrEmpty(input) && ! string.IsNullOrEmpty(suffix) && input.EndsWith(suffix))
            {
                int trimmedLength = input.Length-suffix.Length;
                return input.Remove(trimmedLength);
            }
            else
            {
                return input;
            }
        }
        public static SecureString ToSecureString(this string input)
        {
            if (input == null)
            {
                return null;
            }
            SecureString output = new SecureString();
            foreach (char c in input.ToCharArray())
            {
                output.AppendChar(c);
            }
            output.MakeReadOnly();
            return output;
        }

        public static byte[] SddlToBinary(this string securityDescriptor)
        {
            Validator.AssertNotNullOrWhiteSpace(securityDescriptor, "securityDescriptor");
            byte[] binarySecurityDescriptor;
            Win32ErrorCode result = NativeMethods.ConvertStringSecurityDescriptorToSecurityDescriptor(securityDescriptor, GenericSecurityDescriptor.Revision, out binarySecurityDescriptor);
            Validator.AssertSuccess(result);
            return binarySecurityDescriptor;
        }

        /// <summary>
        /// The DNWithBinary LDAP attribute syntax has the following format: B:<char count>:<binary value>:<object DN>
        /// </summary>
        public static byte[] FromDNWithBinary(this string input)
        {
            Validator.AssertNotNullOrEmpty(input, nameof(input));

            bool hasCorrectPrefix = input.StartsWith(DBWithBinaryPrefix);
            int valueLeadingColonIndex = input.IndexOf(DBWithBinarySeparator, DBWithBinaryPrefix.Length);
            int valueTrailingColonIndex = input.LastIndexOf(DBWithBinarySeparator);
            bool has4Parts = valueLeadingColonIndex >= 3 && (valueLeadingColonIndex + 1) < valueTrailingColonIndex;

            if(!hasCorrectPrefix || !has4Parts)
            {
                // We do not need to perform a more thorough validation.
                throw new ArgumentException(Resources.NotDNWithBinaryMessage, nameof(input));
            }

            return input.HexToBinary(valueLeadingColonIndex + 1, valueTrailingColonIndex - valueLeadingColonIndex - 1);
        }

        public static string ToDNWithBinary(this byte[] binaryData, string distinguishedName)
        {
            Validator.AssertNotNull(binaryData, nameof(binaryData));
            Validator.AssertNotNull(distinguishedName, nameof(distinguishedName));

            return String.Format(DNWithBinaryFormat, binaryData.Length * 2, binaryData.ToHex(true), distinguishedName);
        }
    }
}
