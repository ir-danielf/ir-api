
namespace IRLib.Paralela.Codigo.Brainiac
{
    public static class BuscaMenorEsforco
    {
        public static int EncontrarDistancia(string nomeCorreto, string nomeDigitado)
        {

            int n = nomeCorreto.Length;
            int m = nomeDigitado.Length;
            int[,] d = new int[n + 1, m + 1];

            int cost = 0;
            if (n == 0)
                return m;

            if (m == 0)
                return n;

            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= m; j++)
                {
                    cost = ((nomeCorreto[i - 1] == nomeDigitado[j - 1]) ? 0 : 1);
                    d[i, j] = System.Math.Min(
                                        System.Math.Min(d[i - 1, j] + 1,
                                                        d[i, j - 1] + 1),
                                        d[i - 1, j - 1] + cost);
                }
    
            return d[n, m];
        }
    }
}
