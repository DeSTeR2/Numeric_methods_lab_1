namespace Numerical_Methods_Lab_1;

public class Newton
{
    private double F(double x) => Math.Pow(x, 3) - 7 * x - 7;

    private double Fp(double x) => 3 * Math.Pow(x, 2) - 7;

    public (double root, int iterations) ModifiedNewton(double x0, double tol, int maxIter = 1000)
    {
        var dfx0 = Fp(x0); 
        if (Math.Abs(dfx0) < 1e-12)
            throw new ArgumentException("Похідна в початковій точці близька до нуля. Оберіть інший x0.");

        var x = x0;
        var k = 0;

        while (k < maxIter)
        {
            var fx = F(x);
            var xNew = x - fx / dfx0;

            if (Math.Abs(F(xNew)) < tol || Math.Abs(xNew - x) < tol)
                return (xNew, k + 1);

            x = xNew;
            k++;
        }

        return (x, maxIter);
    }
}