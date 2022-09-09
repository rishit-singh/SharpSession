/// Author: https://github.com/rishit-singh/CryptographyHelperLib
/// This code is licensed under MIT license.

///Copyright (c) 2022 Rishit Singh

/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:

/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.

/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.

using System;
using System.Text;
using System.Security.Cryptography;

namespace SharpSession.Cryptography
{
    /// <summary>
    /// Contains string hashing functions.
    /// </summary>
    public class Hashing
    {
        protected static string ConvertBytesToString(Byte[] bytes)
        {
            int byteSize = bytes.Length;

            string hashString = null;

            for (int x = 0; x < byteSize; x++)
                hashString += bytes[x].ToString("x2");

            return hashString;
        }

        public static string GetSHA256(string str)
        {
            try
            {
                if (str == null)
                    throw new NullReferenceException();
            }
            catch (NullReferenceException)
            {
                return str;
            }

            return Hashing.ConvertBytesToString(SHA256Managed.Create().ComputeHash(Encoding.UTF8.GetBytes(str)));
        }
    }
}
