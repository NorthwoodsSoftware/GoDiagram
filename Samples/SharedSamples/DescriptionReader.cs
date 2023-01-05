/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.IO;
using System.Reflection;

namespace Demo {
  internal class DescriptionReader {
    internal static string Read(string fileName) {
      var assembly = Assembly.GetExecutingAssembly();
      var resource = $"{assembly.GetName().Name}.{fileName}";
      using var stream = assembly.GetManifestResourceStream(resource);
      using var reader = new StreamReader(stream);
      return reader.ReadToEnd();
    }
  }
}
