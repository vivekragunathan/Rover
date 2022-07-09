using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace ThoughtWorks
{
   class Program
   {
      private const string InputFile = @"Input.txt";

      public static void Main(string[] args)
      {
         try
         {
            FileStream fs = new FileStream(Program.InputFile, FileMode.Open, FileAccess.Read);
            ExternalInput.ExecuteFile(fs);
         }
         catch (Exception ex)
         {
            Console.WriteLine("Error! {0}", ex.Message);
         }
      }
   }
}