using System.Text;

namespace NTTLapso.Tools
{
    public class LogBuilder
    {
        private StringBuilder builder = new StringBuilder();

        public string Message { get { return builder.ToString(); } } 

        public void LogIf(string bodyMsg)
        {
            DateTime current = DateTime.Now;
            builder.AppendLine("[" + current.ToShortDateString() + "] [" + current.ToShortTimeString() + "] [IF] - " + bodyMsg + " ");
        }

        public void LogOk(string bodyMsg)
        {
            DateTime current = DateTime.Now;
            builder.AppendLine("[" + current.ToShortDateString() + "] [" + current.ToShortTimeString() + "] [OK] - " + bodyMsg + " ");
        }

        public void LogKo(string bodyMsg)
        {
            DateTime current = DateTime.Now;
            builder.AppendLine("[" + current.ToShortDateString() + "] [" + current.ToShortTimeString() + "] [KO] - " + bodyMsg + " ");
        }

        public void LogErr(string bodyMsg) 
        {
            DateTime current = DateTime.Now;
            builder.AppendLine( "[" + current.ToShortDateString() + "] [" + current.ToShortTimeString() + "] [ER] - " + bodyMsg + " ");
        }
    }
}
