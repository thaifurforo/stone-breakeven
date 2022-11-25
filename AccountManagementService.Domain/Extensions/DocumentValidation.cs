namespace AccountManagementService.Domain.Extensions
{
    public static class DocumentValidation
    {
        public static bool IsValidCpf(this string document)
        {
            try
            {
                var multiplier1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                var multiplier2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                document = document.Trim().Replace(".", "").Replace("-", "");
                if (document.Length != 11)
                {
                    return false;
                }

                for (var j = 0; j < 10; j++)
                {
                    if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == document)
                    {
                        return false;
                    }
                }

                var hasCpf = document.Substring(0, 9);
                var sum = 0;

                for (var i = 0; i < 9; i++)
                {
                    sum += int.Parse(hasCpf[i].ToString()) * multiplier1[i];
                }

                var modResult = sum % 11;
                if (modResult < 2)
                {
                    modResult = 0;
                }
                else
                {
                    modResult = 11 - modResult;
                }

                var digit = modResult.ToString();
                hasCpf = hasCpf + digit;
                sum = 0;
                for (var i = 0; i < 10; i++)
                {
                    sum += int.Parse(hasCpf[i].ToString()) * multiplier2[i];
                }

                modResult = sum % 11;
                if (modResult < 2)
                {
                    modResult = 0;
                }
                else
                {
                    modResult = 11 - modResult;
                }

                digit = digit + modResult;
                return document.EndsWith(digit);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsValidCnpj(this string document)
        {
            try
            {
                var multiplier1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                var multiplier2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

                document = document.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
                if (document.Length != 14)
                {
                    return false;
                }
                var hasCnpj = document.Substring(0, 12);
                var sum = 0;

                for (var i = 0; i < 12; i++)
                {
                    sum += int.Parse(hasCnpj[i].ToString()) * multiplier1[i];
                }

                if (sum <= 0)
                {
                    return false;
                }

                var modResult = (sum % 11);
                if (modResult < 2)
                {
                    modResult = 0;
                }
                else
                {
                    modResult = 11 - modResult;
                }

                var digit = modResult.ToString();
                hasCnpj = hasCnpj + digit;
                sum = 0;
                for (var i = 0; i < 13; i++)
                {
                    sum += int.Parse(hasCnpj[i].ToString()) * multiplier2[i];
                }

                modResult = (sum % 11);
                if (modResult < 2)
                {
                    modResult = 0;
                }
                else
                {
                    modResult = 11 - modResult;
                }

                digit = digit + modResult;
                return document.EndsWith(digit);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
