using System;
using System.Text;
using System.Security.Cryptography;
using SharpSession.Tools;

namespace SharpSession.Cryptography
{
	public enum ByteEncoding
	{
		UTF8,
		UTF7
	}

	public delegate string HashFunction(string val, ByteEncoding encoding); // Stores a hash function.
	
	/// <summary>
	/// Contains string hashing functions. 
	/// </summary>
	public class Hashing
	{
		public enum HashFunctionType
		{
			Sha256
		}
			
		protected static HashFunction[] HashFunctions = new HashFunction[] {
			Hashing.GetSHA256
		};

		protected static Encoding[] Encoders = new Encoding[] {
			Encoding.UTF8,
			Encoding.UTF7
		}; 

		/// <summary>
		/// Hashes the provided string with SHA256.
		/// </summary>
		/// <param name="str">String to be hashed.</param>
		/// <param name="encoding"> Encoding to be used. </param>
		/// <returns>Hashed string. </returns>
		public static string GetSHA256(string str, ByteEncoding encoding)
		{
			Encoding encoder = null;

			try
			{
				if (str == null)
					throw new NullReferenceException();
				
				if (!GeneralTools.InRange((int)encoding, 0, Hashing.Encoders.Length))
					throw new Exception("Invalid encoding");
							
				encoder = Hashing.Encoders[(int)encoding];
			}
			catch (Exception e)
			{	
				#if DEBUG
					Console.WriteLine(e.Message);
				#endif

				return str;
			}

			return GeneralTools.ConvertBytesToString(SHA256Managed.Create().ComputeHash(encoder.GetBytes(str)));
		}
		

		/// <summary>
		/// Hashes= the provided string.
		/// </summary>
		/// <param name="str"> String to be hashed. </param>
		/// <param name="encoding">Encoding type to be used.</param>
		/// <param name="functionType">Hash function to be used.</param>
		/// <returns> Hashed string. </returns>
		public static string GetHash(string str, ByteEncoding encoding = ByteEncoding.UTF8, Hashing.HashFunctionType functionType = Hashing.HashFunctionType.Sha256)
		{
			int type;

			string hashedString = null;

			try
			{
				if (!GeneralTools.InRange(type = (int)functionType, 0, Hashing.HashFunctions.Length))
					throw new Exception("Invalid hash function");

				hashedString = Hashing.HashFunctions[type](str, encoding);
			}
			catch (Exception e)
			{
				#if DEBUG
					Console.WriteLine(e.Message);
				#endif
			}

			return hashedString;
		}

		/// <summary>
		/// Hashes the provided string.
		/// </summary>
		/// <param name="str"> String to be hashed. </param>
		/// <param name="encoding">Encoding type to be used.</param>
		/// <returns> Hashed string. </returns>
		public static string GetHash(string str, ByteEncoding encoding = ByteEncoding.UTF8)
		{
			int type;

			string hashedString = null;

			try
			{
				if (!GeneralTools.InRange(type = (int)HashFunctionType.Sha256, 0, Hashing.HashFunctions.Length))
					throw new Exception("Invalid hash function");

				hashedString = Hashing.HashFunctions[type](str, encoding);
			}
			catch (Exception e)
			{
				#if DEBUG
					Console.WriteLine(e.Message);
				#endif
			}

			return hashedString;
		}

		/// <summary>
		/// Hashes the provided string.
		/// </summary>
		/// <param name="str"> String to be hashed. </param>
		/// <param name="functionType">Hash function to be used.</param>
		/// <returns> Hashed string. </returns>
		public static string GetHash(string str, Hashing.HashFunctionType functionType = Hashing.HashFunctionType.Sha256)
		{
			int type;

			string hashedString = null;
			
			try
			{
				if (!GeneralTools.InRange(type = (int)functionType, 0, Hashing.HashFunctions.Length))
					throw new Exception("Invalid hash function");
					
				hashedString = Hashing.HashFunctions[type](str, ByteEncoding.UTF8);
			}
			catch (Exception e)
			{
				#if DEBUG
					Console.WriteLine(e.Message);
				#endif
			}
 
			return hashedString;
		}
	}
} 
