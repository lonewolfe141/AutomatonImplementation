// Written and Developed by Ritvik Tiwari. Copyright© 2024 Ritvik Tiwari. All rights reserved.
using System;
using System.Collections.Generic;

namespace AutomatonSimulator
{
    public class DFA
    {
        private string currentState;
        private readonly HashSet<string> finalStates;
        private readonly Dictionary<(string, char), string> transitions;

        public DFA()
        {
            currentState = "q0";
            finalStates = new HashSet<string> { "q5" };
            transitions = new Dictionary<(string, char), string>
            {
                { ("q0", 'a'), "q1" },
                { ("q1", 'a'), "q2" },
                { ("q2", 'a'), "q3" },
                { ("q3", 'a'), "q4" },
                { ("q4", 'b'), "q5" },
                { ("q5", 'b'), "q4" },
                { ("q0", 'b'), "qReject" },
                { ("q1", 'b'), "qReject" },
                { ("q2", 'b'), "qReject" },
                { ("q3", 'b'), "qReject" },
                { ("q4", 'a'), "qReject" },
                { ("q5", 'a'), "qReject" }
            };
        }

        public bool Accepts(string input)
        {
            currentState = "q0";
            foreach (char symbol in input)
            {
                if (!transitions.ContainsKey((currentState, symbol)))
                {
                    currentState = "qReject";
                    break;
                }
                currentState = transitions[(currentState, symbol)];
            }
            return finalStates.Contains(currentState);
        }
    }

    public class NFA
    {
        private readonly Dictionary<string, Dictionary<char, List<string>>> transitions;
        private readonly HashSet<string> finalStates;

        public NFA()
        {
            transitions = new Dictionary<string, Dictionary<char, List<string>>>
            {
                { "s0", new Dictionary<char, List<string>> { { 'a', new List<string> { "s1", "s5" } }, { 'b', new List<string> { "s5" } } } },
                { "s1", new Dictionary<char, List<string>> { { 'b', new List<string> { "s2" } } } },
                { "s2", new Dictionary<char, List<string>> { { 'a', new List<string> { "s3" } }, { 'b', new List<string> { "s3" } } } },
                { "s3", new Dictionary<char, List<string>> { { 'a', new List<string> { "s4" } }, { 'b', new List<string> { "s4" } } } },
                { "s4", new Dictionary<char, List<string>> { { 'a', new List<string> { "s4" } }, { 'b', new List<string> { "s4" } } } },
                { "s5", new Dictionary<char, List<string>> { { 'a', new List<string> { "s6" } }, { 'b', new List<string> { "s6" } } } },
                { "s6", new Dictionary<char, List<string>> { { 'a', new List<string> { "s6", "s7" } }, { 'b', new List<string> { "s6" } } } },
                { "s7", new Dictionary<char, List<string>> { { 'b', new List<string> { "s9" } } } },
                { "s9", new Dictionary<char, List<string>>() }
            };
            finalStates = new HashSet<string> { "s4", "s9" };
        }

        public bool Accepts(string input)
        {
            return AcceptsRecursive("s0", input, 0);
        }

        public bool AcceptsRecursive(string currentState, string input, int position)
        {
            if (position == input.Length)
            { return finalStates.Contains(currentState); }

            char currentSymbol = input[position];

            if (transitions.ContainsKey(currentState) && transitions[currentState].ContainsKey(currentSymbol))
            {
                foreach (string nextState in transitions[currentState][currentSymbol])
                {
                    if (AcceptsRecursive(nextState, input, position + 1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    public class PDA
    {
        private readonly Stack<char> stack;
        private string currentState;

        public PDA()
        {
            stack = new Stack<char>();
            currentState = "q0";
        }

        public bool Accepts(string input)
        {
            stack.Clear();
            currentState ="q0";

            foreach (char symbol in input)
            {
                switch (currentState)
                {
                    case "q0":
                    if (symbol == 'a')
                    {   stack.Push('Z');
                        currentState = "q1"; 
                    }
                    else
                    { return false; }
                    break;

                    case "q1":
                    if (symbol == 'a')
                    { stack.Push('B'); }
                    else if (symbol == 'b' && stack.Count > 0 && stack.Peek() == 'B')
                    {
                        stack.Pop();
                        currentState = "q2";
                    }
                    else
                    { return false; }
                    break;

                    case "q2":
                    if (symbol == 'b' && stack.Peek() == 'B' && stack.Count > 0)
                    { stack.Pop(); }
                    else if (stack.Count == 1 && stack.Peek() == 'Z')
                    {
                        currentState = "q3";
                    }
                    else
                    { return false; }
                    break;

                    default:
                    return false;
                }  //end switch
            }
            return currentState == "q3" && stack.Peek() == 'Z' && stack.Count == 1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n======================== Automaton Simulator =================================");
                Console.WriteLine("\n=============== Written and Developed by: Ritvik Tiwari ======================");
                Console.WriteLine("\n=========Copyright © 2024 Ritvik Tiwari. All rights reserved==================");
                Console.WriteLine("\n1. Test DFA (Four A's followed by odd B's)");
                Console.WriteLine("\n2. Test NFA (Strings either ending with 'ab' or starting with 'ab' and have at least 4 characters in it)");
                Console.WriteLine("\n3. Test PDA (L = aa^nb^n, n >= 1)");
                Console.WriteLine("\n4. Exit");
                Console.Write("\nSelect an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        TestDFA();
                        break;
                    case "2":
                        TestNFA();
                        break;
                    case "3":
                        TestPDA();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void TestDFA()
        {
            var dfa = new DFA();
            Console.Write("Enter a string for the DFA: ");
            string input = Console.ReadLine();
            Console.WriteLine(dfa.Accepts(input) ? "Accepted" : "Rejected");
        }

        static void TestNFA()
        {
            var nfa = new NFA();
            Console.Write("Enter a string for the NFA: ");
            string input = Console.ReadLine();
            Console.WriteLine(nfa.Accepts(input) ? "Accepted" : "Rejected");
        }

        static void TestPDA()
        {
            var pda = new PDA();
            Console.Write("Enter a string for the PDA: ");
            string input = Console.ReadLine();
            Console.WriteLine(pda.Accepts(input) ? "Accepted" : "Rejected");
        }
    }
}