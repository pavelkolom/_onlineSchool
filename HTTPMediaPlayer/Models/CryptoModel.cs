using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace HTTPMediaPlayer.Models
{
  public static class CryptoModel
  {
    public static byte[] GetMD5Hash(string inputString)
    {
      if (inputString == null) inputString = string.Empty;
      HashAlgorithm algorithm = MD5.Create();
      return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static byte[] GetSHA256Hash(string inputString)
    {
      HashAlgorithm algorithm = SHA256.Create();
      return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static string GetMD5HashString(string inputString)
    {
      StringBuilder sb = new StringBuilder();
      foreach (byte b in GetMD5Hash(inputString))
        sb.Append(b.ToString("X2"));
      return sb.ToString();
    }

    public static string GetSHA256HashString(string inputString)
    {
      StringBuilder sb = new StringBuilder();
      foreach (byte b in GetSHA256Hash(inputString))
        sb.Append(b.ToString("X2"));
      return sb.ToString();
    }

    public static string CreatePassword(int length)
    {
      const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
      StringBuilder res = new StringBuilder();
      Random rnd = new Random();
      while (0 < length--)
      {
        res.Append(valid[rnd.Next(valid.Length)]);
      }
      return res.ToString();
    }
  }
}