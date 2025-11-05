using libData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace libSql
{
    public class ValueOrderSearch
    {
        public List<OrderHeadSearch> SearchOrderHeadIndexes(string searchNumber)
        {
            List<OrderHeadSearch> orderHeads = new List<OrderHeadSearch>();

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,
                    //CommandText = @"SELECT * FROM dbo.order_head WHERE status = '1' AND order_num LIKE '@order_num'"
                    CommandText = @"SELECT 
                                        order_head.id_order_head, 
	                                    order_head.order_num, 
	                                    common_ul_directory.ul_name
                                    FROM 
                                        dbo.order_head
	                                INNER JOIN
	                                dbo.common_ul_directory
	                                ON 
		                                order_head.id_customer = common_ul_directory.id_common_ul_directory
                                    WHERE 
                                        (status = '1' AND order_num LIKE @order_num)"
                };//(status = '1' AND order_num LIKE '%" + searchNumber + "%')"
                Command.Parameters.AddWithValue("@order_num", "%" + searchNumber + "%");

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    orderHeads.Add(new OrderHeadSearch(
                        Convert.ToInt32(sqlReader["id_order_head"]),
                        sqlReader["order_num"].ToString(),
                        sqlReader["ul_name"].ToString()
                        ));
                }

                connection.Close();
            }

            return orderHeads;
        }

        public List<OrderSearchValue> OrdersListFromIDHead(int idOrderHead)
        {
            List<OrderSearchValue> orderSearchValues = new List<OrderSearchValue>();

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,
                    //CommandText = @"SELECT * FROM dbo.order_head WHERE status = '1' AND order_num LIKE '@order_num'"
                    CommandText = @"SELECT
	                                    man_factjob.id_equip, 
	                                    man_factjob.id_common_employee, 
	                                    man_factjob.flags, 
	                                    man_planjob_list.plan_out_qty, 
	                                    man_factjob.date_begin, 
	                                    man_factjob.date_end, 
	                                    man_factjob.duration, 
	                                    man_factjob.fact_out_qty, 
	                                    man_planjob_list.id_man_order_job_item, 
	                                    idletime_directory.idletime_name,
	                                    norm_operation_table.ord 'operationType'
                                    FROM
	                                    dbo.man_order_job
	                                    INNER JOIN
	                                    dbo.man_order_job_item
	                                    ON 
		                                    man_order_job.id_man_order_job = man_order_job_item.id_man_order_job
	                                    INNER JOIN
	                                    dbo.man_planjob_list
	                                    ON 
		                                    man_order_job_item.id_man_order_job_item = man_planjob_list.id_man_order_job_item
	                                    INNER JOIN
	                                    dbo.man_factjob
	                                    ON 
		                                    man_planjob_list.id_man_planjob_list = man_factjob.id_man_planjob_list
	                                    LEFT JOIN
	                                    dbo.man_idletime
	                                    ON 
		                                    man_order_job.id_man_order_job = man_idletime.id_man_order_job
	                                    LEFT JOIN
	                                    dbo.idletime_directory
	                                    ON 
		                                    man_idletime.id_idletime = idletime_directory.id_idletime_directory
	                                    LEFT JOIN
	                                    dbo.norm_operation_table
	                                    ON 
		                                    man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
                                    WHERE
	                                    man_order_job.id_order_head = @id_order_head AND
	                                    man_factjob.id_equip IS NOT NULL AND
	                                    man_factjob.id_common_employee IS NOT NULL
                                    ORDER BY
	                                    man_factjob.date_begin"
                };//(status = '1' AND order_num LIKE '%" + searchNumber + "%')"
                Command.Parameters.AddWithValue("@id_order_head", idOrderHead);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    int factOut = sqlReader["fact_out_qty"] == DBNull.Value ? 0 : Convert.ToInt32(sqlReader["fact_out_qty"]);
                    int planOut = sqlReader["plan_out_qty"] == DBNull.Value ? 0 : Convert.ToInt32(sqlReader["plan_out_qty"]);
                    string idletimeName = sqlReader["idletime_name"] == DBNull.Value ? string.Empty : sqlReader["idletime_name"].ToString();
                    int operationType = sqlReader["operationType"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["operationType"]);

                    orderSearchValues.Add(new OrderSearchValue(
                        Convert.ToInt32(sqlReader["id_equip"]),
                        Convert.ToInt32(sqlReader["id_common_employee"]),
                        Convert.ToInt32(sqlReader["flags"]),
                        planOut,
                        sqlReader["date_begin"].ToString(),
                        sqlReader["date_end"].ToString(),
                        Convert.ToInt32(sqlReader["duration"]),
                        factOut,
                        Convert.ToInt32(sqlReader["id_man_order_job_item"]),
                        idletimeName,
                        operationType
                        ));
                }

                connection.Close();
            }

            return orderSearchValues;
        }
    }
}
