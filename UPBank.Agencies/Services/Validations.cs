namespace UPBank.Agencies.Services
{
    public static class Validations
    {
        public static bool CNPJ(string cnpj)
        {
            if (cnpj.Length != 14)
                return false;

            if (cnpj.Count(value => value == cnpj[0]) > cnpj.Length - 1)
                return false;

            return ValidarDV(cnpj, 1) && ValidarDV(cnpj, 2);
        }

        private static bool ValidarDV(string cpf, int dv)
        {
            int result = 0;

            for (int posicao = 0, multiplicador = 4 + dv; posicao < 11 + dv; posicao++, multiplicador--)
            {
                if (multiplicador == 1)
                    multiplicador = 9;
                result += int.Parse(cpf.Substring(posicao, 1)) * multiplicador;
            }

            result %= 11;

            return (result == 0 || result == 1 ? 0 : 11 - result) == int.Parse(cpf.Substring(11 + dv, 1));
        }
    }
}