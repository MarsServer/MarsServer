﻿using System;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using System.IO;

public class CSharpEncrypt
{

	public static string keyStory = "qwulkaawppeowipoaskdaouisdofiusoilewrkj";

	/// <summary>
	/// 构造方法
	/// </summary>
	public CSharpEncrypt()
	{
		
	}

	public static string Encrypt(string original)
	{
		return Encrypt(original, keyStory);
	}
	/// <summary>
	/// 使用缺省密钥解密
	/// </summary>
	/// <param name="original">密文</param>
	/// <returns>明文</returns>
	public static string Decrypt(string original)
	{
		return Decrypt(original, keyStory, System.Text.Encoding.UTF8);
	}
	/// <summary>
	/// 使用给定密钥解密
	/// </summary>
	/// <param name="original">密文</param>
	/// <param name="key">密钥</param>
	/// <returns>明文</returns>
	public static string Decrypt(string original, string key)
	{
		return Decrypt(original, key, System.Text.Encoding.Default);
	}
	/// <summary>
	/// 使用缺省密钥解密,返回指定编码方式明文
	/// </summary>
	/// <param name="original">密文</param>
	/// <param name="encoding">编码方式</param>
	/// <returns>明文</returns>
	public static string Decrypt(string original, Encoding encoding)
	{
		return Decrypt(original, keyStory, encoding);
	}
	/// <summary>
	/// 使用给定密钥加密
	/// </summary>
	/// <param name="original">原始文字</param>
	/// <param name="key">密钥</param>
	/// <param name="encoding">字符编码方案</param>
	/// <returns>密文</returns>
	public static string Encrypt(string original, string key)
	{
		byte[] buff = System.Text.Encoding.Default.GetBytes(original);
		byte[] kb = System.Text.Encoding.Default.GetBytes(key);
		return Convert.ToBase64String(Encrypt(buff, kb));
	}
	/// <summary>
	/// 使用给定密钥解密
	/// </summary>
	/// <param name="encrypted">密文</param>
	/// <param name="key">密钥</param>
	/// <param name="encoding">字符编码方案</param>
	/// <returns>明文</returns>
	public static string Decrypt(string encrypted, string key, Encoding encoding)
	{
		byte[] buff = Convert.FromBase64String(encrypted);
		byte[] kb = System.Text.Encoding.Default.GetBytes(key);
		return encoding.GetString(Decrypt(buff, kb));
	}
	/// <summary>
	/// 生成MD5摘要
	/// </summary>
	/// <param name="original">数据源</param>
	/// <returns>摘要</returns>
	public static byte[] MakeMD5(byte[] original)
	{
		MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
		byte[] keyhash = hashmd5.ComputeHash(original);
		hashmd5 = null;
		return keyhash;
	}
	/// <summary>
	/// 使用给定密钥加密
	/// </summary>
	/// <param name="original">明文</param>
	/// <param name="key">密钥</param>
	/// <returns>密文</returns>
	public static byte[] Encrypt(byte[] original, byte[] key)
	{
		TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
		des.Key = MakeMD5(key);
		des.Mode = CipherMode.ECB;
		return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
	}
	/// <summary>
	/// 使用给定密钥解密数据
	/// </summary>
	/// <param name="encrypted">密文</param>
	/// <param name="key">密钥</param>
	/// <returns>明文</returns>
	public static byte[] Decrypt(byte[] encrypted, byte[] key)
	{
		TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
		des.Key = MakeMD5(key);
		des.Mode = CipherMode.ECB;
		return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
	}
	/// <summary>
	/// 使用给定密钥加密
	/// </summary>
	/// <param name="original">原始数据</param>
	/// <param name="key">密钥</param>
	/// <returns>密文</returns>
	public static byte[] Encrypt(byte[] original)
	{
		byte[] key = System.Text.Encoding.Default.GetBytes(keyStory);
		return Encrypt(original, key);
	}
	/// <summary>
	/// 使用缺省密钥解密数据
	/// </summary>
	/// <param name="encrypted">密文</param>
	/// <param name="key">密钥</param>
	/// <returns>明文</returns>
	public static byte[] Decrypt(byte[] encrypted)
	{
		byte[] key = System.Text.Encoding.Default.GetBytes(keyStory);
		return Decrypt(encrypted, key);
	}
}
