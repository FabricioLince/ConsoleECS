using ConsoleECS.Core;
using ConsoleECS.Core.Components;
using ConsoleECS.Core.Components.GUI;
using ConsoleECS.Core.Components.Scripts;
using ConsoleECS.Core.Input;
using ConsoleECS.Core.Systems;
using ConsoleECS.Core.Vector;
using ConsoleECS.Examples.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleECS
{
    class Program
    {
        Engine engine = new Engine();

        public void Run()
        {
            Console.WindowHeight = 30;
            Console.WindowWidth = 40;
            engine.Screen.UpdateBufferAllocation();

            var hero = engine.CreateEntity();
            {
                hero.AddComponent<Position>();
                var renderer = hero.AddComponent<Renderer>();
                renderer.symbol = 'A';
                renderer.foregroundColor = ConsoleColor.Blue;
                hero.AddComponent<Collider>();
                hero.AddComponent<Player>();
            }

            hero = engine.CreateEntity();
            {
                hero.AddComponent<Position>().Vector2Int = new Vector2Int(2, 0);
                var renderer = hero.AddComponent<Renderer>();
                renderer.symbol = 'B';
                renderer.foregroundColor = ConsoleColor.Cyan;
                hero.AddComponent<Collider>();
                var player = hero.AddComponent<Player>();
                player.upKey = KeyCode.I;
                player.downKey = KeyCode.K;
                player.leftKey = KeyCode.J;
                player.rightKey = KeyCode.L;
            }

            var npc = engine.CreateEntity();
            {
                npc.AddComponent<Position>().Vector2 = new Vector2(10, 0);
                npc.AddComponent<Collider>();
                npc.AddComponent<Renderer>().symbol = 'n';
                npc.AddComponent<Npc>();
            }

            while (true)
            {
                engine.RunSystems();
            }
        }

        Entity CreateEnvPiece(Vector2Int position, Vector2Int size, ConsoleColor bgColor, ConsoleColor fgColor = ConsoleColor.White, char symbol = ' ')
        {
            var piece = engine.CreateEntity();
            piece.AddComponent<Position>().Vector2Int = position;

            var env = piece.AddComponent<EnvironmentPiece>();
            env.backgroundColor = bgColor;
            env.foregroundColor = fgColor;
            env.symbol = symbol;
            env.size = size;

            return piece;
        }

        public static Entity CreateMessageBox(Engine engine, string message)
        {
            var guiEntity = engine.CreateEntity();
            guiEntity.Name = "guiEntity";
            var frame = guiEntity.AddComponent<Frame>();
            frame.position = new Vector2Int(1, engine.Screen.WindowHeight - 6);
            frame.Size = new Vector2Int(engine.Screen.WindowWidth - 3, 4);
            frame.foregroundColor = ConsoleColor.DarkMagenta;
            frame.backgroundColor = ConsoleColor.Gray;
            frame.line = Frame.LineKind.Single;

            var text = guiEntity.AddComponent<Text>();
            text.position = new Vector2Int(2, engine.Screen.WindowHeight - 6 + 1);
            text.text = message;
            text.foregroundColor = ConsoleColor.DarkRed;
            text.backgroundColor = frame.backgroundColor;
            /*
            text = guiEntity.AddComponent<Text>();
            text.position = frame.position + frame.Size - new Vector2Int(1, 1);
            text.text = "X";
            text.foregroundColor = ConsoleColor.Black;
            text.backgroundColor = frame.backgroundColor;
            
            var scr = guiEntity.AddComponent<DelegateScript>();
            scr.LoopFunction = (script) =>
            {
                if (Keyboard.IsKeyDown(KeyCode.X))
                {
                    while (Keyboard.IsKeyDown(KeyCode.X)) Thread.Sleep(1);
                    engine.DestroyEntity(guiEntity);
                }
                if (((int)(script.Engine.Time * 2)) % 2 == 0)
                {
                    text.text = "";
                }
                else
                {
                    text.text = "X";
                }
            };
            */
            return guiEntity;
        }

        static void Main(string[] args)
        {
            //new Program().Run();
            //new Interpreter().Run();

            var testCases = new string[]
            {
                "-(3*2)",
                "2(5+1)",
                "-2(5+1)",
                "(4*(2+7))+(2*3)",
                "(4*(2+5+2))+(2*3)",
                "(4*(-2-3-2-2))-(2*3)",
                "2a+3a*(a*-a)-a",
            };
            foreach (var testCase in testCases)
            {
                Console.WriteLine("Testing " + testCase);
                ExpressionInterpreter.Expand(testCase, out string result);
                Console.WriteLine("expanded: " + result);
                Console.WriteLine(ExpressionInterpreter.Run(testCase));
            }
            Dictionary<string, float> memory = new Dictionary<string, float>();
            ExpressionInterpreter.EvaluateIdentifierFunction = (name) =>
            {
                Console.WriteLine("Evaluating " + name);
                if (memory.ContainsKey(name)) return memory[name];
                return null;
            };

            while (true)
            {
                string line = Console.ReadLine();
                if (line == "exit") break;
                else
                {
                    if (AttribuitionInterpreter.Match(line, out string var, out float value))
                    {
                        Console.WriteLine(var + " = " + value);
                        memory[var] = value;
                    }
                    else if (ExpressionInterpreter.Run(line, out float result))
                    {
                        Console.WriteLine("= " + result);
                    }
                    else
                    {
                        Console.WriteLine("Couldn't parse command");
                    }
                }
            }
        }
    }

    public class ExpressionInterpreter
    {
        const string Name = @"\s*[a-z]+(?:[a-z]|[0-9])*\s*";
        const string Identifier = @"(?<id>" + Name + @")";
        const string Parameter = Name + "|" + Number;
        const string Function = Identifier + @"\((" + Parameter + @")*\)\s*";
        const string Number = @"\s*(?:\-|\+)?[0-9]+(?:\,[0-9]+)?\s*";
        const string Value = @"\s*(?<val>" + Number + @")\s*";
        const string LeftHand = @"\s*(?<lh>" + Number + "|" + Name + "|" + Function + @")\s*";
        const string RightHand = @"\s*(?<rh>" + Number + "|" + Name + @")\s*";

        public static Func<string, float?> EvaluateIdentifierFunction;

        enum Parenteses
        {
            None = 0,
            Left = 1,
            Right = 2,
            Both = 3
        }
        public static float Run(string input)
        {
            if (MatchExpression(input, out string r))
            {
                if (float.TryParse(r, out float f))
                {
                    return f;
                }
            }
            return 0;
        }
        public static bool Run(string input, out float result)
        {
            result = 0;
            return
                MatchExpression(input, out string resultString) &&
                float.TryParse(resultString, out result);
        }

        public static bool MatchExpression(string line, out string result)
        {
            var done = false;
            result = line;

            while (result.Contains("("))
            {
                if (result.Count("(") != result.Count(")")) return false;

                done |= MatchOperation(result, out result, @"\*|/", Parenteses.Both);
                //Console.WriteLine(result);
                done |= MatchOperation(result, out result, @"\+|\-", Parenteses.Both);
                //Console.WriteLine(result);
                done |= MatchSingleValue(result, out result);
                //Console.WriteLine(result);
            }
            //Console.WriteLine(">"+result);
            done |= MatchOperation(result, out result, @"\*|/");
            //Console.WriteLine(result);
            done |= MatchOperation(result, out result, @"\+|\-");
            //Console.WriteLine(result);
            done |= MatchSingleValue(result, out result);
            //Console.WriteLine(result);
            return done;
        }

        static bool MatchOperation(string input, out string result, string op, Parenteses parenteses = Parenteses.None)
        {
            Expand(input, out result);

            //Console.WriteLine("Matching " + op);
            string opPattern = @"(?<op>" + op + @")";
            string pattern = LeftHand + opPattern + RightHand;
            if ((parenteses & Parenteses.Left) == Parenteses.Left)
            {
                pattern = @"\s*\(" + pattern;
            }
            if ((parenteses & Parenteses.Right) == Parenteses.Right)
            {
                pattern = pattern + @"\)\s*";
            }
            Regex regex = new Regex(pattern);
            //Console.WriteLine(pattern);

            var match = regex.Match(result);

            if (!match.Success)
            {
                // if we're looking for both parenteses, try and find just the left one
                if (parenteses == Parenteses.Both)
                    return MatchOperation(result, out result, op, Parenteses.Left);
                return false;
            }

            while (match.Success)
            {
                //Console.WriteLine(":" + PrintMatch(match));
                if (parenteses == Parenteses.Left)
                    result = regex.Replace(result, m => "(" + ExpressionEvaluator(m));
                else
                    result = regex.Replace(result, ExpressionEvaluator);

                Console.WriteLine(result); // Print each resolution step

                match = regex.Match(result);
            }
            return true;
        }

        static bool MatchSingleValue(string input, out string result)
        {
            Expand(input, out result);

            Regex regex = new Regex(@"^" + LeftHand);
            var match = regex.Match(result);

            if (match.Success)
            {
                result = regex.Replace(result, m => EvaluateSingle(m.Groups["lh"].Value).ToString());
            }

            regex = new Regex(@"\s*\(" + LeftHand + @"\)\s*");
            match = regex.Match(result);
            while (match.Success)
            {
                result = regex.Replace(result, m => EvaluateSingle(m.Groups["lh"].Value).ToString());

                match = regex.Match(result);
            }
        
            return true;
        }

        public static void Expand(string input, out string result)
        {
            result = input;

            Regex regex = new Regex(@"\s*\-\s*\(");
            var match = regex.Match(result);

            if (match.Success)
            {
                result = regex.Replace(result, "-1*(");
                //return;
            }

            regex = new Regex(Value + @"\(");
            match = regex.Match(result);
            if (match.Success)
            {
                result = regex.Replace(result, m => m.Groups["val"].Value + "*(");
                //Console.WriteLine("Caiu aqui: "+result);
                //return;
            }

            regex = new Regex(@"\s*\-\s*" + Identifier);
            match = regex.Match(input);
            if (match.Success)
            {
                result = regex.Replace(result, m => "-1*" + m.Groups["id"].Value);
                //return;
            }

            regex = new Regex(Value + Identifier);
            match = regex.Match(input);
            if (match.Success)
            {
                result = regex.Replace(result, m =>
                {
                    string lh = m.Groups["val"].Value;
                    if (EvaluateSingle(lh) >= 0) lh = "+" + lh;
                    return lh + "*" + m.Groups["id"].Value;
                });
                //return;
            }
        }

        static string ExpressionEvaluator(Match match)
        {
            var lhName = match.Groups["lh"].Value;
            var rhName = match.Groups["rh"].Value;

            if (!EvaluateSingle(lhName, out float lh)) return "null";

            if (!EvaluateSingle(rhName, out float rh)) return "null";

            float resultValue = 0;
        
            switch (match.Groups["op"].Value)
            {
                case "*":
                    resultValue = lh * rh;
                    break;
                case "/":
                    resultValue = lh / rh;
                    break;
                case "+":
                    resultValue = lh + rh;
                    break;
                case "-":
                    resultValue = lh - rh;
                    break;
            }

            return resultValue.ToString();
        }
        static float? EvaluateSingle(string input)
        {
            if (EvaluateSingle(input, out float value)) return value;
            return null;
        }
        static bool EvaluateSingle(string input, out float value)
        {
            if (!float.TryParse(input, out value))
            {
                if (EvaluateIdentifierFunction == null) return false;

                var temp = EvaluateIdentifierFunction(input);
                if (temp == null)
                {
                    Console.WriteLine(input + " is not defined");
                    return false;
                }
                value = temp ?? 0;
            }
            return true;
        }

        static string PrintMatch(Match match)
        {
            float.TryParse(match.Groups["lh"].Value, out float lh);
            float.TryParse(match.Groups["rh"].Value, out float rh);
            return lh + " " + match.Groups["op"].Value + " " + rh;
        }
    }

    public class AttribuitionInterpreter
    {
        public static bool Match(string line, out string varName, out float varValue)
        {
            Regex regex = new Regex(@"\s*(?<var>[a-z]+(?:[a-z]|[0-9])*)\s*=\s*");
            var match = regex.Match(line);

            varName = "";
            varValue = 0;

            if (!match.Success) return false;

            varName = match.Groups["var"].Value;

            var expr = regex.Replace(line, "");
            //Console.WriteLine("Expr = " + expr);
            if (!ExpressionInterpreter.MatchExpression(expr, out string result)) return false;

            varValue = float.Parse(result);

            return true;
        }
    }

    public class Interpreter
    {
        public void Run()
        {
            using (StreamReader reader = new StreamReader("code.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    ExpressionInterpreter.MatchExpression(line, out string result);

                    Console.WriteLine(result);
                }
            }
            Console.ReadKey(true);
        }
    }
}
