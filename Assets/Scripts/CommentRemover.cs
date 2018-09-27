using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SoldakModdingTool
{
    public static class CommentRemover
    {
        public static string RemoveComments(string file)
        {
            Regex comments1 = new Regex(@"//.*?\n");
            Regex comments2 = new Regex(@"/\*(.|\n)*?\*/");

            string afterRemoval = comments1.Replace(file, "");
            afterRemoval = comments2.Replace(afterRemoval, "");

            return afterRemoval;
        }

        public static ConcurrentBag<string> RemoveCommentsFromList(ConcurrentBag<string> files)
        {
            ConcurrentBag<string> list = new ConcurrentBag<string>();
            foreach (string s in files) {
                list.Add(RemoveComments(s));
            }
            return list;
        }
    }
}