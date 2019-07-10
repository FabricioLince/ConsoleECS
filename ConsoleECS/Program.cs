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

            while (true)
            {
                string line = Console.ReadLine();
                if (line == "exit") break;
                else
                {
                    if (AttribuitionInterpreter.Match(line, out string var, out float value))
                    {
                        Console.WriteLine(var + " = " + value);
                    }
                    else
                    {
                        Console.WriteLine(">" + ExpressionInterpreter.Run(line));
                    }
                }
            }
        }
    }

    public class ExpressionInterpreter
    {
        public static float Run(string input)
        {
            if (MatchExpression(input, out string result))
            {
                return float.Parse(result);
            }
            return 0.0f;
        }

        public static bool MatchExpression(string line, out string result)
        {
            var done = MatchOperation(line, out result, @"\*|/", true);
            Console.WriteLine(result);
            done |= MatchOperation(result, out result, @"\+|\-", true);
            Console.WriteLine(result);
            done |= MatchOperation(result, out result, @"\*|/", false);
            Console.WriteLine(result);
            done |= MatchOperation(result, out result, @"\+|\-", false);
            Console.WriteLine(result);
            done |= MatchSingleValue(result, out result);
            Console.WriteLine(result);
            return done;
        }

        static bool MatchOperation(string line, out string result, string op, bool parenteses)
        {
            string pattern = @"\s*(?<lh>-?[0-9]+)\s*(?<op>" + op + @")\s*(?<rh>-?[0-9]+)";
            if (parenteses)
            {
                pattern = @"\s*\(\s*" + pattern + @"\s*\)\s*";
            }
            Regex regex = new Regex(pattern);
            //Console.WriteLine(pattern);
            result = line;
            var match = regex.Match(line);

            if (!match.Success) return false;

            while (match.Success)
            {
                //Console.WriteLine(":" + PrintMatch(match));

                result = regex.Replace(result, ExpressionEvaluator);

                match = regex.Match(result);
            }
            return true;
        }

        static bool MatchSingleValue(string line, out string result)
        {
            Regex regex = new Regex(@"\s*(?<lh>-?[0-9]+)\s*");
            var match = regex.Match(line);
            result = line;
            if (!match.Success) return false;

            result = match.Groups["lh"].Value;
            return true;
        }

        static string ExpressionEvaluator(Match match)
        {
            float.TryParse(match.Groups["lh"].Value, out float lh);
            float.TryParse(match.Groups["rh"].Value, out float rh);
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
            Regex regex = new Regex(@"\s*(?<var>[a-z]+)\s*=\s*");
            var match = regex.Match(line);

            varName = "";
            varValue = 0;

            if (!match.Success) return false;

            varName = match.Groups["var"].Value;

            var expr = regex.Replace(line, "");
            Console.WriteLine("Expr = " + expr);
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
                while(!reader.EndOfStream)
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
