namespace Numerical_Methods_Lab_1;

public class Iteration
{
    public double F(double x) => x*x*x - 6*x*x + 5*x + 12;

    private double Fp(double x) => 3*x*x - 12*x + 5;

    public (double root, int iters) SimpleIteration(double x0, double tol, double alpha = 0.0, int maxIter = 1000)
    {
        var x = x0;

        if (alpha <= 0.0)
        {
            var M = Math.Abs(Fp(x0));
            var safety = 1.0; 
            alpha = 1.0 / (M + safety);
            
            if (alpha > 1.0) alpha = 1.0;
        }

        for (var k = 1; k <= maxIter; k++)
        {
            var fx = F(x);
            var xNew = x - alpha * fx;

            Console.WriteLine($"{k,3}: x = {xNew,12:F10}    f(x) = {F(xNew),12:E6}    |dx| = {Math.Abs(xNew - x):E6}");

            if (Math.Abs(xNew - x) < tol || Math.Abs(F(xNew)) < tol)
                return (xNew, k);

            x = xNew;
        }

        return (x, maxIter);
    }
}