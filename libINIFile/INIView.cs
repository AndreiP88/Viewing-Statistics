using System;

namespace libINIFile
{
    public class INIView
    {
        string iniFile;

        public INIView(string iniFile) 
        { 
            this.iniFile = iniFile;
        }

        public INIView()
        {
            this.iniFile = "view.ini";
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
        /// Получит норму выработки за смену
        /// </summary>
        /// <returns></returns>
        public int GetNormTime()
        {
            int result = 0;

            result = GetParameterNumber("main", "normTime");

            return result;
        }

        /// <summary>
        /// Получить количество смен
        /// </summary>
        /// <returns></returns>
        public int GetCountShifts()
        {
            int result = 0;

            result = GetParameterNumber("main", "countShifts");

            return result;
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

        /// <summary>
        /// Получить значение указывающее способ загрузки оборудования для сотрудника: загружать все оборудование или только указанное
        /// </summary>
        /// <returns></returns>
        public bool GetLoadAllEquipForUser()
        {
            bool result = false;

            result = GetParameterBoolean("main", "loadAllEquipForUser");

            return result;
        }

        public int GetWidthWorkingOutCol()
        {
            int result = 0;

            result = GetParameterNumber("columns", "wColsWorkOut");

            return result;
        }
        public void SetNormTime(int value)
        {
            SetParameter("main", "normTime", value.ToString());
        }

        public void SetCountShifts(decimal value)
        {
            SetParameter("main", "countShifts", value.ToString());
        }

        public void SetLoadAllEquipForUser(bool value)
        {
            SetParameter("main", "loadAllEquipForUser", value.ToString());
        }

        public bool GetLoadCurrentShift()
        {
            bool result = false;

            result = GetParameterBoolean("statistic", "loadCurrentShift");

            return result;
        }

        public bool GetAutoUpdateStatistic()
        {
            bool result = false;

            result = GetParameterBoolean("statistic", "autoUpdateStatistic");

            return result;
        }

        public int GetPeriodAutoUpdateStatistic()
        {
            int result = 0;

            result = GetParameterNumber("statistic", "periodAutoUpdateStatistic");

            return result;
        }

        public void SetLoadCurrentShift(bool value)
        {
            SetParameter("statistic", "loadCurrentShift", value.ToString());
        }

        public void SetAutoUpdateStatistic(bool value)
        {
            SetParameter("statistic", "autoUpdateStatistic", value.ToString());
        }

        public void SetPeriodAutoUpdateStatistic(int value)
        {
            SetParameter("statistic", "periodAutoUpdateStatistic", value.ToString());
        }

        public int GetLastTabIndex()
        {
            int result = 0;

            result = GetParameterNumber("main", "lastTabIndex");

            return result;
        }

        public void SetLastTabIndex(int value)
        {
            SetParameter("main", "lastTabIndex", value.ToString());
        }






        public bool GetAutoUpdate()
        {
            bool result = false;
            result = Convert.ToBoolean(GetParameter("update", "autoUpdate"));

            return result;
        }

        public String GetLastDateVersion()
        {
            String result = GetParameter("update", "lastVersion");

            return result;
        }

        public String GetLastUpdateTime()
        {
            String result = GetParameter("update", "lastUpdateTime");

            return result;
        }

        public String GetPeriodUpdate()
        {
            String result = GetParameter("update", "periodUpdate");

            return result;
        }


        public void SetAutoUpdate(bool value)
        {
            SetParameter("update", "autoUpdate", value.ToString());
        }

        public void SetLastDateVersion(String value)
        {
            SetParameter("update", "lastDateVersion", value);
        }

        public void SetLastUpdateTime(String value)
        {
            SetParameter("update", "lastUpdateTime", value);
        }

        public void SetPeriodUpdate(String value)
        {
            SetParameter("update", "periodUpdate", value);
        }


        public String GetViewedEquipment()
        {
            String result = GetParameter("equips", "viewedEquipment");

            return result;
        }

        public void SetViewedEquipment(string value)
        {
            SetParameter("equips", "viewedEquipment", value);
        }
    }


}
