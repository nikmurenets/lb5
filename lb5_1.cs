using System;
using System.Drawing;

namespace Laba5_1
{
    public class SquareTransformer
    {
        protected PointF[] squarePoints;

        public void ReadSquareArguments()
        {
            Console.Write("Координата X: ");
            float x = float.Parse(Console.ReadLine());

            Console.Write("Координата Y: ");
            float y = float.Parse(Console.ReadLine());

            Console.Write("Размер стороны: ");
            float side = float.Parse(Console.ReadLine());

            squarePoints = new PointF[]
            {
                new PointF(x,        y       ),
                new PointF(x + side, y       ),
                new PointF(x + side, y - side),
                new PointF(x,        y - side)
            };
        }

        public virtual void LeftArrowPress()
        {
            ThrowIfNotInitialized(); // Перемещение квадрата на 1 влево                      
            Move(new SizeF(-1f, 0f));
        }

        public virtual void RightArrowPress()
        {
            ThrowIfNotInitialized(); // Перемещение квадрата на 1 вправо                    
            Move(new SizeF(1f, 0f));
        }
        public virtual void UpArrowPress()
        {
            ThrowIfNotInitialized(); // Перемещение квадрата на 1 вверх                   
            Move(new SizeF(0f, 1f));
        }
        public virtual void DownArrowPress()
        {
            ThrowIfNotInitialized();    // Перемещение квадрата на 1 вниз                
            Move(new SizeF(0f, -1f));
        }

        public void PrintSquareParamsToConsole()
        {
            ThrowIfNotInitialized();

            Console.WriteLine("Координаты вершин квадрата:");

            int i;
            for (i = 0; i < squarePoints.Length - 1; i++)
                Console.Write(
                    $"({squarePoints[i].X};" +
                    $"{squarePoints[i].Y}), "
                );

            Console.WriteLine(squarePoints[i]);
        }

        protected void Move(SizeF offset)
        {
            for (int i = 0; i < squarePoints.Length; i++)
                squarePoints[i] += offset;
        }

        // Бросает исключение если объект
        // не инициализирован
        protected void ThrowIfNotInitialized()
        {
            if (squarePoints is null)
                throw new Exception($"{this.GetType().Name} не инициализирован");
        }
    }
    public class RotatingSquareTransformer
            : SquareTransformer
    {       
        protected const double RotationAngle = 10d;
             
        
        public override void LeftArrowPress()
        {
            ThrowIfNotInitialized(); // Поворачивает квадрат против часовой стрелки      
            Rotate(RotationAngle);
        }

        
        public override void RightArrowPress()
        {
            ThrowIfNotInitialized(); // Поворачивает квадрат против часовой стрелки      
            Rotate(-RotationAngle);
        }
       
        protected void Rotate(double degrees)
        {
            double radians = degrees / 180d * Math.PI;

            for (int i = 0; i < squarePoints.Length; i++)
            {
                double x = squarePoints[i].X;
                double y = squarePoints[i].Y;

                double x1 = x * Math.Cos(radians) - y * Math.Sin(radians);
                double y1 = x * Math.Sin(radians) + y * Math.Cos(radians);

                squarePoints[i].X = Convert.ToSingle(x1);
                squarePoints[i].Y = Convert.ToSingle(y1);
            }
        }
    }


    class Program
{
    static void Main(string[] args)
    {
        
            Console.InputEncoding =
                Console.OutputEncoding =
                    System.Text.Encoding.Unicode;

            Console.WriteLine(
                "Перемещать квадрат или вращать?"
            );
            Console.WriteLine("1 - перемещать");
            Console.WriteLine("2 - вращать");
            Console.Write("Ваш выбор: ");
            int choice = int.Parse(Console.ReadLine());

            SquareTransformer transformer = null;

            if (choice == 1)
                transformer = new SquareTransformer();
            else if (choice == 2)
                transformer = new RotatingSquareTransformer();
            else
                throw new Exception("Неизвестный ответ");

            transformer.ReadSquareArguments();

            Console.Clear();
            PrintKeys();
            Console.WriteLine();
            transformer.PrintSquareParamsToConsole();

            while (true)
            {
                var keyInfo = Console.ReadKey();

                bool exit = false;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        transformer.LeftArrowPress();
                        break;
                    case ConsoleKey.RightArrow:
                        transformer.RightArrowPress();
                        break;
                    case ConsoleKey.UpArrow:
                        transformer.UpArrowPress();
                        break;
                    case ConsoleKey.DownArrow:
                        transformer.DownArrowPress();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }

                if (exit)
                    return;

                Console.Clear();
                PrintKeys();
                Console.WriteLine();
                transformer.PrintSquareParamsToConsole();
            }
        
    }

    private static void PrintKeys()
    {
        Console.WriteLine("Стрелка влево - повернуть/переместить влево");
        Console.WriteLine("Стрелка вправо - повернуть/переместить вправо");
        Console.WriteLine("Стрелка вверх - переместить вверх");
        Console.WriteLine("Стрелка вниз - переместить вниз");
        Console.WriteLine("Escape - выход");
    }
}
}
