namespace NTTLapso.Support_methods
{
    public class SupportMethods
    {
        //Method to check if the first posistion of the sentence have a comma, then remove it.
        public string CheckCommas(string sqlSentence)
        {
            char[] stringArray = sqlSentence.ToCharArray();
            if (stringArray[0] == ',')
                sqlSentence = sqlSentence.Substring(1, sqlSentence.Length - 1);

            return sqlSentence;
        }
    }
}
