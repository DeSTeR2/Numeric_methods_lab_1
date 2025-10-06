using System;

namespace Numerical_Methods_Lab_1
{
    public class Newton
    {
        private double F(double x) => Math.Pow(x, 3) - 7 * x - 7;
        private double Fp(double x) => 3 * Math.Pow(x, 2) - 7;

        private Func<double, double> MakeG(double x0)
        {
            var dfx0 = Fp(x0);
            return (x) => x - F(x) / dfx0;
        }

        private Func<double, double> MakeGp(double x0)
        {
            var dfx0 = Fp(x0);
            return (x) => 1.0 - Fp(x) / dfx0;
        }

        private double EstimateL(Func<double,double> gp, double x0, double delta = 1.0, int samples = 100)
        {
            double a = x0 - delta, b = x0 + delta;
            if (a == b) return Math.Abs(gp(x0));
            var max = 0.0;
            for (var i = 0; i <= samples; i++)
            {
                var t = a + (b - a) * i / (double)samples;
                var val = Math.Abs(gp(t));
                if (val > max) max = val;
            }
            return max;
        }

        private int EstimateIterationsApriori(Func<double,double> g, Func<double,double> gp, double x0, double tol)
        {
            var delta = 1.0; 
            var L = EstimateL(gp, x0, delta, 200);
            if (L >= 1.0)
                return -1;

            var x1 = g(x0);
            var initDiff = Math.Abs(x1 - x0);
            if (initDiff == 0.0) return 1; 

            if (L == 0.0)
                return 1;

            var numerator = (1.0 - L) * tol / initDiff;
            if (numerator <= 0.0)
                return -1;

            var nReal = Math.Log(numerator) / Math.Log(L);
            var n = (int)Math.Ceiling(nReal);
            if (n < 1) n = 1;
            return n;
        }

        public (double root, int iterations) ModifiedNewton(double x0, double tol, int maxIter = 1000)
        {
            var dfx0 = Fp(x0);
            if (Math.Abs(dfx0) < 1e-12)
                throw new ArgumentException("Похідна в початковій точці близька до нуля. Оберіть інший x0.");

            var g = MakeG(x0);
            var gp = MakeGp(x0);

            var apriori = EstimateIterationsApriori(g, gp, x0, tol);
            Console.WriteLine(apriori == -1
                ? "Апріорна оцінка: L >= 1 або не вдалося отримати гарантовану збіжність на обраному інтервалі."
                : $"Апріорна оцінка потрібної кількості ітерацій: {apriori}");

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
}
