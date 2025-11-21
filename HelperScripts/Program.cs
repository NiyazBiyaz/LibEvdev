// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace HelperScripts
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("O_o");
                return;
            }

            uint type = uint.Parse(args[0]);
            uint code = uint.Parse(args[1]);

            string typeName = EvdevBindings.GetEventTypeName(type);
            string codeName = EvdevBindings.GetEventCodeName(type, code);

            Console.WriteLine($"{codeName} is member of the {typeName}: {type}, {code}.");
        }
    }
}
