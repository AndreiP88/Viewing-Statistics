using System;

namespace libINIFile
{
    public class INIUpdate
    {
        string iniFile;

        public INIUpdate(string iniFile) 
        { 
            this.iniFile = iniFile;
        }

        public INIUpdate()
        {
            this.iniFile = "update.ini";
        }

        private string GetParameter(string section, string key)
        {
            IniFile INI = new IniFile(iniFile);

            String result = "";

            if (INI.KeyExists(key))
                result = INI.ReadString(key, section);

            //MessageBox.Show("GET: [" + section + "][" + key + "]: " + result);
            return result;
        }

        private int GetParameterNumber(string section, string key)
        {
            IniFile INI = new IniFile(iniFile);

            int result = 0;

            if (INI.KeyExists(key))
                result = INI.ReadInt(key, section);

            //MessageBox.Show("GET: [" + section + "][" + key + "]: " + result);
            return result;
        }

        private bool GetParameterBoolean(string section, string key)
        {
            IniFile INI = new IniFile(iniFile);

            bool result = false;

            if (INI.KeyExists(key))
                result = INI.ReadBool(key, section);

            //MessageBox.Show("GET: [" + section + "][" + key + "]: " + result);
            return result;
        }

        private void SetParameter(String section, String key, String value)
        {
            IniFile INI = new IniFile(iniFile);

            //if (INI.KeyExists(key))
                INI.Write(key, value, section);

            //MessageBox.Show("SET: [" + section + "][" + key + "]: " + value);
        }

        /// <summary>
        /// Получить время последней проверки обновления
        /// </summary>
        /// <returns></returns>
        public DateTime GetLastTimeCheckUpdate()
        {
            DateTime result = DateTime.Now.AddYears(-1);

            string temp = GetParameter("general", "lastCheckUpdateTime");

            if (!string.IsNullOrEmpty(temp))
            {
                result = Convert.ToDateTime(temp);
            }

            return result;
        }

        public void SetLastTimeCheckUpdate(DateTime value)
        {
            SetParameter("general", "lastCheckUpdateTime", value.ToString("dd.MM.yyyy HH:mm"));
        }

        /// <summary>
        /// Получить период проверки обновления
        /// </summary>
        /// <returns></returns>
        public int GetPeriodCheckUpdate()
        {
            int result = 60;

            result = GetParameterNumber("general", "periodCheckUpdate");

            return result;
        }

        public void SetPeriodCheckUpdate(int value)
        {
            SetParameter("general", "periodCheckUpdate", value.ToString());
        }

        /// <summary>
        /// Получить период дней для отображения
        /// </summary>
        /// <returns></returns>
        public int GetPeriod()
        {
            int result = 0;

            result = GetParameterNumber("main", "period");

            return result;
        }

        public void SetPeriod(decimal value)
        {
            SetParameter("main", "period", value.ToString());
        }
    }
}
