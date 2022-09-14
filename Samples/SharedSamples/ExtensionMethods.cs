using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Demo {
  static class ExtensionMethods {
    public static void Sort<T>(this ObservableCollection<T> collection, IComparer<T> comparer) where T : IComparable {
      var sorted = collection.OrderBy(x => x, comparer).ToList();
      for (var i = 0; i < sorted.Count; i++) {
        collection.Move(collection.IndexOf(sorted[i]), i);
      }
    }
  }

  public class SmartStringComparer : IComparer<string> {
    public int Compare(string a, string b) {
      if (a != null) {
        if (b != null) {
          var aLower = a.ToLower();
          var bLower = b.ToLower();
          // split the string, separating numbers
          var pattern = @"([+\-]?[\.]?\d+(?:\.\d*)?(?:e[+\-]?\d+)?)";
          var aSplit = Regex.Split(aLower, pattern);
          var bSplit = Regex.Split(bLower, pattern);
          var aLen = aSplit.Length;
          var bLen = bSplit.Length;
          var i = 0;
          for (; i < aLen && i < bLen; i++) {
            if (aSplit[i] != "" && bSplit[i] != "") {
              var isNumA = double.TryParse(aSplit[i], out var aCurrentNum);
              var isNumB = double.TryParse(bSplit[i], out var bCurrentNum);
              if (isNumA) {
                if (isNumB) {
                  // both numbers
                  if (aCurrentNum.CompareTo(bCurrentNum) != 0) {
                    return aCurrentNum.CompareTo(bCurrentNum);
                  } else {
                    // same number, move to next element
                    continue;
                  }
                } else {
                  // aSplit[i] is a number, bSplit[i] is NaN
                  return -1;
                }
              } else {
                if (isNumB) {
                  // aSplit[i] is NaN, bSplit[i] is a number
                  return 1;
                } else {
                  // both strings
                  if (string.CompareOrdinal(aSplit[i], bSplit[i]) != 0) {
                    return string.CompareOrdinal(aSplit[i], bSplit[i]) > 0 ? 1 : -1;
                  } else {
                    // same string, move to next element
                    continue;
                  }
                }
              }
            } else {
              // aSplit[i] or bSplit[i] is ""
              if (aSplit[i] == "" && bSplit[i] == "") continue;
              else if (aSplit[i] == "" && bSplit[i] != "") return -1;
              else return 1;
            }
          }
          // lengths may be different, if they're the same, returns 0
          return aLen.CompareTo(bLen);
        } else {
          return 1;
        }
      } else {
        if (b != null) {
          return -1;
        } else {
          return 0;
        }
      }
    }
  }
}
