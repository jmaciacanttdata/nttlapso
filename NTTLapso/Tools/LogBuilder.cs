using System.Text;

namespace NTTLapso.Tools
{
    public class LogBuilder
    {
        private List<string> _logList = new List<string>();

        public List<string> LogList { get { return _logList; } }

        public void LogIf(string bodyMsg)
        {
            DateTime current = DateTime.Now;
            _logList.Add("[" + current.ToShortDateString() + "] [" + current.ToShortTimeString() + "] [IF] - " + bodyMsg + " ");
        }

        public void LogOk(string bodyMsg)
        {
            DateTime current = DateTime.Now;
            _logList.Add("[" + current.ToShortDateString() + "] [" + current.ToShortTimeString() + "] [OK] - " + bodyMsg + " ");
        }

        public void LogKo(string bodyMsg)
        {
            DateTime current = DateTime.Now;
            _logList.Add("[" + current.ToShortDateString() + "] [" + current.ToShortTimeString() + "] [KO] - " + bodyMsg + " ");
        }

        public void LogErr(string bodyMsg) 
        {
            DateTime current = DateTime.Now;
            _logList.Add( "[" + current.ToShortDateString() + "] [" + current.ToShortTimeString() + "] [ER] - " + bodyMsg + " ");
        }

        public void Append(LogBuilder other, bool toBeginning = false)
        {
            if (other == null) return;

            List<string> temp = new List<string>(_logList.Count + other._logList.Count);

            if (!toBeginning)
            {
                _logList.ForEach((msg) => { temp.Add(msg); });
                other._logList.ForEach((msg) => { temp.Add(msg); });
            }
            else
            {
                other._logList.ForEach((msg) => { temp.Add(msg); });
                _logList.ForEach((msg) => { temp.Add(msg); });
            }

            _logList = temp;
        }
    }
}
