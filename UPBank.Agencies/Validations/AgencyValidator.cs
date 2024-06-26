using UPBank.Models;

namespace UPBank.Agencies.Validations
{
    public class AgencyValidator
    {
        public static bool Validate(Agency agency)
        {
            if (agency == null)
                throw new Exception("Agency value is null.");

            if (ValidateCNPJ(agency.Cnpj) == false)
                throw new Exception("Agency CNPJ is invalid.");

            if (string.IsNullOrEmpty(agency.Address.Street))
                throw new Exception("Agency address is invalid.");

            if (agency.Employees.Count == 0)
                throw new Exception("Agency must have at least one employee.");

            if (agency.Employees.Count(e => e.Manager) == 0)
                throw new Exception("Agency must have a manager.");

            return true;
        }
        private static bool ValidateCNPJ(string cnpj)
        {
            if (cnpj.Length != 14)
                return false;

            if (cnpj.Count(value => value == cnpj[0]) > cnpj.Length - 1)
                return false;

            return ValidateDV(cnpj, 1) && ValidateDV(cnpj, 2);
        }

        private static bool ValidateDV(string cpf, int dv)
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