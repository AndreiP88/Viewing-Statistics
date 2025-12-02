using libData;
using libINIFile;
using libSql;
using libTime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Productivity
{
    public partial class FormViewShiftDetails : Form
    {
        DateTime Date;
        int Shift;
        string Value;

        private int LoadUser = -1;
        private int LoadEquip = -1;

        public FormViewShiftDetails(DateTime loadDate, int loadShift, string loadValue)
        {
            InitializeComponent();

            Date = loadDate;
            Shift = loadShift;
            Value = loadValue;
        }

        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, string> machines = new Dictionary<int, string>();

        private async void FormAddEquips_Load(object sender, EventArgs e)
        {
            try
            {
                ValueEquips equipsValue = new ValueEquips();
                ValueUsers usersValue = new ValueUsers();
                
                machines = equipsValue.LoadMachine();
                users = usersValue.LoadAllUsersNames();

                ParseUserAndEquip();

                //LoadOrdersSelectedDateAndShiftDetails(Date, Shift);

                string nameUserEquiup = "";

                if (LoadUser != -1)
                {
                    nameUserEquiup = users[LoadUser];
                }

                if (LoadEquip != -1)
                {
                    nameUserEquiup += ", " + machines[LoadEquip];
                }

                dateTimePicker1.Value = Date;
                metroSetLabel1.Text = Shift + " смена";
                metroSetLabel2.Text = nameUserEquiup;

                await LoadOrdersSelectedDateAndShiftDetailsAsync(Date, Shift);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }

        private void ParseUserAndEquip()
        {
            if (Value.StartsWith("e"))
            {
                int indexU = Value.IndexOf("u");

                if (indexU != -1)
                {
                    var matches = Regex.Matches(Value, @"\d+");
                    LoadEquip = int.Parse(matches[0].Value);
                    LoadUser = int.Parse(matches[1].Value);
                }
                else
                {
                    var matches = Regex.Matches(Value, @"\d+");
                    LoadEquip = int.Parse(matches[0].Value);
                    LoadUser = -1;
                }
            }

            if (Value.StartsWith("u"))
            {
                var matches = Regex.Matches(Value, @"\d+");
                LoadUser = int.Parse(matches[0].Value);
                LoadEquip = -1;
            }
        }

        private void CreateColomnsToDataGridForOneShift()
        {
            dataGridViewOneShift.Rows.Clear();
            dataGridViewOneShift.Columns.Clear();

            dataGridViewOneShift.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridViewOneShift.AllowUserToResizeColumns = false;
            dataGridViewOneShift.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewOneShift.AllowUserToResizeRows = false;
            dataGridViewOneShift.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            DataGridViewContentAlignment right = DataGridViewContentAlignment.MiddleRight;
            DataGridViewContentAlignment left = DataGridViewContentAlignment.MiddleLeft;
            DataGridViewContentAlignment center = DataGridViewContentAlignment.MiddleCenter;

            string[] colNames = { "№", "Имя", "Заказ", "Заказчик", "Операция", "Тираж", "Дано времени", "Начало", "Завершение", "Продолжительность", "Планируемое время завершения", "Отклонение", "Сделано", "Выработка" };
            int[] colWidth = { 30, 280, 100, 280, 200, 140, 70, 135, 135, 80, 135, 90, 80, 120 };
            DataGridViewContentAlignment[] colAligment = { right, left, left, left, left, left, center, left, left, center, left, center, left, center };

            for (int i = 0; i < colNames.Length; i++)
            {
                int indexCol = dataGridViewOneShift.Columns.Add(colNames[i], colNames[i]);
                dataGridViewOneShift.Columns[indexCol].Width = colWidth[i];
                dataGridViewOneShift.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewOneShift.Columns[indexCol].DefaultCellStyle.Alignment = colAligment[i];
            }

            dataGridViewOneShift.Columns[0].Frozen = true;
            dataGridViewOneShift.Columns[1].Frozen = true;

            dataGridViewOneShift.Rows.Add();
            dataGridViewOneShift.Rows[0].Height = 60;
            dataGridViewOneShift.Rows[0].Frozen = true;

            for (int i = 0; i < colNames.Length; i++)
            {
                AddCellToGrid(dataGridViewOneShift, 0, i);
                dataGridViewOneShift.Rows[0].Cells[i].Value = colNames[i];
            }
        }

        private void AddCellToGrid(DataGridView dataGrid, int indexRow, int indexCell, int collSpan = 1)
        {
            HMergedCell pCell;

            //int nOffset = indexCell;

            for (int j = indexCell; j < indexCell + collSpan; j++)
            {
                dataGrid.Rows[indexRow].Cells[j] = new HMergedCell();
                pCell = (HMergedCell)dataGrid.Rows[indexRow].Cells[j];
                pCell.LeftColumn = indexCell;
                pCell.RightColumn = indexCell + collSpan - 1;
            }
            //nOffset += collSpan + 1;
        }

        private List<int> CategoryEquipToListSelectedEquip(List<Category> categories)
        {
            List<int> equips = new List<int>();

            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].Selected)
                {
                    for (int j = 0; j < categories[i].Equips.Count; j++)
                    {
                        if (categories[i].Equips[j].Selected)
                        {
                            equips.Add(categories[i].Equips[j].Id);
                        }
                    }
                }
            }

            return equips;
        }

        private List<UserShiftOrder> SelectedFullStepsForCurrentOrder(List<UserShiftOrder> userShiftOrders, int idManOrderJobItem)
        {
            List<UserShiftOrder> userShiftOrder = new List<UserShiftOrder>();

            for (int i = 0; i < userShiftOrders.Count; i++)
            {
                if (userShiftOrders[i].IdManOrderJobItem == idManOrderJobItem)
                {
                    userShiftOrder.Add(userShiftOrders[i]);
                }
                else
                {
                    if (AreThereAnyMoreOrders(userShiftOrders.GetRange(i, userShiftOrders.Count - i), idManOrderJobItem) && i != 0)
                    {
                        userShiftOrder.Add(userShiftOrders[i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return userShiftOrder;
        }

        private bool AreThereAnyMoreOrders(List<UserShiftOrder> userShiftOrders, int idManOrderJobItem)
        {
            bool result = false;

            for (int i = 0; i < userShiftOrders.Count; i++)
            {
                if (idManOrderJobItem != userShiftOrders[i].IdManOrderJobItem)
                {
                    if (userShiftOrders[i].IdletimeName == "")
                    {
                        result = false;
                        break;
                    }
                }

                if (idManOrderJobItem == userShiftOrders[i].IdManOrderJobItem)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private bool IsLastRecordOfOrder(List<UserShiftOrder> userShiftOrders)
        {
            bool result = true;

            for (int i = 1; i < userShiftOrders.Count; i++)
            {
                if (userShiftOrders[i].IdletimeName == "")
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
        private float CalculateWorkTimeForOneOrder(UserShiftOrder order)
        {
            float workingOut = 0;

            if (order.Normtime > 0)
            {
                if (order.PlanOutQty > 0)
                {
                    workingOut += ((float)order.FactOutQty * (float)order.Normtime) / (float)order.PlanOutQty;
                }
                else
                {
                    if (order.FactOutQty > 0)
                    {
                        workingOut += (float)order.Normtime;
                    }
                }
            }

            return workingOut;
        }

        private int AddDinnerTimeToWorkingOut(string firstTime, string secondTime)
        {
            int result = 0;

            DateTime firstDateTime = Convert.ToDateTime(firstTime);
            DateTime secondDateTime = Convert.ToDateTime(secondTime);

            DateTime firstDate = new DateTime(firstDateTime.Year, firstDateTime.Month, firstDateTime.Day);
            DateTime secondDate = new DateTime(secondDateTime.Year, secondDateTime.Month, secondDateTime.Day);

            string[] breakeTimes = { "11:30", "30", "15:30", "30", "18:00", "10", "23:30", "30", "03:30", "30", "06:00", "10" };

            int dayDifference = (secondDate - firstDate).Days;

            for (int day = 0; day <= dayDifference; day++)
            {
                DateTime selectDate = firstDateTime.AddDays(day);

                for (int i = 0; i < breakeTimes.Length; i += 2)
                {
                    DateTime breakeDateTime = Convert.ToDateTime(selectDate.ToString("dd.MM.yyyy") + " " + breakeTimes[i] + ":00");
                    int breakTime = Convert.ToInt32(breakeTimes[i + 1]);

                    if (firstDateTime < breakeDateTime && breakeDateTime < secondDateTime)
                    {
                        secondDateTime.AddMinutes(breakTime);
                        result += breakTime;
                    }
                }
            }

            return result;
        }

        private async Task LoadOrdersSelectedDateAndShiftDetailsAsync(DateTime selectDate, int selectShift)
        {
            CreateColomnsToDataGridForOneShift();

            ValueShifts shifts = new ValueShifts();
            ValueDateTime time = new ValueDateTime();
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            INISettings settings = new INISettings();

            bool givenShiftNumber = settings.GetGivenShiftNumber();
            int normTime = settings.GetNormTime();

            List<Category> categoryEquip = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            List<int> equips = new List<int>();

            if (LoadEquip == -1)
            {
                equips = CategoryEquipToListSelectedEquip(categoryEquip);
            }
            else
            {
                equips.Add(LoadEquip);
            }

            string timeStartShift = time.StartShiftPlanedDateTime(selectDate, selectShift);
            string timeEndShift = time.EndShiftPlanedDateTime(timeStartShift);

            List<User> usersShiftList = await shifts.LoadOrdersAsync(selectDate, selectShift, givenShiftNumber, LoadUser, LoadEquip);

            List<int> usersCurrent = new List<int>();

            //Пока так, потом сделаю отдельную выборку оборудования с сортировкой, либо без сортирвки а в порядке загрузки
            //если бы БД адекватно хранила индексы смены, а не херила их после редактирования записи, то было бы проще привязываться к смене в fbc_brigade
            for (int i = 0; i < equips.Count; i++)
            {
                for (int j = 0; j < usersShiftList.Count; j++)
                {
                    for (int k = 0; k < usersShiftList[j].Shifts.Count; k++)
                    {
                        for (int l = 0; l < usersShiftList[j].Shifts[k].Orders.Count; l++)
                        {
                            if (usersShiftList[j].Shifts[k].Orders[l].IdEquip == equips[i])
                            {
                                if (!usersCurrent.Contains(usersShiftList[j].Id))
                                {
                                    usersCurrent.Add(usersShiftList[j].Id);
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < usersCurrent.Count; i++)
            {
                int indexRow = dataGridViewOneShift.Rows.Add();

                dataGridViewOneShift.Rows[indexRow].Cells[0].Value = (i + 1).ToString();
                dataGridViewOneShift.Rows[indexRow].Cells[1].Value = users[usersCurrent[i]];
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.Font = new Font(dataGridViewOneShift.Font, FontStyle.Bold);
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.BackColor = Color.Gray;
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                float userWorkingOut = 0;
                float userDone = 0;
                int dinnerTime = 0;

                int idletime = 0;
                //int indexRowForUser = listView1.Items.Count - 1;

                //Сделать детальное отображение выполняемых заказов
                for (int j = 0; j < usersShiftList.Count; j++)
                {
                    if (usersShiftList[j].Id == usersCurrent[i])
                    {
                        UserShift userShift = usersShiftList[j].Shifts[0];

                        int currentStep = 0;
                        int countOrder = 0;
                        int countOperation = 0;

                        //dataGridViewOneShift.Rows[indexRow].Cells[7].Value = userShift.ShiftDateBegin;
                        //dataGridViewOneShift.Rows[indexRow].Cells[8].Value = userShift.ShiftDateEnd;

                        while (currentStep < userShift.Orders.Count)
                        {
                            countOperation++;
                            //countOrder++;

                            bool isMakeready = false;

                            List<UserShiftOrder> userShiftOrders = SelectedFullStepsForCurrentOrder(userShift.Orders.GetRange(currentStep, userShift.Orders.Count - currentStep), userShift.Orders[currentStep].IdManOrderJobItem);

                            for (int l = 0; l < userShiftOrders.Count; l++)
                            {
                                UserShiftOrder order = userShiftOrders[l];
                                ViewOrder view = new ViewOrder();

                                string orderStartTime = userShiftOrders[0].DateBegin;
                                string orderEndTime = userShiftOrders[l].DateEnd;

                                string timeBegin = Convert.ToDateTime(order.DateBegin).ToString("dd.MM.yyyy HH:mm");
                                string timeEnd = Convert.ToDateTime(order.DateEnd).ToString("dd.MM.yyyy HH:mm");

                                view.TimeBegin = timeBegin + "";

                                view.WorkingOut += CalculateWorkTimeForOneOrder(order);

                                //if (order.Flags != 576)
                                if (order.OperationType == 1)
                                {
                                    if (order.IdletimeName == "")
                                    {
                                        view.Done += order.FactOutQty;
                                    }

                                    view.Amount = order.PlanOutQty;
                                    view.NormTimeWork = order.Normtime;
                                    view.IdletimeName = "работа";
                                }

                                //if (order.Flags == 576)
                                if (order.OperationType == 0)
                                {
                                    view.DoneMakeReady += order.FactOutQty;

                                    view.MakeReady = order.PlanOutQty;
                                    view.NormTimeMakeReady = order.Normtime;
                                    view.IdletimeName = "приладка";
                                }

                                if (order.IdletimeName != "")
                                {
                                    //countOrder -= 1;

                                    view.IdletimeName = order.IdletimeName;

                                    if (order.FactOutQty > 0)
                                    {
                                        //idletime += order.Normtime
                                        //Поскольку время планового простоя учитывается в выработке, то не смысла в этой записи
                                    }
                                }

                                string lastTimeEndPlanedOrder = time.DateTimeAmountMunutes(timeStartShift, (int)Math.Round(userWorkingOut));
                                view.Duration += order.Duration;

                                userWorkingOut += view.WorkingOut;
                                userDone += view.Done;

                                //string lastTimeEndPlanedOrder = time.DateTimeAmountMunutes(timeStartShift, (int)Math.Round(userWorkingOut));

                                //может сразу при запросе считаьь времия на приладку делить время на количество приладко???
                                Normtime normtime = shifts.GetNormTimeForOrder(order.IdManOrderJobItem);

                                float orderPreviousAmount = shifts.GetAmountDoneFromPreviousShifts(userShift.Orders[currentStep].IdManOrderJobItem, order.DateBegin, 1);
                                float lastAmount = (view.Amount - orderPreviousAmount) < 0 ? 0 : (view.Amount - orderPreviousAmount);

                                float orderPreviousMakeReady = shifts.GetAmountDoneFromPreviousShifts(userShift.Orders[currentStep].IdManOrderJobItem, order.DateBegin, 0);
                                float lastMakeReady = orderPreviousMakeReady == 0 ? 1 : (normtime.PlanOutQtyMakeReady - orderPreviousMakeReady);

                                int normTimeFull = 0;
                                int normTimeGeneral = 0;

                                float lastTimeWork = 0;
                                float lastTimeMakeReady = (normtime.PlanNormtimeMakeReady / (normtime.PlanOutQtyMakeReady == 0 ? 1 : normtime.PlanOutQtyMakeReady)) * lastMakeReady;

                                Console.WriteLine(order.OrderNumber + ": " + order.OrderName + " | " + order.IdManOrderJobItem + " - " + normtime.PlanNormtimeMakeReady + " / " + normtime.PlanOutQtyMakeReady + " * " + lastMakeReady + " = " + lastTimeMakeReady + "\nРабота: " + view.NormTimeWork + " == " + normtime.PlanNormtimeWork + "\nПриладка: " + view.NormTimeMakeReady + " == " + normtime.PlanNormtimeMakeReady +
                                    "\nТираж: " + view.Amount + " Сделано: " + view.Done + "\nПрил: " + view.MakeReady + " Сделано: " + view.DoneMakeReady + "\n---------------------------------");

                                //Сделать подсчёт оставшегося времени с учетом оставшейся части приладки view.NormTimeMakeReady * lastMakeReady
                                //Что делает GetNormTimeForOrder?
                                //Возможно переделать
                                if (normtime.PlanNormtimeWork > 0)
                                {
                                    float norm = view.Amount / normtime.PlanNormtimeWork;

                                    if (norm > 0)
                                    {
                                        lastTimeWork = lastAmount / norm;
                                    }
                                }

                                if (orderPreviousAmount > 0)//заказ уже прилажен и начато выполнение
                                {
                                    normTimeGeneral = (int)lastTimeWork;
                                    normTimeFull = (int)lastTimeWork;
                                }
                                else
                                {
                                    if (isMakeready)
                                    {
                                        normTimeGeneral = (int)normtime.PlanNormtimeWork;
                                    }
                                    else
                                    {
                                        normTimeGeneral = (int)lastTimeMakeReady + (int)normtime.PlanNormtimeWork;
                                    }

                                    normTimeFull = view.NormTimeMakeReady + view.NormTimeWork;
                                }

                                ///////TEST
                                float fullLastTime = (normtime.PlanNormtimeMakeReady / (normtime.PlanOutQtyMakeReady == 0 ? 1 : normtime.PlanOutQtyMakeReady)) * (1 - orderPreviousMakeReady) + lastTimeWork;

                                if (order.Status == 2)
                                {
                                    //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, order.DateEnd);
                                    //dinnerTime += AddDinnerTimeToWorkingOut(order.DateBegin, time.DateTimeAmountMunutes(order.DateBegin, (int)Math.Round(userWorkingOut)));
                                    //dinnerTime += AddDinnerTimeToWorkingOut(order.DateBegin, order.DateEnd);
                                    dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, order.DateEnd);
                                    view.TimePlanedEndOrder = time.DateTimeAmountMunutes(timeStartShift, (int)Math.Round(userWorkingOut) + dinnerTime + idletime);

                                    view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);

                                    /*timeBegin = Convert.ToDateTime(order.DateBegin).ToString("dd.MM.yyyy HH:mm");
                                    timeEnd = Convert.ToDateTime(order.DateEnd).ToString("dd.MM.yyyy HH:mm");*/

                                    view.TimeEnd = timeEnd + "";
                                }

                                if (order.Status != 2)
                                {
                                    //timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeFull);

                                    //if (user.Shifts[0].Orders[indexesUserShiftsOrders[0]].DateBegin == user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd)
                                    if (order.DateBegin == order.DateEnd)
                                    {
                                        if (shifts.CheckShiftIsActive(order.IDFBCBrigade))
                                        {
                                            string factTimeEndOrder = time.DateTimeAmountMunutes(orderStartTime, normTimeGeneral);

                                            if (time.StringToDateTime(factTimeEndOrder) < DateTime.Now)
                                            {
                                                factTimeEndOrder = DateTime.Now.ToString();
                                            }

                                            //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, DateTime.Now.ToString());
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, time.DateTimeAmountMunutes(orderStartTime, normTimeGeneral));
                                            //dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, time.DateTimeAmountMunutes(orderStartTime, normTimeGeneral));
                                            dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, factTimeEndOrder);
                                            view.TimePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeGeneral + dinnerTime + idletime);
                                            view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, DateTime.Now.ToString());
                                            view.Duration = time.DateDifferenceToMinutes(DateTime.Now.ToString(), order.DateBegin);
                                            view.TimeEnd = "выполняется ";

                                            int dinnerForCorrentTimeBegin = AddDinnerTimeToWorkingOut(timeStartShift, DateTime.Now.ToString());
                                            int currentTimeBegin = time.DateDifferenceToMinutes(DateTime.Now.ToString(), lastTimeEndPlanedOrder) - (dinnerForCorrentTimeBegin + idletime);

                                            if (lastTimeMakeReady < currentTimeBegin)
                                            {
                                                float norm = view.Amount / normtime.PlanNormtimeWork;
                                                float workCount = (currentTimeBegin - lastTimeMakeReady) * norm;
                                            }
                                        }
                                        else
                                        {
                                            //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, order.DateEnd);
                                            //dinnerTime += AddDinnerTimeToWorkingOut(order.DateBegin, time.DateTimeAmountMunutes(order.DateBegin, (int)Math.Round(view.WorkingOut)));
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, order.DateEnd);
                                            dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, order.DateEnd);
                                            view.TimePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, (int)Math.Round(view.WorkingOut) + dinnerTime + idletime);
                                            view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);
                                            view.TimeEnd = timeEnd + " ";

                                            /*dinnerTime += AddDinnerTimeToWorkingOut(selectDate, firstTimeBegin, timeEndShift);
                                            timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeFull + dinnerTime);
                                            differentTime = time.DateDifferenceToMinutes(timePlanedEndOrder, timeEndShift);
                                            duration = time.DateDifferenceToMinutes(timeEndShift, firstTimeBegin);
                                            timeEnd = timeEndShift; */
                                        }
                                    }
                                    else
                                    {
                                        if (shifts.CheckShiftIsActive(order.IDFBCBrigade))
                                        {
                                            int normTimeCurrent;

                                            //
                                            //
                                            // поправить это условие, для незавершенного заказа и незавершенной смены показывает планируемое время завершения заказа общее
                                            if (order.IdletimeName == "")
                                            {
                                                if (IsLastRecordOfOrder(userShiftOrders.GetRange(l, userShiftOrders.Count - l)))
                                                {
                                                    normTimeCurrent = (int)Math.Round(view.WorkingOut);
                                                }
                                                else
                                                {
                                                    normTimeCurrent = normTimeGeneral;
                                                }
                                            }
                                            else
                                            {
                                                normTimeCurrent = (int)Math.Round(view.WorkingOut);
                                            }

                                            //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, DateTime.Now.ToString());
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, time.DateTimeAmountMunutes(orderStartTime, normTimeGeneral));
                                            dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, time.DateTimeAmountMunutes(orderStartTime, normTimeCurrent));
                                            view.TimePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeCurrent + dinnerTime + idletime);
                                            view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);
                                            //view.Duration = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateBegin);
                                            view.TimeEnd = timeEnd + " ";
                                        }
                                        else
                                        {
                                            //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, order.DateEnd);
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, time.DateTimeAmountMunutes(orderStartTime, (int)Math.Round(view.WorkingOut)));
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, orderEndTime);
                                            dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, orderEndTime);
                                            view.TimePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, (int)Math.Round(view.WorkingOut) + dinnerTime + idletime);
                                            view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);
                                            view.TimeEnd = timeEnd + " ";
                                        }
                                    }
                                }

                                //if (order.Flags == 576)
                                if (order.OperationType == 0)
                                {
                                    isMakeready = true;
                                }

                                indexRow = dataGridViewOneShift.Rows.Add();

                                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                                if (l == 0)
                                {
                                    if (order.IdletimeName == "")
                                    {
                                        countOrder++;

                                        dataGridViewOneShift.Rows[indexRow].Cells[0].Value = (countOrder).ToString();
                                    }

                                    dataGridViewOneShift.Rows[indexRow].Cells[1].Value = "    " + machines[order.IdEquip];
                                    dataGridViewOneShift.Rows[indexRow].Cells[2].Value = order.OrderNumber;
                                    dataGridViewOneShift.Rows[indexRow].Cells[3].Value = order.OrderName;
                                }

                                dataGridViewOneShift.Rows[indexRow].Cells[4].Value = view.IdletimeName;

                                //if (order.Flags == 576 || order.IdletimeName != "")
                                //Подумать(
                                if (order.OperationType == -1 && order.IdletimeName != "")
                                {
                                    //dataGridViewOneShift.Rows[indexRow].Cells[5].Value = order.PlanOutQty.ToString("N0") + " | " + order.FactOutQty.ToString("N0");
                                    if (order.Normtime > 0)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(order.Normtime);
                                    }
                                }
                                else if (order.OperationType == 0)
                                {
                                    if (lastMakeReady != order.PlanOutQty)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[5].Value = lastMakeReady.ToString("P0") + " / " + order.PlanOutQty.ToString("P0");
                                    }

                                    dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString((int)lastTimeMakeReady);
                                }
                                else if (order.OperationType == 1)
                                {
                                    if (lastAmount != view.Amount)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[5].Value = lastAmount.ToString("N0") + " / " + view.Amount.ToString("N0");
                                    }
                                    else
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[5].Value = view.Amount.ToString("N0");
                                    }

                                    if (order.DateBegin == order.DateEnd)
                                    {
                                        if (shifts.CheckShiftIsActive(order.IDFBCBrigade))
                                        {
                                            dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(normTimeGeneral);
                                        }
                                        else
                                        {
                                            dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(normTimeFull);
                                        }
                                    }
                                    else
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(normTimeGeneral);
                                    }
                                }

                                if (order.IdletimeName == "")
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[10].Value = view.TimePlanedEndOrder;

                                    if (l == userShiftOrders.Count - 1)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[11].Value = time.MinuteToTimeString(view.DifferentTime);
                                    }
                                }
                                else
                                {
                                    if ((currentStep == userShift.Orders.Count - 1) && !shifts.CheckShiftIsActive(order.IDFBCBrigade))
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[11].Value = time.MinuteToTimeString(view.DifferentTime);
                                    }

                                    if (order.IdletimeName != "" && order.FactOutQty > 0)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[10].Value = view.TimePlanedEndOrder;
                                        dataGridViewOneShift.Rows[indexRow].Cells[11].Value = time.MinuteToTimeString(view.DifferentTime);
                                    }
                                }

                                if (view.DoneMakeReady > 0)
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[12].Value = view.DoneMakeReady.ToString("P0");
                                }

                                if (view.Done > 0)
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[12].Value = view.Done.ToString("N0");
                                }

                                //тут еще ничего не работает
                                //dataGridViewOneShift.Rows[indexRow].Cells[4].Value = order.PlanOutQty.ToString("N0") + " | " + order.FactOutQty.ToString("N0");

                                dataGridViewOneShift.Rows[indexRow].Cells[7].Value = view.TimeBegin;
                                //dataGridViewOneShift.Rows[indexRow].Cells[8].Value = user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd;
                                dataGridViewOneShift.Rows[indexRow].Cells[8].Value = view.TimeEnd;
                                dataGridViewOneShift.Rows[indexRow].Cells[9].Value = time.MinuteToTimeString(view.Duration);

                                //dataGridViewOneShift.Rows[indexRow].Cells[11].Value = time.MinuteToTimeString(view.DifferentTime);
                                //dataGridViewOneShift.Rows[indexRow].Cells[12].Value = view.Done.ToString("N0");
                                dataGridViewOneShift.Rows[indexRow].Cells[13].Value = time.MinuteToTimeString((int)Math.Round(view.WorkingOut));

                                if (view.DifferentTime >= 0)
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[11].Style.ForeColor = Color.Green;
                                }
                                else
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[11].Style.ForeColor = Color.Red;
                                }

                                Color color = Color.White;

                                if (countOperation % 2 == 0)
                                {
                                    color = Color.Gainsboro;
                                }

                                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.BackColor = color;
                            }

                            currentStep += userShiftOrders.Count;
                        }
                    }
                }

                indexRow = dataGridViewOneShift.Rows.Add();

                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.Font = new Font(dataGridViewOneShift.Font, FontStyle.Bold);
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.BackColor = Color.Silver;
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                dataGridViewOneShift.Rows[indexRow].Cells[12].Value = userDone.ToString("N0");
                dataGridViewOneShift.Rows[indexRow].Cells[13].Value = time.MinuteToTimeString((int)Math.Round(userWorkingOut)) + " (" + (userWorkingOut / normTime).ToString("P1") + ")";
            }
        }
    }
}
