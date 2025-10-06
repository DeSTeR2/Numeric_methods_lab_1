using System.Text;

namespace Numerical_Methods_Lab_1;

internal static class Program
{
    static void Newton(double x0, double epsilon)
    {
        Console.WriteLine("Розв'язок x^3 - 7x - 7 = 0 модифікованим методом ньютона.");
        
        var newton = new Newton();
        var (root, iterations) = newton.ModifiedNewton(x0, epsilon);

        Console.WriteLine($"\nЗнайдений корінь: {root:F6}");
        Console.WriteLine($"Кількість ітерацій: {iterations}");
    }
    
    static void Iteration(double x0, double epsilon)
    {
        Console.WriteLine("Розв'язок x^3 - 6x^2 + 5x + 12 = 0 методом простої ітерації.");
        
        Console.Write("Введіть alpha: ");
        var aStr = Console.ReadLine();
        var alpha = 0.0;
        if (!string.IsNullOrWhiteSpace(aStr))
        {
            if (!double.TryParse(aStr, out alpha) || alpha <= 0)
            {
                Console.WriteLine("Невірне alpha; буде підібрано автоматично.");
                alpha = 0.0;
            }
        }

        Console.WriteLine($"\nПочинаємо з x0={x0}, eps={epsilon}, alpha={(alpha>0?alpha.ToString():"(авто)")} \n");
        var iteration = new Iteration();
        var (root, iters) = iteration.SimpleIteration(x0, epsilon, alpha);

        Console.WriteLine($"\nЗавершено: наближений корінь = {root:F10}, ітерацій: {iters}");
        Console.WriteLine($"f(root) = {iteration.F(root):E6}");
    }
    
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.Write("Введіть початкове наближення x0: ");
        var x0 = double.Parse(Console.ReadLine() ?? string.Empty);

        Console.Write("Введіть точність (наприклад 0,001): ");
        var epsilon = double.Parse(Console.ReadLine() ?? string.Empty);

        Newton(x0, epsilon);
        Iteration(x0, epsilon);
    }
}