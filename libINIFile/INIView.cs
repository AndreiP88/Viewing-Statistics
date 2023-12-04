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
        /// Учитавыть при рассчетах смены без выработки
        /// </summary>
        /// <returns></returns>
        public bool GetCalculateShiftsInIdletime()
        {
            bool result = false;

            result = GetParameterBoolean("main", "calculateShiftsInIdletime");

            return result;
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

        public void SetNormTime(int value)
        {
            SetParameter("main", "normTime", value.ToString());
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

        public void SetCountShifts(decimal value)
        {
            SetParameter("main", "countShifts", value.ToString());
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

        public void SetLoadAllEquipForUser(bool value)
        {
            SetParameter("main", "loadAllEquipForUser", value.ToString());
        }

        /// <summary>
        /// Загружать список закозов 
        /// </summary>
        /// <returns></returns>
        public bool GetGivenShiftNumber()
        {
            bool result = true;

            result = GetParameterBoolean("main", "givenShiftNumber");

            return result;
        }

        public void SetGivenShiftNumber(bool value)
        {
            SetParameter("main", "givenShiftNumber", value.ToString());
        }

        public bool GetAutoAddDays()
        {
            bool result = false;

            result = GetParameterBoolean("main", "autoAddDays");

            return result;
        }

        public void SetAutoAddDays(bool value)
        {
            SetParameter("main", "autoAddDays", value.ToString());
        }

        public bool GetViewCurrentDay()
        {
            bool result = false;

            result = GetParameterBoolean("main", "viewCurrentDay");

            return result;
        }

        public void SetViewCurrentDay(bool value)
        {
            SetParameter("main", "viewCurrentDay", value.ToString());
        }

        public bool GetColWorksOutAutoWidth()
        {
            bool result = false;

            result = GetParameterBoolean("columns", "wColWorksOutAutoWidth");

            return result;
        }

        public void SetColWorksOutAutoWidth(bool value)
        {
            SetParameter("columns", "wColWorksOutAutoWidth", value.ToString());
        }

        public int GetWidthNumberCol()
        {
            int result = 0;

            result = GetParameterNumber("columns", "wColNum");

            return result;
        }

        public void SetWidthNumberCol(decimal value)
        {
            SetParameter("columns", "wColNum", value.ToString());
        }

        public int GetWidthNameCol()
        {
            int result = 0;

            result = GetParameterNumber("columns", "wColName");

            return result;
        }

        public void SetWidthNameCol(decimal value)
        {
            SetParameter("columns", "wColName", value.ToString());
        }

        public int GetWidthWorkingOutCol()
        {
            int result = 0;

            result = GetParameterNumber("columns", "wColsWorkOut");

            return result;
        }

        public void SetWidthWorkingOutCol(decimal value)
        {
            SetParameter("columns", "wColsWorkOut", value.ToString());
        }

        public int GetWidthResultsCol()
        {
            int result = 0;

            result = GetParameterNumber("columns", "wColsResults");

            return result;
        }

        public void SetWidthResultsCol(decimal value)
        {
            SetParameter("columns", "wColsResults", value.ToString());
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
